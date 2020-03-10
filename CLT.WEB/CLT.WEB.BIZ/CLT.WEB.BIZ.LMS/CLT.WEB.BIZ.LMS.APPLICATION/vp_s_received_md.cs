using System;
using System.Collections.Generic;
using System.Text;

// 필수 using 문
using System.IO;
using System.Data;
using System.Data.OracleClient;
using CLT.WEB.BIZ.FX.BIZBASE;
using System.Data.Common;
using CLT.WEB.BIZ.LMS.COMMON;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Collections;
using System.Globalization;


namespace CLT.WEB.BIZ.LMS.APPLICATION
{
    /// <summary>
    /// 1. 작업개요 : vp_c_course_md Class
    /// 
    /// 2. 주요기능 : 과정(등록,개설) 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_c_course_md
    /// 
    /// 4. 작 업 자 : 임양춘 / 2011.12.08
    /// </summary>
    public class vp_s_received_md : DAC
    {
        /************************************************************
      * Function name : GetTakingCourseList
      * Purpose       : 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetReceivedList(string[] rParams, CultureInfo rArgCultureInfo)
        public DataTable GetReceivedList(string[] rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = this.PageSize.ToString(); // pagesize
                //xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                //xParams[2] = user_id


                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT 
                          O.OPEN_COURSE_ID 
                          , O.COURSE_TYPE ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += "     , C.course_nm ";  // 과정명

                }
                else
                {
                    xSql += "     , C.COURSE_NM_ABBR as course_nm ";
                }
                xSql += @"
                          , O.COURSE_YEAR || '-' || O.COURSE_SEQ AS COURSE_SEQ
                          , TO_CHAR(O.COURSE_BEGIN_APPLY_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(O.COURSE_END_APPLY_DT, 'YYYY.MM.DD') AS APPLY_DT
                          , TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_DT
                          , C.COURSE_DAY 

                          /*, (SELECT COUNT(USER_ID) FROM T_COURSE_RESULT WHERE OPEN_COURSE_ID = O.OPEN_COURSE_ID) */
                          /*해당 사업주의 신청인원을 확인 해야 함 */
                        ";
                if (rParams[7] == "000001")
                {
                    xSql += @"
                          , (SELECT COUNT(R.USER_ID) FROM T_COURSE_RESULT R, T_USER U 
                             WHERE R.OPEN_COURSE_ID = O.OPEN_COURSE_ID
                               AND R.USER_ID = U.USER_ID
                               --AND U.COMPANY_ID IN (SELECT COMPANY_ID FROM T_COMPANY WHERE COMPANY_KIND = '000001')
                               AND R.CONFIRM = '1'
                            ) 
                            || ' / ' || C.CLASS_MAN_COUNT AS CLASS_MAN_COUNT
                            ";
                }
                else
                {
                    xSql += @"
                          , (SELECT COUNT(R.USER_ID) FROM T_COURSE_RESULT R, T_USER U 
                             WHERE R.OPEN_COURSE_ID = O.OPEN_COURSE_ID
                               AND R.USER_ID = U.USER_ID
                               --AND U.COMPANY_ID IN (SELECT COMPANY_ID FROM T_USER WHERE USER_ID = '{0}') 
                               AND R.CONFIRM = '1'
                            ) 
                            || ' / ' || C.CLASS_MAN_COUNT AS CLASS_MAN_COUNT
                            ";
                }

                xSql += @"
                          , COUNT(*) OVER() TOTALRECORDCOUNT 
                        FROM T_OPEN_COURSE O, T_COURSE C
                        WHERE O.COURSE_ID = C.COURSE_ID 
                          AND O.COURSE_TYPE LIKE '%000002%' --사업주위탁 
                          AND O.USE_FLG = 'Y' 
                           ";
                if (rParams[2] != string.Empty)
                    xSql += " AND TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') >= '" + rParams[2] + "' ";
                if (rParams[3] != string.Empty)
                    xSql += " AND TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') <= '" + rParams[3] + "' ";
                if (rParams[4] != string.Empty)
                    xSql += " AND UPPER(C.COURSE_NM) LIKE '%" + rParams[4].Replace("'", "''").ToUpper() + "%' ";
                if (rParams[5] != string.Empty)
                    xSql += " AND C.COURSE_TYPE = '" + rParams[5] + "' ";

                xSql += @"   ORDER BY O.INS_DT desc, C.COURSE_NM " + "\r\n";
                xSql += @" ) O
                )
                ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                //xSql += " AND SUBSTR(CLASS_MAN_COUNT, 1, 1) > 0 "; // 수강신청 인원이 있는 과정만 표시 (신청인원/전체인원)
                xSql = string.Format(xSql, rParams[6]); 

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
      * Function name : GetReceivedListExcel
      * Purpose       : 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetReceivedListExcel(string[] rParams, CultureInfo rArgCultureInfo)
        public DataTable GetReceivedListExcel(string[] rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                string xSql = string.Empty;



                xSql = @" 
                        SELECT   
                          ROW_NUMBER() OVER(ORDER BY O.INS_DT DESC) AS NO 
                          , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE  ";
                if (rArgCultureInfo.Name.ToLower() == "ko - kr")
                {
                    xSql += "     , C.course_nm ";  // 과정명

                }
                else
                {
                    xSql += "     , C.COURSE_NM_ABBR as course_nm ";
                }
                xSql += @"
                          , O.COURSE_SEQ
                          , TO_CHAR(O.COURSE_BEGIN_APPLY_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(O.COURSE_END_APPLY_DT, 'YYYY.MM.DD') AS APPLY_DT
                          , TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_DT
                          , C.COURSE_DAY 
                        "; 

                          //, (SELECT COUNT(R.USER_ID) FROM T_COURSE_RESULT R, T_USER U 
                          //   WHERE R.OPEN_COURSE_ID = O.OPEN_COURSE_ID
                          //     AND R.USER_ID = U.USER_ID
                          //     AND U.COMPANY_ID = (SELECT COMPANY_ID FROM T_USER WHERE USER_ID = '{0}')
                          //  ) 
                          //  || ' / ' || C.CLASS_MAN_COUNT AS CLASS_MAN_COUNT

                if (rParams[7] == "000001")
                {
                    xSql += @" 
                          , (SELECT COUNT(R.USER_ID) FROM T_COURSE_RESULT R, T_USER U 
                             WHERE R.OPEN_COURSE_ID = O.OPEN_COURSE_ID
                               AND R.USER_ID = U.USER_ID
                               --AND U.COMPANY_ID IN (SELECT COMPANY_ID FROM T_COMPANY WHERE COMPANY_KIND = '000001')
                               AND R.CONFIRM = '1'
                            ) 
                            || ' / ' || C.CLASS_MAN_COUNT AS CLASS_MAN_COUNT
                            ";
                }
                else
                {
                    xSql += @"
                          , (SELECT COUNT(R.USER_ID) FROM T_COURSE_RESULT R, T_USER U 
                             WHERE R.OPEN_COURSE_ID = O.OPEN_COURSE_ID
                               AND R.USER_ID = U.USER_ID
                               --AND U.COMPANY_ID IN (SELECT COMPANY_ID FROM T_USER WHERE USER_ID = '{0}') 
                               AND R.CONFIRM = '1'
                            ) 
                            || ' / ' || C.CLASS_MAN_COUNT AS CLASS_MAN_COUNT
                            ";
                }

                xSql += @" 
                        FROM T_OPEN_COURSE O, T_COURSE C
                        WHERE O.COURSE_ID = C.COURSE_ID 
                          AND O.COURSE_TYPE LIKE  '%000002%' 
                          AND O.USE_FLG = 'Y' 
                           ";
                if (rParams[2] != string.Empty)
                    xSql += " AND TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') >= '" + rParams[2] + "' ";
                if (rParams[3] != string.Empty)
                    xSql += " AND TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') <= '" + rParams[3] + "' ";
                if (rParams[4] != string.Empty)
                    xSql += " AND UPPER(C.COURSE_NM) LIKE '%" + rParams[4].Replace("'", "''").ToUpper() + "%' ";
                if (rParams[5] != string.Empty)
                    xSql += " AND C.COURSE_TYPE = '" + rParams[5] + "' ";

                xSql = string.Format(xSql, rParams[6]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
      * Function name : GetReceivedCourse
      * Purpose       : 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetReceivedCourseInfo(string rParam)
        public DataTable GetReceivedCourseInfo(string rOpenCourseId, string rUser)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = this.PageSize.ToString(); // pagesize
                //xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                //xParams[2] = user_id


                xSql = @"                
                        SELECT 
                          O.OPEN_COURSE_ID 
                          , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE
                          , C.COURSE_NM 
                          , O.COURSE_SEQ
                          , TO_CHAR(O.COURSE_BEGIN_APPLY_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(O.COURSE_END_APPLY_DT, 'YYYY.MM.DD') AS APPLY_DT
                          , TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_DT
                          , C.COURSE_DAY 
                        FROM T_OPEN_COURSE O, T_COURSE C
                        WHERE O.COURSE_ID = C.COURSE_ID 
                          AND O.OPEN_COURSE_ID = '{0}' "; 
                           
                xSql = string.Format(xSql, rOpenCourseId); 

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
      * Function name : GetReceivedUserList
      * Purpose       : 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetReceivedUserList(string[] rParams)
        public DataTable GetReceivedUserList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = this.PageSize.ToString(); // pagesize
                //xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                //xParams[2] = open_course_id 

                //Session["USER_ID"].ToString(); 

                //그룹사직원일 경우.. 해당 그룹사에 속한 유저 정보 모두 보이게.. 
                //그룹사 직원이 아닐 경우 자기 COMPANY와 동일한 유저만 보이게.. 
                //if (rParams[4] == "000001")
                //{

                    xSql = @" 
                    SELECT * FROM 
                    (
                        SELECT ROWNUM AS RNUM, O.* FROM 
                        (
                            SELECT U.USER_NM_KOR
                              , REGEXP_REPLACE(HINDEV.CRYPTO_AES256.DEC_AES(U.PERSONAL_NO),'\d','*', 9) AS PERSONAL_NO
                              , (SELECT DUTY_WORK_NAME FROM V_HDUTYWORK WHERE DUTY_WORK = U.DUTY_WORK) AS DUTY_WORK
                              , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0062' AND D_CD = U.TRAINEE_CLASS) AS STATUS 
                              , U.INS_DT 
                              , O.OPEN_COURSE_ID 
                              , U.USER_ID 
                              , U.ENTER_DT
                              , U.MOBILE_PHONE
                              , COUNT(*) OVER() TOTALRECORDCOUNT 
                            FROM T_OPEN_COURSE O, T_COURSE_RESULT C, T_USER U
                            WHERE O.OPEN_COURSE_ID = C.OPEN_COURSE_ID 
                              AND C.USER_ID = U.USER_ID 
                              AND O.OPEN_COURSE_ID = '{0}'
                              AND C.CONFIRM = '1' ";

                //그룹사직원일 경우.. 해당 그룹사에 속한 유저 정보 모두 보이게.. 
                //그룹사 직원이 아닐 경우 자기 COMPANY와 동일한 유저만 보이게.. 
                if (rParams[4] == "000001")                    
                        xSql += @" AND U.COMPANY_ID IN (SELECT COMPANY_ID FROM T_COMPANY WHERE COMPANY_KIND = '000001') ";                     
                    else 
                        xSql += @" AND U.COMPANY_ID IN (SELECT COMPANY_ID FROM T_USER WHERE USER_ID = '{1}') "; 
                    xSql += @" ) O
                    )
                    ";

                    xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                    xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));


                    xSql = string.Format(xSql, rParams[2], rParams[3]);

//                }
//                else
//                {

//                    xSql = @" 
//                    SELECT * FROM 
//                    (
//                        SELECT ROWNUM AS RNUM, O.* FROM 
//                        (
//                            SELECT U.USER_NM_KOR
//                              , U.PERSONAL_NO
//                              , (SELECT DUTY_WORK_NAME FROM V_HDUTYWORK WHERE DUTY_WORK = U.DUTY_WORK) AS DUTY_WORK
//                              , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0062' AND D_CD = U.TRAINEE_CLASS) AS STATUS 
//                              , U.INS_DT 
//                              , O.OPEN_COURSE_ID 
//                              , U.USER_ID 
//                              , U.ENTER_DT
//                              , U.MOBILE_PHONE
//                              , COUNT(*) OVER() TOTALRECORDCOUNT 
//                            FROM T_OPEN_COURSE O, T_COURSE_RESULT C, T_USER U
//                            WHERE O.OPEN_COURSE_ID = C.OPEN_COURSE_ID 
//                              AND C.USER_ID = U.USER_ID 
//                              AND O.OPEN_COURSE_ID = '{0}' 
//                              AND U.COMPANY_ID = (SELECT COMPANY_ID FROM T_USER WHERE USER_ID = '{1}' )
//                               ";
//                    xSql += @" ) O
//                    )
//                    ";

//                    xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
//                    xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));


//                    xSql = string.Format(xSql, rParams[2], rParams[3]);
//                }

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 
        
        /************************************************************
      * Function name : GetReceivedUserInfoList
      * Purpose       : 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetReceivedUserInfoList(string[] rParams)
        public DataTable GetReceivedUserInfoList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = this.PageSize.ToString(); // pagesize
                //xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                //xParams[2] = open_course_id 

                xSql = @" 
                    SELECT * FROM 
                    (
                        SELECT ROWNUM AS RNUM, O.* FROM 
                        (
                            SELECT 
                                UD.USER_NM_KOR
                                , REGEXP_REPLACE(HINDEV.CRYPTO_AES256.DEC_AES(UD.PERSONAL_NO),'\d','*', 9) AS PERSONAL_NO
                                , (SELECT DUTY_WORK_NAME FROM V_HDUTYWORK WHERE DUTY_WORK = UD.DUTY_WORK) AS DUTY_WORK
                                , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0062' AND D_CD = UD.TRAINEE_CLASS) AS STATUS 
                                
                                , UD.USER_NM_ENG_FIRST || ' ' || UD.USER_NM_ENG_LAST AS USER_NM_ENG 
                                , UD.MOBILE_PHONE
                                , UD.EMAIL_ID
                                
                                , UD.INS_DT 
                                , UD.ENTER_DT
                                , UD.USER_ID 
                                , COUNT(*) OVER() TOTALRECORDCOUNT   
                            FROM T_USER UD
                        ";
                //그룹사직원일 경우.. 해당 그룹사에 속한 유저 정보 모두 보이게.. 
                //그룹사 직원이 아닐 경우 자기 COMPANY와 동일한 유저만 보이게.. 
                if (rParams[4] == "000001")
                    xSql += " WHERE UD.COMPANY_ID IN (SELECT COMPANY_ID FROM T_COMPANY WHERE COMPANY_KIND = '000001') ";
                else
                    xSql += " WHERE UD.COMPANY_ID IN (SELECT COMPANY_ID FROM T_USER WHERE USER_ID = '{0}') ";

                xSql += " AND UPPER(UD.USER_NM_KOR) LIKE '%{1}%' ";
                xSql += " ORDER BY NVL(UD.UPT_DT, UD.INS_DT) ";
                xSql += @" ) O
                    )
                    ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));


                xSql = string.Format(xSql, rParams[2], rParams[3].Replace("'", "''").ToUpper());


                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 


        /************************************************************
      * Function name : GetReceivedUserInfo
      * Purpose       : 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetReceivedUserInfo(string[] rParams)
        public DataTable GetReceivedUserInfo(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                xSql = @" 
                        SELECT 
                            U.USER_NM_KOR
                            , U.PERSONAL_NO
                            , (SELECT DUTY_WORK_NAME FROM V_HDUTYWORK WHERE DUTY_WORK = U.DUTY_WORK) AS DUTY_WORK
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0041' AND D_CD = U.STATUS) AS STATUS_NM 
                            , U.INS_DT 
                            , TO_CHAR(U.ENTER_DT, 'YYYY.MM.DD') AS ENTER_DT
                            , U.USER_ID 
                            , U.USER_NM_ENG_FIRST || ' ' || U.USER_NM_ENG_LAST AS USER_NM_ENG 
                            , U.MOBILE_PHONE
                            , U.EMAIL_ID
                            , U.OFFICE_PHONE 
                            , U.STATUS
                            , (SELECT C.INSURANCE_FLG FROM T_OPEN_COURSE O, T_COURSE_RESULT C 
                                WHERE O.OPEN_COURSE_ID = C.OPEN_COURSE_ID
                                  AND C.USER_ID = U.USER_ID
                                  AND O.OPEN_COURSE_ID = '{0}'
                              ) AS INSURANCE_FLG

                            , (SELECT c.employed_state FROM T_OPEN_COURSE O, T_COURSE_RESULT C 
                                WHERE O.OPEN_COURSE_ID = C.OPEN_COURSE_ID
                                  AND C.USER_ID = U.USER_ID
                                  AND O.OPEN_COURSE_ID = '{0}'
                              ) AS employed_state  

                        FROM T_USER U
                        WHERE U.USER_ID = '{1}'
                          ";

                xSql = string.Format(xSql, rParams[0], rParams[1]); 

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return null; 

        }
        #endregion 
        
        /************************************************************
      * Function name : SetReceivedCourseResult
      * Purpose       : Course Result 등록 
      * Input         : 
      * Output        : string
      *************************************************************/
        #region public string SetReceivedCourseResult(object[] rParams)
        public string SetReceivedCourseResult(object[] rParams)
        {
            Database db = null;
            string xRtn = string.Empty;
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS"); //Database 생성
                using (OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection()) // base.CreateConnection("LMS");
                {
                    xCnnLMS.Open();
                    OracleTransaction xTransLMS = xCnnLMS.BeginTransaction(); // 트랜잭션 시작
                    OracleCommand xCmdLMS = null;

                    try
                    {
                        xCmdLMS = base.GetSqlCommand(db);

                        OracleParameter[] xPara = null;
                        ArrayList xarrSMSUser = new ArrayList(); //sms 발송할 user 정보 담기 
                        DataSet dsUser = null; 

                        string[] xUserarr = rParams[2].ToString().Split('|');
                        for (int i = 0; i < xUserarr.Length; i++)
                        {
                            if (xUserarr[i] != string.Empty)
                            {
                                xSql = " SELECT USER_ID FROM T_COURSE_RESULT WHERE USER_ID = '" + xUserarr[i] + "' AND OPEN_COURSE_ID = '" + rParams[0] + "'  AND COURSE_RESULT_SEQ = 1 ";
                                xCmdLMS.CommandText = xSql;
                                object xobj = base.ExecuteScalar(db, xCmdLMS, xTransLMS);
                                if (xobj == null || xobj.ToString() == string.Empty)
                                { 
                                    //insert 
                                    xSql = @" INSERT INTO T_COURSE_RESULT 
                                                   ( USER_ID
                                                    , OPEN_COURSE_ID
                                                    , COURSE_RESULT_SEQ
                                                    , USER_COMPANY_ID
                                                    , USER_DEPT_CODE
                                                    , USER_DUTY_STEP
                                                    , APPROVAL_FLG
                                                    , APPROVAL_DT
                                                    , PASS_FLG 
                                                    , USER_COURSE_BEGIN_DT, USER_COURSE_END_DT 
                                                    , INSURANCE_FLG 
                                                    , INSURANCE_DT 
                                                    , INS_ID	,INS_DT	,UPT_ID	,UPT_DT
                                                    , CONFIRM
                                                   )                                                   
                                                    SELECT U.USER_ID
                                                        , O.OPEN_COURSE_ID
                                                        , 1
                                                        , U.COMPANY_ID
                                                        , U.DEPT_CODE
                                                        , U.DUTY_STEP
                                                        , '000003' /*APPROVAL_FLG    M_CD = '0019'*/
                                                        , SYSDATE
                                                        , '000004' /*PASS_FLG        M_CD = '0010'*/
                                                        , O.COURSE_BEGIN_DT
                                                        , O.COURSE_END_DT
                                                        , CASE WHEN U.ENTER_DT IS NOT NULL THEN 'Y' ELSE 'N' END INSURANCE_FLG 
                                                        , U.ENTER_DT
                                                        , :ADUSER_ID, SYSDATE, :ADUSER_ID, SYSDATE
                                                        , '1' /*확정*/
                                                    FROM T_USER U, T_OPEN_COURSE O 
                                                    WHERE O.OPEN_COURSE_ID = :OPEN_COURSE_ID
                                                      AND U.USER_ID = :USER_ID
                                                ";

                                    xPara = new OracleParameter[3];
                                    xPara[0] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, rParams[0]);
                                    xPara[1] = base.AddParam("ADUSER_ID", OracleType.VarChar, rParams[1]);
                                    xPara[2] = base.AddParam("USER_ID", OracleType.VarChar, xUserarr[i]);
                                    xCmdLMS.CommandText = xSql;
                                    base.Execute(db, xCmdLMS, xPara, xTransLMS);


                                    //sms 발송할 사용자 정보 담기 
                                    xSql = " SELECT USER_ID, USER_NM_KOR, MOBILE_PHONE FROM T_USER WHERE USER_ID = '" + xUserarr[i] + "' "; 
                                    xCmdLMS.CommandText = xSql;
                                    dsUser = base.ExecuteDataSet(db, xCmdLMS, xTransLMS);
                                    if (dsUser.Tables[0].Rows.Count > 0)
                                    {
                                        DataRow dr = dsUser.Tables[0].Rows[0]; 
                                        xarrSMSUser.Add(new string[] { dr["USER_ID"].ToString(), dr["USER_NM_KOR"].ToString(), dr["MOBILE_PHONE"].ToString()}); 
                                    }

                                    ////설문 target 추가 하는 부분 
                                    //vp_l_common_md lc = new vp_l_common_md();
                                    //lc.SetSurveyTarget(new string[]{rParams[0].ToString(), xUserarr[i]}, db, xCmdLMS, xTransLMS); 
                                    
                                }
                            }
                        }

                        //=============================================
                        /*SMS 문자는 데이터 저장 후 발송...*/
                        string[] xMasterParams = new string[9];
                        xMasterParams[0] = "지마린 운항훈련원";
                        xMasterParams[1] = string.Empty; // SMS 회신번호
                        xMasterParams[2] = rParams[3] + " [예약] " + rParams[4];  // SMS 발송내용
                        xMasterParams[3] = xarrSMSUser.Count.ToString(); //보낼사람 count 
                        xMasterParams[4] = string.Empty; // 과정코드
                        xMasterParams[5] = rParams[1].ToString();
                        xMasterParams[6] = "I"; // 지금 보낼 경우                    
                        //xMasterParams[8] = "10"; // 10: 이미지 없는 MMS 코드, 00:일반 문자 
                        if (Encoding.Default.GetByteCount(xMasterParams[2]) > 80)
                        {
                            xMasterParams[8] = "10"; // 10: 이미지 없는 MMS 코드, 00:일반 문자 
                        }
                        else
                        {
                            xMasterParams[8] = "00"; // 10: 이미지 없는 MMS 코드, 00:일반 문자 
                        }    

                        string[,] xDetailParams = new string[xarrSMSUser.Count, 3];
                        string[] xDetail = null; 
                        for (int i = 0; i < xarrSMSUser.Count; i++)
                        {
                            xDetail = (string[])xarrSMSUser[i];
                            xDetailParams[i, 0] = xDetail[0];  //수신자id
                            xDetailParams[i, 1] = xDetail[1];  //수신자 이름
                            xDetailParams[i, 2] = xDetail[2].Replace("-", ""); //수신자 전화번호
                        }                        

                        object[] xParamsO = new object[2];
                        xParamsO[0] = (object)xMasterParams;
                        xParamsO[1] = (object)xDetailParams;

                        vp_l_common_md sms = new vp_l_common_md();
                        sms.SetSentSMSBoxInsert(xParamsO);
                        //=============================================


                        xRtn = "Y";
                        xTransLMS.Commit(); //트렌잭션 커밋
                    }
                    catch (Exception ex)
                    {
                        // 트랜잭션 롤백
                        xTransLMS.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        if (xCmdLMS != null) xCmdLMS.Dispose();
                        if (xTransLMS != null) xTransLMS.Dispose();
                        if (xCnnLMS != null)
                        { if (xCnnLMS.State == ConnectionState.Open) xCnnLMS.Close(); }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db = null;
            }
            return xRtn;//리터값

        }
        #endregion 
        
        /************************************************************
      * Function name : SetReceivedCourseResult_Del
      * Purpose       : Course Result 등록 
      * Input         : 
      * Output        : string
      *************************************************************/
        #region public string SetReceivedCourseResult_Del(object[] rParams)
        public string SetReceivedCourseResult_Del(object[] rParams)
        {
            Database db = null;
            string xRtn = string.Empty;
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS"); //Database 생성
                using (OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection()) // base.CreateConnection("LMS");
                {
                    xCnnLMS.Open();
                    OracleTransaction xTransLMS = xCnnLMS.BeginTransaction(); // 트랜잭션 시작
                    OracleCommand xCmdLMS = null;

                    try
                    {
                        xCmdLMS = base.GetSqlCommand(db);

                        OracleParameter[] xPara = null;
                        ArrayList xarrSMSUser = new ArrayList(); //sms 발송할 user 정보 담기 
                        DataSet dsUser = null; 

                        string[] xUserarr = rParams[2].ToString().Split('|');
                        for (int i = 0; i < xUserarr.Length; i++)
                        {
                            if (xUserarr[i] != string.Empty)
                            {
                                xSql = " SELECT USER_ID FROM T_COURSE_RESULT WHERE USER_ID = '" + xUserarr[i] + "' AND OPEN_COURSE_ID = '" + rParams[0] + "'  AND COURSE_RESULT_SEQ = 1 ";
                                xCmdLMS.CommandText = xSql;
                                object xobj = base.ExecuteScalar(db, xCmdLMS, xTransLMS);
                                if (xobj != null && xobj.ToString() != string.Empty)
                                {
                                    //insert 
                                    xSql = @" DELETE FROM T_COURSE_RESULT 
                                                    WHERE OPEN_COURSE_ID = :OPEN_COURSE_ID
                                                        AND USER_ID = :USER_ID 
                                                        AND COURSE_RESULT_SEQ = 1                                                    
                                                ";

                                    xPara = new OracleParameter[2];
                                    xPara[0] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, rParams[0]);
                                    xPara[1] = base.AddParam("USER_ID", OracleType.VarChar, xUserarr[i]);
                                    xCmdLMS.CommandText = xSql;
                                    base.Execute(db, xCmdLMS, xPara, xTransLMS);


                                    //sms 발송할 사용자 정보 담기 
                                    xSql = " SELECT USER_ID, USER_NM_KOR, MOBILE_PHONE FROM T_USER WHERE USER_ID = '" + xUserarr[i] + "' ";
                                    xCmdLMS.CommandText = xSql;
                                    dsUser = base.ExecuteDataSet(db, xCmdLMS, xTransLMS);
                                    if (dsUser.Tables[0].Rows.Count > 0)
                                    {
                                        DataRow dr = dsUser.Tables[0].Rows[0];
                                        xarrSMSUser.Add(new string[] { dr["USER_ID"].ToString(), dr["USER_NM_KOR"].ToString(), dr["MOBILE_PHONE"].ToString() });
                                    }

                                    //설문 target 삭제 하는 부분 

                                }
                            }
                        }


                        //=============================================
                        /*SMS 문자는 데이터 저장 후 발송...*/
                        string[] xMasterParams = new string[9];
                        xMasterParams[0] = "한진해운 운항훈련원";
                        xMasterParams[1] = string.Empty; // SMS 회신번호
                        xMasterParams[2] = rParams[3] + " [예약취소] " + rParams[4];  // SMS 발송내용
                        xMasterParams[3] = xarrSMSUser.Count.ToString(); //보낼사람 count 
                        xMasterParams[4] = string.Empty; // 과정코드
                        xMasterParams[5] = rParams[1].ToString();
                        xMasterParams[6] = "I"; // 지금 보낼 경우                    

                        if (Encoding.Default.GetByteCount(xMasterParams[2]) > 80)
                        {
                            xMasterParams[8] = "10"; // 10: 이미지 없는 MMS 코드, 00:일반 문자 
                        }
                        else
                        {
                            xMasterParams[8] = "00"; // 10: 이미지 없는 MMS 코드, 00:일반 문자 
                        }                        

                        string[,] xDetailParams = new string[xarrSMSUser.Count, 3];
                        string[] xDetail = null;
                        for (int i = 0; i < xarrSMSUser.Count; i++)
                        {
                            xDetail = (string[])xarrSMSUser[i];
                            xDetailParams[i, 0] = xDetail[0];
                            xDetailParams[i, 1] = xDetail[1];
                            xDetailParams[i, 2] = xDetail[2].Replace("-", "");
                        }

                        object[] xParamsO = new object[2];
                        xParamsO[0] = (object)xMasterParams;
                        xParamsO[1] = (object)xDetailParams;

                        vp_l_common_md sms = new vp_l_common_md();
                        sms.SetSentSMSBoxInsert(xParamsO);
                        //=============================================

                        xRtn = "Y";
                        xTransLMS.Commit(); //트렌잭션 커밋
                    }
                    catch (Exception ex)
                    {
                        // 트랜잭션 롤백
                        xTransLMS.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        if (xCmdLMS != null) xCmdLMS.Dispose();
                        if (xTransLMS != null) xTransLMS.Dispose();
                        if (xCnnLMS != null)
                        { if (xCnnLMS.State == ConnectionState.Open) xCnnLMS.Close(); }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db = null;
            }
            return xRtn;//리터값

        }
        #endregion
        
        public string GetCompareRcvCount(string[] rParams)
        {
            string xRtn = Boolean.FalseString;
            try
            {
                DataTable xDt = null;
                string xSql = string.Empty;
                xSql += " SELECT TO_NUMBER(NVL(C.CLASS_MAN_COUNT, '0')) - (SELECT COUNT(DISTINCT R.USER_ID) FROM T_COURSE_RESULT R WHERE R.OPEN_COURSE_ID = O.OPEN_COURSE_ID AND R.CONFIRM = '1') AS ODD_MAN_COUNT ";
                xSql += "   FROM t_open_course O ";
                xSql += "  INNER JOIN t_course C ";
                xSql += "     ON O.course_id = C.course_id ";
                xSql += "  WHERE 1 = 1 ";
                xSql += string.Format("    AND O.open_course_id = '{0}'", rParams[0]); // 개설과정 ID
                xDt = base.ExecuteDataTable("LMS", xSql);

                if (Convert.ToInt16(xDt.Rows[0][0]) <= 0)
                    xRtn = Boolean.FalseString;
                else
                {
                    xRtn = Boolean.TrueString;
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
