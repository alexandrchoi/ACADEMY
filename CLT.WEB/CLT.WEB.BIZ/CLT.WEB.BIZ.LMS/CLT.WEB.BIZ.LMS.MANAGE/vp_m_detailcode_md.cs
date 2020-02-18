using System;
using System.Collections.Generic;
using System.Text;

// 필수 using 문
using System.Data;
using System.Data.OracleClient;
using CLT.WEB.BIZ.FX.BIZBASE;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace CLT.WEB.BIZ.LMS.MANAGE
{
    /// <summary>
    /// 1. 작업개요 : 코드 관리 
    /// 
    /// 2. 주요기능 : 코드 관리
    ///				  
    /// 3. Class 명 : vp_m_detailcode_md
    /// 
    /// 4. 작 업 자 : 
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_m_detailcode_md
    ///        * Comment 
    ///          Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제따른 변경
    /// </summary>
    class vp_m_detailcode_md : DAC
    {
        /************************************************************
        * Function name : GetDetailCode
        * Purpose       : Master 공통코드 조회
        * Input         : string[] rParams (0: pageSize)  (1: pageIndex)
        *                                  (2: d_cd)      (3: m_desc) 
        *                                  (4: m_enm)     (5: m_knm)       (6: m_cd)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetDetailCode(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;
                if (rParams[7] == "GRID")
                {
                    xSql += " SELECT * FROM ( ";
                    xSql += " SELECT rownum rnum, b.*, c.m_enm, c.m_knm, c.m_desc FROM ( ";
                    xSql += " SELECT m_cd, d_cd, d_desc, d_enm, d_knm, use_yn, send_flg, TO_CHAR(send_dt,'YYYY.MM.DD HH24:MI:SS') send_dt, count(*) over() totalrecordcount ";
                    xSql += "  FROM t_code_detail ";
                    xSql += string.Format(" WHERE  m_cd = '{0}' ", rParams[6]);
                    if (!string.IsNullOrEmpty(rParams[2]))
                        xSql += " AND d_cd LIKE '%" + rParams[2] + "%' ";

                    if (!string.IsNullOrEmpty(rParams[3]))
                        xSql += " AND d_desc LIKE '%" + rParams[3] + "%' ";

                    if (!string.IsNullOrEmpty(rParams[4]))
                        xSql += " AND d_enm LIKE '%" + rParams[4] + "%' ";

                    if (!string.IsNullOrEmpty(rParams[5]))
                        xSql += " AND d_knm LIKE '%" + rParams[5] + "%' ";

                    xSql += " ORDER BY m_cd ASC, d_cd ASC ";
                    xSql += " ) b, t_code_master c where b.m_cd = c.m_cd ";
                    xSql += " ) ";
                    xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                    xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));


                }
                else if (rParams[7] == "EXCEL")
                {
                    xSql += " SELECT b.d_cd, b.d_desc, b.d_enm, b.d_knm, DECODE(b.use_yn, 'Y','Yes','No') AS use_yn, DECODE(b.send_flg, '2','Yes','No') AS send_flg, TO_CHAR(b.send_dt,'YYYY.MM.DD HH24:MI:SS') send_dt ";
                    xSql += "  FROM t_code_detail b, t_code_master c ";
                    xSql += string.Format(" WHERE  b.m_cd = c.m_cd AND b.m_cd = '{0}' ", rParams[6]);
                    xSql += " ORDER BY b.m_cd ASC, b.d_cd ASC ";
                }

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /************************************************************
        * Function name : GetDetailCodeEdit
        * Purpose       : Master 공통코드 조회(수정용)
        * Input         : string[] rParams (0: m_cd)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetDetailCodeEdit(string[] rParams)
        {
            try
            {
                string xSql = "SELECT d_cd, m_cd, d_desc, d_enm, d_knm, use_yn " +
                              "  FROM t_code_detail " +
                              " WHERE m_cd = '" + rParams[0] + "' " +
                              "   AND d_cd = '" + rParams[1] + "' ";

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /************************************************************
        * Function name : SetCodeDetailEdit
        * Purpose       : 사용자정보 변경
        * Input         : string[] rParams 
        * Output        : String Bollean Type
        *************************************************************/
        public string SetCodeDetailEdit(string[] rParams)
        {
            string xRtn = Boolean.FalseString;

            Database db = base.GetDataBase("LMS");;

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
                string xsql = string.Empty;
                string xSendFLG = "1";

                try
                {
                    xsql += "SELECT user_code_yn FROM t_code_master ";
                    xsql += string.Format(" WHERE m_cd = '{0}' ", rParams[6]);
                    object xResult = base.ExecuteScalar("LMS", xsql);
                    if (xResult.ToString() == "Y")
                        xSendFLG = "3";

                    xsql = " UPDATE t_code_detail SET ";
                    xsql += string.Format(" d_desc = '{0}', ", rParams[1]);
                    xsql += string.Format(" d_knm = '{0}', ", rParams[2]);
                    xsql += string.Format(" d_enm = '{0}', ", rParams[3]);
                    xsql += string.Format(" use_yn = '{0}', ", rParams[4]);
                    xsql += string.Format(" upt_id = '{0}', ", rParams[5]);
                    xsql += string.Format(" send_flg = '{0}', ", xSendFLG);
                    xsql += " upt_dt = SYSDATE ";
                    xsql += string.Format(" WHERE m_cd = '{0}' ", rParams[6]);
                    xsql += string.Format("   AND d_cd = '{0}' ", rParams[0]);

                    xCmdLMS.CommandText = xsql;
                    base.Execute(db, xCmdLMS, xTransLMS);


                    if (xSendFLG == "1") // 전송대상이면
                    {
                        OracleParameter[] oOraParams = new OracleParameter[2];
                        oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_CODE_DETAIL");
                        oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_CODE_DETAIL");

                        int i = base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);
                    }

                    xRtn = Boolean.TrueString;

                    xTransLMS.Commit(); // 트랜잭션 커밋

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

        /************************************************************
        * Function name : SetCodeDetail
        * Purpose       : 공통코드(Detail) 정보 등록
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetCodeDetail(string[] rParams)
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
                string xSql = string.Empty;
                

                try
                {
                    xSql += "SELECT user_code_yn FROM t_code_master ";
                    xSql += string.Format(" WHERE m_cd = '{0}' ", rParams[7]);
                    object xResult = base.ExecuteScalar("LMS", xSql);
                    if (xResult.ToString() == "Y")
                        rParams[4] = "3";

                    xSql = string.Empty;
                    xSql = "INSERT INTO t_code_detail  (m_cd, " +   // Master Code
                                                      " d_cd, " +          // Detail Code
                                                      " d_desc, " +       // 코드명
                                                      " d_knm, " +   // 코드 한글명
                                                      " d_enm, " +  // 코드 영문명
                                                      " send_flg, " +  // 전송유무
                                                      " use_yn, " +  // 사용유무
                                                      " ins_id, " +  // 등록자
                                                      " ins_dt, " +  // 등록일자
                                                      " upt_id, " +  // 수정자 
                                                      " upt_dt) " +  // 수정일자
                                                      " VALUES( ";
                    xSql += string.Format(" '{0}', ", rParams[7]);  // Master Code
                    xSql += string.Format(" '{0}', ", rParams[0]);  // Detail Code
                    xSql += string.Format(" '{0}', ", rParams[1]);  // 코드명
                    xSql += string.Format(" '{0}', ", rParams[2]);  // 코드 한글명
                    xSql += string.Format(" '{0}', ", rParams[3]);  // 코드 영문명
                    xSql += string.Format(" '{0}', ", rParams[4]);  // 전송유무  
                    xSql += string.Format(" '{0}', ", rParams[5]);  // 사용유무
                    xSql += string.Format(" '{0}', ", rParams[6]);  // 등록자
                    xSql += " SYSDATE, "; // 등록일자
                    xSql += string.Format(" '{0}', ", rParams[6]);  // 수정자
                    xSql += " SYSDATE) "; // 수정일자


                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);

                    if (rParams[4] == "1") // 전송대상이면
                    {
                        OracleParameter[] oOraParams = new OracleParameter[2];
                        oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_CODE_DETAIL");
                        oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_CODE_DETAIL");

                        int i = base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);
                    }

                    xRtn = Boolean.TrueString;

                    xTransLMS.Commit(); // 트랜잭션 커밋
                }
                catch (Exception ex)
                {
                    xTransLMS.Rollback(); // Exception 발생시 롤백처리
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


        /************************************************************
        * Function name : SetCodeDetailEditUse_YN
        * Purpose       : 공통코드(Detail) 사용유무 변경
        * Input         : string[] rParams 
        * Output        : String Bollean Type
        *************************************************************/
        public string SetCodeDetailEditUse_YN(string[,] rParams)
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
                        string xsql = " UPDATE t_code_detail SET ";
                        xsql += string.Format(" use_yn = '{0}', ", rParams[i, 1]);
                        xsql += string.Format(" upt_id = '{0}', ", rParams[i, 2]);
                        xsql += " upt_dt = SYSDATE ";
                        xsql += string.Format(" WHERE m_cd = '{0}' ", rParams[i, 3]);
                        xsql += string.Format("   AND d_cd = '{0}' ", rParams[i, 0]);

                        xCmdLMS.CommandText = xsql;
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
                throw ex;
            }
            return xRtn;
        }
    }
}
