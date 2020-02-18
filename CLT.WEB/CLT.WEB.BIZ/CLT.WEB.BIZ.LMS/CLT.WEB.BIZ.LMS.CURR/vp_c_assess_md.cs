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

namespace CLT.WEB.BIZ.LMS.CURR
{ 
    /// <summary>
    /// 1. 작업개요 : vp_c_assess_md Class
    /// 
    /// 2. 주요기능 : 평가 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_c_assess_md
    /// 
    /// 4. 작 업 자 : 임양춘 / 2011.12.26
    /// </summary>
    public class vp_c_assess_md : DAC
    {
        /************************************************************
       * Function name : GetContentsInfo
       * Purpose       : 평가문제 목록 조회 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetAssessList(string[] rParams)
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
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0003' AND D_CD = COURSE_GROUP) AS COURSE_GROUP
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0004' AND D_CD = COURSE_FIELD) AS COURSE_FIELD
                            , QUESTION_CONTENT 
                            , (SELECT D_KNM FROM T_CODE_DETAIL WHERE M_CD = '0045' AND D_CD = QUESTION_TYPE) AS QUESTION_TYPE
                            , INS_DT
                            , COUNT(*) OVER() TOTALRECORDCOUNT     
                            , QUESTION_ID 
                        FROM T_ASSESS_QUESTION
                        WHERE (TEMP_SAVE_FLG <> 'Y' OR TEMP_SAVE_FLG IS NULL) ";

                if(rParams[2] != string.Empty)
                    xSql +=" AND QUESTION_KIND = '" + rParams[2] + "' ";
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


        /************************************************************
       * Function name : GetAssessInfo
       * Purpose       : 평가문제 상세조회 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetAssessInfo(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;                

                xSql = " SELECT * FROM T_ASSESS_QUESTION ";                 
                xSql += string.Format("        WHERE QUESTION_ID ='{0}' ", rParams[0]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /************************************************************
       * Function name : GetAssessCourseInfo
       * Purpose       : 평가문제 course list 상세조회 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetAssessCourseInfo(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                xSql = " SELECT COURSE_ID || ' | ' || COURSE_NM  AS COURSE_NM , COURSE_ID FROM T_COURSE ";
                xSql += string.Format("        WHERE COURSE_ID IN ({0}) ", rParams[0]);                

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
       * Function name : GetAssessSubjectInfo
       * Purpose       : 평가문제 subject list 상세조회 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetAssessSubjectInfo(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                xSql = " SELECT SUBJECT_ID || ' | ' || SUBJECT_NM  AS SUBJECT_NM, SUBJECT_ID FROM T_SUBJECT ";
                xSql += string.Format("        WHERE SUBJECT_ID IN ({0}) ", rParams[0]);                

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /************************************************************
       * Function name : SetAssessInfo
       * Purpose       : 평가문제(ASSESS QUESTION) 신규 항목을 등록하는 처리
       * Input         : 
       * Output        : string
       *************************************************************/
        public string SetAssessInfo(object[] rParams)
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

                        bool xUpdate = false; //update 할지 여부 
                        string xQID = string.Empty;

                        if (rParams[12].ToString() == "Y")
                        {
                            //subject id를 확인 하여 temp_save_flg = 'Y' 이면 update
                            //아니면 insert 하기                             
                            xSql = " SELECT TEMP_SAVE_FLG FROM T_ASSESS_QUESTION WHERE QUESTION_ID = '" + rParams[0] + "' ";
                            xCmdLMS.CommandText = xSql;

                            object xobj = base.ExecuteScalar(db, xCmdLMS, xTransLMS);
                            if (xobj != null && xobj.ToString() == "Y")
                                xUpdate = true;
                            else
                                xUpdate = false; //temp_save_flg = 'Y', send_flg = "1"
                        }
                        else
                        {
                            //무조건 insert 
                            xUpdate = false;  //temp_save_flg = 'N', send_flg = "1"
                        }


                        if (xUpdate)
                        {
                            xQID = rParams[0].ToString();

                            xSql = @" UPDATE T_ASSESS_QUESTION SET 
                                    QUESTION_KIND = :QUESTION_KIND
                                    , QUESTION_LANG = :QUESTION_LANG
                                    , QUESTION_TYPE   = :QUESTION_TYPE
                                    , COURSE_GROUP = :COURSE_GROUP
                                    , COURSE_FIELD = :COURSE_FIELD
                                    , QUESTION_SCORE = :QUESTION_SCORE
                                    , QUESTION_ANSWER = :QUESTION_ANSWER
                                    , QUESTION_CONTENT = :QUESTION_CONTENT
                                    , QUESTION_EXAMPLE = :QUESTION_EXAMPLE
                                    , QUESTION_DESC = :QUESTION_DESC
                                    , UPT_ID = :INS_ID
                                    , UPT_DT = SYSDATE
                                    , USE_FLG = :USE_FLG
                                    , TEMP_SAVE_FLG = :TEMP_SAVE_FLG
                                    , COURSE_LIST = :COURSE_LIST
                                    , SUBJECT_LIST = :SUBJECT_LIST
                                    WHERE QUESTION_ID = :QUESTION_ID 
                                ";

                            xPara = new OracleParameter[16];
                            xPara[0] = base.AddParam("QUESTION_ID", OracleType.VarChar, xQID);
                            xPara[1] = base.AddParam("QUESTION_KIND", OracleType.VarChar, rParams[1]);
                            xPara[2] = base.AddParam("QUESTION_LANG", OracleType.VarChar, rParams[2]);
                            xPara[3] = base.AddParam("QUESTION_TYPE", OracleType.VarChar, rParams[3]);
                            xPara[4] = base.AddParam("COURSE_GROUP", OracleType.VarChar, rParams[4]);
                            xPara[5] = base.AddParam("COURSE_FIELD", OracleType.VarChar, rParams[5]);
                            xPara[6] = base.AddParam("QUESTION_SCORE", OracleType.Number, rParams[10]);
                            xPara[7] = base.AddParam("QUESTION_ANSWER", OracleType.Clob, rParams[6]);
                            xPara[8] = base.AddParam("QUESTION_CONTENT", OracleType.Clob, rParams[7]);
                            xPara[9] = base.AddParam("QUESTION_EXAMPLE", OracleType.Clob, rParams[8]);
                            xPara[10] = base.AddParam("QUESTION_DESC", OracleType.Clob, rParams[9]);
                            xPara[11] = base.AddParam("INS_ID", OracleType.VarChar, rParams[11]);
                            xPara[12] = base.AddParam("USE_FLG", OracleType.VarChar, "Y");
                            xPara[13] = base.AddParam("TEMP_SAVE_FLG", OracleType.VarChar, rParams[12]);

                            xPara[14] = base.AddParam("COURSE_LIST", OracleType.VarChar, rParams[13]);
                            xPara[15] = base.AddParam("SUBJECT_LIST", OracleType.VarChar, rParams[14]);

                        }
                        else
                        {

                            // 2. DB 저장 (DB 작업 중에 문제발생 시, 저장된 파일 제거)
                            xSql = @" INSERT INTO T_ASSESS_QUESTION 
                                    ( QUESTION_ID
                                    , QUESTION_KIND
                                    , QUESTION_LANG
                                    , QUESTION_TYPE
                                    , COURSE_GROUP
                                    , COURSE_FIELD
                                    , QUESTION_SCORE
                                    , QUESTION_ANSWER
                                    , QUESTION_CONTENT
                                    , QUESTION_EXAMPLE
                                    , QUESTION_DESC
                                    , INS_ID
                                    , INS_DT
                                    , UPT_ID
                                    , UPT_DT
                                    , SEND_DT
                                    , SEND_FLG
                                    , USE_FLG
                                    , TEMP_SAVE_FLG
                                    , COURSE_LIST
                                    , SUBJECT_LIST
                                    ) VALUES (
                                    :QUESTION_ID
                                    , :QUESTION_KIND
                                    , :QUESTION_LANG
                                    , :QUESTION_TYPE
                                    , :COURSE_GROUP
                                    , :COURSE_FIELD
                                    , :QUESTION_SCORE
                                    , :QUESTION_ANSWER
                                    , :QUESTION_CONTENT
                                    , :QUESTION_EXAMPLE
                                    , :QUESTION_DESC
                                    , :INS_ID
                                    , SYSDATE
                                    , :INS_ID
                                    , SYSDATE
                                    , :SEND_DT
                                    , :SEND_FLG
                                    , :USE_FLG
                                    , :TEMP_SAVE_FLG
                                    , :COURSE_LIST
                                    , :SUBJECT_LIST
                                    ) ";

                            vp_l_common_md com = new vp_l_common_md();
                            xQID = com.GetMaxIDOfTable(new string[] { "T_ASSESS_QUESTION", "QUESTION_ID" });

                            xPara = new OracleParameter[18];
                            xPara[0] = base.AddParam("QUESTION_ID", OracleType.VarChar, xQID);
                            xPara[1] = base.AddParam("QUESTION_KIND", OracleType.VarChar, rParams[1]);
                            xPara[2] = base.AddParam("QUESTION_LANG", OracleType.VarChar, rParams[2]);
                            xPara[3] = base.AddParam("QUESTION_TYPE", OracleType.VarChar, rParams[3]);
                            xPara[4] = base.AddParam("COURSE_GROUP", OracleType.VarChar, rParams[4]);
                            xPara[5] = base.AddParam("COURSE_FIELD", OracleType.VarChar, rParams[5]);
                            xPara[6] = base.AddParam("QUESTION_SCORE", OracleType.Number, rParams[10]);
                            xPara[7] = base.AddParam("QUESTION_ANSWER", OracleType.Clob, rParams[6]);
                            xPara[8] = base.AddParam("QUESTION_CONTENT", OracleType.Clob, rParams[7]);
                            xPara[9] = base.AddParam("QUESTION_EXAMPLE", OracleType.Clob, rParams[8]);
                            xPara[10] = base.AddParam("QUESTION_DESC", OracleType.Clob, rParams[9]);
                            xPara[11] = base.AddParam("INS_ID", OracleType.VarChar, rParams[11]);
                            //xPara[8] = base.AddParam("INS_DT", OracleType.VarChar, rParams[7]);
                            xPara[12] = base.AddParam("SEND_DT", OracleType.DateTime, DBNull.Value);
                            xPara[13] = base.AddParam("SEND_FLG", OracleType.VarChar, "1");
                            xPara[14] = base.AddParam("USE_FLG", OracleType.VarChar, "Y");
                            xPara[15] = base.AddParam("TEMP_SAVE_FLG", OracleType.VarChar, rParams[12]);

                            xPara[16] = base.AddParam("COURSE_LIST", OracleType.VarChar, rParams[13]);
                            xPara[17] = base.AddParam("SUBJECT_LIST", OracleType.VarChar, rParams[14]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);

                            xRtn = xQID;
                        }

                        //선박발송 하는 PACKAGE 호출 부분 추가 필요 
                        if (rParams[12].ToString() == "N")
                        {
                            OracleParameter[] oOraParams = null;
                            oOraParams = new OracleParameter[2];
                            oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_ASSESS_QUESTION");
                            oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_ASSESS_QUESTION");
                            base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);
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



    }
}
