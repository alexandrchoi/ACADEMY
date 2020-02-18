using System;
using System.Collections.Generic;
using System.Text;


using System.Data;
using System.Data.OracleClient;
using CLT.WEB.BIZ.FX.BIZBASE;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace CLT.WEB.BIZ.LMS.MANAGE
{
    /// <summary>
    /// 1. 작업개요 : vp_m_company_md
    /// 
    /// 2. 주요기능 : 법인사(회사) 가입 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_m_company_md
    /// 
    /// 4. 작 업 자 : 김민규 / 2012.03.05
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_m_company_md
    ///        * Comment 
    ///          Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제따른 변경
    /// </summary>
    /// 
    class vp_m_company_md : DAC
    {
        public DataTable GetCompany(string[] rParams)
        {
            DataTable rDt = null;
            try
            {
                string xSql = string.Empty;
                xSql += "SELECT * FROM ( ";
                xSql += "      SELECT rownum rnum, b.* FROM ( ";
                xSql += "             SELECT  company.company_nm, company.company_ceo, ";

                //2014.03.19 seojw COMPANY_SCALE을 company_kind로 잘못 Bind하고 있었음.....
                //xSql += "                    (SELECT d_knm FROM t_code_detail WHERE d_cd = company.company_kind AND m_cd = '0043') kindknm, ";
                xSql += "                    (SELECT d_knm FROM t_code_detail WHERE d_cd = company.COMPANY_SCALE AND m_cd = '0043') kindknm, ";

                xSql += "                    tuser.user_nm_kor user_nm, company.tel_no, TO_CHAR(company.ins_dt,'YYYY.MM.DD') company_dt, ";
                if (rParams[8] == "Y")
                {
                    xSql += "                    company.company_id,  company_nm_eng, ";

                    //2014.03.19 seojw COMPANY_SCALE을 company_kind로 잘못 Bind 있었음.....
                    //xSql += "                    (SELECT d_enm FROM t_code_detail WHERE d_cd = company.company_kind AND m_cd = '0043') kindenm, ";
                    xSql += "                    (SELECT d_enm FROM t_code_detail WHERE d_cd = company.COMPANY_SCALE AND m_cd = '0043') kindenm, ";
                    
                    xSql += "                    (SELECT d_enm FROM t_code_detail WHERE d_cd = company.status AND m_cd = '0044') statusenm, ";
                    xSql += "                    count(*) over() totalrecordcount, company.status companystatus,";
                    xSql += "                    company.company_code code, company.company_kind, company.zip_code, company.company_addr, ";
                }
                xSql += "                    (SELECT d_knm FROM t_code_detail WHERE d_cd = company.status AND m_cd = '0044') statusknm, ";
                xSql += "                    company.emp_cnt_vessel + company.emp_cnt_shore as emp_cnt ";

                /*
                 * 2014.03.19 seojw
                 * 이전에는 담당자를 법인사 등록시 등록자(ins_id)로 Bind 했으나
                 * 문서영대리 요청으로 각 회사의 법인사 관리자를 Bind하기로 함.
                 * 법인사 관리자는 각 회사에 1명만 등록된다고 함. 그러나 만약을 위해 여러명 존재할 경우 
                 * 오류방지를 위해 1명만 들고 오도록 함.
                */
                //xSql += "               FROM t_company company, t_user tuser ";
                xSql += "               FROM t_company company, ";
                xSql += @"                     (SELECT A.*
                                                  FROM (SELECT USER_NM_KOR,
                                                               USER_ID,
                                                               COMPANY_ID,
                                                               ROW_NUMBER() OVER(PARTITION BY COMPANY_ID ORDER BY INS_DT DESC) RN
                                                          FROM T_USER
                                                         WHERE USER_GROUP = '000007') A
                                                 WHERE A.RN = 1) TUSER
                ";
                //xSql += "              WHERE company.ins_id = tuser.user_id(+) ";
                xSql += "              WHERE company.company_id = tuser.company_id(+) ";


                //xSql += "              WHERE company.ins_id IS NOT NULL ";
                if (rParams[6].ToString() != "000001")  // 관리자가 아닐경우
                {
                    xSql += string.Format(" AND company.company_id = '{0}' ", rParams[7]);
                    xSql += string.Format(" AND company.status IN ('{0}','{1}','{2}') ", "000001", "000003", "000004");   // 미승인, 승인, 승인대기 만 조회 사용안함은 삭제된 정보..
                }
                
                if (!string.IsNullOrEmpty(rParams[2]))
                    xSql += string.Format(" AND company_nm LIKE '%{0}%' ", rParams[2]); // 회사명

                if (!string.IsNullOrEmpty(rParams[3]))
                    xSql += string.Format(" AND company.ins_dt >= TO_DATE('{0}','YYYY.MM.DD.HH24MISS')", rParams[3] + ".000000"); // 가입기간 From

                if (!string.IsNullOrEmpty(rParams[4]))
                    xSql += string.Format(" AND company.ins_dt <= TO_DATE('{0}','YYYY.MM.DD.HH24MISS')", rParams[4] + ".235959"); // 가입기간 To

                if (!string.IsNullOrEmpty(rParams[5]))  // 상태
                    xSql += string.Format(" AND company.status = '{0}' ", rParams[5]);

                xSql += " ORDER BY company.company_nm ASC ";

                xSql += "              ) b ";
                xSql += "       ) ";

                if (rParams[8] == "Y")
                {
                    xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                    xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));
                }

                

                rDt = base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return rDt;
        }


        /************************************************************
        * Function name : SetCompanyDelete
        * Purpose       : 회원사정보 삭제(변경) 실제로 삭제 하지는 않음
        * Input         : string[] rParams 
        * Output        : String Bollean Type
        *************************************************************/
        public DataTable GetCompanyDetail(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT company_id, company_code, company_nm, company_kind, tax_no, company_ceo, company_type, busi_conditions, ";
                xSql += "        zip_code, company_addr, tel_no, fax_no, home_add, company_scale, reg_no, empoly_ins_no, company_nm_eng, emp_cnt_shore, emp_cnt_vessel ";
                xSql += "   FROM t_company ";
                xSql += string.Format(" WHERE company_id = '{0}' ", rParams[0]);

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
        * Function name : GetCompanyDup
        * Purpose       : 회원사 코드 중복 Count를 리턴
        * Input         : string[] rParams 
        * Output        : String Bollean Type
        *************************************************************/
        public DataTable GetCompanyDup(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT COUNT(*) as CNT";
                xSql += "   FROM t_company ";
                xSql += string.Format(" WHERE company_code = '{0}' ", rParams[1]);

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
        * Function name : SetCompanyDelete
        * Purpose       : 회원사정보 삭제(변경) 실제로 삭제 하지는 않음
        * Input         : string[] rParams 
        * Output        : String Bollean Type
        *************************************************************/
        public string SetCompanyDelete(string[,] rParams)
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
                        string xsql = " UPDATE t_company SET ";
                        xsql += string.Format(" Status = '{0}', ", rParams[i,2]);
                        xsql += string.Format(" upt_id = '{0}', ", rParams[i,1]);
                        xsql += " upt_dt = SYSDATE ";
                        xsql += string.Format(" WHERE company_id = '{0}'", rParams[i,0]);

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
        * Function name : SetCompanyInfo
        * Purpose       : 법인사 정보 등록
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetCompanyInfo(string[] rParams)
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
                    xSql += "INSERT INTO t_company (company_id, ";       // 업체ID
                    xSql += " company_code, ";                           // 업체코드(약어)
                    xSql += " company_nm, ";                             // 업체명             
                    xSql += " company_kind, ";                           // 업체구분                                       
                    xSql += " tax_no, ";                                 // 사업자등록번호
                    xSql += " reg_no, ";                                 // 법인등록번호
                    xSql += " empoly_ins_no, ";                          // 고용보험번호
                    xSql += " company_ceo, ";                            // 대표자명
                    xSql += " busi_conditions, ";                        // 업태
                    xSql += " company_type, ";                           // 종목
                    xSql += " zip_code, ";                               // 우편번호
                    xSql += " company_addr, ";                           // 주소
                    xSql += " tel_no, ";                                 // 전화번호
                    xSql += " fax_no, ";                                 // 팩스번호
                    xSql += " home_add, ";                               // 홈페이지
                    xSql += " company_scale, ";                          // 회사규모
                    xSql += " status, ";                                 // 상태
                    xSql += " company_nm_eng, ";                         // 회사명(영문)
                    xSql += " emp_cnt_shore, ";                          // 
                    xSql += " emp_cnt_vessel, ";                         // 
                    xSql += " ins_id, ";                                 // 작성자 ID
                    xSql += " ins_dt, ";                                 // 작성일자
                    xSql += " upt_id, ";                                 // 수정자 ID
                    xSql += " upt_dt) ";                                 // 수정일자
                    xSql += string.Format(" VALUES ( '{0}', ",rParams[0]);        // 업체ID
                    xSql += string.Format("'{0}', ", rParams[1]);  // 업체코드(약어)
                    xSql += string.Format("'{0}', ", rParams[2]);  // 업체명
                    xSql += string.Format("'{0}', ", rParams[16]); // 업체구분
                    xSql += string.Format("'{0}', ", rParams[4]);  // 사업자 등록번호
                    xSql += string.Format("'{0}', ", rParams[5]);  // 법인등록번호
                    xSql += string.Format("'{0}', ", rParams[6]);  // 고용보험번호
                    xSql += string.Format("'{0}', ", rParams[7]);  // 대표자명
                    xSql += string.Format("'{0}', ", rParams[9]);  // 업태
                    xSql += string.Format("'{0}', ", rParams[10]);  // 종목
                    xSql += string.Format("'{0}', ", rParams[11]);  // 우편번호
                    xSql += string.Format("'{0}', ", rParams[12]);  // 주소
                    xSql += string.Format("'{0}', ", rParams[13]);  // 전화번호
                    xSql += string.Format("'{0}', ", rParams[14]);  // 팩스번호
                    xSql += string.Format("'{0}', ", rParams[8]);  // 홈페이지
                    xSql += string.Format("'{0}', ", rParams[3]);  // 회사규모
                    xSql += "'000004', ";              // 사용자 상태(승인대기)  회원가입후에는 로그인이 안되므로 승인대기 처리시킴
                    xSql += string.Format("'{0}', ", rParams[17]); // 회사명(영문)

                    xSql += string.Format("{0}, ", rParams[18]); // 
                    xSql += string.Format("{0}, ", rParams[19]); // 

                    xSql += string.Format("'{0}', ", rParams[15]); // 작성자 ID
                    xSql += " SYSDATE, ";              // 작성일자
                    xSql += string.Format("'{0}', ", rParams[15]); // 수정자 ID
                    xSql += " SYSDATE) ";              // 수정일자

                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);

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

         
        /************************************************************
        * Function name : SetCompanyInfoUpdate
        * Purpose       : 법인사 정보 등록
        * Input         : string[] rParams 
        * Output        : String Boolean
        *************************************************************/
        public string SetCompanyInfoUpdate(string[] rParams)
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
                    xSql += " UPDATE t_company SET ";
                    xSql += string.Format(" company_code = '{0}', ", rParams[1]);  // 업체코드(약어)
                    xSql += string.Format(" company_nm = '{0}', ", rParams[2]);  // 업체명             
                    xSql += string.Format(" company_kind = '{0}', ", rParams[16]);  // 업체구분                                       
                    xSql += string.Format(" tax_no = '{0}', ", rParams[4]);  // 사업자등록번호
                    xSql += string.Format(" reg_no = '{0}', ", rParams[5]);  // 법인등록번호
                    xSql += string.Format(" empoly_ins_no = '{0}', ", rParams[6]);  // 고용보험번호
                    xSql += string.Format(" company_ceo = '{0}', ", rParams[7]);   // 대표자명
                    xSql += string.Format(" busi_conditions = '{0}', ", rParams[9]);  // 업태
                    xSql += string.Format(" company_type = '{0}', ", rParams[10]);  // 종목
                    xSql += string.Format(" zip_code = '{0}', ", rParams[11]);  // 우편번호
                    xSql += string.Format(" company_addr = '{0}', ", rParams[12]);  // 주소
                    xSql += string.Format(" tel_no = '{0}', ", rParams[13]);  // 전화번호
                    xSql += string.Format(" fax_no = '{0}', ", rParams[14]);  // 팩스번호
                    xSql += string.Format(" home_add = '{0}', ", rParams[8]);  // 홈페이지
                    xSql += string.Format(" company_scale = '{0}', ", rParams[3]);  // 회사규모
                    xSql += string.Format(" company_nm_eng = '{0}', ", rParams[17]);  // 회사명(영문)

                    xSql += string.Format(" emp_cnt_shore = {0}, ", rParams[18]);  // 
                    xSql += string.Format(" emp_cnt_vessel = {0}, ", rParams[19]);  // 
                    
                    xSql += string.Format(" upt_id = '{0}', ", rParams[15]);  // 수정자 ID
                    xSql += " upt_dt = SYSDATE ";  // 수정일자
                    xSql += string.Format("WHERE company_id = '{0}' ", rParams[0]);       // 업체ID

                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);

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
