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
    /// 1. 작업개요 : 
    /// 
    /// 2. 주요기능 : 
    ///				  
    /// 3. Class 명 : vp_m_mastercode_md
    /// 
    /// 4. 작 업 자 : 
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_m_mastercode_md
    ///        * Comment 
    ///          Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제따른 변경
    /// </summary>
    class vp_m_mastercode_md : DAC
    {
        /************************************************************
        * Function name : GetMasterCode
        * Purpose       : Master 공통코드 조회
        * Input         : string[] rParams (0: pageSize)  (1: pageIndex)
        *                                  (2: m_cd)      (3: m_desc) 
        *                                  (4: m_enm)     (5: m_knm)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetMasterCode(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;
                if (rParams[6] == "GRID")
                {
                    xSql += " SELECT * FROM ( ";
                    xSql += " SELECT rownum rnum, b.* FROM ( ";
                    xSql += " SELECT m_cd, m_desc, m_enm, m_knm, use_yn, user_code_yn, sent_flg, TO_CHAR(sent_dt, 'YYYY.MM.DD HH24:MI:SS') sent_dt, count(*) over() totalrecordcount ";
                    xSql += "  FROM t_code_master ";
                    xSql += "  WHERE m_cd IS NOT NULL ";
                    if (!string.IsNullOrEmpty(rParams[2]))
                        xSql += " AND m_cd LIKE '%" + rParams[2] + "%' ";

                    if (!string.IsNullOrEmpty(rParams[3]))
                        xSql += " AND m_desc LIKE '%" + rParams[3] + "%' ";

                    if (!string.IsNullOrEmpty(rParams[4]))
                        xSql += " AND m_enm LIKE '%" + rParams[4] + "%' ";

                    if (!string.IsNullOrEmpty(rParams[5]))
                        xSql += " AND m_knm LIKE '%" + rParams[5] + "%' ";

                    xSql += " ORDER BY m_cd ASC ";
                    xSql += " ) b ";
                    xSql += " ) ";
                    xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                    xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));

                }
                else if (rParams[6] == "EXCEL")
                {
                    xSql += " SELECT m_cd, m_desc, m_enm, m_knm, DECODE(use_yn, 'Y','Yes','No') AS use_yn, DECODE(sent_flg, '2','Yes','No') AS sent_flg, TO_CHAR(sent_dt, 'YYYY.MM.DD HH24:MI:SS') sent_dt ";
                    xSql += "   FROM t_code_master ";
                    xSql += "  ORDER BY m_cd ASC ";
                    if (!string.IsNullOrEmpty(rParams[2]))
                        xSql += " AND m_cd LIKE '%" + rParams[2] + "%' ";

                    if (!string.IsNullOrEmpty(rParams[3]))
                        xSql += " AND m_desc LIKE '%" + rParams[3] + "%' ";

                    if (!string.IsNullOrEmpty(rParams[4]))
                        xSql += " AND m_enm LIKE '%" + rParams[4] + "%' ";

                    if (!string.IsNullOrEmpty(rParams[5]))
                        xSql += " AND m_knm LIKE '%" + rParams[5] + "%' ";
                }

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
       * Function name : GetMasterCodeEdit
       * Purpose       : Master 공통코드 조회(수정용)
       * Input         : string[] rParams (0: m_cd)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetMasterCodeEdit(string[] rParams)
        {
            try
            {
                string xSql = "SELECT m_cd, m_desc, m_enm, m_knm, use_yn, user_code_yn, sent_flg" +
                              "  FROM t_code_master " +
                              " WHERE m_cd = '" + rParams[0] + "' ";

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /************************************************************
        * Function name : SetCodeMaster
        * Purpose       : 공통코드(Master) 정보 등록
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetCodeMaster(string[] rParams)
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
                    string xSql = "INSERT INTO t_code_master  (m_cd, " +   // Master Code
                                                      " m_desc, " +       // 코드명
                                                      " m_knm, " +   // 코드 한글명
                                                      " m_enm, " +  // 코드 영문명
                                                      " sent_flg, " +  // 전송유무
                                                      " use_yn, " +  // 사용유무
                                                      " user_code_yn, " + // 사용자코드 유무
                                                      " ins_id, " +  // 등록자
                                                      " ins_dt, " +  // 등록일자
                                                      " upt_id, " +  // 수정자 
                                                      " upt_dt) " +  // 수정일자
                                                      " VALUES( ";
                    xSql += string.Format(" '{0}', ", rParams[0]);  // Master Code
                    xSql += string.Format(" '{0}', ", rParams[1]);  // 코드명
                    xSql += string.Format(" '{0}', ", rParams[2]);  // 코드 한글명
                    xSql += string.Format(" '{0}', ", rParams[3]);  // 코드 영문명
                    xSql += string.Format(" '{0}', ", rParams[4]);  // 전송유무  
                    xSql += string.Format(" '{0}', ", rParams[5]);  // 사용유무
                    xSql += string.Format(" '{0}', ", rParams[7]); // 사용자코드 유무
                    xSql += string.Format(" '{0}', ", rParams[6]);  // 등록자
                    xSql += " SYSDATE, "; // 등록일자
                    xSql += string.Format(" '{0}', ", rParams[6]);  // 수정자
                    xSql += " SYSDATE) "; // 수정일자


                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);

                    if (rParams[4] == "1") // 전송대상이면
                    {
                        OracleParameter[] oOraParams = new OracleParameter[2];
                        oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_CODE_MASTER");
                        oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_CODE_MASTER");

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
         * Function name : SetCodeMasterEdit
         * Purpose       : 사용자정보 변경
         * Input         : string[] rParams 
         * Output        : String Bollean Type
         *************************************************************/
        public string SetCodeMasterEdit(string[] rParams)
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
                    string xsql = " UPDATE t_code_master SET ";
                    xsql += string.Format(" m_desc = '{0}', ", rParams[1]);
                    xsql += string.Format(" m_knm = '{0}', ", rParams[2]);
                    xsql += string.Format(" m_enm = '{0}', ", rParams[3]);
                    xsql += string.Format(" use_yn = '{0}', ", rParams[4]);
                    xsql += string.Format(" upt_id = '{0}', ", rParams[5]);
                    xsql += string.Format(" user_code_yn = '{0}', ", rParams[6]);
                    xsql += string.Format(" sent_flg = '{0}', ", rParams[8]);
                    xsql += " upt_dt = SYSDATE ";
                    xsql += string.Format(" WHERE m_cd = '{0}' ", rParams[0]);

                    xCmdLMS.CommandText = xsql;
                    base.Execute(db, xCmdLMS, xTrnsLMS);

                    if (rParams[8] == "1") // 전송대상이면
                    {
                        
                        
                        OracleParameter[] oOraParams = new OracleParameter[2];
                        oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_CODE_MASTER");
                        oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_CODE_MASTER");

                        int i = base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTrnsLMS);
                    }

                    xRtn = Boolean.TrueString;

                    xTrnsLMS.Commit(); // 트랜잭션 커밋

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



        /************************************************************
        * Function name : SetCodeMasterEditUse_YN
        * Purpose       : 공통코드 사용유무 변경
        * Input         : string[] rParams 
        * Output        : String Bollean Type
        *************************************************************/
        public string SetCodeMasterEditUse_YN(string[,] rParams)
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

                        string xsql = " UPDATE t_code_master SET ";
                        xsql += string.Format(" use_yn = '{0}', ", rParams[i,1]);
                        xsql += string.Format(" upt_id = '{0}', ", rParams[i, 2]);
                        xsql += " upt_dt = SYSDATE ";
                        xsql += string.Format(" WHERE m_cd = '{0}' ", rParams[i, 0]);

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
