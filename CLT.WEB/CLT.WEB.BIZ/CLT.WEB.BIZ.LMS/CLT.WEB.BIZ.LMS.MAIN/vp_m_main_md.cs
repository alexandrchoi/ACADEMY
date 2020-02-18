using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

// 필수 using 문

using System.Data;
using System.Data.OracleClient;
using CLT.WEB.BIZ.FX.BIZBASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace CLT.WEB.BIZ.LMS.MAIN
{
    /// <summary>
    /// 1. 작업개요 : vp_m_main_md Class
    /// 
    /// 2. 주요기능 : Main 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_m_main_md
    /// 
    /// 4. 작 업 자 : 김양도 / 2011.12.08
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_m_main_md
    ///        * Comment 
    ///          * Comment 
    ///          개설된 과정 중에 최근 N개의 데이터를 현재기준 수강신청 가능일자 포함된는 과정만 조회
    ///          영문조회시 과정명 영문화 처리
    /// </summary>
    public class vp_m_main_md : DAC
    {
        /************************************************************
        * Function name : GetAllMenuInfo
        * Purpose       : User가 속한 User_Group에 해당하는 전체 메뉴 정보를 가져오는 처리
        * Input         : string[] rParams (0:user_id => user가 속한 user_group_id )
        * Output        : DataTable
        *************************************************************/
        public DataTable GetAllMenuInfo(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                
                string xSql = string.Format(@" SELECT m.menu_group, m.menu_nm_kor, m.menu_nm_eng, m.path,
                                                 CASE WHEN (substr(m.menu_group,3,1)) != '0' THEN m.menu_nm_kor
                                                  END AS menu3
                                                    , m.seq 
                                                 From t_menu m
                                                 INNER JOIN t_menu_group g
                                                 ON m.menu_group = g.menu_group
                                                 WHERE g.user_id='{0}' and inquiry_yn = 'Y' 
                                                 ORDER BY m.seq ASC ", rParams[0]);

                xDt = base.ExecuteDataTable("LMS", xSql);

                // 사용자가 조회권한을 가진 소메뉴만 조회한다.
                DataTable xDtPath = null;
                xSql = string.Empty;
                xSql += " SELECT mgroup.menu_group, tmenu.path ";
                xSql += "   FROM t_menu tmenu, t_menu_group mgroup ";
                xSql += "  WHERE tmenu.menu_group = mgroup.menu_group ";
                xSql += "    AND mgroup.inquiry_yn = 'Y' ";
                xSql += string.Format("AND mgroup.user_id = '{0}'", rParams[0]);
                xSql += " AND substr(tmenu.menu_group, 3, 1) != '0' ";
                xSql += " ORDER BY mgroup.menu_group ASC ";


                xDtPath = base.ExecuteDataTable("LMS", xSql);
                DataRow[] xSub = null;

                string xCode = string.Empty;
                int nCount = 0;
                
                /*
                foreach (DataRow xDrs in xDt.Rows)
                {
                    if (xDrs["menu_group"].ToString().Substring(1, 1) == "0") // 대메뉴이면...
                        xCode = xDrs["menu_group"].ToString().Substring(0, 1);
                    else if (xDrs["menu_group"].ToString().Substring(1, 1) != "0") // 중메뉴이면...
                        xCode = xDrs["menu_group"].ToString().Substring(0, 2);


                    xSub = xDtPath.Select(string.Format("menu_group LIKE '{0}%' ", xCode));
                    if (xSub.Length > 0)
                        xDt.Rows[nCount]["path"] = xSub[0]["path"].ToString();
                    
                    nCount++;
                }
                */
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }

        /************************************************************
        * Function name : GetMenuInfoByTopCode
        * Purpose       : User가 속한 User_Group에 해당하는 전체 메뉴 중에서

        *                 넘겨 받은 대분류 메뉴에 해당하는 메뉴 정보를 가져오는 처리
        * Input         : string[] rParams (0: menu_gruop, 1:user_id => user가 속한 user_group_id) )
        * Output        : DataTable
        *************************************************************/
        public DataTable GetMenuInfoByTopCode(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Format(@" SELECT * FROM
                (
                    SELECT 
                    m.menu_group, m.menu_nm_kor, m.menu_nm_eng, m.path,
                    g.inquiry_yn, g.edit_yn, g.admin_yn, g.del_yn,
                    g.inquiry_yn || g.edit_yn || g.admin_yn || g.del_yn as auth,
                    CASE WHEN (substr(m.menu_group,3,1)) != '0' THEN m.menu_nm_kor
                     END AS menu3
                    , m.seq 
                    FROM t_menu m
                    INNER JOIN t_menu_group g
                    ON m.menu_group = g.menu_group
                    WHERE substr(m.menu_group, 1,1) = '{0}' AND g.user_id ='{1}'
                    ORDER BY seq asc
                )
                WHERE auth LIKE '%Y%'", rParams[0], rParams[1]);

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
       * Function name : GetIsAuthority
       * Purpose       : 사용자 그룹(User_Group)과 메뉴 코드(Menu_Code)로 특정
       *                 사용자가 메뉴에 대한 권한 확인
       * Input         : string[] rParams (0: user_id, 1:menu_group)
       * Output        : string
       *************************************************************/
        public string GetIsAuthority(string rUserId, string rMenuGroup)
        {
            string xRtn = null;
            try
            {
                string xSql = null;
                xSql = " SELECT ";
                xSql += " inquiry_yn ";
                xSql += "FROM ";
                xSql += " t_menu_group ";
                xSql += "WHERE ";
                xSql += " user_id = '" + rUserId + "' ";
                xSql += " AND menu_group = '" + rMenuGroup + "' ";

                xRtn = Convert.ToString(base.ExecuteScalar("LMS", xSql));
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xRtn;
        }


        /************************************************************
        * Function name : GetRecentOpenCourseInfoCountBy
        * Purpose       : 개설된 과정 중에 최근 N개의 데이터를 GridView에 바인딩을 위한 처리
        * Input         : string[] rParams (0: count)
        * Output        : DataTable
        *************************************************************/
        public DataSet GetRecentOpenCourseInfoCountBy(string[] rParams, CultureInfo rArgCultureInfo)
        {
            Database db = null;
            DataSet xDs = new DataSet();
            string xSql = string.Empty;

            try
            {
                db = base.GetDataBase("LMS");

                DataTable xDt = null;
                //로그인 하지 않았을 경우
                string xWhere = "";

                //-------------------------------------------------------------------------------------------------------------
                if (rParams[3] != "000001")
                {
                    xWhere = " and company_kind IN ('000000','000002') ";
                    if (rParams[2] == "000001")      //그룹사

                        xWhere = " and company_kind IN ('000000','000001') ";
                    else if (rParams[2] == "000002") //사업주위탁

                        xWhere = " and company_kind IN ('000000','000002') ";
                }
                xWhere += " and NOT_KIND = '000001' ";

                xSql = @"
                    SELECT *
                      FROM (
                            SELECT NOT_NO
                                    , NOT_SUB
                                    , TO_CHAR(INS_DT,'YYYY.MM.DD') AS NOTICE_BEGIN_DT
                                    , (case when INS_DT >= SYSDATE-" + rParams[1] + @" then 'Y' else 'N' end) AS NEW_ICON
                              FROM T_NOTICE
                             WHERE DEL_YN = 'N'
                               AND 1=1 --notice_end_dt >= SYSDATE
                                   " + xWhere + @"
                             ORDER BY INS_DT DESC
                           ) N
                     WHERE ROWNUM <= " + rParams[0] + " ";
                xDt = base.ExecuteDataTable(db, xSql);
                base.MergeTable(ref xDs, xDt, "table1");




                //-------------------------------------------------------------------------------------------------------------
                xWhere = "";
                if (rParams[3] != "000001")
                {
                    xWhere = " and company_kind IN ('000000','000002') ";
                    if (rParams[2] == "000001")      //그룹사

                        xWhere = " and company_kind IN ('000000','000001') ";
                    else if (rParams[2] == "000002") //사업주위탁

                        xWhere = " and company_kind IN ('000000','000002') ";
                }
                xWhere += " and NOT_KIND = '000002' ";

                xSql = @"
                    SELECT *
                      FROM (
                            SELECT NOT_NO
                                    , NOT_SUB
                                    , TO_CHAR(INS_DT,'YYYY.MM.DD') AS NOTICE_BEGIN_DT
                                    , (case when INS_DT >= SYSDATE-" + rParams[1] + @" then 'Y' else 'N' end) AS NEW_ICON
                              FROM T_NOTICE
                             WHERE DEL_YN = 'N'
                               AND 1=1 --notice_end_dt >= SYSDATE
                                   " + xWhere + @"
                             ORDER BY INS_DT DESC
                           ) N
                     WHERE ROWNUM <= " + rParams[0] + " ";
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
        * Function name : GetCheckMenuInfo
        * Purpose       : User가 접속 할 수 있는 권한을 가진 메뉴를 조회
        * Input         : string[] rParams (0:user_id => user가 속한 user_group_id )
        * Output        : DataTable
        *************************************************************/
        public DataTable GetCheckMenuInfo(string[] rParams)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT mgroup.menu_group, mgroup.inquiry_yn, menu.path ";
                xSql += "   FROM t_menu_group mgroup, t_menu menu ";
                xSql += "  WHERE mgroup.menu_group = menu.menu_group ";
                xSql += string.Format(" AND mgroup.user_id =  '{0}' ", rParams[0]);
                xSql += string.Format(" AND mgroup.menu_group LIKE '{0}%' ", rParams[1]);
                xSql += " AND substr(mgroup.MENU_GROUP, 3, 1)  != '0' ";
                xSql += " ORDER BY mgroup.MENU_GROUP ASC ";

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
        * Function name : GetEduList
        * Purpose       : 
        * Input         : 
        * Output        : DataTable
        *************************************************************/
        public DataTable GetEduList(string[] rParams, CultureInfo rArgCultureInfo)
        {
            DataTable xDt = null;
            try
            {
                string xSql = string.Empty;
                
                xSql = " SELECT * FROM ( ";
                xSql += "   SELECT OC.open_course_id            AS \"id\" ";
                xSql += "        , OC.course_id                 AS \"cid\" ";
                xSql += "        , TO_CHAR(OC.course_seq)       AS \"seq\" ";

                xSql += "        , DECODE(TO_CHAR(OC.course_begin_dt, 'YYYYMMDD'),  TO_CHAR(OC.course_end_dt, 'YYYYMMDD') ";
                xSql += "                                                        ,  '' ";
                xSql += "                                                        ,  TO_CHAR(OC.course_begin_dt, 'MM/DD') || ' ~ ' || TO_CHAR(OC.course_end_dt, 'MM/DD') ";
                xSql += "          || '　　') || ";

                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += " BC.course_nm ";
                }
                else
                {
                    xSql += " BC.course_nm_abbr ";
                }
                xSql += "                                       AS \"title\" ";
                xSql += "        , TO_CHAR(OC.course_begin_dt, 'YYYY-MM-DD') ||" +
                    "              DECODE(TO_CHAR(OC.course_begin_dt, 'HH24:MI'), '00:00', '', TO_CHAR(OC.course_begin_dt, 'HH24:MI')) " +
                    "                                           AS \"start\"";
                xSql += "        , DECODE( TO_CHAR(OC.course_begin_dt, 'YYYYMMDD'),  TO_CHAR(OC.course_end_dt, 'YYYYMMDD'), TO_CHAR(OC.course_end_dt, 'YYYY-MM-DD'), TO_CHAR(OC.course_end_dt+1, 'YYYY-MM-DD') ) ||" +
                    "              DECODE(TO_CHAR(OC.course_end_dt, 'HH24:MI'), '00:00', '', TO_CHAR(OC.course_end_dt, 'HH24:MI')) " +
                    "                                           AS \"end\"";
                xSql += "        , BC.course_intro              AS \"description\" ";
                xSql += "        , BC.course_objective          AS \"objective\" ";
                xSql += "        , NVL(OC.course_place, ' ')    AS \"place\" ";
                xSql += "        , OC.course_inout              AS \"inout\" ";
                xSql += "        , OC.educational_org           AS \"org\" ";
                xSql += "        , 'edu'                        AS \"type\" ";
                xSql += "        , DECODE(TO_CHAR(OC.course_begin_dt, 'YYYYMMDD'),  TO_CHAR(OC.course_end_dt, 'YYYYMMDD'), 'false', 'true')";
                xSql += "                                       AS \"allDay\" ";

                xSql += "        , (SELECT approval_flg ";
                xSql += "           FROM t_course_result ";
                xSql += "           WHERE open_course_id = OC.open_course_id ";
                xSql += string.Format("           AND user_id = '{0}') \"approval_code\" ", rParams[1]);  // 사용자 ID

                Random random = new Random();
                string[] vArr = new string[] { "D25565", "9775fa", "ffa94d", "74c0fc", "f06595", "63e6be", "a9e34b", "4d638c", "495057" };
                string xColor = vArr[random.Next(vArr.Length)];
                
                xSql += "        , '#"+ xColor + "'             AS \"backgroundColor\" ";
                //xSql += "        , '#74c0fc'                    AS \"backgroundColor\" ";
                xSql += "        , '#ffffff'                    AS \"textColor\" ";

                xSql += "         ";
                //xSql += ", TO_CHAR(OC.ins_dt, 'YYYY') || '-' || TO_CHAR(OC.course_seq) course_seq, ";
                //xSql += "          TO_CHAR(OC.course_begin_apply_dt, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(OC.course_end_apply_dt, 'YYYY.MM.DD') apply_date, ";
                //xSql += "          TO_CHAR(OC.course_begin_dt, 'YYYY.MM.DD') || ' ~ ' || TO_CHAR(OC.course_end_dt, 'YYYY.MM.DD') course_Date, ";
                //xSql += "          (OC.course_end_dt - OC.course_begin_dt) +1 course_day, ";
                //xSql += "          count(*) over() totalrecordcount, ";
                //xSql += "          OC.open_course_id ";
                xSql += "   FROM t_open_course  OC ";
                xSql += "   INNER JOIN t_course BC ";
                xSql += "   ON OC.course_id = BC.course_id ";

                xSql += " AND BC.use_flg = 'Y' "; // 사용유무가 Y 인것
                xSql += " AND OC.use_flg = 'Y' "; // 사용유무가 Y 인것
                
                if (!string.IsNullOrEmpty(rParams[0]))
                {
                    xSql += string.Format("   AND ( (TO_DATE('{0}','YYYY-MM') <= OC.course_begin_dt AND LAST_DAY(TO_DATE('{0}','YYYY-MM')) + 1 >  OC.course_end_dt) ", rParams[0]);
                    xSql += string.Format("       OR(TO_DATE('{0}','YYYY-MM') >  OC.course_begin_dt AND LAST_DAY(TO_DATE('{0}','YYYY-MM')) + 1 <= OC.course_end_dt) ", rParams[0]);
                    xSql += string.Format("       OR TO_DATE('{0}','YYYY-MM') BETWEEN OC.course_begin_dt AND OC.course_end_dt ", rParams[0]);
                    xSql += string.Format("       OR TO_DATE(TO_CHAR(LAST_DAY(TO_DATE('{0}','YYYY-MM')),'YYYY-MM-DD') ||' 235959', 'YYYY-MM-DD HH24MISS') BETWEEN OC.course_begin_dt AND OC.course_end_dt ) ", rParams[0]);
                }
                else
                {
                    xSql += "                 AND OC.course_end_dt >= SYSDATE - 1200 ";
                }

                //////////////////////////////////////////////////////////////////////////////////////////////////////////
                // UNION ALL 교육안내
                xSql += " UNION ALL ";
                xSql += " SELECT '10000001'                                \"id\", ";
                xSql += "        NVL(notice.open_course_id, '10000001')    \"cid\", ";
                xSql += "        TO_CHAR(notice.not_no)                    \"seq\", ";
                xSql += "        notice.not_sub                            \"title\", ";
                xSql += "        TO_CHAR(notice.ins_dt, 'YYYY-MM-DD')      \"start\", ";
                xSql += "        TO_CHAR(notice.ins_dt, 'YYYY-MM-DD')      \"end\", ";
                xSql += "        notice.not_sub                            \"description\", ";
                xSql += "        TO_CHAR(DBMS_LOB.SUBSTR(notice.not_content, 500, 1)) \"objective\", ";
                xSql += "        DECODE(TO_CHAR(notice_begin_dt, 'YYYYMMDD'), TO_CHAR(notice_end_dt, 'YYYYMMDD'), '' , TO_CHAR(notice_begin_dt, 'MM/DD') || ' ~ ' || TO_CHAR(notice_end_dt, 'MM/DD')) \"place\", ";
                xSql += "        to_char(hit_cnt)                          \"inout\", ";
                xSql += "        notice.company_kind                       \"org\", ";
                xSql += "        'notice'                                  \"type\", ";
                xSql += "        'true'                                    \"allDay\", ";
                xSql += "        tuser.user_nm_kor                         \"approval_code\", ";
                xSql += "        '#"+ xColor + "'                          \"backgroundColor\", ";
                //xSql += "        '#74c0fc'                    AS \"backgroundColor\" ";
                xSql += "        '#ffffff'                                 \"textColor\" ";
                xSql += "   FROM t_notice notice, t_user tuser ";
                xSql += "  WHERE notice.ins_id = tuser.user_id ";
                if (rParams[2] == "000007" || rParams[2] == "000008" || rParams[2] == "000009") // 법인사 관리자, 법인사 수강자, 손님일경우
                {
                    xSql += " AND notice.company_kind IN ('000000','000002') ";
                }
                else
                {
                    xSql += " AND notice.company_kind IN ('000000','000001') ";
                }
                if (rParams[2] != "000001" && rParams[2] != "000002" && rParams[2] != "000003") // 관리자, 행정담당, 교관
                {
                    xSql += " AND notice.notice_end_dt >= SYSDATE";
                }
                xSql += "     AND notice.not_kind = '000002' ";
                xSql += "     AND del_yn = 'N' ";

                xSql += string.Format(" AND notice.ins_dt BETWEEN LAST_DAY(ADD_MONTHS(TO_DATE('{0}', 'YYYY-MM'), -1))+1 AND TO_DATE(TO_CHAR(LAST_DAY(TO_DATE('{0}', 'YYYY-MM')), 'YYYY-MM-DD') ||' 235959', 'YYYY-MM-DD HH24MISS')", rParams[0]);

                xSql += " ) ";
                xSql += " ORDER BY 1,2,3,4 ";
                //////////////////////////////////////////////////////////////////////////////////////////////////////////

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
            return xDt;
        }
    }
}
