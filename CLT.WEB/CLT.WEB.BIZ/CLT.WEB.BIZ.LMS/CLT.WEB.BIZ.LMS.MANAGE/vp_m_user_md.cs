using System;
using System.Collections.Generic;
using System.Text;


// 필수 using 문

using System.Data;
using System.Data.OracleClient;
using CLT.WEB.BIZ.FX.BIZBASE;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.IO;

namespace CLT.WEB.BIZ.LMS.MANAGE
{
    /// <summary>
    /// 1. 작업개요 : vp_m_user_md Class
    /// 
    /// 2. 주요기능 : 회원(법인사) 가입 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_m_user_md
    /// 
    /// 4. 작 업 자 : 김민규 / 2011.12.14
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_m_user_md
    ///        * Comment 
    ///          Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제따른 변경

    /// </summary>
    /// 
    public class vp_m_user_md : DAC
    {
        /************************************************************
       * Function name : GetCompany
       * Purpose       : 사업자 번호로 해당 회사 검색

       * Input         : string[] rParams (0: reg_no)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetCompany(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = "SELECT company_id, company_nm, reg_no FROM t_company " +
                    //" WHERE company_nm = '" + rParams[0] + "'" +
                              "   WHERE  tax_no = '" + rParams[1] + "'";

                xDt = base.ExecuteDataTable("LMS", xSql);

                if (xDt.Rows.Count > 0)
                {

                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }


        /************************************************************
        * Function name : GetCompanySearch
        * Purpose       : 회사명 검색으로 
        * Input         : string[] rParams (0: reg_no)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetCompanySearch(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = "SELECT company_id, company_nm FROM t_company ";
                xSql += " WHERE status = '000003' ";
                if (!string.IsNullOrEmpty(rParams[0]))
                    xSql += string.Format("   AND company_nm Like '%{0}%'", rParams[0]);

                xSql += " ORDER BY company_nm ASC ";
                //if (!string.IsNullOrEmpty(rParams[0]))
                //    xSql += string.Format(" WHERE company_nm Like '%{0}%'", rParams[0]);

                

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
        * Function name : GetCompanyName
        * Purpose       : 회사명 검색으로 
        * Input         : string[] rParams (0: reg_no)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetCompanyName(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = "SELECT company_nm FROM t_company ";
                xSql += string.Format(" WHERE company_id = '{0}' ",rParams[0]);

                xSql += " ORDER BY company_nm ASC ";
                //if (!string.IsNullOrEmpty(rParams[0]))
                //    xSql += string.Format(" WHERE company_nm Like '%{0}%'", rParams[0]);



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
       * Function name : GetCompanyUser
       * Purpose       : 해당 법인사의 관리자 유무 검색

       * Input         : string[] rParams (0: company_id)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetCompanyUser(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = "SELECT * FROM t_user" +
                              " WHERE company_id = '" + rParams[0] + "'";

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
        * Function name : GetUser
        * Purpose       : ID 중복여부 검색

        * Input         : string[] rParams (0: company_id)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetUser(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = "SELECT user_id, user_group, company_id, office_phone FROM t_user" +
                              " WHERE user_id = '" + rParams.GetValue(0) + "'";

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
        * Function name : GetZipcode
        * Purpose       : 우편번호 검색

        * Input         : string[] rParams (0: Dong)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetZipcode(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = "SELECT seq, zipcode, sido || gugun || dong || bunji as address FROM zipcode" +
                              " WHERE dong LIKE '%" + rParams[0] + "%'";

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
        * Function name : GetUserEdit
        * Purpose       : 사용자 정보 검색 및 편집

        * Input         : string[] rParams
        * Output        : DataTable
        *************************************************************/
        public DataTable GetUserEdit(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = "SELECT user_id, user_no, personal_no, user_nm_kor, user_nm_eng_first, user_nm_eng_last, " +
                              "       tuser.duty_step, mobile_phone, email_id, office_phone, tuser.company_id, user_zip_code, " +
                              "       tcompany.company_nm, user_addr, tuser.user_group, sms_yn, mail_yn " +
                              " , tuser.dept_code, nvl((SELECT dept_name FROM v_hdeptcode WHERE dept_code = tuser.dept_code), tuser.dept_code) AS dept_name " +
                              " , trainee_class, to_char(enter_dt, 'yyyy.MM.dd') as enter_dt, to_char(birth_dt, 'yyyy.MM.dd') as birth_dt, pic_file_nm, pic_file " + 
                              "  FROM t_user tuser, t_company tcompany " +
                              " WHERE tuser.user_id = '" + rParams.GetValue(0) + "'" +
                              "   AND tuser.company_id = tcompany.company_id(+) ";
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
        * Function name : GetUserInfo
        * Purpose       : 사용자 정보 조회
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public DataTable GetUserInfo(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                /*
                string xSql = "SELECT * FROM ( ";
                if (rParams[13] == "Y")
                {
                    xSql += "    SELECT rownum rnum, b.* FROM ( ";
                }
                else
                {
                    xSql += "    SELECT b.* FROM ( ";
                }
                xSql += "        SELECT  tuser.user_no, tuser.user_nm_kor, tuser.personal_no, code.d_knm user_group, tcompany.company_nm, tuser.mobile_phone, ";
                xSql += "                code2.d_knm status, ";  // hdutystep.step_name dutystep, datachk.dname socialpos

                // 직급 
                xSql += "                CASE tuser.user_group ";
                xSql += "                WHEN '000007' THEN (SELECT d_knm FROM t_code_detail WHERE m_cd = '0023' AND d_cd = tuser.duty_step) ";
                xSql += "                ELSE (SELECT step_name FROM v_hdutystep WHERE v_hdutystep.duty_step = tuser.duty_step AND use_yn = 'Y') END AS dutystep, ";

                xSql += "                (SELECT dname FROM v_datachkd WHERE hcode = 'HC11' AND dcode = tuser.socialpos ) socialpos, ";
                if (rParams[13] == "Y")
                {
                    xSql += "                tuser.user_id, tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last user_nm_eng , ";
                    xSql += "                tuser.status statusvalue, count(*) over() totalrecordcount, ";
                }
                xSql += "               TO_CHAR(tuser.enter_dt, 'YYYY.MM.DD') enter_dt ";
                xSql += "          FROM t_user tuser, t_company tcompany, t_code_detail code, t_code_detail code2 ";  // v_hdutystep hdutystep, v_datachkd datachk
                xSql += "          WHERE tuser.status IS NOT NULL "; // 사용자 상태가 미사용이 아닐 경우!
                xSql += "            AND code.m_cd = '0041' ";  // 0041 : 사용자 그룹
                xSql += "            AND code2.m_cd = '0044' ";   // 0044 : 상태      
                //xSql += "            AND hdutystep.duty_step = tuser.duty_step ";
                */


                string xSql = string.Empty;

                if (rParams[13] == "Y")
                {
                    // 암호화 모듈제거
                    //xSql = "SELECT RNUM, USER_NO, USER_NM_KOR, SUBSTRB(DAMO.DEC_VARCHAR('LMS','T_USER','PERSONAL_NO',SEC_PERSONAL_NO),1,14) PERSONAL_NO, USER_GROUP, COMPANY_NM, MOBILE_PHONE, STATUS, DUTYSTEP, SOCIALPOS, TRAINEE_CLASS, USER_ID, USER_NM_ENG, STATUSVALUE, TOTALRECORDCOUNT, ENTER_DT FROM ( ";
                    xSql = "SELECT RNUM, USER_NO, USER_NM_KOR, PERSONAL_NO, USER_GROUP, COMPANY_NM, MOBILE_PHONE, STATUS, DUTYSTEP, SOCIALPOS, TRAINEE_CLASS, USER_ID, USER_NM_ENG, STATUSVALUE, TOTALRECORDCOUNT, ENTER_DT, dept_nm,email_id,edu_cnt,ins_dt FROM ( ";
                    xSql += "    SELECT rownum rnum, b.* FROM ( ";
                }
                else
                {
                    //xSql = "SELECT USER_NO, USER_NM_KOR, SUBSTRB(DAMO.DEC_VARCHAR('LMS','T_USER','PERSONAL_NO',SEC_PERSONAL_NO),1,14) PERSONAL_NO, USER_GROUP, COMPANY_NM, MOBILE_PHONE, STATUS,  SOCIALPOS,     DUTYSTEP, ENTER_DT FROM ( ";

                    // 암호화 모듈제거
                    //xSql = "SELECT USER_ID, USER_NM_KOR, SUBSTRB(DAMO.DEC_VARCHAR('LMS','T_USER','PERSONAL_NO',SEC_PERSONAL_NO),1,14) PERSONAL_NO, USER_GROUP, COMPANY_NM, MOBILE_PHONE, STATUS,  TRAINEE_CLASS, DUTYSTEP, ENTER_DT FROM ( ";
                    xSql = "SELECT USER_ID, USER_NM_KOR, PERSONAL_NO, USER_GROUP, COMPANY_NM, MOBILE_PHONE, STATUS, TRAINEE_CLASS, DUTYSTEP, ENTER_DT, dept_nm,email_id,edu_cnt,ins_dt FROM ( ";
                    xSql += "    SELECT b.* FROM ( ";
                }
                // 암호화 모듈제거
                //xSql += "        SELECT  tuser.user_no, tuser.user_nm_kor, tuser.SEC_PERSONAL_NO, code.d_knm user_group, tcompany.company_nm, tuser.mobile_phone, ";
                xSql += "        SELECT  tuser.user_no, tuser.user_nm_kor, ";
                // 복호화
                xSql += " REGEXP_REPLACE(HINDEV.CRYPTO_AES256.DEC_AES(tuser.PERSONAL_NO),'\\d','*', 9) AS PERSONAL_NO, ";

                xSql += "                code.d_knm user_group, tcompany.company_nm, tuser.mobile_phone, ";
                xSql += "                code2.d_knm status, ";  // hdutystep.step_name dutystep, datachk.dname socialpos

                // 직급 
                xSql += "                CASE tuser.user_group ";
                xSql += "                WHEN '000007' THEN (SELECT d_knm FROM t_code_detail WHERE m_cd = '0023' AND d_cd = tuser.duty_step) ";
                xSql += "                ELSE (SELECT step_name FROM v_hdutystep WHERE v_hdutystep.duty_step = tuser.duty_step AND use_yn = 'Y') END AS dutystep, ";

                //2014.03.19 seojw
                //지금까지 훈련생구분이 신분구분으로 잘못나오고 있었음.... 일단 신분구분쿼리는 놔둠....socialpos --> TRAINEE_CLASS
                xSql += "                (SELECT dname FROM v_datachkd WHERE hcode = 'HC11' AND dcode = tuser.socialpos ) socialpos, ";
                xSql += "                (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0062' AND D_CD = TUSER.TRAINEE_CLASS) TRAINEE_CLASS, ";


                xSql += "                tuser.user_id, tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last user_nm_eng , ";

                if (rParams[13] == "Y")
                {

                    xSql += "                tuser.status statusvalue, count(*) over() totalrecordcount, ";
                }
                xSql += "               TO_CHAR(tuser.enter_dt, 'YYYY.MM.DD') enter_dt, ";

                xSql += "               NVL((SELECT dept_ename from V_HDEPTCODE where dept_code = tuser.dept_code), tuser.dept_code) as dept_nm, ";
                xSql += "               tuser.email_id, ";
                xSql += "               (SELECT count(distinct open_course_id) from T_COURSE_RESULT where user_id = tuser.user_id) as edu_cnt, ";
                xSql += "               TO_CHAR(NVL(tuser.ins_dt, tuser.upt_dt), 'YYYY.MM.DD') ins_dt ";
                
                // 암호화 모듈제거
                //xSql += "          FROM T_USER_DAMO        TUSER, t_company tcompany, t_code_detail code, t_code_detail code2 ";  // v_hdutystep hdutystep, v_datachkd datachk
                xSql += "          FROM T_USER TUSER, t_company tcompany, t_code_detail code, t_code_detail code2 ";  // v_hdutystep hdutystep, v_datachkd datachk
                xSql += "          WHERE tuser.status IS NOT NULL "; // 사용자 상태가 미사용이 아닐 경우!
                xSql += "            AND code.m_cd = '0041' ";  // 0041 : 사용자 그룹
                xSql += "            AND code2.m_cd = '0044' ";   // 0044 : 상태      

                if (rParams[10].ToString() == "000001")
                {
                    xSql += " AND tuser.company_id = tcompany.company_id(+) ";
                }
                else
                {
                    xSql += " AND tuser.company_id = tcompany.company_id ";
                }

                xSql += " AND tuser.user_group = code.d_cd ";
                xSql += " AND tuser.status = code2.d_cd ";

                //xSql += " AND tuser.socialpos = datachk.dcode(+) ";
                if (rParams[10].ToString() != "000001")  // 관리자가 아닐경우
                {
                    // 행정담당, 교관, 해상인사담당, 강사는 그룹사 사용자를 모두 볼 수 있다. 2012-05-07 문서영대리가 요구
                    if (rParams[10].ToString() == "000002" || rParams[10].ToString() == "000003" || rParams[10].ToString() == "000004" || rParams[10].ToString() == "000005")
                    {
                        xSql += " AND tuser.company_id IN (SELECT company_id FROM T_COMPANY WHERE company_kind = '000001') ";
                    }
                    else
                    {
                        xSql += string.Format("           AND tuser.company_id = '{0}'", rParams[11]);  // 자신이 소속된 회원사의 회원정보만 조회...
                        xSql += string.Format("           AND tuser.status IN ('{0}','{1}','{2}') ", "000001", "000003", "000004");   // 미승인, 승인, 승인대기 만 조회 사용안함은 삭제된 정보..
                        if (rParams[10].ToString() != "000007")  // 법인사 관리자가 아니면 자기 사용자만 조회
                            xSql += string.Format("           AND tuser.user_id = '{0}' ", rParams[12]);
                    }
                }
                if (rParams[10].ToString() == "000006" && rParams[10].ToString() == "000008") // 사용자 그룹이 그룹사 수강자, 법인사 수강자 일경우 
                    xSql += string.Format("           AND tuser.user_id = '{0}'", rParams[12]);   // 자신의 정보만 조회...

                if (!string.IsNullOrEmpty(rParams[2].ToString()))
                    xSql += string.Format("           AND tcompany.company_nm like '%{0}%' ", rParams[2]);

                if (!string.IsNullOrEmpty(rParams[3].ToString()))
                    xSql += string.Format("           AND tuser.duty_step = '{0}' ", rParams[3]);

                if (!string.IsNullOrEmpty(rParams[4].ToString()))
                    xSql += string.Format("           AND tuser.socialpos = '{0}' ", rParams[4]);

                if (!string.IsNullOrEmpty(rParams[5].ToString()))
                    xSql += string.Format("           AND tuser.enter_dt like to_char(to_date('{0}','YYYYMMDD')) ", rParams[5]);

                if (!string.IsNullOrEmpty(rParams[6].ToString()))
                    xSql += string.Format("           AND tuser.user_nm_kor like '%{0}%' ", rParams[6]);

                if (!string.IsNullOrEmpty(rParams[7].ToString()))
                    xSql += string.Format("           AND tuser.user_group = '{0}' ", rParams[7]);

                //if (!string.IsNullOrEmpty(rParams[8].ToString()))
                //    xSql += string.Format("           AND tuser.personal_no like '%{0}%' ", rParams[8]);

                if (!string.IsNullOrEmpty(rParams[8].ToString()))
                    xSql += string.Format("           AND DAMO.PRED_META_VARCHAR2('LMS','T_USER','PERSONAL_NO',TUSER.SEC_PERSONAL_NO) like '%'||DAMO.PRED_META_PLAIN_V('{0}')||'%' ", rParams[8]);
                //                    xSql += string.Format("           AND SUBSTRB(DAMO.DEC_VARCHAR('LMS','T_USER','PERSONAL_NO',SEC_PERSONAL_NO),1,14) like '%{0}%' ", rParams[8]);


                if (!string.IsNullOrEmpty(rParams[9].ToString()))
                    xSql += string.Format("           AND tuser.status = '{0}' ", rParams[9]);

                // --AND (tuser.user_nm_eng_first like 'a%' or  tuser.user_nm_eng_last like '%o%') ;

                xSql += " ORDER BY tuser.user_nm_kor ASC ";

                xSql += "           ) b ";
                xSql += "    ) ";

                if (rParams[13] == "Y")
                {

                    xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                    xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                }

                xSql += " ORDER BY user_nm_kor ASC ";

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
        * Function name : GetUserDup
        * Purpose       : 사용자 주민번호로 기등록 데이터 검색
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public DataTable GetUserDup(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;

                xSql = @" select USER_ID
                            FROM T_USER_DAMO        TUSER
                           where SEC_PERSONAL_NO = DAMO.ENC_VARCHAR('LMS', 'T_USER', 'PERSONAL_NO','{0}')
                             and COMPANY_ID = '{1}' 
                        ";

                xSql = string.Format(xSql, rParams[0], rParams[1]);

                xDt = base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }


        ///************************************************************
        //* Function name : GetUserInfoForExcel
        //* Purpose       : 사용자 정보 조회
        //* Input         : string[] rParams 
        //* Output        : String Boolean
        //*************************************************************/
        //public DataTable GetUserInfoForExcel(string[] rParams)
        //{
        //    DataTable xDt = null;
        //    try
        //    {
        //        string xSql = "        SELECT  tuser.user_no, tuser.user_id, tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last user_nm_eng , tuser.user_nm_kor, " +
        //                      "                tuser.personal_no, code.d_knm user_group, tcompany.company_nm, tuser.mobile_phone, code2.d_knm status, tuser.socialpos, " +
        //                      "                tuser.enter_dt " +

        //                      "          WHERE tuser.status != '000002' " + // 사용자 상태가 미사용이 아닐 경우!
        //                      "           AND code.m_cd = '0041' " +  // 0041 : 사용자 그룹
        //                      "           AND code2.m_cd = '0044' ";   // 0044 : 상태      
        //        if (rParams[10].ToString() != "000001")  // 관리자가 아닐경우
        //            xSql += string.Format("           AND tuser.company_id = '{0}'", rParams[11]);  // 자신이 소속된 회원사의 회원정보만 조회...

        //        if (rParams[10].ToString() == "000006" && rParams[10].ToString() == "000008") // 사용자 그룹이 그룹사 수강자, 법인사 수강자 일경우 
        //            xSql += string.Format("           AND tuser.user_id = '{0}'", rParams[12]);   // 자신의 정보만 조회...



        //        if (!string.IsNullOrEmpty(rParams[2].ToString()))
        //            xSql += string.Format("           AND tcompany.company_nm like '%{0}%' ", rParams[2]);

        //        if (!string.IsNullOrEmpty(rParams[3].ToString()))
        //            xSql += string.Format("           AND tuser.duty_step = '{0}' ", rParams[3]);

        //        if (!string.IsNullOrEmpty(rParams[4].ToString()))
        //            xSql += string.Format("           AND tuser.socialpos = '{0}' ", rParams[4]);

        //        if (!string.IsNullOrEmpty(rParams[5].ToString()))
        //            xSql += string.Format("           AND tuser.enter_dt like to_char(to_date('{0}','YYYYMMDD')) ", rParams[5]);

        //        if (!string.IsNullOrEmpty(rParams[6].ToString()))
        //            xSql += string.Format("           AND tuser.user_nm_kor like '%{0}%' ", rParams[6]);

        //        if (!string.IsNullOrEmpty(rParams[7].ToString()))
        //            xSql += string.Format("           AND tuser.user_group like '%{0}%' ", rParams[7]);

        //        if (!string.IsNullOrEmpty(rParams[8].ToString()))
        //            xSql += string.Format("           AND tuser.personal_no like '%{0}%' ", rParams[8]);

        //        if (!string.IsNullOrEmpty(rParams[9].ToString()))
        //            xSql += string.Format("           AND tuser.status like '%{0}%' ", rParams[9]);

        //        xDt = base.ExecuteDataTable("LMS", xSql);
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
        //        if (rethrow) throw;
        //    }
        //    return xDt;
        //}

        /************************************************************
        * Function name : SetUserInfo
        * Purpose       : 법인사 사용자 정보 등록
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetUserInfo(string[] rParams)
        {
            string xRtn = Boolean.FalseString;

            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTransLMS = null;
            OracleCommand xCmdLMS = null;
            
            try
            {
                xTransLMS = xCnnLMS.BeginTransaction(); // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db); 
                xCmdLMS.Connection = xCnnLMS;           // Command 에 DB 연결
                xCmdLMS.Transaction = xTransLMS;        // Command 에 트랜잭션 연결
                
                try
                {
                    string xSql = string.Empty;
                    xSql = "INSERT INTO t_company          (company_id, " +                            // 업체ID
                                                           " company_code, " +                           // 업체코드(약어)
                                                           " company_nm, " +                             // 업체명
                                                           " company_kind, " +                           // 업체구분
                                                           " tax_no, " +                                 // 사업자등록번호
                                                           " reg_no, " +                                 // 법인등록번호
                                                           " empoly_ins_no, " +                          // 고용보험번호
                                                           " company_ceo, " +                            // 대표자명
                                                           " busi_conditions, " +                        // 업태
                                                           " company_type, " +                           // 종목
                                                           " zip_code, " +                               // 우편번호
                                                           " company_addr, " +                           // 주소
                                                           " tel_no, " +                                 // 전화번호
                                                           " fax_no, " +                                 // 팩스번호
                                                           " home_add, " +                               // 홈페이지
                                                           " company_scale, " +                          // 회사규모
                                                           " status, " +                                 // 상태
                                                           " ins_id, " +                                 // 작성자 ID
                                                           " ins_dt, " +                                 // 작성일자
                                                           " upt_id, " +                                 // 수정자 ID
                                                           " upt_dt) " +                                 // 수정일자
                                                           " VALUES ( '" + rParams[20] + "', " +         // 업체ID
                                                           "'', " +  // 업체코드(약어)
                                                           "'" + rParams.GetValue(9) + "', " +  // 업체명
                                                           //"'000002', " +  // 업체구분(중소기업)
                                                           "'" + rParams[21] + "', " +  // 회사구분
                                                           "'" + rParams[10] + "', " +  // 사업자 등록번호
                                                           "'" + rParams[11] + "', " +  // 법인등록번호
                                                           "'" + rParams[12] + "', " +  // 고용보험번호
                                                           "'" + rParams[2] + "', " +  // 대표자명
                                                           "'" + rParams[13] + "', " +  // 업태
                                                           "'" + rParams[14] + "', " +  // 종목
                                                           "'" + rParams[15] + "', " +  // 우편번호
                                                           "'" + rParams[16] + "', " +  // 주소
                                                           "'" + rParams[19] + "', " +  // 전화번호
                                                           "'" + rParams[7] + "', " +   // 팩스번호
                                                           "'" + rParams[17] + "', " +  // 홈페이지
                                                           "'" + rParams[18] + "', " +  // 회사규모
                                                           "'000004', " +              // 사용자 상태(승인대기)  회원가입후에는 로그인이 안되므로 승인대기 처리시킴
                                                           "'" + rParams[0] + "', " +  // 작성자 ID
                                                           " SYSDATE, " +              // 작성일자
                                                           "'" + rParams[0] + "', " +  // 수정자 ID
                                                           " SYSDATE) ";              // 수정일자

                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);
                    
                    xSql = string.Empty;
                    xSql = "INSERT INTO t_user  (user_id, " +   // 사용자 ID
                                                          " pwd, " +   // 비밀번호
                                                  " user_nm_kor, " +   // 대표자명(사용자명)
                                                    " duty_step, " +   // 사용자 직급 (Code)
                                                 " mobile_phone, " +   // 휴대폰번호
                                                 " office_phone, " +   // 사무실 번호
                                                     " email_id, " +   // 메일주소
                                                       " fax_no, " +   // 팩스번호
                                                   " supp_admin, " +   // 담당자명
                                                       " ins_id, " +   // 작성자 ID
                                                       " ins_dt, " +   // 작성일자
                                                       " upt_id, " +   // 수정자 ID
                                                       " upt_dt, " +   // 수정일자
                                                   " company_id, " +   // 회사ID
                                                       " status, " +   // 상태
                                                    " user_addr, " +   // 주소(회사주소)
                                                    " user_zip_code, " +    // 우편번호
                                                   " user_group, " +   // 사용자 그룹
                                                    " user_nm_eng_first, " +    // 
                                                    " user_nm_eng_last, " +    // 
                                                    " sms_yn, " +    // 
                                                    " mail_yn, " +    // 
                                                    " dept_code, " +    // 
                                                    " enter_dt, " +    // 
                                                    " birth_dt) " +    // 
                                                   " VALUES( '" + rParams[0] + "', " +  // 사용자 ID
                                                            "'" + rParams[1] + "', " +  // 비밀번호
                                                            "'" + rParams[8] + "', " +  // 담당자명
                                                            "'" + rParams[3] + "', " +  // 사용자 직급(Code)
                                                            "'" + rParams[4] + "', " +  // 휴대폰 번호
                                                            "'" + rParams[5] + "', " +  // 사무실 번호
                                                            "'" + rParams[28] + "', " + // 메일주소
                                                            "'" + rParams[7] + "', " +  // 팩스번호
                                                            "'" + rParams[8] + "', " +  // 담당자명
                                                            "'" + rParams[0] + "', " +  // 작성자 ID
                                                            " SYSDATE, " +              // 작성일자
                                                            "'" + rParams[0] + "', " +  // 수정자 ID
                                                            " SYSDATE, " +              // 수정일자
                                                            "'" + rParams[20] + "', " + // 회사 ID
                                                            "'000004', " +              // 사용자 상태(승인대기)  회원가입후에는 로그인이 안되므로 승인대기 처리시킴
                                                            "'" + rParams[16] + "', " + // 주소(회사주소)
                                                            "'" + rParams[15] + "', " + // 우편번호
                                                            "'000007', " +              // 사용자 그룹(법인사 관리자)
                                                            "'" + rParams[24] + "', " + // 영문명 First
                                                            "'" + rParams[25] + "', " + // 영문명 Last
                                                            "'" + rParams[30] + "', " + // SMS 수신여부
                                                            "'" + rParams[31] + "',  "+ // MAIL 수신여부
                                                            "'" + rParams[27] + "',  "; // MAIL 수신여부

                    if (rParams[29] == null)
                        xSql += string.Format(" null, ", rParams[29]);
                    else
                        xSql += string.Format(" TO_DATE('{0}', 'yyyy.MM.dd'), ", rParams[29]);
                    if (rParams[32] == null)
                        xSql += string.Format(" null ", rParams[32]);  // 생년월일
                    else
                        xSql += string.Format(" TO_DATE('{0}', 'yyyy.MM.dd') ", rParams[32]);
                    xSql += ")";



                xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);


                    xRtn = Boolean.TrueString;// +"|" + rParams[0];

                    xTransLMS.Commit(); // 트랜잭션 커밋
                }
                catch (Exception ex)
                {
                    xTransLMS.Rollback(); // Exception 발생시 롤백처리
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
        * Function name : GetUserIDOfCode
        * Purpose       : 사용자(법인사 수강자) ID 생성
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public string GetUserIDOfCode(string[] rParams, OracleCommand rCmd)
        {
            string xSql = "";
            object xTemp = null;
            string xRtnID = string.Empty; // 테이블별 리턴 ID를 담는 변수

            string xCompanyCode = string.Empty;
            string xUserID = string.Empty;

            try
            {
                //xSql = string.Format(" SELECT NVL(MAX({0}+1),0) id FROM t_notice ", rParams[0]);

                //rCmd.CommandText = xSql;
                //xRtnID = Convert.ToString(rCmd.ExecuteScalar());

                xSql = string.Empty;
                xSql += " SELECT company_code FROM T_COMPANY ";
                xSql += string.Format(" WHERE company_id = '{0}' ", rParams[0]);

                rCmd.CommandText = xSql;
                xCompanyCode = Convert.ToString(rCmd.ExecuteScalar()).ToLower();


                xSql = string.Empty;
                xSql += " SELECT max(user_id) FROM T_USER ";
                xSql += "  WHERE user_id LIKE ";
                xSql += "       (SELECT LOWER(company_code) || '%' AS user_id ";
                xSql += "          FROM T_COMPANY ";
                xSql += string.Format("         WHERE company_id = '{0}') ", rParams[0]);
                xSql += "    AND user_group = '000008' "; // 그룹사 관리자만 적용... 
                //xSql += "         ORDER BY user_id DESC ";

                rCmd.CommandText = xSql;
                xRtnID = Convert.ToString(rCmd.ExecuteScalar());

                if (string.IsNullOrEmpty(xRtnID))
                    xUserID = xCompanyCode + "00001";
                else
                {
                    xUserID = xCompanyCode + Convert.ToInt32(Convert.ToInt32(xRtnID.Substring(3, 5)) + 1).ToString("00000");
                }


                //base.ExecuteDataTable("LMS", rCmd);

                //if (xRtnID == "0")
                //    xRtnID = "1";
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return xUserID;
        }

        /************************************************************
        * Function name : SetUser
        * Purpose       : 법인사 사용자 정보 등록
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetUser(string[] rParams)
        {
            string xRtn = Boolean.FalseString;
            string xUserID = string.Empty;
            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTransLMS = null;
            OracleCommand xCmdLMS = null;


            try
            {
                xTransLMS = xCnnLMS.BeginTransaction(); // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db); 
                xCmdLMS.Connection = xCnnLMS;           // Command 에 DB 연결
                xCmdLMS.Transaction = xTransLMS;        // Command 에 트랜잭션 연결

                // 법인사 수강자 이면...
                if (rParams[20] == "000008")
                {
                    xUserID = GetUserIDOfCode(new string[] { rParams[18] }, xCmdLMS);
                    rParams[0] = xUserID;
                }

                try
                {
                    string xSql = "INSERT INTO t_user  (user_id, " +   // 사용자 ID
                                                      " pwd, " +       // 비밀번호
                                                      " user_no, " +   // 사번
                                                      " user_nm_kor, " +  // 사용자명(한글)
                                                      " personal_no, " +  // 주민번호 
                                                      " user_nm_eng_first, " +  // 사용자명(영문)First
                                                      " user_nm_eng_last, " +   // 사용자명(영문)Last
                                                      " duty_step, " +  // 직급
                                                      " dept_code, " +  // 부서


                                                      " mobile_phone, " +  // 휴대폰번호


                                                      " duty_gu, " +  // 고용형태
                                                      " email_id, " +  // 메일주소
                                                      " office_phone, " +  // 전화번호
                                                      " fax_no, " +  // 팩스번호
                                                      " supp_admin, " + // 승인 관리자
                                                      " admin_phone, " +  // 업체  연락처
                                                      " admin_id, " +  // 업체 담당자 
                                                      " status, " +  // 상태
                                                      " company_id, " +  // 회사 ID
                                                      " enter_dt, " +  // 입사일자
                                                      " user_group, " +  // 사용자그룹코드


                                                      " socialpos, " + // 신분코드
                                                      " user_zip_code, " +  // 우편번호
                                                      " user_addr, " +  // 주소
                                                      " sms_yn, " +  // SMS 수신여부
                                                      " mail_yn, " +  // MAIL 수신여부
                                                      " ins_id, " +  // 등록자


                                                      " ins_dt, " +  // 등록일자
                                                      " upt_id, " +  // 수정자 
                                                      " upt_dt, " +  // 수정일자
                                                      " trainee_class, " +  // 
                                                      " birth_dt) " +  // 생년월일
                                                      " VALUES( ";
                    xSql += string.Format(" '{0}', ", rParams[0]);  // 사용자 ID
                    xSql += string.Format(" '{0}', ", rParams[1]);  // 비밀번호
                    xSql += string.Format(" '{0}', ", rParams[2]);  // 사번
                    xSql += string.Format(" '{0}', ", rParams[3]);  // 사용자명(한글)
                    xSql += string.Format(" '{0}', ", rParams[4]);  // 주민번호  
                    xSql += string.Format(" '{0}', ", rParams[5]);  // 사용자명 영문(first)
                    xSql += string.Format(" '{0}', ", rParams[6]);  // 사용자명 영문(last)
                    xSql += string.Format(" '{0}', ", rParams[7]);  // 직급  
                    xSql += string.Format(" '{0}', ", rParams[8]);  // 부서


                    xSql += string.Format(" '{0}', ", rParams[9]);  // 휴대폰번호


                    xSql += string.Format(" '{0}', ", rParams[10]);  // 고용형태
                    xSql += string.Format(" '{0}', ", rParams[11]);  // 메일주소
                    xSql += string.Format(" '{0}', ", rParams[12]);  // 전화번호
                    xSql += string.Format(" '{0}', ", rParams[13]);  // 팩스번호
                    xSql += string.Format(" '{0}', ", rParams[14]);  // 승인 관리자
                    xSql += string.Format(" '{0}', ", rParams[15]);  // 업체 연락처
                    xSql += string.Format(" '{0}', ", rParams[16]);  // 업체 담당자


                    xSql += string.Format(" '{0}', ", rParams[17]);  // 상태
                    xSql += string.Format(" '{0}', ", rParams[18]);  // 회사 ID

                    if(rParams[19] == null)
                        xSql += string.Format(" null, ", rParams[19]);  // 입사일자
                    else 
                        xSql += string.Format(" TO_DATE('{0}', 'yyyy.MM.dd'), ", rParams[19]);  // 입사일자

                    xSql += string.Format(" '{0}', ", rParams[20]);  // 사용자 그룹코드
                    xSql += string.Format(" '{0}', ", rParams[21]);  // 신분코드
                    xSql += string.Format(" '{0}', ", rParams[22]);  // 우편번호
                    xSql += string.Format(" '{0}', ", rParams[23]);  // 주소
                    xSql += string.Format(" '{0}', ", rParams[24]);  // 주소
                    xSql += string.Format(" '{0}', ", rParams[25]);  // 주소
                    xSql += string.Format(" '{0}', ", rParams[26]);  // 등록자


                    xSql += " SYSDATE, "; // 등록일자
                    xSql += string.Format(" '{0}', ", rParams[27]);  // 수정자


                    xSql += " SYSDATE, "; // 수정일자
                    xSql += string.Format(" '{0}', ", rParams[28]);  // 등록자

                    if (rParams[29] == null)
                        xSql += string.Format(" null ", rParams[29]);  // 생년월일
                    else
                        xSql += string.Format(" TO_DATE('{0}', 'yyyy.MM.dd') ", rParams[29]);


                    xSql += " ) ";

                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);



                    //xRtn = Boolean.TrueString;
                    xRtn = Boolean.TrueString + "|" + rParams[0];

                    xTransLMS.Commit(); // 트랜잭션 커밋
                }
                catch (Exception ex)
                {
                    xTransLMS.Rollback(); // Exception 발생시 롤백처리
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

        public DataTable GetMenuAuthority(string rMenuGroup, string rUserGroup)
        {
            DataTable xReturn = null;
            try
            {
                string xSql = null;
                /*
                xSql = " SELECT ";
                xSql += " inquiry_yn, ";
                xSql += " edit_yn, ";
                xSql += " del_yn, ";
                xSql += " admin_yn ";
                xSql += "FROM ";
                xSql += " t_menu_group ";
                xSql += "WHERE ";
                xSql += " menu_group = '" + rMenuGroup + "' ";
                xSql += " AND user_id = '" + rUserGroup + "' ";
                */
                
                //2013.05.22 Seojw
                /*
                    상단 메뉴에서 상세 메뉴가 아닌 중간메뉴 클릭 시 상세메뉴 중 첫번째로 이동하는데 
                    이동한 메뉴의 권한이 잘못타는 오류 수정.
                 */

                xSql = @"
                SELECT INQUIRY_YN,
                       EDIT_YN,
                       DEL_YN,
                       ADMIN_YN
                  FROM T_MENU_GROUP
                 WHERE MENU_GROUP = DECODE((SELECT INQUIRY_YN
                                             FROM T_MENU_GROUP
                                            WHERE MENU_GROUP = '" + rMenuGroup + @"'
                                              AND USER_ID = '" + rUserGroup + @"'), 'N', (SELECT X.MENU_GROUP
                                               FROM (SELECT A.MENU_GROUP
                                                       FROM T_MENU_GROUP A,
                                                            T_MENU       B
                                                      WHERE A.MENU_GROUP =
                                                            B.MENU_GROUP
                                                        AND A.MENU_GROUP LIKE
                                                            SUBSTR('" + rMenuGroup + @"', 1, 2) || '%'
                                                        AND A.USER_ID = '" + rUserGroup + @"'
                                                        AND A.INQUIRY_YN = 'Y'
                                                        AND SUBSTR(A.MENU_GROUP, 3, 1) != '0'
                                                      ORDER BY B.SEQ) X
                                              WHERE ROWNUM = 1), '" + rMenuGroup + @"')
                   AND USER_ID = '" + rUserGroup + @"'
                ";

                xReturn = base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xReturn;
        }


        public DataTable GetMenuAuthority(string rMenuGroup)
        {
            DataTable xReturn = null;
            try
            {
                string xSql = null;
                xSql = " SELECT ";
                xSql += " inquiry_yn, ";
                xSql += " edit_yn, ";
                xSql += " del_yn, ";
                xSql += " admin_yn ";
                xSql += "FROM ";
                xSql += " t_menu_group ";
                xSql += "WHERE ";
                xSql += " menu_group = '" + rMenuGroup + "' ";
                

                xReturn = base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xReturn;
        }

        /************************************************************
         * Function name : SetUserEdit
         * Purpose       : 사용자정보 변경

         * Input         : string[] rParams 
         * Output        : String Bollean Type
         *************************************************************/
        public string SetUserEdit(string[] rParams)
        {
            string xRtn = Boolean.FalseString;

            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTrnsLMS = null;
            OracleCommand xCmdLMS = null;

            try
            {
                xTrnsLMS = xCnnLMS.BeginTransaction();  // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db); 
                xCmdLMS.Connection = xCnnLMS;
                xCmdLMS.Transaction = xTrnsLMS;

                //그룹아이디
                string sGroupId = rParams[17].ToString().Trim();
                
                try
                {
                    string xsql = " UPDATE t_user SET ";
                    //xsql += string.Format(" user_id = '{0}', ", rParams[0]);

                    //2013.07.10 문서영대리 요청
                    //법인사 관리자,법인사 수강자만 비번 변경가능
                    if (sGroupId == "000007" || sGroupId == "000008")
                    {
                        //비밀번호가 입력됬을때만
                        if (rParams[1].ToString().Trim() != string.Empty)
                        {
                            xsql += string.Format(" pwd = '{0}', ", rParams[1]);
                            
                        }

                        //주민번호가 입력됬을때만
                        if (rParams[6].ToString().Trim().Replace("-","") != string.Empty)
                        {
                            xsql += string.Format(" personal_no = '{0}', ", rParams[6]);
                        }
                    }

                    xsql += string.Format(" user_nm_kor = '{0}', ", rParams[2]);
                    xsql += string.Format(" user_nm_eng_first = '{0}', ", rParams[3]);
                    xsql += string.Format(" user_nm_eng_last = '{0}', ", rParams[4]);
                    xsql += string.Format(" duty_step = '{0}', ", rParams[5]);
                    
                    xsql += string.Format(" email_id = '{0}', ", rParams[7]);
                    xsql += string.Format(" office_phone = '{0}', ", rParams[8]);
                    xsql += string.Format(" mobile_phone = '{0}', ", rParams[9]);
                    xsql += string.Format(" company_id  = '{0}', ", rParams[11]);
                    xsql += string.Format(" user_zip_code = '{0}', ", rParams[12]);
                    xsql += string.Format(" user_addr = '{0}', ", rParams[13]);
                    xsql += string.Format(" sms_yn = '{0}', ", rParams[14]);
                    xsql += string.Format(" mail_yn = '{0}', ", rParams[15]);
                    xsql += string.Format(" upt_id = '{0}', ", rParams[16]);
                    if (rParams.Length == 22)
                    {
                        if (rParams[20] == "000001")
                            xsql += string.Format(" user_group = '{0}', ", rParams[17]);
                    }

                    xsql += string.Format(" trainee_class = '{0}', ", rParams[18]);

                    if(rParams[19] == null)
                        xsql += string.Format(" enter_dt = null, ", rParams[19]);
                    else 
                        xsql += string.Format(" enter_dt = TO_DATE('{0}', 'yyyy.MM.dd'), ", rParams[19]);

                    if (rParams[21] == null)
                        xsql += string.Format(" birth_dt = null, ", rParams[21]);
                    else
                        xsql += string.Format(" birth_dt = TO_DATE('{0}', 'yyyy.MM.dd'), ", rParams[21]);

                    if (rParams[22] != null)
                        xsql += string.Format(" dept_code = '{0}', ", rParams[22]);

                    xsql += " upt_dt = SYSDATE ";
                    xsql += string.Format(" WHERE UPPER(user_id) = UPPER('{0}') ", rParams[0]);


                    xCmdLMS.CommandText = xsql;
                    base.Execute(db, xCmdLMS, xTrnsLMS);

                    xRtn = Boolean.TrueString;

                    xTrnsLMS.Commit(); // 트랜잭션 커밋

                }
                catch (Exception ex)
                {
                    xTrnsLMS.Rollback(); // Exception 발생시 롤백처리...
                    throw ex;
                }
                finally
                {
                    if (xCmdLMS != null)
                        xCmdLMS.Dispose();

                    if (xTrnsLMS != null)
                        xTrnsLMS.Dispose();
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
        * Function name : SetUserDelete
        * Purpose       : 사용자정보 삭제(변경) 실제로 삭제 하지는 않음
        * Input         : string[] rParams 
        * Output        : String Bollean Type
        *************************************************************/
        public string SetUserDelete(string[,] rParams)
        {

            string xRtn = Boolean.FalseString;

            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTrnsLMS = null;
            OracleCommand xCmdLMS = null;

            try
            {
                xTrnsLMS = xCnnLMS.BeginTransaction();  // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db); 
                xCmdLMS.Connection = xCnnLMS;
                xCmdLMS.Transaction = xTrnsLMS;

                try
                {
                    //int j = xParams.GetLength(0);
                    for (int i = 0; i < rParams.GetLength(0); i++)
                    {
                        string xsql = " UPDATE t_user SET ";
                        xsql += string.Format(" Status = '{0}', ", rParams[i, 2]);
                        xsql += string.Format(" upt_id = '{0}', ", rParams[i, 1]);
                        xsql += " upt_dt = SYSDATE ";
                        xsql += string.Format(" WHERE user_id = '{0}'", rParams[i, 0]);

                        xCmdLMS.CommandText = xsql;
                        base.Execute(db, xCmdLMS, xTrnsLMS);
                    }


                    xTrnsLMS.Commit(); // 트랜잭션 커밋
                    xRtn = Boolean.TrueString;
                }
                catch (Exception ex)
                {
                    xTrnsLMS.Rollback(); // Exception 발생시 롤백처리...
                    throw ex;
                }
                finally
                {
                    if (xCmdLMS != null)
                        xCmdLMS.Dispose();

                    if (xTrnsLMS != null)
                        xTrnsLMS.Dispose();
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
        * Function name : GetCompanyUserInfo
        * Purpose       : 법인사 수강자의 경우 해당 법인사 관리자 정보를 입력하기 위한 정보
        * Input         : string[] rParams 
        * Output        : Datatable
        *************************************************************/
        public DataTable GetCompanyUserInfo(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;

                /*
                xSql += "SELECT tuser.user_id, company.fax_no, company.tel_no ";
                xSql += "  FROM t_user tuser, t_company company ";
                xSql += " WHERE tuser.user_id = company.ins_id ";
                xSql += string.Format(" AND user_id = ( SELECT ins_id FROM t_company WHERE company_id = '{0}') ", rParams[0]);
                */

                /*
                 * 2014.03.19 seojw
                 * 이전에는 담당자를 법인사 등록시 등록자(ins_id)로 Bind 했으나
                 * 문서영대리 요청으로 각 회사의 법인사 관리자를 Bind하기로 함.
                 * 법인사 관리자는 각 회사에 1명만 등록된다고 함. 그러나 만약을 위해 여러명 존재할 경우 
                 * 오류방지를 위해 1명만 들고 오도록 함.
                */
                xSql = @"
                SELECT B.USER_ID,
                       A.FAX_NO,
                       A.TEL_NO,
                       SUBSTR(A.TAX_NO, 6, 5) TAX_NO_PW
                  FROM T_COMPANY A,
                       (
                        SELECT Z.*
                          FROM (
                                
                                SELECT ROW_NUMBER() OVER(ORDER BY INS_DT DESC) RN,
                                        USER_ID,
                                        COMPANY_ID
                                  FROM T_USER
                                 WHERE COMPANY_ID = '{0}'
                                   AND USER_GROUP = '000007'
                                ) Z
                         WHERE Z.RN = 1
                        ) B
                 WHERE A.COMPANY_ID = B.COMPANY_ID(+)
                   AND A.COMPANY_ID = '{1}'
                ";

                xSql = string.Format(xSql, rParams[0], rParams[0]);

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
        * Function name : SetUserUpload
        * Purpose       : 법인사 사용자 정보 엑셀 업로드

        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetUserUpload(params object[] rParams)
        {
            string xRtn = Boolean.FalseString;

            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTransLMS = null;
            OracleCommand xCmdLMS = null;

            try
            {
                xTransLMS = xCnnLMS.BeginTransaction(); // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db); 
                xCmdLMS.Connection = xCnnLMS;           // Command 에 DB 연결
                xCmdLMS.Transaction = xTransLMS;        // Command 에 트랜잭션 연결

                DataTable xDtUser = (DataTable)rParams[0];

                //xRow["COMPANY_ID"].ToString()

                string xUserID = string.Empty;

                try
                {
                    foreach (DataRow xRow in xDtUser.Rows)
                    {
                        string xSql = string.Empty;

                        string[] obj = new string[2];
                        obj[0] = xRow["PERSONAL_NO"].ToString();
                        obj[1] = xRow["COMPANY_ID"].ToString();


                        //2014.03.20 seojw(문서영 대리 요청)
                        //법인사 수강자인 경우 주민번호로 기등록된 데이터 있는지 검색
                        //만약 데이터 있다면 insert가 아닌 update 처리 
                        if (GetUserDup(obj).Rows.Count == 0)
                        {
                            xUserID = GetUserIDOfCode(new string[] { xRow["COMPANY_ID"].ToString() }, xCmdLMS);

                            xSql = "INSERT INTO t_user  (user_id, " +   // 사용자 ID
                                                              " pwd, " +       // 비밀번호
                                                              " user_no, " +   // 사번
                                                              " user_nm_kor, " +  // 사용자명(한글)
                                                              " personal_no, " +  // 주민번호 
                                                              " user_nm_eng_first, " +  // 사용자명(영문)First
                                                              " user_nm_eng_last, " +   // 사용자명(영문)Last
                                                              " duty_step, " +  // 직급
                                                              " dept_code, " +  // 부서

                                                              " mobile_phone, " +  // 휴대폰번호

                                                              " duty_gu, " +  // 고용형태
                                                              " email_id, " +  // 메일주소
                                                              " office_phone, " +  // 전화번호
                                                              " fax_no, " +  // 팩스번호
                                                              " supp_admin, " + // 업체담당자

                                                              " admin_phone, " +  // 업체 담당자 연락처

                                                              " admin_id, " +  // 업체 담당자 
                                                              " status, " +  // 상태
                                                              " company_id, " +  // 회사 ID
                                                              " enter_dt, " +  // 입사일자
                                                              " user_group, " +  // 사용자그룹코드

                                                              " socialpos, " + // 신분코드
                                                              " user_zip_code, " +  // 우편번호
                                                              " user_addr, " +  // 주소
                                                              " sms_yn, " +  // SMS 수신여부
                                                              " mail_yn, " +  // MAIL 수신여부
                                                              " ins_id, " +  // 등록자

                                                              " ins_dt, " +  // 등록일자
                                                              " upt_id, " +  // 수정자 
                                                              " upt_dt, " +  // 수정일자
                                                              " trainee_class) " +
                                                              " VALUES( ";
                            xSql += string.Format(" '{0}', ", xUserID); //xRow["ID"].ToString());  // 사용자 ID
                            xSql += string.Format(" '{0}', ", xRow["PWD"]);  // 비밀번호
                            xSql += string.Format(" '{0}', ", string.Empty);  // 사번
                            xSql += string.Format(" '{0}', ", xRow["USER_NM_KOR"].ToString());  // 사용자명(한글)
                            xSql += string.Format(" '{0}', ", xRow["PERSONAL_NO"].ToString());  // 주민번호  
                            xSql += string.Format(" '{0}', ", xRow["USER_NM_ENG_FIRST"].ToString());  // 사용자명 영문(first)
                            xSql += string.Format(" '{0}', ", xRow["USER_NM_ENG_LAST"].ToString());  // 사용자명 영문(last)
                            xSql += string.Format(" '{0}', ", xRow["DUTY_STEP"].ToString());  // 직급  
                            xSql += string.Format(" '{0}', ", string.Empty);  // 부서

                            xSql += string.Format(" '{0}', ", xRow["MOBILE_PHONE"].ToString());  // 휴대폰번호

                            xSql += string.Format(" '{0}', ", string.Empty);  // 고용형태
                            xSql += string.Format(" '{0}', ", xRow["EMAIL_ID"].ToString());  // 메일주소
                            xSql += string.Format(" '{0}', ", xRow["OFFICE_PHONE"].ToString());  // 전화번호
                            xSql += string.Format(" '{0}', ", string.Empty);  // 팩스번호
                            xSql += string.Format(" '{0}', ", string.Empty);  // 승인 관리자
                            xSql += string.Format(" '{0}', ", xRow["ADMIN_PHONE"].ToString());  // 업체 담당자 연락처

                            xSql += string.Format(" '{0}', ", xRow["USER_ID"].ToString());  // 업체 담당자

                            xSql += string.Format(" '{0}', ", xRow["STATUS"].ToString());  // 상태
                            xSql += string.Format(" '{0}', ", xRow["COMPANY_ID"].ToString());  // 회사 ID
                            if (string.IsNullOrEmpty(xRow["ENTER_DT"].ToString()))
                                xSql += " null, ";  // 입사일자
                            else
                                xSql += string.Format(" TO_DATE('{0}', 'yyyy.MM.dd'), ", xRow["ENTER_DT"].ToString());  // 입사일자
                            xSql += string.Format(" '{0}', ", xRow["USER_GROUP"].ToString());  // 사용자 그룹코드
                            xSql += string.Format(" '{0}', ", string.Empty);  // 신분코드
                            xSql += string.Format(" '{0}', ", xRow["USER_ZIP_CODE"].ToString());  // 우편번호
                            xSql += string.Format(" '{0}', ", xRow["USER_ADDR1"].ToString());// + " | " + xRow["USER_ADDR2"].ToString());  // 주소

                            //if (xRow["SMS_YN"].ToString() != "N")
                            xSql += string.Format(" '{0}', ", "Y");  // SMS 수신여부
                            //else
                            //  xSql += string.Format(" '{0}', ", "N");  // SMS 수신여부

                            //if (xRow["MAIL_YN"].ToString() != "N")
                            xSql += string.Format(" '{0}', ", "Y");  // MAIL 수신여부
                            //else
                            //    xSql += string.Format(" '{0}', ", "N");  // MAIL 수신여부

                            xSql += string.Format(" '{0}', ", xRow["USER_ID"].ToString());  // 등록자

                            xSql += " SYSDATE, "; // 등록일자
                            xSql += string.Format(" '{0}', ", xRow["USER_ID"].ToString());  // 수정자

                            xSql += " SYSDATE, "; // 수정일자
                            xSql += string.Format(" '{0}') ", xRow["TRAINEE_CLASS"].ToString());  // 훈련생구분
                        }
                        else
                        { 
                            xSql = " UPDATE t_user SET ";

                            xSql += string.Format(" user_nm_kor = '{0}', ", xRow["USER_NM_KOR"].ToString());
                            xSql += string.Format(" user_nm_eng_first = '{0}', ", xRow["USER_NM_ENG_FIRST"].ToString());
                            xSql += string.Format(" user_nm_eng_last = '{0}', ", xRow["USER_NM_ENG_LAST"].ToString());
                            xSql += string.Format(" duty_step = '{0}', ", xRow["DUTY_STEP"].ToString());

                            xSql += string.Format(" email_id = '{0}', ", xRow["EMAIL_ID"].ToString());
                            xSql += string.Format(" office_phone = '{0}', ", xRow["OFFICE_PHONE"].ToString());
                            xSql += string.Format(" mobile_phone = '{0}', ", xRow["MOBILE_PHONE"].ToString());
                            xSql += string.Format(" company_id  = '{0}', ", xRow["COMPANY_ID"].ToString());
                            xSql += string.Format(" user_zip_code = '{0}', ", xRow["USER_ZIP_CODE"].ToString());
                            xSql += string.Format(" user_addr = '{0}', ", xRow["USER_ADDR1"].ToString());
                            xSql += string.Format(" sms_yn = '{0}', ", "Y");
                            xSql += string.Format(" mail_yn = '{0}', ", "Y");
                            xSql += string.Format(" upt_id = '{0}', ", xRow["USER_ID"].ToString());
                            xSql += string.Format(" user_group = '{0}', ", xRow["USER_GROUP"].ToString());
                            xSql += string.Format(" trainee_class = '{0}', ", xRow["TRAINEE_CLASS"].ToString());
                            
                            if (string.IsNullOrEmpty(xRow["ENTER_DT"].ToString()))
                                xSql += " enter_dt = null, ";  // 입사일자
                            else
                                xSql += string.Format(" enter_dt = TO_DATE('{0}', 'yyyy.MM.dd'), ", xRow["ENTER_DT"].ToString());  // 
                            
                            xSql += " upt_dt = SYSDATE ";
                            
                            xSql += string.Format(" WHERE UPPER(user_id) = UPPER('{0}') ", GetUserDup(obj).Rows[0]["USER_ID"].ToString());

                        }
                        

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);
                    }


                    xRtn = Boolean.TrueString;

                    xTransLMS.Commit(); // 트랜잭션 커밋
                }
                catch (Exception ex)
                {
                    xTransLMS.Rollback(); // Exception 발생시 롤백처리
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

        public DataTable GetUserImage(string[] rParams)
        {
            DataTable xDt = null;

            try
            {
                string xSql = string.Empty;
                //xSql += " SELECT SNO, FILENAME, IMAGEFILE  FROM HPERINFOIMAGE ";

                xSql += " SELECT * FROM ( ";
                xSql += "   SELECT rownum rnum, b.* FROM ( ";
                xSql += "          SELECT sno, filename, imagefile, count(*) over() totalrecordcount FROM HPERINFOIMAGE ";
                xSql += "           ) b ) ";
                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[1]));


                xDt = base.ExecuteDataTable("PUSSHR", xSql);
            }
            catch(Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return xDt;
        }



        /************************************************************
        * Function name : SetUserImageUpload
        * Purpose       : 그룹사 사용자 이미지 정보 업로드

        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetUserImageUpload(params object[] rParams)
        {
            string xRtn = Boolean.FalseString;

            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTransLMS = null;
            OracleCommand xCmdLMS = null;
            
            try
            {
                xTransLMS = xCnnLMS.BeginTransaction(); // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db); 
                xCmdLMS.Connection = xCnnLMS;           // Command 에 DB 연결
                xCmdLMS.Transaction = xTransLMS;        // Command 에 트랜잭션 연결

                DataTable xDt = (DataTable)rParams[0];

                try
                {
                    int nCount = 0;
                    foreach (DataRow xDr in xDt.Rows)
                    {
                        

                        //FileStream temp = (FileStream)xDr["imagefile"];



                        byte[] xFileData = (byte[])xDr["imagefile"];//new byte[temp.Length]; //(byte[])xDr["imagefile"];




                        string xSql = string.Empty;

                        xSql += " UPDATE t_user SET ";
                        xSql += string.Format(" pic_file_nm = '{0}', ", xDr["filename"].ToString().Trim().Replace("'", "''"));
                        xSql += "pic_file =  :ATTFILE ";
                        xSql += string.Format(" WHERE user_id = '{0}' ", xDr["sno"].ToString());


                        OracleParameter ATTFILE = new OracleParameter();
                        ATTFILE.OracleType = OracleType.Blob;
                        ATTFILE.ParameterName = "ATTFILE";
                        ATTFILE.Value = xFileData;

                        xCmdLMS.Parameters.Add(ATTFILE);

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);

                        if (nCount == 100)
                        {
                            xRtn = Boolean.TrueString;
                            xTransLMS.Commit(); // 트랜잭션 커밋
                            nCount = 0;

                            xTransLMS = xCnnLMS.BeginTransaction(); // 트랜잭션 시작
                            xCmdLMS = base.GetSqlCommand(db); 
                            xCmdLMS.Connection = xCnnLMS;           // Command 에 DB 연결
                            xCmdLMS.Transaction = xTransLMS;        // Command 에 트랜잭션 연결
                        }

                        nCount++;
                    }
                }
                catch (Exception ex)
                {
                    xTransLMS.Rollback(); // Exception 발생시 롤백처리
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

        public DataTable GetUserPwd(string[] rParams)
        {
            DataTable xDt = null;

            try
            {
                string xSql = string.Empty;

                xSql += " SELECT user_id, pwd FROM t_user ";
                xSql += " WHERE pwd NOT LIKE '%==' ";
                xSql += "   AND pwd IS NOT NULL ";
                //xSql += "  WHERE status = '000004' ";

                //xSql += " SELECT SNO, FILENAME, IMAGEFILE  FROM HPERINFOIMAGE ";

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
        * Function name : SetUserInterfacePwd
        * Purpose       : 육상 사용자 비밀번호 변경

        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetUserInterfacePwd(params object[] rParams)
        {
            string xRtn = Boolean.FalseString;

            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTransLMS = null;
            OracleCommand xCmdLMS = null;

            try
            {
                xTransLMS = xCnnLMS.BeginTransaction(); // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db); 
                xCmdLMS.Connection = xCnnLMS;           // Command 에 DB 연결
                xCmdLMS.Transaction = xTransLMS;        // Command 에 트랜잭션 연결

                string[,] xParams = (string[,])rParams[0];
                //DataTable xDt = (DataTable)rParams[0];
                string xSql = string.Empty;
                try
                {
                    //foreach (DataRow xDr in xDt.Rows)
                    for (int i = 0; i < xParams.GetLength(1); i++)  // 208
                    //foreach (DataRow xDr in xDt.Rows) 
                    {
                     
                        //string xUserID = GetUserIDCheck(new string[] { xDr["user_id"].ToString() }, xCmdLMS);
                        xSql = string.Empty;
                        xSql += " UPDATE t_user SET ";
                        xSql += string.Format(" pwd = '{0}' ", xParams[1,i]);
                        xSql += string.Format(" WHERE user_id = '{0}' ", xParams[0, i]);

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);

                    }

                    xRtn = Boolean.TrueString;
                    xTransLMS.Commit(); // 트랜잭션 커밋

                }
                catch (Exception ex)
                {
                    xTransLMS.Rollback(); // Exception 발생시 롤백처리
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
        * Function name : SetUserUpdateSSO
        * Purpose       : 아직 사용자 정보가 지정되지 않은 육상사용자 Update 
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public string SetUserUpdateSSO(string[] rParams)
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
                
                DataTable xDt = null;

                xTransLMS = xCnnLMS.BeginTransaction();  // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db); 
                xCmdLMS.Connection = xCnnLMS;
                xCmdLMS.Transaction = xTransLMS;

                try
                {
                    string xSql = string.Empty;

                    xSql += " SELECT user_id FROM t_user ";
                    xSql += "  WHERE status IS NULL ";
                    xSql += "    AND user_group IS NULL ";
                    xSql += "    AND use_yn IS NOT NULL ";
                    xDt = base.ExecuteDataTable("LMS", xSql);


                    foreach (DataRow xDr in xDt.Rows)
                    {

                        // 기존 공지사항 Update
                        xSql = string.Empty;
                        xSql += " UPDATE t_user SET ";
                        xSql += " status = '000001' ";
                        xSql += " ,user_group = '000009'";
                        xSql += string.Format(" WHERE user_id = '{0}' ", xDr["user_id"].ToString());
                        //xSql += string.Format(" WHERE user_id = '{0}' ", rParams[0]);

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);

                        
                    }
                    if (xDt.Rows.Count > 0)
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


        /************************************************************
         * Function name : SetMyInfoEdit
         * Purpose       : 사용자정보 변경

         * Input         : string[] rParams 
         * Output        : String Bollean Type
         *************************************************************/
        public string SetMyInfoEdit(string[] rParams)
        {
            string xRtn = Boolean.FalseString;

            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTrnsLMS = null;
            OracleCommand xCmdLMS = null;

            try
            {
                xTrnsLMS = xCnnLMS.BeginTransaction();  // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db);
                xCmdLMS.Connection = xCnnLMS;
                xCmdLMS.Transaction = xTrnsLMS;

                try
                {
                    string xsql = " UPDATE t_user SET ";
                    
                    //새 비밀번호가 입력됬을때만
                    if (rParams[1].ToString().Trim() != string.Empty)
                    {
                        xsql += string.Format(" pwd = '{0}', ", rParams[1]);

                    }
                    
                    xsql += string.Format(" user_nm_eng_first = '{0}', ", rParams[3]);
                    xsql += string.Format(" user_nm_eng_last = '{0}', ", rParams[4]);
                    xsql += string.Format(" duty_step = '{0}', ", rParams[5]);

                    xsql += string.Format(" email_id = '{0}', ", rParams[7]);
                    xsql += string.Format(" office_phone = '{0}', ", rParams[8]);
                    xsql += string.Format(" mobile_phone = '{0}', ", rParams[9]);
                    xsql += string.Format(" user_zip_code = '{0}', ", rParams[12]);
                    xsql += string.Format(" user_addr = '{0}', ", rParams[13]);
                    xsql += string.Format(" sms_yn = '{0}', ", rParams[14]);
                    xsql += string.Format(" mail_yn = '{0}', ", rParams[15]);
                    xsql += string.Format(" upt_id = '{0}', ", rParams[16]);

                    if (rParams[19] == null)
                        xsql += string.Format(" enter_dt = null, ", rParams[19]);
                    else
                        xsql += string.Format(" enter_dt = TO_DATE('{0}', 'yyyy.MM.dd'), ", rParams[19]);

                    if (rParams[21] == null)
                        xsql += string.Format(" birth_dt = null, ", rParams[21]);
                    else
                        xsql += string.Format(" birth_dt = TO_DATE('{0}', 'yyyy.MM.dd'), ", rParams[21]);

                    if (rParams[22] != null)
                        xsql += string.Format(" dept_code = '{0}', ", rParams[22]);

                    xsql += " upt_dt = SYSDATE ";
                    xsql += string.Format(" WHERE UPPER(user_id) = UPPER('{0}') ", rParams[0]);


                    xCmdLMS.CommandText = xsql;
                    base.Execute(db, xCmdLMS, xTrnsLMS);

                    xRtn = Boolean.TrueString;

                    xTrnsLMS.Commit(); // 트랜잭션 커밋

                }
                catch (Exception ex)
                {
                    xTrnsLMS.Rollback(); // Exception 발생시 롤백처리...
                    throw ex;
                }
                finally
                {
                    if (xCmdLMS != null)
                        xCmdLMS.Dispose();

                    if (xTrnsLMS != null)
                        xTrnsLMS.Dispose();
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
