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
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling; 

namespace CLT.WEB.BIZ.LMS.MYPAGE
{
    public class vp_p_study_md:DAC
    {
        /************************************************************
      * Function name : GetStudyTreeList
      * Purpose       : 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetStudyTreeList(string[] rParams)
        public DataTable GetStudyTreeList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;
                
                xSql = @" 
                        SELECT OC.OPEN_COURSE_ID, S.SUBJECT_NM, S.SUBJECT_ID
                            , C.CONTENTS_NM, C.CONTENTS_ID, C.CONTENTS_FILE_NM
                            , CR.LAST_SUBJECT_ID, CR.LAST_CONTENTS_ID, CS.SUBJECT_SEQ, SC.CONTENTS_SEQ
                            /*마지막으로 수강한 컨텐츠의 SEQ 정보*/
                            , (SELECT SUBJECT_SEQ FROM T_COURSE_SUBJECT WHERE COURSE_ID = OC.COURSE_ID AND SUBJECT_ID = CR.LAST_SUBJECT_ID) AS LAST_SUBJECT_SEQ
                            , (SELECT CONTENTS_SEQ FROM T_SUBJECT_CONTENTS WHERE SUBJECT_ID = CR.LAST_SUBJECT_ID AND CONTENTS_ID = CR.LAST_CONTENTS_ID) AS LAST_CONTENTS_SEQ
                            , S.SUBJECT_ID || '|' || C.CONTENTS_ID || '|' || C.CONTENTS_FILE_NM AS VINFO_T
                            , S.SUBJECT_ID || '|' || C.CONTENTS_ID || '|' || C.CONTENTS_FILE_NM || '|' || ROW_NUMBER() OVER(ORDER BY CS.SUBJECT_SEQ, SC.CONTENTS_SEQ) AS VINFO
                            , (SELECT COUNT(*) FROM T_SUBJECT_CONTENTS WHERE SUBJECT_ID = SC.SUBJECT_ID) AS TOTAL
                            , ROW_NUMBER() OVER(ORDER BY CS.SUBJECT_SEQ, SC.CONTENTS_SEQ) AS ROW_SEQ
                            , OC.COURSE_ID
                            , (SELECT COURSE_NM FROM T_COURSE WHERE COURSE_ID = OC.COURSE_ID) AS COURSE_NM 
                        FROM T_COURSE_RESULT CR, T_OPEN_COURSE OC
                          , T_COURSE_SUBJECT CS, T_SUBJECT_CONTENTS SC
                          , T_SUBJECT S, T_CONTENTS C
                        WHERE CR.OPEN_COURSE_ID = OC.OPEN_COURSE_ID
                          AND OC.COURSE_ID = CS.COURSE_ID
                          AND CS.SUBJECT_ID = SC.SUBJECT_ID 
                          AND SC.SUBJECT_ID = S.SUBJECT_ID
                          AND SC.CONTENTS_ID = C.CONTENTS_ID 
                          AND OC.OPEN_COURSE_ID = '{0}'
                          AND CR.USER_ID = '{1}'                          
                        ORDER BY CS.SUBJECT_SEQ, SC.CONTENTS_SEQ
                ";

                xSql = string.Format(xSql, rParams[0], rParams[1]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
      * Function name : GetStudyTreeTextList
      * Purpose       : 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetStudyTreeTextList(string[] rParams)
        public DataTable GetStudyTreeTextList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                xSql = @" 
                        SELECT OC.OPEN_COURSE_ID
                            , T.TEXTBOOK_NM
                            --, DECODE(T.TEXTBOOK_FILE_NM, NULL, T.TEXTBOOK_NM, T.TEXTBOOK_NM || ' (' || T.TEXTBOOK_FILE_NM || ')') AS TEXTBOOK_NM 
                            , T.TEXTBOOK_ID
                            , T.TEXTBOOK_FILE_NM
                            , OC.COURSE_ID 
                        FROM T_COURSE_RESULT CR, T_OPEN_COURSE OC, T_COURSE_TEXTBOOK CT, T_TEXTBOOK T 
                        WHERE CR.OPEN_COURSE_ID = OC.OPEN_COURSE_ID
                          AND OC.COURSE_ID = CT.COURSE_ID
                          AND CT.TEXTBOOK_ID = T.TEXTBOOK_ID
                          AND OC.OPEN_COURSE_ID = '{0}'
                          AND CR.USER_ID = '{1}'                          
                        ORDER BY T.TEXTBOOK_ID, CT.TEXTBOOK_SEQ 
                ";

                xSql = string.Format(xSql, rParams[0], rParams[1]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
        * Function name : GetStudyTreeTextFile
        * Purpose       : 첨부파일조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public object GetStudyTreeTextFile(string rParams)
        {
            try
            {
                string xSql = string.Empty;
                xSql = "SELECT TEXTBOOK_FILE FROM T_TEXTBOOK WHERE TEXTBOOK_ID = '" + rParams + "' "; 
                object obj = base.ExecuteScalar("LMS", xSql);

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /************************************************************
      * Function name : SetStudyUpdate
      * Purpose       : 
      * Input         : 
      * Output        : string
      *************************************************************/
        #region public string SetStudyUpdate(object[] rParams)
        public string SetStudyUpdate(object[] rParams)
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


                        
                        //전체 contents 수 정보 
                        xSql = @" 
                                SELECT COUNT(*) AS TOTALCNT
                                FROM T_COURSE_RESULT CR, T_OPEN_COURSE OC
                                  , T_COURSE_SUBJECT CS, T_SUBJECT_CONTENTS SC
                                  , T_SUBJECT S, T_CONTENTS C
                                WHERE CR.OPEN_COURSE_ID = OC.OPEN_COURSE_ID
                                  AND OC.COURSE_ID = CS.COURSE_ID
                                  AND CS.SUBJECT_ID = SC.SUBJECT_ID 
                                  AND SC.SUBJECT_ID = S.SUBJECT_ID
                                  AND SC.CONTENTS_ID = C.CONTENTS_ID 
                                  AND CR.USER_ID = '{0}'
                                  AND OC.OPEN_COURSE_ID = '{1}' ";
                        xSql = string.Format(xSql, rParams[0], rParams[1]);
                        xCmdLMS.CommandText = xSql;
                        object xTotalCnt = base.ExecuteScalar(db, xCmdLMS, xTransLMS);

                        //최초 
                        if (rParams[4].ToString() == "1")
                        {
                            xSql = @" UPDATE T_COURSE_RESULT SET
                                             USER_COURSE_BEGIN_DT = SYSDATE 
                                            WHERE USER_ID = :USER_ID
                                                AND OPEN_COURSE_ID = :OPEN_COURSE_ID ";

                            xPara = new OracleParameter[2];
                            xPara[0] = base.AddParam("USER_ID", OracleType.VarChar, rParams[0]);
                            xPara[1] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, rParams[1]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);
                        }

                        //마지막 컨텐츠 
                        if (rParams[4].ToString() == xTotalCnt.ToString())
                        {
                            xSql = @" UPDATE T_COURSE_RESULT SET
                                             USER_COURSE_END_DT = SYSDATE 
                                            WHERE USER_ID = :USER_ID
                                                AND OPEN_COURSE_ID = :OPEN_COURSE_ID ";

                            xPara = new OracleParameter[2];
                            xPara[0] = base.AddParam("USER_ID", OracleType.VarChar, rParams[0]);
                            xPara[1] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, rParams[1]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);
                        }


                        //마지막 들은 contents 정보 seq 보다 현재 들으려고 하는 seq가 클 경우 last 정보 update 
                        //사용자가 들은 마지막 컨텐츠 정보 
                        xSql = @" SELECT LAST_SUBJECT_ID || '|' || LAST_CONTENTS_ID 
                                      FROM T_COURSE_RESULT 
                                        WHERE USER_ID = '{0}' 
                                            AND OPEN_COURSE_ID = '{1}' ";

                        xSql = string.Format(xSql, rParams[0], rParams[1]);
                        xCmdLMS.CommandText = xSql;
                        object xLast = base.ExecuteScalar(db, xCmdLMS, xTransLMS);

                        if (xLast != null && xLast.ToString().Replace("|", "") != string.Empty)
                        {
                            xSql = @" 
                                        SELECT OC.OPEN_COURSE_ID, S.SUBJECT_NM, S.SUBJECT_ID
                                            , C.CONTENTS_NM, C.CONTENTS_ID, C.CONTENTS_FILE_NM
                                            , CR.LAST_SUBJECT_ID, CR.LAST_CONTENTS_ID, SC.CONTENTS_SEQ
                                            /*마지막으로 수강한 컨텐츠의 SEQ 정보*/
                                            , (SELECT CONTENTS_SEQ FROM T_SUBJECT_CONTENTS WHERE SUBJECT_ID = CR.LAST_SUBJECT_ID AND CONTENTS_ID = CR.LAST_CONTENTS_ID) AS LAST_CONTENTS_SEQ
                                            , S.SUBJECT_ID || '|' || C.CONTENTS_ID || '|' || C.CONTENTS_FILE_NM AS VINFO_T
                                            , S.SUBJECT_ID || '|' || C.CONTENTS_ID || '|' || C.CONTENTS_FILE_NM || '|' || ROW_NUMBER() OVER(ORDER BY CS.SUBJECT_SEQ, SC.CONTENTS_SEQ) AS VINFO
                                            , (SELECT COUNT(*) FROM T_SUBJECT_CONTENTS WHERE SUBJECT_ID = SC.SUBJECT_ID) AS TOTAL
                                            , ROW_NUMBER() OVER(ORDER BY CS.SUBJECT_SEQ, SC.CONTENTS_SEQ) AS ROW_SEQ
                                        FROM T_COURSE_RESULT CR, T_OPEN_COURSE OC
                                          , T_COURSE_SUBJECT CS, T_SUBJECT_CONTENTS SC
                                          , T_SUBJECT S, T_CONTENTS C
                                        WHERE CR.OPEN_COURSE_ID = OC.OPEN_COURSE_ID
                                          AND OC.COURSE_ID = CS.COURSE_ID
                                          AND CS.SUBJECT_ID = SC.SUBJECT_ID 
                                          AND SC.SUBJECT_ID = S.SUBJECT_ID
                                          AND SC.CONTENTS_ID = C.CONTENTS_ID 
                                          AND OC.OPEN_COURSE_ID = '{0}'
                                          AND CR.USER_ID = '{1}'                          
                                        ORDER BY CS.SUBJECT_SEQ, SC.CONTENTS_SEQ
                                ";

                            xSql = string.Format(xSql, rParams[1], rParams[0]);
                            xCmdLMS.CommandText = xSql;
                            DataSet xds = base.ExecuteDataSet(db, xCmdLMS, xTransLMS);
                            string[] xLastKey = xLast.ToString().Split('|');
                            if (xds.Tables[0].Rows.Count > 0)
                            {
                                DataRow[] xdrarr = xds.Tables[0].Select("SUBJECT_ID = '" + xLastKey[0] + "' AND CONTENTS_ID = '" + xLastKey[1] + "' ");
                                DataRow xdr = null;
                                if (xdrarr.Length > 0)
                                {
                                    xdr = xdrarr[0];
                                    if (Convert.ToInt32(rParams[4]) > Convert.ToInt32(xdr["ROW_SEQ"].ToString()))
                                    {
                                        xTotalCnt = xTotalCnt == null || xTotalCnt.ToString() == string.Empty ? "1" : xTotalCnt.ToString();
                                        double xProgress = (Convert.ToDouble(rParams[4]) / Convert.ToDouble(xTotalCnt)) * 100;

                                        xSql = @" UPDATE T_COURSE_RESULT SET
                                             PROGRESS_RATE = :PROGRESS_RATE
                                                , LAST_SUBJECT_ID = :LAST_SUBJECT_ID
                                                , LAST_CONTENTS_ID = :LAST_CONTENTS_ID 
                                            WHERE USER_ID = :USER_ID
                                                AND OPEN_COURSE_ID = :OPEN_COURSE_ID ";

                                        xPara = new OracleParameter[5];
                                        xPara[0] = base.AddParam("USER_ID", OracleType.VarChar, rParams[0]);
                                        xPara[1] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, rParams[1]);
                                        xPara[2] = base.AddParam("PROGRESS_RATE", OracleType.VarChar, Convert.ToString(Math.Round(xProgress)));
                                        xPara[3] = base.AddParam("LAST_SUBJECT_ID", OracleType.VarChar, rParams[2]);
                                        xPara[4] = base.AddParam("LAST_CONTENTS_ID", OracleType.VarChar, rParams[3]);

                                        xCmdLMS.CommandText = xSql;
                                        base.Execute(db, xCmdLMS, xPara, xTransLMS);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //최초이면 
                            xTotalCnt = xTotalCnt == null || xTotalCnt.ToString() == string.Empty ? "1" : xTotalCnt.ToString();
                            double xProgress = (Convert.ToDouble(rParams[4]) / Convert.ToDouble(xTotalCnt)) * 100;

                            xSql = @" UPDATE T_COURSE_RESULT SET
                                             PROGRESS_RATE = :PROGRESS_RATE
                                                , LAST_SUBJECT_ID = :LAST_SUBJECT_ID
                                                , LAST_CONTENTS_ID = :LAST_CONTENTS_ID 
                                            WHERE USER_ID = :USER_ID
                                                AND OPEN_COURSE_ID = :OPEN_COURSE_ID ";

                            xPara = new OracleParameter[5];
                            xPara[0] = base.AddParam("USER_ID", OracleType.VarChar, rParams[0]);
                            xPara[1] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, rParams[1]);
                            xPara[2] = base.AddParam("PROGRESS_RATE", OracleType.VarChar, Convert.ToString(Math.Round(xProgress)));
                            xPara[3] = base.AddParam("LAST_SUBJECT_ID", OracleType.VarChar, rParams[2]);
                            xPara[4] = base.AddParam("LAST_CONTENTS_ID", OracleType.VarChar, rParams[3]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);
                        }



                    
                        xRtn = "Y";
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
      * Function name : GetExamProgress
      * Purpose       : 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetExamProgress(string[] rParams)
        public DataTable GetExamProgress(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                xSql = @" 
                        SELECT PROGRESS_RATE, PASS_FLG 
                        FROM T_COURSE_RESULT 
                        WHERE OPEN_COURSE_ID = '{0}'
                          AND USER_ID = '{1}' 
                ";

                xSql = string.Format(xSql, rParams[0], rParams[1]);

                return base.ExecuteDataTable("LMS", xSql);
                //object xRate = base.ExecuteScalar("LMS", xSql);
                //xRate = xRate == null ? string.Empty : xRate.ToString();
                //return xRate.ToString(); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
      * Function name : GetExamQuestion
      * Purpose       : 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetExamQuestion(string[] rParams)
        public DataTable GetExamQuestion_Random(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                xSql = @" 
                    SELECT * 
                    FROM (
                        SELECT CR.USER_ID, CR.OPEN_COURSE_ID, CR.COURSE_RESULT_SEQ
                                , CAQ.COURSE_ID, CAQ.QUESTION_ID, CAQ.QUESTION_SEQ
                                , AQ.QUESTION_SCORE, AQ.QUESTION_TYPE, AQ.QUESTION_ANSWER, AQ.QUESTION_CONTENT, AQ.QUESTION_EXAMPLE
                            FROM T_COURSE_RESULT CR, T_OPEN_COURSE OC, T_COURSE_ASSESS_QUESTION CAQ, T_ASSESS_QUESTION AQ
                            WHERE CR.OPEN_COURSE_ID = OC.OPEN_COURSE_ID 
                              AND OC.COURSE_ID = CAQ.COURSE_ID 
                              AND CAQ.QUESTION_ID = AQ.QUESTION_ID
                              AND CR.OPEN_COURSE_ID = '{0}'  
                              AND CR.USER_ID = '{1}'    
                         ORDER BY dbms_random.value  
                    ) 
                    WHERE ROWNUM <= 20 /*문제 20개만 가져오기*/
                ";

                xSql = string.Format(xSql, rParams[0], rParams[1]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
      * Function name : GetExamQuestion_RandomUse
      * Purpose       : 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetExamQuestion_RandomUse(string[] rParams)
        public DataTable GetExamQuestion_RandomUse(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                xSql = @" 
                        SELECT CR.USER_ID, CR.OPEN_COURSE_ID, CR.COURSE_RESULT_SEQ
                                , CAQ.COURSE_ID, CAQ.QUESTION_ID, CAQ.QUESTION_SEQ
                                , AQ.QUESTION_SCORE, AQ.QUESTION_TYPE, AQ.QUESTION_ANSWER, AQ.QUESTION_CONTENT, AQ.QUESTION_EXAMPLE
                            FROM T_COURSE_RESULT CR, T_OPEN_COURSE OC, T_COURSE_ASSESS_QUESTION CAQ, T_ASSESS_QUESTION AQ
                            WHERE CR.OPEN_COURSE_ID = OC.OPEN_COURSE_ID 
                              AND OC.COURSE_ID = CAQ.COURSE_ID 
                              AND CAQ.QUESTION_ID = AQ.QUESTION_ID
                              AND CR.OPEN_COURSE_ID = '{0}'  
                              AND CR.USER_ID = '{1}'    
                              AND (CAQ.COURSE_ID, CAQ.QUESTION_ID) IN ({2})
                ";

                xSql = string.Format(xSql, rParams[0], rParams[1], rParams[2]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
      * Function name : GetExamQuestion
      * Purpose       : 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        #region public DataTable GetExamQuestion(string[] rParams)
        public DataTable GetExamQuestion(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                xSql = @" 
                        SELECT 
                            A.USER_ID, A.OPEN_COURSE_ID, A.COURSE_RESULT_SEQ
                            , A.COURSE_ID, A.QUESTION_ID
                            , A.QUESTION_SCORE, A.QUESTION_TYPE, A.QUESTION_ANSWER, A.QUESTION_CONTENT, A.QUESTION_EXAMPLE
                            , AR.USER_ANSWER, AR.ISRIGHT_FLG
                            , AR.QUESTION_SEQ
                        FROM 
                            (SELECT CR.USER_ID, CR.OPEN_COURSE_ID, CR.COURSE_RESULT_SEQ
                                , CAQ.COURSE_ID, CAQ.QUESTION_ID, CAQ.QUESTION_SEQ
                                , AQ.QUESTION_SCORE, AQ.QUESTION_TYPE, AQ.QUESTION_ANSWER, AQ.QUESTION_CONTENT, AQ.QUESTION_EXAMPLE
                            FROM T_COURSE_RESULT CR, T_OPEN_COURSE OC, T_COURSE_ASSESS_QUESTION CAQ, T_ASSESS_QUESTION AQ
                            WHERE CR.OPEN_COURSE_ID = OC.OPEN_COURSE_ID 
                              AND OC.COURSE_ID = CAQ.COURSE_ID 
                              AND CAQ.QUESTION_ID = AQ.QUESTION_ID
                              AND CR.OPEN_COURSE_ID = '{0}'  
                              AND CR.USER_ID = '{1}'                               
                            ) A, T_ASSESS_RESULT AR
                        WHERE A.USER_ID = AR.USER_ID
                          AND A.OPEN_COURSE_ID = AR.OPEN_COURSE_ID
                          AND A.COURSE_RESULT_SEQ = AR.COURSE_RESULT_SEQ
                          AND A.COURSE_ID = AR.COURSE_ID
                          AND A.QUESTION_ID = AR.QUESTION_ID
                        ORDER BY AR.QUESTION_SEQ  
                ";

                xSql = string.Format(xSql, rParams[0], rParams[1]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 

        /************************************************************
       * Function name : SetExam
       * Purpose       : 
       * Input         : 
       * Output        : string
       *************************************************************/
        #region public string SetExam(string rParam)
        public string SetExam(string rParam)
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

                        ArrayList xarr = STR2MD2ArrayList(rParam);
                        string[] xParam; 

                        for (int i = 0; i < xarr.Count; i++)
                        {
                            //xarr.Add(new string[] { dr["USER_ID"].ToString()
                            //        , dr["OPEN_COURSE_ID"].ToString()
                            //        , dr["COURSE_RESULT_SEQ"].ToString()
                            //        , dr["COURSE_ID"].ToString()
                            //        , dr["QUESTION_ID"].ToString() 
                            //        , xAnswer
                            //    }); 
                            xParam = (string[])xarr[i];
                            
                            //--USER_ID
                            //--OPEN_COURSE_ID
                            //--COURSE_RESULT_SEQ
                            //--COURSE_ID
                            //--QUESTION_ID
                            xSql = " SELECT OPEN_COURSE_ID FROM T_ASSESS_RESULT "; 
                            xSql += " WHERE USER_ID = '" + xParam[0] + "' "; 
                            xSql += "    AND OPEN_COURSE_ID = '" + xParam[1] + "' ";
                            xSql += "    AND COURSE_RESULT_SEQ = " + xParam[2] + " ";
                            xSql += "    AND COURSE_ID = '" + xParam[3] + "' ";
                            xSql += "    AND QUESTION_ID = '" + xParam[4] + "' "; 
                            xCmdLMS.CommandText = xSql;
                            object xobj = base.ExecuteScalar(db, xCmdLMS, xTransLMS);

                            if (xobj != null && xobj.ToString() != string.Empty)
                            {
                                //update 
                                xSql = @" UPDATE T_ASSESS_RESULT SET
                                            USER_ANSWER = :USER_ANSWER
                                            , UPT_ID = :USER_ID 
                                            , UPT_DT = SYSDATE 
                                            WHERE USER_ID = :USER_ID 
                                                AND OPEN_COURSE_ID = :OPEN_COURSE_ID 
                                                AND COURSE_RESULT_SEQ = :COURSE_RESULT_SEQ
                                                AND COURSE_ID = :COURSE_ID
                                                AND QUESTION_ID = :QUESTION_ID ";

                                xPara = new OracleParameter[6];
                                xPara[0] = base.AddParam("USER_ID", OracleType.VarChar, xParam[0]);
                                xPara[1] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, xParam[1]);
                                xPara[2] = base.AddParam("COURSE_RESULT_SEQ", OracleType.Number, xParam[2]);
                                xPara[3] = base.AddParam("COURSE_ID", OracleType.VarChar, xParam[3]);
                                xPara[4] = base.AddParam("QUESTION_ID", OracleType.VarChar, xParam[4]);
                                xPara[5] = base.AddParam("USER_ANSWER", OracleType.VarChar, xParam[5]);
                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS); 

                            }
                            else
                            {
                                //insert 
                                xSql = @" INSERT INTO T_ASSESS_RESULT 
                                            (USER_ID, OPEN_COURSE_ID, COURSE_RESULT_SEQ, COURSE_ID, QUESTION_ID
                                            , USER_ANSWER
                                            , QUESTION_SEQ
                                            , INS_ID, INS_DT, UPT_ID, UPT_DT)
                                            VALUES 
                                            (:USER_ID, :OPEN_COURSE_ID, :COURSE_RESULT_SEQ, :COURSE_ID, :QUESTION_ID
                                            , :USER_ANSWER
                                            , :QUESTION_SEQ
                                            , :USER_ID, SYSDATE, :USER_ID, SYSDATE) ";

                                xPara = new OracleParameter[7];
                                xPara[0] = base.AddParam("USER_ID", OracleType.VarChar, xParam[0]);
                                xPara[1] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, xParam[1]);
                                xPara[2] = base.AddParam("COURSE_RESULT_SEQ", OracleType.Number, xParam[2]);
                                xPara[3] = base.AddParam("COURSE_ID", OracleType.VarChar, xParam[3]);
                                xPara[4] = base.AddParam("QUESTION_ID", OracleType.VarChar, xParam[4]);
                                xPara[5] = base.AddParam("USER_ANSWER", OracleType.VarChar, xParam[5]);
                                xPara[6] = base.AddParam("QUESTION_SEQ", OracleType.Number, i+1);
                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS); 
                            }
                        }

                        xRtn = "Y";
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
       * Function name : SetExamSubmit
       * Purpose       : 
       * Input         : 
       * Output        : string
       *************************************************************/
        #region public string SetExamSubmit(string rParam)
        public string SetExamSubmit(string rParam)
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

                        ArrayList xarr = STR2MD2ArrayList(rParam);
                        string[] xParam;
                        string xUser = string.Empty;
                        string xOpenId = string.Empty;
                        string xSeq = string.Empty; 

                        for (int i = 0; i < xarr.Count; i++)
                        {
                            //xarr.Add(new string[] { dr["USER_ID"].ToString()
                            //        , dr["OPEN_COURSE_ID"].ToString()
                            //        , dr["COURSE_RESULT_SEQ"].ToString()
                            //        , dr["COURSE_ID"].ToString()
                            //        , dr["QUESTION_ID"].ToString() 
                            //        , xAnswer
                            //    }); 
                            xParam = (string[])xarr[i];

                            if (xUser == string.Empty)
                            {
                                xUser = xParam[0];
                                xOpenId = xParam[1];
                                xSeq = xParam[2];
                            }

                            //--USER_ID
                            //--OPEN_COURSE_ID
                            //--COURSE_RESULT_SEQ
                            //--COURSE_ID
                            //--QUESTION_ID
                            xSql = " SELECT OPEN_COURSE_ID FROM T_ASSESS_RESULT ";
                            xSql += " WHERE USER_ID = '" + xParam[0] + "' ";
                            xSql += "    AND OPEN_COURSE_ID = '" + xParam[1] + "' ";
                            xSql += "    AND COURSE_RESULT_SEQ = " + xParam[2] + " ";
                            xSql += "    AND COURSE_ID = '" + xParam[3] + "' ";
                            xSql += "    AND QUESTION_ID = '" + xParam[4] + "' ";
                            xCmdLMS.CommandText = xSql;
                            object xobj = base.ExecuteScalar(db, xCmdLMS, xTransLMS);                            

                            if (xobj != null && xobj.ToString() != string.Empty)
                            {
                                //update 
                                xSql = @" UPDATE T_ASSESS_RESULT SET
                                            USER_ANSWER = :USER_ANSWER
                                            , UPT_ID = :USER_ID 
                                            , UPT_DT = SYSDATE 
                                            , ISRIGHT_FLG = :ISRIGHT_FLG
                                            WHERE USER_ID = :USER_ID 
                                                AND OPEN_COURSE_ID = :OPEN_COURSE_ID 
                                                AND COURSE_RESULT_SEQ = :COURSE_RESULT_SEQ
                                                AND COURSE_ID = :COURSE_ID
                                                AND QUESTION_ID = :QUESTION_ID ";

                                xPara = new OracleParameter[7];
                                xPara[0] = base.AddParam("USER_ID", OracleType.VarChar, xParam[0]);
                                xPara[1] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, xParam[1]);
                                xPara[2] = base.AddParam("COURSE_RESULT_SEQ", OracleType.Number, xParam[2]);
                                xPara[3] = base.AddParam("COURSE_ID", OracleType.VarChar, xParam[3]);
                                xPara[4] = base.AddParam("QUESTION_ID", OracleType.VarChar, xParam[4]);
                                xPara[5] = base.AddParam("USER_ANSWER", OracleType.VarChar, xParam[5]);
                                xPara[6] = base.AddParam("ISRIGHT_FLG", OracleType.VarChar, xParam[6]);
                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS);

                            }
                            else
                            {
                                //insert 
                                xSql = @" INSERT INTO T_ASSESS_RESULT 
                                            (USER_ID, OPEN_COURSE_ID, COURSE_RESULT_SEQ, COURSE_ID, QUESTION_ID
                                            , USER_ANSWER
                                            , INS_ID, INS_DT, UPT_ID, UPT_DT,ISRIGHT_FLG )
                                            VALUES 
                                            (:USER_ID, :OPEN_COURSE_ID, :COURSE_RESULT_SEQ, :COURSE_ID, :QUESTION_ID
                                            , :USER_ANSWER
                                            , :USER_ID, SYSDATE, :USER_ID, SYSDATE, :ISRIGHT_FLG) ";

                                xPara = new OracleParameter[7];
                                xPara[0] = base.AddParam("USER_ID", OracleType.VarChar, xParam[0]);
                                xPara[1] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, xParam[1]);
                                xPara[2] = base.AddParam("COURSE_RESULT_SEQ", OracleType.Number, xParam[2]);
                                xPara[3] = base.AddParam("COURSE_ID", OracleType.VarChar, xParam[3]);
                                xPara[4] = base.AddParam("QUESTION_ID", OracleType.VarChar, xParam[4]);
                                xPara[5] = base.AddParam("USER_ANSWER", OracleType.VarChar, xParam[5]);
                                xPara[6] = base.AddParam("ISRIGHT_FLG", OracleType.VarChar, xParam[6]);
                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS);
                            }
                        }


                        //T_COURSE_RESULT.ASSESS_SCORE에 점수 반영 
                        xSql = @"                              
                        SELECT NVL(SUM(AQ.QUESTION_SCORE), 0) AS TOTSUM
                        FROM T_ASSESS_QUESTION AQ, T_ASSESS_RESULT AR
                        WHERE AQ.QUESTION_ID = AR.QUESTION_ID 
                          AND AR.USER_ID = '{0}'
                          AND AR.OPEN_COURSE_ID = '{1}' 
                          AND AR.COURSE_RESULT_SEQ = {2}
                          AND AR.ISRIGHT_FLG = 'Y' 
                        ";
                        xSql = string.Format(xSql, xUser, xOpenId, xSeq); 
                        xCmdLMS.CommandText = xSql;
                        object xTotal = base.ExecuteScalar(db, xCmdLMS, xTransLMS);

                        xSql = @" UPDATE T_COURSE_RESULT SET 
                                          ASSESS_SCORE = :ASSESS_SCORE
                                        WHERE USER_ID = :USER_ID
                                           AND OPEN_COURSE_ID = :OPEN_COURSE_ID
                                           AND COURSE_RESULT_SEQ = :COURSE_RESULT_SEQ
                                    ";

                        xPara = new OracleParameter[4];
                        xPara[0] = base.AddParam("USER_ID", OracleType.VarChar, xUser);
                        xPara[1] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, xOpenId);
                        xPara[2] = base.AddParam("COURSE_RESULT_SEQ", OracleType.Number, xSeq);
                        xPara[3] = base.AddParam("ASSESS_SCORE", OracleType.VarChar, xTotal.ToString());
                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xPara, xTransLMS);

                        /*
                         -. 총점(T_COURSE_RESULT.TOTAL_SCORE): 개설과정의 이수기준 PROGRESS RATE(T_OPEN_COURSE.STD_PROGRESS_RATE), FINAL TEST(T_OPEN_COURSE.STD_FINAL_EXAM) 항목의 각 %를 나의 강의실에 진도, 시험에 반영 하여 
                          총점에 반영 하고, 개설과정의 수료점수(T_OPEN_COURSE.PASS_SCORE)에 총점이 만족되면 PASS_FLG를 수료로 UPDATE 
                          만족되지 못하면 PASS_FLG를 미수료로 UPDATE 

                        Ex) 예를들어 
                          개설과정: Progress Rate=80%, Final Test=20%, 수료점수60점 
                          나의강의실: 진도=80%, 시험=70점 => 개설과정 이수기준에 맞추면 진도=64점, 시험=14점 => 총점 78점 
                          개설과정의 수료점수 60점에 만족 하므로 Pass~
                         */

                        xSql = @"
                            SELECT CR.PROGRESS_RATE, CR.ASSESS_SCORE, CR.TOTAL_SCORE, CR.PASS_FLG 
                                , OC.STD_PROGRESS_RATE, OC.STD_FINAL_EXAM, OC.PASS_SCORE
                                , TO_NUMBER(CR.PROGRESS_RATE) * (OC.STD_PROGRESS_RATE/100) AS PRO
                                , TO_NUMBER(CR.ASSESS_SCORE) * (OC.STD_FINAL_EXAM/100) AS EXAM
                                , TO_NUMBER(CR.PROGRESS_RATE) * (OC.STD_PROGRESS_RATE/100) + TO_NUMBER(CR.ASSESS_SCORE) * (OC.STD_FINAL_EXAM/100) AS TOT
                                , CASE WHEN TO_NUMBER(CR.PROGRESS_RATE) * (OC.STD_PROGRESS_RATE/100) + TO_NUMBER(CR.ASSESS_SCORE) * (OC.STD_FINAL_EXAM/100) >= OC.PASS_SCORE
                                  THEN '000001'
                                  ELSE '000005' END PASS
                            FROM T_COURSE_RESULT CR, T_OPEN_COURSE OC 
                            WHERE CR.OPEN_COURSE_ID = OC.OPEN_COURSE_ID
                              AND CR.USER_ID = '{0}'
                              AND CR.OPEN_COURSE_ID = '{1}'
                              AND CR.COURSE_RESULT_SEQ = {2}
                        ";
                        xSql = string.Format(xSql, xUser, xOpenId, xSeq);
                        xCmdLMS.CommandText = xSql;
                        DataSet xdsResult = base.ExecuteDataSet(db, xCmdLMS, xTransLMS);
                        if (xdsResult.Tables[0].Rows.Count > 0)
                        {
                            DataRow drResult = xdsResult.Tables[0].Rows[0];

                            xSql = @" UPDATE T_COURSE_RESULT SET 
                                              TOTAL_SCORE = :TOTAL_SCORE
                                              , PASS_FLG  = :PASS_FLG
                                        WHERE USER_ID = :USER_ID
                                           AND OPEN_COURSE_ID = :OPEN_COURSE_ID
                                           AND COURSE_RESULT_SEQ = :COURSE_RESULT_SEQ
                                    ";

                            xPara = new OracleParameter[5];
                            xPara[0] = base.AddParam("USER_ID", OracleType.VarChar, xUser);
                            xPara[1] = base.AddParam("OPEN_COURSE_ID", OracleType.VarChar, xOpenId);
                            xPara[2] = base.AddParam("COURSE_RESULT_SEQ", OracleType.Number, xSeq);
                            xPara[3] = base.AddParam("TOTAL_SCORE", OracleType.VarChar, drResult["TOT"].ToString());
                            xPara[4] = base.AddParam("PASS_FLG", OracleType.VarChar, drResult["PASS"].ToString());
                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);
                        }

                        xRtn = "Y";
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
       * Function name : GetExamCheck
       * Purpose       : 시험 전 설문조사 완료유무 체크
       * Input         : 
       * Output        : string
       *************************************************************/
        #region public DataTable GetExamCheck(string[] rParams)
        public DataTable GetExamCheck(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT target.res_no, target.user_id, target.answer_yn ";
                xSql += " FROM t_research_target target ";
                xSql += string.Format(" WHERE target.res_no = (SELECT res_no FROM t_research res WHERE res.open_course_id = '{0}') ", rParams[0]); // 개설과정 ID
                xSql += string.Format("   AND target.user_id = '{0}' ", rParams[1]); // 사용자 ID

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
