using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

// 필수 using 문


using System.Data;
using System.Data.OracleClient;
using CLT.WEB.BIZ.FX.BIZBASE;
using Microsoft.Practices.EnterpriseLibrary.Data;
using CLT.WEB.UI.FX.UTIL;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.BIZ.LMS.APPR
{
    /// <summary>
    /// 1. 작업개요 : vp_c_course_md Class
    /// 
    /// 2. 주요기능 : 과정(등록,개설) 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_c_course_md
    /// 
    /// 4. 작 업 자 : 김양도 / 2011.12.08
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_l_common_md
    ///        * Comment 
    ///          * Comment 
    ///          Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제따른 변경
    ///          
    ///   [CHM-201221637] LMS 직무기술역량 평가 기능 수정 요청
    ///      * 김은정 2012.11.30
    ///      * Source
    ///        : vp_a_appraisal_md.GetApprList, SetApprResultDetail
    ///      * Comment 
    ///        : 역량평가 결과 조회 시 평가자 사번 추가, 역량평가 상세결과 저장 

    /// </summary>
    public class vp_a_appraisal_md : DAC
    {
        /************************************************************
        * Function name : GetCourseInfo
        * Purpose       : 등록과정 전체 목록을 전달하는 처리
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetApprCode(string[] rParams, CultureInfo rArgCultureInfo)
        {
            DataTable xDt = null;
            try
            {
                string xSql = " SELECT * FROM ";
                xSql += "           ( SELECT rownum rnum , b.* FROM ";
                xSql += "               ( ";
                xSql += "                   SELECT grade, score ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", grade_nm ";
                    xSql += ", grade_desc ";
                }
                else
                {
                    xSql += ", grade_nm_eng as grade_nm ";
                    xSql += ", grade_desc_eng as grade_desc ";
                }
                xSql += "                    , (SELECT count(*) FROM t_appraisal_code) totalrecordcount ";
                xSql += "                       FROM t_appraisal_code ";
                xSql += "                       WHERE 0=0 ";
                xSql += "                       AND (del_flg is null OR del_flg = 'N') ";
                xSql += "                       ORDER BY score DESC ";
                xSql += "               ) b ";
                xSql += "           ) ";

                if (rParams[2] != "all")
                {
                    xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                    xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
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
        * Function name : GetCourseInfo
        * Purpose       : 등록과정 전체 목록을 전달하는 처리
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetApprCode_Excel(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = @" SELECT GRADE
                                       , GRADE_NM
                                       , GRADE_NM_ENG
                                       , SCORE
                                       , GRADE ||'|'|| SCORE AS GS
                                       , GRADE_DESC
                                       , GRADE_DESC_ENG
                                       , SEND_DT
                                       , SEND_FLG
                                       , INS_ID
                                       , INS_DT
                                       , UPT_ID
                                       , UPT_DT
                                       , DEL_FLG ";
                xSql += "       FROM t_appraisal_code ";
                xSql += "       ORDER BY score DESC  ";

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
      * Function name : GetAssessInfo
      * Purpose       : 평가문제 상세조회 
      * Input         : string[] rParams (0: pagesize, 1: pageno)
      * Output        : DataTable
      *************************************************************/
        public DataTable GetApprCodeInfo(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;

                xSql = " SELECT * FROM t_appraisal_code ";
                xSql += string.Format("        WHERE grade ='{0}' ", rParams[0]);

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
        * Function name : SetCodeMaster
        * Purpose       : 공통코드(Master) 정보 등록
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetApprCodeInsert(string[] rParams)
        {
            string xSql = string.Empty;
            string xReturn = string.Empty;
            string xRtn = Boolean.FalseString;

            try
            {
                Database db = null;

                db = base.GetDataBase("LMS");
                using (OracleConnection cnn = (OracleConnection)db.CreateConnection())
                {
                    cnn.Open();
                    OracleTransaction trx = cnn.BeginTransaction();
                    OracleCommand cmd = null;

                    try
                    {
                        cmd = base.GetSqlCommand(db);

                        xSql = " SELECT grade FROM t_appraisal_code WHERE grade='" + rParams[0] + "' ";
                        cmd.CommandText = xSql;
                        object xExitGrade = base.ExecuteScalar(db, cmd, trx);

                        if (Util.IsNullOrEmptyObject(xExitGrade))
                        {
                            xSql = "INSERT INTO t_appraisal_code (grade, " + " grade_nm, " + " score, " + " grade_desc, " +
                                                                        " ins_id, " + " ins_dt, " + " upt_id, " + " upt_dt) " +
                                          " VALUES( ";
                            xSql += string.Format(" '{0}', ", rParams[0]);  // 등급
                            xSql += string.Format(" '{0}', ", rParams[1]);  // 평가내용
                            xSql += string.Format(" '{0}', ", rParams[2]);  // 점수
                            xSql += string.Format(" '{0}', ", rParams[3]);  // 설명                    
                            xSql += string.Format(" '{0}', ", rParams[4]);  // 등록자


                            xSql += " SYSDATE, "; // 등록일자
                            xSql += string.Format(" '{0}', ", rParams[4]);  // 수정자


                            xSql += " SYSDATE) "; // 수정일자

                            cmd.CommandText = xSql;
                            base.Execute(db, cmd, trx);

                            xRtn = Boolean.TrueString;
                        }
                        else
                        {
                            //데이터 중복
                            xRtn = "DLC";
                        }

                        trx.Commit(); // 트랜잭션 커밋
                    }
                    catch (Exception ex)
                    {
                        trx.Rollback(); // Exception 발생시 롤백처리

                        bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                        if (rethrow) throw;
                    }
                    finally
                    {
                        if (cmd != null)
                            cmd.Dispose();

                        if (trx != null)
                            trx.Dispose();
                    }
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
        * Function name : SetCodeMaster
        * Purpose       : 공통코드(Master) 정보 등록
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetApprCodeUpdate(string[] rParams)
        {
            string xRtn = Boolean.FalseString;

            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTransLMS = null;
            OracleCommand xCmdLMS = null;

            try
            {
                xTransLMS = xCnnLMS.BeginTransaction(); // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db);
                xCmdLMS.Connection = xCnnLMS;           // Command 에 DB 연결
                xCmdLMS.Transaction = xTransLMS;        // Command 에 트랜잭션 연결

                try
                {
                    string xSql = " UPDATE t_appraisal_code SET ";
                    xSql += string.Format(" grade_nm = '{0}', ", rParams[1]);
                    xSql += string.Format(" score = '{0}', ", rParams[2]);
                    xSql += string.Format(" grade_desc = '{0}', ", rParams[3]);
                    xSql += string.Format(" upt_id = '{0}', ", rParams[4]);
                    xSql += " upt_dt = SYSDATE ";
                    xSql += string.Format(" WHERE grade = '{0}' ", rParams[0]);

                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);

                    xRtn = Boolean.TrueString;

                    xTransLMS.Commit(); // 트랜잭션 커밋
                }
                catch (Exception ex)
                {
                    xTransLMS.Rollback(); // Exception 발생시 롤백처리

                    bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                    if (rethrow) throw;
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
        * Function name : SetShipSchoolResultList
        * Purpose       : 등록과정 전체 목록을 전달하는 처리
        * Input         : DataTable
        * Output        : DataTable
        *************************************************************/
        public string SetApprCodeDelete(DataTable rDt)
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
                                UPDATE t_appraisal_code SET                     
                                del_flg = 'Y'
                                WHERE grade = '" + drDet["grade"] + "' ";

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

        /************************************************************
        * Function name : GetCourseInfo
        * Purpose       : 등록과정 전체 목록을 전달하는 처리
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataSet GetApprItem(string[] rParams, CultureInfo rArgCultureInfo)
        {
            Database db = null;
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");

                string xWhere = " AND (i.del_flg is null OR i.del_flg = 'N') ";
                DataTable xDt = null;

                xSql = @"
                    SELECT count(*) FROM t_appraisal_item i WHERE 0=0 " + "\r\n" + xWhere;

                xDt = base.ExecuteDataTable(db, xSql);
                base.MergeTable(ref xDs, xDt, "table1");

                xSql = "";
                xSql += @"   
                     select *
                       from (
                        select rownum rnum ";
                xSql += "      , a.* " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += @"     SELECT 
                                      i.app_item_no, to_char(i.app_base_dt, 'yyyy.MM.dd') as app_base_dt, ";
                xSql += "             (SELECT ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += " d_knm ";
                }
                else
                {
                    xSql += " d_enm ";
                }
                xSql += "             FROM t_code_detail WHERE m_cd = '0052' AND d_cd = i.step_gu) as step_gu, ";
                //xSql += "             (SELECT d_knm FROM V_COMMON_VSL_TYPE_P WHERE m_cd = '0028' AND d_cd = i.vsl_type) as vsl_type, ";
                xSql += "             (select type_p_short_desc from V_COMMON_VSL_TYPE_P where use_flg='Y' and type_p_cd=i.vsl_type) as vsl_type, ";
                xSql += "             (select type_c_short_desc from V_COMMON_VSL_TYPE_C where use_flg='Y' and type_c_cd=i.type_c_cd) as type_c_type_nm, ";
                xSql += "             (SELECT ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += " d_knm ";
                }
                else
                {
                    xSql += " d_enm ";
                }
                xSql += "            FROM t_appraisal_target WHERE d_cd = i.app_duty_step) as app_duty_step,  i.app_item_seq, ";
                xSql += "             (SELECT d_knm FROM t_code_detail WHERE m_cd = '0017' AND d_cd = i.app_lang) as app_lang, ";
                xSql += "             app_case_seq, (select course_nm from t_course where course_id = i.course_ojt) as course_ojt, (select course_nm from t_course where course_id = i.course_lms) as course_lms, i.course_etc ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", i.app_item_nm ";
                    xSql += ", i.app_item_desc ";
                    xSql += ", i.app_case_desc ";
                }
                else
                {
                    xSql += ", app_item_nm_eng as app_item_nm ";
                    xSql += ", app_item_desc_eng as app_item_desc ";
                    xSql += ", app_case_desc_eng as app_case_desc ";
                }
                xSql += @"       FROM t_appraisal_item i ";
                xSql += "       WHERE 0=0 ";
                xSql += "             " + xWhere;
                xSql += @"   ORDER BY i.app_base_dt desc, i.app_lang, i.step_gu, i.vsl_type, to_number(i.app_item_seq), to_number(i.app_case_seq) " + "\r\n";
                xSql += @"      ) a ";
                xSql += @"  ) a ";

                if (rParams[2] != "all")
                {
                    if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                    {
                        xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                        xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                    }
                }
                //xSql += @"   ORDER BY a.step_gu, a.app_item_seq, to_number(a.app_case_seq) " + "\r\n";

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
        * Function name : GetCourseInfo
        * Purpose       : 등록과정 전체 목록을 전달하는 처리
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetApprItemExcel(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = " SELECT *  ";
                xSql += "       FROM t_appraisal_item ";
                xSql += "       ORDER BY app_item_seq, app_case_seq  ";

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
        * Function name : SetApprItemDelete
        * Purpose       : 평가 항목 삭제
        * Input         : DataTable
        * Output        : DataTable
        *************************************************************/
        public string SetApprItemDelete(DataTable rDt)
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
                                UPDATE t_appraisal_item SET                     
                                del_flg = 'Y'
                                WHERE app_item_no = '" + drDet["app_item_no"] + "' ";

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

        /************************************************************
        * Function name : GetApprItemInfo
        * Purpose       : 평가 항목 상세조회 
        * Input         : string[] rParams
        * Output        : DataTable
        *************************************************************/
        public DataTable GetApprItemInfo(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;

                xSql += @"
                         SELECT I.app_item_no
                                , to_char(I.app_base_dt, 'yyyy.MM.dd') as app_base_dt
                                , I.step_gu
                                , I.app_duty_step
                                , I.app_lang
                                , I.vsl_type
                                , I.app_item_seq
                                , I.app_item_nm
                                , I.app_item_nm_eng
                                , I.app_item_desc
                                , I.app_item_desc_eng
                                , I.app_case_seq
                                , I.app_case_desc
                                , I.app_case_desc_eng
                                , I.course_ojt
                                , C.COURSE_NM as course_ojt_nm
                                , I.course_lms
                                , C1.COURSE_NM as course_lms_nm
                                , I.course_etc 
                                , I.TYPE_C_CD
                           FROM t_appraisal_item I
                                , T_COURSE C
                                , T_COURSE C1
                          WHERE I.COURSE_OJT = C.COURSE_ID(+)
                                AND I.COURSE_LMS = C1.COURSE_ID(+)
                        ";
                xSql += string.Format(" and I.app_item_no ='{0}' ", rParams[0]);

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
        * Function name : SetApprItemInsert
        * Purpose       : 평가 항목 등록
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetApprItemInsert(object[] rParams)
        {
            Database db = null;
            string xRtn = Boolean.FalseString;
            string xSql = string.Empty;
            string xItemNo = "";

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
                            xSql = " SELECT max(app_item_no) FROM t_appraisal_item WHERE substr(app_item_no, 1, 6) = to_char(sysdate, 'yyMMdd') ";

                            DataTable xDt = base.ExecuteDataTable("LMS", xSql);
                            xItemNo = xDt.Rows[0][0].ToString();

                            if (string.IsNullOrEmpty(xItemNo))
                            {
                                xItemNo = GetMaxItemNo(db, xCmdLMS, xTransLMS);
                            }
                            else
                            {
                                xItemNo = string.Format(xItemNo.Substring(0, 6) + "{0:000#}", Convert.ToInt32(xItemNo.Substring(6)) + 1);
                            }

                            xSql = @" 
                                INSERT INTO t_appraisal_item 
                                ( 
                                    app_item_no
                                    , app_base_dt
                                    , step_gu
                                    , vsl_type
                                    , app_duty_step
                                    , app_item_nm
                                    , app_item_nm_eng
                                    , app_item_seq
                                    , app_item_desc
                                    , app_item_desc_eng
                                    , app_lang
                                    , app_case_seq
                                    , app_case_desc
                                    , app_case_desc_eng
                                    , course_ojt
                                    , course_lms
                                    , course_etc                                    
                                    , ins_id
                                    , ins_dt
                                    , send_flg
                                    , type_c_cd
                                ) VALUES (
                                    :app_item_no
                                    , :app_base_dt
                                    , :step_gu
                                    , :vsl_type
                                    , :app_duty_step
                                    , :app_item_nm
                                    , :app_item_nm_eng
                                    , :app_item_seq
                                    , :app_item_desc
                                    , :app_item_desc_eng
                                    , :app_lang
                                    , :app_case_seq
                                    , :app_case_desc
                                    , :app_case_desc_eng
                                    , :course_ojt
                                    , :course_lms
                                    , :course_etc
                                    , :ins_id
                                    , sysdate
                                    , '1'
                                    , :type_c_cd
                                ) 
                            ";

                            xPara = new OracleParameter[19];
                            xPara[0] = base.AddParam("app_item_no", OracleType.VarChar, xItemNo);
                            xPara[1] = base.AddParam("app_base_dt", OracleType.DateTime, rParams[1]);
                            xPara[2] = base.AddParam("step_gu", OracleType.VarChar, rParams[2]);
                            xPara[3] = base.AddParam("vsl_type", OracleType.VarChar, rParams[5]);
                            xPara[4] = base.AddParam("app_duty_step", OracleType.VarChar, rParams[3]);
                            xPara[5] = base.AddParam("app_item_nm", OracleType.VarChar, rParams[7]);
                            xPara[6] = base.AddParam("app_item_nm_eng", OracleType.VarChar, rParams[8]);
                            xPara[7] = base.AddParam("app_item_seq", OracleType.VarChar, rParams[6]);
                            xPara[8] = base.AddParam("app_item_desc", OracleType.VarChar, rParams[9]);
                            xPara[9] = base.AddParam("app_item_desc_eng", OracleType.VarChar, rParams[10]);
                            xPara[10] = base.AddParam("app_lang", OracleType.VarChar, rParams[4]);
                            xPara[11] = base.AddParam("app_case_seq", OracleType.VarChar, rParams[11]);
                            xPara[12] = base.AddParam("app_case_desc", OracleType.VarChar, rParams[12]);
                            xPara[13] = base.AddParam("app_case_desc_eng", OracleType.VarChar, rParams[13]);
                            xPara[14] = base.AddParam("course_ojt", OracleType.VarChar, rParams[14]);
                            xPara[15] = base.AddParam("course_lms", OracleType.VarChar, rParams[15]);
                            xPara[16] = base.AddParam("course_etc", OracleType.VarChar, rParams[16]);
                            xPara[17] = base.AddParam("ins_id", OracleType.VarChar, rParams[17]);
                            xPara[18] = base.AddParam("type_c_cd", OracleType.VarChar, rParams[18]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);
                        }
                        else
                        {
                            xSql = @" UPDATE t_appraisal_item
                                         SET app_base_dt = :app_base_dt
                                           , step_gu = :step_gu
                                           , vsl_type = :vsl_type
                                           , app_duty_step = :app_duty_step
                                           , app_item_nm = :app_item_nm
                                           , app_item_nm_eng = :app_item_nm_eng
                                           , app_item_seq = :app_item_seq
                                           , app_item_desc = :app_item_desc
                                           , app_item_desc_eng = :app_item_desc_eng
                                           , app_lang = :app_lang
                                           , app_case_seq = :app_case_seq
                                           , app_case_desc = :app_case_desc
                                           , app_case_desc_eng = :app_case_desc_eng
                                           , course_ojt = :course_ojt
                                           , course_lms = :course_lms
                                           , course_etc = :course_etc
                                           , upt_id = :upt_id
                                           , upt_dt = sysdate
                                           , send_flg = '1'
                                           , type_c_cd = :type_c_cd
                                     WHERE app_item_no = :app_item_no
                                ";

                            xPara = new OracleParameter[19];
                            xPara[0] = base.AddParam("app_item_no", OracleType.VarChar, rParams[0]);
                            xPara[1] = base.AddParam("app_base_dt", OracleType.DateTime, rParams[1]);
                            xPara[2] = base.AddParam("step_gu", OracleType.VarChar, rParams[2]);
                            xPara[3] = base.AddParam("vsl_type", OracleType.VarChar, rParams[5]);
                            xPara[4] = base.AddParam("app_duty_step", OracleType.VarChar, rParams[3]);
                            xPara[5] = base.AddParam("app_item_nm", OracleType.VarChar, rParams[7]);
                            xPara[6] = base.AddParam("app_item_nm_eng", OracleType.VarChar, rParams[8]);
                            xPara[7] = base.AddParam("app_item_seq", OracleType.VarChar, rParams[6]);
                            xPara[8] = base.AddParam("app_item_desc", OracleType.VarChar, rParams[9]);
                            xPara[9] = base.AddParam("app_item_desc_eng", OracleType.VarChar, rParams[10]);
                            xPara[10] = base.AddParam("app_lang", OracleType.VarChar, rParams[4]);
                            xPara[11] = base.AddParam("app_case_seq", OracleType.VarChar, rParams[11]);
                            xPara[12] = base.AddParam("app_case_desc", OracleType.VarChar, rParams[12]);
                            xPara[13] = base.AddParam("app_case_desc_eng", OracleType.VarChar, rParams[13]);
                            xPara[14] = base.AddParam("course_ojt", OracleType.VarChar, rParams[14]);
                            xPara[15] = base.AddParam("course_lms", OracleType.VarChar, rParams[15]);
                            xPara[16] = base.AddParam("course_etc", OracleType.VarChar, rParams[16]);
                            xPara[17] = base.AddParam("upt_id", OracleType.VarChar, rParams[17]);
                            xPara[18] = base.AddParam("type_c_cd", OracleType.VarChar, rParams[18]);

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
        private string GetMaxItemNo(Database db, OracleCommand rCmdLMS, OracleTransaction rTransLMS)
        {
            string xItemNo = "";
            string xNowDate = System.DateTime.Now.ToString("yyMMdd");
            string xSql = " SELECT max(app_item_no) FROM t_appraisal_item WHERE app_item_no like '" + xNowDate + "%'";
            rCmdLMS.CommandText = xSql;
            DataSet xDs = base.ExecuteDataSet(db, rCmdLMS, rTransLMS);

            xItemNo = xDs.Tables[0].Rows[0][0].ToString();
            if (xItemNo == "")
                xItemNo = System.DateTime.Now.ToString("yyMMdd") + "0000";
            else
            {
                xItemNo = string.Format(xItemNo.Substring(0, 6) + "{0:000#}", Convert.ToInt32(xItemNo.Substring(6)) + 1);
            }
            return xItemNo;
        }
        /************************************************************
        * Function name : SetApprItemInsert
        * Purpose       : 평가 항목 등록
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetApprItemInsert(DataTable rDt)
        {
            Database db = null;
            string xRtn = Boolean.FalseString;
            string xSql = string.Empty;
            string xItemNo = "";

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
                        for (int i = 0; i < rDt.Rows.Count; i++)
                        {
                            xSql = @" SELECT app_item_no
                                        FROM t_appraisal_item 
                                       WHERE APP_BASE_DT = TO_DATE('" + Convert.ToString(rDt.Rows[i]["APP_BASE_DT"]) + @"', 'YYYY.MM.DD')
                                             AND STEP_GU = '" + Convert.ToString(rDt.Rows[i]["STEP_GU"]) + @"' 
                                             AND VSL_TYPE = '" + Convert.ToString(rDt.Rows[i]["VSL_TYPEP"]) + @"' 
                                             AND APP_DUTY_STEP = '" + Convert.ToString(rDt.Rows[i]["APP_DUTY_STEP"]) + @"' 
                                             AND APP_ITEM_SEQ = '" + Convert.ToString(rDt.Rows[i]["app_item_seq"]) + @"' 
                                             AND APP_CASE_SEQ = '" + Convert.ToString(rDt.Rows[i]["app_case_seq"]) + @"' 
                                             AND TYPE_C_CD = '" + Convert.ToString(rDt.Rows[i]["VSL_TYPEC"]) + @"' "
                                   ;
                            xCmdLMS.CommandText = xSql;
                            DataSet xDs = base.ExecuteDataSet(db, xCmdLMS, xTransLMS);

                            if (xDs.Tables[0].Rows.Count > 0)
                            {
                                xSql = @" 
                                    UPDATE t_appraisal_item
                                       SET app_item_nm = :app_item_nm
                                            , app_item_nm_eng = :app_item_nm_eng
                                            , app_item_desc = :app_item_desc
                                            , app_item_desc_eng = :app_item_desc_eng
                                            , app_lang = :app_lang
                                            , app_case_desc = :app_case_desc
                                            , app_case_desc_eng = :app_case_desc_eng
                                            , course_ojt = :course_ojt
                                            , course_lms = :course_lms
                                            , course_etc = :course_etc        
                                            , send_flg = '1'                           
                                    WHERE APP_BASE_DT = :APP_BASE_DT
                                             AND STEP_GU = :STEP_GU
                                             AND VSL_TYPE = :VSL_TYPEP
                                             AND APP_DUTY_STEP = :APP_DUTY_STEP
                                             AND APP_ITEM_SEQ = :app_item_seq
                                             AND APP_CASE_SEQ = :app_case_seq
                                             AND TYPE_C_CD = :TYPE_C_CD
                                ";

                                xPara = new OracleParameter[17];
                                xPara[0] = base.AddParam("app_item_nm", OracleType.VarChar, rDt.Rows[i]["app_item_nm"]);
                                xPara[1] = base.AddParam("app_item_nm_eng", OracleType.VarChar, rDt.Rows[i]["app_item_nm_eng"]);
                                xPara[2] = base.AddParam("app_item_desc", OracleType.VarChar, rDt.Rows[i]["app_item_desc"]);
                                xPara[3] = base.AddParam("app_item_desc_eng", OracleType.VarChar, rDt.Rows[i]["app_item_desc_eng"]);
                                xPara[4] = base.AddParam("app_lang", OracleType.VarChar, rDt.Rows[i]["app_lang"]);
                                xPara[5] = base.AddParam("app_case_desc", OracleType.VarChar, rDt.Rows[i]["app_case_desc"]);
                                xPara[6] = base.AddParam("app_case_desc_eng", OracleType.VarChar, rDt.Rows[i]["app_case_desc_eng"]);
                                xPara[7] = base.AddParam("course_ojt", OracleType.VarChar, rDt.Rows[i]["course_ojt"]);
                                xPara[8] = base.AddParam("course_lms", OracleType.VarChar, rDt.Rows[i]["course_lms"]);
                                xPara[9] = base.AddParam("course_etc", OracleType.VarChar, rDt.Rows[i]["course_etc"]);
                                xPara[10] = base.AddParam("app_base_dt", OracleType.DateTime, rDt.Rows[i]["app_base_dt"]);
                                xPara[11] = base.AddParam("step_gu", OracleType.VarChar, rDt.Rows[i]["step_gu"]);
                                xPara[12] = base.AddParam("vsl_typep", OracleType.VarChar, rDt.Rows[i]["vsl_typep"]);
                                xPara[13] = base.AddParam("app_duty_step", OracleType.VarChar, rDt.Rows[i]["app_duty_step"]);
                                xPara[14] = base.AddParam("app_item_seq", OracleType.VarChar, rDt.Rows[i]["app_item_seq"]);
                                xPara[15] = base.AddParam("app_case_seq", OracleType.VarChar, rDt.Rows[i]["app_case_seq"]);
                                xPara[16] = base.AddParam("type_c_cd", OracleType.VarChar, rDt.Rows[i]["vsl_typec"]);

                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS);
                                xCmdLMS.Parameters.Clear();
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(xItemNo))
                                {
                                    xItemNo = GetMaxItemNo(db, xCmdLMS, xTransLMS);
                                }
                                else
                                {
                                    xItemNo = string.Format(xItemNo.Substring(0, 6) + "{0:000#}", Convert.ToInt32(xItemNo.Substring(6)) + 1);
                                }

                                xSql = @" 
                                    INSERT INTO t_appraisal_item 
                                    ( 
                                        app_item_no
                                        , app_base_dt
                                        , step_gu
                                        , vsl_type
                                        , app_duty_step
                                        , app_item_nm
                                        , app_item_nm_eng
                                        , app_item_seq
                                        , app_item_desc
                                        , app_item_desc_eng
                                        , app_lang
                                        , app_case_seq
                                        , app_case_desc
                                        , app_case_desc_eng
                                        , course_ojt
                                        , course_lms
                                        , course_etc                                    
                                        , ins_id
                                        , ins_dt
                                        , send_flg
                                        , type_c_cd
                                        ) VALUES (
                                        :app_item_no
                                        , :app_base_dt
                                        , :step_gu
                                        , :vsl_type
                                        , :app_duty_step
                                        , :app_item_nm
                                        , :app_item_nm_eng
                                        , :app_item_seq
                                        , :app_item_desc
                                        , :app_item_desc_eng
                                        , :app_lang
                                        , :app_case_seq
                                        , :app_case_desc
                                        , :app_case_desc_eng
                                        , :course_ojt
                                        , :course_lms
                                        , :course_etc
                                        , :ins_id
                                        , sysdate
                                        , '1'
                                        , :type_c_cd
                                    ) ";

                                xPara = new OracleParameter[19];
                                xPara[0] = base.AddParam("app_item_no", OracleType.VarChar, xItemNo);
                                xPara[1] = base.AddParam("app_base_dt", OracleType.DateTime, rDt.Rows[i]["app_base_dt"]);
                                xPara[2] = base.AddParam("step_gu", OracleType.VarChar, rDt.Rows[i]["step_gu"]);
                                xPara[3] = base.AddParam("vsl_type", OracleType.VarChar, rDt.Rows[i]["VSL_TYPEP"]);
                                xPara[4] = base.AddParam("app_duty_step", OracleType.VarChar, rDt.Rows[i]["app_duty_step"]);
                                xPara[5] = base.AddParam("app_item_nm", OracleType.VarChar, rDt.Rows[i]["app_item_nm"]);
                                xPara[6] = base.AddParam("app_item_nm_eng", OracleType.VarChar, rDt.Rows[i]["app_item_nm_eng"]);
                                xPara[7] = base.AddParam("app_item_seq", OracleType.VarChar, rDt.Rows[i]["app_item_seq"]);
                                xPara[8] = base.AddParam("app_item_desc", OracleType.VarChar, rDt.Rows[i]["app_item_desc"]);
                                xPara[9] = base.AddParam("app_item_desc_eng", OracleType.VarChar, rDt.Rows[i]["app_item_desc_eng"]);
                                xPara[10] = base.AddParam("app_lang", OracleType.VarChar, rDt.Rows[i]["app_lang"]);
                                xPara[11] = base.AddParam("app_case_seq", OracleType.VarChar, rDt.Rows[i]["app_case_seq"]);
                                xPara[12] = base.AddParam("app_case_desc", OracleType.VarChar, rDt.Rows[i]["app_case_desc"]);
                                xPara[13] = base.AddParam("app_case_desc_eng", OracleType.VarChar, rDt.Rows[i]["app_case_desc_eng"]);
                                xPara[14] = base.AddParam("course_ojt", OracleType.VarChar, rDt.Rows[i]["course_ojt"]);
                                xPara[15] = base.AddParam("course_lms", OracleType.VarChar, rDt.Rows[i]["course_lms"]);
                                xPara[16] = base.AddParam("course_etc", OracleType.VarChar, rDt.Rows[i]["course_etc"]);
                                xPara[17] = base.AddParam("ins_id", OracleType.VarChar, rDt.Rows[i]["ins_id"]);
                                xPara[18] = base.AddParam("type_c_cd", OracleType.VarChar, rDt.Rows[i]["VSL_TYPEC"]);

                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS);
                                xCmdLMS.Parameters.Clear();
                            }
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
        * Function name : GetApprList
        * Purpose       : 등록과정 전체 목록을 전달하는 처리
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataSet GetApprList(string[] rParams
                                    , string rGubun, CultureInfo rArgCultureInfo)
        {
            Database db = null;
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");

                string xWhere = " --and (app_complete = '0' or app_complete is null) " + "\r\n";
                string xWhere1 = "";
                DataTable xDt = null;

                // 평가일자
                xWhere += " AND r.app_dt >= to_date('" + rParams[2] + "', 'yyyy.mm.dd') " + "\r\n";
                xWhere += " AND r.app_dt <= to_date('" + rParams[3] + "', 'yyyy.mm.dd') " + "\r\n";

                // 현직/상위직

                if (rParams[4] != "*" && !String.IsNullOrEmpty(rParams[4]))
                    xWhere += @" AND r.step_gu = '" + rParams[4] + "' " + "\r\n";

                // 선종
                if (rParams[5] != "*" && !String.IsNullOrEmpty(rParams[5]))
                    xWhere += @" AND r.vsl_type = '" + rParams[5] + "' " + "\r\n";

                // 선박
                if (rParams[6] != "*" && !String.IsNullOrEmpty(rParams[6]))
                    xWhere += @" AND r.vsl_cd = '" + rParams[6] + "' " + "\r\n";

                // 성명
                if (!String.IsNullOrEmpty(rParams[7]))
                {
                    xWhere += @" AND (lower(u.user_nm_kor) like '%" + rParams[7].ToLower() + "%' ";
                    xWhere += @" OR lower(u.user_nm_eng_first || ' ' || u.user_nm_eng_last) like '%" + rParams[7].ToLower() + "%') " + "\r\n";

                }

                // 직급
                if (Convert.ToString(rParams[8]) != "*" && !String.IsNullOrEmpty(rParams[8]))
                    xWhere += @" AND r.user_duty_step = '" + rParams[8] + "' " + "\r\n";

                // 재평가대상

                if (rParams[9] != "*" && !String.IsNullOrEmpty(rParams[9]))
                {
                    if (rParams[9] == "Y")
                        xWhere += @" AND (app_complete = '0' or app_complete is null) " + "\r\n";
                    else if (rParams[9] == "N")
                        xWhere += @" AND (app_complete = '1') " + "\r\n";
                }

                // USER_ID : * 아니거나 사번이 존재할 경우
                if (Convert.ToString(rParams[10]) != "*" && !String.IsNullOrEmpty(rParams[10]))
                {
                    if (Convert.ToString(rParams[13]) == string.Empty || Convert.ToString(rParams[13]) == "N") //Admin 권한이 아닌 경우 User_ID 기준으로 검색

                        //  xWhere += @" AND r.user_id = '" + rParams[10] + "' " + "\r\n";

                        //김진일 수정 : 2014.10.23
                        //공통사번으로 조회되도록 수정 
                        xWhere += @" AND r.user_id IN (select sno from hperinfo where com_sno = (select com_sno from hperinfo where sno = '" + rParams[10] + "')) " + "\r\n";
                }

                // 선종
                if (rParams[12] != "*" && !String.IsNullOrEmpty(rParams[12]))
                    xWhere += @" AND r.type_c_cd = '" + rParams[12] + "' " + "\r\n";

                xSql = @"
                SELECT count(r.app_no)
                  FROM (
                         SELECT r.app_no        
                                , (SELECT count(*) from T_APPRAISAL_RESULT_DETAIL WHERE app_no = r.app_no AND (grade != 'S' and grade > 'B')) AS re_cnt
                           FROM T_APPRAISAL_RESULT r
                                , T_USER u
                                , T_APPRAISAL_TARGET t
                                , V_HDUTYSTEP s                 --평가자를 자져오기 위한것
                                , V_HDUTYSTEP S1                --사용자를 가져오기 위한것
                                , V_HDUTYWORK W                 --사용자를 가져오기 위한것
                                 " + "\r\n";
                xSql += @"                      
                          WHERE R.USER_ID = U.USER_ID
                                and r.app_duty_step = s.duty_step(+)
                                and u.duty_step = s1.duty_step
                                and u.duty_work = w.duty_work(+)
                            " + "\r\n";
                if (rParams[4] == "000001") //현직
                {
                    xSql += @" and u.duty_work = t.d_cd(+) " + "\r\n";
                }
                else if (rParams[4] == "000002") //상위직
                {
                    xSql += @" and u.duty_step = t.d_cd(+) " + "\r\n";
                }
                xSql += @"  " + xWhere;
                xSql += @" ) r
                 WHERE 1=1
                      ";
                xSql += @"  " + xWhere1;
                xDt = base.ExecuteDataTable(db, xSql);
                base.MergeTable(ref xDs, xDt, "table1");

                xSql = "";
                xSql += @"   
                             select *
                               from (
                                select rownum rnum ";
                xSql += "               , a.* " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += "           SELECT  " + "\r\n";
                xSql += @"                  r.app_no as app_keys
                                            , r.app_no
                                            , r.app_dt as app_dt --평가일자
                                            , r.vsl_cd --선박명
                                            , r.on_dt
                                            , r.off_dt
                                            , r.user_id
                                            , r.step_gu
                                            , r.user_duty_step
                                            , r.user_duty_work
                                            , r.duty_work_ename
                                            , r.vsl_type
                                            , R.TYPE_C_CD
                                            , r.user_nm_kor
                                            , r.step_name
                                            , r.tot_score
                                            , r.re_cnt
                                            , r.app_nm
                                            , r.app_duty_step 
                                            , r.app_duty_nm 
                                            , DECODE(R.APP_COMPLETE,'1','Y','N') APP_COMPLETE
                                            , r.app_sno as app_user_id " + "\r\n";
                xSql += @"            FROM  (
                                        SELECT r.app_no
                                                , to_char(r.app_dt,'yyyy.mm.dd') as app_dt --평가일자
                                                , r.vsl_cd --선박명
                                                , to_char(r.on_dt, 'yyyy.mm.dd') as on_dt
                                                , to_char(r.off_dt,'yyyy.mm.dd') as off_dt
                                                , r.user_id
                                                , r.step_gu
                                                , r.user_duty_step
                                                , r.user_duty_work 
                                                , r.app_sno 
                                                , (select ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += " d_knm ";
                }
                else
                {
                    xSql += " d_enm ";
                }
                xSql += @"                  from T_APPRAISAL_TARGET where d_cd = r.user_duty_work) duty_work_ename
                                                , r.vsl_type
                                                , R.TYPE_C_CD,R.APP_COMPLETE";

                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", u.user_nm_kor ";
                    xSql += ", s.step_name as step_name ";
                }
                else
                {
                    xSql += ", u.user_nm_eng_first || ' ' || u.user_nm_eng_last as user_nm_kor ";
                    xSql += ", s.step_ename as step_name ";
                }

                xSql += @"                  , ( select sum(c.score)
                                                      from t_appraisal_result_detail d
                                                            , t_appraisal_code c
                                                      where app_no = r.app_no
                                                            and d.grade = c.grade 
                                                            ) as tot_score
                                                , (SELECT count(*) from T_APPRAISAL_RESULT_DETAIL WHERE app_no = r.app_no AND (grade != 'S' and grade > 'B')) AS re_cnt 
                                                , (SELECT ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += " user_nm_kor ";
                }
                else
                {
                    xSql += " user_nm_eng_first || ' ' || user_nm_eng_last ";
                }
                xSql += @"                 from t_user a where a.user_id = r.app_sno)  as app_nm "; // 평가자이름 
                //xSql += @"                 , (SELECT ";
                //                               if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                //                               {
                //                                   xSql += " duty_work_name ";
                //                                   //xSql += " step_name ";
                //                               }
                //                               else
                //                               {
                //                                   xSql += " duty_work_ename ";
                //                                  // xSql += " step_ename as step_name ";
                //                               }
                //xSql += @"                 from V_HDUTYWORK a where a.duty_work = r.app_duty_step)  as app_duty_step "; // 평가자직급


                xSql += @"                  , r.app_duty_step "; //평가자직급코드 (국/영문 작업 필요 X)


                xSql += @"                 , (SELECT ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    //xSql += " duty_work_name ";
                    xSql += " step_name ";
                }
                else
                {
                    //xSql += " duty_work_ename ";
                    xSql += " step_ename as step_name ";
                }
                xSql += @"                 from V_HDUTYSTEP a where a.duty_step = r.app_duty_step)  as app_duty_nm "; // 평가자직급

                //  , s.step_name as app_duty_nm
                xSql += @"                 FROM T_APPRAISAL_RESULT r
                                                , T_USER u
                                                , T_APPRAISAL_TARGET t
                                                , V_HDUTYSTEP s                 --평가자를 자져오기 위한것
                                                , V_HDUTYSTEP S1                --사용자를 가져오기 위한것
                                                , V_HDUTYWORK W                 --사용자를 가져오기 위한것
                                                 ";

                xSql += @"                      
                                          WHERE R.USER_ID = U.USER_ID
                                                and r.app_duty_step = s.duty_step(+)
                                                and u.duty_step = s1.duty_step
                                                and u.duty_work = w.duty_work(+)
                        " + "\r\n";

                if (rParams[4] == "000001") //현직
                {
                    xSql += @"                  and u.duty_work = t.d_cd(+) " + "\r\n";
                }
                else if (rParams[4] == "000002") //상위직
                {
                    xSql += @"                  and u.duty_step = t.d_cd(+) " + "\r\n";
                }
                xSql += @"                      " + xWhere;

                if (rParams[4] == "000001") //현직
                {
                    xSql += @"            ORDER BY r.app_dt desc, w.work_seq, r.vsl_cd " + "\r\n";
                }
                else if (rParams[4] == "000002") //상위직
                {
                    xSql += @"            ORDER BY r.app_dt desc, s1.step_seq, r.vsl_cd  " + "\r\n";
                }

                xSql += @"                 ) r
                               WHERE 1=1 " + xWhere1;
                xSql += "          ) a " + "\r\n";
                xSql += "   ) a " + "\r\n";

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
       * Function name : GetApprLatestList
       * Purpose       : 등록과정 전체 목록을 전달하는 처리
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        public DataSet GetApprLatestList(string[] rParams
                                    , string rGubun)
        {
            Database db = null;
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");

                string xWhere = " and r.app_complete='1' ";
                DataTable xDt = null;

                // 현직/상위직

                if (rParams[2] != "*" && !String.IsNullOrEmpty(rParams[2]))
                    xWhere += @" AND r.step_gu = '" + rParams[2] + "' " + "\r\n";

                xSql = @"
                    SELECT count(r.app_no)
                      FROM T_APPRAISAL_RESULT r
                           , (SELECT * FROM T_USER) u
                           , T_APPRAISAL_TARGET d
                           , V_HDUTYSTEP s
                           , V_HDUTYSTEP S1                
                           , V_HDUTYWORK W 
                     WHERE 1=1
                           " + xWhere + @"
                           and r.user_duty_step = d.d_cd(+)
                           and r.app_duty_step = s.duty_step(+)
                           and r.USER_ID = u.USER_ID
                           and u.duty_step = s1.duty_step
                           and u.duty_work = w.duty_work 
                     ";
                xDt = base.ExecuteDataTable(db, xSql);
                base.MergeTable(ref xDs, xDt, "table1");

                xSql = "";
                xSql += @"   
                             select *
                               from (
                                select rownum rnum ";
                xSql += "               , a.* " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += @"     SELECT 
                                    r.app_no  as app_keys
                                    , r.app_no
                                    , to_char(r.app_dt,'yyyy.mm.dd') as app_dt --평가일자
                                    , r.vsl_cd --선박명
                                    , to_char(r.on_dt, 'yyyy.mm.dd') as on_dt
                                    , to_char(r.off_dt, 'yyyy.mm.dd') as off_dt
                                    , r.user_id
                                    , r.step_gu
                                    , r.user_duty_step
                                    , r.user_duty_work
                                    , (select d_knm from T_APPRAISAL_TARGET where d_cd = r.user_duty_work) duty_work_ename
                                    , r.vsl_type
                                    , u.user_nm_kor
                                    , d.d_knm as step_name
                                    , r.tot_score
                                    , (SELECT count(*) from T_APPRAISAL_RESULT_DETAIL WHERE app_no = r.app_no AND grade IN ('C', 'D')) AS re_cnt
                                    , r.app_nm
                                    , r.app_duty_step
                                    , s.step_name as app_duty_nm
                              FROM T_APPRAISAL_RESULT r
                                    , (SELECT * FROM T_USER) u
                                    , T_APPRAISAL_TARGET d
                                    , V_HDUTYSTEP s
                                    , V_HDUTYSTEP S1                
                                    , V_HDUTYWORK W 
                              WHERE 1=1
                                    " + xWhere + @"
                                    and r.user_duty_step = d.d_cd(+)
                                    and r.app_duty_step = s.duty_step(+)
                                    and r.USER_ID = u.USER_ID 
                                    and u.duty_step = s1.duty_step
                                    and u.duty_work = w.duty_work ";
                if (rParams[2] == "000001") //현직
                {
                    xSql += @" ORDER BY r.vsl_cd, w.work_seq, r.app_dt desc ";
                }
                else if (rParams[2] == "000002") //상위직
                {
                    xSql += @" ORDER BY r.vsl_cd, s1.step_seq, r.app_dt desc ";
                }
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
            return xDs;
        }

        /************************************************************
        * Function name : Get_Appr_Detail
        * Purpose       : 평가항목
        * Input         : 
        * Output        : DataTable
        *************************************************************/
        #region Get_Appr_Detail(string rAppNo )
        public DataTable Get_Appr_Detail(string[] rParams
                                           , string rGubun, CultureInfo rArgCultureInfo)
        {
            DataTable xDt = null;
            string xSql = "";
            string rInquiry = rParams[0];
            string rVesselType = rParams[1];
            string rShipCode = rParams[2];
            string rName = (String.IsNullOrEmpty(rParams[3]) ? "" : rParams[3]);
            string rDutyStep = rParams[4];
            string rAppDate = rParams[5];
            string rAppNo = rParams[6];
            string rUserId = rParams[7];
            string rVesselTypeC = rParams[8];

            if (rInquiry == "*")
                rInquiry = "";
            if (rVesselType == "*")
                rVesselType = "";
            if (rShipCode == "*")
                rShipCode = "";
            if (rDutyStep == "*")
                rDutyStep = "";

            string rWhereAppDate = "";
            if (!string.IsNullOrEmpty(rAppDate))
                rWhereAppDate = " r.app_dt =  to_date('" + rAppDate + "','yyyy.mm.dd') " + "\r\n";

            try
            {
                if (rGubun == "new")
                {
                    xSql = @"
                        SELECT '^' || i.app_item_no as app_keys
                                , i.app_item_no
                                , i.app_item_seq ";

                    if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                    {
                        xSql += ", i.app_item_nm ";
                        xSql += ", i.app_item_desc ";
                        xSql += ", i.app_case_desc ";
                    }
                    else
                    {
                        xSql += ", app_item_nm_eng as app_item_nm ";
                        xSql += ", app_item_desc_eng as app_item_desc ";
                        xSql += ", app_case_desc_eng as app_case_desc ";
                    }

                    xSql += @", i.app_case_seq 
                                , i.course_ojt
                                , i.course_lms
                                , i.course_etc
                                , i.vsl_type
                                , i.type_c_cd
                                , '' as grade
                                , '' as score
                                , '' as app_nm
                                , '' as app_duty_work
                                , '' as tot_score
                                , '' as remark
                                , '' as app_complete
                          FROM (
                                 SELECT  I.*
                                   FROM  (  
                                            SELECT  MAX(APP_BASE_DT) AS APP_BASE_DT
                                                    , STEP_GU
                                                    , VSL_TYPE
                                                    , TYPE_C_CD
                                                    , APP_DUTY_STEP
                                                    , APP_ITEM_SEQ
                                                    , APP_CASE_SEQ
                                              FROM  T_APPRAISAL_ITEM
                                             WHERE  app_base_dt <= to_date('" + rAppDate + @"','yyyy.mm.dd') 
                                                    AND step_gu =  '" + rInquiry + @"' 
                                                    AND vsl_type =  '" + rVesselType + @"' 
                                                    AND type_c_cd =  '" + rVesselTypeC + @"' 
                                                    AND app_duty_step =  '" + rDutyStep + @"'
                                          GROUP BY  STEP_GU
                                                    , VSL_TYPE
                                                    , TYPE_C_CD
                                                    , APP_DUTY_STEP
                                                    , APP_ITEM_SEQ
                                                    , APP_CASE_SEQ  
                                         ) A
                                         , T_APPRAISAL_ITEM I
                                   WHERE A.APP_BASE_DT = I.APP_BASE_DT
                                         AND A.STEP_GU = I.STEP_GU
                                         AND A.VSL_TYPE = I.VSL_TYPE
                                         AND A.TYPE_C_CD = I.TYPE_C_CD
                                         AND A.APP_DUTY_STEP = I.APP_DUTY_STEP
                                         AND A.APP_ITEM_SEQ = I.APP_ITEM_SEQ
                                         AND A.APP_CASE_SEQ = I.APP_CASE_SEQ
                                order by to_number(i.app_item_seq), to_number(i.app_case_seq) 
                                ) i ";
                }
                else
                {
                    //평가 완료여부 확인
                    xSql = @" SELECT app_complete FROM t_appraisal_result WHERE app_no =  '" + rAppNo + @"' ";
                    xDt = base.ExecuteDataTable("LMS", xSql);

                    //평가 완료 된경우

                    if (Convert.ToString(xDt.Rows[0]["app_complete"]) == "1")
                    {
                        xSql = @"
                         SELECT d.app_no || '^' || d.app_item_no as app_keys
                                , d.app_no 
                                , d.app_item_no
                                , i.app_item_seq
                                , d.grade ";
                        if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                        {
                            xSql += ", i.app_item_nm ";
                            xSql += ", i.app_item_desc ";
                            xSql += ", i.app_case_desc ";
                        }
                        else
                        {
                            xSql += ", app_item_nm_eng as app_item_nm ";
                            xSql += ", app_item_desc_eng as app_item_desc ";
                            xSql += ", app_case_desc_eng as app_case_desc ";
                        }
                        xSql += @"
                               
                                , i.app_case_seq 
                                , i.course_ojt
                                , i.course_lms
                                , i.course_etc
                                , r.user_id
                                , r.vsl_cd
                                , r.vsl_type
                                , r.type_c_cd
                                , r.app_nm
                                , r.user_duty_step
                                , r.tot_score
                                , to_char(r.app_dt,'yyyy.mm.dd') as app_dt
                                , d.remark
                                , r.app_complete
                           FROM t_appraisal_result r
                                , t_appraisal_result_detail d
                                , t_appraisal_item i
                          WHERE r.app_no = d.app_no
                                and d.app_item_no = i.app_item_no
                                and r.app_no = '" + rAppNo + @"' 
                        ";
                    }
                    else
                    {
                        //평가 완료 되지 않은경우
                        if (rInquiry == "000001")
                        {
                            xSql = @"
                         SELECT i.app_no || '^' || i.app_item_no as app_keys
                                , i.app_no 
                                , i.app_item_no
                                , i.app_item_seq
                                , d.grade ";
                            if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                            {
                                xSql += ", i.app_item_nm ";
                                xSql += ", i.app_item_desc ";
                                xSql += ", i.app_case_desc ";
                            }
                            else
                            {
                                xSql += ", app_item_nm_eng as app_item_nm ";
                                xSql += ", app_item_desc_eng as app_item_desc ";
                                xSql += ", app_case_desc_eng as app_case_desc ";
                            }
                            xSql += @" , i.app_case_seq 
                                , i.course_ojt
                                , i.course_lms
                                , i.course_etc
                                , i.user_id
                                , i.vsl_cd
                                , i.vsl_type
                                , i.type_c_cd
                                , i.app_nm
                                , i.app_duty_work
                                , i.tot_score
                                , to_char(i.app_dt,'yyyy.mm.dd') as app_dt
                                , d.remark
                                , i.app_complete
                           FROM (
                                    (   
                                        SELECT i.* 
                                                , r.app_no 
                                                , r.vsl_cd
                                                , r.user_id
                                                , r.app_nm
                                                , r.tot_score
                                                , r.app_duty_work
                                                , r.app_dt
                                                , r.app_complete
                                          FROM  (
                                                     SELECT  I.*
                                                       FROM  (  
                                                                SELECT  MAX(APP_BASE_DT) AS APP_BASE_DT
                                                                        , STEP_GU
                                                                        , VSL_TYPE
                                                                        , TYPE_C_CD
                                                                        , APP_DUTY_STEP
                                                                        , APP_ITEM_SEQ
                                                                        , APP_CASE_SEQ
                                                                  FROM  T_APPRAISAL_ITEM
                                                                 WHERE  app_base_dt <= to_date('" + rAppDate + @"','yyyy.mm.dd') 
                                                                        AND step_gu =  '" + rInquiry + @"' 
                                                                        AND vsl_type =  '" + rVesselType + @"' 
                                                                        AND type_c_cd =  '" + rVesselTypeC + @"' 
                                                                        AND app_duty_step =  '" + rDutyStep + @"'
                                                              GROUP BY  STEP_GU
                                                                        , VSL_TYPE
                                                                        , TYPE_C_CD
                                                                        , APP_DUTY_STEP
                                                                        , APP_ITEM_SEQ
                                                                        , APP_CASE_SEQ  
                                                                ) A
                                                                , T_APPRAISAL_ITEM I
                                                       WHERE A.APP_BASE_DT = I.APP_BASE_DT
                                                             AND A.STEP_GU = I.STEP_GU
                                                             AND A.VSL_TYPE = I.VSL_TYPE
                                                             AND A.TYPE_C_CD = I.TYPE_C_CD
                                                             AND A.APP_DUTY_STEP = I.APP_DUTY_STEP
                                                             AND A.APP_ITEM_SEQ = I.APP_ITEM_SEQ
                                                             AND A.APP_CASE_SEQ = I.APP_CASE_SEQ
                                               ) i 
                                               LEFT JOIN (
                                                     SELECT 
                                                            r.app_no 
                                                            , r.vsl_cd
                                                            , r.vsl_type
                                                            , r.type_c_cd
                                                            , r.step_gu
                                                            , r.user_duty_step
                                                            , r.user_duty_work
                                                            , r.user_id
                                                            , r.app_nm
                                                            , h.duty_work as app_duty_work
                                                            , h.ename_f
                                                            , h.ename_l
                                                            , r.tot_score
                                                            , r.app_dt
                                                            , r.app_complete
                                                       FROM t_appraisal_result r
                                                            INNER JOIN hperinfo h
                                                            ON r.app_sno = h.sno(+) 
                                                      WHERE r.app_no =  '" + rAppNo + @"'     
                                                      ORDER BY r.app_no desc
                                               ) r
                                               ON i.VSL_TYPE = r.VSL_TYPE
                                               AND i.type_c_cd = r.type_c_cd
                                               AND i.step_gu = r.step_gu
                                               AND i.app_duty_step = r.user_duty_work
                                         ORDER BY to_number(app_item_seq), to_number(app_case_seq)
                                    ) i
                                    LEFT JOIN (
                                        SELECT * 
                                          FROM T_APPRAISAL_RESULT_DETAIL 
                                    ) D 
                                     ON I.APP_NO = D.APP_NO
                                        AND I.APP_ITEM_NO = D.APP_ITEM_NO
                                )
                                LEFT JOIN T_APPRAISAL_CODE C
                                  ON D.GRADE = C.GRADE
                        ";
                        }
                        else if (rInquiry == "000002")
                        {
                            xSql = @"
                         SELECT i.app_no || '^' || i.app_item_no as app_keys
                                , i.app_no 
                                , i.app_item_no
                                , i.app_item_seq
                                , d.grade ";
                            if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                            {
                                xSql += ", i.app_item_nm ";
                                xSql += ", i.app_item_desc ";
                                xSql += ", i.app_case_desc ";
                            }
                            else
                            {
                                xSql += ", app_item_nm_eng as app_item_nm ";
                                xSql += ", app_item_desc_eng as app_item_desc ";
                                xSql += ", app_case_desc_eng as app_case_desc ";
                            }
                            xSql += @", i.app_case_seq 
                                , i.course_ojt
                                , i.course_lms
                                , i.course_etc
                                , i.user_id
                                , i.vsl_cd
                                , i.vsl_type
                                , i.app_nm
                                , i.app_duty_work
                                , i.tot_score
                                , to_char(i.app_dt, 'yyyy.mm.dd')
                                , d.remark
                                , i.app_complete
                           FROM (
                                    (   
                                        SELECT i.* 
                                                , r.app_no 
                                                , r.vsl_cd
                                                , r.user_id
                                                , r.app_nm
                                                , r.tot_score
                                                , r.app_duty_work
                                                , r.app_dt
                                                , r.app_complete
                                          FROM (

                                                   SELECT  I.*
                                                     FROM  (  
                                                            SELECT  MAX(APP_BASE_DT) AS APP_BASE_DT
                                                                    , STEP_GU
                                                                    , VSL_TYPE
                                                                    , TYPE_C_CD
                                                                    , APP_DUTY_STEP
                                                                    , APP_ITEM_SEQ
                                                                    , APP_CASE_SEQ
                                                              FROM  T_APPRAISAL_ITEM
                                                             WHERE  app_base_dt <= to_date('" + rAppDate + @"','yyyy.mm.dd') 
                                                                    AND step_gu =  '" + rInquiry + @"' 
                                                                    AND vsl_type =  '" + rVesselType + @"' 
                                                                    AND type_c_cd =  '" + rVesselTypeC + @"' 
                                                                    AND app_duty_step =  '" + rDutyStep + @"'
                                                          GROUP BY  STEP_GU
                                                                    , VSL_TYPE
                                                                    , TYPE_C_CD
                                                                    , APP_DUTY_STEP
                                                                    , APP_ITEM_SEQ
                                                                    , APP_CASE_SEQ  
                                                            ) A
                                                            , T_APPRAISAL_ITEM I
                                                      WHERE A.APP_BASE_DT = I.APP_BASE_DT
                                                            AND A.STEP_GU = I.STEP_GU
                                                            AND A.VSL_TYPE = I.VSL_TYPE
                                                            AND A.TYPE_C_CD = I.TYPE_C_CD
                                                            AND A.APP_DUTY_STEP = I.APP_DUTY_STEP
                                                            AND A.APP_ITEM_SEQ = I.APP_ITEM_SEQ
                                                            AND A.APP_CASE_SEQ = I.APP_CASE_SEQ

                                               ) i 
                                               LEFT JOIN (
                                                     SELECT 
                                                            r.app_no 
                                                            , r.vsl_cd
                                                            , r.vsl_type
                                                            , r.type_c_cd
                                                            , r.step_gu
                                                            , r.user_duty_step
                                                            , r.user_duty_work
                                                            , r.user_id
                                                            , r.app_nm
                                                            , h.duty_work as app_duty_work
                                                            , h.ename_f
                                                            , h.ename_l
                                                            , r.tot_score
                                                            , r.app_dt
                                                            , r.app_complete
                                                       FROM t_appraisal_result r
                                                            INNER JOIN hperinfo h
                                                            ON r.app_sno = h.sno(+) 
                                                      WHERE r.app_no =  '" + rAppNo + @"'     
                                                      ORDER BY r.app_no desc
                                               ) r
                                               ON i.VSL_TYPE = r.VSL_TYPE
                                               AND i.type_c_cd = r.type_c_cd
                                               AND i.step_gu = r.step_gu
                                               AND i.app_duty_step = r.user_duty_step
                                         ORDER BY to_number(app_item_seq), to_number(app_case_seq)
                                    ) i
                                    LEFT JOIN (
                                        SELECT * 
                                          FROM T_APPRAISAL_RESULT_DETAIL 
                                    ) D 
                                     ON I.APP_NO = D.APP_NO
                                        AND I.APP_ITEM_NO = D.APP_ITEM_NO
                                )
                                LEFT JOIN T_APPRAISAL_CODE C
                                  ON D.GRADE = C.GRADE
                        ";
                        }
                    }
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
        #endregion

        /************************************************************
       * Function name : GetShipSchoolResultList
       * Purpose       : 등록과정 전체 목록을 전달하는 처리
       * Input         : string[] rParams (0: pagesize, 1: pageno, 2:user_id, 3:dept_code, 4:user_nm_kor)
       * Output        : DataSet
       *************************************************************/
        public DataSet GetUserList(string[] rParams, CultureInfo rArgCultureInfo)
        {
            DataSet xDs = new DataSet();
            try
            {
                string xWhere = "";
                string xTbl = "";

                /*사용자 요청사항 - 부서코드 없는 사용자도 조회되도록 Seo.jw*/
                /*
                if (rParams[7] == "edu_user")
                {
                    xTbl = @" FROM t_user u
                                      , v_hdeptcode d
                                      , v_hdutystep s
                                      , v_hdutywork w
                                WHERE u.dept_code = d.dept_code
                                  AND u.duty_step = s.duty_step(+)
                                  AND u.duty_work = w.duty_work(+)
                               " + "\r\n";
                }
                else
                {
                    xTbl = @" FROM t_user u
                                      , v_hdeptcode d
                                      , v_hdutystep s
                                      , v_hdutywork w
                                WHERE u.dept_code = d.dept_code
                                  AND u.duty_step = s.duty_step
                                  AND u.duty_work = w.duty_work(+)
                               " + "\r\n";
                }
                 */
                xTbl = @" FROM t_user u
                              , v_hdeptcode d
                              , v_hdutystep s
                              , v_hdutywork w
                        WHERE u.dept_code = d.dept_code(+)
                          AND u.duty_step = s.duty_step
                          AND u.duty_work = w.duty_work(+)
                       " + "\r\n";


                if (rParams[7] == "appr")
                    xWhere = " and u.duty_step in (SELECT DUTY_STEP FROM V_HDUTYSTEP WHERE use_yn='Y' AND STEP_GUBUN='M') ";

                DataTable xDt = null;

                // 사번
                if (!string.IsNullOrEmpty(rParams[2]))
                    xWhere += @" AND u.user_id = '" + rParams[2] + "' " + "\r\n";

                // 부서

                if (!string.IsNullOrEmpty(rParams[3] == "*" ? "" : rParams[3]))
                    xWhere += @" AND u.dept_code = '" + rParams[3] + "' " + "\r\n";

                // 직급
                if (!string.IsNullOrEmpty(rParams[6] == "*" ? "" : rParams[6]))
                    xWhere += @" AND u.duty_step = '" + rParams[6] + "' " + "\r\n";

                // 성명
                if (!String.IsNullOrEmpty(rParams[4]))
                    xWhere += @" and (u.user_nm_kor like '%" + rParams[4] + "%' or user_nm_eng_first || ' ' || user_nm_eng_last like '%" + rParams[4] + "%') " + "\r\n";

                // 주민번호
                if (!String.IsNullOrEmpty(rParams[5]))
                    xWhere += @" AND u.personal_no like '%" + rParams[5] + "%' " + "\r\n";

                string xSql = @"
                       SELECT COUNT(*) ";
                xSql += "   " + xTbl;
                xSql += "   " + xWhere;
                xDt = base.ExecuteDataTable("LMS", xSql);
                base.MergeTable(ref xDs, xDt, "table1");

                xSql = "";

                xSql += @"   
                             select *
                               from (
                            select rownum rnum ";
                xSql += "              , a.* " + "\r\n";
                xSql += "     FROM ( " + "\r\n";
                xSql += "           SELECT   " + "\r\n";
                xSql += @"                  u.user_id ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", u.user_nm_kor ";
                    xSql += ", s.step_name as step_name ";
                    xSql += ", duty_work_name ";
                    xSql += ", dept_ename1 ";
                }
                else
                {
                    xSql += ", u.user_nm_eng_first || ' ' || u.user_nm_eng_last as user_nm_kor ";
                    xSql += ", s.step_ename as step_name ";
                    xSql += ", duty_work_ename as duty_work_name ";
                    xSql += ", dept_ename as dept_ename1 ";
                }
                // , u.user_nm_kor,  , s.step_name, w.duty_work_name, dept_ename1

                xSql += @"              , u.dept_code
                                            , u.duty_step
                                            , w.duty_work
                                            , u.personal_no ";
                xSql += "                " + xTbl;
                xSql += "                " + xWhere;
                xSql += "             order by s.step_seq, u.user_nm_kor ";
                xSql += "          ) a " + "\r\n";
                xSql += "   ) a " + "\r\n";

                if (!String.IsNullOrEmpty(rParams[0]) && !String.IsNullOrEmpty(rParams[1]))
                {
                    xSql += string.Format(" WHERE a.rnum > {0} " + "\r\n", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                    xSql += string.Format("   AND a.rnum <= {0} " + "\r\n", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                }

                xDt = base.ExecuteDataTable("LMS", xSql);
                base.MergeTable(ref xDs, xDt, "table2");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return xDs;
        }

        /********************************************************
        * Function name  : GET_MAX_APP_NO
        * Purpose		 : app_no 키 생성
        * 
        * Input		     : 
        * 
        * Output		 : string - 생성한 키
        *********************************************************/
        #region GET_MAX_APP_NO()
        public string GET_MAX_APP_NO()
        {
            string xApp_no = null; //반환값

            try
            {

                string xKey = "APPR" + System.DateTime.Now.ToString("yyMM"); //조회조건
                string xSql = "select max(app_no) from T_APPRAISAL_RESULT where app_no like '" + xKey + "%'";

                DataTable xDt = base.ExecuteDataTable("LMS", xSql);
                string xResult = xDt.Rows[0][0].ToString();

                if (xResult == "")
                    xApp_no = xKey + "0000";
                else
                    xApp_no = string.Format(xKey.Substring(0, 8) + "{0:000#}", Convert.ToInt32(xResult.Substring(8)) + 1);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }

            return xApp_no;
        }
        #endregion

        /************************************************************
        * Function name : SetApprResultDetail
        * Purpose       : 평가 항목 등록
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetApprResultDetail(object[] rParams
                                            , DataTable rDt
                                            , string gubun)
        {
            Database db = null;
            string xRtn = "";
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS"); //Database 생성
                using (OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection())
                {
                    xCnnLMS.Open();
                    OracleTransaction xTransLMS = xCnnLMS.BeginTransaction();
                    OracleCommand xCmdLMS = null;
                    int CDCnt = 0;
                    string xAppNo = "";
                    int xTotScore = 0;
                    try
                    {
                        xCmdLMS = base.GetSqlCommand(db);
                        OracleParameter[] xPara = null;

                        //평가항목 맞는지 체크
                        bool xIsTrue = false;
                        for (int i = 0; i < rDt.Rows.Count; i++)
                        {
                            DataRow row = rDt.Rows[i];

                            xSql = @" 
                                    SELECT APP_ITEM_NO
                                      FROM T_APPRAISAL_ITEM
                                     WHERE APP_ITEM_NO = :APP_ITEM_NO
                                            AND STEP_GU = :STEP_GU
                                            AND VSL_TYPE = :VSL_TYPE
                                            AND APP_DUTY_STEP = :APP_DUTY_STEP 
                                            AND VSL_TYPE = :VSL_TYPE
                                            AND TYPE_C_CD = :TYPE_C_CD
                                    ";

                            xPara = new OracleParameter[5];
                            xPara[0] = base.AddParam("APP_ITEM_NO", OracleType.VarChar, row["APP_ITEM_NO"]);
                            xPara[1] = base.AddParam("STEP_GU", OracleType.VarChar, rParams[1]);
                            xPara[2] = base.AddParam("VSL_TYPE", OracleType.VarChar, rParams[3]);
                            xPara[3] = base.AddParam("APP_DUTY_STEP", OracleType.VarChar, rParams[2]);
                            xPara[4] = base.AddParam("TYPE_C_CD", OracleType.VarChar, rParams[13]);

                            Object xAppItemNo = base.ExecuteScalar(db, xSql, xPara, xTransLMS);
                            if (!Util.IsNullOrEmptyObject(xAppItemNo))
                            {
                                xIsTrue = true;
                            }
                            else
                            {
                                xIsTrue = false;
                                break;
                            }
                        }

                        if (xIsTrue)
                        {
                            if (gubun == "new")
                            {
                                //같은날 두번 평가 불가
                                xSql = @" select app_no 
                                            from T_APPRAISAL_RESULT 
                                           where rownum=1 
                                                  and user_id = :user_id 
                                                  and step_gu = :step_gu 
                                                  and app_dt = :app_dt ";
                                xPara = new OracleParameter[3];
                                xPara[0] = base.AddParam("USER_ID", OracleType.VarChar, rParams[0]);
                                xPara[1] = base.AddParam("STEP_GU", OracleType.VarChar, rParams[1]);
                                xPara[2] = base.AddParam("APP_DT", OracleType.DateTime, rParams[6]);
                                Object xAppNoKey = base.ExecuteScalar(db, xSql, xPara, xTransLMS);

                                if (xAppNoKey == null)
                                {
                                    // app_no 키생성

                                    xAppNo = GET_MAX_APP_NO();

                                    for (int i = 0; i < rDt.Rows.Count; i++)
                                    {
                                        DataRow row = rDt.Rows[i];
                                        if (!String.IsNullOrEmpty(Convert.ToString(row["score"])))
                                        {
                                            xSql = @" 
                                          INSERT INTO T_APPRAISAL_RESULT_DETAIL 
                                          ( 
                                            APP_NO
                                            ,APP_ITEM_NO
                                            ,GRADE
                                            ,APP_DT
                                            ,REMARK
                                            ,UPT_ID
                                            ,UPT_DT 
                                            ,INS_ID
                                            ,INS_DT
                                            ,APP_NM
                                          ) VALUES (
                                            :APP_NO
                                            ,:APP_ITEM_NO
                                            ,:GRADE
                                            ,:APP_DT
                                            ,:REMARK
                                            ,:INS_ID
                                            ,sysdate
                                            ,:INS_ID
                                            ,sysdate
                                            ,:APP_NM
                                          ) ";

                                            xTotScore += Convert.ToInt32(String.IsNullOrEmpty(Convert.ToString(row["score"])) ? "0" : Convert.ToString(row["score"]));
                                            xPara = new OracleParameter[7];
                                            xPara[0] = base.AddParam("APP_NO", OracleType.VarChar, xAppNo);
                                            xPara[1] = base.AddParam("APP_ITEM_NO", OracleType.VarChar, row["app_item_no"]);
                                            xPara[2] = base.AddParam("GRADE", OracleType.VarChar, row["grade"]);
                                            xPara[3] = base.AddParam("APP_DT", OracleType.DateTime, rParams[6]);
                                            xPara[4] = base.AddParam("REMARK", OracleType.VarChar, "");
                                            xPara[5] = base.AddParam("INS_ID", OracleType.VarChar, rParams[14]);
                                            xPara[6] = base.AddParam("APP_NM", OracleType.VarChar, rParams[8]);

                                            xCmdLMS.CommandText = xSql;
                                            base.Execute(db, xCmdLMS, xPara, xTransLMS);
                                            xCmdLMS.Parameters.Clear();
                                        }

                                        if (string.IsNullOrEmpty(Convert.ToString(rDt.Rows[i]["grade"])))
                                        {
                                            CDCnt++;
                                        }
                                        else
                                        {
                                            if (Convert.ToString(rDt.Rows[i]["grade"]) != "S" && Convert.ToString(rDt.Rows[i]["grade"]).CompareTo("B") > 0)
                                                CDCnt++;
                                        }
                                    }

                                    xSql = @" INSERT INTO t_appraisal_result
                                  ( 
                                    APP_NO
                                    ,USER_ID
                                    ,STEP_GU
                                    ,USER_DUTY_STEP
                                    ,VSL_TYPE
                                    ,VSL_CD
                                    ,ON_DT
                                    ,APP_DT
                                    ,TOT_SCORE
                                    ,APP_SNO
                                    ,APP_NM
                                    ,APP_DUTY_STEP
                                    ,INS_ID
                                    ,INS_DT
                                    ,USER_DUTY_WORK
                                    ,TYPE_C_CD
                                  ) 
                                  values
                                  (
                                    :APP_NO
                                    ,:USER_ID
                                    ,:STEP_GU
                                    ,:USER_DUTY_STEP
                                    ,:VSL_TYPE
                                    ,:VSL_CD
                                    ,to_date((select ord_fdate from V_HORDERDET_ORD_LATEST_ONDATE where rownum=1 and SNO=:user_id),'yyyymmdd')
                                    ,:APP_DT
                                    ,:TOT_SCORE
                                    ,:APP_SNO
                                    ,:APP_NM
                                    ,:APP_DUTY_STEP
                                    ,:INS_ID
                                    ,sysdate
                                    ,:USER_DUTY_WORK
                                    ,:TYPE_C_CD
                                  )
                                 ";
                                    xPara = new OracleParameter[14];
                                    xPara[0] = base.AddParam("APP_NO", OracleType.VarChar, xAppNo);
                                    xPara[1] = base.AddParam("USER_ID", OracleType.VarChar, rParams[0]);
                                    xPara[2] = base.AddParam("STEP_GU", OracleType.VarChar, rParams[1]);
                                    xPara[3] = base.AddParam("USER_DUTY_STEP", OracleType.VarChar, rParams[2]);
                                    xPara[4] = base.AddParam("VSL_TYPE", OracleType.VarChar, rParams[3]);
                                    xPara[5] = base.AddParam("VSL_CD", OracleType.VarChar, rParams[4]);
                                    //xPara[6] = base.AddParam("ON_DT", OracleType.VarChar, rParams[5]);
                                    xPara[6] = base.AddParam("APP_DT", OracleType.DateTime, rParams[6]);
                                    xPara[7] = base.AddParam("TOT_SCORE", OracleType.Int32, xTotScore);
                                    xPara[8] = base.AddParam("APP_SNO", OracleType.VarChar, rParams[7]);
                                    xPara[9] = base.AddParam("APP_NM", OracleType.VarChar, rParams[8]);
                                    xPara[10] = base.AddParam("APP_DUTY_STEP", OracleType.VarChar, rParams[9]);
                                    xPara[11] = base.AddParam("INS_ID", OracleType.VarChar, rParams[14]);
                                    xPara[12] = base.AddParam("USER_DUTY_WORK", OracleType.VarChar, rParams[2]);
                                    xPara[13] = base.AddParam("TYPE_C_CD", OracleType.VarChar, rParams[13]);

                                    xCmdLMS.CommandText = xSql;
                                    base.Execute(db, xCmdLMS, xPara, xTransLMS);
                                    xCmdLMS.Parameters.Clear();

                                    xRtn = xAppNo;
                                }
                                else
                                {
                                    //같은날 두번 평가 불가
                                    xRtn = "DLP";
                                }
                            }
                            else
                            {
                                for (int i = 0; i < rDt.Rows.Count; i++)
                                {
                                    DataRow row = rDt.Rows[i];
                                    xTotScore += Convert.ToInt32(String.IsNullOrEmpty(Convert.ToString(row["score"])) ? "0" : Convert.ToString(row["score"]));

                                    xSql = @" 
                                   SELECT APP_ITEM_NO
                                     FROM T_APPRAISAL_RESULT_DETAIL
                                    WHERE APP_NO = :APP_NO
                                      AND APP_ITEM_NO = :APP_ITEM_NO ";
                                    xPara = new OracleParameter[2];
                                    xPara[0] = base.AddParam("APP_NO", OracleType.VarChar, rParams[11]);
                                    xPara[1] = base.AddParam("APP_ITEM_NO", OracleType.VarChar, row["app_item_no"]);
                                    Object xAppItemNo = base.ExecuteScalar(db, xSql, xPara, xTransLMS);

                                    if (string.IsNullOrEmpty(Convert.ToString(xAppItemNo)))
                                    {
                                        if (!String.IsNullOrEmpty(Convert.ToString(row["score"])))
                                        {
                                            xSql = @" 
                                          INSERT INTO T_APPRAISAL_RESULT_DETAIL 
                                          ( 
                                            APP_NO
                                            ,APP_ITEM_NO
                                            ,GRADE
                                            ,APP_DT
                                            ,REMARK
                                            ,UPT_ID
                                            ,UPT_DT 
                                            ,INS_ID
                                            ,INS_DT
                                            ,APP_NM
                                          ) VALUES (
                                            :APP_NO
                                            ,:APP_ITEM_NO
                                            ,:GRADE
                                            ,:APP_DT
                                            ,:REMARK
                                            ,:INS_ID
                                            ,sysdate
                                            ,:INS_ID
                                            ,sysdate
                                            ,:APP_NM
                                          ) ";
                                            xPara = new OracleParameter[7];
                                            xPara[0] = base.AddParam("APP_NO", OracleType.VarChar, rParams[11]);
                                            xPara[1] = base.AddParam("APP_ITEM_NO", OracleType.VarChar, row["app_item_no"]);
                                            xPara[2] = base.AddParam("GRADE", OracleType.VarChar, row["grade"]);
                                            xPara[3] = base.AddParam("APP_DT", OracleType.DateTime, rParams[6]);
                                            xPara[4] = base.AddParam("REMARK", OracleType.VarChar, "");
                                            xPara[5] = base.AddParam("INS_ID", OracleType.VarChar, rParams[14]);
                                            xPara[6] = base.AddParam("APP_NM", OracleType.VarChar, rParams[8]);

                                            xCmdLMS.CommandText = xSql;
                                            base.Execute(db, xCmdLMS, xPara, xTransLMS);
                                            xCmdLMS.Parameters.Clear();
                                        }
                                    }
                                    else
                                    {
                                        xSql = @" 
                                  UPDATE T_APPRAISAL_RESULT_DETAIL 
                                     SET GRADE = :GRADE
                                         ,REMARK = (CASE WHEN (GRADE IS NULL OR GRADE = :GRADE) THEN REMARK ELSE (decode(remark, '', '', remark || '|') || to_char(upt_dt,'yyyy.mm.dd') || '/' || grade || '/' || nvl(app_nm,'')) END) 
                                         ,UPT_ID = :UPT_ID
                                         ,UPT_DT = SYSDATE
                                         ,APP_NM = :APP_NM
                                   WHERE APP_NO = :APP_NO
                                     AND APP_ITEM_NO = :APP_ITEM_NO
                                  ";
                                        xPara = new OracleParameter[5];
                                        xPara[0] = base.AddParam("GRADE", OracleType.VarChar, row["grade"]);
                                        xPara[1] = base.AddParam("UPT_ID", OracleType.VarChar, rParams[14]);
                                        xPara[2] = base.AddParam("APP_NO", OracleType.VarChar, rParams[11]);
                                        xPara[3] = base.AddParam("APP_ITEM_NO", OracleType.VarChar, row["app_item_no"]);
                                        xPara[4] = base.AddParam("APP_NM", OracleType.VarChar, rParams[8]);

                                        xCmdLMS.CommandText = xSql;
                                        base.Execute(db, xCmdLMS, xPara, xTransLMS);
                                        xCmdLMS.Parameters.Clear();
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(rDt.Rows[i]["grade"])))
                                    {
                                        if (Convert.ToString(rDt.Rows[i]["grade"]) != "S" && Convert.ToString(rDt.Rows[i]["grade"]).CompareTo("B") > 0)
                                            CDCnt++;
                                    }
                                    else
                                        CDCnt++;
                                }

                                xSql = @" 
                                    UPDATE t_appraisal_result
                                       SET STEP_GU = :STEP_GU
                                            ,USER_DUTY_STEP = :USER_DUTY_STEP
                                            ,VSL_TYPE = :VSL_TYPE
                                            ,VSL_CD = :VSL_CD
                                            ,APP_DT = :APP_DT
                                            ,TOT_SCORE = :TOT_SCORE
                                            ,APP_SNO = :APP_SNO
                                            ,APP_NM = :APP_NM
                                            ,APP_DUTY_STEP = :APP_DUTY_STEP
                                            ,UPT_ID = :UPT_ID
                                            ,UPT_DT = SYSDATE
                                            ,TYPE_C_CD = :TYPE_C_CD
                                     WHERE APP_NO = :APP_NO
                                  ";
                                //xAppNo = Convert.ToString(rParams[12]);

                                xPara = new OracleParameter[12];
                                xPara[0] = base.AddParam("APP_NO", OracleType.VarChar, rParams[11]);
                                xPara[1] = base.AddParam("STEP_GU", OracleType.VarChar, rParams[1]);
                                xPara[2] = base.AddParam("USER_DUTY_STEP", OracleType.VarChar, rParams[2]);
                                xPara[3] = base.AddParam("VSL_TYPE", OracleType.VarChar, rParams[3]);
                                xPara[4] = base.AddParam("VSL_CD", OracleType.VarChar, rParams[4]);
                                xPara[5] = base.AddParam("APP_DT", OracleType.DateTime, rParams[6]);
                                xPara[6] = base.AddParam("TOT_SCORE", OracleType.Int32, xTotScore);
                                xPara[7] = base.AddParam("APP_SNO", OracleType.VarChar, rParams[7]);
                                xPara[8] = base.AddParam("APP_NM", OracleType.VarChar, rParams[8]);
                                xPara[9] = base.AddParam("APP_DUTY_STEP", OracleType.VarChar, rParams[9]);
                                xPara[10] = base.AddParam("UPT_ID", OracleType.VarChar, rParams[14]);
                                xPara[11] = base.AddParam("TYPE_C_CD", OracleType.VarChar, rParams[13]);

                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS);
                                xCmdLMS.Parameters.Clear();

                                xAppNo = Convert.ToString(rParams[11]);
                                xRtn = xAppNo;
                            }

                            //평가완료여부 체크
                            if (CDCnt == 0 && (!(string.IsNullOrEmpty(xRtn))))
                            {
                                xSql = @"
                              UPDATE T_APPRAISAL_RESULT
                                 SET APP_COMPLETE = '1'
                             "
                               + " WHERE app_no = :app_no ";
                                xPara = new OracleParameter[1];
                                xPara[0] = base.AddParam("APP_NO", OracleType.VarChar, xAppNo);

                                xCmdLMS.CommandText = xSql;
                                base.Execute(db, xCmdLMS, xPara, xTransLMS);
                                xCmdLMS.Parameters.Clear();
                            }

                            //if (string.IsNullOrEmpty(xRtn))
                            //    xRtn = Boolean.TrueString;
                        }
                        else
                        {
                            //조회안한경우
                            xRtn = "search";
                        }

                        xTransLMS.Commit(); //트렌잭션 커밋
                    }
                    catch (Exception ex)
                    {
                        // 트랜잭션 롤백
                        xRtn = string.Empty; // Boolean.FalseString;
                        xTransLMS.Rollback();

                        bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                        if (rethrow) throw;
                    }
                    finally
                    {
                        if (xCmdLMS != null) xCmdLMS.Dispose();
                        if (xTransLMS != null) xTransLMS.Dispose();
                        if (xCnnLMS != null) { if (xCnnLMS.State == ConnectionState.Open) xCnnLMS.Close(); }
                    }
                }
            }
            catch (Exception ex)
            {
                //xRtn = Boolean.FalseString;
                xRtn = string.Empty;
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
        * Function name : DelApprResult
        * Purpose       : 평가 항목 삭제
        * Input         : DataTable rDt
        * Output        : String Boolean
        *************************************************************/
        public string DelApprResult(DataTable rDt)
        {
            Database db = null;
            string xRtn = Boolean.FalseString;
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS"); //Database 생성
                using (OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection())
                {
                    xCnnLMS.Open();
                    OracleTransaction xTransLMS = xCnnLMS.BeginTransaction();
                    OracleCommand xCmdLMS = null;

                    try
                    {
                        xCmdLMS = base.GetSqlCommand(db);
                        OracleParameter[] xPara = null;

                        for (int i = 0; i < rDt.Rows.Count; i++)
                        {
                            xSql = @" 
                                DELETE FROM T_APPRAISAL_RESULT 
                                 WHERE APP_NO = :APP_NO
                              ";
                            DataRow row = rDt.Rows[i];
                            xPara = new OracleParameter[1];
                            xPara[0] = base.AddParam("APP_NO", OracleType.VarChar, row["app_no"]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);
                            xCmdLMS.Parameters.Clear();

                            xSql = @" 
                                DELETE FROM T_APPRAISAL_RESULT_DETAIL 
                                 WHERE APP_NO = :APP_NO
                              ";
                            xPara = new OracleParameter[1];
                            xPara[0] = base.AddParam("APP_NO", OracleType.VarChar, row["app_no"]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);
                            xCmdLMS.Parameters.Clear();
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
            return xRtn;
        }

        /************************************************************
       * Function name : GetApprTarget
       * Purpose       : 
       * Input         : string[] rParams (0: pagesize, 1: pageno)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetDtApprTarget(string[] rParams, CultureInfo rArgCultureInfo)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;

                xSql = "  SELECT ";
                xSql += "  d_cd, ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += "  d_knm ";
                }
                else
                {
                    xSql += "  d_enm AS d_knm ";
                }

                xSql += " FROM ";
                xSql += " T_APPRAISAL_TARGET ";
                xSql += "  WHERE 1=1 ";

                if (rParams.Length > 0)
                {
                    //현직
                    if (rParams[0] == "000001")
                        xSql += string.Format(" AND G_CD ='10' ");
                    else if (rParams[0] == "000002") //상위직

                        xSql += string.Format(" AND G_CD ='30' ");
                }
                xSql += " order by SORTORDER ";

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
        * Function name : SetAppraisalItemInf
        * Purpose       : 
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public string SetAppraisalItemInf(string[] rParams)
        {
            Database db = null;
            string xRtn = Boolean.FalseString;

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
                        OracleParameter[] oOraParams = new OracleParameter[2];
                        oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_APPRAISAL_ITEM");
                        oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_APPRAISAL_ITEM");

                        base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);
                        xRtn = Boolean.TrueString;
                        xTransLMS.Commit();
                    }
                    catch (Exception ex)
                    {
                        // 트랜잭션 롤백
                        xTransLMS.Rollback();
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

    }
}
