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
    /// 3. Class 명 : vp_y_community_notice_md
    /// 
    /// 4. 작 업 자 : 
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_y_community_notice_md
    ///        * Comment 
    ///          * Comment 
    ///          Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제따른 변경

    ///          영문화 작업

    /// </summary>
    class vp_y_community_notice_md : DAC
    {
        /************************************************************
        * Function name : GetNoticeMaster
        * Purpose       : 공지사항 List 조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public DataTable GetNoticeMaster(string[] rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT * FROM ( ";
                xSql += " SELECT rownum  rnum, b.* FROM ( ";
                xSql += " SELECT notice.not_no not_no, notice.not_sub not_sub ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", tuser.user_nm_kor ";
                }
                else
                {
                    xSql += ", tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last as user_nm_kor ";
                }
                //xSql += "        tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last user_nm_eng, ";
                xSql += "        , TO_CHAR(notice.ins_dt,'YYYY.MM.DD') ins_dt, ";
                xSql += "        DECODE(notice.send_flg, '3', 'N', '2', 'Y') sent_flg, ";
                xSql += "        notice.hit_cnt hit_cnt, notice.del_yn del_yn, ";
                xSql += "        (SELECT count(*) FROM t_notice_att WHERE notice.not_no = not_no) attcount, ";
                xSql += "        count(*) over() totalrecordcount ";
                xSql += "   FROM t_notice notice, ";
                //xSql += "        t_notice_att att, ";
                xSql += "        t_user tuser ";
                xSql += "  WHERE notice.ins_id = tuser.user_id ";
                if (!string.IsNullOrEmpty(rParams[2]))
                    xSql += string.Format("   AND notice.ins_dt >= TO_DATE('{0}','YYYY.MM.DD.HH24MISS') ", rParams[2] + "000000");
                if (!string.IsNullOrEmpty(rParams[3]))
                    xSql += string.Format("   AND notice.ins_dt <= TO_DATE('{0}','YYYY.MM.DD.HH24MISS') ", rParams[3] + "235959");

                //xSql += " AND notice.not_no = att.not_no(+) ";
                if (!string.IsNullOrEmpty(rParams[5])) // 검색내용이 있으면

                {
                    if (rParams[4] == "000001")    // 000001 : 모두, 000002 : 제목, 000003 내용
                        xSql += string.Format(" AND (notice.not_sub LIKE '%{0}%' OR notice.not_content LIKE '%{0}%') ", rParams[5]);
                        //xSql += string.Format(" AND (notice.not_sub LIKE '%{0}%') OR  ", rParams[5]);
                    else if (rParams[4] == "000002")    // 000001 : 모두, 000002 : 제목, 000003 내용
                        xSql += string.Format(" AND notice.not_sub LIKE '%{0}%' ", rParams[5]);
                    else if (rParams[4] == "000003")    // 000001 : 모두, 000002 : 제목, 000003 내용
                        xSql += string.Format(" AND notice.not_content LIKE '%{0}%' ", rParams[5]);
                }

                if (!string.IsNullOrEmpty(rParams[6]))
                    xSql += string.Format(" AND notice.open_course_id = '{0}' ", rParams[6]);

                if (rParams[7] != "000001")  // 업체구분 m_cd = 0061  회사구분 (000000 : 전체, 000001 : 그룹사, 000002 : 사업자 위수탁)
                {
                    if (rParams[7] == "000007" || rParams[7] == "000008" || rParams[7] == "000009") // 법인사 관리자, 법인사 수강자, 손님일경우

                        xSql += " AND notice.company_kind IN ('000000','000002') ";
                    else
                        xSql += " AND notice.company_kind IN ('000000','000001') ";

                    xSql += " AND notice.notice_end_dt >= SYSDATE ";
                    xSql += " AND del_yn = 'N' ";
                }

                xSql += string.Format("   AND notice.not_kind = NVL('{0}', '000001') ", rParams[8]);

                if (!string.IsNullOrEmpty(rParams[9]))
                    xSql += string.Format(" AND notice.del_yn = '{0}' ", rParams[9]);

                xSql += " ORDER BY  notice.not_no DESC ";
                xSql += " ) b ";
                xSql += " ) ";
                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));

                return base.ExecuteDataTable("LMS", xSql);

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        /************************************************************
        * Function name : GetNoticeDetail
        * Purpose       : 공지사항 List 상세조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public DataTable GetNoticeDetail(string[] rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT notice.not_sub not_sub "; //
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", tuser.user_nm_kor as user_nm";
                }
                else
                {
                    xSql += ", tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last as user_nm ";
                }

                xSql += ", notice.hit_cnt hit_cnt, ";
                xSql += "        TO_CHAR(notice.ins_dt, 'YYYY.MM.DD') ins_dt, DECODE(notice.send_flg,'1','N', '2','Y') send_flg, ";
                xSql += "        att.file_nm file_nm, notice.not_content content, ";
                xSql += "        TO_CHAR(notice.notice_begin_dt, 'YYYY.MM.DD') notice_begin_dt, ";
                xSql += "        TO_CHAR(notice.notice_end_dt, 'YYYY.MM.DD') notice_end_dt, ";
                xSql += "        notice.open_course_id open_course_id, ";
                xSql += "        course.course_id, course.course_nm, ";
                xSql += "        notice.del_yn ";
                xSql += "   FROM t_notice notice, t_notice_att att, t_user tuser, ";
                xSql += "        t_open_course open_course, t_course course ";
                xSql += string.Format(" WHERE notice.not_no = {0} ", Convert.ToInt32(rParams[0]));
                xSql += "    AND notice.not_no = att.not_no(+) ";
                xSql += "    AND notice.ins_id = tuser.user_id ";
                xSql += "    AND notice.open_course_id = open_course.open_course_id(+)";
                xSql += "    AND open_course.course_id = course.course_id(+) ";

                if (rParams[1] == "000007" || rParams[1] == "000008" || rParams[1] == "000009") // 법인사 관리자, 법인사 수강자, 손님일경우
                {
                    xSql += " AND notice.company_kind IN ('000000','000002') ";
                }
                else
                {
                    xSql += " AND notice.company_kind IN ('000000','000001') ";
                }

                if (rParams[1] != "000001" && rParams[1] != "000002" && rParams[1] != "000003") // 관리자, 행정담당, 교관
                {
                    xSql += "    AND notice.notice_end_dt >= SYSDATE";
                }

                return base.ExecuteDataTable("LMS", xSql);

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        /************************************************************
        * Function name : GetNoticeFile
        * Purpose       : 공지사항 List 첨부파일조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public object GetNoticeFile(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;
                xSql += "SELECT att_file FROM t_notice_att ";
                xSql += string.Format(" WHERE not_no = {0} AND file_NM = '{1}' ", rParams[0], rParams[1]);

                object obj = base.ExecuteScalar("LMS", xSql);
                
                return obj;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /************************************************************
        * Function name : GetNoticeFileName
        * Purpose       : 공지사항 List 첨부파일명 조회(Edit용)
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public DataTable GetNoticeFileName(string rParams)
        {
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT not_no, file_nm FROM t_notice_att  ";
                xSql += string.Format(" WHERE not_no = {0} ", Convert.ToInt32(rParams));

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        /************************************************************
        * Function name : SetNoticeHitCnt
        * Purpose       : 공지사항 읽은수 Update
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public string SetNoticeHitCnt(string rParams)
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
                    oOraParams[1] = base.AddParam("p_in_table", OracleType.VarChar, "T_NOTICE");

                    int j = base.Execute(db, CommandType.StoredProcedure, "pkg_lms_hitcount.lms_hitcount", oOraParams, xTransLMS);

                    //string xSql = string.Empty;
                    //xSql += " UPDATE t_notice SET hit_cnt = hit_cnt + 1 ";
                    //xSql += string.Format(" WHERE not_no = {0} ", Convert.ToInt32(rParams));

                    //xCmdLMS.CommandText = xSql;
                    //base.Execute("LMS", xCmdLMS, xTransLMS);

                    xTransLMS.Commit(); // 트랜잭션 커밋
                    xRtn = Boolean.TrueString;
                }
                catch (Exception ex)
                {
                    // 파일 제거
                    //if (xFilePath.Trim() != "")
                    //    File.Delete(xFilePath);

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

                throw ex;
            }

            return xRtn;
        }

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
                xSql = string.Format(" SELECT NVL(MAX({0}+1),0) id FROM t_notice ", rParams[0]);

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
        * Purpose       : 공지사항 ID 생성
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
        * Function name : SetNotice
        * Purpose       : 공지사항 저장

        * Input         : string[] rParams
        * Output        : String
        *************************************************************/
        public string SetNotice(params object[] rParams)
        {
            string xRtn = Boolean.FalseString;
            string xDebug = string.Empty;

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


                string xFilePath = string.Empty;
                string[] rMasterParams = (string[])rParams[0];
                object[,] rDetailParams = (object[,])rParams[1];

                string xSeq = rMasterParams[7];
                string xSendFLG = rMasterParams[8];
                //string xSeq = GetMaxIDOfCode(new string[] { "not_no" }, xCmdLMS);

                //xDebug = "seq : " + xSeq;

                try
                {
                    string xSql = string.Empty;
                    xSql += " INSERT INTO t_notice ( ";
                    xSql += " not_no, "; // 공지사항 번호
                    xSql += " not_kind, "; // 공지사항 타입 m_cd = '0055' , 000001: 일반공지 , 000002: 과정공지
                    xSql += " open_course_id, "; // 과정 ID
                    xSql += " notice_begin_dt, "; // 공지사항 게시일자
                    xSql += " notice_end_dt, "; // 공지사항 종료일자
                    xSql += " not_sub, "; // 공지사항 제목
                    xSql += " not_content, ";  // 공지사항 내용
                    xSql += " hit_cnt, "; // 읽은수

                    xSql += " del_yn, "; // 삭제유무
                    xSql += " send_flg, "; // 전송유무 1: 전송대기, 2: 전송
                    xSql += " company_kind, ";  // 업체구분 m_cd = 0061  회사구분 (그룹사, 사업자 위수탁)
                    xSql += " upt_id, "; // 수정자 ID
                    xSql += " upt_dt, "; // 수정일시
                    xSql += " ins_id, "; // 생성자 ID
                    xSql += " ins_dt) "; // 생성일시
                    xSql += " VALUES( ";
                    xSql += string.Format(" {0}, ", Convert.ToInt32(xSeq)); // 공지사항 번호
                    xSql += string.Format(" '{0}', ", rMasterParams[0]); // 공지사항 타입 m_cd = '0055' , 000001: 일반공지 , 000002: 과정공지
                    xSql += string.Format(" '{0}', ", rMasterParams[1]); // 과정 ID (일반공지의 경우 Empty Value)
                    xSql += string.Format(" TO_DATE('{0}','YYYY.MM.DD.HH24MISS'), ", rMasterParams[2] + "000000"); // 공지 게시일자
                    xSql += string.Format(" TO_DATE('{0}','YYYY.MM.DD.HH24MISS'), ", rMasterParams[3] + "235959"); // 공지 종료일자
                   
                    xSql += string.Format(" '{0}', ", rMasterParams[4]); // 공지사항 제목
                    xSql += " :NOTCONTENT, ";
                    //xSql += string.Format(" '{0}', ", rMasterParams[5]); // 공지사항 내용
                    xSql += " 0, "; // 읽은수

                    xSql += " 'N', "; // 삭제유무
                    xSql += string.Format(" '{0}', ", xSendFLG); // 전송유무 1: 전송대기, 2: 전송
                    xSql += string.Format(" '{0}', ", rMasterParams[9]);  // 업체구분 m_cd = 0061  회사구분 (그룹사, 사업자 위수탁)
                    xSql += string.Format(" '{0}', ", rMasterParams[6]); // 수정자 ID
                    xSql += " SYSDATE, ";  // 수정일시
                    xSql += string.Format(" '{0}', ", rMasterParams[6]); // 생성자 ID
                    xSql += " SYSDATE) ";  // 생성일시

                    OracleParameter NOTCONTENT = new OracleParameter();
                    NOTCONTENT.OracleType = OracleType.Clob;
                    NOTCONTENT.ParameterName = "NOTCONTENT";
                    NOTCONTENT.Value = rMasterParams[5];
                    xCmdLMS.Parameters.Add(NOTCONTENT);


                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);


                    for (int i = 0; i < rDetailParams.GetLength(0); i++)
                    {

                        byte[] xFileData = (byte[])rDetailParams[i, 0];
                        string[] xTemp = (string[])rDetailParams[i, 1];
                                               
                        xSql = string.Empty;
                        xSql += " INSERT INTO t_notice_att ( ";
                        xSql += " NOT_NO, ";
                        xSql += " FILE_NM, ";
                        xSql += " att_file ";
                        xSql += " ) ";
                        xSql += " VALUES ( ";
                        xSql += string.Format(" {0}, ", Convert.ToInt32(xSeq));
                        xSql += string.Format(" '{0}', ", xTemp[0]);
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

                    if (xSendFLG == "1") // 전송대상이면

                    {
                        OracleParameter[] oOraParams = new OracleParameter[2];
                        oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_NOTICE");
                        oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_BULL");

                        int i = base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);
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

                    //throw new Exception(xDebug + " | " + ex.Message);
                    bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                    if (rethrow) throw;
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
                
                throw ex;
            }

            return xRtn;
        }


        /************************************************************
        * Function name : SetNoticeUpdate
        * Purpose       : 공지사항 저장(기존 공지사항 저장 Update)
        * Input         : string[] rParams
        * Output        : String
        *************************************************************/
        public string SetNoticeUpdate(params object[] rParams)
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
                        xSql += string.Format(" DELETE FROM t_notice_att WHERE not_no = {0} AND file_nm = '{1}' ", Convert.ToInt32(rMasterParams[7]), rDeleteFile[i]);

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);
                    }

                    // 기존 공지사항 Update
                    xSql = string.Empty;
                    xSql += " UPDATE t_notice SET ";
                    xSql += string.Format(" not_kind = '{0}', ", rMasterParams[0]);  // 공지사항 타입 m_cd = '0055' , 000001: 일반공지 , 000002: 과정공지
                    xSql += string.Format(" open_course_id = '{0}', ", rMasterParams[1]); // 과정 ID (일반공지의 경우 Empty Value)
                    xSql += string.Format(" notice_begin_dt = TO_DATE('{0}','YYYY.MM.DD'), ", rMasterParams[2]); // 공지 게시일자
                    xSql += string.Format(" notice_end_dt = TO_DATE('{0}','YYYY.MM.DD'), ", rMasterParams[3]); // 공지 종료일자
                    xSql += string.Format(" not_sub = '{0}', ", rMasterParams[4]); // 공지사항 제목
                    //xSql += string.Format(" not_content = '{0}', ", rMasterParams[5]); // 공지사항 내용
                    xSql += "not_content = :NOTCONTENT, ";
                    xSql += string.Format(" upt_id = '{0}', ", rMasterParams[6]); // 수정자 ID
                    xSql += string.Format(" send_flg = '{0}', ", rMasterParams[8]); // Sednf FLG 1 : 전송대상, 2 : 전송완료, 3 : 전송안함
                    xSql += " upt_dt = SYSDATE ";
                    xSql += string.Format(" WHERE not_no = {0} ", Convert.ToInt32(rMasterParams[7]));

                    OracleParameter NOTCONTENT = new OracleParameter();
                    NOTCONTENT.OracleType = OracleType.Clob;
                    NOTCONTENT.ParameterName = "NOTCONTENT";
                    NOTCONTENT.Value = rMasterParams[5];
                    xCmdLMS.Parameters.Add(NOTCONTENT);

                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);


                    for (int i = 0; i < rDetailParams.GetLength(0); i++)
                    {

                        byte[] xFileData = (byte[])rDetailParams[i, 0];

                        xSql = string.Empty;
                        xSql += " INSERT INTO t_notice_att ( ";
                        xSql += " NOT_NO, ";
                        xSql += " FILE_NM, ";
                        xSql += " att_file ";
                        xSql += " ) ";
                        xSql += " VALUES ( ";
                        xSql += string.Format(" {0}, ", Convert.ToInt32(rMasterParams[7]));
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


                    if (rMasterParams[8] == "1") // 전송대상이면

                    {
                        OracleParameter[] oOraParams = new OracleParameter[2];
                        oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_NOTICE");
                        oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_BULL");

                        int i = base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);
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
                
                throw ex;
            }

            return xRtn;
        }


        /************************************************************
        * Function name : SetNoticeDelete
        * Purpose       : 공지사항 삭제(기존 공지사항 저장 Update 삭제유무 Y 로 변경)
        * Input         : string rParams(not_no)
        * Output        : String
        *************************************************************/
        public string SetNoticeDelete(params object[] rParams)
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
                    xSql += " UPDATE t_notice SET ";
                    xSql += string.Format(" del_yn = '{0}' ", rParams[1]);
                    xSql += string.Format(" WHERE not_no = {0} ", Convert.ToInt32(rParams[0]));

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
                
                throw ex;
            }

            return xRtn;
        }

    }
}
