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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.BIZ.LMS.CURR
{ 
    /// <summary>
    /// 1. 작업개요 : vp_c_lecturer_md Class
    /// 
    /// 2. 주요기능 : 강사 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_c_lecturer_md
    /// 
    /// 4. 작 업 자 : 최인재 / 2020.02.
    /// </summary>
    public class vp_c_lecturer_md : DAC
    {
        /************************************************************
        * Function name : GetLecturerList
        * Purpose       : 강사 목록 조회 
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetLecturerList(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;
                
                xSql = @" 
                SELECT * FROM 
                (
                    SELECT ROWNUM AS RNUM, LECTURER.* FROM 
                    (
                        SELECT b.*,
                               TO_CHAR(b.ins_dt,'YYYY.MM.DD') birthday_dt, TO_CHAR(b.ins_dt,'YYYY.MM.DD') regist_dt,
                               DECODE(b.res_file_nm, NULL, 'N', 'Y') as resume, DECODE(b.doc_file_nm1, NULL, 'N', 'Y') as document,
                               (SELECT step_name FROM v_hdutystep WHERE v_hdutystep.duty_step = b.duty_step AND use_yn = 'Y') AS dutystep,
                               count(*) over() totalrecordcount
                FROM T_LECTURER b
                        WHERE 1 = 1 ";

                if (!string.IsNullOrEmpty(rParams[2]))
                    xSql += string.Format(" AND lecturer_nm LIKE '%{0}%' ", rParams[2]); // 강사명

                if (!string.IsNullOrEmpty(rParams[3]))
                    xSql += string.Format(" AND ins_dt >= TO_DATE('{0}','YYYY.MM.DD.HH24MISS')", rParams[3] + ".000000"); // 가입기간 From

                if (!string.IsNullOrEmpty(rParams[4]))
                    xSql += string.Format(" AND ins_dt <= TO_DATE('{0}','YYYY.MM.DD.HH24MISS')", rParams[4] + ".235959"); // 가입기간 To

                if (!string.IsNullOrEmpty(rParams[5]))  // 상태
                {
                    string xDelYN = (Convert.ToString(rParams[5]) == "Y" ? "9" : "1");
                    xSql += string.Format(" AND status = '{0}' ", xDelYN);
                }
                xSql += @"
                        ORDER BY INS_DT DESC 
                    ) LECTURER
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
        * Function name : GetLecturerInfo
        * Purpose       : 강사 상세조회 
        * Input         : string[] rParams (0: pagesize, 1: pageno)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetLecturerInfo(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;                

                xSql = " SELECT a.*, to_char(birth_dt, 'yyyy.MM.dd') as birth_date FROM t_lecturer a ";                 
                xSql += string.Format("        WHERE lecturer_id ='{0}' ", rParams[0]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
        * Function name : GetLecturerDup
        * Purpose       : 강사 ID 중복 Count를 리턴
        * Input         : string[] rParams 
        * Output        : String Bollean Type
        *************************************************************/
        public DataTable GetLecturerDup(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT COUNT(*) as CNT";
                xSql += "   FROM t_lecturer ";
                xSql += string.Format(" WHERE lecturer_id = '{0}' ", rParams[1]);

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
        * Function name : SetLecturerDelete
        * Purpose       : 강사정보 삭제(변경) 실제로 삭제 하지는 않음
        * Input         : string[] rParams 
        * Output        : String Bollean Type
        *************************************************************/
        public string SetLecturerDelete(string[,] rParams)
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
                    //int j = xParams.GetLength(0);
                    for (int i = 0; i < rParams.GetLength(0); i++)
                    {
                        string xsql = " UPDATE t_lecturer SET ";
                        xsql += string.Format(" status = '{0}', ", rParams[i, 2]);
                        xsql += string.Format(" del_id = '{0}', ", rParams[i, 1]);
                        xsql += " del_dt = SYSDATE ";
                        xsql += string.Format(" WHERE lecturer_id = '{0}'", rParams[i, 0]);

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
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xRtn;
        }

        /************************************************************
        * Function name : SetLecturerInfo
        * Purpose       : 강사 정보 등록
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetLecturerInfo(string[] rParams, byte[] rFileRes, string rFileNameRes, byte[] rFileDoc, string rFileNameDoc)
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
                    string xSql = string.Empty;
                    xSql += "INSERT INTO t_lecturer (lecturer_id, ";    // 강사ID
                    xSql += " lecturer_nm, ";                           // 강사명
                    xSql += " lecturer_nm_eng, ";                       // 강사영문명
                    xSql += " birth_dt, ";                              // 생년월일
                    xSql += " grade, ";                                 // 강사등급(A,B,C,D)
                    xSql += " user_id, ";                               // 사용자ID

                    xSql += " job, ";                                   // 직업
                    xSql += " education, ";                             // 학력
                    xSql += " major, ";                                 // 전공
                    xSql += " org_nm, ";                                // 소속구분
                    xSql += " company_nm, ";                            // 회사명
                    xSql += " duty_step, ";                             // 직위

                    xSql += " zip_code, ";                              // 우편번호
                    xSql += " company_addr, ";                          // 주소
                    xSql += " tel_no, ";                                // 전화번호
                    xSql += " mobile_phone, ";                          // 휴대폰번호
                    xSql += " email, ";                                 // 이메일

                    xSql += " acc_bank, ";                              // 은행코드
                    xSql += " account, ";                               // 계좌번호
                    xSql += " status, ";                                // 상태

                    xSql += " ins_id, ";                                // 작성자 ID
                    xSql += " ins_dt, ";                                // 작성일자
                    xSql += " upt_id, ";                                // 수정자 ID
                    xSql += " upt_dt) ";                                // 수정일자
                    xSql += string.Format(" VALUES ( '{0}', ", rParams[0]);
                    xSql += string.Format("'{0}', ", rParams[1]);
                    xSql += string.Format("'{0}', ", rParams[2]);

                    if (rParams[3] == null)
                        xSql += string.Format(" null, ", rParams[3]);
                    else
                        xSql += string.Format(" TO_DATE('{0}', 'yyyy.MM.dd'), ", rParams[3]);

                    xSql += string.Format("'{0}', ", rParams[4]);
                    xSql += string.Format("'{0}', ", rParams[5]);

                    xSql += string.Format("'{0}', ", rParams[6]);
                    xSql += string.Format("'{0}', ", rParams[7]);
                    xSql += string.Format("'{0}', ", rParams[8]);
                    xSql += string.Format("'{0}', ", rParams[9]);
                    xSql += string.Format("'{0}', ", rParams[10]);
                    xSql += string.Format("'{0}', ", rParams[11]);
                    
                    xSql += string.Format("'{0}', ", rParams[12]);
                    xSql += string.Format("'{0}', ", rParams[13]);
                    xSql += string.Format("'{0}', ", rParams[14]);
                    xSql += string.Format("'{0}', ", rParams[15]);
                    xSql += string.Format("'{0}', ", rParams[16]);

                    xSql += string.Format("'{0}', ", rParams[17]);
                    xSql += string.Format("'{0}', ", rParams[18]);
                    xSql += string.Format("'{0}', ", rParams[19]);

                    xSql += string.Format("'{0}', ", rParams[20]); // 작성자 ID
                    xSql += " SYSDATE, ";              // 작성일자
                    xSql += string.Format("'{0}', ", rParams[20]); // 수정자 ID
                    xSql += " SYSDATE) ";              // 수정일자

                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);
                    xTransLMS.Commit(); // 트랜잭션 커밋

                    OracleParameter[] oraParams = null;
                    try
                    {
                        xSql = @"
                        UPDATE t_lecturer
                           SET RES_FILE_NM = :RES_FILE_NM
                             , RES_FILE = :RES_FILE
                         WHERE lecturer_id = :LECTURER_ID
                        ";
                        oraParams = new OracleParameter[3];
                        oraParams[0] = base.AddParam("RES_FILE_NM", OracleType.VarChar, rFileNameRes);
                        oraParams[1] = base.AddParam("RES_FILE", OracleType.Blob, rFileRes.Length, rFileRes);
                        oraParams[2] = base.AddParam("LECTURER_ID", OracleType.VarChar, rParams[0]);
                        base.ExecuteScalar("LMS", xSql, oraParams);
                    }
                    catch { }
                    try
                    {
                        xSql = @"
                        UPDATE t_lecturer
                           SET DOC_FILE_NM1 = :DOC_FILE_NM1
                             , DOC_FILE1 = :DOC_FILE1
                         WHERE lecturer_id = :LECTURER_ID
                        ";
                        oraParams = new OracleParameter[3];
                        oraParams[0] = base.AddParam("DOC_FILE_NM1", OracleType.VarChar, rFileNameDoc);
                        oraParams[1] = base.AddParam("DOC_FILE1", OracleType.Blob, rFileDoc.Length, rFileDoc);
                        oraParams[2] = base.AddParam("LECTURER_ID", OracleType.VarChar, rParams[0]);
                        base.ExecuteScalar("LMS", xSql, oraParams);
                    }
                    catch { }

                    xRtn = Boolean.TrueString;
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
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xRtn;
        }
        
        /************************************************************
        * Function name : SetLecturerInfoUpdate
        * Purpose       : 강사 정보 등록
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetLecturerInfoUpdate(string[] rParams, byte[] rFileRes, string rFileNameRes, byte[] rFileDoc, string rFileNameDoc)
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
                    string xSql = string.Empty;
                    xSql += " UPDATE t_lecturer SET ";
                    xSql += string.Format(" lecturer_nm = '{0}', ", rParams[1]);        // 
                    xSql += string.Format(" lecturer_nm_eng = '{0}', ", rParams[2]);    //        

                    if (rParams[3] == null)
                        xSql += string.Format(" birth_dt = null, ", rParams[3]);
                    else
                        xSql += string.Format(" birth_dt = TO_DATE('{0}', 'yyyy.MM.dd'), ", rParams[3]);

                    xSql += string.Format(" grade = '{0}', ", rParams[4]);              //                                        
                    xSql += string.Format(" user_id = '{0}', ", rParams[5]);            // 
                    xSql += string.Format(" job = '{0}', ", rParams[6]);                // 
                    xSql += string.Format(" education = '{0}', ", rParams[7]);          // 
                    xSql += string.Format(" major = '{0}', ", rParams[8]);              // 
                    xSql += string.Format(" org_nm = '{0}', ", rParams[9]);             // 
                    xSql += string.Format(" company_nm = '{0}', ", rParams[10]);        // 
                    xSql += string.Format(" duty_step = '{0}', ", rParams[11]);         // 
                    xSql += string.Format(" zip_code = '{0}', ", rParams[12]);          // 
                    xSql += string.Format(" company_addr = '{0}', ", rParams[13]);      // 
                    xSql += string.Format(" tel_no = '{0}', ", rParams[14]);            // 
                    xSql += string.Format(" mobile_phone = '{0}', ", rParams[15]);      // 
                    xSql += string.Format(" email = '{0}', ", rParams[16]);             // 

                    xSql += string.Format(" acc_bank = '{0}', ", rParams[17]);          // 
                    xSql += string.Format(" account = '{0}', ", rParams[18]);           // 
                    //xSql += string.Format(" status = '{0}', ", rParams[19]);            // 
                    xSql += string.Format(" upt_id = '{0}', ", rParams[20]);            // 수정자 ID
                    xSql += " upt_dt = SYSDATE ";                                       // 수정일자
                    xSql += string.Format("WHERE lecturer_id = '{0}' ", rParams[0]);    // 강사ID

                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);

                    OracleParameter[] oraParams = null;
                    try
                    {
                        xSql = @"
                        UPDATE t_lecturer
                           SET RES_FILE_NM = :RES_FILE_NM
                             , RES_FILE = :RES_FILE
                         WHERE lecturer_id = :LECTURER_ID
                        ";
                        oraParams = new OracleParameter[3];
                        oraParams[0] = base.AddParam("RES_FILE_NM", OracleType.VarChar, rFileNameRes);
                        oraParams[1] = base.AddParam("RES_FILE", OracleType.Blob, rFileRes.Length, rFileRes);
                        oraParams[2] = base.AddParam("LECTURER_ID", OracleType.VarChar, rParams[0]);
                        base.ExecuteScalar("LMS", xSql, oraParams);
                    }
                    catch { }
                    try
                    {
                        xSql = @"
                        UPDATE t_lecturer
                           SET DOC_FILE_NM1 = :DOC_FILE_NM1
                             , DOC_FILE1 = :DOC_FILE1
                         WHERE lecturer_id = :LECTURER_ID
                        ";
                        oraParams = new OracleParameter[3];
                        oraParams[0] = base.AddParam("DOC_FILE_NM1", OracleType.VarChar, rFileNameDoc);
                        oraParams[1] = base.AddParam("DOC_FILE1", OracleType.Blob, rFileDoc.Length, rFileDoc);
                        oraParams[2] = base.AddParam("LECTURER_ID", OracleType.VarChar, rParams[0]);
                        base.ExecuteScalar("LMS", xSql, oraParams);
                    }
                    catch { }

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
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xRtn;
        }
        
    }
}
