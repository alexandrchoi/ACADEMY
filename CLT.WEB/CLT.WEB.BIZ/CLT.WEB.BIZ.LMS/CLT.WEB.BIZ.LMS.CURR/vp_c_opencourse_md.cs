using System;
using System.Collections.Generic;
using System.Text;

// 필수 using 문

using System.IO;
using System.Data;
using System.Web;
using System.Data.OracleClient;
using CLT.WEB.BIZ.FX.BIZBASE;
using System.Data.Common;
using CLT.WEB.BIZ.LMS.COMMON;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Globalization;

namespace CLT.WEB.BIZ.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : 과정개설 관련 BIZ 처리
    /// 
    /// 2. 주요기능 : 과정개설 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_c_opencourse_md
    /// 
    /// 4. 작 업 자 : 임양춘 / 2012.01.03
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_c_opencourse_md
    ///        * Comment 
    ///          영문화 작업

    /// </summary>
    public class vp_c_opencourse_md: DAC
    {
        /************************************************************
       * Function name : GetOpencourseList
       * Purpose       : 과정개설 목록 조회 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetOpencourseList(string[] rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = pagesize
                //xParams[1] = currentPageIndex

                //xParams[2] = this.txtBeginDt.Text;
                //xParams[3] = this.txtEndDt.Text;
                //xParams[4] = this.ddlCourseType.SelectedItem.Value.Replace("*", ""); //교육유형
                //xParams[5] = this.txtCourseNM.Text; //과정명

                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT
                            O.OPEN_COURSE_ID 
                            , O.COURSE_YEAR                            
                            , C.COURSE_ID
                        ";

                        if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                        {
                            xSql += ", C.COURSE_NM ";
                        }
                        else
                        {
                            xSql += ", C.COURSE_NM_ABBR AS COURSE_NM ";
                        }

                        xSql += @" , (SELECT ";
                        if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                        {
                            xSql += " D_KNM ";
                        }
                        else
                        {
                            xSql += " D_ENM AS D_KNM";
                        }

                        xSql += @" FROM T_CODE_DETAIL
                             WHERE M_CD = '0017' AND D_CD = C.COURSE_LANG ) AS COURSE_LANG 
                            , O.COURSE_SEQ                            
                            , TO_CHAR(O.COURSE_BEGIN_APPLY_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(O.COURSE_END_APPLY_DT, 'YYYY.MM.DD') AS COURSE_APPLY_DATE
                            , TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_DATE
                            , O.INS_DT
                            , DECODE(O.USE_FLG , 'Y', 'YES', 'NO') AS USE_FLG 
                            , COUNT(*) OVER() TOTALRECORDCOUNT 
                            , O.COURSE_TYPE
                            , O.MANAGER
                        FROM T_OPEN_COURSE O, T_COURSE C
                        WHERE O.COURSE_ID = C.COURSE_ID(+) 
                            AND C.COURSE_TYPE <> '000005'  /* COURSE에서 COURSE_TYPE이 OJT일 경우 제외하여 DISPLAY*/
                        ";

                if (rParams[2] != string.Empty)
                    xSql += " AND TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') >= '" + rParams[2] + "' ";
                if (rParams[3] != string.Empty)
                    xSql += " AND TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') <= '" + rParams[3] + "' ";
                if (rParams[4] != string.Empty)
                    xSql += " AND C.COURSE_TYPE = '" + rParams[4] + "' ";
                if (rParams[5] != string.Empty)
                    xSql += " AND UPPER(C.COURSE_NM) LIKE '%" + rParams[5].Replace("'", "''").ToUpper() + "%' ";

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

        //GetOpencourseList_Excel
        /************************************************************
      * Function name : GetOpencourseList_Excel
      * Purpose       : 과정개설 목록 excel 출력 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        public DataTable GetOpencourseList_Excel(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = pagesize
                //xParams[1] = currentPageIndex

                //xParams[2] = this.txtBeginDt.Text;
                //xParams[3] = this.txtEndDt.Text;
                //xParams[4] = this.ddlCourseType.SelectedItem.Value.Replace("*", ""); //교육유형
                //xParams[5] = this.txtCourseNM.Text; //과정명

                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT
                            --O.OPEN_COURSE_ID 
                            --, 
                            O.COURSE_YEAR                            
                            , C.COURSE_ID
                            , C.COURSE_NM 
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0017' AND D_CD = C.COURSE_LANG ) AS COURSE_LANG 
                            , O.COURSE_SEQ                            
                            , TO_CHAR(O.COURSE_BEGIN_APPLY_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(O.COURSE_END_APPLY_DT, 'YYYY.MM.DD') AS COURSE_APPLY_DATE
                            , TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_DATE
                            , TO_CHAR(O.INS_DT, 'YYYY.MM.DD') AS INS_DT 
                            , DECODE(O.USE_FLG , 'Y', 'YES', 'NO') AS USE_FLG 
                            --, COUNT(*) OVER() TOTALRECORDCOUNT 
                        FROM T_OPEN_COURSE O, T_COURSE C
                        WHERE O.COURSE_ID = C.COURSE_ID(+) 
                            AND C.COURSE_TYPE <> '000005'  /* COURSE에서 COURSE_TYPE이 OJT일 경우 제외하여 DISPLAY*/
                        ";

                if (rParams[2] != string.Empty)
                    xSql += " AND TO_CHAR(O.COURSE_BEGIN_DT, 'YYYY.MM.DD') >= '" + rParams[2] + "' ";
                if (rParams[3] != string.Empty)
                    xSql += " AND TO_CHAR(O.COURSE_END_DT, 'YYYY.MM.DD') <= '" + rParams[3] + "' ";
                if (rParams[4] != string.Empty)
                    xSql += " AND C.COURSE_TYPE = '" + rParams[4] + "' ";
                if (rParams[5] != string.Empty)
                    xSql += " AND UPPER(C.COURSE_NM) LIKE '%" + rParams[5].Replace("'", "''").ToUpper() + "%' ";

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

        /************************************************************
       * Function name : GetCompany
       * Purpose       : Company Accept 정보 가져오기 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetCompany(CultureInfo rArgCultureInfo)
        {
            try
            {
                string xSql = string.Empty;

                xSql = @" SELECT COMPANY_ID ";
                
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += " , COMPANY_NM ";
                }
                else
                {
                    xSql += ", COMPANY_NM_ENG AS COMPANY_NM ";
                }
               
                xSql += " FROM T_COMPANY WHERE STATUS = '000003' ";        //STATUS = '000003' : 승인         

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /************************************************************
       * Function name : GetOpencourseInfo
       * Purpose       : 개설과정 상세조회 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetOpencourseInfo(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                xSql = @" 
                SELECT 

                    /*
                    CASE WHEN C.COURSE_INOUT = '000001' THEN (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0008' AND D_CD = C.COURSE_PLACE) 
                            WHEN C.COURSE_INOUT = '000002' THEN (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0009' AND D_CD = C.COURSE_PLACE) 
                      END AS COURSE_PLACE
                    */
                    
                    OC.COURSE_INOUT
                    , OC.COURSE_PLACE

                    , C.COURSE_ID
                    , C.COURSE_NM 

                    , OC.COURSE_YEAR
                    , OC.COURSE_SEQ
                    , OC.EDUCATIONAL_ORG
                    , OC.TRAINING_FEE
                    , OC.COURSE_TYPE
                    , OC.PASS_SCORE
                    , OC.TRAINING_SUPPORT_FEE
                    , OC.TRAINING_SUPPORT_COMP_FEE
                    , TO_CHAR(OC.COURSE_BEGIN_APPLY_DT, 'YYYY.MM.DD') AS COURSE_BEGIN_APPLY_DT
                    , TO_CHAR(OC.COURSE_END_APPLY_DT, 'YYYY.MM.DD') AS COURSE_END_APPLY_DT
                    , TO_CHAR(OC.COURSE_BEGIN_DT, 'YYYY.MM.DD') AS COURSE_BEGIN_DT
                    , TO_CHAR(OC.COURSE_END_DT, 'YYYY.MM.DD') AS COURSE_END_DT
                    , TO_CHAR(OC.RES_BEGIN_DT, 'YYYY.MM.DD') AS RES_BEGIN_DT
                    , TO_CHAR(OC.RES_END_DT, 'YYYY.MM.DD') AS RES_END_DT
                    , OC.MIN_MAN_COUNT
                    , OC.MAX_MAN_COUNT
                    , OC.COMPANY_ACCEPT
                    , OC.STD_PROGRESS_RATE
                    , OC.STD_FINAL_EXAM
                    , OC.USE_FLG
                    , OC.OPEN_COURSE_ID
                    , OC.RES_NO 
                    , R.RES_SUB
                    , OC.COURSE_GUBUN
                    , OC.MANAGER
                FROM T_OPEN_COURSE OC, T_COURSE C, T_RESEARCH R
                WHERE OC.COURSE_ID = C.COURSE_ID(+)
                  AND OC.RES_NO = R.RES_NO(+)
                  AND OC.OPEN_COURSE_ID = '{0}' 
                "; 
                xSql = string.Format(xSql, rParams[0]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
       * Function name : GetCoursePlace
       * Purpose       : Course의 Course Place 가져오기 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        public string GetCoursePlace(string rCoursId)
        {
            try
            {
                string xSql = string.Empty;

                xSql = @" 
                    SELECT CASE WHEN C.COURSE_INOUT = '000001' THEN (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0008' AND D_CD = C.COURSE_PLACE) 
                                       WHEN C.COURSE_INOUT = '000002' THEN (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0009' AND D_CD = C.COURSE_PLACE) 
                               END AS COURSE_PLACE
                    FROM T_COURSE C
                    WHERE COURSE_ID = '{0}' ";
                xSql = string.Format(xSql, rCoursId); 
                object xobj = base.ExecuteScalar("LMS", xSql);

                return (xobj == null ? string.Empty : xobj.ToString()); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /************************************************************
      * Function name : SetOpencourseInfo
      * Purpose       : 개설과정(open course) 신규 항목을 등록하는 처리
      * Input         : 
      * Output        : string
      *************************************************************/
        public string SetOpencourseInfo(object[] rParams)
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
                        //xCmdLMS.Connection = xCnnLMS;
                        //xCmdLMS.Transaction = xTransLMS;

                        OracleParameter[] xPara = null;

                        // 2. DB 저장 (DB 작업 중에 문제발생 시, 저장된 파일 제거)
                        xSql = @" INSERT INTO T_OPEN_COURSE 
                                    ( OPEN_COURSE_ID
                                    , COURSE_ID
                                    , COURSE_YEAR
                                    , COURSE_SEQ
                                    , EDUCATIONAL_ORG
                                    , COURSE_BEGIN_DT
                                    , COURSE_END_DT
                                    , COURSE_BEGIN_APPLY_DT
                                    , COURSE_END_APPLY_DT
                                    , RES_BEGIN_DT
                                    , RES_END_DT
                                    , COURSE_TYPE
                                    , STD_PROGRESS_RATE
                                    , STD_FINAL_EXAM
                                    , STD_REPORT
                                    , PASS_SCORE
                                    , COMPANY_ACCEPT
                                    , MIN_MAN_COUNT
                                    , MAX_MAN_COUNT
                                    , TRAINING_FEE
                                    , TRAINING_SUPPORT_FEE
                                    , TRAINING_SUPPORT_COMP_FEE
                                    , RES_NO 
                                    , INS_ID
                                    , INS_DT
                                    , SEND_DT
                                    , SEND_FLG
                                    , USE_FLG
                                    , COURSE_INOUT
                                    , COURSE_PLACE
                                    , COURSE_GUBUN
                                    , MANAGER
                                    ) VALUES (
                                    :OPEN_COURSE_ID
                                    , :COURSE_ID
                                    , :COURSE_YEAR
                                    , (SELECT NVL(MAX(COURSE_SEQ) + 1, 1) FROM T_OPEN_COURSE WHERE COURSE_ID = :COURSE_ID)
                                    , :EDUCATIONAL_ORG
                                    , :COURSE_BEGIN_DT
                                    , :COURSE_END_DT
                                    , :COURSE_BEGIN_APPLY_DT
                                    , :COURSE_END_APPLY_DT
                                    , :RES_BEGIN_DT
                                    , :RES_END_DT
                                    , :COURSE_TYPE
                                    , :STD_PROGRESS_RATE
                                    , :STD_FINAL_EXAM
                                    , :STD_REPORT
                                    , :PASS_SCORE
                                    , :COMPANY_ACCEPT
                                    , :MIN_MAN_COUNT
                                    , :MAX_MAN_COUNT
                                    , :TRAINING_FEE
                                    , :TRAINING_SUPPORT_FEE
                                    , :TRAINING_SUPPORT_COMP_FEE
                                    , :RES_NO 
                                    , :INS_ID
                                    , SYSDATE
                                    , :SEND_DT
                                    , :SEND_FLG
                                    , :USE_FLG
                                    , :COURSE_INOUT
                                    , :COURSE_PLACE
                                    , :COURSE_GUBUN
                                    , :MANAGER
                                    ) ";

                        vp_l_common_md com = new vp_l_common_md();
                        string xQID = com.GetMaxIDOfTable(new string[] { "T_OPEN_COURSE", "OPEN_COURSE_ID" });

                        xPara = new OracleParameter[30];
                        xPara[0] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, xQID);
                        xPara[1] = base.AddParam("COURSE_ID", OracleType.VarChar, rParams[1]);
                        xPara[2] = base.AddParam("COURSE_YEAR", OracleType.VarChar, rParams[2]);
                        //xPara[3] = base.AddParam("COURSE_SEQ", OracleType.Number, rParams[3]);
                        xPara[3] = base.AddParam("COURSE_INOUT", OracleType.VarChar, rParams[24]);
                        xPara[4] = base.AddParam("EDUCATIONAL_ORG", OracleType.VarChar, rParams[4]);

                        xPara[5] = base.AddParam("COURSE_BEGIN_DT", OracleType.DateTime, rParams[12]);
                        xPara[6] = base.AddParam("COURSE_END_DT", OracleType.DateTime, rParams[13]);
                        xPara[7] = base.AddParam("COURSE_BEGIN_APPLY_DT", OracleType.DateTime, rParams[10]);
                        xPara[8] = base.AddParam("COURSE_END_APPLY_DT", OracleType.DateTime, rParams[11]);
                        xPara[9] = base.AddParam("RES_BEGIN_DT", OracleType.DateTime, rParams[14]);
                        xPara[10] = base.AddParam("RES_END_DT", OracleType.DateTime, rParams[15]);

                        xPara[11] = base.AddParam("COURSE_TYPE", OracleType.VarChar, rParams[6]);
                        xPara[12] = base.AddParam("STD_PROGRESS_RATE", OracleType.Number, rParams[19]);
                        xPara[13] = base.AddParam("STD_FINAL_EXAM", OracleType.Number, rParams[20]);
                        xPara[14] = base.AddParam("STD_REPORT", OracleType.Number, DBNull.Value);  //현재는 값 넘어오지 않음 
                        xPara[15] = base.AddParam("PASS_SCORE", OracleType.Number, rParams[7]);
                        xPara[16] = base.AddParam("COMPANY_ACCEPT", OracleType.VarChar, rParams[18]);
                        xPara[17] = base.AddParam("MIN_MAN_COUNT", OracleType.Number, rParams[16]);
                        xPara[18] = base.AddParam("MAX_MAN_COUNT", OracleType.Number, rParams[17]);
                        xPara[19] = base.AddParam("TRAINING_FEE", OracleType.Number, rParams[5]);
                        xPara[20] = base.AddParam("TRAINING_SUPPORT_FEE", OracleType.Number, rParams[8]);
                        xPara[21] = base.AddParam("TRAINING_SUPPORT_COMP_FEE", OracleType.Number, rParams[9]);                        
                        xPara[22] = base.AddParam("INS_ID", OracleType.VarChar, rParams[22]);
                        //xPara[11] = base.AddParam("INS_DT", OracleType.VarChar, rParams[11]);
                        xPara[23] = base.AddParam("SEND_DT", OracleType.VarChar,DBNull.Value);
                        xPara[24] = base.AddParam("SEND_FLG", OracleType.VarChar, "1");
                        xPara[25] = base.AddParam("USE_FLG", OracleType.VarChar, rParams[21]);
                        xPara[26] = base.AddParam("RES_NO", OracleType.VarChar, rParams[23]);
                        xPara[27] = base.AddParam("COURSE_PLACE", OracleType.VarChar, rParams[25]);
                        xPara[28] = base.AddParam("COURSE_GUBUN", OracleType.Char, rParams[26]);  // 국토해양부 과정여부 추가
                        xPara[29] = base.AddParam("MANAGER", OracleType.VarChar, rParams[27]);  //

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xPara, xTransLMS);


                        //선박발송 하는 PACKAGE 호출 부분 추가 필요 
                        //사업주 위탁 교육은 본선으로 발송 하지 않는다!! COURSE_TYPE 
                        if (rParams[6].ToString() != "000002|")
                        {
                            OracleParameter[] oOraParams = new OracleParameter[2];
                            oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_OPEN_COURSE");
                            oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_OPEN_COURSE");

                            base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);
                        }
                        

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




        /************************************************************
       * Function name : GetSurveyInfo
       * Purpose       : 설문조사 
       * Input         : 
       * Output        : DataTable
       *************************************************************/
        public DataTable GetSurveyInfo(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                //xParams[0] = pagesize
                //xParams[1] = currentPageIndex

                //xParams[2] = this.txtBeginDt.Text;
                //xParams[3] = this.txtEndDt.Text;
                //xParams[4] = this.txtResNm.Text; 

                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, O.* FROM 
                    (
                        SELECT 
                          RES.RES_SUB,
                          TO_CHAR(RES.INS_DT, 'YYYY.MM.DD') INS_DT,
                          TO_CHAR(RES.RES_FROM, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(RES.RES_TO, 'YYYY.MM.DD') RES_DATE,
                          RES.RES_SUM_CNT,
                          RES_REC_CNT,
                          RES.RES_NO,
                          RES.RES_SUM_CNT - RES.RES_REC_CNT RES_NREC_CNT,
                          RES_QUE_CNT,
                          RES_KIND,
                          NOTICE_YN
                          , COUNT(*) OVER() TOTALRECORDCOUNT 
                        FROM T_RESEARCH RES
                        WHERE 1=1 
                        ";

                if (rParams[2] != string.Empty)
                    xSql += " AND TO_CHAR(RES.RES_FROM, 'YYYY.MM.DD') >= '" + rParams[2] + "' ";
                if (rParams[3] != string.Empty)
                    xSql += " AND TO_CHAR(RES.RES_TO, 'YYYY.MM.DD') <= '" + rParams[3] + "' ";
                if (rParams[4] != string.Empty)
                    xSql += " AND UPPER(RES.RES_SUB) LIKE '%" + rParams[4].Replace("'", "''").ToUpper() + "%' ";

                xSql += @"                  
                        ORDER BY RES.INS_DT DESC 
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


    }
}
