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

namespace CLT.WEB.BIZ.LMS.APPLICATION
{
    public class vp_s_agreement_md : DAC
    {

        /************************************************************
      * Function name : GetAgreementList
      * Purpose       : 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetAgreementList(string[] rParams)
        public DataTable GetAgreementList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;
             

                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT O.OPEN_COURSE_ID
                            , C.COURSE_NM
                            , O.COURSE_BEGIN_DT, O.COURSE_END_DT 
                            , TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_DT

                            --, R.USER_COURSE_BEGIN_DT, R.USER_COURSE_END_DT     
                            --, TO_CHAR(R.USER_COURSE_BEGIN_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(R.USER_COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_DT
                            , COUNT(*) OVER() TOTALRECORDCOUNT 
                            , COUNT(DISTINCT U.USER_ID) AS REG_COUNT 
                        FROM T_COURSE C, T_OPEN_COURSE O, T_COURSE_RESULT R , T_USER U
                        WHERE C.COURSE_ID = O.COURSE_ID
                          AND O.OPEN_COURSE_ID = R.OPEN_COURSE_ID
                          AND R.USER_ID = U.USER_ID 
                          AND U.COMPANY_ID = (SELECT COMPANY_ID FROM T_USER WHERE USER_ID = '{0}')  /*로그인한 사업자의 수강생들 정보..*/
                           ";
                if (rParams[2] != string.Empty)
                    xSql += " AND TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') >= '" + rParams[2] + "' ";
                if (rParams[3] != string.Empty)
                    xSql += " AND TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') <= '" + rParams[3] + "' ";
                if (rParams[4] != string.Empty)
                    xSql += " AND UPPER(C.COURSE_NM) LIKE '%" + rParams[4].Replace("'", "''").ToUpper() + "%' ";

                xSql += @" 
                        GROUP BY O.OPEN_COURSE_ID, C.COURSE_NM, O.COURSE_BEGIN_DT, O.COURSE_END_DT
                        ORDER BY O.COURSE_BEGIN_DT DESC
                    ) O
                )
                ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));

                xSql = string.Format(xSql, rParams[5]); 

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
        * Function name : GetAgreementReport
        * Purpose       : 
        * Input         : string[] rParams ()
        * Output        : DataSet
        *************************************************************/
        #region public DataSet GetAgreementReport((string[] rParams)
        public DataSet GetAgreementReport(string[] rParams)
        {
            DataSet xDs = new DataSet();
            try
            {
                // 0 : company_id
                // 1 : open_course_id
                string xSql = null;

                // Company Information
                xSql = " SELECT company_nm, company_ceo FROM t_company WHERE company_id = '" + rParams[0] + "' ";
                //xDs = base.ExecuteDataSet("LMS", xSql);
                base.MergeTable(ref xDs, base.ExecuteDataTable("LMS", xSql), "t_company");

                // Course Result Information
                xSql = " SELECT ";
                xSql += " b.user_nm_kor, ";
                xSql += " b.personal_no, ";
                xSql += " c.empoly_ins_no, ";
                xSql += " d.duty_work_ename, ";
                xSql += " TO_CHAR(b.enter_dt, 'yyyy.MM.dd') as enter_dt, ";
                xSql += " TO_CHAR(a.insurance_dt, 'yyyy.MM.dd') as insurance_dt, ";
                xSql += " e.d_knm ";
                xSql += "FROM ";
                xSql += " t_course_result a, ";
                xSql += " t_user b, ";
                xSql += " t_company c, ";
                xSql += " v_hdutywork d, ";
                xSql += " t_code_detail e ";
                xSql += "WHERE ";
                xSql += " a.user_id = b.user_id ";
                xSql += " AND b.company_id = c.company_id ";
                xSql += " AND b.duty_work = d.duty_work(+) ";
                xSql += " AND b.status = e.d_cd ";
                xSql += " AND e.m_cd = '0002' ";
                xSql += " AND a.open_course_id = '" + rParams[1] + "' ";

                if (rParams[2] == "000001")
                    xSql += " AND a.user_company_id IN (SELECT COMPANY_ID FROM T_COMPANY WHERE COMPANY_KIND = '000001') ";
                else
                    xSql += " AND a.user_company_id = '" + rParams[0] + "' ";

                //if (rParams[2].ToUpper() != "ADMIN")
                //    xSql += " AND a.user_company_id in ('12030012', '12040001') ";
                //else
                //    xSql += " AND a.user_company_id = '" + rParams[0] + "' ";
                base.MergeTable(ref xDs, base.ExecuteDataTable("LMS", xSql), "t_course_result");

                // Course Information
                xSql = " SELECT ";
                xSql += " b.course_nm, ";
                xSql += " b.course_day || 'D ' || b.course_time || 'H' AS course_day_time, ";
                xSql += " b.class_man_count, ";
                xSql += " TO_CHAR(a.course_begin_dt, 'yyyy.MM.dd') AS course_begin_dt, ";
                xSql += " TO_CHAR(a.course_end_dt, 'yyyy.MM.dd') AS course_end_dt, ";
                xSql += " a.training_fee ";
                xSql += "FROM ";
                xSql += " t_open_course a, ";
                xSql += " t_course b ";
                xSql += "WHERE ";
                xSql += " a.open_course_id = '" + rParams[1] + "' ";
                xSql += " AND a.course_id = b.course_id ";
                base.MergeTable(ref xDs, base.ExecuteDataTable("LMS", xSql), "t_course");

                // textbook
                xSql = " SELECT ";
                xSql += " b.textbook_nm ";
                xSql += "FROM ";
                xSql += " t_course_textbook a, ";
                xSql += " t_textbook b, ";
                xSql += " t_open_course c, ";
                xSql += " t_course d ";
                xSql += "WHERE ";
                xSql += " c.open_course_id = '" + rParams[1] + "' ";
                xSql += " AND c.course_id = d.course_id ";
                xSql += " AND d.course_id = a.course_id ";
                xSql += " AND a.textbook_id = b.textbook_id ";
                base.MergeTable(ref xDs, base.ExecuteDataTable("LMS", xSql), "t_textbook");

                // 훈련교육생 Count 
                //xSql = " SELECT ";
                //xSql += " count(user_id) as user_cnt ";
                //xSql += "FROM ";
                //xSql += " t_course_result ";
                //xSql += "WHERE ";
                //xSql += " open_course_id = '" + rParams[1] + "' ";
                //xSql += " Group by open_course_id ";
                //base.MergeTable(ref xDs, base.ExecuteDataTable("LMS", xSql), "t_course_result_cnt");

                xSql = " SELECT ";
                xSql += "count(a.user_id) as user_cnt ";
                xSql += "FROM ";
                xSql += " t_course_result a, ";
                xSql += " t_user b, ";
                xSql += " t_company c, ";
               // xSql += " v_hdutywork d, ";
                xSql += " t_code_detail e ";
                xSql += "WHERE ";
                xSql += " a.user_id = b.user_id ";
                xSql += " AND b.company_id = c.company_id ";
                //xSql += " AND b.duty_work = d.duty_work(+) ";
                xSql += " AND b.status = e.d_cd ";
                xSql += " AND e.m_cd = '0002' ";
                xSql += " AND a.open_course_id = '" + rParams[1] + "' ";

                if (rParams[2] == "000001")
                    xSql += " AND a.user_company_id IN (SELECT COMPANY_ID FROM T_COMPANY WHERE COMPANY_KIND = '000001') ";
                else
                    xSql += " AND a.user_company_id = '" + rParams[0] + "' ";

                base.MergeTable(ref xDs, base.ExecuteDataTable("LMS", xSql), "t_course_result_cnt");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return xDs;
        }
        #endregion 

        /************************************************************
        * Function name : GetAgreementContractReport
        * Purpose       : 
        * Input         : string[] rParams ()
        * Output        : DataSet
        *************************************************************/
        #region public DataSet GetAgreementContractReport((string[] rParams)
        public DataSet GetAgreementContractReport(string[] rParams)
        {
            DataSet xDs = new DataSet();
            try
            {
                // 0 : company_id
                // 1 : open_course_id
                string xSql = null;

                // Company Information
                xSql = " SELECT company_nm, company_ceo, company_addr FROM t_company WHERE company_id = '" + rParams[0] + "' ";
                //xDs = base.ExecuteDataSet("LMS", xSql);
                base.MergeTable(ref xDs, base.ExecuteDataTable("LMS", xSql), "t_company");

                // Course Result Information
                xSql = " SELECT ";
                xSql += " rownum AS row_seq, ";
                xSql += " b.user_nm_kor, ";
                xSql += " b.personal_no, ";
                xSql += " REPLACE(b.user_addr, '|', '') AS user_addr, ";
                xSql += " b.mobile_phone ";
                xSql += "FROM ";
                xSql += " t_course_result a, ";
                xSql += " t_user b, ";
                xSql += " t_code_detail e ";
                xSql += "WHERE ";
                xSql += " a.user_id = b.user_id ";

                //xSql += " AND b.status = e.d_cd ";
                //xSql += " AND e.m_cd = '0002' "; // 재직자/채용예정자
                //xSql += " AND e.d_cd = '000002' "; // 채용예정자

                xSql += " AND b.trainee_class = e.d_cd ";
                xSql += " AND e.m_cd = '0062' "; //훈련생 구분 
                xSql += " AND e.d_cd = '000002' "; //채용예정자 

                xSql += " AND a.open_course_id = '" + rParams[1] + "' ";
                //xSql += " AND a.user_company_id = '" + rParams[0] + "' ";

                if (rParams[2] == "000001")
                    xSql += " AND a.user_company_id IN (SELECT COMPANY_ID FROM T_COMPANY WHERE COMPANY_KIND = '000001') ";
                else
                    xSql += " AND a.user_company_id = '" + rParams[0] + "' ";

                base.MergeTable(ref xDs, base.ExecuteDataTable("LMS", xSql), "t_course_result");

                // Course Information
                xSql = " SELECT ";
                xSql += " b.course_nm, ";
                xSql += " TO_CHAR(a.course_begin_dt, 'yyyy.MM.dd') AS course_begin_dt, ";
                xSql += " TO_CHAR(a.course_end_dt, 'yyyy.MM.dd') AS course_end_dt ";
                xSql += "FROM ";
                xSql += " t_open_course a, ";
                xSql += " t_course b ";
                xSql += "WHERE ";
                xSql += " a.open_course_id = '" + rParams[1] + "' ";
                xSql += " AND a.course_id = b.course_id ";
                base.MergeTable(ref xDs, base.ExecuteDataTable("LMS", xSql), "t_course");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return xDs;
        }
        #endregion 

    }
}
