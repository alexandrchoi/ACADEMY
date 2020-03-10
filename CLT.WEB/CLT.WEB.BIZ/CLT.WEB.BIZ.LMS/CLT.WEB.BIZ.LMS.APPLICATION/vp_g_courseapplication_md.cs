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

namespace CLT.WEB.BIZ.LMS.APPLICATION
{
    /// <summary>
    /// 1. 작업개요 : 
    /// 
    /// 2. 주요기능 : 
    ///				  
    /// 3. Class 명 : vp_g_courseapplication_md
    /// 
    /// 4. 작 업 자 : 
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_g_courseapplication_md
    ///        * Comment 
    ///          * Comment 
    ///          //이러닝 과정에 대하여 수강신청 즉시 승인상태로 처리
    ///            영문화 작업
    /// </summary>
    class vp_g_courseapplication_md : DAC
    {
        /************************************************************
        * Function name : GetCourseApplication
        * Purpose       : 수강신청 List 조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public DataTable GetCourseApplication(string[] rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                //DataTable xDtClass = null;
                string xClass = string.Empty;
                string xSql = string.Empty;

                //xSql += string.Format("SELECT trainee_class FROM t_user WHERE user_id = '{0}'", rParams[2]); // 사용자 ID
                //xDtClass = base.ExecuteDataTable("LMS", xSql);
                //if (xDtClass != null && xDtClass.Rows.Count > 0)
                //{
                //    xClass = xDtClass.Rows[0]["trainee_class"].ToString();
                //}
                

                xSql = string.Empty;
                xSql = " SELECT * FROM ( ";
                xSql += " SELECT rownum  rnum, b.* FROM ( ";
                xSql += "   SELECT opencour.course_type ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", cour.course_nm ";
                }
                else
                {
                    xSql += ", cour.course_nm_abbr as course_nm ";
                }
                xSql += ", TO_CHAR(opencour.ins_dt, 'YYYY') || '-' || TO_CHAR(opencour.course_seq) course_seq, ";
                xSql += "          TO_CHAR(opencour.course_begin_apply_dt, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(opencour.course_end_apply_dt, 'YYYY.MM.DD') apply_date, ";
                xSql += "          TO_CHAR(opencour.course_begin_dt, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(opencour.course_end_dt, 'YYYY.MM.DD') course_Date, ";
                xSql += "          (opencour.course_end_dt - opencour.course_begin_dt) +1 course_day, ";
                xSql += "          count(*) over() totalrecordcount, ";
                xSql += "          (SELECT approval_flg ";
                xSql += "           FROM t_course_result ";
                xSql += "           WHERE open_course_id = opencour.open_course_id ";
                xSql += string.Format("           AND user_id = '{0}') approval_code, ", rParams[2]);  // 사용자 ID
                xSql += "          (SELECT d_knm FROM t_course_result, t_code_detail ";
                xSql += "                       WHERE t_course_result.approval_flg = t_code_detail.d_cd ";
                xSql += "                         AND t_code_detail.m_cd = '0019' ";
                xSql += "                         AND open_course_id = opencour.open_course_id ";
                xSql += string.Format("           AND user_id = '{0}') approval_flg, ", rParams[2]);  // 사용자 ID
                xSql += "          opencour.open_course_id ";
                xSql += " , (SELECT COUNT(DISTINCT R.USER_ID) FROM T_COURSE_RESULT R, T_USER U  ";
                xSql += "    WHERE R.OPEN_COURSE_ID = opencour.OPEN_COURSE_ID ";
                xSql += "      AND R.USER_ID = U.USER_ID ";
                //xSql += "      AND U.COMPANY_ID IN (SELECT COMPANY_ID FROM T_USER WHERE USER_ID = '" + rParams[2] + "')  ";
                xSql += "   ) || ' / ' || cour.CLASS_MAN_COUNT AS CLASS_MAN_COUNT ";
                xSql += "   FROM t_open_course opencour ";
                xSql += "   INNER JOIN t_course cour ";
                xSql += "   ON opencour.course_id = cour.course_id ";
                //xSql += "   WHERE opencour.open_course_id IS NOT NULL";
                xSql += "   WHERE opencour.course_end_apply_dt >= SYSDATE ";

                xSql += " AND cour.use_flg = 'Y' "; // 사용유무가 Y 인것
                xSql += " AND opencour.use_flg = 'Y' "; // 사용유무가 Y 인것

                if (rParams[3] == "000001") // 수강신청일자
                {
                    if (!string.IsNullOrEmpty(rParams[4]))
                    {
                        xSql += string.Format("   AND opencour.course_begin_apply_dt >= TO_DATE('{0}','YYYY.MM.DD.HH24MISS') ", rParams[4] + "000000");
                        xSql += string.Format("   AND opencour.course_begin_apply_dt <= TO_DATE('{0}','YYYY.MM.DD.HH24MISS') ", rParams[5] + "235959");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(rParams[4]))
                    {
                        xSql += string.Format("   AND opencour.course_begin_dt >= TO_DATE('{0}','YYYY.MM.DD.HH24MISS') ", rParams[4] + "000000");
                        xSql += string.Format("   AND opencour.course_begin_dt <= TO_DATE('{0}','YYYY.MM.DD.HH24MISS') ", rParams[5] + "235959");
                    }
                }
                if (!string.IsNullOrEmpty(rParams[6]))
                    xSql += string.Format(" AND opencour.course_id = '{0}'", rParams[6]);  // 과정ID 08050001

                if (!string.IsNullOrEmpty(rParams[7])) // 과정타입(CourseType)
                    xSql += string.Format(" AND cour.course_type = '{0}' ", rParams[7]);

                if (rParams[8] == "000006")  // 사용자그룹이 그룹사 이면 (자사 근로자)이면 (자체교육, 청년취업아카데미가 있고 사용유무가 Yes 인것
                {
                    //xSql += " AND opencour.course_type IN ('000001', '000003') ";  
                    xSql += " AND (opencour.course_type Like '%000001%' OR  opencour.course_type Like '%000003%') ";  // 000001 자체교육, 000002 사업주 위수탁, 000003 청년취업 아카데미
                }
                else if (rParams[8] == "000008")  // 사용자 그룹이 법인사 수강자 이면 (사업주 위수탁이면) (사업주 위수탁이 있고 사용유무가 Yes 인것)
                {
                    xSql += " AND opencour.course_type Like '%000002%' ";  // 000001 자체교육, 000002 사업주 위수탁, 000003 청년취업 아카데미
                }

                xSql += " ORDER BY opencour.ins_dt DESC";
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
        * Function name : GetDutyStep
        * Purpose       : 직급코드 조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public DataTable GetDutyStep(string[] rParams)
        {
            try
            {
                string xSql = " SELECT duty_step, step_name, step_ename FROM V_HDUTYSTEP ";
                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        /************************************************************
        * Function name : GetDutyWork
        * Purpose       : 직책코드 조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public DataTable GetDutyWork(string[] rParams)
        {
            try
            {
                string xSql = " SELECT duty_work, duty_work_name, duty_work_ename FROM V_HDUTYWORK ";
                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /************************************************************
        * Function name : GetCourseApplicationResult
        * Purpose       : 수강신청할 과목 상세조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public DataTable GetCourseApplicationResult(string[] rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                string xSql = string.Empty;
                xSql += "SELECT TO_CHAR(opencour.course_begin_dt, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(opencour.course_end_dt, 'YYYY.MM.DD') course_Date, ";  // 교육기간
                xSql += "       TO_CHAR(opencour.course_begin_apply_dt, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(opencour.course_end_apply_dt, 'YYYY.MM.DD') apply_date, "; // 학습시간
                xSql += "       cour.course_type, TO_CHAR(opencour.ins_dt, 'YYYY') || '-' || TO_CHAR(opencour.course_seq) course_seq, ";  // 차수
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += "       cour.course_nm, ";  // 과정명
                }
                else
                {
                    xSql += "       cour.COURSE_NM_ABBR as course_nm, ";  // 과정명
                }

                xSql += "       cour.course_intro, ";  // 과정소개
                xSql += "       cour.course_objective, ";  // 학습목표
                xSql += "       ess_duty_step, "; // 필수직급
                xSql += "       opt_duty_work, ";  // 선택직책
                xSql += "       std_progress_rate, "; // 진도율
                xSql += "       std_final_exam, "; // 기말고사
                xSql += "       std_report, "; // 레포트
                xSql += "       opencour.open_course_id, ";  // 개설과정 ID
                xSql += "       insurance_flg "; // 고용보험 대상여부
                xSql += "  FROM t_open_course opencour ";
                xSql += "  INNER JOIN t_course cour ";
                xSql += "    ON opencour.course_id = cour.course_id ";
                xSql += string.Format(" WHERE opencour.open_course_id = '{0}' ", rParams[0]);

                return base.ExecuteDataTable("LMS", xSql);     
            }
            catch (Exception ex)
            {
                
                throw ex; 
            }
        }


        /************************************************************
        * Function name : GetMaxIDOfCode
        * Purpose       : 교육이력 차수 생성
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
                xSql += " SELECT NVL(MAX(course_result_seq+1),0) FROM t_course_result ";
                xSql += string.Format(" WHERE open_course_id = '{0}' ", rParams[0]);  // 09060102
                xSql += string.Format("   AND user_id = '{0}' ", rParams[1]);  // 0315033

                rCmd.CommandText = xSql;
                xRtnID = Convert.ToString(rCmd.ExecuteScalar());

                if (xRtnID == "0")
                    xRtnID = "1";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return xRtnID;
        }

        public DataTable GetCoursePermissions(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                DataTable xDtClass = null;
                DataTable xDtClassChk = null;
                string xSql = string.Empty;
                string xClass = string.Empty;

                xSql = string.Empty;
                xSql += string.Format("SELECT user_group FROM t_user WHERE user_id = '{0}'", rParams[1]); // 사용자 ID
                xDtClass = base.ExecuteDataTable("LMS", xSql);
                if (xDtClass != null && xDtClass.Rows.Count > 0)
                {
                    xClass = xDtClass.Rows[0]["user_group"].ToString();
                }

                if (string.IsNullOrEmpty(xClass))
                    return xDtClassChk;

                xSql = string.Empty;
                xSql += "   SELECT TO_CHAR(opencour.course_begin_dt, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(opencour.course_end_dt, 'YYYY.MM.DD') course_Date, ";
                xSql += "          (opencour.course_end_dt - opencour.course_begin_dt) +1 course_day, ";
                xSql += "          (SELECT approval_flg ";
                xSql += "           FROM t_course_result ";
                xSql += "           WHERE open_course_id = opencour.open_course_id ";
                xSql += string.Format("           AND user_id = '{0}') approval_code, ", rParams[1]);  // 사용자 ID
                xSql += "          (SELECT d_knm FROM t_course_result, t_code_detail ";
                xSql += "                       WHERE t_course_result.approval_flg = t_code_detail.d_cd ";
                xSql += "                         AND t_code_detail.m_cd = '0019' ";
                xSql += "                         AND open_course_id = opencour.open_course_id ";
                xSql += string.Format("           AND user_id = '{0}') approval_flg, ", rParams[1]);  // 사용자 ID
                xSql += "          opencour.open_course_id ";
                xSql += "   FROM t_open_course opencour ";
                xSql += "   INNER JOIN t_course cour ";
                xSql += "   ON opencour.course_id = cour.course_id ";
                xSql += "   WHERE opencour.course_end_apply_dt >= SYSDATE ";
                xSql += string.Format(" AND opencour.open_course_id = '{0}' ", rParams[0]);

                if (xClass == "000006")  // 사용자그룹이 그룹사 이면 (자사 근로자)이면 (자체교육, 청년취업아카데미가 있고 사용유무가 Yes 인것
                {
                    //xSql += " AND opencour.course_type IN ('000001', '000003') ";  
                    xSql += " AND (opencour.course_type Like '%000001%' OR  opencour.course_type Like '%000003%') ";  // 000001 자체교육, 000002 사업주 위수탁, 000003 청년취업 아카데미
                }
                else if (xClass == "000008")  // 사용자 그룹이 법인사 수강자 이면 (사업주 위수탁이면) (사업주 위수탁이 있고 사용유무가 Yes 인것)
                {
                    xSql += " AND opencour.course_type Like '%000002%' ";  // 000001 자체교육, 000002 사업주 위수탁, 000003 청년취업 아카데미
                }
                //else if (xClass == "000010")  // 사용자 그룹이 개인회원이면,
                //{
                //    xSql += " AND opencour.course_type Like '%000005 %' ";  // 000005 자체교육, 000002 사업주 위수탁, 000003 청년취업 아카데미
                //}
                else
                {
                    xSql += " AND opencour.course_type IS NULL ";
                }

                xSql += " AND cour.use_flg = 'Y' "; // 사용유무가 Y 인것
                xSql += " AND opencour.use_flg = 'Y' "; // 사용유무가 Y 인것


                //if (!string.IsNullOrEmpty(rParams[6]))
                //    xSql += string.Format(" AND opencour.course_id = '{0}'", rParams[6]);  // 과정ID 08050001

                xSql += " ORDER BY TO_CHAR(opencour.ins_dt, 'YYYY') ASC, opencour.course_seq ASC ";

                xDtClassChk = base.ExecuteDataTable("LMS", xSql);
                if (xDtClassChk.Rows.Count == 0)
                    return xDtClassChk;
                


                xSql = string.Empty;
                xSql += " SELECT ess_duty_step, opt_duty_work FROM t_course ";
                xSql += string.Format(" WHERE course_id = (SELECT course_id FROM t_open_course WHERE open_course_id = '{0}') ", rParams[0]);
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
        * Function name : SetCourseApplication
        * Purpose       : 수강신청(교육이력 Insert)
        * Input         : string[] rParams
        * Output        : String
        *************************************************************/
        public string SetCourseApplication(string[] rParams)
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

                string xFilePath = string.Empty;
                string xSeq = GetMaxIDOfCode(new string[] { rParams[1], rParams[13] }, xCmdLMS);

                try
                {
                    string xSql = string.Empty;

                    //이러닝 과정에 대하여 수강신청 즉시 승인상태로 처리
                    xSql = "SELECT DECODE(TC.COURSE_TYPE,'000003','Y','N') CHK_VALUE ";
                    xSql += "FROM T_OPEN_COURSE TOC, T_COURSE TC ";
                    xSql += "WHERE TOC.COURSE_ID = TC.COURSE_ID ";
                    xSql += "AND TOC.OPEN_COURSE_ID = '" + rParams[1] + "' "; // 개설과정 ID

                    xCmdLMS.CommandText = xSql;

                    //이러닝 과정확인
                    string xElearningChk = (string)xCmdLMS.ExecuteScalar(); // Y OR N로 반환
                    
                    xSql = " INSERT INTO t_course_result ( user_id , "; // 사용자 ID
                    xSql += "                               open_course_id, "; // 개설과정 ID
                    xSql += "                               course_result_seq, "; // 이수차수
                    xSql += "                               user_company_id, ";  // 사용자 회사
                    xSql += "                               user_dept_code, ";  // 사용자 부서
                    xSql += "                               user_duty_step, ";  // 사용자 직급
                    xSql += "                               employed_state, ";  // 재직, 채용예정자 구분

                    if (xElearningChk == "N") //이러닝과정 체크하여 이러닝 아닌경우는 전달받은 값처리하고, 이러닝은 승인처리(일자포함)
                    {
                        xSql += "                               approval_flg, ";  // 승인여부
                        //xSql += "                               approval_dt, ";  // 승인일자
                    }
                    else
                    {
                        xSql += "                               approval_flg, ";  // 승인여부
                        xSql += "                               approval_dt, ";  // 승인일자
                    }
                    //xSql += "                               approval_dt, ";  // 승인일자
                    //xSql += "                               non_approval_cd, ";  // 교육불가사유 코드
                    //xSql += "                               non_approval_remark, ";  // 교육불가사유
                    xSql += "                               pass_flg, ";  // 이수여부
                    xSql += "                               progress_rate, ";  // 진도율
                    //xSql += "                               total_score, ";  // 총 취득점수
                    //xSql += "                               report_score, ";  // 과제점수
                    //xSql += "                               assess_score, ";  // 기말고사 점수
                    //xSql += "                               last_subject_id, ";  // 최종학습 과목 ID
                    //xSql += "                               last_contents_id, ";  // 최종학습 컨텐츠 ID
                    xSql += "                               user_course_begin_dt, "; // 학습 시작일자
                    xSql += "                               user_course_end_dt, "; // 학습 종료일자
                    //xSql += "                               course_start_flg, "; // 교육 입과여부
                    //xSql += "                               order_flg, "; // 발령처리여부
                    //xSql += "                               non_pass_cd, "; // 미이수 사유코드
                    //xSql += "                               non_pass_remark, "; // 미이수사유 직접입력
                    xSql += "                               insurance_flg, "; // 고용보험 여부
                    if (!string.IsNullOrEmpty(rParams[12]))
                        xSql += "                               insurance_dt, "; // 피보험 취득일자
                    xSql += "                              confirm, "; // 확정/비확정 : 0 : 비확정, 1 : 확정
                    xSql += "                               ins_id, "; // 작성자 ID
                    xSql += "                               ins_dt, "; // 작성일자
                    xSql += "                               upt_id, "; // 수정자 ID
                    xSql += "                               upt_dt) "; // 수정일자
                    xSql += " VALUES ( ";
                    xSql += string.Format(" '{0}', ", rParams[0]); // 사용자 ID
                    xSql += string.Format(" '{0}', ", rParams[1]); // 개설과정 ID
                    xSql += string.Format(" {0}, ", xSeq); // 이수 차수
                    xSql += string.Format(" '{0}', ", rParams[2]); // 사용자 회사
                    xSql += string.Format(" '{0}', ", rParams[3]); // 사용자 부서
                    xSql += string.Format(" '{0}', ", rParams[4]); // 사용자 직급
                    xSql += string.Format(" '{0}', ", rParams[5]); // 재직, 채용예정자 구분
                    if (xElearningChk == "N") //이러닝과정 체크하여 이러닝 아닌경우는 전달받은 값처리하고, 이러닝은 승인처리(일자포함)
                    {
                        xSql += string.Format(" '{0}', ", rParams[6]); // 승인여부
                        //xSql += string.Format(" '{0}', ", ); // 승인일자
                    }
                    else
                    {
                        xSql += string.Format(" '{0}', ", "000001"); // 승인여부
                        xSql += " SYSDATE, ";
                    }
                    
                    //xSql += string.Format(" '{0}', ", ); // 교육불가사유 코드
                    //xSql += string.Format(" '{0}', ", ); // 교육불가사유
                    xSql += string.Format(" '{0}', ", rParams[7]); // 이수여부
                    xSql += string.Format(" '{0}', ", rParams[8]); // 진도율
                    //xSql += string.Format(" '{0}', ", ); // 총 취득점수
                    //xSql += string.Format(" '{0}', ", ); // 과제점수
                    //xSql += string.Format(" '{0}', ", ); // 기말고사 점수
                    //xSql += string.Format(" '{0}', ", ); // 최종학습 과목 ID
                    //xSql += string.Format(" '{0}', ", ); // 최종학습 컨텐츠 ID
                    xSql += string.Format(" TO_DATE('{0}','YYYY.MM.DD'), ", rParams[9]); // 학습 시작일자
                    xSql += string.Format(" TO_DATE('{0}','YYYY.MM.DD'), ", rParams[10]); // 학습 종료일자
                    //xSql += string.Format(" '{0}', ", ); // 교육 입과여부
                    //xSql += string.Format(" '{0}', ", ); // 발령 처리여부
                    //xSql += string.Format(" '{0}', ", ); // 미이수사유 코드
                    //xSql += string.Format(" '{0}', ", ); // 미이수사유 직접입력
                    xSql += string.Format(" '{0}', ", rParams[11]); // 고용보험 여부
                    if (!string.IsNullOrEmpty(rParams[12]))
                        xSql += string.Format(" TO_DATE('{0}','YYYY.MM.DD'), ", rParams[12]); // 피보험 취득일자
                    xSql += " '1', "; // 확정/비확정 : 0 : 비확정, 1 : 확정
                    xSql += string.Format(" '{0}', ", rParams[13]); // 작성자 ID
                    xSql += string.Format(" {0}, ", "SYSDATE"); // 작성일자
                    xSql += string.Format(" '{0}', ", rParams[13]); // 수정자 ID
                    xSql += string.Format(" {0}) ", "SYSDATE"); // 수정일자

                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);

                    #region 수강신청시 설문조사 대상에 Insert
                    string[] xParams = new string[2];
                    xParams[0] = rParams[1];  // 개설과정 ID
                    xParams[1] = rParams[0];  // 사용자(수강신청자) ID
                    COMMON.vp_l_common_md common = new CLT.WEB.BIZ.LMS.COMMON.vp_l_common_md();
                    common.SetSurveyTarget(xParams, db, xCmdLMS, xTransLMS);
                    #endregion 수강신청시 설문조사 대상에 Insert

                    

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

        public string GetCourseStatus(string[] rParams)
        {
            string xRtn = Boolean.FalseString;
            try
            {
                DataTable xDt = null;
                string xSql = string.Empty;
                xSql += " SELECT approval_flg AS approval_code ";
                xSql += "   FROM t_course_result ";
                //xSql += "  WHERE open_course_id = opencour.open_course_id ";
                xSql += string.Format(" WHERE user_id = '{0}' ", rParams[1]);  // 사용자 ID
                xSql += string.Format("   AND open_course_id = '{0}'", rParams[0]); // 개설과정 ID
                xDt = base.ExecuteDataTable("LMS", xSql);

                if (xDt == null || xDt.Rows.Count == 0)
                    xRtn = Boolean.FalseString;
                else
                {
                    if (!string.IsNullOrEmpty(xDt.Rows[0]["approval_code"].ToString()))
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

        public string GetCompareAppCount(string[] rParams)
        {
            string xRtn = Boolean.FalseString;
            try
            {
                DataTable xDt = null;
                string xSql = string.Empty;
                xSql += " SELECT TO_NUMBER(NVL(C.CLASS_MAN_COUNT, '0')) - (SELECT COUNT(DISTINCT R.USER_ID) FROM T_COURSE_RESULT R WHERE R.OPEN_COURSE_ID = O.OPEN_COURSE_ID) AS ODD_MAN_COUNT ";
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
