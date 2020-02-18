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
    /// 3. Class 명 : vp_m_manage_md
    /// 
    /// 4. 작 업 자 : 
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_m_manage_md
    ///        * Comment 
    ///          Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제따른 변경
    /// </summary>
    public class vp_m_manage_md : DAC
    {
        public DataTable GetUserGroupCode(string[] rParams)
        {
            try
            {
                string xSql = "SELECT d_cd detailcode, d_knm groupname FROM T_CODE_DETAIL" +
                              " WHERE m_cd = " + "'0041'";

                return base.ExecuteDataTable("LMS", xSql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetUser(string[] rParams)
        {
            try
            {
                string xSql = "SELECT user_id FROM t_user WHERE user_group = '" + rParams[0] + "'";

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetMenuGroup(string[] rParams)
        {
            try
            {
                string xSql = " SELECT rownum rnum, " +
                              " CASE WHEN (SUBSTR(mgroup.menu_group,2,2)) = '00' THEN menu.menu_nm_kor " +
                              "  END AS menu1, " +
                              " CASE WHEN ((substr(mgroup.menu_group,2,1)) != '0') AND ((SUBSTR(mgroup.menu_group,3,1)) = '0') THEN menu.menu_nm_kor " +
                              "  END AS menu2, " +
                              " CASE WHEN (substr(mgroup.menu_group,3,1)) != '0' THEN menu.menu_nm_kor " +
                              "  END AS menu3, " +
                              "  menu.menu_group mgroupcode, mgroup.inquiry_yn, mgroup.edit_yn, mgroup.del_yn, mgroup.admin_yn, " +
                              "  (SELECT count(*) FROM t_menu_group) totalrecordcount " + 
                              " FROM t_menu_group mgroup, t_menu menu, t_user tuser " +
                              " WHERE mgroup.menu_group = menu.menu_group  " +
                              "   AND tuser.user_id = mgroup.user_id " +
                              "   AND tuser.user_group = '" + rParams.GetValue(2) + "'" +
                              " ORDER BY mgroup.menu_group ASC";
                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
        * Function name : SetMenuGroup
        * Purpose       : 메뉴권한 변경
        * Input         : string[] rParams (0: menu_group), (1: user_id)
        *                                  (2: inquiry_yn), (3: edit_yn)
        *                                  (4: del_yn),     (5: admin_yn)
        * Output        : String Bollean Type
        *************************************************************/
        public string SetMenuGroup(string[,] rParams)
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
                        string xSql = string.Empty;
                        xSql += " UPDATE t_menu_group SET ";
                        xSql += string.Format(" inquiry_yn = '{0}', ", rParams[i,2]);
                        xSql += string.Format(" edit_yn = '{0}', ", rParams[i,3]);
                        xSql += string.Format(" del_yn = '{0}', ", rParams[i,4]);
                        xSql += string.Format(" admin_yn = '{0}' ", rParams[i,5]);
                        xSql += string.Format(" WHERE menu_group = '{0}' ", rParams[i, 0]);
                        xSql += string.Format("   AND user_id = '{0}' ", rParams[i, 1]);

                        xCmdLMS.CommandText = xSql;
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
