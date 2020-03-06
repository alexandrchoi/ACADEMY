using System;
using System.Collections.Generic;
using System.Text;

// 필수 using 문
using System.Data;
using System.Data.OracleClient;
using CLT.WEB.BIZ.FX.BIZBASE;
using System.Web;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Globalization;

namespace CLT.WEB.BIZ.LMS.COMMUNITY
{
    /// <summary>
    /// 1. 작업개요 : 
    /// 
    /// 2. 주요기능 : 
    ///				  
    /// 3. Class 명 : vp_y_community_qna_md
    /// 
    /// 4. 작 업 자 : 
    /// 
    /// 5. Revision History : 

    /// </summary>
    class vp_y_community_qna_md : DAC
    {
        /************************************************************
        * Function name : GetQnAMaster
        * Purpose       : 공지사항 List 조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public DataTable GetQnAMaster(string[] rParams, CultureInfo rArgCultureInfo)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT * FROM ( ";
                xSql += "     SELECT rownum rnum, b.* FROM ( ";
                xSql += "         SELECT boa.boa_no " ;
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", tuser.user_nm_kor ";
                }
                else
                {
                    xSql += ", tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last as user_nm_kor ";
                }
               
                xSql += "                ,boa.ins_dt, boa.boa_sub, (SELECT count(*) FROM t_board_replace WHERE boa.boa_no = boa_no) boa_count, ";
                xSql += "                tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last user_nm_eng, ";
                xSql += "                boa.hit_cnt, boa.del_yn, count(*) over() totalrecordcount, ";
                xSql += "                (SELECT count(*) FROM t_board_att WHERE boa.boa_no = boa_no) attcount ";
                xSql += "           FROM t_board boa, t_user tuser, t_board_att att ";
                xSql += "          WHERE boa.ins_id = tuser.user_id ";
                xSql += "            AND boa.boa_no = att.boa_no (+)";
                xSql += "            AND boa.boa_kind = '000001' "; // 000001 : Q&A, 000002 : FAQ, 000003 : 자료실

                if (!string.IsNullOrEmpty(rParams[2])) // 검색내용이 있으면 
                    xSql += string.Format("   AND boa.ins_dt >= TO_DATE('{0}','YYYY.MM.DD.HH24MISS') ", rParams[2] + "000000");
            
                if (!string.IsNullOrEmpty(rParams[3])) // 검색내용이 있으면
                    xSql += string.Format("   AND boa.ins_dt <= TO_DATE('{0}','YYYY.MM.DD.HH24MISS') ", rParams[3] + "235959");
                if (!string.IsNullOrEmpty(rParams[5])) // 검색내용이 있으면
                {
                    if (rParams[4] == "000001")    // 000001 : 모두, 000002 : 제목, 000003 내용              
                        xSql += string.Format(" AND ( boa.boa_sub LIKE '%{0}%' OR boa.boa_content LIKE '%{1}%') ", rParams[5], rParams[5]);
                    
                    else if (rParams[4] == "000002")    // 000001 : 모두, 000002 : 제목, 000003 내용
                        xSql += string.Format(" AND boa.boa_sub LIKE '%{0}%' ", rParams[5]);
                   
                    else if (rParams[4] == "000003")    // 000001 : 모두, 000002 : 제목, 000003 내용
                        xSql += string.Format(" AND boa.boa_content LIKE '%{0}%' ", rParams[5]);
                }

                if (rParams[6] != "000001")  // 관리자가 아닐경우 2013.10.22 
                {
                    xSql += " AND boa.del_yn = 'N' ";
                }

                xSql += "            ORDER BY boa.boa_no DESC ) b ) ";
                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));                    

                xDt = base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }


        /************************************************************
        * Function name : GetQnADetail
        * Purpose       : Q&A List 조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public DataTable GetQnADetail(string[] rParams, CultureInfo rArgCultureInfo)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT boa.boa_no, boa.boa_sub, boa.boa_content ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", tuser.user_nm_kor ";
                }
                else
                {
                    xSql += ", tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last as user_nm_kor ";
                }

                //xSql += "        tuser.user_nm_kor user_nm_kor, ";
                //xSql += "        tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last user_nm_eng, ";
                xSql += "   ,  TO_CHAR(boa.ins_dt, 'YYYY.MM.DD') ins_dt, boa.hit_cnt, att.file_nm, boa.ins_id, boa.del_yn ";
                xSql += "   FROM t_board boa, t_board_att att, t_user tuser ";
                xSql += "  WHERE boa.ins_id = tuser.user_id ";
                xSql += "    AND boa.boa_no = att.boa_no(+) ";
                xSql += "    AND boa.boa_kind = '000001' ";
                xSql += string.Format(" AND boa.boa_no = {0} ", Convert.ToInt32(rParams[0]));

                if (rParams[1] != "000001")  // 관리자가 아닌경우 처리
                {
                    xSql += " AND boa.del_yn = 'N' ";
                }

                xDt = base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }


        /************************************************************
        * Function name : GetQnAFileName
        * Purpose       : Q&A List 첨부파일명 조회(Edit용)
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public DataTable GetQnAFileName(string rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT boa_no, file_nm FROM t_board_att  ";
                xSql += string.Format(" WHERE boa_no = {0} ", Convert.ToInt32(rParams));

                xDt = base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }
        
        /************************************************************
        * Function name : GetQnAReplace
        * Purpose       : Q&A List 조회
        * Input         : string[] rParamsFIFA12 DB edit
        * Output        : string
        *************************************************************/
        public DataTable GetQnAReplace(string rParams, CultureInfo rArgCultureInfo)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT re.rep_no, re.boa_no, re.rep_content, re.ins_dt ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", tuser.user_nm_kor ";
                }
                else
                {
                    xSql += ", tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last as user_nm_kor ";
                }
                //xSql += "        tuser.user_nm_kor user_nm_kor, ";
               // xSql += "        tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last user_nm_eng ";
                xSql += "   FROM t_board_replace re, t_user tuser ";
                xSql += "  WHERE re.ins_id = tuser.user_id ";
                xSql += string.Format(" AND re.boa_no = {0} ", Convert.ToInt32(rParams));

                xDt = base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }


        /************************************************************
        * Function name : GetQnAFile
        * Purpose       : Q&A List 첨부파일조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public object GetQnAFile(string[] rParams)
        {
            object xobj = null;
            try
            {
                string xSql = string.Empty;
                xSql += "SELECT att_file FROM t_board_att ";
                xSql += string.Format(" WHERE boa_no = {0} AND file_NM = '{1}' ", rParams[0], rParams[1]);

                xobj = base.ExecuteScalar("LMS", xSql);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xobj;
        }


        /************************************************************
        * Function name : GetReplace
        * Purpose       : Q&A 댓글 조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public DataTable GetReplace(string rParams, CultureInfo rArgCultureInfo)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT re.rep_no, re.boa_no, re.rep_content, TO_CHAR(re.ins_dt, 'YYYY.MM.DD') ins_dt ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", tuser.user_nm_kor ";
                }
                else
                {
                    xSql += ", tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last as user_nm_kor ";
                }
                //xSql += "        tuser.user_nm_kor user_nm_kor, ";
                //xSql += "        tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last user_nm_eng ";
                xSql += "   FROM t_board_replace re, t_user tuser ";
                xSql += string.Format(" WHERE re.boa_no = {0} ", Convert.ToInt32(rParams));
                xSql += "    AND re.ins_id = tuser.user_id ";

                xDt = base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {   
                throw ex;
            }

            return xDt;
        }


        /************************************************************
        * Function name : SetQ&ADelete
        * Purpose       : 자료실 삭제(기존 자료실 저장 Update 삭제유무 Y 로 변경)
        * Input         : string rParams(not_no)
        * Output        : String
        *************************************************************/
        #region public string SetQnADelete(params object[] rParams)
        public string SetQnADelete(params object[] rParams)
        {
            string xRtn = Boolean.FalseString;
            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTransLMS = null;
            OracleCommand xCmdLMS = null;
            try
            {
                xTransLMS = xCnnLMS.BeginTransaction();  // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db); 
                xCmdLMS.Connection = xCnnLMS;
                xCmdLMS.Transaction = xTransLMS;

                try
                {
                    string xSql = string.Empty;
                    xSql += " UPDATE t_board SET ";
                    xSql += string.Format(" del_yn = '{0}' ", rParams[1]);
                    xSql += string.Format(" WHERE boa_no = {0} ", Convert.ToInt32(rParams[0]));

                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);

                    xTransLMS.Commit(); // 트랜잭션 커밋
                    xRtn = Boolean.TrueString;

                }
                catch (Exception ex)
                {
                    xTransLMS.Rollback(); // Exception 발생시 롤백처리...

                    throw ex;
                }
                finally
                {
                    if (xCmdLMS != null)
                        xCmdLMS.Dispose();

                    if (xTransLMS != null)
                        xTransLMS.Dispose();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return xRtn;
        }
        #endregion


        /************************************************************
        * Function name : GetMaxIDOfCode
        * Purpose       : MAIL ID 생성
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public string GetMaxIDOfCode(string[] rParams, OracleCommand rCmd)
        {
            string xSql = "";
            object xTemp = null;
            string xRtnID = string.Empty; // 테이블별 리턴 ID를 담는 변수

            try
            {
                xSql = string.Format(" SELECT NVL(MAX({0}+1),0) id FROM {1} ", rParams[0], rParams[1]);

                rCmd.CommandText = xSql;
                xRtnID = Convert.ToString(rCmd.ExecuteScalar());

                if (xRtnID == "0")
                    xRtnID = "1";
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return xRtnID;
        }


        /************************************************************
        * Function name : GetMaxIDOfCode
        * Purpose       : 자료실 ID 생성
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        #region public string GetMaxIDOfCode(string[] rParams)
        public string GetMaxIDOfCode(string[] rParams)
        {
            string xSql = "";
            object xTemp = null;
            string xRtnID = string.Empty; // 테이블별 리턴 ID를 담는 변수



            try
            {
                xSql = string.Format(" SELECT NVL(MAX({0}+1),0) id FROM {1} ", rParams[0], rParams[1]);

                xTemp = base.ExecuteScalar("LMS", xSql);

                xRtnID = xTemp.ToString();
                //rCmd.CommandText = xSql;
                //xRtnID = Convert.ToString(rCmd.ExecuteScalar());


                if (xRtnID == "0")
                    xRtnID = "1";
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return xRtnID;
        }
        #endregion

        /************************************************************
        * Function name : SetQnA
        * Purpose       : 질문&답변 저장 (Insert)
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public string SetQnA(params object[] rParams)
        {
            string xRtn = Boolean.FalseString;

            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTransLMS = null;
            OracleCommand xCmdLMS = null;

            try
            {
                string xFilePath = string.Empty;

                xTransLMS = xCnnLMS.BeginTransaction();  // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db); 
                xCmdLMS.Connection = xCnnLMS;
                xCmdLMS.Transaction = xTransLMS;

                string[] rMasterParams = (string[])rParams[0];
                object[,] rDetailParams = (object[,])rParams[1];

                string xSeq = rMasterParams[3].ToString(); //GetMaxIDOfCode(new string[] { "boa_no", "t_board" }, xCmdLMS);

                try
                {
                    string xSql = string.Empty;
                    xSql += " INSERT INTO t_board ( ";
                    xSql += " boa_no, ";
                    xSql += " boa_sub, ";
                    xSql += " boa_content, ";
                    xSql += " hit_cnt, ";
                    xSql += " del_yn, ";
                    xSql += " boa_kind, ";
                    xSql += " upt_id, ";
                    xSql += " upt_dt, ";
                    xSql += " ins_id, ";
                    xSql += " ins_dt) ";
                    xSql += " VALUES ( ";
                    xSql += string.Format(" {0}, ", xSeq);
                    xSql += string.Format(" '{0}', ", rMasterParams[0]);
                    //xSql += string.Format(" '{0}', ", rMasterParams[1]);
                    xSql += " :BOACONTENT, ";
                    xSql += " 0, ";
                    xSql += " 'N', ";
                    xSql += " '000001', "; // 000001 Q&A, 000002 FAQ, 000003 자료실
                    xSql += string.Format(" '{0}', ", rMasterParams[2]); // 수정자 ID
                    xSql += " SYSDATE, ";  // 수정일시
                    xSql += string.Format(" '{0}', ", rMasterParams[2]); // 생성자 ID
                    xSql += " SYSDATE) ";  // 생성일시                    

                    OracleParameter BOACONTENT = new OracleParameter();
                    BOACONTENT.OracleType = OracleType.Clob;
                    BOACONTENT.ParameterName = "BOACONTENT";
                    BOACONTENT.Value = rMasterParams[1];
                    xCmdLMS.Parameters.Add(BOACONTENT);

                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);


                    for (int i = 0; i < rDetailParams.GetLength(0); i++)
                    {

                        byte[] xFileData = (byte[])rDetailParams[i, 0];

                        xSql = string.Empty;
                        xSql += " INSERT INTO t_board_att ( ";
                        xSql += " boa_no, ";
                        xSql += " file_nm, ";
                        xSql += " att_file ";
                        xSql += " ) ";
                        xSql += " VALUES ( ";
                        xSql += string.Format(" {0}, ", Convert.ToInt32(xSeq));
                        xSql += string.Format(" '{0}', ", rDetailParams[i, 1]);
                        xSql += " :ATTFILE ";
                        xSql += " ) ";

                        OracleParameter ATTFILE = new OracleParameter();
                        ATTFILE.OracleType = OracleType.Blob;
                        ATTFILE.ParameterName = "ATTFILE";
                        ATTFILE.Value = xFileData;
                        xCmdLMS.Parameters.Add(ATTFILE);

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);
                    }   

                    xTransLMS.Commit(); // 트랜잭션 커밋
                    xRtn = Boolean.TrueString;

                }
                catch (Exception ex)
                {
                    // 파일 제거
                    if (xFilePath.Trim() != "")
                        File.Delete(xFilePath);

                    xTransLMS.Rollback(); // Exception 발생시 롤백처리...

                    throw ex;
                }
                finally
                {
                    if (xCmdLMS != null)
                        xCmdLMS.Dispose();

                    if (xTransLMS != null)
                        xTransLMS.Dispose();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return xRtn;
        }


        /************************************************************
        * Function name : SetQnAUpdate
        * Purpose       : Q&A 저장(기존 Q&A 저장 Update)
        * Input         : string[] rParams
        * Output        : String
        *************************************************************/
        public string SetQnAUpdate(params object[] rParams)
        {
            string xRtn = Boolean.FalseString;
            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTransLMS = null;
            OracleCommand xCmdLMS = null;
            try
            {
                string xFilePath = string.Empty;

                xTransLMS = xCnnLMS.BeginTransaction();  // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db); 
                xCmdLMS.Connection = xCnnLMS;
                xCmdLMS.Transaction = xTransLMS;

                string[] rMasterParams = (string[])rParams[0];
                object[,] rDetailParams = (object[,])rParams[1];
                string[] rDeleteFile = (string[])rParams[2];

                // 1. 기존 첨부파일의 삭제
                // 2. 기존공지사항 Update 
                // 3. 첨부파일 Insert

                try
                {
                    string xSql = string.Empty;

                    // 기존 첨부파일 삭제
                    for (int i = 0; i < rDeleteFile.Length; i++)
                    {
                        xSql = string.Empty;
                        xSql += string.Format(" DELETE FROM t_board_att WHERE boa_no = {0} AND file_nm = '{1}' ", Convert.ToInt32(rMasterParams[2]), rDeleteFile[i]);

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);
                    }

                    // 기존 공지사항 Update
                    xSql = string.Empty;
                    xSql += " UPDATE t_board SET ";
                    xSql += string.Format(" boa_kind = '{0}', ", "000001");  // Q&A 타입
                    xSql += string.Format(" boa_sub = '{0}', ", rMasterParams[0]); // 공지사항 제목
                    //xSql += string.Format(" boa_content = '{0}', ", rMasterParams[1]); // 공지사항 내용
                    xSql += "boa_content = :BOACONTENT, ";
                    xSql += string.Format(" upt_id = '{0}', ", rMasterParams[3]); // 수정자 ID
                    xSql += " upt_dt = SYSDATE ";
                    xSql += string.Format(" WHERE boa_no = {0} ", Convert.ToInt32(rMasterParams[2]));

                    xCmdLMS.CommandText = xSql;

                    OracleParameter BOACONTENT = new OracleParameter();
                    BOACONTENT.OracleType = OracleType.Clob;
                    BOACONTENT.ParameterName = "BOACONTENT";
                    BOACONTENT.Value = rMasterParams[1];
                    xCmdLMS.Parameters.Add(BOACONTENT);

                    base.Execute(db, xCmdLMS, xTransLMS);




                    for (int i = 0; i < rDetailParams.GetLength(0); i++)
                    {

                        byte[] xFileData = (byte[])rDetailParams[i, 0];

                        xSql = string.Empty;
                        xSql += " INSERT INTO t_board_att ( ";
                        xSql += " boa_no, ";
                        xSql += " file_nm, ";
                        xSql += " att_file ";
                        xSql += " ) ";
                        xSql += " VALUES ( ";
                        xSql += string.Format(" {0}, ", Convert.ToInt32(rMasterParams[2]));
                        xSql += string.Format(" '{0}', ", rDetailParams[i, 1]);
                        xSql += " :ATTFILE ";
                        xSql += " ) ";


                        OracleParameter ATTFILE = new OracleParameter();
                        ATTFILE.OracleType = OracleType.Blob;
                        ATTFILE.ParameterName = "ATTFILE";
                        ATTFILE.Value = xFileData;
                        xCmdLMS.Parameters.Add(ATTFILE);

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);
                    } 


                    xTransLMS.Commit(); // 트랜잭션 커밋
                    xRtn = Boolean.TrueString;

                }
                catch (Exception ex)
                {
                    // 파일 제거
                    if (xFilePath.Trim() != "")
                        File.Delete(xFilePath);

                    xTransLMS.Rollback(); // Exception 발생시 롤백처리...

                    throw ex;
                }
                finally
                {
                    if (xCmdLMS != null)
                        xCmdLMS.Dispose();

                    if (xTransLMS != null)
                        xTransLMS.Dispose();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return xRtn;
        }


        /************************************************************
        * Function name : SetQnAReplace
        * Purpose       : Q&A 댓글 저장
        * Input         : string[] rParams
        * Output        : String
        *************************************************************/
        public string SetQnAReplace(string[] rParams)
        {
            string xRtn = Boolean.FalseString;
            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTransLMS = null;
            OracleCommand xCmdLMS = null;

            try
            {

                string xFilePath = string.Empty;

                xTransLMS = xCnnLMS.BeginTransaction();  // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db); 
                xCmdLMS.Connection = xCnnLMS;
                xCmdLMS.Transaction = xTransLMS;

                string xSeq = GetMaxIDOfCode(new string[] { "rep_no", "t_board_replace" }, xCmdLMS);

                try
                {
                    string xSql = string.Empty;
                    xSql += " INSERT INTO t_board_replace ( ";
                    xSql += " boa_no, ";
                    xSql += " rep_no, ";
                    xSql += " rep_content, ";
                    xSql += " ins_id, ";
                    xSql += " ins_dt ";
                    xSql += " ) ";
                    xSql += " VALUES (";
                    xSql += string.Format(" {0}, ", Convert.ToInt32(rParams[0]));
                    xSql += string.Format(" {0}, ", Convert.ToInt32(xSeq));
                    xSql += string.Format(" '{0}', ", rParams[1]);
                    xSql += string.Format(" '{0}', ", rParams[2]);
                    xSql += " SYSDATE) ";

                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);

                    xTransLMS.Commit(); // 트랜잭션 커밋
                    xRtn = Boolean.TrueString;


                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xRtn;
        }

        /************************************************************
        * Function name : SetQnAHitCnt
        * Purpose       : 질문&답변 읽은수 Update
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public string SetQnAHitCnt(string rParams)
        {
            string xRtn = Boolean.FalseString;
            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection(); //base.CreateConnection("LMS");
            xCnnLMS.Open();
            OracleTransaction xTransLMS = null;
            OracleCommand xCmdLMS = null;

            try
            {
                xTransLMS = xCnnLMS.BeginTransaction();  // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db);
                xCmdLMS.Connection = xCnnLMS;
                xCmdLMS.Transaction = xTransLMS;


                try
                {
                    OracleParameter[] oOraParams = new OracleParameter[2];
                    oOraParams[0] = base.AddParam("p_not_no", OracleType.Number, Convert.ToInt32(rParams));
                    oOraParams[1] = base.AddParam("p_in_table", OracleType.VarChar, "T_BOARD");

                    int j = base.Execute(db, CommandType.StoredProcedure, "pkg_lms_hitcount.lms_hitcount", oOraParams, xTransLMS);

                    xTransLMS.Commit(); // 트랜잭션 커밋
                    xRtn = Boolean.TrueString;
                }
                catch (Exception ex)
                {
                    xTransLMS.Rollback(); // Exception 발생시 롤백처리...

                    throw ex;
                }
                finally
                {
                    if (xCmdLMS != null)
                        xCmdLMS.Dispose();

                    if (xTransLMS != null)
                        xTransLMS.Dispose();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return xRtn;
        }




    }
}
