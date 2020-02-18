using System;
using System.Collections.Generic;
using System.Text;


// 필수 using 문
using System.Data;
using System.Data.OracleClient;
using CLT.WEB.BIZ.FX.BIZBASE;
using System.Collections;
using CLT.WEB.BIZ.LMS.COMMON;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Globalization;

namespace CLT.WEB.BIZ.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : vp_m_survey_md
    /// 
    /// 2. 주요기능 : 설문조사(육상) 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_m_survey_md
    /// 
    /// 4. 작 업 자 : 김민규 / 2011.12.14
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_m_survey_md
    ///        * Comment 
    ///          Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제따른 변경
    ///          영문화 작업

    /// 
    /// </summary>
    /// 
    public class vp_m_survey_md : DAC
    {
        /************************************************************
        * Function name : GetSurveyInfo
        * Purpose       : 설문조사정보 조회
        * Input         : string[] rParams (2: res_from) (3: res_to) (4: res_sub)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetSurveyInfo(string[] rParams)
        {
            DataTable xDt = null;
            DataTable xDtCountSum = null;
            DataTable xDtTarget = null;
            try
            {
                string xSql = string.Empty;
                if (rParams[6] == "GRID")
                {
                    xSql += "SELECT * FROM ( ";
                    xSql += "      SELECT rownum rnum, b.* FROM ( ";
                    xSql += " SELECT rownum , res.res_sub, to_char(res.ins_dt,'YYYY.MM.DD') ins_dt, to_char(res.res_from,'YYYY.MM.DD')  || ' ~ ' || to_char(res.res_to, 'YYYY.MM.DD') res_date, count(*) over() totalrecordcount, to_char(res.res_to, 'YYYY.MM.DD') res_to, ";
                    //xSql += " res.res_sum_cnt, res_rec_cnt, res.res_no, res.res_sum_cnt - res.res_rec_cnt res_nrec_cnt, res_que_cnt, res_kind, notice_yn ";
                    xSql += " res.res_no, res_que_cnt, res_kind, notice_yn, ";
                    xSql += " res.open_course_id, res.user_group, res.company_id, res.duty_step, res.socialpos ";
                    xSql += " FROM t_research res ";
                    xSql += " WHERE res.res_no IS NOT NULL ";

                    if (!string.IsNullOrEmpty(rParams[2]))
                        xSql += string.Format(" AND res_from >= TO_DATE('{0}','YYYY.MM.DD')", rParams[2]);

                    if (!string.IsNullOrEmpty(rParams[3]))
                        xSql += string.Format(" AND res_to <= TO_DATE('{0}','YYYY.MM.DD')", rParams[3]);

                    if (!string.IsNullOrEmpty(rParams[4]))
                        xSql += string.Format(" AND res_sub LIKE '%{0}%'", rParams[4]);

                    if (!string.IsNullOrEmpty(rParams[5])) // 설문 결과 화면에서 조회시 게시된 설문조사만 조회
                        xSql += string.Format(" AND notice_yn = '{0}' ", rParams[5]);

                    xSql += " ORDER BY ins_dt DESC ";
                    xSql += "              ) b ";
                    xSql += "       ) ";
                    xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                    xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));


                }
                else if (rParams[6] == "EXCEL")
                {
                    xSql += " SELECT res.res_no, res.res_sub, res_que_cnt, to_char(res.ins_dt,'YYYY.MM.DD') ins_dt, ";
                    xSql += "        to_char(res.res_from,'YYYY.MM.DD')  || ' ~ ' || to_char(res.res_to, 'YYYY.MM.DD') res_date, ";
                    xSql += "        notice_yn, ";
                    xSql += "        res.open_course_id, res.user_group, res.company_id, res.duty_step, res.socialpos ";
                    //, res.res_sum_cnt, res_rec_cnt, res.res_sum_cnt - res.res_rec_cnt res_nrec_cnt
                    xSql += "   FROM t_research res ";
                    xSql += "  WHERE res.res_no IS NOT NULL "; 
                    if (!string.IsNullOrEmpty(rParams[2]))
                        xSql += string.Format(" AND res_from >= TO_DATE('{0}','YYYY.MM.DD')", rParams[2]);

                    if (!string.IsNullOrEmpty(rParams[3]))
                        xSql += string.Format(" AND res_to <= TO_DATE('{0}','YYYY.MM.DD')", rParams[3]);

                    if (!string.IsNullOrEmpty(rParams[4]))
                        xSql += string.Format(" AND res_sub LIKE '%{0}%'", rParams[4]);

                    if (rParams.Length == 6) // 설문 결과 화면에서 조회시 게시된 설문조사만 조회
                        xSql += string.Format(" AND notice_yn = '{0}' ", rParams[5]);

                    xSql += " ORDER BY ins_dt ASC ";
                }

                xDt = base.ExecuteDataTable("LMS", xSql);
                
                string[] xUser_id = null;
                string[] xCompany_id = null;
                string[] xSocialpos = null;
                string[] xDutystep = null;

                xDt.Columns.Add("res_sum_cnt");
                xDt.Columns.Add("res_rec_cnt");
                xDt.Columns.Add("res_nrec_cnt");

                foreach (DataRow xDr in xDt.Rows)  // 조회한 설문조사에서 Count 를 계산하기위해 한번도 조회한다.
                {
                    xDtCountSum = null;
                    if (string.IsNullOrEmpty(xDr["open_course_id"].ToString()))  // 일반과정이면
                    {
                        xUser_id = xDr["user_group"].ToString().Split('┼');
                        xCompany_id = xDr["company_id"].ToString().Split('┼');
                        xSocialpos = xDr["socialpos"].ToString().Split('┼');
                        xDutystep = xDr["duty_step"].ToString().Split('┼');

                        xSql = string.Empty;
                        xSql += " SELECT COUNT(*) FROM t_user ";
                        xSql += "  WHERE user_group IN (";

                        int xCount = 0;
                        if (xUser_id.Length > 0)
                        {
                            // 사용자그룹

                            foreach (string user_group in xUser_id)
                            {
                                if (!string.IsNullOrEmpty(user_group))
                                {
                                    if (xCount == 0)
                                        xSql += string.Format("'{0}'", user_group);
                                    else
                                        xSql += string.Format(",'{0}'", user_group);
                                }
                                xCount++;
                            }
                            xSql += " ) ";
                        }

                        // 업체ID
                        if (xCompany_id.Length > 0 && !string.IsNullOrEmpty(xCompany_id[0]))
                        {
                            xSql += " AND company_id IN (";
                            xCount = 0;
                            foreach (string company_id in xCompany_id)
                            {
                                if (!string.IsNullOrEmpty(company_id))
                                {
                                    if (xCount == 0)
                                        xSql += string.Format("'{0}'", company_id);
                                    else
                                        xSql += string.Format(",'{0}'", company_id);
                                }
                                xCount++;
                            }
                            xSql += " ) ";
                        }

                        if (xSocialpos.Length > 0 && !string.IsNullOrEmpty(xSocialpos[0]))
                        {
                            xSql += " AND socialpos IN (";
                            xCount = 0;
                            foreach (string socialpos in xSocialpos)
                            {
                                if (!string.IsNullOrEmpty(socialpos))
                                {
                                    if (xCount == 0)
                                        xSql += string.Format("'{0}'", socialpos);
                                    else
                                        xSql += string.Format(",'{0}'", socialpos);
                                }
                                xCount++;
                            }
                            xSql += " ) ";
                        }

                        if (xDutystep.Length > 0 && !string.IsNullOrEmpty(xDutystep[0]))
                        {
                            xSql += " AND duty_step IN (";
                            xCount = 0;
                            foreach (string dutystep in xDutystep)
                            {
                                if (!string.IsNullOrEmpty(dutystep))
                                {
                                    if (xCount == 0)
                                        xSql += string.Format("'{0}'", dutystep);
                                    else
                                        xSql += string.Format(",'{0}'", dutystep);
                                }
                                xCount++;
                            }
                            xSql += " ) ";
                        }

                        xSql += " AND status = '000003' ";
                        //xSql += string.Format(" AND ins_dt >= TO_DATE({0},'YYYY.DD.DD')", xDr["res_to"].ToString());
                        xDtCountSum = base.ExecuteDataTable("LMS", xSql);

                        xDr["res_sum_cnt"] = xDtCountSum.Rows[0]["count(*)"].ToString();
                    }
                    else
                    {
                        xSql = string.Empty;
                        xSql += " SELECT COUNT(*) FROM t_course_result ";
                        xSql += string.Format("  WHERE open_course_id = '{0}'", xDr["open_course_id"].ToString());
                        xSql += "                  AND approval_flg = '000001' ";
                        xDtCountSum = base.ExecuteDataTable("LMS", xSql);
                        xDr["res_sum_cnt"] = xDtCountSum.Rows[0]["count(*)"].ToString();
                    }


                    xDtTarget = null;
                    // 응답자 Count를 조회한다.
                    xSql = string.Empty;
                    xSql += " SELECT COUNT(res_no) ";
                    xSql += "   FROM t_research_target ";
                    xSql += string.Format("  WHERE res_no = '{0}' ", xDr["res_no"].ToString());

                    xDtTarget = base.ExecuteDataTable("LMS", xSql);
                    xDr["res_rec_cnt"] = xDtTarget.Rows[0]["count(res_no)"].ToString();


                    int rec_cnt = Convert.ToInt32(xDr["res_rec_cnt"].ToString());
                    int sum_cnt = Convert.ToInt32(xDr["res_sum_cnt"].ToString());

                    if (rec_cnt > sum_cnt)
                    {
                        xDr["res_sum_cnt"] = sum_cnt.ToString();
                        xDr["res_nrec_cnt"] = sum_cnt.ToString();

                    }
                    else
                        xDr["res_nrec_cnt"] = Convert.ToString(sum_cnt - rec_cnt);
                    
                }

                if (rParams[6] == "EXCEL")
                {
                    //xDt.Columns.Remove("notice_yn");
                    //xDt.Columns.Remove("res_no");
                    xDt.Columns.Remove("open_course_id");
                    xDt.Columns.Remove("user_group");
                    xDt.Columns.Remove("company_id");
                    xDt.Columns.Remove("duty_step");
                    xDt.Columns.Remove("socialpos");
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
        * Function name : GetCompanyList
        * Purpose       : 설문조사 대상 선별을 위한 법인사 리스트
        * Input         : string[] rParams
        * Output        : DataTable
        *************************************************************/
        public DataTable GetCompanyList(string[] rParams, CultureInfo rArgCultureInfo)
        {
            DataTable xDt = null;
            try
            {
                string xSql = " SELECT company_id ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", company_nm ";
                  
                }
                else
                {
                    xSql += ", company_nm_eng AS company_nm ";
                    
                }
                xSql +="  FROM t_company ";
                xSql += " ORDER BY company_nm ASC";

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
        * Function name : GetResearchDetailList
        * Purpose       : 설문조사 상세정보 리스트(Update)
        * Input         : string[] rParams
        * Output        : DataTable
        *************************************************************/
        public DataTable GetResearchDetailList(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = " SELECT res_que_id, res_type, res_content, ";
                xSql += " ( ";
                xSql += string.Format(" SELECT COUNT(*) FROM t_research_choice WHERE res_no = '{0}' AND res_que_id = detail.res_que_id ", rParams[0]);
                xSql += " ) AS example_cnt";
                xSql += " FROM t_research_detail detail ";
                xSql += string.Format(" WHERE res_no = '{0}'", rParams[0]);
                xSql += " ORDER BY res_que_id ";

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
        * Function name : GetResearchChoiceList
        * Purpose       : 설문조사 보기 리스트(Update)
        * Input         : string[] rParams
        * Output        : DataTable
        *************************************************************/
        public DataTable GetResearchChoiceList(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = " SELECT res_que_id, seq, Example FROM t_research_choice ";
                xSql += string.Format(" WHERE res_no = '{0}'", rParams[0]);
                xSql += string.Format("   AND res_que_id = '{0}'", rParams[1]);
                xSql += " ORDER BY res_que_id , seq ";

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
        * Function name : GetCourseList
        * Purpose       : 과정정보 조회
        * Input         : 
        * Output        : DataTable
        *************************************************************/
        public DataTable GetCourseList(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = "SELECT course_id, course_nm FROM t_course ";
                xSql += string.Format(" WHERE use_flg = '{0}' ", rParams[1]);
                xSql += " ORDER BY course_id ASC";

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
        * Function name : GetSocialposCodeInfo
        * Purpose       : 신분구분코드용(해상직원, 용역, 실습생 등...)
        * Input         : string[] rParams (0: m_cd, 1:use_yn)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetSocialposCodeInfo(string[] rParams, CultureInfo rArgCultureInfo)
        {
            DataTable xDt = null;
            try
            {
                string xSql = " SELECT dcode ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", dname ";
                }
                else
                {
                    xSql += ", dname2 as dname ";
                }
           
                xSql += string.Format(" FROM V_DATACHKD ");
                xSql += string.Format(" WHERE hcode ='{0}' ", rParams[0]);

                if (rParams.GetLength(0) > 1)
                {
                    xSql += string.Format(" AND use_yn = '{0}' ", rParams[1]);
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
        * Function name : GetSurveyUserList
        * Purpose       : 법인사, 사용자 그룹, 직책, 직급, 과정, 과정 교육기간 에 해당하는 사용자 List Return
        * Input         : param name 참조
        * Output        : DataTable
        *************************************************************/
        /// <summary>
        /// 법인사, 사용자 그룹, 직책, 직급, 과정, 과정 교육기간 에 해당하는 사용자 List Return
        /// </summary>
        /// <param name="법인사 ArraryList"></param>
        /// <param name="사용자 그룹 ArraryList"></param>
        /// <param name="직책 ArraryList"></param>
        /// <param name="직급 ArraryList"></param>
        /// <returns></returns>
        public DataTable GetSurveyUserList(params object[] rParams)
        {
            DataTable xDt = null;

            try
            {
                //DataTable xAddUserlist = new DataTable();
                //ArrayList xalUserlist = new ArrayList();
                ArrayList ralCompany = new ArrayList();
                ArrayList ralUsergroup = new ArrayList();
                ArrayList ralSocialpos = new ArrayList();  // 직책
                ArrayList ralDutystep = new ArrayList();  // 직급

                ralCompany = (ArrayList)rParams[0];
                ralUsergroup = (ArrayList)rParams[1];
                ralSocialpos = (ArrayList)rParams[2];
                ralDutystep = (ArrayList)rParams[3];

                string xSql = "SELECT count(*) ";
                xSql += " FROM t_user ";
                xSql += " WHERE status = '000003' "; // 승인상태 : 승인

                if (ralUsergroup.Count > 0)
                {
                    xSql += " AND user_group IN (";
                    int xCount = 1;
                    foreach (string xCode in ralUsergroup)
                    {
                        if (xCount != ralUsergroup.Count)
                            xSql += string.Format("'{0}',", xCode);
                        else
                            xSql += string.Format("'{0}')", xCode);

                        xCount++;
                    }
                }
                else
                {
                    xSql += " AND user_group IN ('')";
                }

                if (ralCompany.Count > 0)
                {
                    xSql += " AND company_id IN (";
                    int xCount = 1;
                    foreach (string xCode in ralCompany)
                    {
                        if (xCount != ralCompany.Count)
                            xSql += string.Format("'{0}',", xCode);
                        else
                            xSql += string.Format("'{0}')", xCode);

                        xCount++;
                    }
                }

                if (ralSocialpos.Count > 0)
                {
                    xSql += " AND socialpos IN (";
                    int xCount = 1;
                    foreach (string xCode in ralSocialpos)
                    {
                        if (xCount != ralSocialpos.Count)
                            xSql += string.Format("'{0}',", xCode);
                        else
                            xSql += string.Format("'{0}')", xCode);

                        xCount++;
                    }
                }

                if (ralDutystep.Count > 0)
                {
                    xSql += " AND duty_step IN (";
                    int xCount = 1;
                    foreach (string xCode in ralDutystep)
                    {
                        if (xCount != ralDutystep.Count)
                            xSql += string.Format("'{0}',", xCode);
                        else
                            xSql += string.Format("'{0}')", xCode);

                        xCount++;
                    }
                }

                xDt = base.ExecuteDataTable("LMS", xSql);


                //if (ralCompany.Count > 0)  // 법인사 그룹을 기준으로 검색
                //{

                //    string xSql = "SELECT user_id ";
                //    xSql += " FROM t_user ";
                //    xSql += " WHERE status = '000003' "; // 승인상태 : 승인
                //    xSql += " AND company_id IN (";

                //    int xCount = 1;
                //    foreach (string xCode in ralCompany)
                //    {
                //        if (xCount != ralCompany.Count)
                //            xSql += string.Format("'{0}',", xCode);
                //        else
                //            xSql += string.Format("'{0}')", xCode);

                //        xCount++;
                //    }

                //    xDt = base.ExecuteDataTable("LMS", xSql);
                //    foreach (DataRow xDr in xDt.Rows)
                //    {
                //        if (!xalUserlist.Contains(xDr["user_id"].ToString()))
                //            xalUserlist.Add(xDr["user_id"].ToString());
                //    }
                //}


                //if (ralUsergroup.Count > 0)  // 사용자 그룹을 기준으로 검색
                //{

                //    string xSql = "SELECT user_id ";
                //    xSql += " FROM t_user ";
                //    xSql += " WHERE status = '000003' "; // 승인상태 : 승인
                //    xSql += " AND user_group IN (";

                //    int xCount = 1;
                //    foreach (string xCode in ralUsergroup)
                //    {
                //        if (xCount != ralUsergroup.Count)
                //            xSql += string.Format("'{0}',", xCode);
                //        else
                //            xSql += string.Format("'{0}')", xCode);

                //        xCount++;
                //    }

                //    xDt = base.ExecuteDataTable("LMS", xSql);
                //    foreach (DataRow xDr in xDt.Rows)
                //    {
                //        if (!xalUserlist.Contains(xDr["user_id"].ToString()))
                //            xalUserlist.Add(xDr["user_id"].ToString());
                //    }
                //}


                //if (ralSocialpos.Count > 0)  // 직책을 기준으로 검색
                //{

                //    string xSql = "SELECT user_id ";
                //    xSql += " FROM t_user ";
                //    xSql += " WHERE status = '000003' "; // 승인상태 : 승인
                //    xSql += " AND socialpos IN (";

                //    int xCount = 1;
                //    foreach (string xCode in ralSocialpos)
                //    {
                //        if (xCount != ralSocialpos.Count)
                //            xSql += string.Format("'{0}',", xCode);
                //        else
                //            xSql += string.Format("'{0}')", xCode);

                //        xCount++;
                //    }

                //    xDt = base.ExecuteDataTable("LMS", xSql);
                //    foreach (DataRow xDr in xDt.Rows)
                //    {
                //        if (!xalUserlist.Contains(xDr["user_id"].ToString()))
                //            xalUserlist.Add(xDr["user_id"].ToString());
                //    }
                //}


                //if (ralDurystep.Count > 0)  // 직급을 기준으로검색
                //{

                //    string xSql = "SELECT user_id ";
                //    xSql += " FROM t_user ";
                //    xSql += " WHERE status = '000003' "; // 승인상태 : 승인
                //    xSql += " AND duty_step IN (";

                //    int xCount = 1;
                //    foreach (string xCode in ralDurystep)
                //    {
                //        if (xCount != ralDurystep.Count)
                //            xSql += string.Format("'{0}',", xCode);
                //        else
                //            xSql += string.Format("'{0}')", xCode);

                //        xCount++;
                //    }

                //    xDt = base.ExecuteDataTable("LMS", xSql);
                //    foreach (DataRow xDr in xDt.Rows)
                //    {
                //        if (!xalUserlist.Contains(xDr["user_id"].ToString()))
                //            xalUserlist.Add(xDr["user_id"].ToString());
                //    }
                //}

                //xDt.Clear();
                //foreach (string user_id in xalUserlist)
                //{
                //    xDt.Rows.Add(user_id);
                //}

                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }


        /************************************************************
        * Function name : GetCurrSurveyUserList
        * Purpose       : 과정 교육기간 내에 해당 과정을 이수한 법인사, 사용자 그룹, 직책, 직급 별 사용자 List Return
        * Input         : param name 참조
        * Output        : DataTable
        *************************************************************/
        /// <summary>
        /// 과정 교육기간 내에 해당 과정을 이수한 법인사, 사용자 그룹, 직책, 직급 별 사용자 List Return
        /// </summary>
        /// <param name="법인사 ArraryList"></param>
        /// <param name="사용자 그룹 ArraryList"></param>
        /// <param name="직책 ArraryList"></param>
        /// <param name="직급 ArraryList"></param>
        /// <param name="과정ID"></param>
        /// <param name="과정 교육기간 From"></param>
        /// <param name="과정 교육기간 To"></param>
        /// <returns></returns>
        public DataTable GetCurrSurveyUserList(params object[] rParams)
        {
            DataTable xDt = null;
            try
            {
                DataTable xAddUserlist = new DataTable();
                ArrayList xalUserlist = new ArrayList();
                ArrayList ralCompany = new ArrayList();
                ArrayList ralUsergroup = new ArrayList();
                ArrayList ralSocialpos = new ArrayList();  // 직책
                ArrayList ralDurystep = new ArrayList();  // 직급

                string rCurr = string.Empty;
                string rCurrFrom = string.Empty;
                string rCurrTo = string.Empty;


                ralCompany = (ArrayList)rParams[0];
                ralUsergroup = (ArrayList)rParams[1];
                ralSocialpos = (ArrayList)rParams[2];
                ralDurystep = (ArrayList)rParams[3];
                rCurr = (string)rParams[4];
                rCurrFrom = (string)rParams[5];
                rCurrTo = (string)rParams[6];

                // 과정, 과정 교육기간별 사용자 ID 검색

                string xSql = string.Empty;
                int xCount = 1;

                xSql += " SELECT user_id FROM t_user WHERE user_id IN ( ";
                xSql += " SELECT user_id FROM t_course_result WHERE open_course_id IN ";
                xSql += " (SELECT opencourse.open_course_id  open_course_id ";
                xSql += " FROM t_course Course ";
                xSql += " INNER JOIN t_open_course openCourse ";
                xSql += " ON course.course_id = opencourse.course_id ";
                xSql += string.Format(" WHERE openCourse.open_course_id = '{0}' ", rCurr); // 과정 ID (개설과정 ID가 아님!!!)
                //xSql += string.Format(" AND opencourse.course_begin_apply_dt >= to_date('{0}','YYYY.MM.DD') ", rCurrFrom);
                //xSql += string.Format(" AND opencourse.course_begin_apply_dt <= to_date('{0}','YYYY.MM.DD') ", rCurrTo);
                xSql += " ) ";
                xSql += " ) ";
                //xSql += " ) AND (company_id IN (";
                //if (ralCompany.Count == 0)
                //    xSql += "'') ";

                //foreach (string xCode in ralCompany) // 법인사
                //{
                //    if (xCount != ralCompany.Count)
                //        xSql += string.Format("'{0}',", xCode);
                //    else
                //        xSql += string.Format("'{0}')", xCode);

                //    xCount++;
                //}

                //xSql += " OR user_group IN (";
                //if (ralUsergroup.Count == 0)
                //    xSql += "'') ";

                //xCount = 1;
                //foreach (string xCode in ralUsergroup) // 사용자 그룹
                //{
                //    if (xCount != ralUsergroup.Count)
                //        xSql += string.Format("'{0}',", xCode);
                //    else
                //        xSql += string.Format("'{0}')", xCode);

                //    xCount++;
                //}


                //xSql += " OR duty_step IN (";
                //if (ralDurystep.Count == 0)
                //    xSql += "'') ";

                //xCount = 1;
                //foreach (string xCode in ralDurystep)  // 직급(사장, 부장 과장 등등..)
                //{
                //    if (xCount != ralDurystep.Count)
                //        xSql += string.Format("'{0}',", xCode);
                //    else
                //        xSql += string.Format("'{0}')", xCode);

                //    xCount++;
                //}


                //xSql += " OR socialpos IN (";
                //if (ralSocialpos.Count == 0)
                //    xSql += "'') ";

                //xCount = 1;
                //foreach (string xCode in ralSocialpos) // 직책 (해상직원, 용역, 촉탁)
                //{
                //    if (xCount != ralSocialpos.Count)
                //        xSql += string.Format("'{0}',", xCode);
                //    else
                //        xSql += string.Format("'{0}') ", xCode);

                //    xCount++;
                //}

                //xSql += ")";


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
        * Function name : SetResearchInsert
        * Purpose       : 설문조사정보 등록
        * Input         : params object[] rParams
        * Output        : DataTable
        *************************************************************/
        public string SetResearchInsert(params object[] rParams)
        {
            string xRtn = Boolean.FalseString;
            string xQID = string.Empty;

            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTransLMS = null;
            OracleCommand xCmdLMS = null;
            try
            {
                try
                {
                    // 설문ID 생성규칙 : YYMMDD + Seq 2자리
                    vp_l_common_md com = new vp_l_common_md();
                    xQID = com.GetMaxIDOfTables(new string[] { "res_no", "t_research" });  // 설문조사 ID
                    if (string.IsNullOrEmpty(xQID)) // 만약 ID를 따지 못하거나 자릿수가 안 맞을 경우에는 Exception 처리를 해야한다...
                        return Boolean.FalseString;
                                
                    //xMasterParams[0]  선박 전송여부
                    //xMasterParams[1]  과정ID
                    //xMasterParams[2]  과정 교육기간 From
                    //xMasterParams[3]  과정 교육기간 To
                    //xMasterParams[4]  설문 문항
                    //xMasterParams[5]  설문 제목
                    //xMasterParams[6]  설문 목적
                    //xMasterParams[7]  설문 조사기간 From
                    //xMasterParams[8]  설문 조사기간 To
                    //xMasterParams[9]  설문 유형
                    //xMasterParams[10]  로그인한 ID
                    string[] xMasterParams = (string[])rParams[0];

                    //xDetailParams[i, 0]  질문
                    //xDetailParams[i, 1]  설문타입(단일선택, 다중선택 등...)
                    //xDetailParams[i, 2]  보기 항목수
                    //xDetailParams[i, 3]  보기항목 1
                    //xDetailParams[i, 4]  보기항목 2
                    //xDetailParams[i, 5]  보기항목 3
                    //xDetailParams[i, 6]  보기항목 4
                    //xDetailParams[i, 7]  보기항목 5
                    //xDetailParams[i, 8]  보기항목 6
                    //xDetailParams[i, 9]  보기항목 7
                    string[,] xDetailParams = (string[,])rParams[1];

                    // 설문 대상자
                    DataTable xDt = (DataTable)rParams[2]; 

                    xTransLMS = xCnnLMS.BeginTransaction(); // 트랜잭션 시작
                    xCmdLMS = base.GetSqlCommand(db); 
                    xCmdLMS.Connection = xCnnLMS;           // Command 에 DB 연결
                    xCmdLMS.Transaction = xTransLMS;        // Command 에 트랜잭션 연결

                    string xSql = string.Empty;
                    
                    xSql += "Insert INTO t_research ( ";
                    xSql += " res_no, ";
                    xSql += " res_sub, ";
                    xSql += " res_from, ";
                    xSql += " res_to, ";
                    xSql += " res_object, ";
                    xSql += " res_sum_cnt, ";
                    xSql += " res_rec_cnt, ";
                    xSql += " send_flg, ";
                    xSql += " open_course_id, ";
                    xSql += " res_kind, ";
                    xSql += " notice_yn, ";
                    xSql += " ins_id, ";
                    xSql += " ins_dt, ";
                    xSql += " upt_id, ";
                    xSql += " upt_dt, ";
                    xSql += " res_que_cnt,";
                    xSql += " company_id, ";
                    xSql += " user_group, ";
                    xSql += " socialpos, ";
                    xSql += " duty_step ";
                    xSql += " ) ";
                    xSql += " VALUES ( ";
                    xSql += string.Format("'{0}', ", xQID); // 설문조사 ID
                    xSql += string.Format("'{0}', ", xMasterParams[5]); // 설문조사 제목
                    xSql += string.Format("TO_DATE('{0}', 'YYYY.MM.DD'), ", xMasterParams[7]); // 설문 조사기간From
                    xSql += string.Format("TO_DATE('{0}', 'YYYY.MM.DD'), ", xMasterParams[8]); // 설문 조사기간To
                    xSql += string.Format("'{0}', ", xMasterParams[6]); // 설문목적
                    xSql += string.Format("{0}, ", "0"); // 설문 총 대상자
                    xSql += string.Format("{0}, ", "0"); // 응답자
                    xSql += string.Format("'{0}', ", xMasterParams[11]); // 선박 전송유무  1 전송대상, 3 전송안함
                    xSql += string.Format("'{0}', ", xMasterParams[1]); // 과정ID
                    xSql += string.Format("'{0}', ", xMasterParams[9]); // 설문구분(유형) (일반설문, 과정설문)
                    xSql += " 'N', ";
                    xSql += string.Format("'{0}', ", xMasterParams[10]); // 설문을 등록한 ID
                    xSql += " SYSDATE, ";
                    xSql += string.Format("'{0}', ", xMasterParams[10]); // 설문을 등록한 ID
                    xSql += " SYSDATE, ";
                    xSql += string.Format("'{0}', ", xMasterParams[4]); // 설문 문항(항목수)
                    xSql += string.Format("'{0}', ", xMasterParams[12]); // 회사 ID (구분자 : ┼)
                    xSql += string.Format("'{0}', ", xMasterParams[13]); // 사용자 그룹 (구분자 : ┼)
                    xSql += string.Format("'{0}', ", xMasterParams[14]); // 직책 (구분자 : ┼)
                    xSql += string.Format("'{0}' ", xMasterParams[15]); // 직급 (구분자 : ┼)
                    xSql += " ) ";


                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);


                    xSql = string.Empty;
                    int xQueID = Convert.ToInt32(xMasterParams[4]); // 설문 문항(항목수)

                    for (int i = 0; i < xQueID; i++)
                    {
                        xSql = string.Empty;  // 쿼리 초기화..
                        xSql += "INSERT INTO t_research_detail ( ";
                        xSql += " RES_QUE_ID, ";
                        xSql += " RES_TYPE, ";
                        xSql += " RES_CONTENT, ";
                        xSql += " RES_NO, ";
                        xSql += " INS_ID, ";
                        xSql += " INS_DT, ";
                        xSql += " UPT_ID, ";
                        xSql += " UPT_DT) ";
                        xSql += " VALUES ( ";
                        xSql += string.Format("'{0}', ", Convert.ToInt32(i+1).ToString("000")); // 설문문제 ID
                        xSql += string.Format("'{0}', ", xDetailParams[i, 1]); // 설문 타입
                        xSql += string.Format("'{0}', ", xDetailParams[i, 0]); // 설문(질문)내용
                        xSql += string.Format("'{0}', ", xQID); // 설문 ID
                        xSql += string.Format("'{0}', ", xMasterParams[10]); // 설문을 등록한 ID
                        xSql += " SYSDATE, ";
                        xSql += string.Format("'{0}', ", xMasterParams[10]); // 설문을 등록한 ID
                        xSql += " SYSDATE) ";

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);

                        int xSeq = Convert.ToInt32(xDetailParams[i, 2].ToString());
                        for (int j = 0; j < xSeq; j++)
                        {
                            xSql = string.Empty; // 쿼리 초기화
                            xSql += "INSERT INTO t_research_choice ( ";
                            xSql += " res_no, ";
                            xSql += " res_que_id, ";
                            xSql += " seq, ";
                            xSql += " example ) ";
                            xSql += " VALUES ( ";
                            xSql += string.Format("'{0}', ", xQID); // 설문조사 ID
                            xSql += string.Format("'{0}', ", Convert.ToInt32(i + 1).ToString("000")); // 설문문제 ID
                            xSql += string.Format("{0}, ", Convert.ToInt32(j + 1)); // 설문 보기 순서
                            xSql += string.Format("'{0}') ", xDetailParams[i, j + 3]); // 설문 보기

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xTransLMS);
                        }

                    }

                    //foreach(DataRow xDr in xDt.Rows)
                    //{
                    //    xSql = string.Empty; // 쿼리 초기화
                    //    xSql += "INSERT INTO t_research_target ( ";
                    //    xSql += " res_no, ";
                    //    xSql += " user_id, ";
                    //    xSql += " answer_yn) "; 
                    //    xSql += " VALUES ( ";
                    //    xSql += string.Format("'{0}', ", xQID); // 설문조사 ID
                    //    xSql += string.Format("'{0}', ", xDr["user_id"].ToString());
                    //    xSql += string.Format("'{0}') ", "N");

                    //    xCmdLMS.CommandText = xSql;
                    //    base.Execute("LMS", xCmdLMS, xTransLMS);
                    //}

                    xTransLMS.Commit(); // 트랜잭션 커밋
                    xRtn = Boolean.TrueString;

                }
                catch (Exception ex)
                {
                    xTransLMS.Rollback();
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

            return xQID;
        }


        /************************************************************
        * Function name : SetResearchDelete 
        * Purpose       : 설문조사정보 삭제 (게시되지 않은 설문조사만 삭제가능)
        * Input         : string[] rParams 
        * Output        : String Bollean Type
        *************************************************************/
        public string SetResearchDelete(string[] rParams)
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
                        string xSql = string.Empty;  // 쿼리 초기화
                        xSql += string.Format("DELETE FROM t_research_choice WHERE res_no = '{0}'", rParams[i]); // 설문 보기정보 삭제
                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTrnsLMS);

                        xSql = string.Empty;  // 쿼리 초기화
                        xSql += string.Format("DELETE FROM t_research_detail WHERE res_no = '{0}'", rParams[i]); // 설문 문항정보 삭제
                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTrnsLMS);

                        xSql = string.Empty;  // 쿼리 초기화
                        xSql += string.Format("DELETE FROM t_research_target WHERE res_no = '{0}'", rParams[i]); // 설문 대상정보 삭제
                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTrnsLMS);

                        xSql = string.Empty;  // 쿼리 초기화
                        xSql += string.Format("DELETE FROM t_research WHERE res_no = '{0}'", rParams[i]); // 설문정보 삭제
                        xCmdLMS.CommandText = xSql;
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
        * Function name : SetResearchNotice
        * Purpose       : 설문조사 게시 
        * Input         : string[] rParams 
        * Output        : String Bollean Type
        *************************************************************/
        public string SetResearchNotice(string[,] rParams)
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
                    //int j = xParams.GetLength(0);
                    for (int i = 0; i < rParams.GetLength(0); i++)
                    {
                        string xsql = " UPDATE t_research SET ";
                        xsql += string.Format(" notice_yn = '{0}' ", rParams[i,0]);
                        xsql += string.Format(" ,upt_id = '{0}' ", rParams[i,1]);
                        xsql += " ,upt_dt = SYSDATE ";
                        xsql += string.Format(" WHERE res_no = '{0}'", rParams[i,2]);

                        xCmdLMS.CommandText = xsql;
                        base.Execute(db, xCmdLMS, xTransLMS);
                    }

                    OracleParameter[] oOraParams = new OracleParameter[2];
                    oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_RESEARCH");
                    oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_RESEARCH");

                    int j = base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);


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
        * Function name : SetResearchUpdate
        * Purpose       : 설문조사 수정
        * Input         : string[] rParams 
        * Output        : String Bollean Type
        *************************************************************/
        public string SetResearchUpdate(params object[] rParams)
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

                // 0 : 설문 ID
                // 1 : 사용자가 선택한 설문 문항 갯수
                // 2 : Data를 수정한 사용자 ID
                string[] xMasterParams = (string[])rParams[0];

                // ,0 : 질문
                // ,1 : 설문형태
                // ,2 : 보기 항목수
                // ,3 : 보기1
                // ,4 : 보기2
                // ,5 : 보기3
                // ,6 : 보기4
                // ,7 : 보기5
                // ,8 : 보기6
                // ,9 : 보기7
                string[,] xDetailParams = (string[,])rParams[1];

                try
                {
                    int xCount = Convert.ToInt32(xMasterParams[1]);
                    for (int i = 0; i < xCount; i++)
                    {
                        string xSql = string.Empty;  // 쿼리 초기화..
                        xSql += " UPDATE t_research_detail SET ";
                        xSql += string.Format(" res_type = '{0}', ", xDetailParams[i, 1]); // 설문 타입
                        xSql += string.Format(" res_content = '{0}', ", xDetailParams[i, 0]); // 설문(질문)내용
                        xSql += string.Format(" upt_id = '{0}', ", xMasterParams[2]); // 사용자 ID
                        xSql += " upt_dt = SYSDATE ";
                        xSql += string.Format(" WHERE res_no = '{0}' ", xMasterParams[0]);
                        xSql += string.Format("   AND res_que_id = '{0}' ", Convert.ToInt32(i + 1).ToString("000"));


                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);

                        xSql = string.Empty;
                        xSql += " DELETE FROM t_research_choice ";
                        xSql += string.Format(" WHERE res_no = '{0}' ", xMasterParams[0]);
                        xSql += string.Format("   AND res_que_id = '{0}' ", Convert.ToInt32(i + 1).ToString("000"));

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);


                        int xSeq = Convert.ToInt32(xDetailParams[i, 2].ToString());
                        for (int j = 0; j < xSeq; j++)
                        {
                            /*
                            // Update 할지 Insert 할지 검색..
                            xSql = string.Empty;
                            xSql += " SELECT count(*) count FROM t_research_choice ";
                            xSql += string.Format(" WHERE res_no = '{0}' ", xMasterParams[0]);
                            xSql += string.Format("   AND res_que_id = '{0}' ", Convert.ToInt32(i + 1).ToString("000"));
                            xSql += string.Format("   AND seq = {0} ", j + 1);

                            DataTable  xDt = base.ExecuteDataTable("LMS", xSql);
                            string xUpdate_YN = xDt.Rows[0]["count"].ToString();  // 1이면 Update 0이면 Insert

                            if (xUpdate_YN == "1")
                            {
                                xSql = string.Empty; // 쿼리 초기화
                                xSql += " UPDATE t_research_choice SET ";
                                if (!string.IsNullOrEmpty(xDetailParams[i, j + 3].ToString()))
                                    xSql += string.Format(" example = '{0}' ", xDetailParams[i, j + 3]); // 설문 보기
                                else
                                    xSql += string.Format(" example = '{0}' ", " "); // 설문 보기
                                xSql += string.Format(" WHERE res_no = '{0}' ", xMasterParams[0]);
                                xSql += string.Format("   AND res_que_id = '{0}' ", Convert.ToInt32(i + 1).ToString("000"));
                                xSql += string.Format("   AND seq = {0} ", j + 1);
                            }
                            else if (xUpdate_YN == "0")
                            {
                                xSql = string.Empty;
                                xSql += "INSERT INTO t_research_choice ( ";
                                xSql += " res_no, ";
                                xSql += " res_que_id, ";
                                xSql += " seq, ";
                                xSql += " example ) ";
                                xSql += " VALUES ( ";
                                xSql += string.Format("'{0}', ", xMasterParams[0]); // 설문조사 ID
                                xSql += string.Format("'{0}', ", Convert.ToInt32(i + 1).ToString("000")); // 설문문제 ID
                                xSql += string.Format("{0}, ", Convert.ToInt32(j + 1)); // 설문 보기 순서
                                xSql += string.Format("'{0}') ", xDetailParams[i, j + 3]); // 설문 보기
                            }
                            */

                            xSql = string.Empty;
                            xSql += "INSERT INTO t_research_choice ( ";
                            xSql += " res_no, ";
                            xSql += " res_que_id, ";
                            xSql += " seq, ";
                            xSql += " example ) ";
                            xSql += " VALUES ( ";
                            xSql += string.Format("'{0}', ", xMasterParams[0]); // 설문조사 ID
                            xSql += string.Format("'{0}', ", Convert.ToInt32(i + 1).ToString("000")); // 설문문제 ID
                            xSql += string.Format("{0}, ", Convert.ToInt32(j + 1)); // 설문 보기 순서
                            xSql += string.Format("'{0}') ", xDetailParams[i, j + 3]); // 설문 보기

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xTransLMS);
                        }
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
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xRtn;
        }



        /************************************************************
        * Function name : GetResearchResultChoice
        * Purpose       : 설문조사 문항 조회
        * Input         : string[] rParams 
        * Output        : DataTable
        *************************************************************/
        public DataTable GetResearchResultChoice(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = "SELECT res_content, seq, example, (SELECT d_knm FROM t_code_detail where m_cd = '0054' AND d_cd=detail.res_type) res_type, detail.res_type res_typecode ";
                xSql += " FROM t_research_detail detail, t_research_choice choice ";
                xSql += " WHERE detail.res_no = choice.res_no ";
                xSql += "   AND detail.res_que_id = choice.res_que_id ";
                xSql += string.Format("  AND detail.res_no = '{0}' ", rParams[0]); // 설문 ID
                xSql += string.Format("  AND detail.res_que_id = '{0}' ", rParams[1]); // 설문항목 번호(문제번호)
                xSql += " ORDER BY choice.seq ASC ";

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
        * Function name : GetResearchResultDescription
        * Purpose       : 설문조사 문항 조회
        * Input         : string[] rParams 
        * Output        : DataTable
        *************************************************************/
        public DataTable GetResearchResultDescription(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                //string xSql = " SELECT user_id, explain ";
                //xSql += " FROM t_research_result ";
                //xSql += string.Format(" WHERE res_no = '{0}' ", rParams[0]);
                //xSql += string.Format("   AND res_que_id = '{0}' ", rParams[1]);
                //xSql += " ORDER BY user_id ASC ";

                string xSql = " SELECT tuser.user_nm_kor user_id, ' ' || result.explain AS explain ";
                xSql += " FROM t_research_result result, t_user tuser ";
                xSql += " WHERE result.user_id = tuser.user_id ";
                xSql += string.Format("   AND res_no = '{0}' ", rParams[0]);
                xSql += string.Format("   AND res_que_id = '{0}' ", rParams[1]);
                xSql += " ORDER BY user_id ASC ";


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
        * Function name : GetResearchResultArray
        * Purpose       : 설문조사 완료 체크
        * Input         : string[] rParams 
        * Output        : Datatable
        *************************************************************/
        #region GetResearchResultArray
        public DataTable GetResearchResultArray(string[] rParams)
        {
            DataTable xDt = null;
            try
            {

                string xSql = string.Empty;
                xSql += " SELECT '' || choice.seq seq, choice.example example ";
                xSql += " FROM t_research_choice choice, t_research_result result ";
                xSql += " WHERE  choice.res_no = result.res_no(+) ";
                xSql += " AND (choice.res_que_id = result.res_que_id(+)) ";
                xSql += " AND (choice.seq = result.seq(+)) ";
                xSql += string.Format(" AND choice.res_no = '{0}' ", rParams[0]); // 설문 ID
                xSql += string.Format(" AND choice.res_que_id = '{0}' ", rParams[1]); // 설문문제 번호
                xSql += " AND choice.seq = result.seq(+) ";
                xSql += " GROUP BY choice.seq, choice.example ";
                xSql += " ORDER BY choice.seq ASC ";

                xDt = base.ExecuteDataTable("LMS", xSql);


                //DataTable xStep = null;
                DataTable xExample = null;
                DataTable xResult = null;


                xSql = string.Empty;
                xSql += " SELECT seq, example FROM t_research_choice ";
                xSql += string.Format(" WHERE res_no = '{0}' ", rParams[0]); // 설문 ID
                xSql += string.Format("   AND res_que_id = '{0}' ", rParams[1]); // 설문문제 번호
                xSql += " ORDER BY seq ASC ";
                xExample = base.ExecuteDataTable("LMS", xSql);

                // Col 을 생성... 보기항목
                //xDt.Columns.Add(new DataColumn("직급", typeof(string)));
                foreach (DataRow xDr in xExample.Rows)
                {
                    xDt.Columns.Add(new DataColumn(xDr["example"].ToString(), typeof(Int32)));
                    foreach (DataRow xDrs in xDt.Rows)
                    {
                        xDrs[xDt.Columns.Count-1] = 0;
                    }
                }
                xDt.Columns.Remove("example");
                

                xSql = string.Empty;
                xSql += "SELECT * FROM t_research_result ";
                xSql += string.Format(" WHERE res_no = '{0}' ", rParams[0]);
                xSql += string.Format("   AND res_que_id = '{0}' ", rParams[1]);
                xResult = base.ExecuteDataTable("LMS", xSql);


                int xCount = 0; // 순위별 변수
                int xTotal = xResult.Rows.Count; // 전체 응답자

                foreach (DataRow xDrResult in xResult.Rows)
                {
                    string[] xEx = xDrResult["explain"].ToString().Split(',');
                    for (int i = 0; i < xEx.Length-1; i++)
                    {
                        if (!string.IsNullOrEmpty(xEx[i]))
                            xDt.Rows[i][Convert.ToInt32(xEx[i])] = (Int32)xDt.Rows[i][Convert.ToInt32(xEx[i])] + 1;
                    }
                }

                xDt.Columns[0].DataType = typeof(string);
                foreach (DataRow xDr in xDt.Rows)
                {
                    
                    xDr["seq"] = xDr["seq"].ToString() + "순위";
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }
        #endregion GetResearchResultArray

        ///************************************************************
        //* Function name : GetResearchResultChart
        //* Purpose       : 설문조사 차트용 결과 조회
        //* Input         : string[] rParams 
        //* Output        : DataTable
        //*************************************************************/
        //public DataTable GetResearchResultChart(string[] rParams)
        //{
        //    DataTable xDt = new DataTable(); //null;
        //    try
        //    {
                
        //        string xSql = " SELECT '보기' || choice.seq seq, choice.example example, count(result.seq) as count ";
        //        xSql += " FROM t_research_choice choice, t_research_result result ";
        //        xSql += " WHERE  choice.res_no = result.res_no(+) ";
        //        xSql += " AND (choice.res_que_id = result.res_que_id(+)) ";
        //        xSql += " AND (choice.seq = result.seq(+)) ";
        //        xSql += string.Format(" AND choice.res_no = '{0}' ", rParams[0]); // 설문 ID
        //        xSql += string.Format(" AND choice.res_que_id = '{0}' ", rParams[1]); // 설문문제 번호
        //        xSql += " AND choice.seq = result.seq(+) ";
        //        xSql += " GROUP BY choice.seq, choice.example ";
        //        xSql += " ORDER BY choice.seq ASC ";

        //        xDt = base.ExecuteDataTable("LMS", xSql);
                
        //        /*
        //        DataTable xStep = null;
        //        DataTable xExample = null;
        //        DataTable xCount = null;

        //        string xSql = string.Empty;

        //        xSql += " SELECT seq, example FROM t_research_choice ";
        //        xSql += string.Format(" WHERE res_no = '{0}' ", rParams[0]); // 설문 ID
        //        xSql += string.Format("   AND res_que_id = '{0}' ", rParams[1]); // 설문문제 번호
        //        xSql += " ORDER BY seq ASC ";
        //        xExample = base.ExecuteDataTable("LMS", xSql);

        //        // Col 을 생성... 보기항목
        //        xDt.Columns.Add(new DataColumn("직급", typeof(string)));
        //        foreach (DataRow xDr in xExample.Rows)
        //        {
        //            xDt.Columns.Add(new DataColumn(xDr["example"].ToString(), typeof(Int32)));
        //        }


        //        xSql = string.Empty;
                
                                

        //        xSql += " SELECT result.duty_step, step.step_name ";
        //        xSql += " FROM t_research_result result, v_hdutystep step ";
        //        xSql += " WHERE step.duty_step = result.duty_step ";
        //        xSql += string.Format("  AND result.res_no = '{0}' ", rParams[0]);
        //        xSql += string.Format( " AND result.res_que_id = '{0}' ", rParams[1]);
        //        xSql += " GROUP BY result.duty_step, step.step_name, step.step_seq ";
        //        xSql += " ORDER BY step.step_seq ASC ";
        //        xStep = base.ExecuteDataTable("LMS", xSql);

        //        // 투표한 직급 갯수만큼 Row 생성
        //        foreach (DataRow xDr in xStep.Rows) 
        //        {
        //            DataRow xDrCount = xDt.NewRow();
        //            xDrCount["직급"] = xDr["step_name"].ToString();

        //            xSql = string.Empty;
        //            xSql += " SELECT seq, count(*) AS count FROM t_research_result ";
        //            xSql += string.Format(" WHERE res_no = '{0}' ", rParams[0]);
        //            xSql += string.Format("   AND res_que_id = '{0}' ", rParams[1]);
        //            xSql += string.Format("   AND duty_step = '{0}' ", xDr["duty_step"].ToString());
        //            xSql += " GROUP BY seq ";
        //            xSql += " ORDER BY seq ";
        //            xCount = base.ExecuteDataTable("LMS", xSql);

        //            foreach (DataRow xDrs in xCount.Rows)
        //            {
        //                int xcnt = Convert.ToInt32(xDrs["seq"].ToString());
        //                xDrCount[xcnt] = Convert.ToInt32(xDrs["count"].ToString());
        //            }

        //            for (int i = 0; i < xDt.Columns.Count -1; i++)
        //            {
        //                if (string.IsNullOrEmpty(xDrCount[i + 1].ToString()))
        //                    xDrCount[i + 1] = 0;
        //            }

                    
                    
        //            xDt.Rows.Add(xDrCount); 
                    
 
        //        }
        //        */
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
        //        if (rethrow) throw;
        //    }
        //    return xDt;
        //}


        ///************************************************************
        //* Function name : GetResearchResultChart
        //* Purpose       : 설문조사 차트용 결과 조회
        //* Input         : string[] rParams 
        //* Output        : DataTable
        //*************************************************************/
        public DataTable GetResearchResultChart(string[] rParams)
        {
            DataTable xDt = new DataTable(); //null;
            try
            {

                string xSql = " SELECT 'Example ' || choice.seq seq, choice.example example, count(result.seq) as count ";
                xSql += " FROM t_research_choice choice, t_research_result result ";
                xSql += " WHERE  choice.res_no = result.res_no(+) ";
                xSql += " AND (choice.res_que_id = result.res_que_id(+)) ";
                xSql += " AND (choice.seq = result.seq(+)) ";
                xSql += string.Format(" AND choice.res_no = '{0}' ", rParams[0]); // 설문 ID
                xSql += string.Format(" AND choice.res_que_id = '{0}' ", rParams[1]); // 설문문제 번호
                xSql += " AND choice.seq = result.seq(+) ";
                xSql += " GROUP BY choice.seq, choice.example ";
                xSql += " ORDER BY choice.seq ASC ";

                xDt = base.ExecuteDataTable("LMS", xSql);

                // 보기 내용이 없으면 기타로 표기
                foreach (DataRow xDr in xDt.Rows)
                {
                    if (string.IsNullOrEmpty(xDr["example"].ToString()))
                        xDr["example"] = "Etc.";
                }

                
                /*
                DataTable xStep = null;
                DataTable xExample = null;
                DataTable xCount = null;

                string xSql = string.Empty;

                xSql += " SELECT seq, example FROM t_research_choice ";
                xSql += string.Format(" WHERE res_no = '{0}' ", rParams[0]); // 설문 ID
                xSql += string.Format("   AND res_que_id = '{0}' ", rParams[1]); // 설문문제 번호
                xSql += " ORDER BY seq ASC ";
                xExample = base.ExecuteDataTable("LMS", xSql);

                // Col 을 생성... 보기항목
                xDt.Columns.Add(new DataColumn("직급", typeof(string)));
                foreach (DataRow xDr in xExample.Rows)
                {
                    xDt.Columns.Add(new DataColumn(xDr["example"].ToString(), typeof(Int32)));
                }

                xSql = string.Empty;

                xSql += " SELECT result.duty_step, step.step_name ";
                xSql += " FROM t_research_result result, v_hdutystep step ";
                xSql += " WHERE step.duty_step = result.duty_step ";
                xSql += string.Format("  AND result.res_no = '{0}' ", rParams[0]);
                xSql += string.Format( " AND result.res_que_id = '{0}' ", rParams[1]);
                xSql += " GROUP BY result.duty_step, step.step_name, step.step_seq ";
                xSql += " ORDER BY step.step_seq ASC ";
                xStep = base.ExecuteDataTable("LMS", xSql);

                // 투표한 직급 갯수만큼 Row 생성
                foreach (DataRow xDr in xStep.Rows) 
                {
                    DataRow xDrCount = xDt.NewRow();
                    xDrCount["직급"] = xDr["step_name"].ToString();

                    xSql = string.Empty;
                    xSql += " SELECT seq, count(*) AS count FROM t_research_result ";
                    xSql += string.Format(" WHERE res_no = '{0}' ", rParams[0]);
                    xSql += string.Format("   AND res_que_id = '{0}' ", rParams[1]);
                    xSql += string.Format("   AND duty_step = '{0}' ", xDr["duty_step"].ToString());
                    xSql += " GROUP BY seq ";
                    xSql += " ORDER BY seq ";
                    xCount = base.ExecuteDataTable("LMS", xSql);

                    foreach (DataRow xDrs in xCount.Rows)
                    {
                        int xcnt = Convert.ToInt32(xDrs["seq"].ToString());
                        xDrCount[xcnt] = Convert.ToInt32(xDrs["count"].ToString());
                    }

                    for (int i = 0; i < xDt.Columns.Count -1; i++)
                    {
                        if (string.IsNullOrEmpty(xDrCount[i + 1].ToString()))
                            xDrCount[i + 1] = 0;
                    }

                    xDt.Rows.Add(xDrCount);
                }
                */
                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }

        /************************************************************
        * Function name : GetAnswerSuveyInfo
        * Purpose       : 설문대상자용 설문조사 리스트를 가져온다.
        * 
        * Input         : string[] rParams 
        * Output        : DataTable
        *************************************************************/
        public DataTable GetAnswerSuveyInfo(string[] rParams)
        {
            DataTable xDt = null;
            DataTable xDtResult = null;
            try
            {
                string xSql = string.Empty;
                
                xSql += " SELECT * FROM ( ";
                xSql += "     SELECT rownum rnum, b.*, count(*) over() totalrecordcount  FROM (";
                // 일반설문
                xSql += "          SELECT res.res_no, ";
                xSql += "                 to_char(res.res_from, 'YYYY.MM.DD') || ' ~ ' || to_char(res.res_to, 'YYYY.MM.DD') res_date, ";
                xSql += "                 res.res_sub, res.res_object res_object, ";
                xSql += "                 tuser.user_nm_kor, res.ins_id, tuser.user_id, res.res_to ";
                xSql += "            FROM t_research res, t_user tuser ";
                xSql += "           WHERE res.ins_id = tuser.user_id ";
                xSql += "             AND res.notice_yn = 'Y' ";
                xSql += "             AND res.res_kind = '000001' "; // 일반설문
                xSql += "             AND NOT EXISTS ( ";
                xSql += "                             SELECT res_no FROM t_research_target ";
                xSql += "                              WHERE res_no = res.res_no ";
                xSql += string.Format("                  AND user_id = '{0}' ", rParams[2]);
                xSql += "                                AND answer_yn = 'Y' ";
                xSql += "                            ) ";
                xSql += "             AND res.res_to >= TO_DATE(SYSDATE) ";
                xSql += string.Format(" AND res.user_group LIKE '%{0}%' ", rParams[3]);
                xSql += string.Format(" AND ((res.company_id IS NOT NULL AND res.company_id LIKE '%{0}%') OR (res.company_id IS NULL)) ", rParams[4]);
                xSql += string.Format(" AND ((res.duty_step IS NOT NULL AND res.duty_step LIKE '%{0}%') OR (res.duty_step IS NULL)) ", rParams[5]);
                xSql += string.Format(" AND ((res.socialpos IS NOT NULL AND res.socialpos LIKE '%{0}%') OR (res.socialpos IS NULL)) ", rParams[6]);
                xSql += " UNION ALL ";
                // 과정설문
                xSql += "          SELECT res.res_no, ";
                xSql += "                 to_char(res.res_from, 'YYYY.MM.DD') || ' ~ ' || to_char(res.res_to, 'YYYY.MM.DD') res_date, ";
                xSql += "                 res.res_sub, res.res_object res_object, ";
                xSql += "                 tuser.user_nm_kor, res.ins_id, tuser.user_id, res.res_to ";
                xSql += "            FROM t_research res, t_user tuser ";
                xSql += "           WHERE res.ins_id = tuser.user_id ";
                xSql += "             AND res.notice_yn = 'Y' ";
                xSql += "             AND res.res_kind = '000002' "; // 과정설문
                xSql += "             AND NOT EXISTS ( ";
                xSql += "                             SELECT res_no FROM t_research_target ";
                xSql += "                              WHERE res_no = res.res_no ";
                xSql += string.Format("                  AND user_id = '{0}' ", rParams[2]);
                xSql += "                                AND answer_yn = 'Y' ";
                xSql += "                            ) ";
                xSql += "             AND res.res_to >= TO_DATE(SYSDATE) ";
                xSql += string.Format(" AND open_course_id IN (SELECT open_course_id FROM T_COURSE_RESULT WHERE user_id = '{0}' AND approval_flg = '000001') ", rParams[2]);
                xSql += "                                                                        ) b ";
                xSql += "                                                                   ORDER BY res_to ASC ";
                xSql += "             ) ";
                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));

                xDt = base.ExecuteDataTable("LMS", xSql);

                //xDtResult = xDt.DefaultView.ToTable(true, "res_no");



                //int i = 0;
                //while (1 < xDt.Rows.Count)
                //{
                //    DataRow xDr = xDt.Rows[0];
                //    if (xDr["user_id"].ToString() != rParams[2])
                //        xDt.Rows.Remove(xDr);

                //    i++;
                //}

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }


        /************************************************************
        * Function name : Set_Survey_DB
        * Purpose       : 설문조사 저장(Insert 또는 Update)
        * Input         : void
        * Output        : DataTable
        *************************************************************/
        #region Set_Survey_DB()
        public string Set_Survey_DB(params object[] rParams)
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
                    /* 
                    * colUser_ID : UserID   사용자 ID
                    * colRes_No : Res_No 설문 ID 
                    * colRes_Que_ID : Res_Que_ID   설문문항(문제)ID
                    * colSeq : Seq : 보기 ID
                    * colDuty_Step : DutyStep 사용자 직급
                    * colExplain : 서술형일 경우 서술형 답변
                    */
                    DataTable rDt = null;
                    rDt = (DataTable)rParams[0];
                    string xSql = string.Empty;
                    foreach (DataRow chkDr in rDt.Rows)
                    {
                        string[] xChkParams = new string[4];
                        xChkParams[0] = chkDr["colUser_ID"].ToString();
                        xChkParams[1] = chkDr["colRes_No"].ToString();
                        xChkParams[2] = chkDr["colRes_Que_ID"].ToString();
                        //xChkParams[3] = rDr["colSeq"].ToString();

                        xSql = string.Empty;
                        xSql += " DELETE FROM t_research_result ";
                        xSql += string.Format("  WHERE user_id = '{0}' ", chkDr["colUser_ID"].ToString());
                        xSql += string.Format("    AND res_no = '{0}' ", chkDr["colRes_No"].ToString());
                        xSql += string.Format("    AND res_que_id = '{0}' ", chkDr["colRes_Que_ID"].ToString());

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);

                    }

                    foreach (DataRow rDr in rDt.Rows)
                    {
                        xSql = string.Empty;
                        xSql += " INSERT INTO t_research_result ( ";
                        xSql += " user_id, ";
                        xSql += " res_no, ";
                        xSql += " res_que_id, ";
                        xSql += " seq, ";
                        xSql += " explain, ";
                        xSql += " duty_step, ";
                        xSql += " ins_id, ";
                        xSql += " ins_dt) ";
                        xSql += " VALUES ( ";
                        xSql += string.Format(" '{0}', ", rDr["colUser_ID"].ToString());
                        xSql += string.Format(" '{0}', ", rDr["colRes_No"].ToString());
                        xSql += string.Format(" '{0}', ", rDr["colRes_Que_ID"].ToString());
                        xSql += string.Format(" {0}, ", rDr["colSeq"].ToString());
                        xSql += string.Format(" '{0}', ", rDr["colExplain"].ToString());
                        xSql += string.Format(" '{0}', ", rDr["colDuty_Step"].ToString());
                        xSql += string.Format(" '{0}', ", rDr["colUser_ID"].ToString());
                        xSql += " SYSDATE ) ";

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
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xRtn;
        }
        #endregion


        /************************************************************
        * Function name : Get_Survey_DB_Detail
        * Purpose       : 설문조사 상세조회(저장된 설문 조회)
        * Input         : void
        * Output        : DataTable
        *************************************************************/
        #region Get_Survey_DB_Detail
        public DataTable Get_Survey_DB_Detail(string[] rParams)
        {
            DataTable xDt = new DataTable();
            string xSql = string.Empty;
            try
            {
                xSql += string.Empty;
                xSql += " SELECT res_que_id, seq, explain ";
                xSql += "   FROM t_research_result ";
                xSql += string.Format(" WHERE user_id = '{0}' ", rParams[0]);
                xSql += string.Format("   AND res_no = '{0}' ", rParams[1]);
                xSql += " ORDER BY  res_que_id ASC, seq ASC ";

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
                xSql = string.Format(" SELECT NVL(MAX({0}+1),0) id FROM {1} ", rParams[0], rParams[1]);
                xSql += string.Format(" WHERE res_no = '{0}' ", rParams[2]);
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
        * Function name : SetSurveyAnswer
        * Purpose       : 설문조사 완료 체크
        * Input         : string[] rParams 
        * Output        : Datatable
        *************************************************************/
        #region SetSurveyAnswer(string[] rParams)
        public string SetSurveyAnswer(string[] rParams)
        {
            string xRtn = Boolean.FalseString;

            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTrnsLMS = null;
            OracleCommand xCmdLMS = null;

            try
            {
                string xResID = string.Empty;

                xTrnsLMS = xCnnLMS.BeginTransaction();  // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db); 
                xCmdLMS.Connection = xCnnLMS;
                xCmdLMS.Transaction = xTrnsLMS;

                string[] xSeqParams = new string[3];
                xSeqParams[0] = "res_rec_cnt";
                xSeqParams[1] = "t_research";
                xSeqParams[2] = rParams[0];
                string xSeq = GetMaxIDOfCode(xSeqParams, xCmdLMS);

                try
                {
                    string xSql = string.Empty;
                    xSql += string.Format(" UPDATE t_research SET res_rec_cnt = {0}", xSeq); // 응답자
                    xSql += string.Format("  WHERE res_no = '{0}' ", rParams[0]);

                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTrnsLMS);


                    xSql = string.Empty;
                    xSql += " SELECT res_no FROM t_research_target ";
                    xSql += string.Format(" WHERE res_no = '{0}' ", rParams[0]);
                    xSql += string.Format("   AND user_id = '{0}' ", rParams[1]);
                    xCmdLMS.CommandText = xSql;
                    xResID = Convert.ToString(xCmdLMS.ExecuteScalar());

                    if (!string.IsNullOrEmpty(xResID))
                    {
                        xSql = string.Empty;
                        xSql += "UPDATE t_research_target SET answer_yn = 'Y'";
                        xSql += string.Format(" WHERE res_no = '{0}' ", rParams[0]);
                        xSql += string.Format("   AND user_id = '{0}' ", rParams[1]);
                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTrnsLMS);
                    }
                    else
                    {
                        xSql = string.Empty;
                        xSql += " INSERT INTO t_research_target (res_no, user_id, answer_yn) ";
                        xSql += " VALUES ( ";
                        xSql += string.Format(" '{0}' ", rParams[0]);
                        xSql += string.Format(" ,'{0}' ", rParams[1]);
                        xSql += " ,'Y' ) ";
                        xCmdLMS.CommandText = xSql;
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
        #endregion

        #region SOCIALPOS 검색
        public DataTable GetUSERSOCIALPOS(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT socialpos FROM t_user ";
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
        #endregion

        /************************************************************
        * Function name : SetSurveyAnswer
        * Purpose       : 설문조사 완료 체크
        * Input         : string[] rParams 
        * Output        : Datatable
        *************************************************************/
        #region GetSurveyAnswerInfo(string rParams)
        public DataTable GetSurveyAnswerInfo(string rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;

                xSql += " SELECT res_no, res_sub, res_object, res_from || '~' || res_to AS res_date FROM t_research ";
                xSql += string.Format("  WHERE open_course_id = '{0}' ", rParams);
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



    }
}
