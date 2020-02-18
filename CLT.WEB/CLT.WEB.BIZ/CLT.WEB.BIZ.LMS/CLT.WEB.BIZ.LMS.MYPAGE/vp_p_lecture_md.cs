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
using System.Globalization;

namespace CLT.WEB.BIZ.LMS.MYPAGE
{
    /// <summary>
    /// 1. 작업개요 : 과정(등록,개설) 관련 BIZ 처리
    /// 
    /// 2. 주요기능 : 과정(등록,개설) 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_p_lecture_md
    /// 
    /// 4. 작 업 자 : 임양춘 / 2011.12.08
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        * 서진한 2012.08.01
    ///        * Source
    ///          vp_p_lecture_md
    ///        * Comment 
    ///          영문화 작업        
    ///
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        * 김은정 2012.08.08
    ///        * Source
    ///          vp_p_lecture_md
    ///        * Comment 
    ///          교육 이력 조회 시 공통사번으로 모든 이력이 조회되도록 변경
    /// </summary>
    public class vp_p_lecture_md : DAC
    {

        /************************************************************
      * Function name : GetTakingCourseList
      * Purpose       : 수강중인 과정: 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetTakingCourseList(string[] rParams)
        public DataTable GetTakingCourseList(string[] rParams, CultureInfo rArgCultureInfo)
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
                            (SELECT ";
                        if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                        {
                            xSql += " D_KNM ";
                        }
                        else
                        {
                            xSql += " D_ENM AS D_KNM ";
                        }
                 xSql += @"FROM T_CODE_DETAIL WHERE M_CD = '0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE ";

                 if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                 {
                     xSql += " , C.COURSE_NM ";
                 }
                 else
                 {
                     xSql += " ,  C.COURSE_NM_ABBR AS COURSE_NM ";
                 }
                           
                 xSql += @", C.COURSE_TYPE AS COURSE_TYPE_KEY 
                            , R.PROGRESS_RATE
                            , R.ASSESS_SCORE
                            , R.REPORT_SCORE
                            , R.TOTAL_SCORE
                            , R.OPEN_COURSE_ID   
                            , R.USER_COURSE_BEGIN_DT
                            , USER_COURSE_END_DT
                            , COUNT(*) OVER() TOTALRECORDCOUNT 
                        FROM T_COURSE C, T_OPEN_COURSE O, T_COURSE_RESULT R
                        WHERE C.COURSE_ID = O.COURSE_ID
                          AND O.OPEN_COURSE_ID = R.OPEN_COURSE_ID 
                          AND (C.COURSE_TYPE <> '000005' OR C.COURSE_TYPE IS NULL) /*OJT는제외하고 조회!!*/
                          AND R.APPROVAL_FLG = '000001'  /*승인된 애들만~!!*/
                          AND TO_CHAR(SYSDATE, 'YYYY.MM.DD') BETWEEN TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') AND TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') 
                          AND R.USER_ID = '{0}'                          
                        ORDER BY O.COURSE_BEGIN_DT DESC  
                    ) O
                )
                ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));

                xSql = string.Format(xSql, rParams[2]); 

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
      * Function name : GetCompletedCourseList
      * Purpose       : 수강완료 과정: 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetCompletedCourseList(string[] rParams)
        public DataTable GetCompletedCourseList(string[] rParams, CultureInfo rArgCultureInfo)
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
                        SELECT DISTINCT
                            (SELECT ";
                    if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                    {
                        xSql += " D_KNM ";
                    }
                    else
                    {
                        xSql += "D_ENM AS D_KNM ";
                    }
                xSql += @" FROM T_CODE_DETAIL WHERE M_CD = '0006' AND D_CD = C.COURSE_TYPE) AS COURSE_TYPE
                            , C.COURSE_TYPE AS COURSE_TYPE_KEY  ";
                  if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                  {
                      xSql += " , C.COURSE_NM ";
                  }
                  else
                  {
                      xSql += " , C.COURSE_NM_ABBR AS COURSE_NM ";
                  }
                           
                xSql += @", R.PROGRESS_RATE
                            , R.ASSESS_SCORE
                            , R.REPORT_SCORE
                            , R.TOTAL_SCORE
                            , R.OPEN_COURSE_ID   
                            , R.USER_COURSE_BEGIN_DT
                            , USER_COURSE_END_DT ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += @"        , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0010' AND D_CD = R.PASS_FLG) AS PASS_FLG ";
                }
                else
                {
                    xSql += @"        , (SELECT D_ENM FROM T_CODE_DETAIL WHERE M_CD = '0010' AND D_CD = R.PASS_FLG) AS PASS_FLG ";
                }
                xSql += @"            , COUNT(*) OVER() TOTALRECORDCOUNT 
                        FROM T_COURSE C, T_OPEN_COURSE O, T_COURSE_RESULT R
                        WHERE C.COURSE_ID = O.COURSE_ID
                          AND O.OPEN_COURSE_ID = R.OPEN_COURSE_ID 
                          AND (C.COURSE_TYPE <> '000005' OR C.COURSE_TYPE IS NULL) /*OJT는제외하고 조회!!*/
                          AND TO_CHAR(SYSDATE, 'YYYY.MM.DD') > TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') 
                          AND R.PASS_FLG = '000001'  /*이수여부가 이수인 것만 조회함 (문서영 확인)*/
                          AND R.USER_ID IN (SELECT USER_ID FROM T_USER WHERE COM_SNO =  '{0}')       
                        ORDER BY R.USER_COURSE_BEGIN_DT DESC                     
                    ) O
                )
                ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));

                xSql = string.Format(xSql, rParams[2]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 


    }
}
