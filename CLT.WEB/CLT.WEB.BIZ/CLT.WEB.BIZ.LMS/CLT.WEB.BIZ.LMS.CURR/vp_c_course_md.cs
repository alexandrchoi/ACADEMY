using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
// 필수 using 문
using System.IO;
using System.Data;
using System.Data.OracleClient;
using CLT.WEB.BIZ.FX.BIZBASE;
using System.Data.Common;
using CLT.WEB.BIZ.LMS.COMMON;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.BIZ.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : vp_c_course_md Class
    /// 
    /// 2. 주요기능 : 과정(등록,개설) 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_c_course_md
    /// 
    /// 4. 작 업 자 : 김양도 / 2011.12.08
    /// </summary>
    public class vp_c_course_md : DAC
    {
        /************************************************************
        * Function name : GetCourseInfo
        * Purpose       : 등록과정 전체 목록을 전달하는 처리
        * Input         : string[] rParams (0: pagesize, 1: currentPageIndex)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetCourseInfo(string[] rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                string xSql = " SELECT * FROM ";
                xSql += "           ( SELECT rownum rnum, b.* FROM  ";
                xSql += "               ( ";
                xSql += "                   SELECT t.course_id,  t.vsl_type, t.expired_period, t.course_objective, T.ins_dt ";
                //xSql += "                   c1.d_knm as course_type, c2.d_knm as course_group, ";
               // xSql += "                   c3.d_knm as course_field, c4.d_knm as course_lang, "; //c5.d_knm as course_inout, "; 
             
                                                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                                                {
                                                    xSql += ", t.course_nm ";  // t.course_nm
                                                    xSql += ", c1.d_knm as course_type ";
                                                    xSql += ", c2.d_knm as course_group ";
                                                    xSql += ", c3.d_knm as course_field ";
                                                    xSql += ", c4.d_knm as course_lang ";
                                                }
                                                else
                                                {
                                                    xSql += ", t.course_nm_abbr as course_nm ";
                                                    xSql += ", c1.d_enm as course_type ";
                                                    xSql += ", c2.d_enm as course_group ";
                                                    xSql += ", c3.d_enm as course_field ";
                                                    xSql += ", c4.d_enm as course_lang ";
                                                }
                xSql += "                       , count(*) over() totalrecordcount ";
                xSql += "                       FROM t_course t ";
                xSql += "                       INNER JOIN t_code_detail c1 "; // course_type(master_code) : 0006
                xSql += "                       ON t.course_type = c1.d_cd ";
                xSql += "                       INNER JOIN t_code_detail c2 "; // course_group(master_code) : 0003
                xSql += "                       ON t.course_group = c2.d_cd ";
                xSql += "                       INNER JOIN t_code_detail c3 "; // course_field(master_code) : 0004
                xSql += "                       ON t.course_field = c3.d_cd ";
                xSql += "                       INNER JOIN t_code_detail c4 "; // course_lang(master_code) : 0017
                xSql += "                       ON t.course_lang = c4.d_cd ";
                //xSql += "                       INNER JOIN t_code_detail c5 "; // course_inout(master_code) : 0005
                //xSql += "                       ON t.course_inout = c5.d_cd ";
                xSql += "                       WHERE c1.m_cd='0006' AND c2.m_cd='0003' AND c3.m_cd='0004' AND c4.m_cd='0017' "; // AND c5.m_cd='0005' ";
                xSql += "                          AND (T.TEMP_SAVE_FLG = 'N' OR T.TEMP_SAVE_FLG IS NULL) ";

                xSql += "                          AND T.COURSE_TYPE <> '000005'  /* COURSE에서 COURSE_TYPE이 OJT일 경우 제외하여 DISPLAY*/ ";
                xSql += "                          AND T.USE_FLG = 'Y' ";   // 사용 유무 'Y' 인 과정만 
                xSql += "                       ORDER BY t.ins_dt DESC ";
                xSql += "                ) b ";
                xSql += "          ) ";
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
        * Function name : GetCourseInfoByCondition
        * Purpose       : 조회조건에 따른 등록과정 전체 목록을 전달하는 처리
        * Input         : string[] rParams (0: pagesize, 1: currentPageIndex, 2: course_nm, 3: course_lang, 4: course_type)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetCourseInfoByCondition(string[] rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                string xSql = " SELECT * FROM ";
                xSql += "           ( SELECT rownum rnum, b.* FROM  ";
                xSql += "               ( ";
                xSql += "                   SELECT t.course_id, t.vsl_type, t.expired_period, t.course_objective, T.ins_dt ";
                
                //xSql += "                   c1.d_knm as course_type, c2.d_knm as course_group, ";
                //xSql += "                   c3.d_knm as course_field, c4.d_knm as course_lang, "; //c5.d_knm as course_inout, "; 
                                            if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                                            {
                                                xSql += ", t.course_nm ";  // t.course_nm
                                                xSql += ", c1.d_knm as course_type ";
                                                xSql += ", c2.d_knm as course_group ";
                                                xSql += ", c3.d_knm as course_field ";
                                                xSql += ", c4.d_knm as course_lang ";
                                            }
                                            else
                                            {
                                                xSql += ", t.course_nm_abbr as course_nm ";
                                                xSql += ", c1.d_enm as course_type ";
                                                xSql += ", c2.d_enm as course_group ";
                                                xSql += ", c3.d_enm as course_field ";
                                                xSql += ", c4.d_enm as course_lang ";
                                            }

                xSql += "                   , count(*) over() totalrecordcount ";
                xSql += "                       FROM t_course t ";
                xSql += "                       INNER JOIN t_code_detail c1 "; // course_type(master_code) : 0006
                xSql += "                       ON t.course_type = c1.d_cd ";
                xSql += "                       INNER JOIN t_code_detail c2 "; // course_group(master_code) : 0003
                xSql += "                       ON t.course_group = c2.d_cd ";
                xSql += "                       INNER JOIN t_code_detail c3 "; // course_field(master_code) : 0004
                xSql += "                       ON t.course_field = c3.d_cd ";
                xSql += "                       INNER JOIN t_code_detail c4 "; // course_lang(master_code) : 0017
                xSql += "                       ON t.course_lang = c4.d_cd ";
                //xSql += "                       INNER JOIN t_code_detail c5 "; // course_inout(master_code) : 0005
                //xSql += "                       ON t.course_inout = c5.d_cd ";

                // 조회조건 추가 <START>
                xSql += "                       WHERE c1.m_cd='0006' AND c2.m_cd='0003' AND c3.m_cd='0004' AND c4.m_cd='0017' "; //AND c5.m_cd='0005' ";
                xSql += "                          AND (T.TEMP_SAVE_FLG = 'N' OR T.TEMP_SAVE_FLG IS NULL) ";
                xSql += "                          AND T.USE_FLG = 'Y' ";   // 사용 유무 'Y' 인 과정만 

                if (rParams[5] != "000005")
                    xSql += "                          AND T.COURSE_TYPE <> '000005'  /* COURSE에서 COURSE_TYPE이 OJT일 경우 제외하여 DISPLAY*/ ";

                if (!string.IsNullOrEmpty(rParams[2]))
                {
                    xSql += string.Format("   AND (lower(t.course_nm) like '%{0}%' ", rParams[2].ToLower().Replace("'", "''")); // course_nm
                    xSql += string.Format("   OR  lower(t.course_nm_abbr) like '%{0}%') ", rParams[2].ToLower().Replace("'", "''")); // course_nm_abbr
                }
                if (!string.IsNullOrEmpty(rParams[3]))
                    xSql += string.Format("   AND  t.course_lang = '{0}' ", rParams[3]); // course_lang
                if (!string.IsNullOrEmpty(rParams[4]))
                    xSql += string.Format("   AND  t.course_type = '{0}' ", rParams[4]); // course_type
                // 조회조건 추가 <END>
                
                xSql += "                       ORDER BY t.ins_dt DESC ";
                xSql += "               ) b ";
                xSql += "           ) ";
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
        * Function name : GetOpenCourseInfo
        * Purpose       : 개설과정 전체 목록을 전달하는 처리
        * Input         : string[] rParams (0: pagesize, 1: currentPageIndex)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetOpenCourseInfo(string[] rParams)
        {
            try
            {
                string xSql = " SELECT * FROM ";
                xSql += "           ( SELECT rownum rnum, b.* FROM  ";
                xSql += "               ( ";
                xSql += "                   SELECT t.course_id, t.course_nm, c1.d_knm as course_type, c2.d_knm as course_group, ";
                xSql += "                          c3.d_knm as course_field, c4.d_knm as course_lang, c5.d_knm as course_inout, ";
                xSql += "                          o.course_year || '-' || o.course_seq as course_year, ";
                xSql += "                          o.course_begin_dt, o.course_end_dt, count(*) over() totalrecordcount ";
                xSql += "                   FROM t_course t ";
                xSql += "                   INNER JOIN t_open_course o "; // course_id
                xSql += "                   ON t.course_id = o.course_id ";
                xSql += "                   INNER JOIN t_code_detail c1 "; // course_type(master_code) : 0006
                xSql += "                   ON t.course_type = c1.d_cd ";
                xSql += "                   INNER JOIN t_code_detail c2 "; // course_group(master_code) : 0003
                xSql += "                   ON t.course_group = c2.d_cd ";
                xSql += "                   INNER JOIN t_code_detail c3 "; // course_field(master_code) : 0004
                xSql += "                   ON t.course_field = c3.d_cd ";
                xSql += "                   INNER JOIN t_code_detail c4 "; // course_lang(master_code) : 0017
                xSql += "                   ON t.course_lang = c4.d_cd ";
                xSql += "                   INNER JOIN t_code_detail c5 "; // course_inout(master_code) : 0005
                xSql += "                   ON t.course_lang = c5.d_cd ";
                xSql += "                   WHERE c1.m_cd='0006' AND c2.m_cd='0003' AND c3.m_cd='0004' AND c4.m_cd='0017' AND c5.m_cd='0005' ";
                xSql += "                   ORDER BY o.course_begin_dt DESC ";
                xSql += "                ) b ";
                xSql += "          ) ";
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
        * Function name : GetOpenCourseInfoByCondition
        * Purpose       : 조회조건에 따른 개설과정 전체 목록을 전달하는 처리
        * Input         : string[] rParams (0: pagesize, 1: currentPageIndex, 2: course_nm, 3: course_lang, 4: course_type)
        *                                   5: course_year, 6: course_begin_dt, 7: course_end_dt
        * Output        : DataTable
        *************************************************************/
        public DataTable GetOpenCourseInfoByCondition(string[] rParams)
        {
            try
            {
                string xSql = " SELECT * FROM ";
                xSql += "           ( SELECT rownum rnum, b.* FROM  ";
                xSql += "               ( ";
                xSql += "                   SELECT t.course_id, t.course_nm, c1.d_knm as course_type, c2.d_knm as course_group, ";
                xSql += "                          c3.d_knm as course_field, c4.d_knm as course_lang, c5.d_knm as course_inout, ";
                xSql += "                          o.course_year || '-' || o.course_seq as course_year, ";
                xSql += "                          o.course_begin_dt, o.course_end_dt, count(*) over() totalrecordcount ";
                xSql += "                   FROM t_course t ";
                xSql += "                   INNER JOIN t_open_course o "; // course_id
                xSql += "                   ON t.course_id = o.course_id ";
                xSql += "                   INNER JOIN t_code_detail c1 "; // course_type(master_code) : 0006
                xSql += "                   ON t.course_type = c1.d_cd ";
                xSql += "                   INNER JOIN t_code_detail c2 "; // course_group(master_code) : 0003
                xSql += "                   ON t.course_group = c2.d_cd ";
                xSql += "                   INNER JOIN t_code_detail c3 "; // course_field(master_code) : 0004
                xSql += "                   ON t.course_field = c3.d_cd ";
                xSql += "                   INNER JOIN t_code_detail c4 "; // course_lang(master_code) : 0017
                xSql += "                   ON t.course_lang = c4.d_cd ";
                xSql += "                   INNER JOIN t_code_detail c5 "; // course_inout(master_code) : 0005
                xSql += "                   ON t.course_lang = c5.d_cd ";
                
                // 조회조건 추가 <START>
                xSql += "                       WHERE c1.m_cd='0006' AND c2.m_cd='0003' AND c3.m_cd='0004' AND c4.m_cd='0017' AND c5.m_cd='0005' ";

                if (!string.IsNullOrEmpty(rParams[2]))
                    xSql += string.Format("   AND  lower(t.course_nm) like '%{0}%' ", rParams[2].ToLower().Replace("'", "''")); // course_nm
                if (!string.IsNullOrEmpty(rParams[3]))
                    xSql += string.Format("   AND  t.course_lang = '{0}' ", rParams[3]); // course_lang
                if (!string.IsNullOrEmpty(rParams[4]))
                    xSql += string.Format("   AND  t.course_type = '{0}' ", rParams[4]); // course_type
                if (!string.IsNullOrEmpty(rParams[5]))
                    xSql += string.Format("   AND  o.course_year = '{0}' ", rParams[5]); // course_year
                if ((!string.IsNullOrEmpty(rParams[6])) && (!string.IsNullOrEmpty(rParams[7])))
                    xSql += string.Format("   AND ( to_char(o.course_begin_dt, 'yyyy.MM.dd') >= '{0}' AND to_char(o.course_begin_dt, 'yyyy.MM.dd') <= '{1}' ) ", rParams[6], rParams[7]); // course_begin_dt, course_end_dt
                // 조회조건 추가 <END>

                xSql += "                       ORDER BY o.course_begin_dt DESC ";
                xSql += "               ) b ";
                xSql += "           ) ";
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
        * Function name : GetRecentOpenCourseInfoCountBy
        * Purpose       : 개설된 과정 중에 최근 N개의 데이터를 GridView에 바인딩을 위한 처리
        * Input         : string[] rParams (0: count)
        * Output        : DataTable
        *************************************************************/
        public DataSet GetRecentOpenCourseInfoCountBy(string[] rParams)
        {
            Database db = null;
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");
                    
                DataTable xDt = null;

                xSql = @"
                    SELECT *
                      FROM (
                            SELECT NOT_NO
                                    , NOT_SUB
                                    , TO_CHAR(INS_DT,'YYYY.MM.DD') AS NOTICE_BEGIN_DT
                                    , (case when INS_DT >= SYSDATE-" + rParams[1] + @" then 'Y' else 'N' end) AS NEW_ICON
                              FROM T_NOTICE
                             WHERE DEL_YN = 'N'
                                    AND NOT_KIND = '000001'
                             ORDER BY INS_DT DESC
                           ) N
                     WHERE ROWNUM <= " +rParams[0]+" ";
                xDt = base.ExecuteDataTable(db, xSql);
                base.MergeTable(ref xDs, xDt, "table1");
                
                xSql = @"        SELECT * 
                                   FROM (";
                xSql += "                   SELECT o.open_course_id, t.course_nm, ";
                xSql += "                          to_char(o.course_begin_dt,'yyyy.mm.dd') as course_begin_dt ";
                xSql += "                          , (case when o.course_begin_dt >= SYSDATE-" + rParams[1] + @" then 'Y' else 'N' end) AS NEW_ICON ";
                xSql += "                     FROM t_course t ";
                xSql += "                    INNER JOIN t_open_course o "; 
                xSql += "                       ON t.course_id = o.course_id ";
                xSql += "                    WHERE t.use_flg='Y' AND o.use_flg = 'Y'  ";
                xSql += "                    ORDER BY o.course_begin_dt DESC ";
                xSql += "                ) c ";
                xSql += string.Format(" WHERE  rownum <= {0} ", rParams[0]);

                xDt = base.ExecuteDataTable(db, xSql);
                base.MergeTable(ref xDs, xDt, "table2");
                    
                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            finally
            {
                db = null;
            }
            return xDs;
        }

        /************************************************************
        * Function name : GetOpenCourseInfoOfCalendar
        * Purpose       : Default 페이지의 일자별 개설과정 정보 확인하는 Calendar에 데이터를 바인딩하기 위한 처리
        * Input         : string[] rParams (0: course_begin_dt, 1: course_end_dt)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetOpenCourseInfoOfCalendar(string[] rParams)
        {
            try
            {
                string xSql = " SELECT SUBSTR(t.course_nm,0,13) AS course_nm, o.open_course_id ";
                xSql += "       FROM t_course t  ";
                xSql += "       INNER JOIN t_open_course o ";
                xSql += "       ON t.course_id = o.course_id ";
                xSql += "       WHERE to_char(o.course_begin_dt, 'yyyyMMdd') >= '" + rParams[0] + "' ";
                xSql += "       AND to_char(o.course_end_dt, 'yyyyMMdd') <= '" + rParams[1] + "' ";
                xSql += "       AND ROWNUM <= 3 ";
                xSql += "       ORDER BY o.course_begin_dt ASC ";

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
       * Function name : GetCourseList
       * Purpose       : 과정 목록 조회 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        #region public DataTable GetCourseList(string[] rParams)
        public DataTable GetCourseList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = this.PageSize.ToString(); // pagesize
                //xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex

                //xParams[2] = this.ddlGroup.SelectedValue.Replace("*", "");
                //xParams[3] = this.ddlField.SelectedValue.Replace("*", "");
                //xParams[4] = this.ddlType.SelectedValue.Replace("*", "");
                //xParams[5] = this.ddlVslType.SelectedValue.Replace("*", "");
                //xParams[6] = this.ddlLang.SelectedValue.Replace("*", "");
                //xParams[7] = this.txtCourseNm.Text;
                //xParams[8] = this.ddlUse.SelectedValue.Replace("*", ""); 


                                //, (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0028' AND D_CD = SUBSTR(C.VSL_TYPE, 1, INSTR(C.VSL_TYPE, ',', 1, 1)-1)) 
                                //  || ' ' ||
                                //  (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0028' AND D_CD = SUBSTR(C.VSL_TYPE, INSTR(C.VSL_TYPE, ',', 1, 1)+1, INSTR(C.VSL_TYPE, ',', 1, 2) - INSTR(C.VSL_TYPE, ',', 1, 1)-1))
                                //  || ' ' ||
                                //  (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0028' AND D_CD = SUBSTR(C.VSL_TYPE, INSTR(C.VSL_TYPE, ',', 1, 2)+1, INSTR(C.VSL_TYPE, ',', 1, 3) - INSTR(C.VSL_TYPE, ',', 1, 2)-1))
                                //  || ' ' ||
                                //  (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0028' AND D_CD = SUBSTR(C.VSL_TYPE, INSTR(C.VSL_TYPE, ',', 1, 3)+1, INSTR(C.VSL_TYPE, ',', 1, 4) - INSTR(C.VSL_TYPE, ',', 1, 3)-1))
                                //  || ' ' ||
                                //  (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0028' AND D_CD = SUBSTR(C.VSL_TYPE, INSTR(C.VSL_TYPE, ',', 1, 4)+1, INSTR(C.VSL_TYPE, ',', 1, 5) - INSTR(C.VSL_TYPE, ',', 1, 4)-1))
                                //  AS VSL_TYPE 

                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT
                            C.COURSE_ID
                            , C.COURSE_NM
                            , C.EXPIRED_PERIOD
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0017' AND D_CD = C.COURSE_LANG) AS COURSE_LANG

                            , C.VSL_TYPE
                            

                            , DECODE(C.USE_FLG , 'Y', 'YES', 'NO') AS USE_FLG 
                            , C.INS_DT
                            , C.TEMP_SAVE_FLG 
                            , COUNT(*) OVER() TOTALRECORDCOUNT
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0004' AND D_CD = C.COURSE_FIELD) AS COURSE_FIELD
                            , C.MANAGER
                        FROM T_COURSE C
                        WHERE 1=1                         
                        ";

                if (rParams[2] != string.Empty)
                    xSql += " AND C.COURSE_GROUP = '" + rParams[2] + "' ";
                if (rParams[3] != string.Empty)
                    xSql += " AND C.COURSE_FIELD = '" + rParams[3] + "' ";
                if (rParams[4] != string.Empty)
                    xSql += " AND C.COURSE_TYPE = '" + rParams[4] + "' ";
                if (rParams[5] != string.Empty)
                    xSql += " AND C.VSL_TYPE = '" + rParams[5] + "' ";
                if (rParams[6] != string.Empty)
                    xSql += " AND C.COURSE_LANG = '" + rParams[6] + "' ";
                if (rParams[7] != string.Empty)
                    xSql += " AND UPPER(C.COURSE_NM) LIKE '%" + rParams[7].Replace("'", "''").ToUpper() + "%' ";
                if (rParams[8] != string.Empty)
                    xSql += " AND C.USE_FLG = '" + rParams[8] + "' ";            

                xSql += @"                  
                        ORDER BY INS_DT DESC 
                    ) O
                )
                ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));

                DataTable dt = base.ExecuteDataTable("LMS", xSql);
                DataTable dttemp = dt.Clone();
                DataRow drtemp = null;
                string[] xVslType; 
                foreach (DataRow dr in dt.Rows)
                {
                    drtemp = dttemp.NewRow();
                    drtemp["COURSE_ID"] = dr["COURSE_ID"];
                    drtemp["COURSE_NM"] = dr["COURSE_NM"];
                    drtemp["EXPIRED_PERIOD"] = dr["EXPIRED_PERIOD"];
                    drtemp["COURSE_TYPE"] = dr["COURSE_TYPE"];
                    drtemp["COURSE_LANG"] = dr["COURSE_LANG"];
                    drtemp["COURSE_FIELD"] = dr["COURSE_FIELD"];
                    drtemp["MANAGER"] = dr["MANAGER"];

                    xVslType = dr["VSL_TYPE"].ToString().Split(',');

                    for (int i = 0; i < xVslType.Length; i++)
                    {
                        drtemp["VSL_TYPE"] += this.GetVslTypeName("0028", xVslType[i]) + ","; 
                        //drtemp["VSL_TYPE"] = dr["VSL_TYPE"].ToString();
                    }
                    drtemp["VSL_TYPE"] = drtemp["VSL_TYPE"].ToString().Trim(new char[] { ',' }); 

                    drtemp["USE_FLG"] = dr["USE_FLG"].ToString();
                    drtemp["INS_DT"] = dr["INS_DT"].ToString();
                    drtemp["TEMP_SAVE_FLG"] = dr["TEMP_SAVE_FLG"].ToString();
                    drtemp["TOTALRECORDCOUNT"] = dr["TOTALRECORDCOUNT"].ToString();
                    dttemp.Rows.Add(drtemp); 
                }

                return dttemp; 

                //return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        public string GetVslTypeName(string rMCD, string rDCD)
        {
            string xSql = string.Empty; 
            try
            {

                xSql = "SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '" + rMCD + "' AND D_CD = '" + rDCD + "' ";
                object xobj = base.ExecuteScalar("LMS", xSql);
                string xnm = xobj != null ? xobj.ToString() : string.Empty;
                return xnm; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
       * Function name : GetCourseList_Excel
       * Purpose       : 과정 목록 조회 Excel 출력 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        #region public DataTable GetCourseList_Excel(string[] rParams)
        public DataTable GetCourseList_Excel(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = this.PageSize.ToString(); // pagesize
                //xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex

                //xParams[2] = this.ddlGroup.SelectedValue.Replace("*", "");
                //xParams[3] = this.ddlField.SelectedValue.Replace("*", "");
                //xParams[4] = this.ddlType.SelectedValue.Replace("*", "");
                //xParams[5] = this.ddlVslType.SelectedValue.Replace("*", "");
                //xParams[6] = this.ddlLang.SelectedValue.Replace("*", "");
                //xParams[7] = this.txtCourseNm.Text;
                //xParams[8] = this.ddlUse.SelectedValue.Replace("*", ""); 

                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT
                            C.COURSE_ID
                            , C.COURSE_NM
                            , C.EXPIRED_PERIOD
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0017' AND D_CD = C.COURSE_LANG) AS COURSE_LANG
                            , C.VSL_TYPE
                            , DECODE(C.USE_FLG , 'Y', 'YES', 'NO') AS USE_FLG 
                            , TO_CHAR(C.INS_DT, 'YYYY.MM.DD') AS INS_DT 
                            --, C.TEMP_SAVE_FLG 
                            --, COUNT(*) OVER() TOTALRECORDCOUNT                             
                        FROM T_COURSE C
                        WHERE 1=1                         
                        ";

                if (rParams[2] != string.Empty)
                    xSql += " AND C.COURSE_GROUP = '" + rParams[2] + "' ";
                if (rParams[3] != string.Empty)
                    xSql += " AND C.COURSE_FIELD = '" + rParams[3] + "' ";
                if (rParams[4] != string.Empty)
                    xSql += " AND C.COURSE_TYPE = '" + rParams[4] + "' ";
                if (rParams[5] != string.Empty)
                    xSql += " AND C.VSL_TYPE = '" + rParams[5] + "' ";
                if (rParams[6] != string.Empty)
                    xSql += " AND C.COURSE_LANG = '" + rParams[6] + "' ";
                if (rParams[7] != string.Empty)
                    xSql += " AND UPPER(C.COURSE_NM) LIKE '%" + rParams[7].Replace("'", "''").ToUpper() + "%' ";
                if (rParams[8] != string.Empty)
                    xSql += " AND C.USE_FLG = '" + rParams[8] + "' ";

                xSql += @"                  
                        ORDER BY INS_DT DESC 
                    ) O
                )
                ";

                //xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                //xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));


                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
       * Function name : GetCourseInfoEdit
       * Purpose       : 과정 상세조회 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        #region public DataTable GetCourseInfoEdit(string[] rParams)
        public DataTable GetCourseInfoEdit(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                xSql = @" 
                SELECT C.* 
                FROM T_COURSE C 
                WHERE C.COURSE_ID = '{0}' 
                "; 
                    
                xSql = string.Format(xSql, rParams[0]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
      * Function name : GetDutyStep
      * Purpose       : 직급(grade 조회)
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetDutyStep()
        public DataTable GetDutyStep()
        { 
             try
            {
                string xSql = string.Empty;
                //xSql = "SELECT DUTY_STEP,  '[' || DUTY_STEP || ']' || STEP_NAME || '-' || STEP_ENAME AS STEPNM  FROM V_HDUTYSTEP "; 
                xSql = @" SELECT DUTY_STEP,  STEP_NAME, STEP_ENAME 
                                , '[' || DUTY_STEP || '] ' || STEP_NAME || DECODE(STEP_ENAME, NULL, '',  ' (' || STEP_ENAME || ')' ) AS step_nm 
                                FROM V_HDUTYSTEP 
                                WHERE USE_YN ='Y'
                                ORDER BY STEP_SEQ
                                "; 
                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 
        
        /************************************************************
    * Function name : GetDutyWork
    * Purpose       : 직책(rank)
    * Input         : string[] rParams (0: pagesize, 1: pageno)
    * Output        : DataTable
    *************************************************************/
        #region public DataTable GetDutyWork()
        public DataTable GetDutyWork()
        {
            try
            {
                string xSql = string.Empty;
                xSql = @"SELECT DUTY_WORK, DUTY_WORK_NAME, DUTY_WORK_ENAME 
                                , '[' || duty_work || '] ' || duty_work_name || DECODE(DUTY_WORK_ENAME, NULL, '',  ' (' || duty_work_ename || ')' ) AS duty_nm 
                                FROM V_HDUTYWORK 
                                WHERE USE_YN ='Y' AND SHIP_GU = 'B20' 
                                ORDER BY WORK_SEQ
                                ";
                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
       * Function name : GetSubjectList
       * Purpose       : 과목조회 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        #region public DataTable GetSubjectList(string[] rParams)
        public DataTable GetSubjectList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = this.PageSize.ToString(); // pagesize
                //xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                //xParams[2] = ViewState["COURSE_ID"].ToString(); 

                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT
                            S.SUBJECT_ID
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0017' AND D_CD = S.SUBJECT_LANG ) AS SUBJECT_LANG 
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0042' AND D_CD = S.SUBJECT_KIND ) AS SUBJECT_KIND
                            , S.SUBJECT_NM
                            , S.LEARNING_TIME
                            , S.LECTURER_NM AS INSTRUCTOR
                            , DECODE(S.USE_FLG , 'Y', 'YES', 'NO') AS USE_FLG 
                            , S.INS_DT
                            , COUNT(*) OVER() TOTALRECORDCOUNT          
                            , S.TEMP_SAVE_FLG                    
                        FROM T_COURSE_SUBJECT C, T_SUBJECT S
                        WHERE C.SUBJECT_ID = S.SUBJECT_ID 
                        ";                 
                xSql += " AND C.COURSE_ID = '" + rParams[2] + "' ";
                xSql += @"                  
                        ORDER BY C.SUBJECT_SEQ  
                    ) O
                )
                ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));


                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
       * Function name : GetSubjectContentsList
       * Purpose       : 컨텐츠 조회  
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        #region public DataTable GetSubjectContentsList(string rSubjectId)
        public DataTable GetSubjectContentsList(string rSubjectId)
        {
            try
            {
                string xSql = string.Empty;

                xSql = @" 
                        SELECT 'CONTENT | ' || C.CONTENTS_ID || ' | ' || C.CONTENTS_NM  AS CONTENTS_NM
                        FROM T_SUBJECT_CONTENTS S, T_CONTENTS C
                        WHERE S.CONTENTS_ID = C.CONTENTS_ID 
                            AND S.SUBJECT_ID = '" + rSubjectId + "' "; 
                    
                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
       * Function name : GetContentsList
       * Purpose       : 과목조회 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        #region public DataTable GetContentsList(string[] rParams)
        public DataTable GetContentsList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = this.PageSize.ToString(); // pagesize
                //xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                //xParams[2] = ViewState["COURSE_ID"].ToString(); 

                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT 
                            C.CONTENTS_ID
                            , C.CONTENTS_NM 
                            , C.CONTENTS_FILE_NM
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0042' AND D_CD = C.CONTENTS_TYPE) AS CONTENTS_TYPE
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0017' AND D_CD = C.CONTENTS_LANG) AS CONTENTS_LANG
                            , C.CONTENTS_REMARK 
                            , C.INS_DT 
                            , COUNT(*) OVER() TOTALRECORDCOUNT   
                            , S.SUBJECT_NM
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0042' AND D_CD = S.SUBJECT_KIND ) AS SUBJECT_KIND
                            , C.TEMP_SAVE_FLG 
                        FROM T_COURSE_SUBJECT CS, T_SUBJECT_CONTENTS SC, T_SUBJECT S, T_CONTENTS C
                        WHERE CS.SUBJECT_ID = SC.SUBJECT_ID 
                          AND SC.SUBJECT_ID = S.SUBJECT_ID 
                          AND SC.CONTENTS_ID = C.CONTENTS_ID
                        ";
                xSql += " AND CS.COURSE_ID = '" + rParams[2] + "' ";
                xSql += @"                  
                        ORDER BY CS.SUBJECT_SEQ, SC.CONTENTS_SEQ 
                    ) O
                )
                ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));


                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
       * Function name : GetAssessList
       * Purpose       : 과목조회 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        #region public DataTable GetAssessList(string[] rParams)
        public DataTable GetAssessList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = this.PageSize.ToString(); // pagesize
                //xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                //xParams[2] = ViewState["COURSE_ID"].ToString(); 

                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT 
                            (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0042' AND D_CD = A.QUESTION_KIND) AS QUESTION_KIND
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0025' AND D_CD = A.COURSE_GROUP) AS COURSE_GROUP
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0004' AND D_CD = A.COURSE_FIELD) AS COURSE_FIELD
                            , A.QUESTION_CONTENT 
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0045' AND D_CD = A.QUESTION_TYPE) AS QUESTION_TYPE
                            , A.INS_DT
                            , COUNT(*) OVER() TOTALRECORDCOUNT     
                            , A.QUESTION_ID 
                            , A.TEMP_SAVE_FLG 
                        FROM T_COURSE_ASSESS_QUESTION C, T_ASSESS_QUESTION A
                        WHERE C.QUESTION_ID = A.QUESTION_ID
                        ";
                xSql += " AND C.COURSE_ID = '" + rParams[2] + "' ";
                xSql += @"                  
                        ORDER BY INS_DT DESC 
                    ) O
                )
                ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));


                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
       * Function name : GetTextbookList
       * Purpose       : 교재조회  
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        #region public DataTable GetTextbookList(string[] rParams)
        public DataTable GetTextbookList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = this.PageSize.ToString(); // pagesize
                //xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                //xParams[2] = ViewState["COURSE_ID"].ToString(); 

                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT 
                            T.TEXTBOOK_ID
                            , T.TEXTBOOK_NM
                            , T.AUTHOR
                            , T.PUBLISHER
                            , T.PRICE
                            , T.INS_ID
                            , T.INS_DT
                            , T.USE_FLG 
                            , (SELECT USER_NM_KOR FROM T_USER WHERE USER_ID = T.INS_ID) AS USER_NM_KOR 
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0049' AND D_CD = T.TEXTBOOK_TYPE) AS TEXTBOOK_TYPE
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0017' AND D_CD = T.TEXTBOOK_LANG) AS TEXTBOOK_LANG
                            , COUNT(*) OVER() TOTALRECORDCOUNT     
                            , T.TEMP_SAVE_FLG 
                        FROM T_COURSE_TEXTBOOK C, T_TEXTBOOK T
                        WHERE C.TEXTBOOK_ID = T.TEXTBOOK_ID 
                        ";
                xSql += " AND C.COURSE_ID = '" + rParams[2] + "' ";
                xSql += @"                  
                        ORDER BY INS_DT DESC 
                    ) O
                )
                ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));


                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
      * Function name : SetCourseInfo
      * Purpose       : 과정 등록하는 처리
      * Input         : 
      * Output        : string
      *************************************************************/
        #region public string SetCourseInfo(object[] rParams)
        public string SetCourseInfo(object[] rParams)
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
                        bool xUpdate = false; //update 할지 여부 
                        string xQID = string.Empty;

                        if (rParams[0].ToString() != string.Empty)
                        {
                            xSql = "SELECT TEMP_SAVE_FLG FROM T_COURSE WHERE COURSE_ID = '" + rParams[0] + "' ";
                            xCmdLMS.CommandText = xSql;
                            object xobj = base.ExecuteScalar(db, xCmdLMS, xTransLMS);
                            if (xobj != null && xobj.ToString() == "Y")
                                xUpdate = true;
                            else
                                xUpdate = false; 
                        }
                        else
                        {
                            xUpdate = false; 
                        }
                        
                        if (xUpdate)
                        {
                            xQID = rParams[0].ToString();

                            xSql = @"
                            UPDATE T_COURSE 
                            SET 
                                COURSE_LANG       = :COURSE_LANG     
                                ,COURSE_TYPE       = :COURSE_TYPE     
                                ,COURSE_GROUP      = :COURSE_GROUP    
                                ,COURSE_FIELD      = :COURSE_FIELD    
                                ,COURSE_KIND       = :COURSE_KIND     
                                ,VSL_TYPE          = :VSL_TYPE        
                                ,COURSE_NM         = :COURSE_NM       
                                ,COURSE_NM_ABBR    = :COURSE_NM_ABBR  
                                ,COURSE_INTRO      = :COURSE_INTRO    
                                ,COURSE_OBJECTIVE  = :COURSE_OBJECTIVE
                                ,COURSE_DAY        = :COURSE_DAY      
                                ,COURSE_TIME       = :COURSE_TIME     
                                ,ESS_DUTY_STEP     = :ESS_DUTY_STEP   
                                ,OPT_DUTY_WORK     = :OPT_DUTY_WORK   
                                ,CLASS_MAN_COUNT   = :CLASS_MAN_COUNT 
                                ,EXAM_TYPE         = :EXAM_TYPE       
                                ,EXPIRED_PERIOD    = :EXPIRED_PERIOD  
                                ,INSURANCE_FLG     = :INSURANCE_FLG   
                                ,USE_FLG           = :USE_FLG         
                                ,MANAGER           = :MANAGER         
                                , UPT_ID = :INS_ID
                                , UPT_DT = SYSDATE 
                            WHERE COURSE_ID = :COURSE_ID
                            ";

                            xPara = new OracleParameter[22];
                            xPara[0] = base.AddParam("COURSE_LANG", OracleType.VarChar, rParams[1]);
                            xPara[1] = base.AddParam("COURSE_TYPE", OracleType.VarChar, rParams[6]);
                            xPara[2] = base.AddParam("COURSE_GROUP", OracleType.VarChar, rParams[5]);
                            xPara[3] = base.AddParam("COURSE_FIELD", OracleType.VarChar, rParams[7]);
                            xPara[4] = base.AddParam("COURSE_KIND", OracleType.VarChar, rParams[8]);
                            xPara[5] = base.AddParam("VSL_TYPE", OracleType.VarChar, rParams[11]);
                            xPara[6] = base.AddParam("COURSE_NM", OracleType.VarChar, rParams[3]);
                            xPara[7] = base.AddParam("COURSE_NM_ABBR", OracleType.VarChar, rParams[4]);
                            xPara[8] = base.AddParam("COURSE_INTRO", OracleType.VarChar, rParams[13]);
                            xPara[9] = base.AddParam("COURSE_OBJECTIVE", OracleType.VarChar, rParams[14]);
                            xPara[10] = base.AddParam("COURSE_DAY", OracleType.Number, rParams[17]);
                            xPara[11] = base.AddParam("COURSE_TIME", OracleType.Number, rParams[18]);
                            xPara[12] = base.AddParam("ESS_DUTY_STEP", OracleType.VarChar, rParams[10]);
                            xPara[13] = base.AddParam("OPT_DUTY_WORK", OracleType.VarChar, rParams[12]);
                            xPara[14] = base.AddParam("CLASS_MAN_COUNT", OracleType.Number, rParams[15]);
                            xPara[15] = base.AddParam("EXAM_TYPE", OracleType.VarChar, rParams[9]);
                            xPara[16] = base.AddParam("EXPIRED_PERIOD", OracleType.Number, rParams[16]);
                            xPara[17] = base.AddParam("INSURANCE_FLG", OracleType.VarChar, rParams[2]);
                            xPara[18] = base.AddParam("USE_FLG", OracleType.VarChar, rParams[19]);
                            xPara[19] = base.AddParam("MANAGER", OracleType.VarChar, rParams[21]);
                            xPara[20] = base.AddParam("INS_ID", OracleType.VarChar, rParams[20]);
                            xPara[21] = base.AddParam("COURSE_ID", OracleType.VarChar, xQID);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);

                        }
                        else
                        {
                            // 2. DB 저장 (DB 작업 중에 문제발생 시, 저장된 파일 제거)
                            xSql = @" INSERT INTO T_COURSE 
                                    ( 
                                        COURSE_ID       
                                        ,COURSE_LANG     
                                        ,COURSE_TYPE     
                                        ,COURSE_GROUP    
                                        ,COURSE_FIELD    
                                        ,COURSE_KIND     
                                        ,VSL_TYPE        
                                        ,COURSE_NM       
                                        ,COURSE_NM_ABBR  
                                        ,COURSE_INTRO    
                                        ,COURSE_OBJECTIVE
                                        ,COURSE_DAY      
                                        ,COURSE_TIME     
                                        ,ESS_DUTY_STEP   
                                        ,OPT_DUTY_WORK   
                                        ,CLASS_MAN_COUNT 
                                        ,EXAM_TYPE       
                                        ,EXPIRED_PERIOD  
                                        ,INSURANCE_FLG   
                                        ,INS_ID          
                                        ,INS_DT          
                                        , UPT_ID
                                        , UPT_DT
                                        ,SEND_DT         
                                        ,SEND_FLG        
                                        ,USE_FLG         
                                        ,TEMP_SAVE_FLG
                                        ,HRISCODE
                                        ,MANAGER
                                    ) VALUES (
                                        :COURSE_ID       
                                        ,:COURSE_LANG     
                                        ,:COURSE_TYPE     
                                        ,:COURSE_GROUP    
                                        ,:COURSE_FIELD    
                                        ,:COURSE_KIND     
                                        ,:VSL_TYPE        
                                        ,:COURSE_NM       
                                        ,:COURSE_NM_ABBR  
                                        ,:COURSE_INTRO    
                                        ,:COURSE_OBJECTIVE
                                        ,:COURSE_DAY      
                                        ,:COURSE_TIME     
                                        ,:ESS_DUTY_STEP   
                                        ,:OPT_DUTY_WORK   
                                        ,:CLASS_MAN_COUNT 
                                        ,:EXAM_TYPE       
                                        ,:EXPIRED_PERIOD  
                                        ,:INSURANCE_FLG   
                                        ,:INS_ID          
                                        , SYSDATE     
                                        ,:INS_ID          
                                        , SYSDATE     
                                        ,:SEND_DT         
                                        ,:SEND_FLG        
                                        ,:USE_FLG         
                                        ,:TEMP_SAVE_FLG 
                                        ,:HRISCODE 
                                        ,:MANAGER 
                                    ) ";

                            vp_l_common_md com = new vp_l_common_md();
                            xQID = com.GetMaxIDOfTable(new string[] { "T_COURSE", "COURSE_ID" });

                            /*
                             2013.05.02
                             * 과정개설시 HRISCODE 자동입력되도록(COURSE_ID와 동일하게)
                             */
                            xPara = new OracleParameter[26];
                            xPara[0] = base.AddParam("COURSE_LANG", OracleType.VarChar, rParams[1]);
                            xPara[1] = base.AddParam("COURSE_TYPE", OracleType.VarChar, rParams[6]);
                            xPara[2] = base.AddParam("COURSE_GROUP", OracleType.VarChar, rParams[5]);
                            xPara[3] = base.AddParam("COURSE_FIELD", OracleType.VarChar, rParams[7]);
                            xPara[4] = base.AddParam("COURSE_KIND", OracleType.VarChar, rParams[8]);
                            xPara[5] = base.AddParam("VSL_TYPE", OracleType.VarChar, rParams[11]);
                            xPara[6] = base.AddParam("COURSE_NM", OracleType.VarChar, rParams[3]);
                            xPara[7] = base.AddParam("COURSE_NM_ABBR", OracleType.VarChar, rParams[4]);
                            xPara[8] = base.AddParam("COURSE_INTRO", OracleType.VarChar, rParams[13]);
                            xPara[9] = base.AddParam("COURSE_OBJECTIVE", OracleType.VarChar, rParams[14]);
                            xPara[10] = base.AddParam("COURSE_DAY", OracleType.Number, rParams[17]);
                            xPara[11] = base.AddParam("COURSE_TIME", OracleType.Number, rParams[18]);
                            xPara[12] = base.AddParam("ESS_DUTY_STEP", OracleType.VarChar, rParams[10]);
                            xPara[13] = base.AddParam("OPT_DUTY_WORK", OracleType.VarChar, rParams[12]);
                            xPara[14] = base.AddParam("CLASS_MAN_COUNT", OracleType.Number, rParams[15]);
                            xPara[15] = base.AddParam("EXAM_TYPE", OracleType.VarChar, rParams[9]);
                            xPara[16] = base.AddParam("EXPIRED_PERIOD", OracleType.Number, rParams[16]);
                            xPara[17] = base.AddParam("INSURANCE_FLG", OracleType.VarChar, rParams[2]);
                            xPara[18] = base.AddParam("USE_FLG", OracleType.VarChar, rParams[19]);
                            xPara[19] = base.AddParam("INS_ID", OracleType.VarChar, rParams[20]);
                            xPara[20] = base.AddParam("SEND_DT", OracleType.DateTime, DBNull.Value);
                            xPara[21] = base.AddParam("SEND_FLG", OracleType.VarChar, "0"); //최초 0 에서 SEND 시 1로 변경 하여 PACKAGE에서 2로 변경 
                            xPara[22] = base.AddParam("TEMP_SAVE_FLG", OracleType.VarChar, "Y");
                            xPara[23] = base.AddParam("COURSE_ID", OracleType.VarChar, xQID);
                            xPara[24] = base.AddParam("HRISCODE", OracleType.VarChar, xQID);
                            xPara[25] = base.AddParam("MANAGER", OracleType.VarChar, rParams[21]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);                           

                        }
                        //선박발송 하는 PACKAGE 호출 부분 추가 필요 

                        xRtn = xQID;
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
       * Function name : GetPopSubjectList
       * Purpose       : 과목 목록 조회 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        #region public DataTable GetPopSubjectList(string[] rParams)
        public DataTable GetPopSubjectList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = this.PageSize.ToString(); // pagesize
                //xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex

                //xParams[2] = this.ddlLang.SelectedItem.Value.Replace("*", ""); //lang
                //xParams[3] = this.ddlClassification.SelectedItem.Value.Replace("*", ""); //classification
                //xParams[4] = this.txtSubject.Text;
                //xParams[5] = this.txtInstructor.Text; 

                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT
                            S.SUBJECT_ID
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0017' AND D_CD = S.SUBJECT_LANG ) AS SUBJECT_LANG 
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0042' AND D_CD = S.SUBJECT_KIND ) AS SUBJECT_KIND
                            , S.SUBJECT_NM
                            , S.LEARNING_TIME
                            , S.LECTURER_NM AS INSTRUCTOR
                            , DECODE(S.USE_FLG , 'Y', 'YES', 'NO') AS USE_FLG 
                            , S.INS_DT
                            , COUNT(*) OVER() TOTALRECORDCOUNT       
                            , S.TEMP_SAVE_FLG 
                        FROM T_SUBJECT S
                        WHERE 1=1 
                        ";

                if (rParams[2] != string.Empty)
                    xSql += " AND S.SUBJECT_LANG = '" + rParams[2] + "' ";
                if (rParams[3] != string.Empty)
                    xSql += " AND S.SUBJECT_KIND = '" + rParams[3] + "' ";
                if (rParams[4] != string.Empty)
                    xSql += " AND UPPER(S.SUBJECT_NM) LIKE '%" + rParams[4].Replace("'", "''").ToUpper() + "%' ";
                if (rParams[5] != string.Empty)
                    xSql += " AND UPPER(S.LECTURER_NM) LIKE '%" + rParams[5].Replace("'", "''").ToUpper() + "%' ";

                xSql += @"                  
                        ORDER BY INS_DT DESC 
                    ) O
                )
                ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));


                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 



        /************************************************************
       * Function name : GetPopContentsList
       * Purpose       : 조회조건에 따른 컨텐츠 전체 목록을 전달하는 처리
       * Input         : string[] rParams (0: pagesize, 1: currentPageIndex, 2: contents_nm, 3: remark, 4: lang, 5: contents_type)
       * Output        : DataTable
       *************************************************************/
        #region public DataTable GetPopContentsList(string[] rParams)
        public DataTable GetPopContentsList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;
                xSql = @" 

                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT 
                          C.CONTENTS_ID
                          , C.CONTENTS_NM
                          , C.CONTENTS_FILE_NM
                          , C.CONTENTS_REMARK
                          , C.INS_ID
                          , C.INS_DT
                          , C.TEMP_SAVE_FLG 
                          , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0042' AND D_CD = C.CONTENTS_TYPE) AS CONTENTS_TYPE
                          , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0017' AND D_CD = C.CONTENTS_LANG) AS CONTENTS_LANG 
                          , COUNT(*) OVER() TOTALRECORDCOUNT    
                        FROM T_CONTENTS C
                        WHERE 1=1  
                        ";

                if (rParams[2] != string.Empty)
                    xSql += " AND C.CONTENTS_NM LIKE  '%" + rParams[2].Replace("'", "''") + "%' ";
                if (rParams[3] != string.Empty)
                    xSql += " AND UPPER(C.CONTENTS_REMAKR) LIKE '%" + rParams[3].Replace("'", "''").ToUpper() + "%' ";
                if (rParams[4] != string.Empty)
                    xSql += " AND C.CONTENTS_LANG = '" + rParams[4] + "' ";
                if (rParams[5] != string.Empty)
                    xSql += " AND C.CONTENTS_TYPE = '" + rParams[5] + "' ";

                xSql += @"                  
                        ORDER BY INS_DT DESC 
                    ) O
                )
                ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
               
                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 


        /************************************************************
       * Function name : GetPopAssessList
       * Purpose       : 평가문제 목록 조회 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        #region public DataTable GetPopAssessList(string[] rParams)
        public DataTable GetPopAssessList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = pagesize
                //xParams[1] = currentPageIndex

                //xParams[2] = 분류 kind 
                //xParams[3] = 언어 language
                //xParams[4] = 시험유형 type 
                //xParams[5] = 질문 content 
                //xParams[6] = 과정 그룹  group 
                //xParams[7] = 분야 field

                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, ASSESS.* FROM 
                    (
                        SELECT 
                            (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0042' AND D_CD = QUESTION_KIND) AS QUESTION_KIND
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0025' AND D_CD = COURSE_GROUP) AS COURSE_GROUP
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0004' AND D_CD = COURSE_FIELD) AS COURSE_FIELD
                            , QUESTION_CONTENT 
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0045' AND D_CD = QUESTION_TYPE) AS QUESTION_TYPE
                            , INS_DT
                            , COUNT(*) OVER() TOTALRECORDCOUNT     
                            , QUESTION_ID 
                            , TEMP_SAVE_FLG 
                        FROM T_ASSESS_QUESTION
                        WHERE 1=1 ";

                if (rParams[2] != string.Empty)
                    xSql += " AND QUESTION_KIND = '" + rParams[2] + "' ";
                if (rParams[3] != string.Empty)
                    xSql += " AND QUESTION_LANG = '" + rParams[3] + "' ";
                if (rParams[4] != string.Empty)
                    xSql += " AND QUESTION_TYPE = '" + rParams[4] + "' ";
                if (rParams[5] != string.Empty)
                    xSql += " AND UPPER(QUESTION_CONTENT) LIKE '%" + rParams[5].Replace("'", "''").ToUpper() + "%' ";
                if (rParams[6] != string.Empty)
                    xSql += "AND COURSE_GROUP = '" + rParams[6] + "' ";
                if (rParams[7] != string.Empty)
                    xSql += "AND COURSE_FIELD = '" + rParams[7] + "' ";

                if (rParams[8] != string.Empty)
                    xSql += " AND COURSE_LIST LIKE '%" + rParams[8] + "%' ";
                if (rParams[9] != string.Empty)
                    xSql += " AND SUBJECT_LIST LIKE '%" + rParams[9] + "%' "; 

                xSql += @"                  
                        ORDER BY INS_DT DESC 
                    ) ASSESS
                )
                ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
       * Function name : GetPopTextbookList
       * Purpose       : 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        #region public DataTable GetPopTextbookList(string[] rParams)
        public DataTable GetPopTextbookList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;
               

                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, TEXTBOOK.* FROM 
                    (
                        SELECT 
                            T.TEXTBOOK_ID
                            , T.TEXTBOOK_NM
                            , T.AUTHOR
                            , T.PUBLISHER
                            , T.PRICE
                            , T.INS_ID, T.INS_DT
                            , T.USE_FLG
                            , (SELECT USER_NM_KOR FROM T_USER WHERE USER_ID = T.INS_ID) AS USER_NM_KOR
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0049' AND D_CD = T.TEXTBOOK_TYPE) AS TEXTBOOK_TYPE
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0017' AND D_CD = T.TEXTBOOK_LANG) AS TEXTBOOK_LANG
                            , COUNT(*) OVER() TOTALRECORDCOUNT     
                            , TEMP_SAVE_FLG 
                        FROM T_TEXTBOOK T
                        WHERE 1=1 ";

                if (rParams[2] != string.Empty)
                    xSql += " AND UPPER(T.TEXTBOOK_NM) LIKE  '%" + rParams[2].Replace("'", "''").ToUpper() + "%' ";
                if (rParams[3] != string.Empty)
                    xSql += " AND T.TEXTBOOK_TYPE = '" + rParams[3] + "' ";
                if (rParams[4] != string.Empty)
                    xSql += " AND T.COURSE_GROUP = '" + rParams[4] + "' ";
                if (rParams[5] != string.Empty)
                    xSql += " AND T.COURSE_FIELD  = '" + rParams[5] + "' ";
                if (rParams[6] != string.Empty)
                    xSql += " AND T.TEXTBOOK_LANG = '" + rParams[6] + "' ";
                if (rParams[7] != string.Empty && rParams[8] != string.Empty)
                    xSql += " AND TO_CHAR(T.INS_DT, 'YYYY.MM.DD') >= '" + rParams[7] + "' AND TO_CHAR(T.INS_DT, 'YYYY.MM.DD') <= '" + rParams[8] + "' "; 

                xSql += @"                  
                        ORDER BY T.INS_DT DESC 
                    ) TEXTBOOK
                )
                ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 


        /************************************************************
       * Function name :GetSubjectAddContentsList
       * Purpose       : 
       * Input         : string[] rParams (0: pagesize, 1: currentPageIndex, 2: contents_nm, 3: remark, 4: lang, 5: contents_type)
       * Output        : DataTable
       *************************************************************/
        #region public DataTable GetSubjectAddContentsList(string rSubject)
        public DataTable GetSubjectAddContentsList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;
                xSql = @" 

                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT 
                          C.CONTENTS_ID
                          , C.CONTENTS_NM
                          , C.CONTENTS_FILE_NM
                          , C.CONTENTS_REMARK
                          , C.INS_ID
                          , C.INS_DT
                          , C.TEMP_SAVE_FLG 
                          , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0042' AND D_CD = C.CONTENTS_TYPE) AS CONTENTS_TYPE
                          , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0017' AND D_CD = C.CONTENTS_LANG) AS CONTENTS_LANG 
                          , COUNT(*) OVER() TOTALRECORDCOUNT    
                          , S.SUBJECT_ID 
                        FROM T_SUBJECT_CONTENTS S, T_CONTENTS C
                        WHERE S.CONTENTS_ID = C.CONTENTS_ID
                          AND S.SUBJECT_ID = '" + rParams[2] + "' ";                         
                xSql += @"                  
                        ORDER BY INS_DT DESC 
                    ) O
                )
                ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
             
                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 


        /************************************************************
      * Function name : SetSubjectContentsInfo
      * Purpose       : 과목 & 컨텐츠를 등록하는 처리
      * Input         : 
      * Output        : string
      *************************************************************/
        #region public void SetSubjectContentsInfo(object[] rParams)
        public void SetSubjectContentsInfo(object[] rParams)
        {
            Database db = null;
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
                        string[] xlstContents = rParams[1].ToString().Split('|');
                        object xobj = null;

                        xSql = " DELETE FROM T_SUBJECT_CONTENTS WHERE SUBJECT_ID = :SUBJECT_ID ";
                        xPara = new OracleParameter[1];
                        xPara[0] = base.AddParam("SUBJECT_ID", OracleType.VarChar, rParams[0]);                        
                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xPara, xTransLMS);

                        for (int i = 0; i < xlstContents.Length; i++)
                        {
                            if (xlstContents[i] != string.Empty)
                            {
                                xSql = @" INSERT INTO T_SUBJECT_CONTENTS 
                                            (SUBJECT_ID, CONTENTS_ID, CONTENTS_SEQ) 
                                            VALUES 
                                            (:SUBJECT_ID, :CONTENTS_ID
                                            , (SELECT NVL(MAX(CONTENTS_SEQ), 0) + 1 FROM T_SUBJECT_CONTENTS WHERE SUBJECT_ID = :SUBJECT_ID)
                                            ) ";

                                xPara = new OracleParameter[2];
                                xPara[0] = base.AddParam("SUBJECT_ID", OracleType.VarChar, rParams[0]);
                                xPara[1] = base.AddParam("CONTENTS_ID", OracleType.VarChar, xlstContents[i]);

                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS);

                            }
                        }

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

        }
        #endregion 

        /************************************************************
      * Function name : SetCourseSubjectInfo
      * Purpose       : 과정 & 과목 등록하는 처리
      * Input         : 
      * Output        : string
      *************************************************************/
        #region public void SetCourseSubjectInfo(object[] rParams)
        public void SetCourseSubjectInfo(object[] rParams)
        {
            Database db = null;
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
                        string[] xlstSubject = rParams[1].ToString().Split('|');
                        object xobj = null;                        

                        for(int i = 0; i<xlstSubject.Length; i++)
                        {
                            if(xlstSubject[i] != string.Empty)
                            {
                                //데이터 존재유무 확인 하여 update 또는 insert 
                                xSql = "SELECT COURSE_ID FROM T_COURSE_SUBJECT WHERE COURSE_ID = '" + rParams[0] + "' AND SUBJECT_ID = '" + xlstSubject[i] + "' ";
                                xCmdLMS.CommandText = xSql;
                                xobj = base.ExecuteScalar(db, xCmdLMS, xTransLMS);
                                if (xobj == null || xobj.ToString() == string.Empty)
                                {
                                    xSql = @"
                                        INSERT INTO T_COURSE_SUBJECT 
                                        (COURSE_ID, SUBJECT_ID, SUBJECT_SEQ) 
                                        VALUES
                                        (:COURSE_ID, :SUBJECT_ID
                                        , (SELECT NVL(MAX(SUBJECT_SEQ), 0) + 1 FROM T_COURSE_SUBJECT WHERE COURSE_ID = :COURSE_ID )
                                        ) 
                                        ";
                                    xPara = new OracleParameter[2];
                                    xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
                                    xPara[1] = base.AddParam("SUBJECT_ID", OracleType.VarChar, xlstSubject[i]);

                                    xCmdLMS.CommandText = xSql;
                                    base.Execute(db, xCmdLMS, xPara, xTransLMS);
                                }
                            }
                        }

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

        }
        #endregion 

        /************************************************************
      * Function name : SetCourseSubjectInfo_Del
      * Purpose       : 과정 & 과목 삭제하는 처리
      * Input         : 
      * Output        : string
      *************************************************************/
        #region public void SetCourseSubjectInfo_Del(object[] rParams)
        public void SetCourseSubjectInfo_Del(object[] rParams)
        {
            Database db = null;
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
                        string[] xlstSubject = rParams[1].ToString().Split('|');
                        object xobj = null;

                        for (int i = 0; i < xlstSubject.Length; i++)
                        {
                            if (xlstSubject[i] != string.Empty)
                            {

                                xSql = @" DELETE FROM T_COURSE_SUBJECT WHERE COURSE_ID = :COURSE_ID AND SUBJECT_ID = :SUBJECT_ID ";
                                xPara = new OracleParameter[2];
                                xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
                                xPara[1] = base.AddParam("SUBJECT_ID", OracleType.VarChar, xlstSubject[i]);

                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS);
                            }
                        }

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

        }
        #endregion 

        /************************************************************
      * Function name : SetCourseAssessInfo
      * Purpose       : 과정 & 평가문제 등록하는 처리
      * Input         : 
      * Output        : string
      *************************************************************/
        #region public void SetCourseAssessInfo(object[] rParams)
        public void SetCourseAssessInfo(object[] rParams)
        {
            Database db = null;
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
                        string[] xlstAssess = rParams[1].ToString().Split('|');
                        object xobj = null;

                        for (int i = 0; i < xlstAssess.Length; i++)
                        {
                            if (xlstAssess[i] != string.Empty)
                            {
                                //데이터 존재유무 확인 하여 update 또는 insert 
                                xSql = "SELECT COURSE_ID FROM T_COURSE_ASSESS_QUESTION WHERE COURSE_ID = '" + rParams[0] + "' AND QUESTION_ID = '" + xlstAssess[i] + "' ";
                                xCmdLMS.CommandText = xSql;
                                xobj = base.ExecuteScalar(db, xCmdLMS, xTransLMS);
                                if (xobj == null || xobj.ToString() == string.Empty)
                                {
                                    xSql = @"
                                        INSERT INTO T_COURSE_ASSESS_QUESTION 
                                        (COURSE_ID, QUESTION_ID, QUESTION_SEQ) 
                                        VALUES
                                        (:COURSE_ID, :QUESTION_ID
                                        , (SELECT NVL(MAX(QUESTION_SEQ), 0) + 1 FROM T_COURSE_ASSESS_QUESTION WHERE COURSE_ID = :COURSE_ID )
                                        ) 
                                        ";
                                    xPara = new OracleParameter[2];
                                    xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
                                    xPara[1] = base.AddParam("QUESTION_ID", OracleType.VarChar, xlstAssess[i]);

                                    xCmdLMS.CommandText = xSql;
                                    base.Execute(db, xCmdLMS, xPara, xTransLMS);
                                }
                            }
                        }

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

        }
        #endregion 

        /************************************************************
      * Function name : SetCourseAssessInfo_Del
      * Purpose       : 과정 & 평가문제 삭제하는 처리
      * Input         : 
      * Output        : string
      *************************************************************/
        #region public void SetCourseAssessInfo_Del(object[] rParams)
        public void SetCourseAssessInfo_Del(object[] rParams)
        {
            Database db = null;
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
                        string[] xlstAssess = rParams[1].ToString().Split('|');
                        object xobj = null;

                        for (int i = 0; i < xlstAssess.Length; i++)
                        {
                            if (xlstAssess[i] != string.Empty)
                            {

                                xSql = @" DELETE FROM T_COURSE_ASSESS_QUESTION WHERE COURSE_ID = :COURSE_ID AND QUESTION_ID = :QUESTION_ID ";
                                xPara = new OracleParameter[2];
                                xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
                                xPara[1] = base.AddParam("QUESTION_ID", OracleType.VarChar, xlstAssess[i]);

                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS);
                            }
                        }

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

        }
        #endregion 

        /************************************************************
      * Function name : SetCourseTextbookInfo
      * Purpose       : 과정 & 교재 등록하는 처리
      * Input         : 
      * Output        : string
      *************************************************************/
        #region public void SetCourseTextbookInfo(object[] rParams)
        public void SetCourseTextbookInfo(object[] rParams)
        {
            Database db = null;
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
                        string[] xlstTextbook = rParams[1].ToString().Split('|');
                        object xobj = null;

                        for (int i = 0; i < xlstTextbook.Length; i++)
                        {
                            if (xlstTextbook[i] != string.Empty)
                            {
                                //데이터 존재유무 확인 하여 update 또는 insert 
                                xSql = "SELECT COURSE_ID FROM T_COURSE_TEXTBOOK WHERE COURSE_ID = '" + rParams[0] + "' AND TEXTBOOK_ID = '" + xlstTextbook[i] + "' ";
                                xCmdLMS.CommandText = xSql;
                                xobj = base.ExecuteScalar(db, xCmdLMS, xTransLMS);
                                if (xobj == null || xobj.ToString() == string.Empty)
                                {
                                    xSql = @"
                                        INSERT INTO T_COURSE_TEXTBOOK 
                                        (COURSE_ID, TEXTBOOK_ID, TEXTBOOK_SEQ) 
                                        VALUES
                                        (:COURSE_ID, :TEXTBOOK_ID
                                        , (SELECT NVL(MAX(TEXTBOOK_SEQ), 0) + 1 FROM T_COURSE_TEXTBOOK WHERE COURSE_ID = :COURSE_ID )
                                        ) 
                                        ";
                                    xPara = new OracleParameter[2];
                                    xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
                                    xPara[1] = base.AddParam("TEXTBOOK_ID", OracleType.VarChar, xlstTextbook[i]);

                                    xCmdLMS.CommandText = xSql;
                                    base.Execute(db, xCmdLMS, xPara, xTransLMS);
                                }
                            }
                        }

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

        }
        #endregion 

        /************************************************************
      * Function name : SetCourseTextbookInfo_Del
      * Purpose       : 과정 & 교재 삭제하는 처리
      * Input         : 
      * Output        : string
      *************************************************************/
        #region public void SetCourseTextbookInfo_Del(object[] rParams)
        public void SetCourseTextbookInfo_Del(object[] rParams)
        {
            Database db = null;
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
                        string[] xlstTextbook = rParams[1].ToString().Split('|');
                        object xobj = null;

                        for (int i = 0; i < xlstTextbook.Length; i++)
                        {
                            if (xlstTextbook[i] != string.Empty)
                            {

                                xSql = @" DELETE FROM T_COURSE_TEXTBOOK WHERE COURSE_ID = :COURSE_ID AND TEXTBOOK_ID = :TEXTBOOK_ID ";
                                xPara = new OracleParameter[2];
                                xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
                                xPara[1] = base.AddParam("TEXTBOOK_ID", OracleType.VarChar, xlstTextbook[i]);

                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS);
                            }
                        }

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

        }
        #endregion 

        /************************************************************
      * Function name : SetCourseSend
      * Purpose       : 과정 발송함 
      * Input         : 
      * Output        : string
      *************************************************************/
        #region public void SetCourseSend(object[] rParams)        
        public string SetCourseSend(object[] rParams)
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
                        bool xUpdate = false; //update 할지 여부 
                        //string xQID = string.Empty;

                        if (rParams[0].ToString() != string.Empty)
                        {
                            xSql = "SELECT TEMP_SAVE_FLG FROM T_COURSE WHERE COURSE_ID = '" + rParams[0] + "' ";
                            xCmdLMS.CommandText = xSql;
                            object xobj = base.ExecuteScalar(db, xCmdLMS, xTransLMS);
                            if (xobj != null && xobj.ToString() == "Y")
                            {
                                /*OJT(000005) 일 경우 개설과정 정보 임의 생성 하여 송부 해야 함*/
                                xSql = "SELECT COURSE_TYPE FROM T_COURSE WHERE COURSE_ID = '" + rParams[0] + "' ";
                                xCmdLMS.CommandText = xSql;
                                object xType = base.ExecuteScalar(db, xCmdLMS, xTransLMS);
                                if (xType != null && xType.ToString() == "000005")
                                {
                                    xSql = @" INSERT INTO T_OPEN_COURSE 
                                    ( OPEN_COURSE_ID, COURSE_ID, COURSE_YEAR, COURSE_SEQ, EDUCATIONAL_ORG 
                                    , COURSE_BEGIN_DT, COURSE_END_DT, COURSE_BEGIN_APPLY_DT, COURSE_END_APPLY_DT
                                    , STD_PROGRESS_RATE, STD_FINAL_EXAM, PASS_SCORE
                                    , INS_ID, INS_DT, SEND_DT, SEND_FLG
                                    , USE_FLG
                                    ) VALUES (
                                    :OPEN_COURSE_ID, :COURSE_ID
                                    , TO_CHAR(SYSDATE, 'YYYY') /*COURSE_YEAR*/
                                    , (SELECT NVL(MAX(COURSE_SEQ) + 1, 1) FROM T_OPEN_COURSE WHERE COURSE_ID = :COURSE_ID)
                                    , '000007' /*한진운항훈련원*/
                                    , TO_DATE(TO_CHAR(SYSDATE, 'YYYY.MM.DD'), 'YYYY.MM.DD')
                                    , TO_DATE(TO_CHAR(SYSDATE, 'YYYY.MM.DD'), 'YYYY.MM.DD')
                                    , TO_DATE(TO_CHAR(SYSDATE, 'YYYY.MM.DD'), 'YYYY.MM.DD')
                                    , TO_DATE(TO_CHAR(SYSDATE, 'YYYY.MM.DD'), 'YYYY.MM.DD')
                                    , 100, 0, 0 
                                    , :INS_ID, SYSDATE, :SEND_DT, :SEND_FLG
                                    , 'Y'
                                    ) ";

                                    vp_l_common_md com = new vp_l_common_md();
                                    string xID = com.GetMaxIDOfTable(new string[] { "T_OPEN_COURSE", "OPEN_COURSE_ID" });

                                    xPara = new OracleParameter[5];
                                    xPara[0] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, xID);
                                    xPara[1] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
                                    xPara[2] = base.AddParam("INS_ID", OracleType.VarChar, rParams[1]);
                                    //xPara[11] = base.AddParam("INS_DT", OracleType.VarChar, rParams[11]);
                                    xPara[3] = base.AddParam("SEND_DT", OracleType.VarChar, DBNull.Value);
                                    xPara[4] = base.AddParam("SEND_FLG", OracleType.VarChar, "1");

                                    xCmdLMS.CommandText = xSql;
                                    base.Execute(db, xCmdLMS, xPara, xTransLMS);
                                }

                                /*
                                 * < 발송하는 정보 >
                                 * T_COURSE 
                                 * T_SUBJECT
                                 * T_CONTENTS
                                 * T_ASSESS_QUESTION 
                                 * T_TEXTBOOK
                                 * 
                                 * T_COURSE_SUBJECT
                                 * T_SUBJECT_CONTENTS
                                 * T_COURSE_ASSESS_QUESTION
                                 * T_COURSE_TEXTBOOK
                                 * 
                                 * 
                                 * < TEMP_SAVE_FLG UPDATE >
                                 * T_COURSE
                                 * T_SUBJECT
                                 * T_CONTENTS
                                 * T_ASSESS_QUESTION
                                 * T_TEXTBOOK
                                 * 
                                 */

                                //++++++++++++++++++++++++++++++++++++++++++++++++
                                // SEND_FLG UPDATE 
                                //++++++++++++++++++++++++++++++++++++++++++++++++
                                //1. T_COURSE
                                //course의 경우는 임시저장 하면 send_flg를 0으로 변경 함
                                xSql = @" UPDATE T_COURSE SET TEMP_SAVE_FLG = 'N', SEND_FLG = '1' WHERE COURSE_ID = :COURSE_ID ";
                                xPara = new OracleParameter[1];
                                xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS);

                                //2. T_SUBJECT 
                                xSql = @" UPDATE T_SUBJECT SET TEMP_SAVE_FLG = 'N' -- , SEND_FLG = '1'
                                               WHERE SUBJECT_ID IN (SELECT SUBJECT_ID FROM T_COURSE_SUBJECT WHERE COURSE_ID = :COURSE_ID)   ";
                                xPara = new OracleParameter[1];
                                xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS);

                                //3. T_CONTENTS
                                xSql = @" UPDATE T_CONTENTS SET TEMP_SAVE_FLG = 'N' --, SEND_FLG = '1'
                                                WHERE CONTENTS_ID IN (SELECT S.CONTENTS_ID FROM T_COURSE_SUBJECT C, T_SUBJECT_CONTENTS S 
                                                                                    WHERE C.SUBJECT_ID = S.SUBJECT_ID AND C.COURSE_ID = :COURSE_ID )     ";

                                xPara = new OracleParameter[1];
                                xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS);

                                //4. T_ASSESS_QUESTION
                                xSql = @" UPDATE T_ASSESS_QUESTION SET TEMP_SAVE_FLG = 'N' --, SEND_FLG = '1'
                                                WHERE QUESTION_ID IN (SELECT QUESTION_ID FROM T_COURSE_ASSESS_QUESTION WHERE COURSE_ID = :COURSE_ID)   ";
                                xPara = new OracleParameter[1];
                                xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS);

                                //5. T_TEXTBOOK
                                xSql = @" UPDATE T_TEXTBOOK SET TEMP_SAVE_FLG = 'N' --, SEND_FLG = '1'
                                                WHERE TEXTBOOK_ID IN (SELECT TEXTBOOK_ID FROM T_COURSE_TEXTBOOK WHERE COURSE_ID = :COURSE_ID)   ";
                                xPara = new OracleParameter[1];
                                xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS);



                                //++++++++++++++++++++++++++++++++++++++++++++++++
                                // 선박 송부 : 프로시져 호출 (T_COURSE는 젤 마지막에!! )
                                //++++++++++++++++++++++++++++++++++++++++++++++++
                                OracleParameter[] oOraParams = null; 

                                // 1. T_COURSE_SUBJECT
                                oOraParams = new OracleParameter[2];
                                oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_COURSE_SUBJECT");
                                oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_COURSE_SUBJECT");
                                base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);

                                // 2. T_SUBJECT_CONTENTS
                                oOraParams = new OracleParameter[2];
                                oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_SUBJECT_CONTENTS");
                                oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_SUBJECT_CONTENTS");
                                base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);

                                // 3. T_COURSE_TEXTBOOK
                                oOraParams = new OracleParameter[2];
                                oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_COURSE_TEXTBOOK");
                                oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_COURSE_TEXTBOOK");
                                base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);

                                // 4. T_COURSE_ASSESS_QUESTION
                                oOraParams = new OracleParameter[2];
                                oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_COURSE_ASSESS_QUESTION");
                                oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_COURSE_ASSESS_QUESTION");
                                base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);

                                // 5. T_SUBJECT
                                oOraParams = new OracleParameter[2];
                                oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_SUBJECT");
                                oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_SUBJECT");
                                base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);

                                // 6. T_CONTENTS
                                oOraParams = new OracleParameter[2];
                                oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_CONTENTS");
                                oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_CONTENTS");
                                base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);

                                // 7. T_TEXTBOOK
                                oOraParams = new OracleParameter[2];
                                oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_TEXTBOOK");
                                oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_TEXTBOOK");
                                base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);

                                // 8. T_ASSESS_QEUSTION
                                oOraParams = new OracleParameter[2];
                                oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_ASSESS_QUESTION");
                                oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_ASSESS_QUESTION");
                                base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);

                                // 9. T_COURSE
                                oOraParams = new OracleParameter[2];
                                oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_COURSE");
                                oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_COURSE");
                                base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);

                                

                                ////temp_save_flg = 'N'으로 변경 
                                //xSql = @" UPDATE T_COURSE SET TEMP_SAVE_FLG = 'N' WHERE COURSE_ID = :COURSE_ID ";
                                //xPara = new OracleParameter[1];
                                //xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
                                ////xPara[1] = base.AddParam("INS_ID", OracleType.VarChar, rParams[1]);
                                //xCmdLMS.CommandText = xSql;
                                //base.Execute(db, xCmdLMS, xPara, xTransLMS);   


                                

                                                             

//                                //T_SUBJECT
//                                xSql = @" UPDATE T_SUBJECT SET TEMP_SAVE_FLG = 'N', SEND_FLG = '1', INS_DT = SYSDATE, INS_ID = :INS_ID 
//                                                WHERE SUBJECT_ID IN (SELECT SUBJECT_ID FROM T_COURSE_SUBJECT WHERE COURSE_ID = :COURSE_ID)   ";
//                                xPara = new OracleParameter[2];
//                                xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
//                                xPara[1] = base.AddParam("INS_ID", OracleType.VarChar, rParams[1]);
//                                xCmdLMS.CommandText = xSql;
//                                base.Execute(db, xCmdLMS, xPara, xTransLMS);

//                                //T_CONTENTS 
//                                xSql = @" UPDATE T_CONTENTS SET TEMP_SAVE_FLG = 'N', SEND_FLG = '1', INS_DT = SYSDATE, INS_ID = :INS_ID 
//                                                WHERE CONTENTS_ID IN (SELECT S.CONTENTS_ID FROM T_COURSE_SUBJECT C, T_SUBJECT_CONTENTS S 
//                                                                                    WHERE C.SUBJECT_ID = S.SUBJECT_ID AND C.COURSE_ID = :COURSE_ID )     ";
//                                xPara = new OracleParameter[2];
//                                xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
//                                xPara[1] = base.AddParam("INS_ID", OracleType.VarChar, rParams[1]);
//                                xCmdLMS.CommandText = xSql;
//                                base.Execute(db, xCmdLMS, xPara, xTransLMS);

//                                //T_ASSESS_QUESTION
//                                xSql = @" UPDATE T_ASSESS_QUESTION SET TEMP_SAVE_FLG = 'N', SEND_FLG = '1', INS_DT = SYSDATE, INS_ID = :INS_ID 
//                                                WHERE QUESTION_ID IN (SELECT QUESTION_ID FROM T_COURSE_ASSESS_QUESTION WHERE COURSE_ID = :COURSE_ID)   ";
//                                xPara = new OracleParameter[2];
//                                xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
//                                xPara[1] = base.AddParam("INS_ID", OracleType.VarChar, rParams[1]);
//                                xCmdLMS.CommandText = xSql;
//                                base.Execute(db, xCmdLMS, xPara, xTransLMS);

//                                //T_TEXTBOOK
//                                xSql = @" UPDATE T_TEXTBOOK SET TEMP_SAVE_FLG = 'N', SEND_FLG = '1', INS_DT = SYSDATE, INS_ID = :INS_ID 
//                                                WHERE TEXTBOOK_ID IN (SELECT TEXTBOOK_ID FROM T_COURSE_TEXTBOOK WHERE COURSE_ID = :COURSE_ID)   ";
//                                xPara = new OracleParameter[2];
//                                xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
//                                xPara[1] = base.AddParam("INS_ID", OracleType.VarChar, rParams[1]);
//                                xCmdLMS.CommandText = xSql;
//                                base.Execute(db, xCmdLMS, xPara, xTransLMS);

                                xRtn = "Y"; 

                            }
                        }
                        else
                        {
                            xRtn = "N"; 
                        }

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
       * Function name : GetCourseSubjectList
       * Purpose       : course subject list 조회   
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        #region public DataTable GetCourseSubjectList(string rCourseId)
        public DataTable GetCourseSubjectList(string rCourseId)
        {
            try
            {
                string xSql = string.Empty;

                xSql = @" 
                        SELECT S.SUBJECT_ID || ' | ' || S.SUBJECT_NM  AS SUBJECT_NM
                        FROM T_COURSE_SUBJECT C, T_SUBJECT S
                        WHERE S.SUBJECT_ID = C.SUBJECT_ID 
                            AND C.COURSE_ID = '" + rCourseId + "' ";
                xSql += " ORDER BY C.SUBJECT_SEQ ";

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
      * Function name : SetCourseSubjectSortInfo
      * Purpose       : 과정 & 과목 Sort 등록하는 처리
      * Input         : 
      * Output        : string
      *************************************************************/
        #region public void SetCourseSubjectSortInfo(object[] rParams)
        public void SetCourseSubjectSortInfo(object[] rParams)
        {
            Database db = null;
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
                        string[] xlstSubject = rParams[1].ToString().Split('|');
                        object xobj = null;

                        for (int i = 0; i < xlstSubject.Length; i++)
                        {
                            if (xlstSubject[i] != string.Empty)
                            {
                                //데이터 존재유무 확인 하여 update 
                                xSql = "SELECT SUBJECT_ID FROM T_COURSE_SUBJECT WHERE COURSE_ID = '" + rParams[0] + "' AND SUBJECT_ID = '" + xlstSubject[i] + "' ";
                                xCmdLMS.CommandText = xSql;
                                xobj = base.ExecuteScalar(db, xCmdLMS, xTransLMS);

                                if (xobj != null && xobj.ToString() != string.Empty)
                                {
                                    xSql = @"
                                        UPDATE T_COURSE_SUBJECT 
                                        SET SUBJECT_SEQ = :SUBJECT_SEQ
                                        WHERE COURSE_ID = :COURSE_ID
                                            AND SUBJECT_ID = :SUBJECT_ID 
                                        ";
                                    xPara = new OracleParameter[3];
                                    xPara[0] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[0]);
                                    xPara[1] = base.AddParam("SUBJECT_ID", OracleType.VarChar, xlstSubject[i]);
                                    xPara[2] = base.AddParam("SUBJECT_SEQ", OracleType.Number, (i+1));

                                    xCmdLMS.CommandText = xSql;
                                    base.Execute(db, xCmdLMS, xPara, xTransLMS);
 
                                }                              
                            }
                        }

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

        }
        #endregion 


    }
}
