using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using CLT.WEB.BIZ.FX.BIZBASE;
using System.Data;
using System.Data.OracleClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using CLT.WEB.UI.FX.UTIL;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Web;

namespace CLT.WEB.BIZ.LMS.EDUM
{
    /// <summary>
    /// 1. 작업개요 : 
    /// 
    /// 2. 주요기능 : 
    ///				  
    /// 3. Class 명 : vp_a_edumng_md
    /// 
    /// 4. 작 업 자 : 
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_a_edumng_md
    ///        * Comment 
    ///          * Comment 
    ///          Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제따른 변경
    ///          교육 대상자 선발 보완
    /// 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        * 김은정 2012.08.08
    ///        * Source
    ///          vp_a_edumng_md
    ///        * Comment 
    ///          교육 대상자 선발 시 타사 교육이력 대상자도 포함하여 선정 / 직책, 이름, 사번 정렬 적용 - GetEduListNew
    ///          교육 결과 조회 시 직책, 이름, 사전 정렬 적용 - GetEduResultList, GetOJTResultList, GetEduResultNonPassList
    /// 
    ///    [CHM-201218484] 국토해양부 로고 추가 및 데이터 정렬 기준 추가
    ///        * 김은정 2012.09.14
    ///        * Source
    ///          vp_a_edumng_md
    ///        * Comment 
    ///          교육 대상자 선발 시 자사+타사 교육 이력 조회 시 기존 현재사번이 아닌 공통사번을 기준으로 대상자 제외하도록 변경    
    ///          교육 이수증발급 시 국토해양부 과정여부 체크하여 국토해양부 로그 표시되어 출력되도록 변경    
    /// 
    /// </summary>
    public class vp_a_edumng_md : DAC
    {
        /************************************************************
        * Function name : GetShipSchoolResultList
        * Purpose       : 등록과정 전체 목록을 전달하는 처리
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetShipSchoolResultList(string[] rParams, CultureInfo rArgCultureInfo)
        {
            DataTable xDt = null;
            try
            {
                string xSql = "";
                xSql += @"   select *
                               from (
                                select  rownum rnum ";
                xSql += "               , a.* " + "\r\n";
                xSql += "    from ( " + "\r\n";
                xSql += "            select substr(s.shipschool_cd, 0, 4) as ship_code " + "\r\n";
                xSql += "                   , s.shipschool_cd as keys " + "\r\n";
                xSql += "                   , s.shipschool_cd " + "\r\n";
                xSql += "                   , s.shipschool_type " + "\r\n";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += "                   , c.d_knm as CLASSIFICATION   --분류 " + "\r\n";
                }
                else
                {
                    xSql += "                   , c.d_enm as CLASSIFICATION   --분류 " + "\r\n";
                }
                xSql += "                   , s.shipschool_nm             --강의제목 " + "\r\n";
                xSql += "                   , s.shipschool_seq            --차수 " + "\r\n";
                xSql += "                   , to_char(s.learning_dt, 'YYYY.MM.DD') as learning_dt               --교육일자 " + "\r\n";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += "                   , (select d_knm from t_code_detail where m_cd='0040' and d_cd = s.learning_hours) as learning_hours  --교육시간 " + "\r\n";
                    xSql += "                   , d.duty_work_name            --강사직책 " + "\r\n";
                    xSql += "                   , s.lecturer_nm               --강사네임 " + "\r\n";
                }
                else
                {
                    xSql += "                   , (select d_enm from t_code_detail where m_cd='0040' and d_cd = s.learning_hours) as learning_hours  --교육시간 " + "\r\n";
                    xSql += "                   , d.duty_work_ename as duty_work_name           --강사직책 " + "\r\n";
                    xSql += "                   , (SELECT USER_NM_ENG_FIRST || ' ' || USER_NM_ENG_LAST  FROM T_USER WHERE USER_ID = s.lecturer_id) AS lecturer_nm        --강사네임 " + "\r\n";
                }

                xSql += "                   , s.lecturer_id               --사번 " + "\r\n";
                xSql += "                   , s.capt_nm                   --선장이름 " + "\r\n";
                xSql += "                   , s.ce_nm                     --기관장이름 " + "\r\n";
                xSql += "                   , s.attach_file_path          --파일패스 " + "\r\n";
                xSql += "                   , s.lecturer_pay_yn           --승인여부 " + "\r\n";
                xSql += "                   , s.remark                    --비고 " + "\r\n";
                xSql += "                   , s.send_flg                  --비고 " + "\r\n";
                xSql += "                   , (SELECT count(*) from t_shipschool_result r, t_user u where r.user_id=u.user_id and r.shipschool_cd=s.shipschool_cd) as class_man_count  " + "\r\n";
                xSql += "                   , count(*) over() totalrecordcount " + "\r\n";
                xSql += "              FROM t_shipschool s, (select * from t_code_detail where m_cd = '0039') c, V_HDUTYWORK d   " + "\r\n";
                xSql += "             WHERE 1=1 " + "\r\n";
                xSql += "                   and s.shipschool_type= c.d_cd " + "\r\n";
                xSql += "                   and s.lecturer_duty_work = d.duty_work(+)   " + "\r\n";

                // Learning Dt
                xSql += " AND s.learning_dt >= to_date('" + Convert.ToString(rParams[2]) + "', 'yyyy.mm.dd') ";
                xSql += " AND s.learning_dt <= to_date('" + Convert.ToString(rParams[3]) + "', 'yyyy.mm.dd') ";

                // 분류
                if (Convert.ToString(rParams[4]) != "*")
                    xSql += @" AND c.d_cd = '" + Convert.ToString(rParams[4]) + "' " + "\r\n";

                // Ship Code
                if (Convert.ToString(rParams[5]) != "*")
                    xSql += " AND s.shipschool_cd like '" + Convert.ToString(rParams[5]) + "%' " + "\r\n";

                // 직책
                if (Convert.ToString(rParams[6]) != "*")
                    xSql += " AND s.lecturer_duty_work = '" + Convert.ToString(rParams[6]) + "' " + "\r\n";

                // 강의제목
                if (Convert.ToString(rParams[7]).Trim() != "")
                    xSql += " AND UPPER(s.shipschool_nm) like '%" + Convert.ToString(rParams[7]).ToUpper() + "%' " + "\r\n";

                // 강의료승인상태

                if (Convert.ToString(rParams[8]) != "*")
                {
                    if (Convert.ToString(rParams[8]) == "Y")
                        xSql += " AND s.lecturer_pay_yn = '" + Convert.ToString(rParams[8]) + "' ";
                    else
                        xSql += " AND (s.lecturer_pay_yn = '" + Convert.ToString(rParams[8]) + "' or s.lecturer_pay_yn is null)";
                }
                xSql += @" ORDER BY s.learning_dt desc, s.shipschool_nm " + "\r\n";
                xSql += "          ) a " + "\r\n";
                xSql += " ) ";

                xSql += string.Format(" WHERE 1=1 " + "\r\n");

                if (!String.IsNullOrEmpty(Convert.ToString(rParams[0])) && !String.IsNullOrEmpty(Convert.ToString(rParams[1])))
                {
                    xSql += string.Format(" AND rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                    xSql += string.Format(" AND rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
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
        * Function name : GetShipSchoolResult
        * Purpose       : 등록과정 전체 목록을 전달하는 처리
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetShipSchoolResult(string[] rParams, CultureInfo rArgCultureInfo)
        {
            DataTable xDt = null;
            try
            {
                string xSql = "";
                xSql += @"   
                          SELECT S.SHIPSCHOOL_CD
                                , TO_CHAR(S.LEARNING_DT, 'YYYY.MM.DD') AS LEARING_DT
                                , (select d_knm from t_code_detail where m_cd='0040' and d_cd = s.learning_hours) as LEARNING_HOURS
                                , DECODE(S.LECTURER_PAY_YN, 'Y', 'Y', 'N') AS LECTURER_PAY_YN ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"  , S.LECTURER_NM
                                , (SELECT STEP_NAME FROM V_HDUTYSTEP WHERE ROWNUM=1 AND DUTY_WORK = S.LECTURER_DUTY_WORK) AS LECTURER_DUTY_WORK
                                , R.USER_NM ";
                }
                else
                {
                    xSql += @"  , (SELECT USER_NM_ENG_FIRST || ' ' || USER_NM_ENG_LAST  FROM T_USER WHERE USER_ID = s.lecturer_id) AS LECTURER_NM    
                                , (SELECT STEP_ENAME FROM V_HDUTYSTEP WHERE ROWNUM=1 AND DUTY_WORK = S.LECTURER_DUTY_WORK) AS LECTURER_DUTY_WORK
                                , (SELECT USER_NM_ENG_FIRST || ' ' || USER_NM_ENG_LAST FROM T_USER WHERE USER_ID = R.USER_ID) USER_NM ";
                }
                xSql += @"      , R.USER_DUTY_STEP ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"  , W.DUTY_WORK_NAME AS DUTY_WORK_ENAME ";
                }
                else
                {
                    xSql += @"  , W.DUTY_WORK_ENAME AS DUTY_WORK_ENAME";
                }
                xSql += @"      , S.SHIPSCHOOL_TYPE ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"  , (select D_KNM from t_code_detail where m_cd='0039' AND d_cd=s.shipschool_type) AS SHIPSCHOOL_TYPE_NM ";
                }
                else
                {
                    xSql += @"  , (select D_ENM from t_code_detail where m_cd='0039' AND d_cd=s.shipschool_type) AS SHIPSCHOOL_TYPE_NM ";
                }
                xSql += @"      , S.SHIPSCHOOL_NM
                                , S.LEARNING_DESC
                                , S.attach_file_path
                                , S.ATTACH_FILE
                                , S.REMARK
                           FROM T_SHIPSCHOOL S
                                JOIN T_SHIPSCHOOL_RESULT R
                                ON S.SHIPSCHOOL_CD = R.SHIPSCHOOL_CD
                                JOIN T_USER U
                                ON R.USER_ID = U.USER_ID
                                LEFT JOIN V_HDUTYWORK W
                                ON R.USER_DUTY_STEP = W.DUTY_WORK
                         WHERE  S.SHIPSCHOOL_CD = '" + rParams[0] + @"'
                        " + "\r\n";
                xSql += " ORDER BY W.WORK_SEQ, R.USER_NM " + "\r\n";

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
        * Function name : SetShipSchoolResultList
        * Purpose       : 등록과정 전체 목록을 전달하는 처리
        * Input         : DataTable
        * Output        : DataTable
        *************************************************************/
        public string SetShipSchoolResultList(DataTable rDt)
        {
            string xSql = string.Empty;
            string xRtn = Boolean.FalseString;

            try
            {
                Database db = null;

                try
                {
                    db = base.GetDataBase("LMS");
                    using (OracleConnection cnn = (OracleConnection)db.CreateConnection())
                    {
                        cnn.Open();
                        OracleTransaction trx = cnn.BeginTransaction();
                        OracleCommand cmd = null;
                        OracleParameter[] xPara = null;
                        try
                        {
                            cmd = base.GetSqlCommand(db);

                            foreach (DataRow drDet in rDt.Rows)
                            {
                                xSql = @" 
                                UPDATE t_shipschool
                                   SET lecturer_pay_yn = :lecturer_pay_yn
                                        , upt_id = :upt_id
                                        , upt_dt = sysdate
                                        , remark = :remark
                                        , send_flg = '1'
                                 WHERE shipschool_cd = :shipschool_cd ";

                                xPara = new OracleParameter[4];
                                xPara[0] = base.AddParam("lecturer_pay_yn", OracleType.VarChar, Convert.ToString(drDet["lecturer_pay_yn"]));
                                xPara[1] = base.AddParam("upt_id", OracleType.VarChar, Convert.ToString(drDet["upt_id"]));
                                xPara[2] = base.AddParam("remark", OracleType.VarChar, Convert.ToString(drDet["remark"]));
                                xPara[3] = base.AddParam("shipschool_cd", OracleType.VarChar, Convert.ToString(drDet["shipschool_cd"]));
                                cmd.CommandText = xSql;
                                base.Execute(db, cmd, xPara, trx);
                                cmd.Parameters.Clear();
                            }

                            //선박발송
                            xPara = new OracleParameter[2];
                            xPara[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_SHIPSCHOOL");
                            xPara[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_SHIPSCHOOL");
                            base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", xPara, trx);

                            xRtn = Boolean.TrueString;
                            trx.Commit();
                        }
                        catch (Exception ex)
                        {
                            trx.Rollback();

                            bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                            if (rethrow) throw;
                        }
                        finally
                        {
                            if (cmd != null)
                                cmd.Dispose();
                            if (cnn != null)
                            {
                                if (cnn.State == ConnectionState.Open)
                                {
                                    cnn.Close();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                    if (rethrow) throw;
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
        * Function name : GetEduResultList
        * Purpose       : 수료현황 조회
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetEduResultList(DataTable rDt, CultureInfo rArgCultureInfo)
        {
            DataRow rRow = rDt.Rows[0];
            DataTable xDt = null;

            try
            {
                string xSql = "";
                xSql += @"   select *
                               from (
                                select  rownum rnum ";
                xSql += "               , a.* " + "\r\n";
                xSql += "    from ( " + "\r\n";
                xSql += "           SELECT   " + "\r\n";
                xSql += @"                  
                                            c.course_id ||'^'|| o.open_course_id||'^'|| r.course_result_seq||'^'|| u.user_id as KEYS
                                            , c.course_id
                                            , o.open_course_id
                                            , r.course_result_seq
                                            , u.user_id
                                            , u.user_no
                                            , u.duty_step  ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"              , (select step_name from v_hdutystep where duty_step = u.duty_step) as step_name
                                            , u.user_nm_kor ";
                }
                else
                {
                    xSql += @"              , (select step_ename from v_hdutystep where duty_step = u.duty_step) as step_name
                                            , u.user_nm_eng_first || ' ' || u.user_nm_eng_last as user_nm_kor ";
                }
                xSql += @"                  , u.user_nm_eng_first
                                            , u.user_nm_eng_last ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"              , c.course_nm ";
                }
                else
                {
                    xSql += @"              , c.COURSE_NM_ABBR course_nm ";
                }
                xSql += @"                  , o.course_begin_dt
                                            , o.course_end_dt
                                            , case 
                                                when r.user_course_begin_dt is not null then (to_char(r.user_course_begin_dt,'yyyy.mm.dd') ||' ~ '|| to_char(r.user_course_end_dt,'yyyy.mm.dd')) 
                                                else (to_char(o.course_begin_dt,'yyyy.mm.dd') ||' ~ '|| to_char(o.course_end_dt,'yyyy.mm.dd'))    
                                              end                                                                                               as course_dt

                                            --, (to_char(o.course_begin_dt,'yyyy.mm.dd') ||' ~ '|| to_char(o.course_end_dt,'yyyy.mm.dd')) as course_dt
                                            , c.expired_period
                                            , r.pass_flg 
                                            , r.non_approval_cd  ";//    --교육불가사유코드
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"              , (select d_knm from t_code_detail where m_cd='0011' and d_cd = r.non_approval_cd) as non_approval_nm ";//  --교육불가사유 
                }
                else
                {
                    xSql += @"              , (select d_enm from t_code_detail where m_cd='0011' and d_cd = r.non_approval_cd) as non_approval_nm ";//  --교육불가사유 
                }
                xSql += @"                  , r.non_approval_remark --교육불가사유
                                            , r.non_pass_cd ";//        --미이수사유코드
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"              , (select d_knm from t_code_detail where m_cd='0050' and d_cd = r.non_pass_cd) as non_pass_nm ";//   --미이수사유

                }
                else
                {
                    xSql += @"              , (select d_enm from t_code_detail where m_cd='0050' and d_cd = r.non_pass_cd) as non_pass_nm ";//   --미이수사유

                }

                xSql += @"                            , r.non_pass_remark 
                                            ";
                xSql += "                   , count(*) over() totalrecordcount " + "\r\n";
                //교육기관 코드
                xSql += "                   , O.EDUCATIONAL_ORG ";
                //주민번호
                xSql += "                   , TRIM(REPLACE(U.PERSONAL_NO,'-','')) PERSONAL_NO ";

                xSql += "                   , O.course_seq ";
                xSql += "                   , p.company_nm ";
                xSql += @"            FROM  t_open_course o
                                            , t_course c
                                            , (select * from t_course_result where APPROVAL_FLG = '000001' and PASS_FLG = '000001') r --수료현황
                                            , t_user u   
                                            , t_company p   " + "\r\n";
                xSql += @"            WHERE o.course_id = c.course_id
                                            and o.open_course_id = r.open_course_id
                                            and u.company_id = p.company_id(+)
                                            and r.user_id = u.user_id " + "\r\n";

                //교육기간
                xSql += " AND o.course_begin_dt >= to_date('" + Convert.ToString(rRow["start_dt"]) + "', 'yyyy.mm.dd') " + "\r\n";
                xSql += " AND o.course_begin_dt <= to_date('" + Convert.ToString(rRow["end_dt"]) + "', 'yyyy.mm.dd') " + "\r\n";

                // 교육유형
                if (Convert.ToString(rRow["course_type"]) != "*")
                    xSql += @" AND c.course_type = '" + Convert.ToString(rRow["course_type"]) + "' " + "\r\n";

                //// 과정명


                if (Convert.ToString(rRow["course_nm"]).Trim() != "")
                {
                    if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                    {
                        xSql += " AND UPPER(c.course_nm) like '%" + Convert.ToString(rRow["course_nm"]).ToUpper() + "%' " + "\r\n";
                    }
                    else
                    {
                        xSql += " AND UPPER(c.COURSE_NM_ABBR) like '%" + Convert.ToString(rRow["course_nm"]).ToUpper() + "%' " + "\r\n";
                    }
                }

                // 교육불가사유
                if (rRow.Table.Columns.Contains("non_approval_cd") && Convert.ToString(rRow["non_approval_cd"]) != "*")
                    xSql += @" AND r.non_approval_cd = '" + Convert.ToString(rRow["non_approval_cd"]) + "' " + "\r\n";

                // 미이수사유

                if (rRow.Table.Columns.Contains("non_pass_cd") && Convert.ToString(rRow["non_pass_cd"]) != "*")
                    xSql += @" AND r.non_pass_cd = '" + Convert.ToString(rRow["non_pass_cd"]) + "' " + "\r\n";

                //xSql += "          ORDER BY o.course_begin_dt desc, u.user_nm_kor  " + "\r\n";
                xSql += "            ORDER BY NVL(r.upt_dt, r.ins_dt) desc, DUTY_STEP, USER_NM_KOR, USER_ID " + "\r\n";
                xSql += "          ) a " + "\r\n";
                xSql += "          )  " + "\r\n";
                xSql += string.Format(" WHERE 1=1 " + "\r\n");

                if (!String.IsNullOrEmpty(Convert.ToString(rRow["start_rnum"])) && !String.IsNullOrEmpty(Convert.ToString(rRow["end_rnum"])))
                {
                    xSql += string.Format(" AND rnum > {0} " + "\r\n", Convert.ToInt32(rRow["start_rnum"]) * (Convert.ToInt32(rRow["end_rnum"]) - 1));
                    xSql += string.Format(" AND rnum <= {0} " + "\r\n", Convert.ToInt32(rRow["start_rnum"]) * Convert.ToInt32(rRow["end_rnum"]));
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
        * Function name : GetEduResultList
        * Purpose       : 미이수수료현황 조회
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetEduResultNonPassList(DataTable rDt, CultureInfo rArgCultureInfo)
        {
            DataRow rRow = rDt.Rows[0];
            DataTable xDt = null;

            try
            {
                string xSql = "";
                xSql += @"   select *
                               from (
                                select  rownum rnum ";
                xSql += "               , a.* " + "\r\n";
                xSql += "    from ( " + "\r\n";
                xSql += "           SELECT   " + "\r\n";
                xSql += @"                  
                                            c.course_id ||'^'|| o.open_course_id||'^'|| r.course_result_seq||'^'|| u.user_id as KEYS
                                            , c.course_id
                                            , o.open_course_id
                                            , r.course_result_seq
                                            , u.user_id
                                            , u.user_no
                                            , u.duty_step ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"              , (select step_name from v_hdutystep where duty_step = u.duty_step) as step_name
                                            , u.user_nm_kor ";
                }
                else
                {
                    xSql += @"              , (select step_ename from v_hdutystep where duty_step = u.duty_step) as step_name
                                            ,  U.USER_NM_ENG_FIRST || ' ' || U.USER_NM_ENG_LAST AS USER_NM_KOR ";
                }
                xSql += @"                  , u.user_nm_eng_first
                                            , u.user_nm_eng_last ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"      , C.COURSE_NM ";
                }
                else
                {
                    xSql += @"      , C.COURSE_NM_ABBR AS COURSE_NM ";

                }
                xSql += @"                  , o.course_begin_dt
                                            , o.course_end_dt
                                            , case 
                                                when r.user_course_begin_dt is not null then (to_char(r.user_course_begin_dt,'yyyy.mm.dd') ||'~'|| to_char(r.user_course_end_dt,'yyyy.mm.dd')) 
                                                else (to_char(o.course_begin_dt,'yyyy.mm.dd') ||'~'|| to_char(o.course_end_dt,'yyyy.mm.dd'))    
                                              end                                                                                               as course_dt
                                          --  , (to_char(o.course_begin_dt,'yyyy.mm.dd') ||'~'|| to_char(o.course_end_dt,'yyyy.mm.dd')) as course_dt
                                            , c.expired_period
                                            , r.pass_flg 
                                            , r.non_approval_cd ";//    --교육불가사유코드 
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"              , (select d_knm from t_code_detail where m_cd='0011' and d_cd = r.non_approval_cd) as non_approval_nm ";//  --교육불가사유 
                }
                else
                {
                    xSql += @"              , (select d_enm from t_code_detail where m_cd='0011' and d_cd = r.non_approval_cd) as non_approval_nm ";//  --교육불가사유 
                }
                xSql += @"                  , r.non_approval_remark --교육불가사유 
                                            , r.non_pass_cd ";//        --미이수사유코드 
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"              , (select d_knm from t_code_detail where m_cd='0050' and d_cd = r.non_pass_cd) as non_pass_nm ";//   --미이수사유  
                }
                else
                {
                    xSql += @"              , (select d_enm from t_code_detail where m_cd='0050' and d_cd = r.non_pass_cd) as non_pass_nm ";//   --미이수사유  
                }
                xSql += @"                  , r.non_pass_remark 
                                            , o.course_seq
                                            ";
                xSql += "                   , count(*) over() totalrecordcount " + "\r\n";

                // --미확정

                //--미승인,과정취소,본인취소
                //--재시험,재재시험,미이수

                xSql += @"            FROM  t_open_course o
                                            , t_course c
                                            , ( select * 
                                                 from t_course_result 
                                                where CONFIRM='0'
                                                      OR APPROVAL_FLG IN ('000002','000004','000005') 
                                                      OR PASS_FLG IN ('000002','000003','000005') 
                                              ) r --미수료현황
                                            , t_user u   " + "\r\n";
                xSql += @"            WHERE o.course_id = c.course_id
                                            and o.open_course_id = r.open_course_id
                                            and r.user_id = u.user_id " + "\r\n";

                //교육기간
                xSql += " AND o.course_begin_dt >= to_date('" + Convert.ToString(rRow["start_dt"]) + "', 'yyyy.mm.dd') " + "\r\n";
                xSql += " AND o.course_begin_dt <= to_date('" + Convert.ToString(rRow["end_dt"]) + "', 'yyyy.mm.dd') " + "\r\n";

                // 교육유형
                if (Convert.ToString(rRow["course_type"]) != "*")
                    xSql += @" AND c.course_type = '" + Convert.ToString(rRow["course_type"]) + "' " + "\r\n";

                //// 과정명


                if (Convert.ToString(rRow["course_nm"]).Trim() != "")
                {
                    if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                    {
                        xSql += " AND UPPER(c.course_nm) like '%" + Convert.ToString(rRow["course_nm"]).ToUpper() + "%' " + "\r\n";
                    }
                    else
                    {
                        xSql += " AND UPPER(c.COURSE_NM_ABBR) like '%" + Convert.ToString(rRow["course_nm"]).ToUpper() + "%' " + "\r\n";
                    }
                }
                // 교육불가사유
                if (rRow.Table.Columns.Contains("non_approval_cd") && Convert.ToString(rRow["non_approval_cd"]) != "*")
                    xSql += @" AND r.non_approval_cd = '" + Convert.ToString(rRow["non_approval_cd"]) + "' " + "\r\n";

                // 미이수사유

                if (rRow.Table.Columns.Contains("non_pass_cd") && Convert.ToString(rRow["non_pass_cd"]) != "*")
                    xSql += @" AND r.non_pass_cd = '" + Convert.ToString(rRow["non_pass_cd"]) + "' " + "\r\n";

                //xSql += "          ORDER BY o.course_begin_dt desc, u.user_nm_kor  " + "\r\n";
                xSql += "           ORDER BY DUTY_STEP, USER_NM_KOR, USER_ID " + "\r\n";
                xSql += "          ) a " + "\r\n";
                xSql += "   )   " + "\r\n";
                xSql += string.Format(" WHERE 1=1 " + "\r\n");

                if (!String.IsNullOrEmpty(Convert.ToString(rRow["start_rnum"])) && !String.IsNullOrEmpty(Convert.ToString(rRow["end_rnum"])))
                {
                    xSql += string.Format(" AND rnum > {0} " + "\r\n", Convert.ToInt32(rRow["start_rnum"]) * (Convert.ToInt32(rRow["end_rnum"]) - 1));
                    xSql += string.Format(" AND rnum <= {0} " + "\r\n", Convert.ToInt32(rRow["start_rnum"]) * Convert.ToInt32(rRow["end_rnum"]));
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
        * Function name : GetEduSelectList
        * Purpose       : 교육과정 리스트
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataSet GetEduSelectList(string[] rParams
                                    , string rGubun, CultureInfo rArgCultureInfo)
        {
            Database db = null;
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");

                string xWhere = "";
                DataTable xDt = null;

                // 교육기간
                xWhere += " AND O.COURSE_BEGIN_DT >= to_date('" + rParams[2] + "', 'yyyy.mm.dd') ";
                xWhere += " AND O.COURSE_BEGIN_DT <= to_date('" + rParams[3] + "', 'yyyy.mm.dd') ";

                // 교육유형
                if (rParams[4] != "*" && !String.IsNullOrEmpty(rParams[4]))
                    xWhere += @" AND C.COURSE_TYPE = '" + rParams[4] + "' " + "\r\n";

                //언어
                if (rParams[5] != "*" && !String.IsNullOrEmpty(rParams[5]))
                    xWhere += @" AND C.COURSE_LANG = '" + rParams[5] + "' " + "\r\n";

                //과정명

                if (rParams[6] != "*" && !String.IsNullOrEmpty(rParams[6]))
                    xWhere += @" AND UPPER(C.COURSE_NM) LIKE  '%" + rParams[6].ToUpper() + "%' " + "\r\n";

                xSql = @"
                    SELECT count(O.COURSE_ID)
                      FROM T_OPEN_COURSE O, T_COURSE C
                     WHERE O.COURSE_ID = C.COURSE_ID " + "\r\n" + xWhere;
                xDt = base.ExecuteDataTable(db, xSql);
                base.MergeTable(ref xDs, xDt, "table1");

                xSql = "";
                xSql += @"   select *
                               from (
                        select  rownum rnum ";
                xSql += "       , a.* " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += @"     SELECT   C.COURSE_ID
                                        , O.OPEN_COURSE_ID ";

                //국/영문 구분
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += "                          , C.COURSE_NM COURSE_NM ";
                }
                else
                {
                    xSql += "                          , C.COURSE_NM_ABBR COURSE_NM ";
                }

                xSql += @"              , TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_DATE
                                        , TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') AS COURSE_BEGIN_DT
                                        , TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_END_DT
                                        , O.COURSE_SEQ                            
                                        , TO_CHAR(O.COURSE_BEGIN_APPLY_DT, 'YYYY.MM.DD') AS COURSE_BEGIN_APPLY_DT
                                        , TO_CHAR(O.COURSE_END_APPLY_DT, 'YYYY.MM.DD') AS COURSE_END_APPLY_DT
                                        , (TRUNC(O.COURSE_END_DT) - TRUNC(O.COURSE_BEGIN_DT)) + 1 as COURSE_DAY
                                        , (SELECT COUNT(*) FROM T_COURSE_RESULT r, t_user u WHERE r.user_id = u.user_id and r.OPEN_COURSE_ID = O.OPEN_COURSE_ID and CONFIRM='1') AS MAN
                                        , decode(C.expired_period, '0', '', C.expired_period) as expired_period
                                        , C.ESS_DUTY_STEP
                                        , C.OPT_DUTY_WORK
                                 FROM  T_OPEN_COURSE O, T_COURSE C
                                WHERE  O.COURSE_ID = C.COURSE_ID " + "\r\n"
                                     + xWhere;
                //xSql += "    ORDER BY COURSE_BEGIN_DT DESC, C.COURSE_NM ";
                xSql += "    ORDER BY O.INS_DT DESC, C.COURSE_NM ";
                xSql += @"      ) a ";
                xSql += @"  ) a  ";

                if (rGubun != "all")
                {
                    if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                    {
                        xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                        xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                    }
                }

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
            return xDs; ;
        }

        public DataTable GetDtVacationDate(string rOpenCourseId, string rCourseBeginDt, string rCourseEndDt, Database db)
        {
            DataTable xDt = null;
            string xSql = "";
            if (HttpContext.Current.Application["APPR_OPEN_COURSE_ID"] == null || Convert.ToString(HttpContext.Current.Application["APPR_OPEN_COURSE_ID"]) != rOpenCourseId)
            {
                xSql = @" SELECT SNO
                            FROM V_HORDERDET_ORD_VACATIONDATE
                           WHERE ORD_FDATE <= '" + rCourseBeginDt + @"'
                                 AND ORD_TDATE >= '" + rCourseEndDt + @"' ";
                xDt = base.ExecuteDataTable(db, xSql);

                HttpContext.Current.Application["APPR_VACATIONDATE"] = xDt;
                HttpContext.Current.Application["APPR_OPEN_COURSE_ID"] = rOpenCourseId;
            }
            else
            {
                xDt = (DataTable)HttpContext.Current.Application["APPR_VACATIONDATE"];
            }
            return xDt;
        }

        /************************************************************
        * Function name : GetEduListNew
        * Purpose       : 교육대상자:휴가중이면서 해당과정 미이수자, 유효기간 6개월 만료예정자, 이미확정된 대상자                
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetEduListNew(string[] rParams, CultureInfo rArgCultureInfo)
        {
            Database db = null;
            DataSet xDs = new DataSet();
            DataTable xDt = new DataTable();

            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");

                string xWhere = "";

                xSql = @" SELECT TO_CHAR(COURSE_BEGIN_DT, 'YYYYMMDD') AS COURSE_BEGIN_DT
                                 , TO_CHAR(COURSE_END_DT, 'YYYYMMDD') AS COURSE_END_DT
                            FROM T_OPEN_COURSE 
                           WHERE OPEN_COURSE_ID='" + rParams[3] + @"' ";
                xDt = base.ExecuteDataTable(db, xSql);

                string xCourseBeginDt = Convert.ToString(xDt.Rows[0]["COURSE_BEGIN_DT"]);
                string xCourseEndDt = Convert.ToString(xDt.Rows[0]["COURSE_END_DT"]);

                xSql = @" SELECT ESS_DUTY_STEP, OPT_DUTY_WORK FROM T_COURSE WHERE COURSE_ID='" + rParams[2] + @"' ";
                xDt = base.ExecuteDataTable(db, xSql);

                string[] xStrEssDutySteps = Convert.ToString(xDt.Rows[0]["ESS_DUTY_STEP"]).Split(',');
                string[] xStrOptDutyWorks = Convert.ToString(xDt.Rows[0]["OPT_DUTY_WORK"]).Split(',');

                string xStrWhere = "";
                if (xStrEssDutySteps.Length > 0 || xStrOptDutyWorks.Length > 0)
                {
                    string xStrEssDutyStep = "";
                    if (xStrEssDutySteps.Length > 0 && !string.IsNullOrEmpty(xStrEssDutySteps[0]))
                    {
                        xStrEssDutyStep += " U.DUTY_STEP IN (";
                        for (int i = 0; i < xStrEssDutySteps.Length; i++)
                        {
                            if (i > 0)
                                xStrEssDutyStep += ",";
                            xStrEssDutyStep += " '" + xStrEssDutySteps[i] + "' ";
                        }
                        xStrEssDutyStep += " ) ";
                    }

                    string xStrOptDutyWork = "";
                    if (xStrOptDutyWorks.Length > 0 && !string.IsNullOrEmpty(xStrOptDutyWorks[0]))
                    {
                        xStrOptDutyWork += " U.DUTY_WORK IN (";
                        for (int i = 0; i < xStrOptDutyWorks.Length; i++)
                        {
                            if (i > 0)
                                xStrOptDutyWork += ",";
                            xStrOptDutyWork += " '" + xStrOptDutyWorks[i] + "' ";
                        }
                        xStrOptDutyWork += " ) ";
                    }

                    if (!string.IsNullOrEmpty(xStrEssDutyStep) && !string.IsNullOrEmpty(xStrOptDutyWork))
                    {
                        xStrWhere = @"  AND (
                                        " + xStrEssDutyStep + @"
                                        OR " + xStrOptDutyWork + @"
                                        ) ";
                    }
                    else if (!string.IsNullOrEmpty(xStrEssDutyStep))
                    {
                        xStrWhere = @"  AND (
                                        " + xStrEssDutyStep + @"
                                        ) ";
                    }
                    else if (!string.IsNullOrEmpty(xStrOptDutyWork))
                    {
                        xStrWhere = @"  AND (
                                         " + xStrOptDutyWork + @"
                                        ) ";
                    }
                }


                //OPEN_COURSE_ID
                if (!Util.IsNullOrEmptyObject(rParams[3]))
                    xWhere += @" AND R.OPEN_COURSE_ID = '" + rParams[3] + "' " + "\r\n";


                xSql = @"   select *
                               from (
                                select  rownum rnum ";
                xSql += "               , a.*  " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += @"     SELECT  R.KEYS
                                       , R.OPEN_COURSE_ID
                                       , R.USER_ID
                                       , R.COURSE_RESULT_SEQ
                                       , R.DEPT_CODE
                                       , R.DEPT_NAME
                                       , R.DUTY_STEP
                                       , R.STEP_NAME
                                       , R.USER_NM_KOR
                                       , R.ORD_FDATE";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"         , (SELECT DEPT_NAME";
                }
                else
                {
                    xSql += @"         , (SELECT DEPT_ENAME";
                }
                                            //FROM HORDERDET@PUSHRMTLINK A, hdeptcode @PUSHRMTLINK B
                xSql += @"
                                            FROM HINDEV.HORDERDET A, HINDEV.hdeptcode B
                                           WHERE A.A_DEPT_CODE = B.DEPT_CODE
                                             AND A.SNO = R.USER_ID
                                             AND A.ORD_FDATE = REPLACE(R.ORD_FDATE, '.', '')
                                             AND ORD_KIND = 'AF8'
                                             AND A.CONFIRM_YN = 'Y') AS B_DEPT_NAME
                                       , R.INS_DT
                                       , R.USER_COURSE_END_DT
                                       , R.CONFIRM
                                       , R.NON_APPROVAL_CD
                                       , R.NON_APPROVAL_REMARK
                                       , COUNT(*) OVER() TOTALRECORDCOUNT
                                        ,R.PERSONAL_NO
                                        ,R.USER_ADDR
                                        ,R.MOBILE_PHONE
                                        ,R.OFFICE_PHONE
                                        ,(SELECT COMPANY_NM FROM T_COMPANY WHERE COMPANY_ID = R.COMPANY_ID) as COMPANY_NM
                                        ,R.INS_DTIME
                                 FROM  (
                                    ";
                xSql += @"          SELECT (U.USER_ID||'^') AS KEYS
                                           , '' AS OPEN_COURSE_ID
                                           , U.USER_ID
                                           , 0 AS COURSE_RESULT_SEQ
                                           , U.DEPT_CODE
                                           , (SELECT ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"                     DEPT_NAME ";
                }
                else
                {
                    xSql += @"                     DEPT_ENAME ";
                }
                xSql += @"                      FROM V_HDEPTCODE WHERE DEPT_CODE = U.DEPT_CODE) AS DEPT_NAME
                                           , U.DUTY_STEP
                                           , (SELECT ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"                     STEP_NAME ";
                }
                else
                {
                    xSql += @"                     STEP_ENAME ";
                }
                xSql += @"                      FROM V_HDUTYSTEP WHERE DUTY_STEP = U.DUTY_STEP) AS STEP_NAME
                                           , U.USER_NM_KOR
                                           , DECODE(F.ORD_FDATE,'', '', substr(F.ORD_FDATE, 0, 4)||'.'||substr(F.ORD_FDATE, 5, 2)||'.'||substr(F.ORD_FDATE, 7, 2)) AS ORD_FDATE
                                           , '' AS INS_DT
                                           , (SELECT TO_CHAR(MAX(USER_COURSE_END_DT), 'YYYY.MM.DD') FROM (SELECT * FROM T_OPEN_COURSE WHERE COURSE_ID='" + rParams[2] + @"' AND OPEN_COURSE_ID < '" + rParams[3] + @"') C , T_COURSE_RESULT R WHERE C.OPEN_COURSE_ID = R.OPEN_COURSE_ID AND PASS_FLG='000001' AND USER_ID = U.USER_ID GROUP BY USER_ID) AS USER_COURSE_END_DT
                                           , '' AS NON_APPROVAL_CD
                                           , '' AS NON_APPROVAL_REMARK
                                           , '' AS CONFIRM
                                            ,U.PERSONAL_NO
                                            ,U.USER_ADDR
                                            ,U.MOBILE_PHONE
                                            ,U.OFFICE_PHONE
                                            ,U.COMPANY_ID
                                           --, S.STEP_SEQ
                                            ,'' as INS_DTIME
                                      FROM (
                                              SELECT * 
                                                FROM (select user_id,DEPT_CODE,DUTY_STEP,PERSONAL_NO,
                                                            USER_ADDR,
                                                            MOBILE_PHONE,
                                                            OFFICE_PHONE,
                                                            COMPANY_ID ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"                    , USER_NM_KOR ";
                }
                else
                {
                    xSql += @"                    , USER_NM_ENG_FIRST || ' ' || USER_NM_ENG_LAST AS USER_NM_KOR ";
                }
                xSql += @"                                   ,DUTY_WORK, COM_SNO from t_user where SOCIALPOS in('E1','E3', 'E2', 'E4') and status = '000003' ";
                //국적
                if (rParams[4].ToString() != "*")
                {
                    xSql += "                                                 AND COUNTRY_KIND = (SELECT D_DESC FROM T_CODE_DETAIL WHERE M_CD ='0064' AND D_CD = '" + rParams[4].ToString() + "') ";
                }
                //직급
                if (rParams[6].ToString() != "*")
                {
                    xSql += "                                                 AND DUTY_STEP = '" + rParams[6].ToString() + "' ";
                }
                xSql += @"                                   ) U
                                               WHERE 1=1 ";
                //휴가자 구분 Start
                if (rParams[5].ToString() == "Y")
                {
                    xSql += @"                            AND U.USER_ID IN (
                                                          SELECT SNO
                                                            FROM V_HORDERDET_ORD_VACATIONDATE
                                                           WHERE ORD_FDATE <= '" + xCourseBeginDt + @"'
                                                                 AND (T_ORD_TDATE >= '" + xCourseEndDt + @"')  )";
                }
                //휴가자 구분 End
                xSql += @"                    
                                                     AND U.USER_ID NOT IN ( SELECT USER_ID FROM T_COURSE_RESULT WHERE OPEN_COURSE_ID = '" + rParams[3] + @"')
                                                     --이수대상자
                                                     " + xStrWhere + @"
                                                     AND U.COM_SNO NOT IN (
                                                            -- 이수한 사람 제외 (교육 대상자 선발 시 자사+타사 교육 이력 조회 시 기존 현재사번이 아닌 공통사번을 기준으로 대상자 제외하도록 변경)
                                                            SELECT NVL(COM_SNO, '0000000') FROM T_USER COM_USER
                                                            WHERE COM_USER.USER_ID IS NOT NULL
                                                            AND COM_USER.USER_ID IN  
                                                            (
                                                             SELECT P.USER_ID
                                                             FROM (      
                                                             select --o.open_course_id
                                                                    r.user_id
                                                                    , max(o.course_end_dt) as course_end_dt
                                                                    , expired_period
                                                                    , add_months(max(o.course_end_dt),expired_period*12) as exprired_dt
                                                                    , DECODE(expired_period, '0', '1', (add_months(max(o.course_end_dt),(expired_period * 12)-6) - to_Date(to_char(sysdate, 'yyyy.MM.dd'), 'yyyy.MM.dd'))) as exprired
                                                              from  T_COURSE c
                                                                    , t_open_course o
                                                                    , t_course_result r
                                                             where c.course_id = o.course_id
                                                                    and o.open_course_id = r.open_course_id
                                                                    and r.pass_flg = '000001'
                                                                    and o.course_id = '" + rParams[2] + @"'
                                                             group by o.open_course_id, r.user_id, c.expired_period

			                                                 UNION ALL
                                                             -- 타사 교육 이력 테이블에서 해당 과정명 이수한 사람 조회
                                                             select 
                                                                    r.user_id
                                                                    , max(r.course_end_dt) as course_end_dt
                                                                    , expired_period
                                                                    , add_months(max(r.course_end_dt),expired_period*12) as exprired_dt
                                                                    , DECODE(expired_period, '0', '1', (add_months(max(r.course_end_dt),(expired_period * 12)-6) - to_Date(to_char(sysdate, 'yyyy.MM.dd'), 'yyyy.MM.dd'))) as exprired
                                                              from  T_COURSE c
                                                                    , T_REG_TRAINING_RECORD r
                                                             where c.course_id = r.course_id
                                                             and r.course_id = '" + rParams[2] + @"'
                                                             group by r.user_id, c.expired_period

                                                             ) P
                                                             WHERE exprired > 0)
                                                    )
                                             ) U
                                             , V_HORDERDET_ORD_LATEST_OFFDATE F
                                             --, V_HDUTYSTEP S
                                     WHERE --U.DUTY_STEP = S.DUTY_STEP
                                           U.USER_ID = F.SNO(+) " + "\r\n";
                xSql += "          UNION ALL " + "\r\n";
                xSql += @"          
                                    --수강신청한 사람 
                                    SELECT (R.USER_ID||'^'|| R.COURSE_RESULT_SEQ) AS KEYS
                                           , R.OPEN_COURSE_ID
                                           , R.USER_ID
                                           , R.COURSE_RESULT_SEQ
                                           , U.DEPT_CODE
                                           , (SELECT ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"                     DEPT_NAME ";
                }
                else
                {
                    xSql += @"                     DEPT_ENAME ";
                }
                xSql += @"                       FROM V_HDEPTCODE WHERE DEPT_CODE = U.DEPT_CODE) AS DEPT_NAME
                                           , U.DUTY_STEP
                                           , (SELECT ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"                     STEP_NAME ";
                }
                else
                {
                    xSql += @"                     STEP_ENAME ";
                }
                xSql += @"                       FROM V_HDUTYSTEP WHERE DUTY_STEP = U.DUTY_STEP) AS STEP_NAME
                                            ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"                    , USER_NM_KOR ";
                }
                else
                {
                    xSql += @"                    , USER_NM_ENG_FIRST || ' ' || USER_NM_ENG_LAST AS USER_NM_KOR ";
                }
                xSql += @"                                   
                                           , DECODE(F.ORD_FDATE,'', '', substr(F.ORD_FDATE, 0, 4)||'.'||substr(F.ORD_FDATE, 5, 2)||'.'||substr(F.ORD_FDATE, 7, 2)) AS ORD_FDATE
                                           , TO_CHAR(R.INS_DT, 'YYYY.MM.DD') AS INS_DT
                                           , (SELECT TO_CHAR(MAX(USER_COURSE_END_DT), 'YYYY.MM.DD') FROM (SELECT * FROM T_OPEN_COURSE WHERE COURSE_ID='" + rParams[2] + @"' AND OPEN_COURSE_ID < '" + rParams[3] + @"') C , T_COURSE_RESULT R WHERE C.OPEN_COURSE_ID = R.OPEN_COURSE_ID AND PASS_FLG='000001' AND USER_ID = U.USER_ID GROUP BY USER_ID) AS USER_COURSE_END_DT
                                           , R.NON_APPROVAL_CD
                                           , TO_CHAR(R.NON_APPROVAL_REMARK) AS NON_APPROVAL_REMARK
                                           , R.CONFIRM
                                            ,U.PERSONAL_NO
                                            ,U.USER_ADDR
                                            ,U.MOBILE_PHONE
                                            ,U.OFFICE_PHONE
                                            ,U.COMPANY_ID
                                            ,TO_CHAR(R.INS_DT, 'YYYY.MM.DD HH24:MI:SS') AS INS_DTIME

                                           --, S.STEP_SEQ
                                      FROM T_COURSE_RESULT R
                                            , T_USER U
                                            , V_HDEPTCODE D
                                            , V_HORDERDET_ORD_LATEST_OFFDATE F
                                            --, V_HDUTYSTEP S
                                      WHERE R.USER_ID = U.USER_ID
                                            --AND U.DUTY_STEP = S.DUTY_STEP
                                            AND U.DEPT_CODE = D.DEPT_CODE(+)
                                            AND R.USER_ID = F.SNO(+)
                                            " + xWhere;
                xSql += "              ) R " + "\r\n";
                xSql += "          ORDER BY NVL(R.INS_DTIME, '2999.12.31 23:59:59'), R.CONFIRM DESC, R.DUTY_STEP, R.USER_NM_KOR, R.USER_ID " + "\r\n";
                xSql += @"      ) a " + "\r\n";
                xSql += " ) a " + "\r\n";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));

                //if (rGubun != "all")
                //{
                //    if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                //    {
                //        xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                //        xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                //    }
                //}

                xDt = base.ExecuteDataTable(db, xSql);

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

            return xDt;
        }

        /************************************************************
        * Function name : GetEduList
        * Purpose       : 교육대상자:휴가중이면서 해당과정 미이수자, 유효기간 6개월 만료예정자, 이미확정된 대상자                
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataSet GetEduList(string[] rParams
                                    , string rGubun)
        {
            Database db = null;
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");

                string xWhere = "";
                DataTable xDt = null;

                xSql = @" SELECT TO_CHAR(COURSE_BEGIN_DT, 'YYYYMMDD') AS COURSE_BEGIN_DT
                                 , TO_CHAR(COURSE_END_DT, 'YYYYMMDD') AS COURSE_END_DT
                            FROM T_OPEN_COURSE 
                           WHERE OPEN_COURSE_ID='" + rParams[3] + @"' ";
                xDt = base.ExecuteDataTable(db, xSql);

                string xCourseBeginDt = Convert.ToString(xDt.Rows[0]["COURSE_BEGIN_DT"]);
                string xCourseEndDt = Convert.ToString(xDt.Rows[0]["COURSE_END_DT"]);

                xSql = @" SELECT ESS_DUTY_STEP, OPT_DUTY_WORK FROM T_COURSE WHERE COURSE_ID='" + rParams[2] + @"' ";
                xDt = base.ExecuteDataTable(db, xSql);

                string[] xStrEssDutySteps = Convert.ToString(xDt.Rows[0]["ESS_DUTY_STEP"]).Split(',');
                string[] xStrOptDutyWorks = Convert.ToString(xDt.Rows[0]["OPT_DUTY_WORK"]).Split(',');

                string xStrWhere = "";
                if (xStrEssDutySteps.Length > 0 || xStrOptDutyWorks.Length > 0)
                {
                    string xStrEssDutyStep = "";
                    if (xStrEssDutySteps.Length > 0 && !string.IsNullOrEmpty(xStrEssDutySteps[0]))
                    {
                        xStrEssDutyStep += " U.DUTY_STEP IN (";
                        for (int i = 0; i < xStrEssDutySteps.Length; i++)
                        {
                            if (i > 0)
                                xStrEssDutyStep += ",";
                            xStrEssDutyStep += " '" + xStrEssDutySteps[i] + "' ";
                        }
                        xStrEssDutyStep += " ) ";
                    }

                    string xStrOptDutyWork = "";
                    if (xStrOptDutyWorks.Length > 0 && !string.IsNullOrEmpty(xStrOptDutyWorks[0]))
                    {
                        xStrOptDutyWork += " U.DUTY_WORK IN (";
                        for (int i = 0; i < xStrOptDutyWorks.Length; i++)
                        {
                            if (i > 0)
                                xStrOptDutyWork += ",";
                            xStrOptDutyWork += " '" + xStrOptDutyWorks[i] + "' ";
                        }
                        xStrOptDutyWork += " ) ";
                    }

                    if (!string.IsNullOrEmpty(xStrEssDutyStep) && !string.IsNullOrEmpty(xStrOptDutyWork))
                    {
                        xStrWhere = @"  AND (
                                        " + xStrEssDutyStep + @"
                                        OR " + xStrOptDutyWork + @"
                                        ) ";
                    }
                    else if (!string.IsNullOrEmpty(xStrEssDutyStep))
                    {
                        xStrWhere = @"  AND (
                                        " + xStrEssDutyStep + @"
                                        ) ";
                    }
                    else if (!string.IsNullOrEmpty(xStrOptDutyWork))
                    {
                        xStrWhere = @"  AND (
                                         " + xStrOptDutyWork + @"
                                        ) ";
                    }
                }


                //OPEN_COURSE_ID
                if (!Util.IsNullOrEmptyObject(rParams[3]))
                    xWhere += @" AND R.OPEN_COURSE_ID = '" + rParams[3] + "' " + "\r\n";

                xSql = @"
                    SELECT count(*)                              
                      FROM (
                           SELECT ('^'|| U.USER_ID) AS KEYS
                             FROM (
                                      SELECT USER_ID 
                                        FROM (select user_id, DUTY_STEP, duty_work from t_user where SOCIALPOS in('E1','E3') and status = '000003') U --승인된사용자만 보기
                                             , V_HDUTYSTEP S
                                       WHERE 1=1
                                             AND U.USER_ID IN (
                                                  SELECT SNO
                                                    FROM V_HORDERDET_ORD_VACATIONDATE
                                                   WHERE ORD_FDATE <= '" + xCourseBeginDt + @"'
                                                         AND (T_ORD_TDATE >= '" + xCourseEndDt + @"')
                                             )
                                             AND U.USER_ID NOT IN ( SELECT USER_ID FROM T_COURSE_RESULT WHERE OPEN_COURSE_ID = '" + rParams[3] + @"')
                                             --이수대상자
                                             " + xStrWhere + @"
                                             AND U.USER_ID NOT IN (
                                                    --이수한 사람 제외
                                                     SELECT P.USER_ID
                                                       FROM (      
                                                    select o.open_course_id
                                                            , r.user_id
                                                            , max(o.course_end_dt) as course_end_dt
                                                            , expired_period
                                                            , add_months(max(o.course_end_dt),expired_period) as exprired_dt
                                                            , DECODE(expired_period, '0', '1', (add_months(max(o.course_end_dt),(expired_period * 12)-6) - SYSDATE)) as exprired
                                                      from  T_COURSE c
                                                            , t_open_course o
                                                            , t_course_result r
                                                     where c.course_id = o.course_id
                                                            and o.open_course_id = r.open_course_id
                                                            and r.pass_flg = '000001'
                                                            and o.course_id = '" + rParams[2] + @"'
                                                     group by o.open_course_id, r.user_id, c.expired_period
                                                     ) P
                                                     WHERE exprired > 0
                                             )
                                             AND U.DUTY_STEP = S.DUTY_STEP
                             ) U
                             UNION ALL           
                            --수강신청한 사람 
                            SELECT (R.USER_ID||'^'|| R.COURSE_RESULT_SEQ) AS KEYS
                              FROM T_COURSE_RESULT R
                                    , T_USER U
                                    , V_HDEPTCODE D
                                    , V_HORDERDET_ORD_LATEST_OFFDATE F
                                    , V_HDUTYSTEP S
                              WHERE R.USER_ID = U.USER_ID
                                    AND U.DUTY_STEP = S.DUTY_STEP
                                    AND U.DEPT_CODE = D.DEPT_CODE(+)
                                    AND R.USER_ID = F.SNO(+) "
                                   + xWhere + @"
                           ) R ";
                xDt = base.ExecuteDataTable(db, xSql);
                base.MergeTable(ref xDs, xDt, "table1");

                xSql = "";
                xSql += @"   select *
                               from (
                                select  rownum rnum ";
                xSql += "               , a.* " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += @"     SELECT  R.KEYS
                                       , R.OPEN_COURSE_ID
                                       , R.USER_ID
                                       , R.COURSE_RESULT_SEQ
                                       , R.DEPT_CODE
                                       , R.DEPT_NAME
                                       , R.DUTY_STEP
                                       , R.STEP_NAME
                                       , R.USER_NM_KOR
                                       , R.ORD_FDATE
                                       , R.INS_DT
                                       , R.USER_COURSE_END_DT
                                       , R.CONFIRM
                                       , R.NON_APPROVAL_CD
                                       , R.NON_APPROVAL_REMARK
                                 FROM  (
                                    ";
                xSql += @"          SELECT (U.USER_ID||'^') AS KEYS
                                           , '' AS OPEN_COURSE_ID
                                           , U.USER_ID
                                           , 0 AS COURSE_RESULT_SEQ
                                           , U.DEPT_CODE
                                           , (SELECT DEPT_NAME FROM V_HDEPTCODE WHERE DEPT_CODE = U.DEPT_CODE) AS DEPT_NAME
                                           , U.DUTY_STEP
                                           , (SELECT STEP_NAME FROM V_HDUTYSTEP WHERE DUTY_STEP = U.DUTY_STEP) AS STEP_NAME
                                           , U.USER_NM_KOR
                                           , DECODE(F.ORD_FDATE,'', '', substr(F.ORD_FDATE, 0, 4)||'.'||substr(F.ORD_FDATE, 5, 2)||'.'||substr(F.ORD_FDATE, 7, 2)) AS ORD_FDATE
                                           , '' AS INS_DT
                                           , (SELECT TO_CHAR(MAX(USER_COURSE_END_DT), 'YYYY.MM.DD') FROM (SELECT * FROM T_OPEN_COURSE WHERE COURSE_ID='" + rParams[2] + @"' AND OPEN_COURSE_ID < '" + rParams[3] + @"') C , T_COURSE_RESULT R WHERE C.OPEN_COURSE_ID = R.OPEN_COURSE_ID AND PASS_FLG='000001' AND USER_ID = U.USER_ID GROUP BY USER_ID) AS USER_COURSE_END_DT
                                           , '' AS NON_APPROVAL_CD
                                           , '' AS NON_APPROVAL_REMARK
                                           , '' AS CONFIRM
                                           --, S.STEP_SEQ
                                      FROM (
                                              SELECT * 
                                                FROM (select user_id,DEPT_CODE,DUTY_STEP,USER_NM_KOR,DUTY_WORK from t_user where SOCIALPOS in('E1','E3') and status = '000003') U
                                               WHERE 1=1
                                                     AND U.USER_ID IN (
                                                          SELECT SNO
                                                            FROM V_HORDERDET_ORD_VACATIONDATE
                                                           WHERE ORD_FDATE <= '" + xCourseBeginDt + @"'
                                                                 AND (T_ORD_TDATE >= '" + xCourseEndDt + @"')
                                                     )
                                                     AND U.USER_ID NOT IN ( SELECT USER_ID FROM T_COURSE_RESULT WHERE OPEN_COURSE_ID = '" + rParams[3] + @"')
                                                     --이수대상자
                                                     " + xStrWhere + @"
                                                     AND U.USER_ID NOT IN (
                                                            --이수한 사람 제외
                                                             SELECT P.USER_ID
                                                               FROM (      
                                                            select o.open_course_id
                                                                    , r.user_id
                                                                    , max(o.course_end_dt) as course_end_dt
                                                                    , expired_period
                                                                    , add_months(max(o.course_end_dt),expired_period*12) as exprired_dt
                                                                    , DECODE(expired_period, '0', '1', (add_months(max(o.course_end_dt),(expired_period * 12)-6) - SYSDATE)) as exprired
                                                              from  T_COURSE c
                                                                    , t_open_course o
                                                                    , t_course_result r
                                                             where c.course_id = o.course_id
                                                                    and o.open_course_id = r.open_course_id
                                                                    and r.pass_flg = '000001'
                                                                    and o.course_id = '" + rParams[2] + @"'
                                                             group by o.open_course_id, r.user_id, c.expired_period
                                                             ) P
                                                             WHERE exprired > 0
                                                    )
                                             ) U
                                             , V_HORDERDET_ORD_LATEST_OFFDATE F
                                             --, V_HDUTYSTEP S
                                     WHERE --U.DUTY_STEP = S.DUTY_STEP
                                           U.USER_ID = F.SNO(+) " + "\r\n";
                xSql += "          UNION ALL " + "\r\n";
                xSql += @"          
                                    --수강신청한 사람 
                                    SELECT (R.USER_ID||'^'|| R.COURSE_RESULT_SEQ) AS KEYS
                                           , R.OPEN_COURSE_ID
                                           , R.USER_ID
                                           , R.COURSE_RESULT_SEQ
                                           , U.DEPT_CODE
                                           , (SELECT DEPT_NAME FROM V_HDEPTCODE WHERE DEPT_CODE = U.DEPT_CODE) AS DEPT_NAME
                                           , U.DUTY_STEP
                                           , (SELECT STEP_NAME FROM V_HDUTYSTEP WHERE DUTY_STEP = U.DUTY_STEP) AS STEP_NAME
                                           , U.USER_NM_KOR
                                           , DECODE(F.ORD_FDATE,'', '', substr(F.ORD_FDATE, 0, 4)||'.'||substr(F.ORD_FDATE, 5, 2)||'.'||substr(F.ORD_FDATE, 7, 2)) AS ORD_FDATE
                                           , TO_CHAR(R.INS_DT, 'YYYY.MM.DD') AS INS_DT
                                           , (SELECT TO_CHAR(MAX(USER_COURSE_END_DT), 'YYYY.MM.DD') FROM (SELECT * FROM T_OPEN_COURSE WHERE COURSE_ID='" + rParams[2] + @"' AND OPEN_COURSE_ID < '" + rParams[3] + @"') C , T_COURSE_RESULT R WHERE C.OPEN_COURSE_ID = R.OPEN_COURSE_ID AND PASS_FLG='000001' AND USER_ID = U.USER_ID GROUP BY USER_ID) AS USER_COURSE_END_DT
                                           , R.NON_APPROVAL_CD
                                           , TO_CHAR(R.NON_APPROVAL_REMARK) AS NON_APPROVAL_REMARK
                                           , R.CONFIRM
                                           --, S.STEP_SEQ
                                      FROM T_COURSE_RESULT R
                                            , T_USER U
                                            , V_HDEPTCODE D
                                            , V_HORDERDET_ORD_LATEST_OFFDATE F
                                            --, V_HDUTYSTEP S
                                      WHERE R.USER_ID = U.USER_ID
                                            --AND U.DUTY_STEP = S.DUTY_STEP
                                            AND U.DEPT_CODE = D.DEPT_CODE(+)
                                            AND R.USER_ID = F.SNO(+)
                                            " + xWhere;
                xSql += "              ) R " + "\r\n";
                xSql += "          --ORDER BY R.STEP_SEQ " + "\r\n";
                xSql += @"      ) a " + "\r\n";
                xSql += " ) a " + "\r\n";

                #region
                //                xSql = @"
                //                    SELECT count(*)                              
                //                      FROM (
                //                           SELECT ('^'|| U.USER_ID) AS KEYS
                //                             FROM (
                //                                    SELECT SNO
                //                                      FROM V_HORDERDET_ORD_VACATIONDATE
                //                                     WHERE ORD_FDATE <= (SELECT TO_CHAR(COURSE_BEGIN_DT, 'YYYYMMDD') FROM T_OPEN_COURSE WHERE OPEN_COURSE_ID='" + rParams[3] + @"')
                //                                            AND ORD_TDATE >= (SELECT TO_CHAR(COURSE_END_DT, 'YYYYMMDD') FROM T_OPEN_COURSE WHERE OPEN_COURSE_ID='" + rParams[3] + @"')
                //                                   ) V
                //                                   , (
                //                                      SELECT USER_ID 
                //                                        FROM T_USER U
                //                                       WHERE U.USER_ID NOT IN (SELECT USER_ID FROM T_COURSE_RESULT WHERE OPEN_COURSE_ID = '" + rParams[3] + @"')
                //                                             AND 
                //                                             (
                //                                                 --이수대상자
                //                                                 (instr(duty_step,(SELECT ESS_DUTY_STEP FROM T_COURSE WHERE COURSE_ID='" + rParams[2] + @"')) >=0)
                //                                                 OR (instr(duty_work,(SELECT OPT_DUTY_WORK FROM T_COURSE WHERE COURSE_ID='" + rParams[2] + @"')) >=0)
                //                                             )
                //                                             AND U.USER_ID NOT IN (
                //                                                    --이수한 사람 제외
                //                                                     SELECT P.USER_ID
                //                                                       FROM (      
                //                                                    select o.open_course_id
                //                                                            , r.user_id
                //                                                            , max(o.course_end_dt) as course_end_dt
                //                                                            , expired_period
                //                                                            , add_months(max(o.course_end_dt),expired_period) as exprired_dt
                //                                                            , add_months(max(o.course_end_dt),expired_period) - add_months(SYSDATE,-6) as exprired
                //                                                      from T_COURSE c
                //                                                            , t_open_course o
                //                                                            , t_course_result r
                //                                                     where c.course_id = o.course_id
                //                                                            and o.open_course_id = r.open_course_id
                //                                                            and r.pass_flg = '000001'
                //                                                            and o.course_id = '" + rParams[2] + @"'
                //                                                     group by o.open_course_id, r.user_id, c.expired_period
                //                                                     ) P
                //                                                     WHERE exprired > 0
                //                                             )
                //
                //                                     ) U
                //                             WHERE V.SNO = U.USER_ID           
                //                             UNION ALL           
                //                            SELECT (R.OPEN_COURSE_ID ||'^'|| R.USER_ID) AS KEYS
                //                              FROM T_COURSE_RESULT R
                //                                    , T_USER U
                //                                    , V_HDEPTCODE D
                //                                    , V_HORDERDET_ORD_LATEST_OFFDATE F
                //                              WHERE R.USER_ID = U.USER_ID
                //                                    AND U.DEPT_CODE = D.DEPT_CODE(+)
                //                                    AND R.USER_ID = F.SNO(+) "
                //                                   + xWhere + @"
                //                           ) R ";
                //                xDt = base.ExecuteDataTable(db, xSql);
                //                base.MergeTable(ref xDs, xDt, "table1");

                //                xSql = "";
                //                xSql += @"   select *
                //                               from (
                //                                select  rownum rnum ";
                //                xSql += "               , a.* " + "\r\n";
                //                xSql += "     FROM ( " + "\r\n";
                //                xSql += @"     SELECT  R.KEYS
                //                                       , R.OPEN_COURSE_ID
                //                                       , R.USER_ID
                //                                       , R.COURSE_RESULT_SEQ
                //                                       , R.DEPT_CODE
                //                                       , R.DEPT_NAME
                //                                       , R.DUTY_STEP
                //                                       , R.STEP_NAME
                //                                       , R.USER_NM_KOR
                //                                       , R.ORD_FDATE
                //                                       , R.INS_DT
                //                                       , R.USER_COURSE_END_DT
                //                                       , R.CONFIRM
                //                                       , R.NON_APPROVAL_CD
                //                                       , R.NON_APPROVAL_REMARK
                //                                 FROM  (
                //                                    ";
                //                xSql += @"          SELECT (U.USER_ID||'^') AS KEYS
                //                                           , '' AS OPEN_COURSE_ID
                //                                           , U.USER_ID
                //                                           , 0 AS COURSE_RESULT_SEQ
                //                                           , U.DEPT_CODE
                //                                           , (SELECT DEPT_NAME FROM V_HDEPTCODE WHERE DEPT_CODE = U.DEPT_CODE) AS DEPT_NAME
                //                                           , U.DUTY_STEP
                //                                           , (SELECT STEP_NAME FROM V_HDUTYSTEP WHERE DUTY_STEP = U.DUTY_STEP) AS STEP_NAME
                //                                           , U.USER_NM_KOR
                //                                           , DECODE(V.ORD_FDATE,'', '', substr(V.ORD_FDATE, 0, 4)||'.'||substr(V.ORD_FDATE, 5, 2)||'.'||substr(V.ORD_FDATE, 7, 2)) AS ORD_FDATE
                //                                           , '' AS INS_DT
                //                                           , (SELECT TO_CHAR(MAX(USER_COURSE_END_DT), 'YYYY.MM.DD') FROM T_COURSE_RESULT WHERE PASS_FLG='000001' AND USER_ID = U.USER_ID GROUP BY USER_ID) AS USER_COURSE_END_DT --M_CD='0010'
                //                                           , '0' AS CONFIRM
                //                                           , '' AS NON_APPROVAL_CD
                //                                           , '' AS NON_APPROVAL_REMARK
                //                                      FROM (
                //                                            SELECT * 
                //                                              FROM V_HORDERDET_ORD_VACATIONDATE
                //                                             WHERE ORD_FDATE <= (SELECT TO_CHAR(COURSE_BEGIN_DT, 'YYYYMMDD') FROM T_OPEN_COURSE WHERE OPEN_COURSE_ID='" + rParams[3] + @"')
                //                                                    AND ORD_TDATE >= (SELECT TO_CHAR(COURSE_END_DT, 'YYYYMMDD') FROM T_OPEN_COURSE WHERE OPEN_COURSE_ID='" + rParams[3] + @"')
                //                                           ) V
                //                                           , (
                //                                              SELECT * 
                //                                                FROM T_USER U
                //                                               WHERE U.USER_ID NOT IN ( SELECT USER_ID FROM T_COURSE_RESULT WHERE OPEN_COURSE_ID = '" + rParams[3] + @"')
                //                                                     AND 
                //                                                     (
                //                                                         --이수대상자
                //                                                         (instr(duty_step,(SELECT ESS_DUTY_STEP FROM T_COURSE WHERE COURSE_ID='" + rParams[2] + @"')) >=0)
                //                                                         OR (instr(duty_work,(SELECT OPT_DUTY_WORK FROM T_COURSE WHERE COURSE_ID='" + rParams[2] + @"')) >=0)
                //                                                     )
                //                                                     AND U.USER_ID NOT IN (
                //                                                            --이수한 사람 제외
                //                                                             SELECT P.USER_ID
                //                                                               FROM (      
                //                                                            select o.open_course_id
                //                                                                    , r.user_id
                //                                                                    , max(o.course_end_dt) as course_end_dt
                //                                                                    , expired_period
                //                                                                    , add_months(max(o.course_end_dt),expired_period) as exprired_dt
                //                                                                    , add_months(max(o.course_end_dt),expired_period) - add_months(SYSDATE,-6) as exprired
                //                                                              from T_COURSE c
                //                                                                    , t_open_course o
                //                                                                    , t_course_result r
                //                                                             where c.course_id = o.course_id
                //                                                                    and o.open_course_id = r.open_course_id
                //                                                                    and r.pass_flg = '000001'
                //                                                                    and o.course_id = '" + rParams[2] + @"'
                //                                                             group by o.open_course_id, r.user_id, c.expired_period
                //                                                             ) P
                //                                                             WHERE exprired > 0
                //                                                    )
                //                                             ) U
                //                                     WHERE V.SNO = U.USER_ID " + "\r\n";
                //                xSql += "          UNION ALL " + "\r\n";
                //                xSql += @"          
                //                                    --수강신청한 사람 
                //                                    SELECT (R.USER_ID||'^'|| R.COURSE_RESULT_SEQ) AS KEYS
                //                                           , R.OPEN_COURSE_ID
                //                                           , R.USER_ID
                //                                           , R.COURSE_RESULT_SEQ
                //                                           , U.DEPT_CODE
                //                                           , (SELECT DEPT_NAME FROM V_HDEPTCODE WHERE DEPT_CODE = U.DEPT_CODE) AS DEPT_NAME
                //                                           , U.DUTY_STEP
                //                                           , (SELECT STEP_NAME FROM V_HDUTYSTEP WHERE DUTY_STEP = U.DUTY_STEP) AS STEP_NAME
                //                                           , U.USER_NM_KOR
                //                                           , DECODE(F.ORD_FDATE,'', '', substr(F.ORD_FDATE, 0, 4)||'.'||substr(F.ORD_FDATE, 5, 2)||'.'||substr(F.ORD_FDATE, 7, 2)) AS ORD_FDATE
                //                                           , TO_CHAR(R.INS_DT, 'YYYY.MM.DD') AS INS_DT
                //                                           , (SELECT TO_CHAR(MAX(USER_COURSE_END_DT), 'YYYY.MM.DD') FROM T_COURSE_RESULT WHERE PASS_FLG='000001' AND USER_ID = U.USER_ID GROUP BY USER_ID) AS USER_COURSE_END_DT --M_CD='0010'
                //                                           , '1' AS CONFIRM
                //                                           , R.NON_APPROVAL_CD
                //                                           , TO_CHAR(R.NON_APPROVAL_REMARK) AS NON_APPROVAL_REMARK
                //                                      FROM T_COURSE_RESULT R
                //                                            , T_USER U
                //                                            , V_HDEPTCODE D
                //                                            , V_HORDERDET_ORD_LATEST_OFFDATE F
                //                                      WHERE R.USER_ID = U.USER_ID
                //                                            AND U.DEPT_CODE = D.DEPT_CODE(+)
                //                                            AND R.USER_ID = F.SNO(+)
                //                                            " + xWhere;
                //                xSql += "              ) R " + "\r\n";
                //                xSql += "          ORDER BY R.USER_ID, R.OPEN_COURSE_ID DESC ";
                //                xSql += @"      ) a "+"\r\n";
                //                xSql += " ) a ";
                #endregion

                if (rGubun != "all")
                {
                    if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                    {
                        xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                        xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                    }
                }

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
            return xDs; ;
        }

        /************************************************************
        * Function name : GetEduUserList
        * Purpose       : 전체해상직원-승선중인 사람중에서 해당과정 미이수자, 유효기간 6개월 만료예정자
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataSet GetEduUserList(string[] rParams
                                    , string rGubun)
        {
            Database db = null;
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");

                DataTable xDt = null;
                xSql = @" SELECT TO_CHAR(COURSE_BEGIN_DT, 'YYYYMMDD') AS COURSE_BEGIN_DT
                                 , TO_CHAR(COURSE_END_DT, 'YYYYMMDD') AS COURSE_END_DT
                            FROM T_OPEN_COURSE 
                           WHERE OPEN_COURSE_ID='" + rParams[3] + @"' ";
                xDt = base.ExecuteDataTable(db, xSql);

                string xCourseBeginDt = Convert.ToString(xDt.Rows[0]["COURSE_BEGIN_DT"]);
                string xCourseEndDt = Convert.ToString(xDt.Rows[0]["COURSE_END_DT"]);

                xSql = @" SELECT ESS_DUTY_STEP, OPT_DUTY_WORK FROM T_COURSE WHERE COURSE_ID='" + rParams[2] + @"' ";
                xDt = base.ExecuteDataTable(db, xSql);

                string[] xStrEssDutySteps = Convert.ToString(xDt.Rows[0]["ESS_DUTY_STEP"]).Split(',');
                string[] xStrOptDutyWorks = Convert.ToString(xDt.Rows[0]["OPT_DUTY_WORK"]).Split(',');

                string xStrWhere = "";
                if (xStrEssDutySteps.Length > 0 || xStrOptDutyWorks.Length > 0)
                {
                    string xStrEssDutyStep = "";
                    if (xStrEssDutySteps.Length > 0 && !string.IsNullOrEmpty(xStrEssDutySteps[0]))
                    {
                        xStrEssDutyStep += " U.DUTY_STEP IN (";
                        for (int i = 0; i < xStrEssDutySteps.Length; i++)
                        {
                            if (i > 0)
                                xStrEssDutyStep += ",";
                            xStrEssDutyStep += " '" + xStrEssDutySteps[i] + "' ";
                        }
                        xStrEssDutyStep += " ) ";
                    }

                    string xStrOptDutyWork = "";
                    if (xStrOptDutyWorks.Length > 0 && !string.IsNullOrEmpty(xStrOptDutyWorks[0]))
                    {
                        xStrOptDutyWork += " U.DUTY_WORK IN (";
                        for (int i = 0; i < xStrOptDutyWorks.Length; i++)
                        {
                            if (i > 0)
                                xStrOptDutyWork += ",";
                            xStrOptDutyWork += " '" + xStrOptDutyWorks[i] + "' ";
                        }
                        xStrOptDutyWork += " ) ";
                    }

                    if (!string.IsNullOrEmpty(xStrEssDutyStep) && !string.IsNullOrEmpty(xStrOptDutyWork))
                    {
                        xStrWhere = @"  AND (
                                        " + xStrEssDutyStep + @"
                                        OR " + xStrOptDutyWork + @"
                                        ) ";
                    }
                    else if (!string.IsNullOrEmpty(xStrEssDutyStep))
                    {
                        xStrWhere = @"  AND (
                                        " + xStrEssDutyStep + @"
                                        ) ";
                    }
                    else if (!string.IsNullOrEmpty(xStrOptDutyWork))
                    {
                        xStrWhere = @"  AND (
                                         " + xStrOptDutyWork + @"
                                        ) ";
                    }
                }

                xSql = @"
                    SELECT count(*)                              
                      FROM (
                              SELECT * 
                                FROM (select user_id, duty_step, duty_work from t_user where SOCIALPOS in('E1','E3') and status = '000003') U
                               WHERE 1=1
                                     AND U.USER_ID IN (
                                          SELECT SNO
                                            FROM V_HORDERDET_ORD_ONDATE
                                           WHERE ORD_FDATE <= '" + xCourseBeginDt + @"'
                                                 AND (T_ORD_TDATE >= '" + xCourseEndDt + @"')
                                     )
                                     AND U.USER_ID NOT IN ( SELECT USER_ID FROM T_COURSE_RESULT WHERE OPEN_COURSE_ID = '" + rParams[3] + @"')
                                     " + xStrWhere + @"
                                     AND U.USER_ID NOT IN (
                                            --이수한 사람 제외
                                             SELECT P.USER_ID
                                               FROM (      
                                            select o.open_course_id
                                                    , r.user_id
                                                    , max(o.course_end_dt) as course_end_dt
                                                    , expired_period
                                                    , add_months(max(o.course_end_dt),expired_period*12) as exprired_dt
                                                    , DECODE(expired_period, '0', '1', (add_months(max(o.course_end_dt),(expired_period * 12)-6) - SYSDATE)) as exprired
                                              from  T_COURSE c
                                                    , t_open_course o
                                                    , t_course_result r
                                             where c.course_id = o.course_id
                                                    and o.open_course_id = r.open_course_id
                                                    and r.pass_flg = '000001'
                                                    and o.course_id = '" + rParams[2] + @"'
                                             group by o.open_course_id, r.user_id, c.expired_period
                                             ) P
                                             WHERE exprired > 0
                                    )
                             ) U
                           , V_HORDERDET_ORD_LATEST_OFFDATE F
                           , (SELECT * FROM T_COURSE_RESULT WHERE OPEN_COURSE_ID='" + rParams[3] + @"' ) R
                     WHERE U.USER_ID = F.SNO(+)
                           AND U.USER_ID = R.USER_ID(+)
                           ";
                xDt = base.ExecuteDataTable(db, xSql);
                base.MergeTable(ref xDs, xDt, "table1");

                xSql = "";
                xSql += @"   
                     select *
                       from (
                        select  rownum rnum ";
                xSql += "       , a.* " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += @"          SELECT 
                                            (U.USER_ID||'^'|| R.COURSE_RESULT_SEQ) AS KEYS
                                           , R.OPEN_COURSE_ID AS OPEN_COURSE_ID
                                           , U.USER_ID
                                           , 0 AS COURSE_RESULT_SEQ
                                           , U.DEPT_CODE
                                           , (SELECT DEPT_NAME FROM V_HDEPTCODE WHERE DEPT_CODE = U.DEPT_CODE) AS DEPT_NAME
                                           , U.DUTY_STEP
                                           , (SELECT STEP_NAME FROM V_HDUTYSTEP WHERE DUTY_STEP = U.DUTY_STEP) AS STEP_NAME
                                           , U.USER_NM_KOR
                                           , DECODE(F.ORD_FDATE,'', '', substr(F.ORD_FDATE, 0, 4)||'.'||substr(F.ORD_FDATE, 5, 2)||'.'||substr(F.ORD_FDATE, 7, 2)) AS ORD_FDATE
                                           , TO_CHAR(R.INS_DT, 'YYYY.MM.DD') AS INS_DT
                                           , (SELECT TO_CHAR(MAX(USER_COURSE_END_DT), 'YYYY.MM.DD') FROM (SELECT * FROM T_OPEN_COURSE WHERE COURSE_ID='" + rParams[2] + @"' AND OPEN_COURSE_ID < '" + rParams[3] + @"') C , T_COURSE_RESULT R WHERE C.OPEN_COURSE_ID = R.OPEN_COURSE_ID AND PASS_FLG='000001' AND USER_ID = U.USER_ID GROUP BY USER_ID) AS USER_COURSE_END_DT
                                           , R.NON_APPROVAL_CD AS NON_APPROVAL_CD
                                           , R.NON_APPROVAL_REMARK AS NON_APPROVAL_REMARK
                                           , R.CONFIRM 
                                      FROM (
                                              SELECT * 
                                                FROM (
                                                      select U.user_id,U.DEPT_CODE,U.DUTY_STEP,U.USER_NM_KOR,U.DUTY_WORK 
                                                        from t_user U
                                                       where SOCIALPOS in ('E1','E3') and status = '000003'
                                                       ) U
                                               WHERE 1=1
                                                     AND U.USER_ID IN (
                                                          SELECT SNO
                                                            FROM V_HORDERDET_ORD_ONDATE
                                                           WHERE ORD_FDATE <= '" + xCourseBeginDt + @"'
                                                                 AND (T_ORD_TDATE >= '" + xCourseEndDt + @"')
                                                     )
                                                     AND U.USER_ID NOT IN ( SELECT USER_ID FROM T_COURSE_RESULT WHERE OPEN_COURSE_ID = '" + rParams[3] + @"')
                                                     --이수대상자
                                                     " + xStrWhere + @"
                                                     AND U.USER_ID NOT IN (
                                                            --이수한 사람 제외
                                                             SELECT P.USER_ID
                                                               FROM (      
                                                            select o.open_course_id
                                                                    , r.user_id
                                                                    , max(o.course_end_dt) as course_end_dt
                                                                    , expired_period
                                                                    , add_months(max(o.course_end_dt),expired_period*12) as exprired_dt
                                                                    , DECODE(expired_period, '0', '1', (add_months(max(o.course_end_dt),(expired_period * 12)-6) - SYSDATE)) as exprired
                                                              from T_COURSE c
                                                                    , t_open_course o
                                                                    , t_course_result r
                                                             where c.course_id = o.course_id
                                                                    and o.open_course_id = r.open_course_id
                                                                    and r.pass_flg = '000001'
                                                                    and o.course_id = '" + rParams[2] + @"'
                                                             group by o.open_course_id, r.user_id, c.expired_period
                                                             ) P
                                                             WHERE exprired > 0
                                                    )
                                             ) U
                                           , V_HORDERDET_ORD_LATEST_OFFDATE F
                                           , (SELECT * FROM T_COURSE_RESULT WHERE OPEN_COURSE_ID='" + rParams[3] + @"' ) R
                                     WHERE U.USER_ID = F.SNO(+)
                                           AND U.USER_ID = R.USER_ID(+)
                                     " + "\r\n";
                xSql += @"         ) a " + "\r\n";
                xSql += @"  ) a " + "\r\n";

                if (rGubun != "all")
                {
                    if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                    {
                        xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                        xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                    }
                }

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
            return xDs; ;
        }


        /************************************************************
        * Function name : GetEduConfirmList
        * Purpose       : 교육대상자:이미확정된 대상자                
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetEduConfirmList(string[] rParams, CultureInfo rArgCultureInfo)
        {
            Database db = null;
            DataSet xDs = new DataSet();
            DataTable xDt = new DataTable();

            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");

                string xWhere = "";

                xSql = @" SELECT TO_CHAR(COURSE_BEGIN_DT, 'YYYYMMDD') AS COURSE_BEGIN_DT
                                 , TO_CHAR(COURSE_END_DT, 'YYYYMMDD') AS COURSE_END_DT
                            FROM T_OPEN_COURSE 
                           WHERE OPEN_COURSE_ID='" + rParams[3] + @"' ";
                xDt = base.ExecuteDataTable(db, xSql);

                string xCourseBeginDt = Convert.ToString(xDt.Rows[0]["COURSE_BEGIN_DT"]);
                string xCourseEndDt = Convert.ToString(xDt.Rows[0]["COURSE_END_DT"]);

                xSql = @" SELECT ESS_DUTY_STEP, OPT_DUTY_WORK FROM T_COURSE WHERE COURSE_ID='" + rParams[2] + @"' ";
                xDt = base.ExecuteDataTable(db, xSql);

                string[] xStrEssDutySteps = Convert.ToString(xDt.Rows[0]["ESS_DUTY_STEP"]).Split(',');
                string[] xStrOptDutyWorks = Convert.ToString(xDt.Rows[0]["OPT_DUTY_WORK"]).Split(',');

                string xStrWhere = "";
                if (xStrEssDutySteps.Length > 0 || xStrOptDutyWorks.Length > 0)
                {
                    string xStrEssDutyStep = "";
                    if (xStrEssDutySteps.Length > 0 && !string.IsNullOrEmpty(xStrEssDutySteps[0]))
                    {
                        xStrEssDutyStep += " U.DUTY_STEP IN (";
                        for (int i = 0; i < xStrEssDutySteps.Length; i++)
                        {
                            if (i > 0)
                                xStrEssDutyStep += ",";
                            xStrEssDutyStep += " '" + xStrEssDutySteps[i] + "' ";
                        }
                        xStrEssDutyStep += " ) ";
                    }

                    string xStrOptDutyWork = "";
                    if (xStrOptDutyWorks.Length > 0 && !string.IsNullOrEmpty(xStrOptDutyWorks[0]))
                    {
                        xStrOptDutyWork += " U.DUTY_WORK IN (";
                        for (int i = 0; i < xStrOptDutyWorks.Length; i++)
                        {
                            if (i > 0)
                                xStrOptDutyWork += ",";
                            xStrOptDutyWork += " '" + xStrOptDutyWorks[i] + "' ";
                        }
                        xStrOptDutyWork += " ) ";
                    }

                    if (!string.IsNullOrEmpty(xStrEssDutyStep) && !string.IsNullOrEmpty(xStrOptDutyWork))
                    {
                        xStrWhere = @"  AND (
                                        " + xStrEssDutyStep + @"
                                        OR " + xStrOptDutyWork + @"
                                        ) ";
                    }
                    else if (!string.IsNullOrEmpty(xStrEssDutyStep))
                    {
                        xStrWhere = @"  AND (
                                        " + xStrEssDutyStep + @"
                                        ) ";
                    }
                    else if (!string.IsNullOrEmpty(xStrOptDutyWork))
                    {
                        xStrWhere = @"  AND (
                                         " + xStrOptDutyWork + @"
                                        ) ";
                    }
                }


                //OPEN_COURSE_ID
                if (!Util.IsNullOrEmptyObject(rParams[3]))
                    xWhere += @" AND R.OPEN_COURSE_ID = '" + rParams[3] + "' " + "\r\n";


                xSql = @"   select *
                               from (
                                select  rownum rnum ";
                xSql += "               , a.*  " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += @"     SELECT  R.KEYS
                                       , R.OPEN_COURSE_ID
                                       , R.USER_ID
                                       , R.COURSE_RESULT_SEQ
                                       , R.DEPT_CODE
                                       , R.DEPT_NAME
                                       , R.DUTY_STEP
                                       , R.STEP_NAME
                                       , R.USER_NM_KOR
                                       , R.ORD_FDATE";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"         , (SELECT DEPT_NAME";
                }
                else
                {
                    xSql += @"         , (SELECT DEPT_ENAME";
                }
                //FROM HORDERDET@PUSHRMTLINK A, hdeptcode @PUSHRMTLINK B
                xSql += @"
                                            FROM HINDEV.HORDERDET A, HINDEV.hdeptcode B
                                           WHERE A.A_DEPT_CODE = B.DEPT_CODE
                                             AND A.SNO = R.USER_ID
                                             AND A.ORD_FDATE = REPLACE(R.ORD_FDATE, '.', '')
                                             AND ORD_KIND = 'AF8'
                                             AND A.CONFIRM_YN = 'Y') AS B_DEPT_NAME
                                       , R.INS_DT
                                       , R.USER_COURSE_END_DT
                                       , R.CONFIRM
                                       , R.NON_APPROVAL_CD
                                       , R.NON_APPROVAL_REMARK
                                       , COUNT(*) OVER() TOTALRECORDCOUNT
                                        ,R.PERSONAL_NO
                                        ,R.USER_ADDR
                                        ,R.MOBILE_PHONE
                                        ,R.OFFICE_PHONE
                                        ,(SELECT COMPANY_NM FROM T_COMPANY WHERE COMPANY_ID = R.COMPANY_ID) as COMPANY_NM
                                        ,R.INS_DTIME
                                 FROM  (
                                    ";
                xSql += @"          
                                    --수강신청한 사람 
                                    SELECT (R.USER_ID||'^'|| R.COURSE_RESULT_SEQ) AS KEYS
                                           , R.OPEN_COURSE_ID
                                           , R.USER_ID
                                           , R.COURSE_RESULT_SEQ
                                           , U.DEPT_CODE
                                           , (SELECT ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"                     DEPT_NAME ";
                }
                else
                {
                    xSql += @"                     DEPT_ENAME ";
                }
                xSql += @"                       FROM V_HDEPTCODE WHERE DEPT_CODE = U.DEPT_CODE) AS DEPT_NAME
                                           , U.DUTY_STEP
                                           , (SELECT ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"                     STEP_NAME ";
                }
                else
                {
                    xSql += @"                     STEP_ENAME ";
                }
                xSql += @"                       FROM V_HDUTYSTEP WHERE DUTY_STEP = U.DUTY_STEP) AS STEP_NAME
                                            ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"                    , USER_NM_KOR ";
                }
                else
                {
                    xSql += @"                    , USER_NM_ENG_FIRST || ' ' || USER_NM_ENG_LAST AS USER_NM_KOR ";
                }
                xSql += @"                                   
                                           , DECODE(F.ORD_FDATE,'', '', substr(F.ORD_FDATE, 0, 4)||'.'||substr(F.ORD_FDATE, 5, 2)||'.'||substr(F.ORD_FDATE, 7, 2)) AS ORD_FDATE
                                           , TO_CHAR(R.INS_DT, 'YYYY.MM.DD') AS INS_DT
                                           , (SELECT TO_CHAR(MAX(USER_COURSE_END_DT), 'YYYY.MM.DD') FROM (SELECT * FROM T_OPEN_COURSE WHERE COURSE_ID='" + rParams[2] + @"' AND OPEN_COURSE_ID < '" + rParams[3] + @"') C , T_COURSE_RESULT R WHERE C.OPEN_COURSE_ID = R.OPEN_COURSE_ID AND PASS_FLG='000001' AND USER_ID = U.USER_ID GROUP BY USER_ID) AS USER_COURSE_END_DT
                                           , R.NON_APPROVAL_CD
                                           , TO_CHAR(R.NON_APPROVAL_REMARK) AS NON_APPROVAL_REMARK
                                           , R.CONFIRM
                                            ,U.PERSONAL_NO
                                            ,U.USER_ADDR
                                            ,U.MOBILE_PHONE
                                            ,U.OFFICE_PHONE
                                            ,U.COMPANY_ID
                                           
                                            ,TO_CHAR(R.INS_DT, 'YYYY.MM.DD HH24:MI:SS') AS INS_DTIME
                                           --, S.STEP_SEQ
                                      FROM T_COURSE_RESULT R
                                            , T_USER U
                                            , V_HDEPTCODE D
                                            , V_HORDERDET_ORD_LATEST_OFFDATE F
                                            --, V_HDUTYSTEP S
                                      WHERE R.USER_ID = U.USER_ID
                                            --AND U.DUTY_STEP = S.DUTY_STEP
                                            AND U.DEPT_CODE = D.DEPT_CODE(+)
                                            AND R.USER_ID = F.SNO(+)
                                            AND R.CONFIRM = '1'
                                            " + xWhere;
                xSql += "              ) R " + "\r\n";
                xSql += "          ORDER BY R.INS_DTIME, R.DUTY_STEP, R.USER_NM_KOR, R.USER_ID " + "\r\n";
                xSql += @"      ) a " + "\r\n";
                xSql += " ) a " + "\r\n";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                
                xDt = base.ExecuteDataTable(db, xSql);

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

            return xDt;
        }

        /************************************************************
       * Function name : SetEduList
       * Purpose       : 교육대상자, 전체해상직원 저장
       * Input         : DataTable
       * Output        : DataTable
       *************************************************************/
        public string SetEduList(DataTable rDt, string rGubun)
        {
            string xSql = string.Empty;
            string xRtn = Boolean.FalseString;

            try
            {
                Database db = null;

                try
                {
                    db = base.GetDataBase("LMS");
                    using (OracleConnection cnn = (OracleConnection)db.CreateConnection())
                    {
                        cnn.Open();
                        OracleTransaction trx = cnn.BeginTransaction();
                        OracleCommand cmd = null;
                        OracleParameter[] xPara = null;
                        try
                        {
                            cmd = base.GetSqlCommand(db);

                            foreach (DataRow drDet in rDt.Rows)
                            {
                                //COURSE_ID || '^' || R.OPEN_COURSE_ID || '^' || U.USER_ID||'^'|| R.COURSE_RESULT_SEQ
                                string[] keys = Util.Split(drDet["keys"].ToString(), "^", 4);
                                string xCourseID = keys[0];
                                string xOpenCourseID = keys[1];
                                string xUserID = keys[2];
                                string xCourseResultSeq = keys[3];
                                //if (drDet["confirm"].ToString() == "1")
                                //{

                                object xExitOpenCourseID = null;
                                if (!Util.IsNullOrEmptyObject(xCourseResultSeq))
                                {
                                    xSql = " SELECT OPEN_COURSE_ID FROM T_COURSE_RESULT WHERE ROWNUM = 1 AND USER_ID='" + xUserID + "' AND OPEN_COURSE_ID='" + xOpenCourseID + "' AND COURSE_RESULT_SEQ='" + xCourseResultSeq + "' ";
                                    cmd.CommandText = xSql;
                                    xExitOpenCourseID = base.ExecuteScalar(db, cmd, trx);
                                }
                                else
                                {
                                    xSql = @" SELECT OPEN_COURSE_ID
                                                         , (select max(COURSE_RESULT_SEQ) from T_COURSE_RESULT where USER_ID='" + xUserID + @"' AND OPEN_COURSE_ID='" + xOpenCourseID + @"' ) as course_result_seq 
                                                    FROM T_COURSE_RESULT 
                                                   WHERE ROWNUM = 1
                                                         AND USER_ID='" + xUserID + @"' 
                                                         AND OPEN_COURSE_ID='" + xOpenCourseID + "' ";

                                    cmd.CommandText = xSql;
                                    DataSet ds = base.ExecuteDataSet(db, cmd, trx);
                                    DataTable xDtCourseResultSeq = ds.Tables[0];
                                    if (xDtCourseResultSeq.Rows.Count > 0)
                                    {
                                        xExitOpenCourseID = xDtCourseResultSeq.Rows[0]["open_course_id"];
                                        xCourseResultSeq = Convert.ToString(xDtCourseResultSeq.Rows[0]["course_result_seq"]);
                                    }
                                }

                                if (Util.IsNullOrEmptyObject(xExitOpenCourseID))
                                {
                                    xSql = @" 
                                            INSERT INTO t_course_result ( 
                                                   user_id                   -- 사용자 ID
                                                   ,open_course_id           -- 개설과정 ID
                                                   ,course_result_seq        -- 이수차수
                                                   ,user_company_id          -- 사용자 회사
                                                   ,user_dept_code           -- 사용자 부서
                                                   ,user_duty_step           -- 사용자 직급
                                                   ,approval_flg             -- 승인여부
                                                   ,pass_flg                 -- 이수여부
                                                   ,user_course_begin_dt     -- 학습 시작일자
                                                   ,user_course_end_dt       -- 학습 종료일자
                                                   ,ins_id                   -- 작성자 ID
                                                   ,ins_dt                   -- 작성일자
                                                   ,confirm                  -- 확정 flag
                                            )  
                                            SELECT u.user_id 
                                                   , '" + xOpenCourseID + @"'
                                                   ,(select (NVL(max(course_result_seq),0)+1) from t_course_result WHERE USER_ID=:user_id AND OPEN_COURSE_ID= :open_course_id)
                                                   ,u.company_id        
                                                   ,u.dept_code          
                                                   ,u.duty_step          
                                                   ,'000003' --승인대기           
                                                   ,'000004' --수강
                                                   ,(select course_begin_dt from t_open_course WHERE OPEN_COURSE_ID= :open_course_id )
                                                   ,(select course_end_dt from t_open_course WHERE OPEN_COURSE_ID= :open_course_id )
                                                   ,u.user_id             
                                                   ,sysdate
                                                   ,:confirm
                                              FROM t_user u
                                             WHERE USER_ID= :user_id 
                                        ";

                                    OracleParameter[] xSqlParamsAddFile = new OracleParameter[3];
                                    xSqlParamsAddFile[0] = base.AddParam("open_course_id", OracleType.VarChar, xOpenCourseID);
                                    xSqlParamsAddFile[1] = base.AddParam("confirm", OracleType.VarChar, Convert.ToString(drDet["confirm"]));
                                    xSqlParamsAddFile[2] = base.AddParam("user_id", OracleType.VarChar, xUserID);
                                    cmd.CommandText = xSql;
                                    base.Execute(db, cmd, xSqlParamsAddFile, trx);
                                }
                                else
                                {
                                    xSql = @" UPDATE T_COURSE_RESULT 
                                                     SET NON_APPROVAL_CD = :NON_APPROVAL_CD
                                                         , NON_APPROVAL_REMARK = :NON_APPROVAL_REMARK
                                                         , CONFIRM = :CONFIRM
                                                         , SEND_FLG = DECODE(:CONFIRM, '0', DECODE(SEND_FLG, '3', '1', SEND_FLG), SEND_FLG)   --3인 경우 선박에서 날라온 경우임
                                                         , APPROVAL_FLG = (CASE WHEN (SEND_FLG='3' AND '0' = :CONFIRM) THEN '000002' ELSE APPROVAL_FLG END) --미확정 미승인처리
                                                   WHERE USER_ID= :USER_ID
                                                         AND OPEN_COURSE_ID= :OPEN_COURSE_ID
                                                         AND COURSE_RESULT_SEQ= :COURSE_RESULT_SEQ
                                                    ";
                                    xPara = new OracleParameter[6];
                                    xPara[0] = base.AddParam("NON_APPROVAL_CD", OracleType.VarChar, drDet["NON_APPROVAL_CD"]);
                                    xPara[1] = base.AddParam("NON_APPROVAL_REMARK", OracleType.VarChar, drDet["NON_APPROVAL_REMARK"]);
                                    xPara[2] = base.AddParam("confirm", OracleType.VarChar, Convert.ToString(drDet["confirm"]));
                                    xPara[3] = base.AddParam("USER_ID", OracleType.VarChar, xUserID);
                                    xPara[4] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, xOpenCourseID);
                                    xPara[5] = base.AddParam("COURSE_RESULT_SEQ", OracleType.Number, xCourseResultSeq);

                                    cmd.CommandText = xSql;
                                    base.Execute(db, cmd, xPara, trx);

                                }


                                //}
                                //else
                                //{
                                //    //미승인으로 관리

                                //    //xSql = " DELETE FROM T_COURSE_RESULT WHERE USER_ID='" + xUserID + "' AND OPEN_COURSE_ID='" + xOpenCourseID + "' AND COURSE_RESULT_SEQ='" + xCourseResultSeq + "' ";
                                //    //cmd.CommandText = xSql;
                                //    //base.Execute(db, cmd, trx);
                                //}

                            }

                            //선박발송
                            //미확정인 경우 선박 발송
                            xPara = new OracleParameter[2];
                            xPara[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_COURSE_REULST_APPROVAL");
                            xPara[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_EDU_APPLY");
                            base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", xPara, trx);


                            xRtn = Boolean.TrueString;
                            trx.Commit();
                        }
                        catch (Exception ex)
                        {
                            trx.Rollback();

                            bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                            if (rethrow) throw;
                        }
                        finally
                        {
                            if (cmd != null)
                                cmd.Dispose();
                            if (cnn != null)
                            {
                                if (cnn.State == ConnectionState.Open)
                                {
                                    cnn.Close();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                    if (rethrow) throw;
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
        * Function name : GetEduApprovalList
        * Purpose       : 수강신청/승인리스트
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataSet
        *************************************************************/
        public DataSet GetEduApprovalList(string[] rParams
                                            , string rGubun, CultureInfo rArgCultureInfo)
        {

            Database db = null;
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");
                using (OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection())
                {
                    try
                    {
                        string xWhere = "";
                        DataTable xDt = null;

                        //교육기간
                        xWhere += @" AND C.COURSE_TYPE != '000005' " + "\r\n";
                        xWhere += @" AND O.COURSE_BEGIN_DT >= to_date('" + rParams[2] + "','YYYY.MM.DD') " + "\r\n";
                        xWhere += @" AND O.COURSE_BEGIN_DT <= to_date('" + rParams[3] + "','YYYY.MM.DD') " + "\r\n";

                        ////교육유형
                        if (rParams[4] != "*" && !Util.IsNullOrEmptyObject(rParams[4]))
                            xWhere += @" AND C.COURSE_TYPE = '" + rParams[4] + "' " + "\r\n";

                        ////과정명


                        if (!Util.IsNullOrEmptyObject(rParams[5]))
                        {
                            if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                            {
                                xWhere += @" AND UPPER(C.COURSE_NM) LIKE '%" + rParams[5].ToUpper() + "%' " + "\r\n";
                            }
                            else
                            {
                                xWhere += @" AND UPPER(C.COURSE_NM_ABBR) LIKE '%" + rParams[5].ToUpper() + "%' " + "\r\n";
                            }
                        }
                        xSql = @"
                            SELECT COUNT(*)                              
                              FROM T_OPEN_COURSE O
                                   , T_COURSE C
                             WHERE O.COURSE_ID = C.COURSE_ID
                                   " + xWhere;
                        xDt = base.ExecuteDataTable(db, xSql);
                        base.MergeTable(ref xDs, xDt, "table1");

                        xSql = "";
                        xSql += @"   
                             select *
                               from (
                                select  rownum rnum ";
                        xSql += "       , A.* " + "\r\n";
                        xSql += "     FROM ( " + "\r\n";
                        xSql += @"     SELECT 
                                              O.COURSE_ID ||'^'|| O.OPEN_COURSE_ID as KEYS
                                            , O.COURSE_ID
                                            , O.OPEN_COURSE_ID
                                            , O.COURSE_TYPE ";
                        if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                        {
                            xSql += @"      , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD='0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE_NM 
                                            , C.COURSE_NM ";
                        }
                        else
                        {
                            xSql += @"      , (SELECT D_ENM FROM T_CODE_DETAIL WHERE M_CD='0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE_NM
                                            , C.COURSE_NM_ABBR AS COURSE_NM ";

                        }
                        xSql += @"          , TO_CHAR(O.COURSE_BEGIN_DT,'YYYY.MM.DD') ||'~'|| TO_CHAR(O.COURSE_END_DT,'YYYY.MM.DD') AS COURSE_DATE
                                            , O.COURSE_SEQ
                                            , C.CLASS_MAN_COUNT
                                            , (SELECT COUNT(*) FROM T_COURSE_RESULT R, T_USER U WHERE R.USER_ID=U.USER_ID AND R.OPEN_COURSE_ID = O.OPEN_COURSE_ID) AS CNT_APP                                     --신청
                                            , (SELECT COUNT(*) FROM T_COURSE_RESULT R, T_USER U WHERE R.CONFIRM='1' AND R.USER_ID=U.USER_ID AND R.OPEN_COURSE_ID = O.OPEN_COURSE_ID) AS CNT_RCV                                     --접수
                                            , (SELECT COUNT(*) FROM T_COURSE_RESULT R, T_USER U WHERE R.CONFIRM='1' AND R.USER_ID=U.USER_ID AND R.OPEN_COURSE_ID = O.OPEN_COURSE_ID AND R.APPROVAL_FLG = '000001') AS CNT_APPR        --승인
                                            , (SELECT COUNT(*) FROM T_COURSE_RESULT R, T_USER U WHERE R.CONFIRM='1' AND R.USER_ID=U.USER_ID AND R.OPEN_COURSE_ID = O.OPEN_COURSE_ID AND R.APPROVAL_FLG != '000001') AS CNT_NONAPPR     --미승인
                                            , (SELECT COUNT(*) FROM T_COURSE_RESULT R, T_USER U WHERE R.CONFIRM='1' AND R.USER_ID=U.USER_ID AND R.OPEN_COURSE_ID = O.OPEN_COURSE_ID AND R.COURSE_START_FLG = 'Y') AS CNT_IN                  --교육입과
                                      FROM T_OPEN_COURSE O
                                            , T_COURSE C
                                     WHERE O.COURSE_ID = C.COURSE_ID
                                            " + xWhere;
                        xSql += @"   ORDER BY O.INS_DT desc, C.COURSE_NM " + "\r\n";
                        xSql += @"         ) A " + "\r\n";
                        xSql += @"  ) A " + "\r\n";

                        if (rGubun != "all")
                        {
                            if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                            {
                                xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                                xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                            }
                        }

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
                        if (xCnnLMS != null) { if (xCnnLMS.State == ConnectionState.Open) xCnnLMS.Close(); }
                    }
                }
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
            return xDs; ;
        }

        /************************************************************
       * Function name : GetEduApprovalExcel
       * Purpose       : 수강신청/승인 수강자 리스트가져오기
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataSet
       *************************************************************/
        public DataSet GetEduApprovalExcel(string[] rParams
                                            , DataTable rDt)
        {
            Database db = null;
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");

                string xWhere = "";
                DataTable xDt = null;

                for (int i = 0; i < rDt.Rows.Count; i++)
                {
                    //Open_Course_id
                    xWhere = @" AND OPEN_COURSE_ID = '" + rDt.Rows[i]["open_course_id"].ToString() + "' " + "\r\n";

                    xSql = @"
                    SELECT TRIM(R.USER_ID) USER_ID
                            , C.COURSE_NM
                            , TO_CHAR(COURSE_BEGIN_DT, 'YYYY.MM.DD') AS COURSE_BEGIN_DT
                            , TO_CHAR(COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_END_DT
                            , S.STEP_NAME
                            , U.USER_NM_KOR
                            , P.COMPANY_NM
                            , HINDEV.CRYPTO_AES256.DEC_AES(U.PERSONAL_NO) AS PERSONAL_NO
                            , U.USER_ADDR
                            , U.MOBILE_PHONE
                            , C.COURSE_DAY
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0006' AND D_CD = C.COURSE_TYPE) as COURSE_TYPE_NM
                            , DECODE(U.trainee_class, '000001', 'C0127', '000002', 'C0123', '') AS T_TRAINEE_CLASS --훈련생구분
                            , DECODE(U.trainee_class, '000001', '00000', '000002', 'C0069', '') AS C_TRAINEE_CLASS --비정규직구분
                            , REPLACE(P.TAX_NO, '-', '') AS TAX_NO
                            , P.EMPOLY_INS_NO       -- 고용보험번호
                            , C.COURSE_TYPE         -- 훈련구분
                            , '5' as LAST_SCHOOL    -- 최종학력
                            , 'N' AS IS_DORMITORY   -- 기숙사사용여부
                            , 'Y' AS IS_MEALS       -- 식비사용여부
                            , ''  AS IS_SUBSTITUTE  -- 대체인력                            , U.EMAIL_ID
                      FROM (SELECT * FROM T_COURSE_RESULT WHERE 1=1 " + xWhere + @" AND APPROVAL_FLG = '000001') R
                            , T_USER U
                            , T_OPEN_COURSE O
                            , T_COURSE C
                            , V_HDUTYSTEP S
                            , T_COMPANY P
                     WHERE R.USER_ID = U.USER_ID
                            AND R.OPEN_COURSE_ID = O.OPEN_COURSE_ID
                            AND O.COURSE_ID = C.COURSE_ID
                            AND U.DUTY_STEP = S.DUTY_STEP(+)
                            AND U.COMPANY_ID = P.COMPANY_ID
                      ORDER BY S.DUTY_STEP, U.USER_ID
                           ";
                    xDt = base.ExecuteDataTable(db, xSql);
                    base.MergeTable(ref xDs, xDt, "table" + i.ToString());
                }

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
            return xDs; ;
        }

        /************************************************************
        * Function name : GetEduApprovalUserList
        * Purpose       : 수강신청/승인리스트 USER 리스트
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataSet
        *************************************************************/
        public DataSet GetEduApprovalUserList(string[] rParams
                                            , string rGubun, CultureInfo rArgCultureInfo)
        {
            Database db = null;
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");
                using (OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection())
                {
                    try
                    {
                        string xWhere = "";
                        DataTable xDt = null;
                        xWhere += @" AND R.OPEN_COURSE_ID = '" + rParams[5] + "' AND R.CONFIRM='1' " + "\r\n";

                        //승인상태
                        if (rParams[3] != "*" && !Util.IsNullOrEmptyObject(rParams[3]))
                            xWhere += @" AND R.APPROVAL_FLG = '" + rParams[3] + "' " + "\r\n";

                        xSql = @"
                            SELECT  COUNT(*)                              
                              FROM  (SELECT * FROM T_COURSE_RESULT R WHERE 1=1 " + xWhere + @") R
                                    , T_USER U
                                    , T_COMPANY C
                                    , V_HDEPTCODE D
                                    , V_HDUTYSTEP S
                             WHERE  R.USER_ID = U.USER_ID(+)
                                    AND U.COMPANY_ID = C.COMPANY_ID(+)
                                    AND U.DEPT_CODE = D.DEPT_CODE(+)
                                    AND U.DUTY_STEP = S.DUTY_STEP(+) " + "\r\n"
                                  + xWhere;
                        xDt = base.ExecuteDataTable(db, xSql);
                        base.MergeTable(ref xDs, xDt, "table1");

                        xSql = "";
                        xSql += @"   
                             select *
                               from (
                                select  rownum rnum ";
                        xSql += "       , a.* " + "\r\n";
                        xSql += "     FROM ( " + "\r\n";
                        xSql += @"     SELECT 
                                            R.USER_ID ||'^'|| R.OPEN_COURSE_ID||'^'|| R.COURSE_RESULT_SEQ AS KEYS
                                            , R.OPEN_COURSE_ID
                                            , R.COURSE_RESULT_SEQ
                                            , TO_CHAR(R.INS_DT, 'YYYY.MM.DD') AS INS_DT
                                            , TO_CHAR(R.INS_DT, 'HH24:MI:SS') AS INS_TIME
                                            , R.USER_ID ";
                        if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                        {
                            xSql += @"      , U.USER_NM_KOR ";
                        }
                        else
                        {
                            xSql += @"      , U.USER_NM_ENG_FIRST || ' ' || U.USER_NM_ENG_LAST AS USER_NM_KOR ";
                        }
                        xSql += @"          , U.COMPANY_ID ";
                        if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                        {
                            xSql += @"      , C.COMPANY_NM ";
                        }
                        else
                        {
                            xSql += @"      , C.COMPANY_NM_ENG AS COMPANY_NM ";
                        }
                        xSql += @"          , REGEXP_REPLACE(HINDEV.CRYPTO_AES256.DEC_AES(U.PERSONAL_NO),'\d','*', 9) AS PERSONAL_NO
                                            , U.DEPT_CODE ";
                        if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                        {
                            xSql += @"      , D.DEPT_NAME ";
                        }
                        else
                        {
                            xSql += @"      , D.DEPT_ENAME AS DEPT_NAME ";
                        }
                        xSql += @"          , U.DUTY_STEP ";

                        if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                        {
                            xSql += @"      , S.STEP_NAME ";
                        }
                        else
                        {
                            xSql += @"      , S.STEP_ENAME AS STEP_NAME ";
                        }
                        xSql += @"          , R.APPROVAL_FLG ";
                        if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                        {
                            xSql += @"      , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD='0019' AND D_CD = R.APPROVAL_FLG) AS APPROVAL_FLG_NM ";
                        }
                        else
                        {
                            xSql += @"      , (SELECT D_ENM FROM T_CODE_DETAIL WHERE M_CD='0019' AND D_CD = R.APPROVAL_FLG) AS APPROVAL_FLG_NM ";
                        }
                        xSql += @"          , U.TRAINEE_CLASS AS EMPLOYED_STATE 
                                            --, (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0062' AND D_CD = U.TRAINEE_CLASS) AS EMPLOYED_STATE 
                                            , TO_CHAR(U.ENTER_DT, 'YYYY.MM.DD') AS ENTER_DT
                                            , R.NON_APPROVAL_CD
                                            , R.NON_APPROVAL_REMARK
                                            , TO_CHAR(R.APPROVAL_DT, 'YYYY.MM.DD') AS APPROVAL_DT
                                            , R.COURSE_START_FLG
                                      FROM (SELECT * FROM T_COURSE_RESULT R WHERE 1=1 " + xWhere + @") R
                                            , T_USER U
                                            , T_COMPANY C
                                            , V_HDEPTCODE D
                                            , V_HDUTYSTEP S
                                     WHERE R.USER_ID = U.USER_ID
                                            AND U.COMPANY_ID = C.COMPANY_ID(+)
                                            AND U.DEPT_CODE = D.DEPT_CODE(+)
                                            AND U.DUTY_STEP = S.DUTY_STEP(+)
                                            
                                    ORDER BY R.INS_DT, U.DUTY_STEP, U.USER_NM_KOR, R.USER_ID";
                        xSql += @"         ) a " + "\r\n";
                        xSql += @" ) a " + "\r\n";

                        if (rGubun != "all")
                        {
                            if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                            {
                                xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                                xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                            }
                        }

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
                        if (xCnnLMS != null) { if (xCnnLMS.State == ConnectionState.Open) xCnnLMS.Close(); }
                    }
                }
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
            return xDs; ;
        }

        /************************************************************
       * Function name : SetEduApprovalUserList
       * Purpose       : 사용자 승인처리
       * Input         : DataTable
       * Output        : 
       *************************************************************/
        public string SetEduApprovalUserList(DataTable rDt, string rGubun)
        {
            string xSql = string.Empty;
            string xRtn = Boolean.FalseString;

            try
            {
                Database db = null;

                try
                {
                    db = base.GetDataBase("LMS");
                    using (OracleConnection cnn = (OracleConnection)db.CreateConnection())
                    {
                        cnn.Open();
                        OracleTransaction trx = cnn.BeginTransaction();
                        OracleCommand cmd = null;
                        try
                        {
                            cmd = base.GetSqlCommand(db);

                            OracleParameter[] xPara = null;
                            foreach (DataRow drDet in rDt.Rows)
                            {
                                //승인처리 한 경우는 다시 승인처리 하면 안됨

                                //R.USER_ID ||'^'|| R.OPEN_COURSE_ID||'^'|| R.COURSE_RESULT_SEQ
                                string[] keys = Util.Split(drDet["keys"].ToString(), "^", 3);
                                string xUserID = keys[0];
                                string xOpenCourseID = keys[1];
                                string xCourseResultSeq = keys[2];

                                xSql = @" UPDATE T_COURSE_RESULT 
                                             SET APPROVAL_FLG= :APPROVAL_FLG
                                                 , EMPLOYED_STATE= :EMPLOYED_STATE
                                                 , INSURANCE_FLG= :INSURANCE_FLG
                                                 , INSURANCE_DT= to_date(:INSURANCE_DT, 'yyyy.mm.dd')
                                                 , NON_APPROVAL_CD = :non_approval_cd
                                                 , NON_APPROVAL_REMARK = :non_approval_remark
                                                 , COURSE_START_FLG= :COURSE_START_FLG
                                                 , APPROVAL_DT = DECODE(:APPROVAL_FLG, '000001', SYSDATE, APPROVAL_DT)
                                                 , SEND_FLG = DECODE(SEND_FLG, '3', '1', SEND_FLG) --3인 경우 선박에서날라온 경우임
                                           WHERE USER_ID= :USER_ID
                                                 AND OPEN_COURSE_ID= :OPEN_COURSE_ID 
                                                 AND COURSE_RESULT_SEQ= :COURSE_RESULT_SEQ 
                                ";

                                xPara = new OracleParameter[11];
                                xPara[0] = base.AddParam("APPROVAL_FLG", OracleType.VarChar, drDet["APPROVAL_FLG"]);
                                xPara[1] = base.AddParam("EMPLOYED_STATE", OracleType.VarChar, drDet["EMPLOYED_STATE"]);
                                xPara[2] = base.AddParam("INSURANCE_FLG", OracleType.VarChar, drDet["INSURANCE_FLG"]);
                                xPara[3] = base.AddParam("INSURANCE_DT", OracleType.VarChar, drDet["INSURANCE_DT"]);
                                xPara[4] = base.AddParam("non_approval_cd", OracleType.VarChar, drDet["non_approval_cd"]);
                                xPara[5] = base.AddParam("non_approval_remark", OracleType.VarChar, drDet["non_approval_remark"]);
                                xPara[6] = base.AddParam("COURSE_START_FLG", OracleType.VarChar, drDet["COURSE_START_FLG"]);
                                xPara[7] = base.AddParam("APPROVAL_FLG", OracleType.VarChar, drDet["APPROVAL_FLG"]);
                                xPara[8] = base.AddParam("USER_ID", OracleType.VarChar, xUserID);
                                xPara[9] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, xOpenCourseID);
                                xPara[10] = base.AddParam("COURSE_RESULT_SEQ", OracleType.VarChar, xCourseResultSeq);
                                cmd.CommandText = xSql;
                                base.Execute(db, cmd, xPara, trx);
                            }

                            //선박발송
                            xPara = new OracleParameter[2];
                            xPara[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_COURSE_REULST_APPROVAL");
                            xPara[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_EDU_APPLY");
                            base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", xPara, trx);


                            trx.Commit();
                            xRtn = Boolean.TrueString;

                        }
                        catch (Exception ex)
                        {
                            trx.Rollback();

                            bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                            if (rethrow) throw;
                        }
                        finally
                        {
                            db = null;

                            if (cmd != null)
                                cmd.Dispose();
                            if (cnn != null)
                            {
                                if (cnn.State == ConnectionState.Open)
                                {
                                    cnn.Close();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                    if (rethrow) throw;
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
        * Function name : GetEduProgressList
        * Purpose       : 학습진행관리리스트
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataSet
        *************************************************************/
        public DataSet GetEduProgressList(string[] rParams
                                            , string rGubun, CultureInfo rArgCultureInfo)
        {
            Database db = null;
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");

                string xWhere = "";
                DataTable xDt = null;
                //교육기간
                xWhere += @" AND O.COURSE_BEGIN_DT >= TO_DATE('" + rParams[2] + "','YYYY.MM.DD') " + "\r\n";
                xWhere += @" AND O.COURSE_BEGIN_DT <= TO_DATE('" + rParams[3] + "','YYYY.MM.DD') " + "\r\n";

                //교육유형
                if (rParams[4] != "*" && !Util.IsNullOrEmptyObject(rParams[4]))
                    xWhere += @" AND C.COURSE_TYPE = '" + rParams[4] + "' " + "\r\n";

                //과정명

                if (!Util.IsNullOrEmptyObject(rParams[5]))
                    xWhere += @" AND UPPER(C.COURSE_NM) LIKE  '%" + rParams[5].ToUpper() + "%' " + "\r\n";

                xSql = @"
                    SELECT  COUNT(*)                              
                      FROM  (SELECT * FROM T_COURSE_RESULT WHERE APPROVAL_FLG='000001') R
                            , T_OPEN_COURSE O
                            , T_COURSE C
                            , T_USER U
                     WHERE R.OPEN_COURSE_ID = O.OPEN_COURSE_ID
                            AND C.COURSE_ID = O.COURSE_ID
                            AND R.USER_ID = U.USER_ID
                     " + xWhere;
                xDt = base.ExecuteDataTable(db, xSql);
                base.MergeTable(ref xDs, xDt, "table1");

                xSql = "";
                xSql += @"   
                             select *
                               from (
                                select  rownum rnum ";
                xSql += "               , a.* " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += @"     SELECT R.OPEN_COURSE_ID
                                    , R.USER_ID
                                    , R.COURSE_RESULT_SEQ
                                    , C.COURSE_ID ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"      , C.COURSE_NM ";
                }
                else
                {
                    xSql += @"      , C.COURSE_NM_ABBR AS COURSE_NM ";

                }
                xSql += @"          , TO_CHAR(O.COURSE_BEGIN_DT,'YYYY.MM.DD')|| '~' || TO_CHAR(O.COURSE_END_DT,'YYYY.MM.DD') AS COURSE_DT
                                    , U.DUTY_STEP ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"      , (SELECT STEP_NAME FROM V_HDUTYSTEP WHERE DUTY_STEP = U.DUTY_STEP) AS STEP_NAME
                                    , U.USER_NM_KOR ";
                }
                else
                {
                    xSql += @"      , (SELECT STEP_ENAME FROM V_HDUTYSTEP WHERE DUTY_STEP = U.DUTY_STEP) AS STEP_NAME
                                    , U.USER_NM_ENG_FIRST || ' ' || U.USER_NM_ENG_LAST AS USER_NM_KOR ";
                }
                xSql += @"          , R.COURSE_START_FLG
                                    , R.PROGRESS_RATE
                                    , R.ASSESS_SCORE
                                    , R.REPORT_SCORE
                                    , R.TOTAL_SCORE
                                    , R.PASS_FLG ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"      , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD='0010' AND D_CD = R.PASS_FLG) AS PASS_FLG_NM  ";
                }
                else
                {
                    xSql += @"      , (SELECT D_ENM FROM T_CODE_DETAIL WHERE M_CD='0010' AND D_CD = R.PASS_FLG) AS PASS_FLG_NM  ";
                }
                xSql += @"   FROM (SELECT * FROM T_COURSE_RESULT WHERE APPROVAL_FLG='000001') R
                                    , T_OPEN_COURSE O
                                    , T_COURSE C
                                    , T_USER U
                                    , V_HDUTYSTEP S
                             WHERE R.OPEN_COURSE_ID = O.OPEN_COURSE_ID
                                    AND C.COURSE_ID = O.COURSE_ID
                                    AND R.USER_ID = U.USER_ID
                                    AND U.DUTY_STEP = S.DUTY_STEP(+)
                                    " + xWhere;
                xSql += " ORDER BY O.COURSE_BEGIN_DT desc, U.USER_NM_KOR ";
                xSql += @"         ) a " + "\r\n";
                xSql += @"  ) a " + "\r\n";

                if (rGubun != "all")
                {
                    if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                    {
                        xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                        xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                    }
                }

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
            return xDs; ;
        }
        
        /************************************************************
        * Function name : GetEduPassList
        * Purpose       : 수료처리리스트
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataSet
        *************************************************************/
        public DataSet GetEduPassList(string[] rParams
                                            , string rGubun, CultureInfo rArgCultureInfo)
        {
            Database db = null;
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");

                string xWhere = "";
                DataTable xDt = null;

                //교육기간
                xWhere += @" AND O.COURSE_BEGIN_DT >= TO_DATE('" + rParams[2] + "','YYYY.MM.DD') " + "\r\n";
                xWhere += @" AND O.COURSE_BEGIN_DT <= TO_DATE('" + rParams[3] + "','YYYY.MM.DD') " + "\r\n";

                //교육유형
                if (rParams[4] != "*" && !Util.IsNullOrEmptyObject(rParams[4]))
                    xWhere += @" AND C.COURSE_TYPE = '" + rParams[4] + "' " + "\r\n";

                //과정명


                if (!Util.IsNullOrEmptyObject(rParams[5]))
                {
                    if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                    {
                        xWhere += @" AND UPPER(C.COURSE_NM) LIKE  '%" + rParams[5].ToUpper() + "%' " + "\r\n";
                    }
                    else
                    {
                        xWhere += @" AND UPPER(C.COURSE_NM_ABBR) LIKE  '%" + rParams[5].ToUpper() + "%' " + "\r\n";
                    }
                }

                xSql = @"
                    SELECT COUNT(*)                              
                      FROM T_COURSE C
                            , T_OPEN_COURSE O
                     WHERE C.COURSE_ID = O.COURSE_ID
                           " + xWhere;
                xDt = base.ExecuteDataTable(db, xSql);
                base.MergeTable(ref xDs, xDt, "table1");

                xSql = "";
                xSql += @"   
                             select *
                               from (
                                select  rownum rnum ";
                xSql += "               , A.* " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += @"  SELECT C.COURSE_ID||'^'||O.OPEN_COURSE_ID AS KEYS
                                   , C.COURSE_ID
                                   , O.OPEN_COURSE_ID
                                   , C.COURSE_TYPE ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"     , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD='0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE_NM
                                   , C.COURSE_NM ";
                }
                else
                {
                    xSql += @"     , (SELECT D_ENM FROM T_CODE_DETAIL WHERE M_CD='0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE_NM
                                   , C.COURSE_NM_ABBR AS COURSE_NM ";
                }
                xSql += @"         , O.COURSE_BEGIN_DT
                                   , O.COURSE_END_DT
                                   , TO_CHAR(O.COURSE_BEGIN_DT,'YYYY.MM.DD') ||'~'|| TO_CHAR(O.COURSE_END_DT,'YYYY.MM.DD') AS COURSE_DATE
                                   , (SELECT COUNT(*) FROM T_COURSE_RESULT R, T_USER U WHERE R.USER_ID=U.USER_ID AND R.OPEN_COURSE_ID = O.OPEN_COURSE_ID AND R.APPROVAL_FLG = '000001') AS CNT_APPR
                                   , (SELECT COUNT(*) FROM T_COURSE_RESULT R, T_USER U WHERE R.USER_ID=U.USER_ID AND R.OPEN_COURSE_ID = O.OPEN_COURSE_ID AND R.APPROVAL_FLG = '000001' and R.PASS_FLG = '000001') AS CNT_PASS --M_CD : 0010
                                   , (SELECT COUNT(*) FROM T_COURSE_RESULT R, T_USER U WHERE R.USER_ID=U.USER_ID AND R.OPEN_COURSE_ID = O.OPEN_COURSE_ID AND R.APPROVAL_FLG = '000001' and R.PASS_FLG <> '000001') AS CNT_NON_PASS 
                              FROM T_COURSE C
                                    , T_OPEN_COURSE O
                             WHERE C.COURSE_ID = O.COURSE_ID
                                    " + xWhere;
                xSql += " ORDER BY O.COURSE_BEGIN_DT DESC, C.COURSE_NM ";
                xSql += @"         ) A " + "\r\n";
                xSql += @"  ) A " + "\r\n";

                if (rGubun != "all")
                {
                    if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                    {
                        xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                        xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                    }
                }

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

            return xDs; ;
        }

        /************************************************************
        * Function name : GetEduPassUserList
        * Purpose       : 수강신청/승인리스트 USER 리스트
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataSet
        *************************************************************/
        public DataSet GetEduPassUserList(string[] rParams
                                            , string rGubun, CultureInfo rArgCultureInfo)
        {
            DataSet xDs = new DataSet();
            string xSql = string.Empty;
            Database db = base.GetDataBase("LMS");

            try
            {
                string xWhere = "";
                DataTable xDt = null;
                xWhere += @" AND O.OPEN_COURSE_ID = '" + rParams[5] + "' " + "\r\n";

                xSql = @"
                    SELECT COUNT(*)                              
                      FROM T_OPEN_COURSE O
                            , (SELECT * FROM T_COURSE_RESULT WHERE APPROVAL_FLG = '000001') R 
                            , T_USER U
                            , T_COMPANY C
                     WHERE O.OPEN_COURSE_ID = R.OPEN_COURSE_ID
                            AND R.USER_ID = U.USER_ID
                            AND U.COMPANY_ID = C.COMPANY_ID(+) " + "\r\n"
                          + xWhere;
                xDt = base.ExecuteDataTable(db, xSql);
                base.MergeTable(ref xDs, xDt, "table1");

                xSql = "";
                xSql += @"   
                             select *
                               from (
                                select  rownum rnum ";
                xSql += "               , a.* " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += @"     SELECT R.USER_ID ||'^'|| R.OPEN_COURSE_ID||'^'|| R.COURSE_RESULT_SEQ AS KEYS
                                    , R.OPEN_COURSE_ID
                                    , R.COURSE_RESULT_SEQ
                                    , R.USER_ID ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"      , U.USER_NM_KOR ";
                }
                else
                {
                    xSql += @"      , U.USER_NM_ENG_FIRST || ' ' || U.USER_NM_ENG_LAST AS USER_NM_KOR ";
                }
                xSql += @"         , U.COMPANY_ID ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"      , C.COMPANY_NM ";
                }
                else
                {
                    xSql += @"      , C.COMPANY_NM_ENG AS COMPANY_NM ";
                }

                xSql += @"          , U.DEPT_CODE ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @" ,    (SELECT DEPT_NAME FROM V_HDEPTCODE WHERE DEPT_CODE = U.DEPT_CODE) AS DEPT_NAME ";
                }
                else
                {
                    xSql += @" ,    (SELECT DEPT_ENAME FROM V_HDEPTCODE WHERE DEPT_CODE = U.DEPT_CODE) AS DEPT_NAME ";
                }
                xSql += @"          , U.DUTY_STEP ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"     , (SELECT STEP_NAME FROM V_HDUTYSTEP WHERE DUTY_STEP = U.DUTY_STEP) AS STEP_NAME ";
                }
                else
                {
                    xSql += @"     , (SELECT STEP_ENAME FROM V_HDUTYSTEP WHERE DUTY_STEP = U.DUTY_STEP) AS STEP_NAME ";
                }
                xSql += @"          , R.PROGRESS_RATE
                                    , R.ASSESS_SCORE
                                    , R.REPORT_SCORE
                                    , R.TOTAL_SCORE
                                    , TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') AS COURSE_BEGIN_DT
                                    , TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_END_DT
                                    , R.PASS_FLG
                                    , R.ORDER_FLG
                                    , R.NON_PASS_CD
                                    , R.NON_PASS_REMARK
                              FROM T_OPEN_COURSE O
                                    , (SELECT * FROM T_COURSE_RESULT WHERE APPROVAL_FLG = '000001') R 
                                    , T_USER U
                                    , T_COMPANY C
                             WHERE O.OPEN_COURSE_ID = R.OPEN_COURSE_ID
                                    AND R.USER_ID = U.USER_ID
                                    AND U.COMPANY_ID = C.COMPANY_ID(+)
                                    " + xWhere;
                xSql += @"      ORDER BY DUTY_STEP, USER_NM_KOR, USER_ID ";
                xSql += @"         ) a " + "\r\n";
                xSql += @"  ) a " + "\r\n";

                if (rGubun != "all")
                {
                    if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                    {
                        xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                        xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                    }
                }

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

            return xDs; ;
        }
        
        /************************************************************
       * Function name : SetEduPassUserList
       * Purpose       : 수료처리
       * Input         : DataTable
       * Output        : 
       *************************************************************/
        public string SetEduPassUserList(DataTable rDt, string rGubun)
        {
            string xSql = string.Empty;
            string xRtn = Boolean.FalseString;

            try
            {
                Database db = null;

                try
                {
                    db = base.GetDataBase("LMS");
                    using (OracleConnection cnn = (OracleConnection)db.CreateConnection())
                    {
                        cnn.Open();
                        OracleTransaction trx = cnn.BeginTransaction();
                        OracleCommand cmd = null;
                        try
                        {
                            cmd = base.GetSqlCommand(db);

                            OracleParameter[] xPara = null;
                            foreach (DataRow drDet in rDt.Rows)
                            {
                                //R.USER_ID ||'^'|| R.OPEN_COURSE_ID||'^'|| R.COURSE_RESULT_SEQ
                                string[] keys = Util.Split(drDet["keys"].ToString(), "^", 3);
                                string xUserID = keys[0];
                                string xOpenCourseID = keys[1];
                                string xCourseResultSeq = keys[2];

                                //this.GetEduIssuingUserReportNo(keys, cnn, trx, cmd);

                                if (Convert.ToString(drDet["PRE_ORDER_FLG"]) != "Y")
                                {
                                    xSql = @" 
                                          UPDATE T_COURSE_RESULT 
                                             SET PASS_FLG= :PASS_FLG
                                                 , ORDER_FLG= :ORDER_FLG
                                                 , NON_PASS_CD = :NON_PASS_CD 
                                                 , NON_PASS_REMARK = :NON_PASS_REMARK
                                           WHERE USER_ID= :USER_ID
                                                 AND OPEN_COURSE_ID= :OPEN_COURSE_ID
                                                 AND COURSE_RESULT_SEQ= :COURSE_RESULT_SEQ
                                        ";
                                    xPara = new OracleParameter[7];
                                    xPara[0] = base.AddParam("PASS_FLG", OracleType.VarChar, drDet["PASS_FLG"]);
                                    xPara[1] = base.AddParam("ORDER_FLG", OracleType.VarChar, drDet["ORDER_FLG"]);
                                    xPara[2] = base.AddParam("NON_PASS_CD", OracleType.VarChar, drDet["NON_PASS_CD"]);
                                    xPara[3] = base.AddParam("NON_PASS_REMARK", OracleType.VarChar, drDet["NON_PASS_REMARK"]);
                                    xPara[4] = base.AddParam("USER_ID", OracleType.VarChar, xUserID);
                                    xPara[5] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, xOpenCourseID);
                                    xPara[6] = base.AddParam("COURSE_RESULT_SEQ", OracleType.VarChar, xCourseResultSeq);
                                    cmd.CommandText = xSql;
                                    base.Execute(db, cmd, xPara, trx);
                                }

                                if (Convert.ToString(drDet["PASS_FLG"]) == "000001" && Convert.ToString(drDet["ORDER_FLG"]) == "Y" && Convert.ToString(drDet["PRE_ORDER_FLG"]) != "Y")
                                {
                                    xSql = @" 
                                            SELECT O.COURSE_BEGIN_DT
                                                    , O.COURSE_END_DT
                                                    , C.COURSE_NM
                                                    , C.HRISCODE
                                              FROM T_OPEN_COURSE O
                                                    , T_COURSE C
                                              WHERE ROWNUM=1
                                                    AND O.COURSE_ID = C.COURSE_ID
                                                    AND O.OPEN_COURSE_ID = '" + xOpenCourseID + @"' ";
                                    cmd.CommandText = xSql;
                                    DataSet ds = base.ExecuteDataSet(db, cmd, trx);
                                    DataTable xDtOpenCourse = ds.Tables[0];
                                    if (xDtOpenCourse.Rows.Count > 0)
                                    {
                                        string xSDate = Convert.ToDateTime(xDtOpenCourse.Rows[0]["COURSE_BEGIN_DT"]).ToString("yyyyMMdd");
                                        string xEDate = Convert.ToDateTime(xDtOpenCourse.Rows[0]["COURSE_END_DT"]).ToString("yyyyMMdd");
                                        string xReasonName = Convert.ToString(xDtOpenCourse.Rows[0]["COURSE_NM"]);
                                        string xHrisCode = Convert.ToString(xDtOpenCourse.Rows[0]["HRISCODE"]);

                                        for (int i = 0; i < 2; i++)
                                        {
                                            string xTbl = "ORDER_SYNC";
                                            if (i == 1)
                                                xTbl = "ORDER_SYNC_BAK";

                                            //이수인 경우 발령 처리
                                            xSql = @" 
                                          MERGE INTO " + xTbl + @" S
                                          USING DUAL
                                             ON (
                                                    RTRIM(S.SNO) = :SNO
                                                    AND RTRIM(S.ORD_KIND) = :ORD_KIND 
                                                    AND RTRIM(S.S_DATE) = :S_DATE 
                                                    AND RTRIM(S.E_DATE) = :E_DATE
                                                    AND RTRIM(S.REASON_CODE) = :REASON_CODE
                                                ) 
                                           WHEN MATCHED THEN 
                                            UPDATE SET S.REMARK = :REMARK
                                           WHEN NOT MATCHED THEN 
                                            INSERT(S.SNO, S.ORD_KIND, S.S_DATE, S.E_DATE, S.REASON_CODE, S.REASON_NAME, S.REMARK)
                                            VALUES(:SNO, :ORD_KIND, :S_DATE, :E_DATE, :REASON_CODE, :REASON_NAME, :REMARK)
                                        ";
                                            xPara = new OracleParameter[7];
                                            xPara[0] = base.AddParam("SNO", OracleType.VarChar, xUserID);
                                            xPara[1] = base.AddParam("ORD_KIND", OracleType.VarChar, "AG41");
                                            xPara[2] = base.AddParam("S_DATE", OracleType.VarChar, xSDate);           //교육시작일

                                            xPara[3] = base.AddParam("E_DATE", OracleType.VarChar, xEDate);           //교육종료일

                                            xPara[4] = base.AddParam("REASON_CODE", OracleType.VarChar, xHrisCode);   //hriscode
                                            xPara[5] = base.AddParam("REASON_NAME", OracleType.VarChar, xReasonName); //과정명

                                            xPara[6] = base.AddParam("REMARK", OracleType.VarChar, Convert.ToString(drDet["NON_PASS_REMARK"]));
                                            cmd.CommandText = xSql;
                                            base.Execute(db, cmd, xPara, trx);
                                        }

                                    }


                                }
                            }
                            xRtn = Boolean.TrueString;
                            trx.Commit();
                        }
                        catch (Exception ex)
                        {
                            trx.Rollback();

                            bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                            if (rethrow) throw;
                        }
                        finally
                        {
                            if (cmd != null)
                                cmd.Dispose();
                            if (cnn != null)
                            {
                                if (cnn.State == ConnectionState.Open)
                                {
                                    cnn.Close();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                    if (rethrow) throw;
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
        * Function name : GetOJTResultList
        * Purpose       : OJT
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataSet GetOJTResultList(string[] rParams
                                    , string rGubun, CultureInfo rArgCultureInfo)
        {
            Database db = base.GetDataBase("LMS");
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                string xWhere = "";
                DataTable xDt = null;

                // 교육기간
                xWhere += " AND O.OJT_BEGIN_DT >= to_date('" + rParams[2] + "', 'yyyy.mm.dd') ";
                xWhere += " AND O.OJT_BEGIN_DT <= to_date('" + rParams[3] + "', 'yyyy.mm.dd') ";

                // 분류
                if (rParams[4] != "*" && !String.IsNullOrEmpty(rParams[4]))
                    xWhere += @" AND T.COURSE_KIND = '" + rParams[4] + "' " + "\r\n";

                //OJT명


                if (rParams[5] != "*" && !String.IsNullOrEmpty(rParams[5]))
                {
                    if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                    {
                        xWhere += @" AND  UPPER(T.COURSE_NM) LIKE  '%" + rParams[5].ToUpper() + "%' " + "\r\n";
                    }
                    else
                    {
                        xWhere += @" AND  UPPER(T.COURSE_NM_ABBR) LIKE  '%" + rParams[5].ToUpper() + "%' " + "\r\n";
                    }
                }

                //사번
                if (rParams[6] != "*" && !String.IsNullOrEmpty(rParams[6]))
                    xWhere += @" AND  UPPER(R.USER_ID) LIKE  '" + rParams[6].ToUpper() + "%' " + "\r\n";

                //성명
                if (rParams[7] != "*" && !String.IsNullOrEmpty(rParams[7]))
                {
                    if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                    {
                        xWhere += @" AND U.USER_NM_KOR LIKE  '%" + rParams[7] + "%' " + "\r\n";
                    }
                    else
                    {
                        xWhere += @" AND (U.USER_NM_ENG_FIRST LIKE  '%" + rParams[7] + "%' " + "\r\n";
                        xWhere += @"    OR U.USER_NM_ENG_LAST LIKE  '%" + rParams[7] + "%') " + "\r\n";
                    }
                }



                //부서

                if (rParams[8] != "*" && !String.IsNullOrEmpty(rParams[8]))
                    xWhere += @" AND U.DEPT_CODE = '" + rParams[8] + "' " + "\r\n";

                //직급
                if (rParams[9] != "*" && !String.IsNullOrEmpty(rParams[9]))
                    xWhere += @" AND U.DUTY_STEP = '" + rParams[9] + "' " + "\r\n";


                xSql = @"
                 SELECT COUNT(O.OJT_CD)
                   FROM T_OJT O
                        JOIN T_OJT_RESULT R
                        ON O.OJT_CD = R.OJT_CD
                        JOIN T_OPEN_COURSE C
                        ON O.OPEN_COURSE_ID = C.OPEN_COURSE_ID
                        JOIN T_COURSE T
                        ON C.COURSE_ID = T.COURSE_ID
                        JOIN T_USER U
                        ON U.USER_ID = R.USER_ID
                        LEFT JOIN V_HDUTYSTEP S
                        ON U.DUTY_STEP = S.DUTY_STEP
                  WHERE 1=1 " + "\r\n" + xWhere;
                xDt = base.ExecuteDataTable(db, xSql);
                base.MergeTable(ref xDs, xDt, "table1");

                xSql = "";
                xSql += @"   
                             select *
                               from (
                                select  rownum rnum ";
                xSql += "               , a.* " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += @"     SELECT  O.OJT_CD ||'^'|| U.USER_ID ||'^'|| '' ||'^'|| '' as KEYS
                                        , O.OJT_CD
                                        , O.OPEN_COURSE_ID
                                        , R.USER_ID ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"           , U.USER_NM_KOR ";
                }
                else
                {
                    xSql += @"           , U.USER_NM_ENG_FIRST || ' ' || U.USER_NM_ENG_LAST AS USER_NM_KOR ";
                }
                xSql += @"              , U.DUTY_STEP ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"          , (SELECT step_name FROM V_HDUTYSTEP WHERE duty_step = U.DUTY_STEP) AS step_name ";
                }
                else
                {
                    xSql += @"          , (SELECT step_ename FROM V_HDUTYSTEP WHERE duty_step = U.DUTY_STEP) AS step_name ";
                }
                xSql += @"          , U.DEPT_CODE
                                        , (SELECT dept_ename1 FROM V_HDEPTCODE WHERE dept_code = U.DEPT_CODE) AS dept_ename1
                                        , T.COURSE_KIND ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"         , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD='0042' AND D_CD = T.COURSE_KIND) AS COUSE_KIND_NM
                                        , T.COURSE_NM ";
                }
                else
                {
                    xSql += @"         , (SELECT D_ENM FROM T_CODE_DETAIL WHERE M_CD='0042' AND D_CD = T.COURSE_KIND) AS COUSE_KIND_NM
                                        , T.COURSE_NM_ABBR AS COURSE_NM ";
                }
                xSql += @"             , TO_CHAR(O.OJT_BEGIN_DT, 'YYYY.MM.DD') AS OJT_BEGIN_DT
                                        , TO_CHAR(O.OJT_END_DT, 'YYYY.MM.DD') AS OJT_END_DT
                                        , TO_CHAR(O.OJT_BEGIN_DT, 'YYYY.MM.DD') || '~' || TO_CHAR(O.OJT_END_DT, 'YYYY.MM.DD') AS OJT_DT
                                        , T.COURSE_TIME
                                        , R.ASSESS_SCORE ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"         , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0027' AND d_cd = R.ASSESS_SCORE) AS ASSESS_SCORE_KNM ";
                }
                else
                {
                    xSql += @"         , (SELECT D_ENM FROM T_CODE_DETAIL WHERE M_CD = '0027' AND d_cd = R.ASSESS_SCORE) AS ASSESS_SCORE_KNM ";
                }
                xSql += @"             , R.ASSESS_REMARK
                                        , O.ASSESSER_ID ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"         , (SELECT USER_NM_KOR FROM T_USER WHERE USER_ID = O.ASSESSER_ID) AS ASSESSER_NM ";
                }
                else
                {
                    xSql += @"         , (SELECT U.USER_NM_ENG_FIRST || ' ' || U.USER_NM_ENG_LAST FROM T_USER WHERE USER_ID = O.ASSESSER_ID) AS ASSESSER_NM ";
                }
                xSql += @"             , O.ASSESSER_DUTY_STEP ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"         , (SELECT STEP_NAME FROM V_HDUTYSTEP S WHERE ROWNUM=1 AND S.DUTY_WORK = O.ASSESSER_DUTY_STEP) AS ASSESSER_step_name ";
                }
                else
                {
                    xSql += @"         , (SELECT STEP_ENAME FROM V_HDUTYSTEP S WHERE ROWNUM=1 AND S.DUTY_WORK = O.ASSESSER_DUTY_STEP) AS ASSESSER_step_name ";
                }
                xSql += @"             , O.OJT_LEARNING_TIME
                                  FROM T_OJT O
                                        JOIN T_OJT_RESULT R
                                        ON O.OJT_CD = R.OJT_CD
                                        JOIN T_OPEN_COURSE C
                                        ON O.OPEN_COURSE_ID = C.OPEN_COURSE_ID
                                        JOIN T_COURSE T
                                        ON C.COURSE_ID = T.COURSE_ID
                                        JOIN T_USER U
                                        ON U.USER_ID = R.USER_ID
                                        LEFT JOIN V_HDUTYSTEP S
                                        ON U.DUTY_STEP = S.DUTY_STEP
                                  WHERE 1=1 " + "\r\n"
                                    + xWhere;
                //xSql += "         ORDER BY O.OJT_BEGIN_DT desc, S.STEP_SEQ  " + "\r\n";
                xSql += "           ORDER BY DUTY_STEP, USER_NM_KOR, USER_ID " + "\r\n";
                xSql += @"      ) a ";
                xSql += @"  ) a ";

                if (rGubun != "all")
                {
                    if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                    {
                        xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                        xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                    }
                }

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
            return xDs; ;
        }

        /************************************************************
        * Function name : GetEduIssuingList
        * Purpose       : 교육이수증 발급
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataSet
        *************************************************************/
        public DataSet GetEduIssuingList(string[] rParams
                                            , string rGubun, CultureInfo rArgCultureInfo)
        {
            Database db = base.GetDataBase("LMS");
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                string xWhere = "";
                DataTable xDt = null;

                //교육기간
                xWhere += @" AND O.COURSE_BEGIN_DT >= TO_DATE('" + rParams[2] + "','YYYY.MM.DD') " + "\r\n";
                xWhere += @" AND O.COURSE_BEGIN_DT <= TO_DATE('" + rParams[3] + "','YYYY.MM.DD') " + "\r\n";

                //교육유형
                if (rParams[4] != "*" && !Util.IsNullOrEmptyObject(rParams[4]))
                    xWhere += @" AND C.COURSE_TYPE = '" + rParams[4] + "' " + "\r\n";

                //과정명

                if (!Util.IsNullOrEmptyObject(rParams[5]))
                    xWhere += @" AND UPPER(C.COURSE_NM) LIKE  '%" + rParams[5].ToUpper() + "%' " + "\r\n";

                //성명
                if (!Util.IsNullOrEmptyObject(rParams[6]))
                    xWhere += @" AND U.USER_NM_KOR LIKE  '%" + rParams[6] + "%' " + "\r\n";

                if (!Util.IsNullOrEmptyObject(rParams[6]))
                {
                    xSql = @"
                    SELECT COUNT(*)                              
                      FROM T_COURSE C
                            , T_COURSE_REPORT_ID I
                            , T_COURSE_REPORT_TYPE T
                            , T_OPEN_COURSE O
                            , T_COURSE_RESULT R
                            , T_USER U
                      WHERE C.COURSE_ID = I.COURSE_ID
                            AND I.REPORT_TYPE_ID = T.REPORT_TYPE_ID
                            AND C.COURSE_ID = O.COURSE_ID
                            AND O.OPEN_COURSE_ID = R.OPEN_COURSE_ID
                            AND R.USER_ID = U.USER_ID
                           " + xWhere;
                    xDt = base.ExecuteDataTable(db, xSql);
                    base.MergeTable(ref xDs, xDt, "table1");

                    xSql = "";

                    xSql += @"   
                             select *
                               from (
                                select  rownum rnum ";
                    xSql += "           , A.* " + "\r\n";
                    xSql += "     FROM ( " + "\r\n";
                    xSql += @"  SELECT 
                                   C.COURSE_ID||'^'||O.OPEN_COURSE_ID AS KEYS
                                   , C.COURSE_ID
                                   , O.OPEN_COURSE_ID
                                   , O.COURSE_TYPE ";
                    if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                    {
                        xSql += @" , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD='0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE_NM
                                   , C.COURSE_NM ";
                    }
                    else
                    {
                        xSql += @" , (SELECT D_ENM FROM T_CODE_DETAIL WHERE M_CD='0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE_NM
                                   , C.COURSE_NM_ABBR AS COURSE_NM ";
                    }
                    xSql += @"     , O.COURSE_BEGIN_DT
                                   , O.COURSE_END_DT
                                   , TO_CHAR(O.COURSE_BEGIN_DT,'YYYY.MM.DD') ||'~'|| TO_CHAR(O.COURSE_END_DT,'YYYY.MM.DD') AS COURSE_DATE
                                   , (SELECT COUNT(*) FROM T_COURSE_RESULT WHERE OPEN_COURSE_ID = O.OPEN_COURSE_ID AND PASS_FLG = '000001') AS CNT_PASS --M_CD : 0010 --수료총원
                                   , T.PAGE_HV
                                   , O.COURSE_SEQ
                              FROM T_COURSE C
                                    , T_COURSE_REPORT_ID I
                                    , T_COURSE_REPORT_TYPE T
                                    , T_OPEN_COURSE O
                                    , T_COURSE_RESULT R
                                    , T_USER U
                              WHERE C.COURSE_ID = I.COURSE_ID
                                    AND I.REPORT_TYPE_ID = T.REPORT_TYPE_ID
                                    AND C.COURSE_ID = O.COURSE_ID
                                    AND O.OPEN_COURSE_ID = R.OPEN_COURSE_ID
                                    AND R.USER_ID = U.USER_ID
                                    " + xWhere;
                    xSql += @" ORDER BY O.COURSE_BEGIN_DT desc, C.COURSE_NM " + "\r\n";
                    xSql += @"         ) A " + "\r\n";
                    xSql += @"  ) A " + "\r\n";

                    if (rGubun != "all")
                    {
                        if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                        {
                            xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                            xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                        }
                    }

                    xDt = base.ExecuteDataTable(db, xSql);
                    base.MergeTable(ref xDs, xDt, "table2");
                }
                else
                {
                    xSql = @"
                    SELECT COUNT(*)                              
                      FROM T_COURSE C
                            , T_COURSE_REPORT_ID I
                            , T_COURSE_REPORT_TYPE T
                            , T_OPEN_COURSE O
                     WHERE C.COURSE_ID = I.COURSE_ID
                            AND I.REPORT_TYPE_ID = T.REPORT_TYPE_ID
                            AND C.COURSE_ID = O.COURSE_ID
                           " + xWhere;
                    xDt = base.ExecuteDataTable(db, xSql);
                    base.MergeTable(ref xDs, xDt, "table1");

                    xSql = "";
                    xSql += @"   
                             select *
                               from (
                                select rownum rnum ";
                    xSql += "          , A.* " + "\r\n";
                    xSql += "     FROM ( " + "\r\n";
                    xSql += @"  SELECT 
                                   C.COURSE_ID||'^'||O.OPEN_COURSE_ID AS KEYS
                                   , C.COURSE_ID
                                   , O.OPEN_COURSE_ID
                                   , O.COURSE_TYPE ";
                    if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                    {
                        xSql += @" , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD='0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE_NM
                                   , C.COURSE_NM ";
                    }
                    else
                    {
                        xSql += @" , (SELECT D_ENM FROM T_CODE_DETAIL WHERE M_CD='0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE_NM
                                   , C.COURSE_NM_ABBR AS COURSE_NM ";
                    }
                    xSql += @"     , O.COURSE_BEGIN_DT
                                   , O.COURSE_END_DT
                                   , TO_CHAR(O.COURSE_BEGIN_DT,'YYYY.MM.DD') ||'~'|| TO_CHAR(O.COURSE_END_DT,'YYYY.MM.DD') AS COURSE_DATE
                                   , (SELECT COUNT(*) FROM T_COURSE_RESULT R, T_USER U WHERE R.USER_ID=U.USER_ID AND R.OPEN_COURSE_ID = O.OPEN_COURSE_ID AND R.PASS_FLG = '000001') AS CNT_PASS --M_CD : 0010 --수료총원
                                   , T.PAGE_HV
                                   , O.COURSE_SEQ
                              FROM T_COURSE C
                                    , T_COURSE_REPORT_ID I
                                    , T_COURSE_REPORT_TYPE T
                                    , T_OPEN_COURSE O
                             WHERE C.COURSE_ID = I.COURSE_ID
                                    AND I.REPORT_TYPE_ID = T.REPORT_TYPE_ID
                                    AND C.COURSE_ID = O.COURSE_ID
                                    " + xWhere;
                    xSql += @" ORDER BY O.COURSE_BEGIN_DT desc, C.COURSE_NM " + "\r\n";
                    xSql += @"         ) A " + "\r\n";
                    xSql += @"  ) A " + "\r\n";

                    if (rGubun != "all")
                    {
                        if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                        {
                            xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                            xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                        }
                    }

                    xDt = base.ExecuteDataTable(db, xSql);
                    base.MergeTable(ref xDs, xDt, "table2");
                }

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
            return xDs; ;
        }

        /************************************************************
        * Function name : GetEduIssuingUserList
        * Purpose       : 수강신청/승인리스트 USER 리스트
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataSet
        *************************************************************/
        public DataSet GetEduIssuingUserList(string[] rParams
                                            , string rGubun, CultureInfo rArgCultureInfo)
        {
            Database db = base.GetDataBase("LMS");
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                string xWhere = "";
                DataTable xDt = null;
                xWhere += @" AND O.OPEN_COURSE_ID = '" + rParams[5] + "' " + "\r\n";


                xSql = @"
                        SELECT  COUNT(*)                              
                          FROM  T_OPEN_COURSE O
                                , (select * from T_COURSE_RESULT where PASS_FLG = '000001') R 
                                , T_USER U
                                , T_COMPANY C
                                , V_HDUTYSTEP S
                         WHERE 
                                O.OPEN_COURSE_ID = R.OPEN_COURSE_ID
                                AND R.USER_ID = U.USER_ID
                                AND U.COMPANY_ID = C.COMPANY_ID(+)
                                AND U.DUTY_STEP = S.DUTY_STEP " + "\r\n"
                          + xWhere;
                xDt = base.ExecuteDataTable(db, xSql);
                base.MergeTable(ref xDs, xDt, "table1");

                xSql = "";
                xSql += @"   
                             select *
                               from (
                                select rownum rnum ";
                xSql += "          , a.* " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += @"     SELECT 
                                          R.USER_ID ||'^'|| R.OPEN_COURSE_ID||'^'|| R.COURSE_RESULT_SEQ AS KEYS
                                        , R.OPEN_COURSE_ID
                                        , R.COURSE_RESULT_SEQ
                                        , R.USER_ID ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"          , U.USER_NM_KOR ";
                }
                else
                {
                    xSql += @"          , U.USER_NM_ENG_FIRST || ' ' || U.USER_NM_ENG_LAST AS USER_NM_KOR ";
                }
                xSql += @"             , U.COMPANY_ID ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"          , C.COMPANY_NM ";
                }
                else
                {
                    xSql += @"          , C.COMPANY_NM_ENG AS COMPANY_NM ";
                }
                xSql += @"              , U.DEPT_CODE ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"          , (SELECT DEPT_NAME FROM V_HDEPTCODE WHERE DEPT_CODE = U.DEPT_CODE) AS DEPT_NAME ";
                }
                else
                {
                    xSql += @"          , (SELECT DEPT_ENAME FROM V_HDEPTCODE WHERE DEPT_CODE = U.DEPT_CODE) AS DEPT_NAME ";
                }
                xSql += @"              , U.DUTY_STEP ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"          , (SELECT STEP_NAME FROM V_HDUTYSTEP WHERE DUTY_STEP = U.DUTY_STEP) AS STEP_NAME
                                        , I.COURSE_NM ";
                }
                else
                {
                    xSql += @"          , (SELECT STEP_ENAME FROM V_HDUTYSTEP WHERE DUTY_STEP = U.DUTY_STEP) AS STEP_NAME
                                        , I.COURSE_NM_ABBR AS COURSE_NM ";
                }
                xSql += @"              , R.CERTIFICATE_NAME || R.CERTIFICATE_KEY           AS CERTIFICATE_NO
                                        , R.CERTIFICATE_KEY
                                        , R.CERTIFICATE_NAME
                                        , TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD')          AS COURSE_BEGIN_DT
                                        , TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD')            AS COURSE_END_DT
                                        , R.PASS_FLG
                                        , R.ORDER_FLG
                                        , R.NON_PASS_CD
                                        , R.NON_PASS_REMARK
                                        , REGEXP_REPLACE(HINDEV.CRYPTO_AES256.DEC_AES(U.PERSONAL_NO), '\d', '*', 9) AS PERSONAL_NO
                                        , U.PIC_FILE
                                        , DECODE(U.pic_file_nm, NULL, 'Ⅹ', '○') AS is_pic_file
                                        , DECODE((select count(*) from t_course_report_hist where user_id = r.user_id and open_course_id = r.open_course_id and course_result_seq = r.course_result_seq), 0, 'Y', 'N') as is_new
                                        , DECODE((select count(*) from t_course_report_hist where user_id = r.user_id and open_course_id = r.open_course_id and course_result_seq = r.course_result_seq and seq = 1 and to_char(ins_dt, 'yyyymmdd') = to_char(sysdate, 'yyyymmdd')), 1, 'Y', 'N') as is_renew
                                        , '' as reason
                                        , '' as is_disabled
                                  FROM    T_OPEN_COURSE O
                                        , T_COURSE I
                                        , (select * from T_COURSE_RESULT where PASS_FLG = '000001') R 
                                        , T_USER U
                                        , T_COMPANY C
                                        , V_HDUTYSTEP S
                                 WHERE    O.COURSE_ID = I.COURSE_ID
                                      AND O.OPEN_COURSE_ID = R.OPEN_COURSE_ID
                                      AND R.USER_ID = U.USER_ID
                                      AND U.COMPANY_ID = C.COMPANY_ID(+)
                                      AND U.DUTY_STEP = S.DUTY_STEP(+)
                                        " + xWhere;
                xSql += @"    ORDER BY R.CERTIFICATE_NAME, R.CERTIFICATE_KEY, STEP_SEQ, U.USER_NM_KOR " + "\r\n";
                xSql += @"         ) a " + "\r\n";
                xSql += @"  ) a " + "\r\n";

                if (rGubun != "all")
                {
                    if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                    {
                        xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                        xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                    }
                }

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
        * Function name : GetEduIssuingUserReport
        * Purpose       : 수강신청/승인 리포트 리스트
        * Input         : string[] rParams 
        * Output        : DataSet
        *************************************************************/
        public DataSet GetEduIssuingUserReport(string[] rParams)
        {
            Database db = null;
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS"); //Database 생성

                using (OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection())
                {
                    xCnnLMS.Open();
                    OracleTransaction xTransLMS = xCnnLMS.BeginTransaction(); // 트랜잭션 시작
                    OracleCommand xCmdLMS = null;

                    try
                    {
                        xCmdLMS = base.GetSqlCommand(db);
                        OracleParameter[] xPara = null;

                        string[] xKeys = null;
                        foreach (string xTmp in rParams)
                        {
                            if (xTmp != string.Empty)
                            {
                                //R.USER_ID ||'^'|| R.OPEN_COURSE_ID||'^'|| R.COURSE_RESULT_SEQ 
                                xKeys = xTmp.Split('^');
                                break;
                            }
                        }

                        string xYear = System.DateTime.Now.Year.ToString("00").Substring(2);
                        xSql = @" 
                              SELECT T.REPORT_TYPE_ID
                                     , T.REPORT_YEAR
                                FROM T_COURSE C
                                    , T_OPEN_COURSE O
                                    , T_COURSE_REPORT_ID I
                                    , T_COURSE_REPORT_TYPE T
                             WHERE  C.COURSE_ID = O.COURSE_ID
                                AND C.COURSE_ID = I.COURSE_ID
                                AND I.REPORT_TYPE_ID = T.REPORT_TYPE_ID
                                AND O.OPEN_COURSE_ID = '" + xKeys[1] + @"'
                                AND T.REPORT_YEAR = '" + xYear + @"'
                            ";
                        xCmdLMS.CommandText = xSql;
                        DataSet xDsType = base.ExecuteDataSet(db, xCmdLMS, xTransLMS);
                        xCmdLMS.Parameters.Clear();

                        DataTable xDtType = xDsType.Tables[0];
                        if (xDtType.Rows.Count > 0)
                        {
                            xSql = @" 
                                    UPDATE T_COURSE_REPORT_TYPE
                                       SET REPORT_CNT = (SELECT NVL(MAX(REPORT_CNT), 0) +1 FROM T_COURSE_REPORT_TYPE WHERE REPORT_YEAR = :REPORT_YEAR AND REPORT_TYPE_ID = :REPORT_TYPE_ID)
                                     WHERE REPORT_YEAR = :REPORT_YEAR
                                            AND REPORT_TYPE_ID = :REPORT_TYPE_ID
                                    ";
                            xPara = new OracleParameter[2];
                            xPara[0] = base.AddParam("REPORT_YEAR", OracleType.VarChar, xDtType.Rows[0]["REPORT_YEAR"]);
                            xPara[1] = base.AddParam("REPORT_TYPE_ID", OracleType.VarChar, xDtType.Rows[0]["REPORT_TYPE_ID"]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);
                            xCmdLMS.Parameters.Clear();
                        }
                        else
                        {
                            xSql = @" 
                                    UPDATE T_COURSE_REPORT_TYPE
                                       SET REPORT_YEAR = :REPORT_YEAR
                                            , REPORT_CNT = 1
                                     WHERE REPORT_TYPE_ID 
                                            =
                                            (
                                              SELECT T.REPORT_TYPE_ID
                                                FROM T_COURSE C
                                                    , T_OPEN_COURSE O
                                                    , T_COURSE_REPORT_ID I
                                                    , T_COURSE_REPORT_TYPE T
                                             WHERE  ROWNUM=1
                                                AND C.COURSE_ID = O.COURSE_ID
                                                AND C.COURSE_ID = I.COURSE_ID
                                                AND I.REPORT_TYPE_ID = T.REPORT_TYPE_ID
                                                AND O.OPEN_COURSE_ID = :OPEN_COURSE_ID
                                            )
                                    ";
                            xPara = new OracleParameter[2];
                            xPara[0] = base.AddParam("REPORT_YEAR", OracleType.VarChar, xYear);
                            xPara[1] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, xKeys[1]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);
                            xCmdLMS.Parameters.Clear();
                        }

                        xTransLMS.Commit(); //트렌잭션 커밋
                    }
                    catch (Exception ex)
                    {
                        // 트랜잭션 롤백
                        xTransLMS.Rollback();

                        bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                        if (rethrow) throw;
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
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            DataSet xDs = new DataSet();
            try
            {
                string xStrWhere = "(";
                foreach (string xTmp in rParams)
                {
                    if (xTmp != string.Empty)
                    {
                        //R.USER_ID ||'^'|| R.OPEN_COURSE_ID||'^'|| R.COURSE_RESULT_SEQ 
                        string[] xKeys = xTmp.Split('^');
                        xStrWhere += " '" + xKeys[0] + xKeys[1] + xKeys[2] + "', ";
                    }
                }
                xStrWhere = xStrWhere.Substring(0, xStrWhere.LastIndexOf(','));
                xStrWhere += ")";

                xSql = @" select u.user_nm_kor
                                , u.user_nm_eng_first
                                , u.user_nm_eng_last
                                , (select step_ename from v_hdutystep where duty_step = u.duty_step) as step_name
                                , HINDEV.CRYPTO_AES256.DEC_AES(u.personal_no) as personal_no
                                , to_char(o.course_begin_dt, 'YYYY.MM.DD') as course_begin_dt
                                , to_char(o.course_end_dt, 'yyyy.mm.dd') as course_end_dt
                                , u.pic_file_nm
                                , u.pic_file
                                , '' AS LECTURER_NM
                                , T.REPORT_YEAR || T.REPORT_CNT AS REPORT_CNT
                                , U.COUNTRY_KIND
                                , (
                                    select DNAME2
                                      from LMS.V_DATACHKD 
                                     where hcode = 'C002'
                                            AND DCODE = U.COUNTRY_KIND
                                            AND ROWNUM = 1
                                    ) as COUNTRY_KIND_NM
                                , to_char(u.birth_dt, 'yyyy.mm.dd') as birth_dt
                                , replace(to_char(u.birth_dt, 'Monthdd, yyyy', 'NLS_DATE_LANGUAGE=AMERICAN'), ' 0', ' ') as birth_dt_eng
                                , replace(to_char(o.course_begin_dt, 'Mon. dd, yyyy', 'NLS_DATE_LANGUAGE=AMERICAN'), '. 0', '. ') as course_begin_dt_en
                                , replace(to_char(o.course_end_dt, 'Mon. dd, yyyy', 'NLS_DATE_LANGUAGE=AMERICAN'), '. 0', '. ') as course_end_dt_en
                                , replace(to_char(o.course_begin_dt, 'Monthdd, yyyy', 'NLS_DATE_LANGUAGE=AMERICAN'), ' 0', ' ') as course_begin_dt_eng
                                , replace(to_char(o.course_end_dt, 'Monthdd, yyyy', 'NLS_DATE_LANGUAGE=AMERICAN'), ' 0', ' ') as course_end_dt_eng
                                , T.REPORT_TYPE_ID
                                , O.COURSE_GUBUN   -- 국토해양부 과정 여부 
                                , R.CERTIFICATE_NAME || R.CERTIFICATE_KEY as CERTIFICATE_CODE -- 증서번호

                                /* 리더십 및 팀워크교육(20020005), 리더십 및 관리기술 직무교육(20020006)을 항해/기관으로 분류
                                   : 회원정보의 현재 직급으로 분류
                                    - 분류가 안되면 항해를 우선 출력 (근무가 해상에서 육상으로 변경되어 현 직급이 구분이 안될 경우)
                                */
                                , (CASE WHEN U.duty_step IN ('SE10','SE08','SE18','SE20','SE30','SE28','SE40','SE38') THEN 'E' ELSE 'M' END)
                                   as me_gubun  /* M 항해(또는 그외) E 기관 */
                                /* 선박위험물관리교육(20020010)을 내부/수탁으로 분류
                                   : 과정 개설시, 사내/외구분 코드로 분류함
                                */
                                , O.course_inout    /* 000001 사내 000002 사외 */

                                , '' as report_desc_kor
                                , '' as report_desc_eng

                            FROM T_COURSE C 
                                , T_COURSE_REPORT_ID I
                                , T_COURSE_REPORT_TYPE T
                                , T_OPEN_COURSE O
                                , T_COURSE_RESULT R 
                                , T_USER U
                          WHERE C.COURSE_ID = I.COURSE_ID
                                AND I.REPORT_TYPE_ID = T.REPORT_TYPE_ID
                                AND C.COURSE_ID = O.COURSE_ID
                                AND O.OPEN_COURSE_ID = R.OPEN_COURSE_ID
                                AND R.USER_ID = U.USER_ID
                        ";
                xSql += " AND R.USER_ID || R.OPEN_COURSE_ID || R.COURSE_RESULT_SEQ IN "
                        + xStrWhere;
                base.MergeTable(ref xDs, base.ExecuteDataTable("LMS", xSql), "table0");

                //강사고정으로 수정
                //                xStrWhere = "";
                //                foreach (string xTmp in rParams)
                //                {
                //                    if (xTmp != string.Empty)
                //                    {
                //                        //R.USER_ID ||'^'|| R.OPEN_COURSE_ID||'^'|| R.COURSE_RESULT_SEQ 
                //                        string[] xKeys = xTmp.Split('^');
                //                        xStrWhere += " '" + xKeys[1] + "' ";
                //                        break;
                //                    }
                //                }
                //                xStrWhere += "";
                //                xSql = @" 
                //                         SELECT  LECTURER_NM
                //                           FROM  T_COURSE C
                //                                 JOIN T_COURSE_SUBJECT S
                //                                 ON C.COURSE_ID = S.COURSE_ID
                //                                 JOIN T_SUBJECT B
                //                                 ON S.SUBJECT_ID = B.SUBJECT_ID
                //                                 JOIN T_OPEN_COURSE O
                //                                 ON C.COURSE_ID = O.COURSE_ID
                //                        ";
                //                xSql += " AND O.OPEN_COURSE_ID = " + xStrWhere;
                //                xSql += " group by LECTURER_NM ";
                //                base.MergeTable(ref xDs, base.ExecuteDataTable("LMS", xSql), "table1");
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
        /*
            증서 고유번호 체크해서 Null일 경우 생성
         */
        public void SetCERTIFICATE_KEY(string rParam)
        {
            string sSql = string.Empty;
            string xSql = string.Empty;
            string xStrWhere = string.Empty;
            string sCourseId = string.Empty;
            string sCertificate_Name = string.Empty;

            Database db = null;
            DataTable dt = null;
            
            db = base.GetDataBase("LMS"); //Database 생성

            using (OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection())
            {
                xCnnLMS.Open();
                OracleTransaction xTransLMS = xCnnLMS.BeginTransaction(); // 트랜잭션 시작
                OracleCommand xCmdLMS = null;
                try
                {
                    xCmdLMS = base.GetSqlCommand(db);

                    OracleParameter[] xPara = null;

                    xStrWhere = "(";

                    //R.USER_ID ||'^'|| R.OPEN_COURSE_ID||'^'|| R.COURSE_RESULT_SEQ 
                    string[] xKeys = rParam.Split('^');
                    xStrWhere += " '" + xKeys[0] + xKeys[1] + xKeys[2] + "')";

                    sSql = @"
                    SELECT O.COURSE_ID
                      FROM T_OPEN_COURSE O
                     WHERE O.OPEN_COURSE_ID = '" + xKeys[1] + "'";

                    dt = base.ExecuteDataTable("LMS", sSql);

                    sCourseId = dt.Rows[0]["COURSE_ID"].ToString();

                    dt.Dispose();

                    /*
                     *  5개 교육만 증서번호 발급(2013.07.12)-문서영대리 요청. 나머지는 증서번호 발급 안함.
                     * 
                        BRTM교육 - BRTM (07080001)
                        ERM교육 - ERM (01010042)
                        S.H.S고급과정 - SHS (07110005)
                        SHS초급과정 - SHS (08050002)
                        ECDIS과정 - ECDIS (08070001)
                        경력사관입사교육(SHEQ) - FML (09030007)
                     */
                    /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------
                     * 2020.02 변경
                        구분                    설명                                    NO      증서명/과정명                           증서번호
                                                                                                                                        기관-과정-년도(2)-발급번호(3)     교육과정코드
                    ------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        해수부지정              해양수산부 지정과정 및 MAKER 증서       1       선박조종시뮬레이터 교육                 GMS-SHS-20001                     20020001
                                                                                        2       선박모의조종 및 선교 팀워크 교육        GMS-SBT-20001                     20020002
                                                                                        3       엔진룸 시뮬레이터 교육                  GMS-ERS-20001                     20020003
                                                                                        4       전자해도장치 교육                       GMS-ECDIS-20001                   20020004
                                                                                        5       리더십 및 팀워크교육                    GMS-LAT-20001                     20020005
                                                                                        6       리더십 및 관리기술 직무교육             GMS-LTM-20001                     20020006
                                                                                        7       JRC ECDIS TST(901)                      GMS-TST-20001                     20020007
                                                                                        8       JRC ECDIS TST(9201)                     GMS-TST-20001                     20020008
                        선박요청                선내교육에 따른 발급 증서               1       선박평형수관리협약교육                  GMS-BWMS-20001                    20020009
                                                                                        2       선박위험물관리교육                      GMS-IMDG-20001                    20020010
                        X(수료현황출력)공통수료증              기타 일반과정 수료증                    1       전과정                                  GMS-TC-20001       나머지과정모두
                    -----------------------------------------------------------------------------------------------------------------------------------------------------------------*/
                    switch (sCourseId)
                    {
                        case "20020001":
                            sCertificate_Name = "GMS-SHS-";
                            break;
                        case "20020002":
                            sCertificate_Name = "GMS-SBT-";
                            break;
                        case "20020003":
                            sCertificate_Name = "GMS-ERS-";
                            break;
                        case "20020004":
                            sCertificate_Name = "GMS-ECDIS-";
                            break;
                        case "20020005":
                            sCertificate_Name = "GMS-LAT-";
                            break;
                        case "20020006":
                            sCertificate_Name = "GMS-LTM-";
                            break;
                        case "20020007":
                        case "20020008":
                            sCertificate_Name = "GMS-TST-";
                            break;
                        case "20020009":
                            sCertificate_Name = "GMS-BWMS-";
                            break;
                        case "20020010":
                            sCertificate_Name = "GMS-IMDG-";
                            break;
                        default:
                            //sCertificate_Name = "GMS-TC-";
                            //break;
                            return;
                    }
                    
                    sSql = @"
                    SELECT CERTIFICATE_KEY 
                      FROM T_COURSE_RESULT R 
                     WHERE R.USER_ID || R.OPEN_COURSE_ID || R.COURSE_RESULT_SEQ = 
                    ";

                    dt = base.ExecuteDataTable("LMS", sSql + xStrWhere);

                    if (dt.Rows[0]["CERTIFICATE_KEY"] == DBNull.Value)
                    {
                        xSql = @" 
                        UPDATE T_COURSE_RESULT A
                        SET CERTIFICATE_KEY = :P_YEAR1 ||
                                             (SELECT LPAD((NVL(MAX(substr(CERTIFICATE_KEY,3,5)), 0) + 1), 3, '0') AS CERTIFICATE_KEY
                                                FROM T_COURSE_RESULT R, T_OPEN_COURSE O
                                               WHERE R.OPEN_COURSE_ID = O.OPEN_COURSE_ID
                                                 -- AND O.COURSE_ID = :P_COURSE_ID
                                                 AND DECODE(O.COURSE_ID,'20020007','99999999','20020008','99999999',O.COURSE_ID) = DECODE(:P_COURSE_ID,'20020007','99999999','20020008','99999999',:P_COURSE_ID) -- JRC ECDIS TST(901)과정과 JRC ECDIS TST(9201)과정은 묶어서 생성하기 위해
                                                 AND SUBSTR(R.CERTIFICATE_KEY, 1, 2) = :P_YEAR2),
                            CERTIFICATE_NAME = :P_CERTIFICATE_NAME                           
                        WHERE A.USER_ID || A.OPEN_COURSE_ID || A.COURSE_RESULT_SEQ = ";

                        xSql = xSql + xStrWhere;

                        xPara = new OracleParameter[4];
                        xPara[0] = base.AddParam("P_YEAR1", OracleType.VarChar, xKeys[1].Substring(0, 2));
                        xPara[1] = base.AddParam("P_COURSE_ID", OracleType.VarChar, sCourseId);
                        xPara[2] = base.AddParam("P_YEAR2", OracleType.VarChar, xKeys[1].Substring(0, 2));
                        xPara[3] = base.AddParam("P_CERTIFICATE_NAME", OracleType.VarChar, sCertificate_Name);


                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xPara, xTransLMS);
                        
                        xTransLMS.Commit(); //트렌잭션 커밋
                    }
                    
                }
                catch (Exception ex)
                {
                    // 트랜잭션 롤백
                    xTransLMS.Rollback();

                    bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                    if (rethrow) throw;
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

        /************************************************************
       * Function name : GetEduIssuingUserReport
       * Purpose       : 수강신청/승인 리포트 리스트
       * Input         : string[] rParams 
       * Output        : DataSet
       *************************************************************/
                    //        public string GetEduIssuingUserReportNo(string[] xKeys, OracleConnection xCnnLMS, OracleTransaction xTransLMS, OracleCommand xCmdLMS)
                    //        {
                    //            Database db = null;
                    //            string xSql = string.Empty;
                    //            string xCertNo = string.Empty;

                    //            try
                    //            {
                    //                db = base.GetDataBase("LMS"); //Database 생성

                    //               // using (OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection())
                    //                //{
                    //                    //xCnnLMS.Open();
                    //                    //OracleTransaction xTransLMS = xCnnLMS.BeginTransaction(); // 트랜잭션 시작
                    //                   // OracleCommand xCmdLMS = null;

                    //                    try
                    //                    {

                    //                        string xYear = System.DateTime.Now.Year.ToString("00").Substring(2);
                    //                        xSql = @" 
                    //                              SELECT T.REPORT_TYPE_ID
                    //                                     , T.REPORT_YEAR
                    //                                FROM T_COURSE C
                    //                                    , T_OPEN_COURSE O
                    //                                    , T_COURSE_REPORT_ID I
                    //                                    , T_COURSE_REPORT_TYPE T
                    //                             WHERE  C.COURSE_ID = O.COURSE_ID
                    //                                AND C.COURSE_ID = I.COURSE_ID
                    //                                AND I.REPORT_TYPE_ID = T.REPORT_TYPE_ID
                    //                                AND O.OPEN_COURSE_ID = '" + xKeys[1] + @"'
                    //                                AND T.REPORT_YEAR = '" + xYear + @"'
                    //                            ";
                    //                        xCmdLMS.CommandText = xSql;
                    //                        DataSet xDsType = base.ExecuteDataSet(db, xCmdLMS, xTransLMS);
                    //                        xCmdLMS.Parameters.Clear();

                    //                        DataTable xDtType = xDsType.Tables[0];
                    //                        if (xDtType.Rows.Count > 0)
                    //                        {
                    //                            xSql = @" 
                    //                                    UPDATE T_COURSE_REPORT_TYPE
                    //                                       SET REPORT_CNT = (SELECT NVL(MAX(REPORT_CNT), 0) +1 FROM T_COURSE_REPORT_TYPE WHERE REPORT_YEAR = :REPORT_YEAR AND REPORT_TYPE_ID = :REPORT_TYPE_ID)
                    //                                     WHERE REPORT_YEAR = :REPORT_YEAR
                    //                                            AND REPORT_TYPE_ID = :REPORT_TYPE_ID
                    //                                    ";
                    //                            OracleParameter[] xPara = new OracleParameter[2];
                    //                            xPara[0] = base.AddParam("REPORT_YEAR", OracleType.VarChar, xDtType.Rows[0]["REPORT_YEAR"]);
                    //                            xPara[1] = base.AddParam("REPORT_TYPE_ID", OracleType.VarChar, xDtType.Rows[0]["REPORT_TYPE_ID"]);

                    //                            xCmdLMS.CommandText = xSql;
                    //                            base.Execute(db, xCmdLMS, xPara, xTransLMS);
                    //                            xCmdLMS.Parameters.Clear();
                    //                        }
                    //                        else
                    //                        {
                    //                            xSql = @" 
                    //                                    UPDATE T_COURSE_REPORT_TYPE
                    //                                       SET REPORT_YEAR = :REPORT_YEAR
                    //                                            , REPORT_CNT = 1
                    //                                     WHERE REPORT_TYPE_ID 
                    //                                            =
                    //                                            (
                    //                                              SELECT T.REPORT_TYPE_ID
                    //                                                FROM T_COURSE C
                    //                                                    , T_OPEN_COURSE O
                    //                                                    , T_COURSE_REPORT_ID I
                    //                                                    , T_COURSE_REPORT_TYPE T
                    //                                             WHERE  ROWNUM=1
                    //                                                AND C.COURSE_ID = O.COURSE_ID
                    //                                                AND C.COURSE_ID = I.COURSE_ID
                    //                                                AND I.REPORT_TYPE_ID = T.REPORT_TYPE_ID
                    //                                                AND O.OPEN_COURSE_ID = :OPEN_COURSE_ID
                    //                                            )
                    //                                    ";
                    //                            OracleParameter[] xPara = new OracleParameter[2];
                    //                            xPara[0] = base.AddParam("REPORT_YEAR", OracleType.VarChar, xYear);
                    //                            xPara[1] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, xKeys[1]);

                    //                            xCmdLMS.CommandText = xSql;
                    //                            base.Execute(db, xCmdLMS, xPara, xTransLMS);
                    //                            xCmdLMS.Parameters.Clear();
                    //                        }

                    //                        xTransLMS.Commit(); //트렌잭션 커밋

                    //                     }
                    //                    catch (Exception ex)
                    //                    {
                    //                        // 트랜잭션 롤백
                    //                        xTransLMS.Rollback();

                    //                        bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                    //                        if (rethrow) throw;
                    //                    }
                    //                    finally
                    //                    {
                    //                        if (xCmdLMS != null) xCmdLMS.Dispose();
                    //                        if (xTransLMS != null) xTransLMS.Dispose();
                    //                        if (xCnnLMS != null)
                    //                        { if (xCnnLMS.State == ConnectionState.Open) xCnnLMS.Close(); }
                    //                    }

                    //                }
                    //            //}
                    //                catch (Exception ex)
                    //                {
                    //                    bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                    //                    if (rethrow) throw;
                    //                }

                    //                finally
                    //                {
                    //                    db = null;
                    //                }
                    //               // return xDs;

                    //        }

                    /************************************************************
                    * Function name : GetEduTrainigRecordList
                    * Purpose       : 교육이력입력 리스트
                    * Input         : string[] rParams (0: pagesize, 1: pageno)
                    * Output        : DataSet
                    *************************************************************/
                    public DataSet GetEduTrainigRecordList(string[] rParams
                                            , string rGubun, CultureInfo rArgCultureInfo)
        {
            Database db = null;
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");

                string xWhere = "";
                DataTable xDt = null;

                //이름
                if (!Util.IsNullOrEmptyObject(rParams[2]))
                {
                    xWhere += @" AND (U.USER_NM_KOR LIKE  '%" + rParams[2] + "%' " + "\r\n";
                    xWhere += @" OR U.USER_NM_ENG_FIRST || ' ' || U.USER_NM_ENG_LAST LIKE  '%" + rParams[2] + "%')" + "\r\n";
                }

                //사번
                //if (rParams[6] == string.Empty || rParams[6] == "N")
                if (!Util.IsNullOrEmptyObject(rParams[3]))
                    xWhere += @" AND T.USER_ID = '" + rParams[3] + "' " + "\r\n";

                //직급
                if (rParams[4] != "*" && !Util.IsNullOrEmptyObject(rParams[4]))
                    xWhere += @" AND U.DUTY_STEP = '" + rParams[4] + "' " + "\r\n";

                //주민번호
                if (!Util.IsNullOrEmptyObject(rParams[5]))
                    xWhere += @" AND U.PERSONAL_NO LIKE  '%" + rParams[5] + "%' " + "\r\n";


                xSql = @"
                    SELECT COUNT(*)                              
                      FROM T_REG_TRAINING_RECORD T
                           JOIN T_USER U
                           ON T.USER_ID = U.USER_ID  
                           JOIN T_COURSE C
                           ON T.COURSE_ID = C.COURSE_ID
                     WHERE 1=1 " + "\r\n"
                          + xWhere;
                xDt = base.ExecuteDataTable(db, xSql);
                base.MergeTable(ref xDs, xDt, "table1");

                xSql = "";
                xSql += @"   
                             select *
                               from (
                                select rownum rnum ";
                xSql += "               , a.* " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += @"     SELECT T.USER_ID || '^' || T.RECORD_ID || '^' || T.COURSE_ID AS KEYS
                                    , T.USER_ID ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", U.USER_NM_KOR ";
                    xSql += ", C.COURSE_NM ";
                }
                else
                {
                    xSql += ", u.user_nm_eng_first || ' ' || u.user_nm_eng_last as USER_NM_KOR ";
                    xSql += ", C.COURSE_NM_ABBR AS COURSE_NM ";
                }
                //, U.USER_NM_KOR
                //, C.COURSE_NM
                xSql += @"      , T.RECORD_ID 
                                    , (SELECT ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += " D_KNM ";
                }
                else
                {
                    xSql += " D_ENM ";
                }
                xSql += @"        FROM T_CODE_DETAIL 
                                      WHERE M_CD = '0007' AND D_CD = T.LEARNING_INSTITUTION) 
                                      AS LEARNING_INSTITUTION
                                    , TO_CHAR(T.COURSE_BEGIN_DT, 'YYYY.MM.DD') AS COURSE_BEGIN_DT
                                    , TO_CHAR(T.COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_END_DT
                                    , T.COURSE_ID
                              FROM T_REG_TRAINING_RECORD T
                                   JOIN T_USER U
                                   ON T.USER_ID = U.USER_ID 
                                   JOIN T_COURSE C
                                   ON T.COURSE_ID = C.COURSE_ID
                             WHERE 1=1
                                    " + xWhere;
                xSql += @"   ORDER BY T.COURSE_BEGIN_DT DESC " + "\r\n";
                xSql += @"         ) a " + "\r\n";
                xSql += @"  ) a " + "\r\n";

                if (rGubun != "all")
                {
                    if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                    {
                        xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                        xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                    }
                }

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
            return xDs; ;
        }

        /************************************************************
        * Function name : SetEduTrainingRecord
        * Purpose       : 평가 항목 등록
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetEduTrainingRecord(object[] rParams)
        {
            Database db = null;
            string xRtn = Boolean.FalseString;
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
                        if (String.IsNullOrEmpty(Convert.ToString(rParams[0])))
                        {
                            xSql = @" 
                                    INSERT INTO T_REG_TRAINING_RECORD
                                    (
                                          USER_ID,
                                          RECORD_ID,  
                                          COURSE_ID,
                                          LEARNING_INSTITUTION,
                                          COURSE_BEGIN_DT,
                                          COURSE_END_DT,
                                          INS_DT
                                    )
                                    VALUES
                                    (
                                          :USER_ID,
                                          (SELECT NVL(MAX(RECORD_ID),0)+1 FROM T_REG_TRAINING_RECORD WHERE USER_ID=:USER_ID),
                                          :COURSE_ID ,
                                          :LEARNING_INSTITUTION ,
                                          :COURSE_BEGIN_DT ,
                                          :COURSE_END_DT ,
                                          SYSDATE
                                    )
                                    ";

                            xPara = new OracleParameter[5];
                            xPara[0] = base.AddParam("USER_ID", OracleType.VarChar, rParams[1]);
                            xPara[1] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[2]);
                            xPara[2] = base.AddParam("LEARNING_INSTITUTION", OracleType.VarChar, rParams[3]);
                            xPara[3] = base.AddParam("COURSE_BEGIN_DT", OracleType.DateTime, rParams[4]);
                            xPara[4] = base.AddParam("COURSE_END_DT", OracleType.DateTime, rParams[5]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);
                        }
                        else
                        {
                            xSql = @" 
                                    UPDATE T_REG_TRAINING_RECORD
                                       SET LEARNING_INSTITUTION = :LEARNING_INSTITUTION,
                                           COURSE_BEGIN_DT = :COURSE_BEGIN_DT,
                                           COURSE_END_DT= :COURSE_END_DT
                                     WHERE USER_ID = :USER_ID
                                            AND RECORD_ID = :RECORD_ID
                                            AND COURSE_ID = :COURSE_ID
                                    ";
                            xPara = new OracleParameter[6];

                            xPara[0] = base.AddParam("LEARNING_INSTITUTION", OracleType.VarChar, rParams[3]);
                            xPara[1] = base.AddParam("COURSE_BEGIN_DT", OracleType.DateTime, rParams[4]);
                            xPara[2] = base.AddParam("COURSE_END_DT", OracleType.DateTime, rParams[5]);
                            xPara[3] = base.AddParam("USER_ID", OracleType.VarChar, rParams[1]);
                            xPara[4] = base.AddParam("RECORD_ID", OracleType.VarChar, rParams[0]);
                            xPara[5] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[2]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);
                        }

                        xTransLMS.Commit(); //트렌잭션 커밋
                        xRtn = Boolean.TrueString;
                    }
                    catch (Exception ex)
                    {
                        // 트랜잭션 롤백
                        xTransLMS.Rollback();

                        bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                        if (rethrow) throw;
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
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            finally
            {
                db = null;
            }
            return xRtn;
        }

        /************************************************************
        * Function name : GetEduTrainigRecord
        * Purpose       : 교육이력입력내용
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetEduTrainigRecord(string[] rParams, CultureInfo rArgCultureInfo)
        {
            string xSql = string.Empty;
            DataTable xDt = null;

            try
            {
                xSql = "";
                xSql += @"   SELECT T.USER_ID
                                    , T.RECORD_ID
                                    , (SELECT MAX(EDUCATIONAL_ORG) FROM T_OPEN_COURSE B WHERE C.COURSE_ID = B.COURSE_ID)  AS LEARNING_INSTITUTION ";

                //                                    , (SELECT ";
                //                                    if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                //                                    {
                //                                        xSql += " D_KNM ";
                //                                    }
                //                                    else
                //                                    {
                //                                        xSql += " D_ENM ";
                //                                    }
                //                xSql += @"       FROM T_CODE_DETAIL 
                //                                     WHERE M_CD = '0007' AND D_CD = T.LEARNING_INSTITUTION) 
                //                                     AS LEARNING_INSTITUTION ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", u.user_nm_kor ";
                    xSql += ", C.COURSE_NM ";
                }
                else
                {
                    xSql += ", u.user_nm_eng_first || ' ' || u.user_nm_eng_last as user_nm_kor ";
                    xSql += ", C.COURSE_NM_ABBR AS COURSE_NM ";
                }
                // , U.USER_NM_KO, C.COURSE_NM, T.LEARNING_INSTITUTION
                xSql += @"     , C.COURSE_ID
                                    , TO_CHAR(T.COURSE_BEGIN_DT, 'YYYY.MM.DD') AS COURSE_BEGIN_DT
                                    , TO_CHAR(T.COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_END_DT
                              FROM T_REG_TRAINING_RECORD T
                                   JOIN T_USER U
                                   ON T.USER_ID = U.USER_ID 
                                   JOIN T_COURSE C
                                   ON T.COURSE_ID = C.COURSE_ID
                             WHERE T.USER_ID = '" + rParams[0] + @"'
                                    AND T.RECORD_ID = '" + rParams[1] + @"'
                                    AND T.COURSE_ID = '" + rParams[2] + @"'
                                    ";
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
        * Function name : GetEduTrainigRecordDelete
        * Purpose       : 평가 항목 삭제
        * Input         : DataTable
        * Output        : DataTable
        *************************************************************/
        public string GetEduTrainigRecordDelete(DataTable rDt)
        {
            string xSql = string.Empty;
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
                    foreach (DataRow drDet in rDt.Rows)
                    {
                        xSql = @" 
                                DELETE
                                  FROM T_REG_TRAINING_RECORD 
                                 WHERE USER_ID = '" + drDet["user_id"] + @"'
                                        AND RECORD_ID = '" + drDet["record_id"] + @"' 
                                        AND COURSE_ID = '" + drDet["course_id"] + "' ";

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTrnsLMS);
                    }

                    xRtn = Boolean.TrueString;
                    xTrnsLMS.Commit(); // 트랜잭션 커밋
                }
                catch (Exception ex)
                {
                    xTrnsLMS.Rollback(); // Exception 발생시 롤백처리...

                    bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                    if (rethrow) throw;
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

        #region public void SetFileAtt(string rKind, byte[] rFile, string rFileName, string rSeq, string rAttKey)
        /************************************************************
        *  Function name : SetFileAtt
        *  Purpose       : 파일 저장
        *  Input         : string rKind, byte[] rFile, string rFileName, string rSeq, string rAttKey
        *  Output        : 
        **************************************************************/
        public void SetFileAtt(byte[] rFile
                                , string rFileName
                                , string rAttKey)
        {
            try
            {
                string xSql = string.Empty;
                string xDelSql = string.Empty;

                OracleParameter[] oraParams = null;
                xSql = @"
                   UPDATE T_USER
                      SET PIC_FILE_NM = :PIC_FILE_NM
                            , PIC_FILE = :PIC_FILE
                    WHERE UPPER(USER_ID) = :USER_ID
                ";
                oraParams = new OracleParameter[3];
                oraParams[0] = base.AddParam("PIC_FILE_NM", OracleType.VarChar, rFileName);
                oraParams[1] = base.AddParam("PIC_FILE", OracleType.Blob, rFile.Length, rFile);
                oraParams[2] = base.AddParam("USER_ID", OracleType.VarChar, Util.Split(rAttKey, "^")[0]);

                base.ExecuteScalar("LMS", xSql, oraParams);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        #endregion

        #region DataTable GetEduIssuingUserHistory(DataTable rDt, CultureInfo rArgCultureInfo)
        /************************************************************
        * Function name : GetEduIssuingHistory
        * Purpose       : 증서발급이력 조회
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetEduIssuingHistory(string[] rParams , string rGubun, CultureInfo rArgCultureInfo)
        {
            Database db = base.GetDataBase("LMS");
            string xSql = string.Empty;
            DataTable xDt = null;

            try
            {
                string xWhere = "";

                //발급일자
                xWhere += @" AND H.INS_DT >= TO_DATE('" + rParams[2] + "','YYYY.MM.DD') " + "\r\n";
                xWhere += @" AND H.INS_DT <= TO_DATE('" + rParams[3] + "','YYYY.MM.DD') " + "\r\n";

                //교육유형
                if (rParams[4] != "*" && !Util.IsNullOrEmptyObject(rParams[4]))
                    xWhere += @" AND C.COURSE_TYPE = '" + rParams[4] + "' " + "\r\n";

                //과정명
                if (!Util.IsNullOrEmptyObject(rParams[5]))
                    xWhere += @" AND UPPER(C.COURSE_NM) LIKE  '%" + rParams[5].ToUpper() + "%' " + "\r\n";

                //성명
                if (!Util.IsNullOrEmptyObject(rParams[6]))
                {
                    xWhere += @" AND U.USER_NM_KOR || ' ' || U.USER_NM_ENG_FIRST || ' ' || U.USER_NM_ENG_LAST LIKE '%" + rParams[6] + "%' " + "\r\n";
                }
                
                xSql += @"   
                            select *
                            from (
                            select  rownum rnum ";
                xSql += "         , A.* " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += @"  SELECT 
                                  H.USER_ID||'^'||H.OPEN_COURSE_ID||'^'||H.COURSE_RESULT_SEQ||'^'||H.SEQ AS KEYS
                                , C.COURSE_ID
                                , H.OPEN_COURSE_ID
                                , H.COURSE_RESULT_SEQ
                                , H.SEQ
                                , O.COURSE_TYPE
                                , NVL(P.COMPANY_NM, ' ') AS COMPANY_NM ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"  , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD='0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE_NM
                                , C.COURSE_NM ";
                }
                else
                {
                    xSql += @"  , (SELECT D_ENM FROM T_CODE_DETAIL WHERE M_CD='0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE_NM
                                , C.COURSE_NM_ABBR AS COURSE_NM ";
                }
                xSql += @"      , H.CERTIFICATE_NAME || H.CERTIFICATE_KEY           AS CERTIFICATE_NO
                                , U.USER_NM_KOR                                     AS USER_NM_KOR
                                , U.USER_NM_ENG_FIRST || ' ' || USER_NM_ENG_LAST    AS USER_NM_ENG
                                , TO_CHAR(U.BIRTH_DT,'YYYY.MM.DD') AS BIRTH_DATE
                                , O.COURSE_BEGIN_DT
                                , O.COURSE_END_DT
                                , TO_CHAR(O.COURSE_BEGIN_DT,'YYYY.MM.DD') ||'~'|| TO_CHAR(O.COURSE_END_DT,'YYYY.MM.DD') AS COURSE_DATE
                                , TO_CHAR(H.INS_DT,'YYYY.MM.DD') AS ISSUE_DATE
                                , H.REASON
                                , T.REPORT_TYPE_ID
                                , T.REPORT_NM
                                , T.PAGE_HV
                                , O.COURSE_SEQ
                                , count(*) over() totalrecordcount 
                             FROM T_COURSE_REPORT_HIST H
                                , T_COURSE_RESULT R
                                , T_COURSE C
                                , T_COURSE_REPORT_ID I
                                , T_COURSE_REPORT_TYPE T
                                , T_OPEN_COURSE O
                                , T_USER U
                                , T_COMPANY P
                              WHERE H.USER_ID = R.USER_ID
                                AND H.OPEN_COURSE_ID = R.OPEN_COURSE_ID
                                AND H.COURSE_RESULT_SEQ = R.COURSE_RESULT_SEQ
                                AND C.COURSE_ID = I.COURSE_ID
                                AND I.REPORT_TYPE_ID = T.REPORT_TYPE_ID
                                AND C.COURSE_ID = O.COURSE_ID
                                AND O.OPEN_COURSE_ID = R.OPEN_COURSE_ID
                                AND R.USER_ID = U.USER_ID
                                AND U.COMPANY_ID = P.COMPANY_ID(+)
                                " + xWhere;
                xSql += @" ORDER BY H.INS_DT desc, C.COURSE_ID, H.OPEN_COURSE_ID, H.COURSE_RESULT_SEQ, H.SEQ " + "\r\n";
                xSql += @"         ) A " + "\r\n";
                xSql += @"  ) A " + "\r\n";

                if (rGubun != "all")
                {
                    if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                    {
                        xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                        xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                    }
                }
                    
                xDt = base.ExecuteDataTable(db, xSql);
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
            return xDt; ;
        }
        #endregion
        
        /************************************************************
        * Function name : SetReportHistory
        * Purpose       : 증서발급 History Insert
        * Input         : string[] rParams (0: menu_group), (1: user_id)
        *                                  (2: inquiry_yn), (3: edit_yn)
        *                                  (4: del_yn),     (5: admin_yn)
        * Output        : String Bollean Type
        *************************************************************/
        public string SetReportHistory(string[,] rParams)
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

                    for (int i = 0; i < rParams.GetLength(0); i++)
                    {
                        string xSql = string.Empty;
                        
                        xSql = " SELECT user_id, open_course_id, course_result_seq, seq FROM t_course_report_hist WHERE user_id='" + rParams[i, 0] + "' AND open_course_id='" + rParams[i, 1] + "' AND course_result_seq=" + rParams[i, 2] + " AND seq = 1 AND TO_CHAR(ins_dt, 'yyyymmdd') = TO_CHAR(sysdate, 'yyyymmdd') ";
                        DataTable xDt = base.ExecuteDataTable(db, xSql);
                        if (xDt.Rows.Count == 0)
                        {
                            xSql = string.Empty;
                            xSql += " INSERT INTO t_course_report_hist ( ";
                            xSql += " user_id, ";
                            xSql += " open_course_id, ";
                            xSql += " course_result_seq, ";
                            xSql += " seq, ";
                            xSql += " certificate_key, ";
                            xSql += " certificate_name, ";
                            xSql += " reason, ";
                            xSql += " ins_id, ";
                            xSql += " ins_dt ";
                            xSql += " ) ";
                            xSql += " VALUES ( ";
                            xSql += string.Format(" '{0}', ", rParams[i, 0]);
                            xSql += string.Format(" '{0}', ", rParams[i, 1]);
                            xSql += string.Format(" {0}, ", rParams[i, 2]);
                            xSql += string.Format(" (select (NVL(max(seq),0)+1) from t_course_report_hist WHERE user_id='{0}' AND open_course_id='{1}' AND course_result_seq={2}), ", rParams[i, 0], rParams[i, 1], rParams[i, 2]);
                            xSql += string.Format(" '{0}', ", rParams[i, 3]);
                            xSql += string.Format(" '{0}', ", rParams[i, 4]);
                            xSql += string.Format(" '{0}', ", rParams[i, 5]);
                            xSql += string.Format(" '{0}', ", rParams[i, 6]);
                            xSql += " SYSDATE ";
                            xSql += " ) ";
                        
                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xTrnsLMS);
                        }
                        else
                        {
                            xSql = string.Empty;
                            xSql += " UPDATE t_course_report_hist ";
                            xSql += " SET ";
                            //xSql += " reason = '" + rParams[i, 5] + "', ";
                            xSql += " ins_id = '" + rParams[i, 6] + "', ";
                            xSql += " ins_dt = SYSDATE ";
                            xSql += " WHERE ";
                            xSql += " user_id = '" + xDt.Rows[0]["user_id"] + "' AND ";
                            xSql += " open_course_id = '" + xDt.Rows[0]["open_course_id"] + "' AND ";
                            xSql += " course_result_seq = " + xDt.Rows[0]["course_result_seq"] + " AND ";
                            xSql += " seq = " + xDt.Rows[0]["seq"] + " ";

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xTrnsLMS);

                        }
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
                throw ex;
            }
            return xRtn;
        }
    }
}
