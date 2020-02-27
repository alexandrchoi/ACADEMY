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
    /// 3. Class 명 : vp_p_training_md
    /// 
    /// 4. 작 업 자 : 임양춘 / 2011.12.08
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        * 서진한 2012.08.01
    ///        * Source
    ///          vp_p_training_md
    ///        * Comment 
    ///          영문화 작업        
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        * 김은정 2012.08.08
    ///        * Source
    ///          vp_p_training_md
    ///        * Comment 
    ///          교육 이력 조회 시 공통사번으로 모든 이력이 조회되도록 변경
    /// </summary>
    public class vp_p_training_md : DAC
    {
        /************************************************************
      * Function name : GetTrainingList
      * Purpose       : 수료현황 조회
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetTrainingList(string[] rParams)
        public DataTable GetTrainingList(string[] rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = this.PageSize.ToString(); // pagesize
                //xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                //xParams[2] = this.ddlCourseType.SelectedItem.Value.Replace("*", ""); //교육유형
                //xParams[3] = this.ddlPass.SelectedItem.Value.Replace("*", ""); //이수상태
                //xParams[4] = Session["USER_ID"].ToString();

                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT T.*, COUNT(*) OVER() TOTALRECORDCOUNT 
                        FROM (

                            SELECT 
                                C.COURSE_ID ";
                                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                                {
                                    xSql += " , C.COURSE_NM ";
                                }
                                else
                                {
                                    xSql += " , C.COURSE_NM_ABBR AS COURSE_NM ";
                                }
                               
                xSql += @"    , O.OPEN_COURSE_ID 
                                , TO_CHAR(R.USER_COURSE_BEGIN_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(R.USER_COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_DT
                                , C.EXPIRED_PERIOD
                                , (SELECT ";
                                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                                {
                                    xSql += " D_KNM ";
                                }
                                else
                                {
                                    xSql += " D_ENM AS D_KNM";
                                }
                
                xSql += @" FROM T_CODE_DETAIL WHERE M_CD = '0007' AND D_CD = O.EDUCATIONAL_ORG) AS EDUCATIONAL_ORG
                                , (SELECT ";
                                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                                {
                                    xSql += " D_KNM ";
                                }
                                else
                                {
                                    xSql += " D_ENM AS D_KNM";
                                }
                 
                xSql += @" FROM T_CODE_DETAIL WHERE M_CD = '0010' AND D_CD = R.PASS_FLG) AS PASS_FLG
                             , R.PASS_FLG AS PASS_FLG_CD 
                             , R.USER_COURSE_BEGIN_DT AS DT 
                            FROM T_COURSE C, T_OPEN_COURSE O, T_COURSE_RESULT R
                            WHERE C.COURSE_ID = O.COURSE_ID
                              AND O.OPEN_COURSE_ID = R.OPEN_COURSE_ID 
                              AND (C.COURSE_TYPE <> '000005' OR C.COURSE_TYPE IS NULL) /*OJT는 제외하고 조회!!*/
                              AND R.APPROVAL_FLG = '000001' /*2012.04.18*/
                              AND R.USER_ID IN (SELECT USER_ID FROM T_USER WHERE COM_SNO =  '{0}') ";

                if (rParams[2] != string.Empty)
                    xSql += " AND C.COURSE_TYPE = '" + rParams[2] + "' ";
                if (rParams[3] != string.Empty)
                    xSql += " AND R.PASS_FLG = '" + rParams[3] + "' ";

                //
                if (rParams[3] == string.Empty || rParams[3] == "000001")
                {
                    xSql += @" 
                            UNION  
                            
                            SELECT 
                                C.COURSE_ID ";
                                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                                {
                                    xSql += " , C.COURSE_NM ";
                                }
                                else
                                {
                                    xSql += " , C.COURSE_NM_ABBR AS COURSE_NM ";
                                }

                    xSql += @" , '' AS OPEN_COURSE_ID 
                                , TO_CHAR(R.COURSE_BEGIN_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(R.COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_DT
                                , C.EXPIRED_PERIOD
                                , (SELECT ";

                                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                                {
                                    xSql += " D_KNM ";
                                }
                                else
                                {
                                    xSql += " D_ENM AS D_KNM";
                                }
                        
                    xSql += @" FROM T_CODE_DETAIL WHERE M_CD = '0007' AND D_CD = R.LEARNING_INSTITUTION) AS EDUCATIONAL_ORG
                                , (SELECT ";
                                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                                {
                                    xSql += " D_KNM ";
                                }
                                else
                                {
                                    xSql += " D_ENM AS D_KNM";
                                }

                    xSql += @" FROM T_CODE_DETAIL WHERE M_CD = '0010' AND D_CD = '000001') AS PASS_FLG
                                , '000000' AS PASS_FLG_CD 
                                , R.COURSE_BEGIN_DT AS DT 
                            FROM T_REG_TRAINING_RECORD R, T_COURSE C
                            WHERE R.COURSE_ID = C.COURSE_ID
                                AND R.USER_ID IN (SELECT USER_ID FROM T_USER WHERE COM_SNO =  '{0}') ";

                    if (rParams[2] != string.Empty)
                        xSql += " AND C.COURSE_TYPE = '" + rParams[2] + "' ";
                }


                xSql += @"
                        ) T                          
                        ORDER BY DT DESC 
                    ) O
                )
                ";

                xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));

                xSql = string.Format(xSql, rParams[4]);

                //SELECT * FROM  T_REG_TRAINING_RECORD


                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
      * Function name : GetTrainingReport
      * Purpose       : 수료증 출력
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetTrainingReport(string[] rParams)
        public DataTable GetTrainingReport(string rOpenCourse, string rUser, string rImgPath)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = this.PageSize.ToString(); // pagesize
                //xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex
                //xParams[2] = this.ddlCourseType.SelectedItem.Value.Replace("*", ""); //교육유형
                //xParams[3] = this.ddlPass.SelectedItem.Value.Replace("*", ""); //이수상태
                //xParams[4] = Session["USER_ID"].ToString();


                //xSql += " c.company_nm, ";
                //xSql += " b.user_nm_kor, ";
                //xSql += " b.personal_no, ";
                //xSql += " f.course_nm, ";
                //xSql += " TO_CHAR(e.course_begin_dt, 'yyyy.MM.dd') || '~' || TO_CHAR(e.course_end_dt, 'yyyy.MM.dd') AS course_begin_end_dt, ";
                //xSql += " TO_CHAR(sysdate, 'yyyy') || '년 ' || TO_CHAR(sysdate, 'mm') || '월 ' || TO_CHAR(sysdate, 'dd') || '일' AS agree_datetime, ";

                xSql = @" 
                
                        SELECT 
                            P.COMPANY_NM
                            , U.USER_NM_KOR
                            , HINDEV.CRYPTO_AES256.DEC_AES(U.PERSONAL_NO) as PERSONAL_NO 
                            , C.COURSE_NM 
                            , CASE 
                                WHEN R.USER_COURSE_BEGIN_DT IS NOT NULL THEN (TO_CHAR(R.USER_COURSE_BEGIN_DT,'YYYY.MM.DD') ||' ~ '|| TO_CHAR(R.USER_COURSE_END_DT,'YYYY.MM.DD')) 
                                ELSE (TO_CHAR(O.COURSE_BEGIN_DT,'YYYY.MM.DD') ||' ~ '|| TO_CHAR(O.COURSE_END_DT,'YYYY.MM.DD'))    
                              END                                                               AS COURSE_BEGIN_END_DT
                            --, TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') || '~' || TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_BEGIN_END_DT
                            , TO_CHAR(SYSDATE, 'YYYY') || '년 ' || TO_CHAR(SYSDATE, 'MM') || '월 ' || TO_CHAR(SYSDATE, 'DD') || '일' AS AGREE_DATETIME                                                      
                            ";
                xSql += " , '" + rImgPath + "' AS logo1 ";
                xSql += @"
                        FROM T_COURSE C, T_OPEN_COURSE O, T_COURSE_RESULT R, T_USER U , T_COMPANY P 
                        WHERE C.COURSE_ID = O.COURSE_ID
                          AND O.OPEN_COURSE_ID = R.OPEN_COURSE_ID 
                          AND R.USER_ID = U.USER_ID 
                          AND U.COMPANY_ID = P.COMPANY_ID 

                          AND R.PASS_FLG = '000001' /*교육 수료 */ 
                          AND O.OPEN_COURSE_ID = '{0}' 
                          AND R.USER_ID = '{1}'  ";

                xSql = string.Format(xSql, rOpenCourse, rUser);


                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
      * Function name : GetTrainingOjtList
      * Purpose       : OJT 결과 조회 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetTrainingOjtList(string[] rParams)
        public DataTable GetTrainingOjtList(string[] rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = this.PageSize.ToString(); // pagesize
                //xParams[1] = this.CurrentPageIndex.ToString(); // currentPageIndex

                //xParams[2] = this.txtOjtNm.Text;
                //xParams[3] = this.txtOjtBeginDt.Text;
                //xParams[4] = this.txtOjtEndDt.Text;
                //xParams[5] = Session["USER_ID"].ToString();


                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT O.OPEN_COURSE_ID ";
                    if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                    {
                        xSql += ", C.COURSE_NM ";
                    }
                    else
                    {
                        xSql += ", C.COURSE_NM_ABBR AS COURSE_NM ";
                    }
                    xSql += @" , TO_CHAR(OH.OJT_BEGIN_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(OH.OJT_END_DT, 'YYYY.MM.DD') AS OJT_DT 
                             , OH.OJT_LEARNING_TIME
                             , SUBSTR(OH.OJT_CD, 0, 4) AS OJT_CD
                              , (SELECT ";
                    if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                    {
                        xSql += " D_KNM ";
                    }
                    else
                    {
                        xSql += " D_ENM AS D_KNM ";
                    }
                         
                        xSql += @" FROM T_CODE_DETAIL WHERE M_CD = '0010' AND D_CD = OD.PASS_FLG) PASS_FLG
                            , COUNT(*) OVER() TOTALRECORDCOUNT 
                        FROM T_COURSE C, T_OPEN_COURSE O, T_OJT OH, T_OJT_RESULT OD
                        WHERE C.COURSE_ID = O.COURSE_ID   
                          AND O.OPEN_COURSE_ID = OH.OPEN_COURSE_ID 
                          AND OH.OJT_CD = OD.OJT_CD
                          AND C.COURSE_TYPE = '000005' /*OJT만 조회*/
                          AND OD.USER_ID IN (SELECT USER_ID FROM T_USER WHERE COM_SNO =  '{0}') "; 

                          

                if (rParams[2] != string.Empty)
                    xSql += " AND UPPER(C.COURSE_NM) LIKE '%" + rParams[2].Replace("'", "''").ToUpper() + "%' ";
                if (rParams[3] != string.Empty)
                    xSql += " AND TO_CHAR(OH.OJT_BEGIN_DT, 'YYYY.MM.DD') >= '" + rParams[3] + "' ";
                if (rParams[4] != string.Empty)
                    xSql += " AND TO_CHAR(OH.OJT_END_DT, 'YYYY.MM.DD') <= '" + rParams[4] + "' ";

                xSql += @"
                       UNION
                        
                       SELECT O.OPEN_COURSE_ID ";
                        if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                        {
                            xSql += ", C.COURSE_NM ";
                        }
                        else
                        {
                            xSql += ", C.COURSE_NM_ABBR AS COURSE_NM ";
                        }

                       xSql += @", TO_CHAR(OD.USER_COURSE_BEGIN_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(OD.USER_COURSE_END_DT, 'YYYY.MM.DD') AS OJT_DT 
                            , C.COURSE_TIME
                            , NULL ";//--SUBSTR(OH.OJT_CD, 0, 4) AS OJT_CD ";
                    if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                    {
                        xSql += @"        , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0010' AND D_CD = OD.PASS_FLG) PASS_FLG ";
                    }
                    else
                    {
                        xSql += @"        , (SELECT D_ENM FROM T_CODE_DETAIL WHERE M_CD = '0010' AND D_CD = OD.PASS_FLG) PASS_FLG ";
                    }
                    xSql += @"        , COUNT(*) OVER() TOTALRECORDCOUNT 
                        FROM T_COURSE C, T_OPEN_COURSE O, T_COURSE_RESULT OD--, T_OJT OH
                        WHERE C.COURSE_ID = O.COURSE_ID   
                          AND O.OPEN_COURSE_ID = OD.OPEN_COURSE_ID 
                          AND O.OPEN_COURSE_ID = OD.OPEN_COURSE_ID
                          AND C.COURSE_TYPE = '000005' /*OJT만 조회*/
                          AND OD.USER_ID IN (SELECT USER_ID FROM T_USER WHERE COM_SNO =  '{0}')  ";
                if (rParams[2] != string.Empty)
                    xSql += " AND UPPER(C.COURSE_NM) LIKE '%" + rParams[2].Replace("'", "''").ToUpper() + "%' ";
                if (rParams[3] != string.Empty)
                    xSql += " AND TO_CHAR(OD.USER_COURSE_BEGIN_DT, 'YYYY.MM.DD') >= '" + rParams[3] + "' ";
                if (rParams[4] != string.Empty)
                    xSql += " AND TO_CHAR(OD.USER_COURSE_END_DT, 'YYYY.MM.DD') <= '" + rParams[4] + "' ";

                    xSql += @"
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

    }
}
