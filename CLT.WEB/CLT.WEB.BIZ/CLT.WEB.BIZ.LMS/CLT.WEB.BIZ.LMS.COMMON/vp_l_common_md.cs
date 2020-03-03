using System;
using System.Collections.Generic;
using System.Text;

// 필수 using 문
using System.Data;
using System.Data.OracleClient;
using CLT.WEB.BIZ.FX.BIZBASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Globalization;

namespace CLT.WEB.BIZ.LMS.COMMON
{
    /// <summary>
    /// 1. 작업개요 : vp_l_common_md Class
    /// 
    /// 2. 주요기능 : 공통 기능(UI 등) 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_l_common_md
    /// 
    /// 4. 작 업 자 : 김양도 / 2011.12.08
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_l_common_md
    ///        * Comment 
    ///          * Comment 
    ///          Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제따른 변경    ///          
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *김은정 2012.08.08
    ///        * Source
    ///          vp_l_common_md.GetUserSessionInfo
    ///        * Comment 
    ///          Session 정보 추가 (공통사번)
    /// </summary>
    public class vp_l_common_md : DAC
    {
        /************************************************************
        * Function name : GetCommonCodeInfo
        * Purpose       : MasterCode 값에 따른 Detail 코드 전달하는 처리
        * Input         : string[] rParams (0: m_cd, 1:use_yn )
        * Output        : DataTable
        *************************************************************/
        public DataTable GetCommonCodeInfo(string[] rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                string xSql = null;
                xSql = "  SELECT ";
                xSql += "  d_cd, ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += "  d_knm ";
                }
                else
                {
                    xSql += "  d_enm AS d_knm ";
                }
                xSql += " FROM ";
                xSql += "  t_code_detail ";
                xSql += string.Format(" WHERE m_cd ='{0}' ", rParams[0]);

                if (rParams.GetLength(0) > 1)
                {
                    xSql += string.Format(" AND use_yn = '{0}' ", rParams[1]);
                }

                xSql += " ORDER BY sortorder, d_cd ";

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
        * Function name : GetCommonCodeInfo
        * Purpose       : MasterCode 값에 따른 Detail 코드 전달하는 처리
        * Input         : string[] rParams (0: m_cd, 1:use_yn )
        * Output        : DataTable
        *************************************************************/
        //public DataTable GetCommonCodeInfo(string[] rParams)
        //{
        //    try
        //    {
        //        string xSql = null;
        //        xSql = "  SELECT ";
        //        xSql += "  d_cd, ";
        //        //if (rArgCultureInfo.Name.ToLower() == "ko-kr")
        //        //{
        //            xSql += "  d_knm ";
        //        //}
        //        //else
        //        //{
        //        //    xSql += "  d_enm AS d_knm ";
        //        //}
        //        xSql += " FROM ";
        //        xSql += "  t_code_detail ";
        //        xSql += string.Format(" WHERE m_cd ='{0}' ", rParams[0]);

        //        if (rParams.GetLength(0) > 1)
        //        {
        //            xSql += string.Format(" AND use_yn = '{0}' ", rParams[1]);
        //        }

        //        xSql += " ORDER BY sortorder ";

        //        return base.ExecuteDataTable("LMS", xSql);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /************************************************************
        * Function name : GetVCommonVslType
        * Purpose       : SIMS에서 선종가져오기
        * Input         : string[] rParams (0:USE_FLG)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetVCommonVslType(string[] rParams)
        {
            try
            {
                string xSql = 
                     @"  SELECT TYPE_P_CD
                                , TYPE_P_DESC
                                , TYPE_P_SHORT_DESC
                                , SORT_CD
                                , USE_FLG ";
                xSql += "  FROM V_COMMON_VSL_TYPE_P ";
                xSql += " WHERE 1=1 " + "\r\n";

                if (rParams.Length >= 1)
                {
                    xSql += string.Format(" AND USE_FLG = '{0}' ", rParams[0]);
                }

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
        * Function name : GetVCommonVslTypeC
        * Purpose       : SIMS에서 선종가져오기
        * Input         : string[] rParams (0:USE_FLG)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetVCommonVslTypeC(string[] rParams)
        {
            try
            {
                string xSql =
                     @"  SELECT TYPE_P_CD,
                                TYPE_C_CD,
                                TYPE_C_DESC,
                                TYPE_C_SHORT_DESC,
                                SORT_CD_C,
                                USE_FLG,
                                TYPE_P_DESC,
                                TYPE_P_SHORT_DESC,
                                SORT_CD_P ";
                xSql += "  FROM V_COMMON_VSL_TYPE_C ";
                xSql += " WHERE 1=1 " + "\r\n";

                if (rParams.Length >= 1)
                {
                    xSql += string.Format(" AND USE_FLG = '{0}' ", rParams[0]);
                }
                if (rParams.Length >= 2)
                {
                    xSql += string.Format(" AND TYPE_P_CD = '{0}' ", rParams[1]);
                }

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
        * Function name : V_HDEPTCODE
        * Purpose       : V_HDEPTCODE 코드가져오기

        * Input         : string[] rParams (0: m_cd, 1:use_yn )
        * Output        : DataTable
        *************************************************************/
        public DataTable GetVHDeptCode(string[] rParams, string rOrderBy)
        {
            try
            {
                string xSql = " SELECT dept_ename1, dept_name, dept_code ";
                xSql += " FROM V_HDEPTCODE ";
                xSql += "WHERE dept_ename1 is not null ";

                if (!string.IsNullOrEmpty(rParams[0]))
                    xSql += string.Format(" AND ship_gu in ('{0}') ", rParams[0]);

                if (rParams.Length > 1)
                {
                    if (!string.IsNullOrEmpty(rParams[1]))
                        xSql += string.Format(" AND use_gu = '{0}' ", rParams[1]);
                }
                if (!string.IsNullOrEmpty(rOrderBy))
                    xSql += rOrderBy;

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
        * Function name : GetVHOrderdetKind
        * Purpose       : 가장 최근 발령값 가져오기
        * Input         : string[] rParams (0: user_id)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetVHOrderdetKind(string[] rParams)
        {
            try
            {
                string xSql = @" 
                        SELECT K.*
                          FROM (
                           SELECT SNO
                                , ORD_FDATE
                                , ORD_KIND
                                , ORD_NAME ";
                xSql += "  FROM V_HORDERDET_ORD_LATEST_KIND ";
                xSql += "     ) K ";
                xSql += " WHERE ROWNUM = 1 ";
                xSql += "   AND K.SNO = '" + rParams[0] + "' ";

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
       * Function name : GetMaxIDOfTable
       * Purpose       : 신규 항목을 입력하기 위한 테이블별 다음 입력 ID를 가져오는 처리
       * Input         : string[] rParams (0: tableName, 1: id(pk))
       * Output        : string
       *************************************************************/
        public string GetMaxIDOfTable(string[] rParams)
        {
            string xSql = "";
            object xTempRtn = ""; // 임시 변수


            string xRtnID = "-1"; // 테이블별 리턴 ID를 담는 변수



            try
            {
                // 형식 : 년(2자리) + 월(2자리) + seq (4자리)
                xSql = string.Format(" SELECT max({0}) + 1 FROM {1} ", rParams[1], rParams[0]);
                xSql += string.Format(" WHERE to_char(ins_dt, 'yyyyMM') = '{0}'", DateTime.Now.ToString("yyyyMM"));

                xTempRtn = base.ExecuteScalar("LMS", xSql);

                if (xTempRtn != null && Convert.ToString(xTempRtn) == "")
                    xRtnID = DateTime.Now.ToString("yyMM") + "0001";
                else
                    xRtnID = Convert.ToString(xTempRtn);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return xRtnID;
        }


        /************************************************************
        * Function name : GetMaxIDOfTable
        * Purpose       : 신규 항목을 입력하기 위한 테이블별 다음 입력 ID를 가져오는 처리
        * Input         : string[] rParams (0: tableName, 1: id(pk))
        * Output        : string
        *************************************************************/
        public string GetMaxIDOfTables(string[] rParams)
        {
            string xSql = "";
            object xTempRtn = string.Empty; // 임시 변수


            try
            {

                // 형식 : 년(2자리) + 월(2자리) +  일(2자리) + seq (2자리)
                xSql += string.Format(" SELECT NVL(MAX({0}+1),0)   FROM {1} ", rParams[0], rParams[1]);
                xSql += string.Format(" WHERE to_char(ins_dt,'YYYYMMDD') = '{0}' ", DateTime.Now.ToString("yyyyMMdd"));

                xTempRtn = base.ExecuteScalar("LMS", xSql);

                if (string.IsNullOrEmpty(xTempRtn.ToString())) // Null Or Empty 를 Return 할 경우 ID 강제생성
                {
                    xTempRtn = DateTime.Now.ToString("yyMMdd") + "01";
                }
                else
                {
                    if (xTempRtn.ToString() == "0")
                        xTempRtn = DateTime.Now.ToString("yyMMdd") + "0" + Convert.ToString(Convert.ToInt32(xTempRtn) + 1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return xTempRtn.ToString();
        }

        /************************************************************
        * Function name : GetMaxIDOfCode
        * Purpose       : 공통코드 ID 생성
        * Input         : string[] rParams (0: tableName, 1: id(pk), 2: where 조건)
        * Output        : string
        *************************************************************/
        public string GetMaxIDOfCode(string[] rParams)
        {
            string xSql = "";
            object xTempRtn = ""; // 임시 변수



            string xRtnID = "-1"; // 테이블별 리턴 ID를 담는 변수




            try
            {
                xSql = string.Format(" SELECT max({0}) + 1 FROM {1} ", rParams[1], rParams[0]);
                if (rParams[2].ToString() == "1")
                    xSql += string.Format(" WHERE m_cd = '{0}'", rParams[4]);
                //xSql += string.Format(" WHERE to_char(ins_dt, 'yyyyMM') = '{0}'", DateTime.Now.ToString("yyyyMM"));

                xTempRtn = base.ExecuteScalar("LMS", xSql);

                if (xTempRtn != null && Convert.ToString(xTempRtn) == "")
                {
                    if (rParams[3] == "4")
                        xRtnID = "0001";
                    else if (rParams[3] == "6")
                        xRtnID = "000001";
                }
                else
                    xRtnID = Convert.ToString(Convert.ToInt32(xTempRtn));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return xRtnID;
        }

        /************************************************************
        * Function name : GetUserSessionInfo
        * Purpose       : ID, Password에 해당하는 사용자 정보 제공
        * Input         : string[] rParams (0: user_id, 1: user_pwd)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetUserSessionInfo(string[] rParams)
        {
            try
            {
                
                //string xSql = " SELECT ";
                //xSql += " u.user_id, u.user_no, u.user_nm_kor, u.user_nm_eng_last || u.user_nm_eng_first as user_nm_eng, ";
                //xSql += " u.duty_step, s.step_name, s.step_ename, u.user_group, e.d_knm as user_group_knm, ";
                //xSql += " e.d_enm as user_group_enm, u.company_id, c.company_nm, c.company_nm_eng, ";
                //xSql += " u.email_id, u.duty_gu, u.dept_code, u.enter_dt, d.dept_name, d.dept_ename, u.status ";
                //xSql += " FROM t_user u ";
                //xSql += " INNER JOIN t_company c ";
                //xSql += " ON u.company_id = c.company_id ";
                //xSql += " INNER JOIN t_code_detail e ";
                //xSql += " ON u.user_group = e.d_cd ";
                //xSql += " INNER JOIN v_hdeptcode d ";
                //xSql += " ON u.dept_code = d.dept_code ";
                //xSql += " INNER JOIN v_hdutystep s ";
                //xSql += " ON u.duty_step = s.duty_step ";
                //xSql += string.Format(" WHERE e.m_cd='0041' AND u.user_id='{0}' AND u.pwd='{1}' ", rParams[0], rParams[1]);

                // 법인사 관리자의 경우 가입시 사용되는 직급(공통코드 : 0023)이 다르며 deptcode, dutystep 을 넣지 않으므로
                // 쿼리를 변경 2012-03-05 By KMK
                string xSql = " SELECT ";
                xSql += " u.user_id, u.user_no, u.user_nm_kor, u.user_nm_eng_last || u.user_nm_eng_first as user_nm_eng, ";
                xSql += " u.duty_step, s.step_name, s.step_ename, u.user_group, e.d_knm as user_group_knm, ";
                xSql += " e.d_enm as user_group_enm, u.company_id, c.company_nm, c.company_nm_eng, ";
                xSql += " u.email_id, u.duty_gu, u.dept_code, u.enter_dt, d.dept_name, d.dept_ename, u.status, u.duty_work, u.office_phone ";
                xSql += " , c.company_kind, u.com_sno "; //사업주위탁에서.. 그룹사 여부 필요 함 
                xSql += " FROM t_user u , t_company c, t_code_detail e, v_hdeptcode d, v_hdutystep s ";
                //xSql += string.Format(" WHERE e.m_cd='0041' AND u.user_id='{0}' AND u.pwd='{1}' ", rParams[0], rParams[1]);
                if (!string.IsNullOrEmpty(rParams[1]))
                    xSql += string.Format(" WHERE e.m_cd='0041' AND u.user_id='{0}' AND u.pwd='{1}' ", rParams[0], rParams[1]);
                else
                    xSql += string.Format(" WHERE e.m_cd='0041' AND u.user_id='{0}' ", rParams[0]);
                xSql += " AND u.company_id = c.company_id(+) ";
                xSql += " AND u.user_group = e.d_cd ";
                xSql += " AND u.dept_code = d.dept_code(+) ";
                xSql += " AND u.duty_step = s.duty_step(+) ";

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
        * Function name : GetDutystepCodeInfo
        * Purpose       : 신분구분코드용(사장, 부장, 과장 등...)
        * Input         : string[] rParams (0: m_cd, 1:use_yn)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetDutystepCodeInfo(string[] rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                //SELECT duty_step, step_name  FROM V_HDUTYSTEP WHERE USE_YN = 'Y';
                string xSql = "SELECT duty_step, ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += " step_name, ";
                }
                else
                {
                    xSql += " step_ename AS step_name, ";
                }
                xSql += " duty_work ";
                xSql += string.Format(" FROM V_HDUTYSTEP ");
                xSql += string.Format(" WHERE use_yn = '{0}' ", rParams[1]);
                //xSql += " ORDER BY duty_step ASC ";

                //사용중 지우지 마세요.
                if (rParams.Length >= 3)
                {
                    xSql += string.Format(" AND STEP_GUBUN = '{0}' ", rParams[2]);
                }

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
        * Function name : GetDutystepCodeInfo
        * Purpose       : 신분구분코드용(사장, 부장, 과장 등...)
        * Input         : string[] rParams (0: m_cd, 1:use_yn)
        * Output        : DataTable
        *************************************************************/
        //public DataTable GetDutystepCodeInfo(string[] rParams)
        //{
        //    try
        //    {
        //        //SELECT duty_step, step_name  FROM V_HDUTYSTEP WHERE USE_YN = 'Y';
        //        string xSql = "SELECT duty_step, ";
        //        //if (rArgCultureInfo.Name.ToLower() == "ko-kr")
        //        //{
        //            xSql += " step_name, ";
        //        //}
        //        //else
        //        //{
        //        //    xSql += " step_ename AS step_name, ";
        //        //}
        //        xSql += " duty_work ";
        //        xSql += string.Format(" FROM V_HDUTYSTEP ");
        //        xSql += string.Format(" WHERE use_yn = '{0}' ", rParams[1]);
        //        //xSql += " ORDER BY duty_step ASC ";

        //        //사용중 지우지 마세요.
        //        if (rParams.Length >= 3)
        //        {
        //            xSql += string.Format(" AND STEP_GUBUN = '{0}' ", rParams[2]);
        //        }

        //        return base.ExecuteDataTable("LMS", xSql);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /************************************************************
        * Function name : GetDutyWorkCodeInfo
        * Purpose       : 직책
        * Input         : string[] rParams (0:use_yn)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetDutyWorkCodeInfo(string[] rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                string xSql = null;
                xSql = " SELECT ";
                xSql += " duty_work, ";

                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += " duty_work_name ";
                }
                else
                {
                    xSql += " duty_work_ename AS duty_work_name ";
                }
                xSql += "FROM ";
                xSql += " v_hdutywork ";
                xSql += "WHERE ";
                xSql += " ship_gu = 'B20' ";

                if (rParams != null && rParams.Length > 0)
                {
                    xSql += " AND use_yn = '" + rParams[0] + "' ";
                }

                xSql += "  ORDER BY work_seq ";

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
        * Function name : GetDutyWorkCodeInfo
        * Purpose       : 직책
        * Input         : string[] rParams (0:use_yn)
        * Output        : DataTable
        *************************************************************/
        //public DataTable GetDutyWorkCodeInfo(string[] rParams)
        //{
        //    try
        //    {
        //        string xSql = null;
        //        xSql = " SELECT ";
        //        xSql += " duty_work, ";

        //        //if (rArgCultureInfo.Name.ToLower() == "ko-kr")
        //        //{
        //            xSql += " duty_work_name ";
        //        //}
        //        //else
        //        //{
        //        //    xSql += " duty_work_ename AS duty_work_name ";
        //        //}
        //        xSql += "FROM ";
        //        xSql += " v_hdutywork ";
        //        xSql += "WHERE ";
        //        xSql += " ship_gu = 'B20' ";

        //        if (rParams != null && rParams.Length > 0)
        //        {
        //            xSql += " AND use_yn = '" + rParams[0] + "' ";
        //        }

        //        xSql += "  ORDER BY work_seq ";

        //        return base.ExecuteDataTable("LMS", xSql);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #region public byte[] GetAttFile(string rKind, string rAttKey, string rFileName)
        /************************************************************
        *  Function name : GetAttFile
        *  Purpose       : 파일 가져오기
        *  Input         : 공용
        *  Output        : xReturn (첨부파일 내용)
        **************************************************************/
        public byte[] GetAttFile(string rKind, string rSeq, string rFseq)
        {
            byte[] xReturn;

            string xSql = "";
            OracleParameter[] oraParams = null;

            try
            {
                switch (rKind)
                {
                    case "shipschool":
                        xSql = @" SELECT attach_file
                                    FROM t_shipschool
                                   WHERE shipschool_cd = :SEQ ";
                        oraParams = new OracleParameter[1];
                        oraParams[0] = base.AddParam("SEQ", OracleType.VarChar, rSeq);
                        xReturn = (byte[])base.ExecuteScalar("LMS", xSql.ToString(), oraParams);
                        break;
                    default:
                        throw new Exception("Wrong Attatch Kind");
                }

                return xReturn;
            }
            catch (Exception ex)
            {
                throw ex;

                return null;
            }
        }
        #endregion







        /************************************************************
        * Function name : GetSMSID
        * Purpose       : SMS ID 생성
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public string GetSMSID(string[] rParams, OracleCommand rCmd)
        {
            string xSql = "";
            object xTemp = null;
            string xRtnID = string.Empty; // 테이블별 리턴 ID를 담는 변수


            try
            {
                xSql = string.Format(" SELECT seq_sms_id.NEXTVAL FROM DUAL ");
                //if (!string.IsNullOrEmpty(rParams[1]))
                //    xSql += string.Format(" WHERE {0} = {1} ", rParams[0], rParams[2]);

                rCmd.CommandText = xSql;
                xRtnID = Convert.ToString(rCmd.ExecuteScalar());
                //xTemp = base.ExecuteScalar("LMS", xSql);

                //xRtnID = xTemp.ToString();

                if (xRtnID == "0")
                    xRtnID = "1";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return xRtnID;
        }

        /************************************************************
       * Function name : GetSMSGroupID
       * Purpose       : SMS ID 생성
       * Input         : string[] rParams
       * Output        : string
       *************************************************************/
        public string GetSMSGroupID(string[] rParams)
        {
            string xSql = "";
            object xTemp = null;
            string xRtnID = string.Empty; // 테이블별 리턴 ID를 담는 변수


            try
            {
                xSql = string.Format(" SELECT seq_sms_group_id.NEXTVAL FROM DUAL ");

                xTemp = base.ExecuteScalar("LMS", xSql);

                xRtnID = xTemp.ToString();

                if (xRtnID == "0")
                    xRtnID = "1";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return xRtnID;
        }


        /************************************************************
        * Function name : GetUserDutyStep
        * Purpose       : SMS 전송관련 사용자 직급
        * Input         : string[] rParams
        * Output        : DataTable
        *************************************************************/
        public DataTable GetUserDutyStep(string rParams)
        {
            try
            {

                string xSql = "SELECT duty_step, company_id FROM t_user ";
                xSql += string.Format("WHERE user_id = '{0}' ", rParams);
                //for (int i = 0; i < rParams.Length; i++)
                //{
                //    if (i < rParams.Length)
                //        xSql += string.Format(" '{0}',  ", rParams[0]);
                //    else
                //        xSql += string.Format(" '{0}' ) ", rParams[0]);
                //}

                return base.ExecuteDataTable("LMS", xSql);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        /************************************************************
        * Function name : SetSentSMSBoxInsert
        * Purpose       : SMS 전송관련 테이블 Insert
        * Input         : string[] rParams
        * Output        : DataTable
        *************************************************************/
        public string SetSentSMSBoxInsert(params object[] rParams)
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

                string[] rMasterParams = (string[])rParams[0];
                string[,] rDetailParams = (string[,])rParams[1];


                string xSMSGroupCode = GetSMSGroupID(new string[] { "sms_group_no" });

                try
                {
                    int xCount = Convert.ToInt32(rMasterParams[3]);
                    for (int i = 0; i < xCount; i++)
                    {
                        string xSeq = GetSMSID(new string[] { "seq", xSMSGroupCode }, xCmdLMS);
                        DataTable xDt = null;
                        if (rDetailParams[i, 0] != "0")
                            xDt = GetUserDutyStep(rDetailParams[i, 0]);

                        string xSql = "INSERT INTO t_sent_sms_box ( ";
                        xSql += " seq, ";
                        xSql += " sms_group_no, ";
                        xSql += " sent_sms, ";
                        xSql += " sent_sms_title, ";
                        xSql += " rec_mobile_phone, ";
                        xSql += " rec_nm, ";
                        xSql += " open_course_id, ";
                        xSql += " rec_dt, ";
                        xSql += " sent_user_id, ";
                        xSql += " sent_mobile_phone, ";
                        xSql += " reserved_yn, ";
                        xSql += " reserved_dt, ";
                        //xSql += " sent_dt, ";
                        xSql += " rec_user_id, ";
                        xSql += " rec_duty_step, ";
                        xSql += " company_id) ";
                        xSql += " VALUES ( ";
                        xSql += string.Format(" {0}, ", Convert.ToInt32(xSeq));  // Seq
                        xSql += string.Format(" {0}, ", Convert.ToInt32(xSMSGroupCode)); // SMS 발송그룹
                        xSql += string.Format(" '{0}', ", rMasterParams[2]);  // SMS 발송 내용
                        xSql += string.Format(" '{0}', ", rMasterParams[0]);  // SMS 제목
                        xSql += string.Format(" '{0}', ", rDetailParams[i, 2]);  // 수신자 전화번호
                        xSql += string.Format(" '{0}', ", rDetailParams[i, 1]); // 수신자 이름
                        xSql += string.Format(" '{0}', ", rMasterParams[4]);  // 개설과정 코드
                        xSql += " SYSDATE, ";
                        xSql += string.Format(" '{0}', ", rMasterParams[5]);  // 발신자 ID
                        xSql += string.Format(" '{0}', ", rMasterParams[1]);  // 발신자 회신번호

                        if (rMasterParams[6] == "R")
                        {
                            xSql += string.Format(" '{0}', ", "Y");
                            xSql += string.Format(" TO_DATE({0},'YYYYMMDDHH24MISS'), ", rMasterParams[7]);
                        }
                        else
                        {
                            xSql += string.Format(" '{0}', ", "N");
                            xSql += " null, ";
                        }

                        if (rDetailParams[i, 0] == "0")
                        {
                            xSql += string.Format(" '{0}', ", " ");  // 수신자 ID (비회원일경우 Empty Value)
                            xSql += string.Format(" '{0}', ", " ");  // 수신자 직급코드
                            xSql += string.Format(" '{0}' ", " ");  // 수신자 회사 ID
                        }
                        else
                        {
                            xSql += string.Format(" '{0}', ", rDetailParams[i, 0]); // 수신자 ID
                            xSql += string.Format(" '{0}', ", xDt.Rows[0]["duty_step"].ToString());  // 수신자 직급코드
                            xSql += string.Format(" '{0}' ", xDt.Rows[0]["company_id"].ToString()); // 수신자 회사 ID
                        }
                        xSql += "        ) ";


                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);


                        // DBAgent SMS 발송
                        xSql = string.Empty;
                        xSql += " INSERT INTO tbl_submit_queue ";
                        xSql += " VALUES ( ";
                        xSql += string.Format(" '{0}', ", xSeq);  // 메시지 ID 기본키 (이력테이블로 이동될떄에도 PK)
                        xSql += string.Format(" '{0}', ", xSMSGroupCode); // 메시지 그룹 ID
                        xSql += string.Format(" '{0}', ", "1613");  // SK 브로드밴드 DBAgentID 
                        xSql += string.Format(" '{0}', ", "1");  // 전송위치 구분 : 1 기업
                        xSql += string.Format(" '{0}', ", rMasterParams[8].Trim());  // 메시지 구분코드
                        xSql += string.Format(" '{0}', ", rMasterParams[6].Trim()); // 예약발송 여부 I : 즉시발송, R : 예약발송
                        if (rMasterParams[6] == "I") // 즉시 발송일 경우
                            xSql += " TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS'), ";
                        else  // 예약발송일 경우
                            xSql += string.Format(" '{0}', ", rMasterParams[7].Trim());
                        xSql += " '1', "; // 서버에서의 메시지 저장 여부 0 : 저장하지 않음
                        xSql += string.Format(" '{0}', ", rDetailParams[i, 2].Trim());  // 수신자 전화번호 // 메시지 받는 사람의 전화번호
                        xSql += string.Format(" '{0}', ", rMasterParams[1].Trim());  // 발신자 회신번호
                        xSql += " '', "; // 국가코드 (국제 메시지 발송시에만 사용)
                        xSql += " '00', "; // 특정통신사 가입자 지정

                        xSql += string.Format(" '{0}', ", rMasterParams[2]);  // SMS 발송 내용
                        xSql += " '', ";  //  CallBack URL

                        if (rMasterParams[8] == "00")  // 첨부된 컨텐츠 개수 used_cd가 SMS(00,01,02) 일 경우 0, used_cd가 LMS(10,11) 일 경우 1
                        {
                            xSql += string.Format(" {0}, ", "0"); // 첨부된 컨텐츠 개수 (SMS)
                            xSql += " '', ";
                        }
                        else
                        {
                            xSql += string.Format(" {0}, ", "1"); // 첨부된 컨텐츠 개수 (MAIL)
                            xSql += " 'text/plain', ";
                        }

                        xSql += " '', "; // 첨부된 이미지 경로
                        xSql += " to_char(sysdate, 'YYYYMMDDHH24MISS'), ";// 법인업체에서 SK 브로드밴드로 전송한 시간
                        xSql += " '', "; // 법인업체에서 결과를 통보받은 시간
                        xSql += " '', "; // SK 브로드밴드가 통신사로 메시지를 보낸 시간
                        xSql += " '', "; // 휴대폰 가입자가 메시지를 받은 시간
                        xSql += " '', "; // WebAgent 에서 사용
                        xSql += " '0', "; // 메시지 처리 상태 0 : 전송할 메시지(0으로 설정된 메시지만 발송) 1: 메시지 전송요청 진행중인 상태, 2: 메시지 전송요청 완료상태(전송결과 수신대기), 9: 처리 완료상태(전송결과를 수신 하였거나 오류를 발생하여 전송 하지 못하고 종료된 상태)
                        xSql += " '', "; // 전송 결과값

                        xSql += string.Format(" '{0}', ", rMasterParams[0]); // 메시지 타이틀
                        xSql += " '', "; // 수신자 이통사 코드
                        xSql += " '', ";
                        xSql += " '', ";
                        xSql += " '', ";
                        xSql += " '', ";
                        xSql += " 0, ";
                        xSql += " 0) ";

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);
                    }

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



        /************************************************************
        * Function name : GetOpenCourseDate
        * Purpose       : 조회기간별 개설과정 리스트 조회
        * Input         : string[] rParams
        * Output        : DataTable
        *************************************************************/
        public DataTable GetOpenCourseDate(string[] rParams)
        {
            try
            {
                string xSql = "SELECT TO_CHAR(opencourse.course_begin_dt,'YYYY.MM.DD') || ' ~ ' || TO_CHAR(opencourse.course_end_dt, 'YYYY.MM.DD') course_date, ";
                xSql += " opencourse.open_course_id open_course_id ";
                xSql += " FROM t_open_course opencourse ";
                xSql += string.Format(" WHERE course_id = '{0}' ", rParams[0]);
                if (rParams.Length > 1)
                {
                    if (!string.IsNullOrEmpty(rParams[1]))
                        xSql += string.Format(" AND opencourse.course_begin_dt >= TO_DATE('{0}','YYYY.MM.DD')", rParams[1]); // 과정 개설기간 From
                    if (!string.IsNullOrEmpty(rParams[2]))
                        xSql += string.Format(" AND opencourse.course_begin_dt <= TO_DATE('{0}','YYYY.MM.DD')", rParams[2]); // 과정 개설기간 To
                }
               
                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
        * Function name : SetSurveyTarget
        * Purpose       : 설문조사 대상 생성
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public string SetSurveyTarget(string[] rParams, Database db, OracleCommand rCmd, OracleTransaction xTransLMS)
        {
            string xRtn = Boolean.TrueString;
            string xSql = string.Empty;
            string xUserID = string.Empty;
            string xResNo = string.Empty;

            try
            {
                xSql = string.Empty;
                xSql += " SELECT user_id FROM t_research_target ";
                xSql += " WHERE res_no =  ";
                xSql += "                ( ";
                xSql += "                 SELECT res_no FROM t_research ";
                xSql += string.Format("    WHERE open_course_id = '{0}' ", rParams[0]);
                xSql += "                ) ";
                xSql += string.Format(" AND user_id = '{0}' ", rParams[1]);

                rCmd.CommandText = xSql;
                xUserID = Convert.ToString(rCmd.ExecuteScalar());
                if (string.IsNullOrEmpty(xUserID))  // 설문조사 대상에 없을때 해당설문이 있는지 Check
                {
                    xSql = string.Empty;
                    xSql += " SELECT user_id FROM t_research_target ";
                    xSql += " WHERE res_no =  ";
                    xSql += "                ( ";
                    xSql += "                 SELECT res_no FROM t_research ";
                    xSql += string.Format("    WHERE open_course_id = '{0}' ", rParams[0]);
                    xSql += "                ) ";

                    rCmd.CommandText = xSql;
                    xResNo = Convert.ToString(rCmd.ExecuteScalar());
                }


                if (!string.IsNullOrEmpty(xResNo))  // 해당 설문이 있을때 INSERT
                {
                    xSql = string.Empty;
                    xSql = @" INSERT INTO t_research_target ( 
                                            res_no
                                            , user_id
                                            , answer_yn )
                                   VALUES (
                                            :res_no
                                            ,:user_id
                                            ,:answer_yn
                                          )";

                    OracleParameter[] xPara = new OracleParameter[3];
                    xPara[0] = base.AddParam("res_no", OracleType.VarChar, rParams[0]);
                    xPara[1] = base.AddParam("user_id", OracleType.VarChar, rParams[1]);
                    xPara[2] = base.AddParam("answer_yn", OracleType.VarChar, "N");

                    rCmd.CommandText = xSql;
                    base.Execute(db, rCmd, xPara, xTransLMS);
                }

            }
            catch (Exception ex)
            {
                xRtn = Boolean.FalseString;
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
                
            }

            return xRtn;
        }


        /************************************************************
        * Function name : GetUserGroup
        * Purpose       : Login 버튼 클릭시 USER_GROUP 을 체크하여 해상직원여부 체크
        * Input         : string[] rParams (0:user_id )
        * Output        : DataTable
        *************************************************************/
        public DataTable GetUserGroup(string[] rParams)
        {
            DataTable xDt = null;

            try
            {
                string xSql = string.Empty;
                xSql += " SELECT use_yn, status, user_group, pwd FROM t_user ";
                xSql += string.Format(" WHERE user_id = '{0}' ", rParams[0]);
                xDt = base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }
    }
}
