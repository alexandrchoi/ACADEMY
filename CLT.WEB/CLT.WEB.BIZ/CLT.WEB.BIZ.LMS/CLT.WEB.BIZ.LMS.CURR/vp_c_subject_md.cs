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
    /// 1. 작업개요 : vp_c_subject_md Class
    /// 
    /// 2. 주요기능 : 과목 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_c_subject_md
    /// 
    /// 4. 작 업 자 : 임양춘 / 2012.01.16
    /// </summary>
    public class vp_c_subject_md : DAC
    {
        /************************************************************
       * Function name : GetSubjectList
       * Purpose       : 과목 목록 조회 
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
                        FROM T_SUBJECT S
                        WHERE (TEMP_SAVE_FLG IS NULL OR TEMP_SAVE_FLG = 'N'  )
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
       * Function name : GetSubjectInfo
       * Purpose       : 과목 상세조회 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        #region public DataTable GetSubjectInfo(string[] rParams)
        public DataTable GetSubjectInfo(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;

                xSql = " SELECT * FROM T_SUBJECT ";
                xSql += string.Format("        WHERE SUBJECT_ID ='{0}' ", rParams[0]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /************************************************************
      * Function name : SetSubjectInfo
      * Purpose       : 과목 등록하는 처리
      * Input         : 
      * Output        : string
      *************************************************************/
        #region public string SetSubjectInfo(object[] rParams)
        public string SetSubjectInfo(object[] rParams)
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

                        if (rParams[10].ToString() == "Y")
                        {
                            //subject id를 확인 하여 temp_save_flg = 'Y' 이면 update
                            //아니면 insert 하기                             
                            xSql = " SELECT TEMP_SAVE_FLG FROM T_SUBJECT WHERE SUBJECT_ID = '" + rParams[0] + "' ";
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

                            xSql = @"
                            UPDATE T_SUBJECT 
                            SET 
                                SUBJECT_NM           = :SUBJECT_NM           
                                , SUBJECT_KIND         = :SUBJECT_KIND       
                                , SUBJECT_TYPE         = :SUBJECT_TYPE       
                                , SUBJECT_LANG         = :SUBJECT_LANG       
                                , LEARNING_TIME        = :LEARNING_TIME      
                                , LECTURER_NM          = :LECTURER_NM      
                                , LECTURER1_NM          = :LECTURER1_NM        
                                , LEARNING_DESC        = :LEARNING_DESC      
                                , LEARNING_OBJECTIVE   = :LEARNING_OBJECTIVE 
                                , USE_FLG              = :USE_FLG                                    
                                , UPT_ID = :UPT_ID
                                , UPT_DT = SYSDATE
                            WHERE SUBJECT_ID = :SUBJECT_ID
                            ";

                            xPara = new OracleParameter[12];
                            xPara[0] = base.AddParam("SUBJECT_ID", OracleType.VarChar, xQID);
                            xPara[1] = base.AddParam("SUBJECT_NM", OracleType.VarChar, rParams[1]);
                            xPara[2] = base.AddParam("SUBJECT_KIND", OracleType.VarChar, rParams[2]);
                            xPara[3] = base.AddParam("SUBJECT_TYPE", OracleType.VarChar, rParams[3]);
                            xPara[4] = base.AddParam("SUBJECT_LANG", OracleType.VarChar, rParams[4]);
                            xPara[5] = base.AddParam("LEARNING_TIME", OracleType.Number, rParams[5]);
                            xPara[6] = base.AddParam("LECTURER_NM", OracleType.VarChar, rParams[6]);
                            xPara[7] = base.AddParam("LECTURER1_NM", OracleType.VarChar, rParams[12]);
                            xPara[8] = base.AddParam("LEARNING_DESC", OracleType.VarChar, rParams[7]);
                            xPara[9] = base.AddParam("LEARNING_OBJECTIVE", OracleType.VarChar, rParams[8]);
                            xPara[10] = base.AddParam("USE_FLG", OracleType.VarChar, rParams[9]);
                            xPara[11] = base.AddParam("UPT_ID", OracleType.VarChar, rParams[11]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);

                        }
                        else
                        {
                            // 2. DB 저장 (DB 작업 중에 문제발생 시, 저장된 파일 제거)
                            xSql = @" INSERT INTO T_SUBJECT 
                                    ( 
                                        SUBJECT_ID
                                        ,SUBJECT_NM
                                        ,SUBJECT_KIND
                                        ,SUBJECT_TYPE
                                        ,SUBJECT_LANG
                                        ,LEARNING_TIME
                                        ,LECTURER_NM
                                        ,LECTURER1_NM
                                        ,LEARNING_DESC
                                        ,LEARNING_OBJECTIVE
                                        ,INS_ID
                                        ,INS_DT
                                        , UPT_ID
                                        , UPT_DT
                                        ,SEND_DT
                                        ,SEND_FLG
                                        ,USE_FLG
                                        ,TEMP_SAVE_FLG
                                    ) VALUES (
                                        :SUBJECT_ID   
                                        ,:SUBJECT_NM   
                                        ,:SUBJECT_KIND 
                                        ,:SUBJECT_TYPE 
                                        ,:SUBJECT_LANG 
                                        ,:LEARNING_TIME
                                        ,:LECTURER_NM
                                        ,:LECTURER1_NM  
                                        ,:LEARNING_DESC
                                        ,:LEARNING_OBJECTIVE
                                        ,:INS_ID       
                                        ,SYSDATE
                                        , :INS_ID
                                        , SYSDATE
                                        ,:SEND_DT      
                                        ,:SEND_FLG     
                                        ,:USE_FLG      
                                        ,:TEMP_SAVE_FLG
                                    ) ";

                            vp_l_common_md com = new vp_l_common_md();
                            xQID = com.GetMaxIDOfTable(new string[] { "T_SUBJECT", "SUBJECT_ID" });

                            xPara = new OracleParameter[15];
                            xPara[0] = base.AddParam("SUBJECT_ID", OracleType.VarChar, xQID);
                            xPara[1] = base.AddParam("SUBJECT_NM", OracleType.VarChar, rParams[1]);
                            xPara[2] = base.AddParam("SUBJECT_KIND", OracleType.VarChar, rParams[2]);
                            xPara[3] = base.AddParam("SUBJECT_TYPE", OracleType.VarChar, rParams[3]);
                            xPara[4] = base.AddParam("SUBJECT_LANG", OracleType.VarChar, rParams[4]);
                            xPara[5] = base.AddParam("LEARNING_TIME", OracleType.Number, rParams[5]);
                            xPara[6] = base.AddParam("LECTURER_NM", OracleType.VarChar, rParams[6]);
                            xPara[7] = base.AddParam("LECTURER1_NM", OracleType.VarChar, rParams[12]);
                            xPara[8] = base.AddParam("LEARNING_DESC", OracleType.VarChar, rParams[7]);
                            xPara[9] = base.AddParam("LEARNING_OBJECTIVE", OracleType.VarChar, rParams[8]);
                            xPara[10] = base.AddParam("INS_ID", OracleType.VarChar, rParams[11]);
                            xPara[11] = base.AddParam("SEND_DT", OracleType.DateTime, DBNull.Value);
                            xPara[12] = base.AddParam("SEND_FLG", OracleType.VarChar, "1");
                            xPara[13] = base.AddParam("USE_FLG", OracleType.VarChar, rParams[9]);
                            xPara[14] = base.AddParam("TEMP_SAVE_FLG", OracleType.VarChar, rParams[10]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);

                            xRtn = xQID;
                        }
                        //선박발송 하는 PACKAGE 호출 부분 추가 필요 
                        if (rParams[10].ToString() == "N")
                        {
                            OracleParameter[] oOraParams = null;
                            oOraParams = new OracleParameter[2];
                            oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_SUBJECT");
                            oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_SUBJECT");
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
        #endregion


    }
}
