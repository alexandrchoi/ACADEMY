using System;
using System.Collections.Generic;
using System.Text;

// 필수 using 문

using System.IO;
using System.Data;
using System.Web;
using System.Data.OracleClient;
using CLT.WEB.BIZ.FX.BIZBASE;
using CLT.WEB.BIZ.LMS.COMMON;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace CLT.WEB.BIZ.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : vp_c_textbook_md Class
    /// 
    /// 2. 주요기능 : 교재 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_c_textbook_md
    /// 
    /// 4. 작 업 자 : 김양도 / 2012.01.12
    /// </summary>
    public class vp_c_textbook_md : DAC
    {
        /************************************************************
        * Function name : GetTextBookInfoByID
        * Purpose       : TextBook_ID에 해당하는 TextBook 상세정보를 가져오는 처리
        * Input         : string[] rParams (0: textbook_id)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetTextBookInfoByID(string[] rParams)
        {
            try
            {
                string xSql = string.Format(@"SELECT t.textbook_id, t.textbook_nm, t.textbook_file_nm, t.textbook_intro,
                                  t.ins_id, t.ins_dt, t.send_dt, t.send_flg, t.use_flg, t.temp_save_flg,
                                  t.textbook_type, t.textbook_lang, t.course_group, t.course_field,
                                  t.publisher, t.author, t.price, t.textbook_desc,
                                  c1.d_knm as textbook_type_nm, c2.d_knm as textbook_lang_nm,
                                  c3.d_knm as course_group_nm, c4.d_knm as course_field_nm,
                                  TO_CHAR(t.pub_dt, 'YYYY.MM.DD') as pub_dt
                                  FROM t_textbook t
                                  INNER JOIN t_code_detail c1
                                  ON t.textbook_type = c1.d_cd
                                  INNER JOIN t_code_detail c2
                                  ON t.textbook_lang = c2.d_cd
                                  INNER JOIN t_code_detail c3
                                  ON t.textbook_lang = c3.d_cd
                                  INNER JOIN t_code_detail c4
                                  ON t.textbook_lang = c4.d_cd
                                  WHERE
                                  c1.m_cd='0049' AND c2.m_cd='0017' AND c3.m_cd='0003' AND c4.m_cd='0004'
                                  AND  t.textbook_id='{0}'", rParams[0]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
       * Function name : GetTextBookMixInfoOfCourse
       * Purpose       : 특정 (등록)과정의 과정&교재 정보를 가져오는 처리
       * Input         : string[] rParams (0: course_id)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetTextBookMixInfoOfCourse(string[] rParams)
        {
            try
            {
                string xSql = string.Format(@" SELECT c.course_nm, ct.textbook_seq, d1.d_knm as textbook_type, 
                                                     t.textbook_nm, t.publisher, t.author 
                                                FROM  t_course_textbook ct
                                                INNER JOIN t_textbook t
                                                ON ct.textbook_id = t.textbook_id
                                                inner join t_course c
                                                ON ct.course_id = c.course_id
                                                INNER JOIN t_code_detail d1
                                                ON t.textbook_type = d1.d_cd
                                                WHERE c.course_id = '{0}' AND d1.m_cd = '0049' ", rParams[0]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
        * Function name : SetTextBookInsert
        * Purpose       : TextBook 신규 항목을 등록하는 처리
        * Input         : object[] rParams (0: textbook_id, 1: rev_columns, 2: ins_id)
        * Output        : string
        *************************************************************/
        public string SetTextBookRevInsert(object[] rParams)
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

                        xSql = @" INSERT INTO t_textbook_rev
                                (
                                    rev_no, textbook_id, rev_columns, ins_id
                                )
                                VALUES(
                                    (SELECT NVL(MAX(REV_NO)+1, 1) FROM T_TEXTBOOK_REV where textbook_id = :textbook_id) 
                                    , :textbook_id, :rev_columns, :ins_id
                                )";

                        // rev_no (최대) 따는 처리 필요
                        // 현재는 테스트로 1로 집어 넣음.

                        xPara = new OracleParameter[3];
                        //xPara[0] = base.AddParam("rev_no", OracleType.VarChar, "1");
                        xPara[0] = base.AddParam("textbook_id", OracleType.VarChar, rParams[0]);
                        xPara[1] = base.AddParam("rev_columns", OracleType.VarChar, rParams[1]);
                        xPara[2] = base.AddParam("ins_id", OracleType.VarChar, rParams[2]);

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xPara, xTransLMS);

                        xRtn = Boolean.TrueString;

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
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return xRtn;//리터값
        }

        /************************************************************
        * Function name : SetTextBookInsert
        * Purpose       : TextBook 신규 항목을 등록하는 처리
        * Input         : object[] rParams (0: textbook_type, 1: textbook_lang, 2: course_group, 3: course_field, 
        *                 4: author, 5: price, 6: publisher, 7: textbook_desc, 8: textbook_intro, 9: textbook_nm
        *                 10: textbook_file, 11: textbook_filename, 12: Ins_ID, 13: send_flg) 
        * Output        : string
        *************************************************************/
        public string SetTextBookInsert(object[] rParams)
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

                        // 1. 파일 읽기

                        // 파일이 변경된 경우만, HttpPostedFile 객체가 넘겨져 온다.
                        byte[] xFileblob = null;                        

                        if (rParams[10] != null)
                        {
                            xFileblob = (byte[])rParams[10]; 
                            //HttpPostedFile xFile = (HttpPostedFile)rParams[10];
                            //int xFileLen = xFile.ContentLength; // 파일 사이즈

                            //byte[] xFileData = new byte[xFileLen]; // 파일 사이즈만큼 byte 배열 설정
                            //xFile.InputStream.Read(xFileData, 0, xFileLen); // 스트림에서 파일 읽어 바이트 배열에 담는 처리

                            //string[] xFileNameSector = xFile.FileName.Split(new char[] { '\\' });
                            //xFileName = xFileNameSector[xFileNameSector.Length - 1];

                            //xFileblob = xFileData; 
                        }
                        else
                        {
                            xSql = " SELECT  TEXTBOOK_FILE FROM t_textbook WHERE TEXTBOOK_ID = '" + rParams[15] + "' ";
                            xCmdLMS.CommandText = xSql;
                            object xobj = base.ExecuteScalar(db, xCmdLMS, xTransLMS);
                            xFileblob = (byte[])xobj; 
                        }

                        bool xUpdate = false; //update 할지 여부 
                        string xQID = string.Empty;

                        if (rParams[14].ToString() == "Y")
                        {
                            //subject id를 확인 하여 temp_save_flg = 'Y' 이면 update
                            //아니면 insert 하기                             
                            xSql = " SELECT TEMP_SAVE_FLG FROM t_textbook WHERE TEXTBOOK_ID = '" + rParams[15] + "' ";
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
                            xQID = rParams[15].ToString();

                            xSql = @"
                                UPDATE T_TEXTBOOK SET
                                  TEXTBOOK_TYPE = :TEXTBOOK_TYPE
                                , TEXTBOOK_LANG = :TEXTBOOK_LANG
                                , COURSE_GROUP = :COURSE_GROUP
                                , COURSE_FIELD = :COURSE_FIELD
                                , AUTHOR = :AUTHOR
                                , PRICE = :PRICE
                                , PUBLISHER = :PUBLISHER
                                , TEXTBOOK_DESC = :TEXTBOOK_DESC
                                , TEXTBOOK_INTRO = :TEXTBOOK_INTRO
                                , TEXTBOOK_NM = :TEXTBOOK_NM
                                , TEXTBOOK_FILE = :TEXTBOOK_FILE
                                , TEXTBOOK_FILE_NM = :TEXTBOOK_FILE_NM
                                , UPT_ID = :INS_ID
                                , UPT_DT = SYSDATE
                                , PUB_DT = :PUB_DT
                                WHERE TEXTBOOK_ID = :TEXTBOOK_ID
                                ";

                            xPara = new OracleParameter[15];
                            xPara[0] = base.AddParam("textbook_id", OracleType.VarChar, rParams[15]);
                            xPara[1] = base.AddParam("textbook_type", OracleType.VarChar, rParams[0]);
                            xPara[2] = base.AddParam("textbook_lang", OracleType.VarChar, rParams[1]);
                            xPara[3] = base.AddParam("course_group", OracleType.VarChar, rParams[2]);
                            xPara[4] = base.AddParam("course_field", OracleType.VarChar, rParams[3]);

                            xPara[5] = base.AddParam("author", OracleType.VarChar, rParams[4]);
                            xPara[6] = base.AddParam("price", OracleType.VarChar, rParams[5]);
                            xPara[7] = base.AddParam("publisher", OracleType.VarChar, rParams[6]);
                            xPara[8] = base.AddParam("textbook_desc", OracleType.Clob, rParams[7]);
                            xPara[9] = base.AddParam("textbook_intro", OracleType.Clob, rParams[8]);
                            xPara[10] = base.AddParam("textbook_nm", OracleType.VarChar, rParams[9]);

                            //xPara[11] = base.AddParam("textbook_file", OracleType.Blob, rParams[10]);
                            xPara[11] = base.AddParam("textbook_file", OracleType.Blob, xFileblob);
                            xPara[12] = base.AddParam("textbook_file_nm", OracleType.VarChar, rParams[11]);
                            xPara[13] = base.AddParam("ins_id", OracleType.VarChar, rParams[12]);

                            xPara[14] = base.AddParam("pub_dt", OracleType.VarChar, rParams[16]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);

                            xRtn = xQID; 
                        }
                        else
                        {

                            // 2. DB 저장 (DB 작업 중에 문제발생 시, 저장된 파일 제거)
                            xSql = @" INSERT INTO t_textbook
                                (
                                    textbook_id, textbook_type, textbook_lang, course_group, 
                                    course_field, author, price, publisher, textbook_desc, textbook_intro, 
                                    textbook_nm, textbook_file, textbook_file_nm, ins_id, INS_DT, UPT_ID, UPT_DT, send_flg
                                    , TEMP_SAVE_FLG , PUB_DT 
                                )
                                VALUES(
                                    :textbook_id, :textbook_type, :textbook_lang, :course_group, 
                                    :course_field, :author, :price, :publisher, :textbook_desc, :textbook_intro, 
                                    :textbook_nm, :textbook_file, :textbook_file_nm, :ins_id, SYSDATE, :INS_ID, SYSDATE, :send_flg
                                    , :TEMP_SAVE_FLG , :PUB_DT 
                                )";

                            vp_l_common_md com = new vp_l_common_md();
                            xQID = com.GetMaxIDOfTable(new string[] { "t_textbook", "textbook_id" });                            

                            xPara = new OracleParameter[17];
                            xPara[0] = base.AddParam("textbook_id", OracleType.VarChar, xQID);
                            xPara[1] = base.AddParam("textbook_type", OracleType.VarChar, rParams[0]);
                            xPara[2] = base.AddParam("textbook_lang", OracleType.VarChar, rParams[1]);
                            xPara[3] = base.AddParam("course_group", OracleType.VarChar, rParams[2]);
                            xPara[4] = base.AddParam("course_field", OracleType.VarChar, rParams[3]);

                            xPara[5] = base.AddParam("author", OracleType.VarChar, rParams[4]);
                            xPara[6] = base.AddParam("price", OracleType.VarChar, rParams[5]);
                            xPara[7] = base.AddParam("publisher", OracleType.VarChar, rParams[6]);
                            xPara[8] = base.AddParam("textbook_desc", OracleType.Clob, rParams[7]);
                            xPara[9] = base.AddParam("textbook_intro", OracleType.Clob, rParams[8]);
                            xPara[10] = base.AddParam("textbook_nm", OracleType.VarChar, rParams[9]);

                            //xPara[11] = base.AddParam("textbook_file", OracleType.Blob, rParams[10]);
                            xPara[11] = base.AddParam("textbook_file", OracleType.Blob, xFileblob);
                            xPara[12] = base.AddParam("textbook_file_nm", OracleType.VarChar, rParams[11]);
                            xPara[13] = base.AddParam("ins_id", OracleType.VarChar, rParams[12]);
                            xPara[14] = base.AddParam("send_flg", OracleType.VarChar, rParams[13]);
                            xPara[15] = base.AddParam("TEMP_SAVE_FLG", OracleType.VarChar, rParams[14]);
                            xPara[16] = base.AddParam("PUB_DT", OracleType.VarChar, rParams[16]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);

                            xRtn = xQID; 
                        }

                        //temp save  아닐 경우선박으로 발송
                        if (rParams[14].ToString() == "N")
                        {
                            OracleParameter[] oOraParams = null; 
                            oOraParams = new OracleParameter[2];
                            oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_TEXTBOOK");
                            oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_TEXTBOOK");
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
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return xRtn;//리터값

        }

        /************************************************************
       * Function name : GetTextBookInfo
       * Purpose       : 교재 전체 목록을 전달하는 처리
       * Input         : string[] rParams (0: pagesize, 1: currentPageIndex)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetTextBookInfo(string[] rParams)
        {
            try
            {
                string xSql = " SELECT * FROM ";
                xSql += "          ( SELECT rownum rnum, b.* FROM  ";
                xSql += "               ( ";
                xSql += "                   SELECT t.textbook_id, t.textbook_nm, t.author, t.publisher, ";
                xSql += "                   t.price, t.ins_id, t.ins_dt, t.use_flg, u.user_nm_kor, ";
                xSql += "                   d1.d_knm as textbook_type, d2.d_knm as textbook_lang, count(*) over() totalrecordcount ";
                xSql += "                       FROM t_textbook t ";
                xSql += "                       INNER JOIN t_code_detail d1 ";
                xSql += "                       ON t.textbook_type = d1.d_cd ";
                xSql += "                       INNER JOIN t_code_detail d2 ";
                xSql += "                       ON t.textbook_lang = d2.d_cd ";
                xSql += "                       INNER JOIN t_user u ";
                xSql += "                       ON t.ins_id = u.user_id ";
                xSql += "                       WHERE d1.m_cd='0049' and d2.m_cd='0017' ";
                xSql += "                       ORDER BY t.ins_dt DESC ";
                xSql += "               ) b ";
                xSql += "          ) ";
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
        * Function name : GetTextBookInfoByCondition
        * Purpose       : 조회조건에 따른 교재 전체 목록을 전달하는 처리
        * Input         : string[] rParams (0: pagesize, 1: currentPageIndex, 2: textbook_nm, 3: textbook_type
        *                 4: course_group, 5: course_field, 6: textbook_lang, 7: ins_begin_dt, 8: ins_end_dt)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetTextBookInfoByCondition(string[] rParams)
        {
            try
            {
                string xSql = " SELECT * FROM ";
                xSql += "           (  SELECT rownum rnum, b.* FROM  ";
                xSql += "               (  ";
                xSql += "                   SELECT t.textbook_id, t.textbook_nm, t.author, t.publisher, ";
                xSql += "                   t.price, t.ins_id, t.ins_dt, t.use_flg, u.user_nm_kor, ";
                xSql += "                   d1.d_knm as textbook_type, d2.d_knm as textbook_lang, count(*) over() totalrecordcount ";
                xSql += "                       FROM t_textbook t ";
                xSql += "                       INNER JOIN t_code_detail d1 ";
                xSql += "                       ON t.textbook_type = d1.d_cd ";
                xSql += "                       INNER JOIN t_code_detail d2 ";
                xSql += "                       ON t.textbook_lang = d2.d_cd ";
                xSql += "                       INNER JOIN t_user u ";
                xSql += "                       ON t.ins_id = u.user_id ";
                // 조회조건 추가 <START>
                xSql += "                       WHERE ";
                xSql += "                       d1.m_cd='0049' AND d2.m_cd='0017' ";

                if (!string.IsNullOrEmpty(rParams[2]))
                    xSql += string.Format("   AND  lower(t.textbook_nm) like '%{0}%' ", rParams[2].ToLower().Replace("'", "''")); // textbook_nm
                if (!string.IsNullOrEmpty(rParams[3]))
                    xSql += string.Format("   AND  t.textbook_type like '%{0}%' ", rParams[3].ToLower().Replace("'", "''")); // textbook_type
                if (!string.IsNullOrEmpty(rParams[4]))
                    xSql += string.Format("   AND  t.course_group like '%{0}%' ", rParams[4].ToLower().Replace("'", "''")); // course_group
                if (!string.IsNullOrEmpty(rParams[5]))
                    xSql += string.Format("   AND  t.course_field like '%{0}%' ", rParams[5].ToLower().Replace("'", "''")); // course_field
                if (!string.IsNullOrEmpty(rParams[6]))
                    xSql += string.Format("   AND  t.textbook_lang like '%{0}%' ", rParams[6].ToLower().Replace("'", "''")); // textbook_lang
                if ((!string.IsNullOrEmpty(rParams[7])) && (!string.IsNullOrEmpty(rParams[8])))
                    xSql += string.Format("   AND ( to_char(t.ins_dt, 'yyyy.MM.dd') >= '{0}' AND to_char(t.ins_dt, 'yyyy.MM.dd') <= '{1}' ) ", rParams[7], rParams[8]); // ins_begin_dt, ins_end_dt

                // 조회조건 추가 <END>
                xSql += "                       ORDER BY t.ins_dt DESC ";
                xSql += "               ) b ";
                xSql += "           ) ";
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
        * Function name : GetTextBookInfoForExcel
        * Purpose       : Excel 표준에 맞는 교재 전체 목록을 전달하는 처리
        * Input         : string[] rParams (0: textbook_nm, 1: textbook_type
        *                 2: course_group, 3: course_field, 4: textbook_lang, 5: ins_begin_dt, 6: ins_end_dt)
        * Output        : DataTable
        *************************************************************/
        public DataTable GetTextBookInfoForExcel(string[] rParams)
        {
            try
            {
                //string xSql = " SELECT t.textbook_id, t.textbook_nm, t.author, t.publisher, ";
                //xSql += "       t.price, t.ins_id, t.ins_dt, t.use_flg, u.user_nm_kor, ";
                //xSql += "       d1.d_knm as textbook_type, d2.d_knm as textbook_lang ";
                //xSql += "           FROM t_textbook t ";
                //xSql += "           INNER JOIN t_code_detail d1 ";
                //xSql += "           ON t.textbook_type = d1.d_cd ";
                //xSql += "           INNER JOIN t_code_detail d2 ";
                //xSql += "           ON t.textbook_lang = d2.d_cd ";
                //xSql += "           INNER JOIN t_user u ";
                //xSql += "           ON t.ins_id = u.user_id ";
                //// 조회조건 추가 <START>
                //xSql += "           WHERE ";
                //xSql += "           d1.m_cd='0049' AND d2.m_cd='0017' ";

                //if (!string.IsNullOrEmpty(rParams[0]))
                //    xSql += string.Format("   AND  lower(t.textbook_nm) like '%{0}%' ", rParams[0].ToLower()); // textbook_nm
                //if (!string.IsNullOrEmpty(rParams[1]))
                //    xSql += string.Format("   AND  t.contents_type like '%{0}%' ", rParams[1].ToLower()); // textbook_type
                //if (!string.IsNullOrEmpty(rParams[2]))
                //    xSql += string.Format("   AND  t.course_group like '%{0}%' ", rParams[2].ToLower()); // course_group
                //if (!string.IsNullOrEmpty(rParams[3]))
                //    xSql += string.Format("   AND  t.course_field like '%{0}%' ", rParams[3].ToLower()); // course_field
                //if (!string.IsNullOrEmpty(rParams[4]))
                //    xSql += string.Format("   AND  t.contents_lang like '%{0}%' ", rParams[4].ToLower()); // textbook_lang
                //if ((!string.IsNullOrEmpty(rParams[5])) && (!string.IsNullOrEmpty(rParams[6])))
                //    xSql += string.Format("   AND ( to_char(t.ins_dt, 'yyyy.MM.dd') >= '{0}' AND to_char(t.ins_dt, 'yyyy.MM.dd') <= '{1}' ) ", rParams[5], rParams[6]); // ins_begin_dt, ins_end_dt

                //// 조회조건 추가 <END>
                //xSql += "                       ORDER BY t.ins_dt DESC ";

                //return base.ExecuteDataTable("LMS", xSql);

                string xSql = " SELECT * FROM ";
                xSql += "           (  SELECT rownum rnum, b.* FROM  ";
                xSql += "               (  ";
                xSql += "                   SELECT ";

                xSql += "                   d1.d_knm as textbook_type, t.textbook_nm, t.author, t.publisher ";
                xSql += "                   , d2.d_knm as textbook_lang , u.user_nm_kor "; 
                xSql += "                   , TO_CHAR(t.ins_dt, 'YYYY.MM.DD') AS INS_DT , t.use_flg "; 

                //xSql += "                   t.textbook_id, t.textbook_nm, t.author, t.publisher, ";
                //xSql += "                   t.price, t.ins_id, t.ins_dt, t.use_flg, u.user_nm_kor, ";
                //xSql += "                   d1.d_knm as textbook_type, d2.d_knm as textbook_lang, count(*) over() totalrecordcount ";
                xSql += "                       FROM t_textbook t ";
                xSql += "                       INNER JOIN t_code_detail d1 ";
                xSql += "                       ON t.textbook_type = d1.d_cd ";
                xSql += "                       INNER JOIN t_code_detail d2 ";
                xSql += "                       ON t.textbook_lang = d2.d_cd ";
                xSql += "                       INNER JOIN t_user u ";
                xSql += "                       ON t.ins_id = u.user_id ";
                // 조회조건 추가 <START>
                xSql += "                       WHERE ";
                xSql += "                       d1.m_cd='0049' AND d2.m_cd='0017' ";

                if (!string.IsNullOrEmpty(rParams[2]))
                    xSql += string.Format("   AND  lower(t.textbook_nm) like '%{0}%' ", rParams[2].ToLower().Replace("'", "''")); // textbook_nm
                if (!string.IsNullOrEmpty(rParams[3]))
                    xSql += string.Format("   AND  t.textbook_type like '%{0}%' ", rParams[3].ToLower().Replace("'", "''")); // textbook_type
                if (!string.IsNullOrEmpty(rParams[4]))
                    xSql += string.Format("   AND  t.course_group like '%{0}%' ", rParams[4].ToLower().Replace("'", "''")); // course_group
                if (!string.IsNullOrEmpty(rParams[5]))
                    xSql += string.Format("   AND  t.course_field like '%{0}%' ", rParams[5].ToLower().Replace("'", "''")); // course_field
                if (!string.IsNullOrEmpty(rParams[6]))
                    xSql += string.Format("   AND  t.textbook_lang like '%{0}%' ", rParams[6].ToLower().Replace("'", "''")); // textbook_lang
                if ((!string.IsNullOrEmpty(rParams[7])) && (!string.IsNullOrEmpty(rParams[8])))
                    xSql += string.Format("   AND ( to_char(t.ins_dt, 'yyyy.MM.dd') >= '{0}' AND to_char(t.ins_dt, 'yyyy.MM.dd') <= '{1}' ) ", rParams[7], rParams[8]); // ins_begin_dt, ins_end_dt

                // 조회조건 추가 <END>
                xSql += "                       ORDER BY t.ins_dt DESC ";
                xSql += "               ) b ";
                xSql += "           ) ";

                //xSql += string.Format(" WHERE  rnum > {0} ", Convert.ToInt32(rParams[0]) * (Convert.ToInt32(rParams[1]) - 1));
                //xSql += string.Format(" AND    rnum <= {0} ", Convert.ToInt32(rParams[0]) * Convert.ToInt32(rParams[1]));

                return base.ExecuteDataTable("LMS", xSql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
