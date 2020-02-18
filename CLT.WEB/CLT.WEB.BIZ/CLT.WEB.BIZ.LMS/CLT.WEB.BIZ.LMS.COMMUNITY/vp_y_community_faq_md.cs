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
using System.Web.UI.WebControls;
using System.Globalization;

namespace CLT.WEB.BIZ.LMS.COMMUNITY
{
    /// <summary>
    /// 1. 작업개요 : 
    /// 
    /// 2. 주요기능 : 
    ///				  
    /// 3. Class 명 : vp_y_community_faq_md
    /// 
    /// 4. 작 업 자 : 
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_y_community_faq_md
    ///        * Comment 
    ///          * Comment 
    ///          Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제따른 변경    ///          영문화 작업

    /// </summary>
    class vp_y_community_faq_md : DAC
    {
        /************************************************************
        * Function name : GetFAQMaster
        * Purpose       : 공지사항 List 조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        #region public DataTable GetFAQMaster(string[] rParams, CultureInfo rArgCultureInfo)
        public DataTable GetFAQMaster(string[] rParams, CultureInfo rArgCultureInfo)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT * FROM ( ";
                xSql += "  SELECT rownum  rnum, b.* FROM (  ";
                xSql += "    SELECT boa.boa_no, SUBSTR(boa.boa_sub,0,30) || '...' boa_sub ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", tuser.user_nm_kor ";
                }
                else
                {
                    xSql += ", tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last as user_nm_kor ";
                }
                
                xSql += ", TO_CHAR(boa.ins_dt,'YYYY.MM.DD') insdt, boa.hit_cnt, boa.del_yn, ";
                xSql += "        count(*) over() totalrecordcount ";
                xSql += "      FROM t_board boa, t_user tuser ";
                xSql += "     WHERE boa.ins_id = tuser.user_id ";
                xSql += "       AND boa_kind = '000002' ";  // FAQ 만 검색

                if (!string.IsNullOrEmpty(rParams[5])) // 검색내용이 있으면
                {
                    if (rParams[4] == "000001")    // 000001 : 모두, 000002 : 제목, 000003 내용              
                        xSql += string.Format(" AND ( boa.boa_sub LIKE '%{0}%' OR boa.boa_content LIKE '%{1}%') ", rParams[5], rParams[5]);

                    else if (rParams[4] == "000002")    // 000001 : 모두, 000002 : 제목, 000003 내용
                        xSql += string.Format(" AND boa.boa_sub LIKE '%{0}%' ", rParams[5]); // 제목 검색
                    else if (rParams[4] == "000003")    // 000001 : 모두, 000002 : 제목, 000003 내용
                        xSql += string.Format(" AND boa.boa_content LIKE '%{0}%' ", rParams[5]);
                }

                if (!string.IsNullOrEmpty(rParams[2]))
                    xSql += string.Format("   AND boa.ins_dt >= TO_DATE('{0}','YYYY.MM.DD.HH24MISS') ", rParams[2] + "000000");

                if (!string.IsNullOrEmpty(rParams[3]))
                    xSql += string.Format("   AND boa.ins_dt <= TO_DATE('{0}','YYYY.MM.DD.HH24MISS') ", rParams[3] + "235959");

                if (rParams[6] != "000001")  // 관리자가 아닐경우
                {
                    xSql += " AND del_yn = 'N' ";
                }
                xSql += " ORDER BY  boa.ins_dt DESC ";
                xSql += " ) b ";
                xSql += " ) ";

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
        #endregion

        /************************************************************
        * Function name : GetMaxIDOfCode
        * Purpose       : FAQ ID 생성
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        #region public string GetMaxIDOfCode(string[] rParams, OracleCommand rCmd)
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
        #endregion

        /************************************************************
        * Function name : GetMaxIDOfCode
        * Purpose       : FAQ ID 생성
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
        * Function name : SetFAQ
        * Purpose       : FAQ 저장 (Insert)
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        #region public string SetFAQ(string[] rParams)
        public string SetFAQ(string[] rParams)
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


                //string xSeq = GetMaxIDOfCode(new string[] { "boa_no", "t_board" }, xCmdLMS);
                
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
                    xSql += string.Format(" {0}, ", rParams[3]);
                    xSql += string.Format(" '{0}', ", rParams[0]);
                    //xSql += string.Format(" '{0}', ", rParams[1]);
                    xSql += " :BOACONTENT, ";
                    xSql += " 0, ";
                    xSql += " 'N', ";
                    xSql += " '000002', "; // 000001 Q&A, 000002 FAQ, 000003 자료실
                    xSql += string.Format(" '{0}', ", rParams[2]); // 수정자 ID
                    xSql += " SYSDATE, ";  // 수정일시
                    xSql += string.Format(" '{0}', ", rParams[2]); // 생성자 ID
                    xSql += " SYSDATE) ";  // 생성일시                    


                    xCmdLMS.CommandText = xSql;

                    OracleParameter BOACONTENT = new OracleParameter();
                    BOACONTENT.OracleType = OracleType.Clob;
                    BOACONTENT.ParameterName = "BOACONTENT";
                    BOACONTENT.Value = rParams[1];
                    xCmdLMS.Parameters.Add(BOACONTENT);

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
        * Function name : SetFAQUpdate
        * Purpose       : FAQ 저장(기존 FAQ 저장 Update)
        * Input         : string[] rParams
        * Output        : String
        *************************************************************/
        #region public string SetFAQUpdate(params object[] rParams)
        public string SetFAQUpdate(params object[] rParams)
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

                try
                {
                    string xSql = string.Empty;

                    // 기존 공지사항 Update
                    xSql = string.Empty;
                    xSql += " UPDATE t_board SET ";
                    xSql += string.Format(" boa_kind = '{0}', ", "000002");  //  000001 Q&A, 000002 FAQ, 000003 자료실
                    xSql += string.Format(" boa_sub = '{0}', ", rParams[0]); // FAQ 제목
                    //xSql += string.Format(" boa_content = '{0}', ", rParams[1]); // FAQ 내용
                    xSql += "boa_content = :BOACONTENT, ";
                    xSql += string.Format(" upt_id = '{0}', ", rParams[2]); // 수정자 ID
                    xSql += " upt_dt = SYSDATE ";
                    xSql += string.Format(" WHERE boa_no = {0} ", Convert.ToInt32(rParams[3]));

                    xCmdLMS.CommandText = xSql;

                    OracleParameter BOACONTENT = new OracleParameter();
                    BOACONTENT.OracleType = OracleType.Clob;
                    BOACONTENT.ParameterName = "BOACONTENT";
                    BOACONTENT.Value = rParams[1];
                    xCmdLMS.Parameters.Add(BOACONTENT);

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
        * Function name : GetFAQDetail
        * Purpose       : Q&A List 조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        #region public DataTable GetFAQDetail(string rParams)
        public DataTable GetFAQDetail(string rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT boa.boa_no, boa.boa_sub, boa.boa_content, ";
                xSql += "        tuser.user_nm_kor user_nm_kor, ";
                xSql += "        tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last user_nm_eng, ";
                xSql += "        TO_CHAR(boa.ins_dt, 'YYYY.MM.DD') ins_dt, boa.hit_cnt, boa.del_yn ";
                xSql += "   FROM t_board boa, t_user tuser ";
                xSql += "  WHERE boa.boa_kind = '000002' ";
                xSql += string.Format(" AND boa.boa_no = {0} ", Convert.ToInt32(rParams));
                xSql += "    AND boa.ins_id = tuser.user_id ";

                xDt = base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }
        #endregion

        /************************************************************
        * Function name : SetFAQDelete
        * Purpose       : 공지사항 삭제(기존 공지사항 저장 Update 삭제유무 Y 로 변경)
        * Input         : string rParams(not_no)
        * Output        : String
        *************************************************************/
        #region public string SetFAQDelete(string rParams)
        public string SetFAQDelete(string rParams)
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
                    xSql += string.Format(" del_yn = '{0}' ", "Y");
                    xSql += string.Format(" WHERE boa_no = {0} ", Convert.ToInt32(rParams));

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
        * Function name : SetFAQHitCnt
        * Purpose       : 질문&답변 읽은수 Update
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        #region SetFAQHitCnt(string rParams)
        public string SetFAQHitCnt(string rParams)
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
        #endregion
    }
}
