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

namespace CLT.WEB.BIZ.LMS.APPLICATION
{
    public class vp_s_record_md: DAC
    {


        /************************************************************
     * Function name : GetRecordtList
     * Purpose       : 
     * Input         : string[] rParams (0: pagesize, 1: pageno)
     * Output        : DataTable
     *************************************************************/
        #region public DataTable GetRecordtList(string[] rParams)
        public DataTable GetRecordtList(string[] rParams)
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

                            , COUNT(R.USER_ID) AS CNT
                            , COUNT(*) OVER() TOTALRECORDCOUNT 
                        FROM T_COURSE C, T_OPEN_COURSE O, T_COURSE_RESULT R, T_USER U
                        WHERE C.COURSE_ID = O.COURSE_ID
                          AND O.OPEN_COURSE_ID = R.OPEN_COURSE_ID
                          AND R.PASS_FLG = '000001'  /*이수 */
                          AND R.USER_ID = U.USER_ID ";

                if (rParams[6] == "000001")
                    xSql += " AND U.COMPANY_ID IN (SELECT COMPANY_ID FROM T_COMPANY WHERE COMPANY_KIND = '000001') ";
                else
                    xSql += " AND U.COMPANY_ID = (SELECT COMPANY_ID FROM T_USER WHERE USER_ID = '" + rParams[5] + "') ";

                //if (rParams[6].ToUpper() != "ADMIN")
                //    xSql += "AND U.COMPANY_ID = (SELECT COMPANY_ID FROM T_USER WHERE USER_ID = '{0}') ";  /*로그인한 사업자의 수강생들 정보..*/
                //else
                //    xSql += "AND U.COMPANY_ID IN ('12030012', '12040001') ";
           
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

                xSql = string.Format(xSql);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 


        /************************************************************
        * Function name : GetAgreementListReport
        * Purpose       : 
        * Input         : string[] rParams ()
        * Output        : DataSet
        *************************************************************/
        #region public DataSet GetAgreementListReport((string[] rParams)
        public DataSet GetAgreementListReport(string[] rParams)
        {
            DataSet xDs = new DataSet();
            try
            {
                // 0 : open_course_id
                string xSql = null;
                string xStrWhere = "(";
                foreach (string xTmp in rParams)
                {
                    if (xTmp != string.Empty)
                    {
                        xStrWhere += "'" + xTmp + "', ";
                    }
                }
                xStrWhere = xStrWhere.Substring(0, xStrWhere.LastIndexOf(','));
                xStrWhere += ")";

                // Course Result Information
                xSql = " SELECT ";
                xSql += " rownum AS row_seq, ";
                xSql += " c.company_nm, ";
                xSql += " d.duty_work_ename, ";
                xSql += " b.user_nm_kor, ";
                xSql += " b.personal_no, ";
                xSql += " f.course_nm, ";
                xSql += " TO_CHAR(e.course_begin_dt, 'yyyy.MM.dd') || '~' || TO_CHAR(e.course_end_dt, 'yyyy.MM.dd') AS course_begin_end_dt, ";
                xSql += " TRIM(REPLACE(TO_CHAR(e.training_fee, '9,999,999,999'), ' ', '')) AS training_fee, ";
                xSql += " TRIM(REPLACE(TO_CHAR(e.training_fee * 0.1, '9,999,999,999'), ' ', '')) AS training_fee_tax, ";
                xSql += " TRIM(REPLACE(TO_CHAR(e.training_fee + (e.training_fee * 0.1), '9,999,999,999'), ' ', '')) AS training_fee_sum, ";
                xSql += " CASE c.company_scale ";
                xSql += "  WHEN '000001' THEN TRIM(REPLACE(TO_CHAR(e.training_support_comp_fee, '9,999,999,999'), ' ', '')) ";
                xSql += "  WHEN '000002' THEN TRIM(REPLACE(TO_CHAR(e.training_support_fee, '9,999,999,999'), ' ', '')) ";
                xSql += " END AS company_scale_fee ";
                xSql += "FROM ";
                xSql += " t_course_result a, ";
                xSql += " t_user b, ";
                xSql += " t_company c, ";
                xSql += " v_hdutywork d, ";
                xSql += " t_open_course e, ";
                xSql += " t_course f ";
                xSql += "WHERE ";
                xSql += " a.user_id = b.user_id ";
                xSql += " AND b.company_id = c.company_id ";
                xSql += " AND b.duty_work = d.duty_work(+) ";
                xSql += " AND a.open_course_id = e.open_course_id ";
                xSql += " AND e.course_id = f.course_id ";
                xSql += " AND a.pass_flg = '000001' "; // 교육이수구분 > 수료
                xSql += " AND a.open_course_id IN " + xStrWhere;
                base.MergeTable(ref xDs, base.ExecuteDataTable("LMS", xSql), "t_course_result");

                // Course Result Sum Information
                xSql = " SELECT ";
                xSql += " TRIM(REPLACE(TO_CHAR(SUM(e.training_fee), '9,999,999,999'), ' ', '')) AS training_fee_sum, ";
                xSql += " TRIM(REPLACE(TO_CHAR(SUM(e.training_fee * 0.1), '9,999,999,999'), ' ', '')) AS training_fee_tax_sum, ";
                xSql += " TRIM(REPLACE(TO_CHAR(SUM(e.training_fee + (e.training_fee * 0.1)), '9,999,999,999'), ' ', '')) AS training_fee_total, ";
                xSql += " TRIM(REPLACE(TO_CHAR(SUM( ";
                xSql += "                           CASE c.company_scale";
                xSql += "                             WHEN '000001' THEN e.training_support_comp_fee ";
                xSql += "                             WHEN '000002' THEN e.training_support_fee ";
                xSql += "                           END) ";
                xSql += " , '9,999,999,999'), ' ', '')) AS company_scale_fee_sum ";
                xSql += "FROM ";
                xSql += " t_course_result a, ";
                xSql += " t_user b, ";
                xSql += " t_company c, ";
                xSql += " v_hdutywork d, ";
                xSql += " t_open_course e, ";
                xSql += " t_course f ";
                xSql += "WHERE ";
                xSql += " a.user_id = b.user_id ";
                xSql += " AND b.company_id = c.company_id ";
                xSql += " AND b.duty_work = d.duty_work(+) ";
                xSql += " AND a.open_course_id = e.open_course_id ";
                xSql += " AND e.course_id = f.course_id ";
                xSql += " AND a.pass_flg = '000001' "; // 교육이수구분 > 수료
                xSql += " AND a.open_course_id IN " + xStrWhere;
                base.MergeTable(ref xDs, base.ExecuteDataTable("LMS", xSql), "t_course_result_sum");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return xDs;
        }
        #endregion 

        /************************************************************
        * Function name : GetAgreementCertificateReport
        * Purpose       : 
        * Input         : string[] rParams ()
        * Output        : DataTable
        *************************************************************/
        #region public DataSet GetAgreementCertificateReport(string[] rParams)
        public DataTable GetAgreementCertificateReport(string[] rParams, string rImgPath)
        {
            DataTable xDt = new DataTable();
            try
            {
                // 0 : open_course_id
                string xSql = null;
                string xStrWhere = "(";
                foreach (string xTmp in rParams)
                {
                    if (xTmp != string.Empty)
                    {
                        xStrWhere += "'" + xTmp + "', ";
                    }
                }
                xStrWhere = xStrWhere.Substring(0, xStrWhere.LastIndexOf(','));
                xStrWhere += ")";

                // Course Result Information
                xSql = " SELECT ";
                xSql += " c.company_nm, ";
                xSql += " b.user_nm_kor, ";
                xSql += " HINDEV.CRYPTO_AES256.DEC_AES(b.personal_no) as personal_no, ";
                xSql += " f.course_nm, ";
                xSql += " TO_CHAR(e.course_begin_dt, 'yyyy.MM.dd') || '~' || TO_CHAR(e.course_end_dt, 'yyyy.MM.dd') AS course_begin_end_dt, ";
                xSql += " TO_CHAR(sysdate, 'yyyy') || '년 ' || TO_CHAR(sysdate, 'mm') || '월 ' || TO_CHAR(sysdate, 'dd') || '일' AS agree_datetime, ";
                xSql += " '" + rImgPath + "' AS logo1 ";
                xSql += "FROM ";
                xSql += " t_course_result a, ";
                xSql += " t_user b, ";
                xSql += " t_company c, ";
                xSql += " t_open_course e, ";
                xSql += " t_course f ";
                xSql += "WHERE ";
                xSql += " a.user_id = b.user_id ";
                xSql += " AND b.company_id = c.company_id ";
                xSql += " AND a.open_course_id = e.open_course_id ";
                xSql += " AND e.course_id = f.course_id ";
                xSql += " AND a.pass_flg = '000001' "; // 교육이수구분 > 수료
                xSql += " AND a.open_course_id IN " + xStrWhere;

                xDt = base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return xDt;
        }
        #endregion 
    }
}
