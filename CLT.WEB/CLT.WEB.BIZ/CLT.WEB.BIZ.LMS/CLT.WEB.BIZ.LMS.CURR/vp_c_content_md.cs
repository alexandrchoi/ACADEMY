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
using System.Configuration;
using System.Web.Security; 


namespace CLT.WEB.BIZ.LMS.CURR
{
    /// <summary>
    /// 1. 작업개요 : vp_c_content_md Class
    /// 
    /// 2. 주요기능 : 컨텐츠 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_c_content_md
    /// 
    /// 4. 작 업 자 : 김양도 / 2011.12.08
    /// </summary>
    public class vp_c_content_md : DAC
    {
        /************************************************************
       * Function name : GetContentsInfo
       * Purpose       : 컨텐츠 전체 목록을 전달하는 처리
       * Input         : string[] rParams (0: pagesize, 1: currentPageIndex)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetContentsInfo(string[] rParams)
        {
            try
            {
                string xSql = " SELECT * FROM ";
                xSql += "          ( SELECT rownum rnum, b.* FROM  ";
                xSql += "               ( ";
                xSql += "                   SELECT t.contents_id, t.contents_nm, t.contents_file_nm, t.contents_remark, ";
                xSql += "                   t.ins_id, t.ins_dt, t.send_dt, t.send_flg, t.use_flg, t.temp_save_flg, ";
                xSql += "                   c1.d_knm as contents_type, c2.d_knm as contents_lang, count(*) over() totalrecordcount ";
                xSql += "                       FROM t_contents t ";
                xSql += "                       INNER JOIN t_code_detail c1 ";
                xSql += "                       ON t.contents_type = c1.d_cd ";
                xSql += "                       INNER JOIN t_code_detail c2 ";
                xSql += "                       ON t.contents_lang = c2.d_cd ";
                xSql += "                       WHERE c1.m_cd='0042' AND c2.m_cd='0017' ";
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
       * Function name : GetContentsInfoByCondition
       * Purpose       : 조회조건에 따른 컨텐츠 전체 목록을 전달하는 처리
       * Input         : string[] rParams (0: pagesize, 1: currentPageIndex, 2: contents_nm, 3: remark, 4: lang, 5: contents_type)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetContentsInfoByCondition(string[] rParams)
        {
            try
            {
                string xSql = " SELECT * FROM ";
                xSql += "           (  SELECT rownum rnum, b.* FROM  ";
                xSql += "               (  ";
                xSql += "                   SELECT t.contents_id, t.contents_nm, t.contents_file_nm, t.contents_remark, ";
                xSql += "                   t.ins_id, t.ins_dt, t.send_dt, t.send_flg, t.use_flg, t.temp_save_flg, ";
                xSql += "                   c1.d_knm as contents_type, c2.d_knm as contents_lang, count(*) over() totalrecordcount ";
                xSql += "                       FROM t_contents t ";
                xSql += "                       INNER JOIN t_code_detail c1 ";
                xSql += "                       ON t.contents_type = c1.d_cd ";
                xSql += "                       INNER JOIN t_code_detail c2 ";
                xSql += "                       ON t.contents_lang = c2.d_cd ";
                // 조회조건 추가 <START>
                xSql += "                       WHERE ";
                xSql += "                       c1.m_cd='0042' AND c2.m_cd='0017' ";

                if (!string.IsNullOrEmpty(rParams[2]))
                    xSql += string.Format("   AND  lower(t.contents_nm) like '%{0}%' ", rParams[2].ToLower().Replace("'", "''")); // contents_nm
                if (!string.IsNullOrEmpty(rParams[3]))
                    xSql += string.Format("   AND  lower(t.contents_remark) like '%{0}%' ", rParams[3].ToLower().Replace("'", "''")); // remark
                if (!string.IsNullOrEmpty(rParams[4]))
                    xSql += string.Format("   AND  t.contents_lang like '%{0}%' ", rParams[4].Replace("'", "''")); // lang
                if (!string.IsNullOrEmpty(rParams[5]))
                    xSql += string.Format("   AND  t.contents_type like '%{0}%' ", rParams[5].Replace("'", "''")); // contents_type

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
       * Function name : GetContentsInfoForExcel
       * Purpose       : Excel 표준에 맞는 컨텐츠 전체 목록을 전달하는 처리
       * Input         : string[] rParams (0: contents_nm, 1: remark, 2: lang, 3: contents_type)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetContentsInfoForExcel(string[] rParams)
        {
            try
            {
                //string xSql = "  SELECT t.contents_id, t.contents_nm, t.contents_file_nm, t.contents_remark, ";
                //xSql += "               t.ins_id, t.ins_dt, t.send_dt, t.send_flg, t.use_flg, t.temp_save_flg, ";
                //xSql += "               c1.d_knm as contents_type, c2.d_knm as contents_lang ";
                //xSql += "        FROM t_contents t ";
                //xSql += "        INNER JOIN t_code_detail c1 ";
                //xSql += "        ON t.contents_type = c1.d_cd ";
                //xSql += "        INNER JOIN t_code_detail c2 ";
                //xSql += "        ON t.contents_lang = c2.d_cd ";
                //// 조회조건 추가 <START>
                //xSql += "                     WHERE ";
                //xSql += "                     c1.m_cd='0042' AND c2.m_cd='0017' ";

                //if (!string.IsNullOrEmpty(rParams[0]))
                //    xSql += string.Format("   AND  lower(t.contents_nm) like '%{0}%' ", rParams[0].ToLower()); // contents_nm
                //if (!string.IsNullOrEmpty(rParams[1]))
                //    xSql += string.Format("   AND  lower(t.contents_remark) like '%{0}%' ", rParams[1].ToLower()); // remark
                //if (!string.IsNullOrEmpty(rParams[2]))
                //    xSql += string.Format("   AND  t.contents_lang like '%{0}%' ", rParams[2]); // lang
                //if (!string.IsNullOrEmpty(rParams[3]))
                //    xSql += string.Format("   AND  t.contents_type like '%{0}%' ", rParams[3]); // contents_type

                //// 조회조건 추가 <END>
                //xSql += "        ORDER BY t.ins_dt DESC ";

                //return base.ExecuteDataTable("LMS", xSql);

                string xSql = " SELECT * FROM ";
                xSql += "           (  SELECT rownum rnum, b.* FROM  ";
                xSql += "               (  ";
                xSql += "                   SELECT t.contents_nm, t.contents_file_nm, c1.d_knm as contents_type ";
                xSql += "                     , c2.d_knm as contents_lang, t.contents_remark ";
                xSql += "                     , TO_CHAR(t.ins_dt, 'YYYY.MM.DD') AS INS_DT  ";
                xSql += "                       FROM t_contents t ";
                xSql += "                       INNER JOIN t_code_detail c1 ";
                xSql += "                       ON t.contents_type = c1.d_cd ";
                xSql += "                       INNER JOIN t_code_detail c2 ";
                xSql += "                       ON t.contents_lang = c2.d_cd ";
                // 조회조건 추가 <START>
                xSql += "                       WHERE ";
                xSql += "                       c1.m_cd='0042' AND c2.m_cd='0017' ";

                if (!string.IsNullOrEmpty(rParams[2]))
                    xSql += string.Format("   AND  lower(t.contents_nm) like '%{0}%' ", rParams[2].ToLower().Replace("'", "''")); // contents_nm
                if (!string.IsNullOrEmpty(rParams[3]))
                    xSql += string.Format("   AND  lower(t.contents_remark) like '%{0}%' ", rParams[3].ToLower().Replace("'", "''")); // remark
                if (!string.IsNullOrEmpty(rParams[4]))
                    xSql += string.Format("   AND  t.contents_lang like '%{0}%' ", rParams[4].Replace("'", "''")); // lang
                if (!string.IsNullOrEmpty(rParams[5]))
                    xSql += string.Format("   AND  t.contents_type like '%{0}%' ", rParams[5].Replace("'", "''")); // contents_type

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

        /************************************************************
       * Function name : GetContentsInfoByID
       * Purpose       : Contents_ID에 해당하는 Contents 상세정보를 가져오는 처리
       * Input         : string[] rParams (0: contents_id)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetContentsInfoByID(string[] rParams)
        {
            try
            {
                string xSql = "  SELECT t.contents_id, t.contents_nm, t.contents_file_nm, t.contents_remark, ";
                xSql += "               t.ins_id, t.ins_dt, t.send_dt, t.send_flg, t.use_flg, t.temp_save_flg, ";
                xSql += "               t.contents_type, contents_lang, ";
                xSql += "               c1.d_knm as contents_type_nm, c2.d_knm as contents_lang_nm ";
                xSql += "        FROM t_contents t ";
                xSql += "        INNER JOIN t_code_detail c1 ";
                xSql += "        ON t.contents_type = c1.d_cd ";
                xSql += "        INNER JOIN t_code_detail c2 ";
                xSql += "        ON t.contents_lang = c2.d_cd ";
                xSql += "        WHERE ";
                xSql += "        c1.m_cd='0042' AND c2.m_cd='0017' ";
                xSql += string.Format("        AND  t.contents_id='{0}' ", rParams[0]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /************************************************************
       * Function name : SetContentsInsert
       * Purpose       : Contents 신규 항목을 등록하는 처리
       * Input         : object[] rParams (0: contents_id, 1: contents_type, 2: lang, 3: contents_name, 4: remark, 
       *                 5: contents_file, 6: contents_filename, 7: contents_filepath, 8: ins_id, 9: send_flg)
       * Output        : string
       *************************************************************/
        public string SetContentsInsert(object[] rParams)
        {
            Database db = null;
            string xRtn = Boolean.FalseString;
            string xSql = "";
            string xFilePath = "";

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

                        // 1. 파일 저장

                        // 파일이 변경된 경우만, HttpPostedFile 객체가 넘겨져 온다.
                        if (rParams[5] != null)
                        {
                                                      
                            byte[] xFileData = (byte[])rParams[5];
                            xFilePath = rParams[11].ToString() +  rParams[6].ToString();
                            FileStream xNewFile = new FileStream(xFilePath, FileMode.Create);
                            xNewFile.Write(xFileData, 0, xFileData.Length);
                            xNewFile.Close();

                            //HttpServerUtility xSU = new HttpServerUtility(); 
                            

                            //HttpPostedFile xFile = (HttpPostedFile)rParams[5];
                            //int xFileLen = xFile.ContentLength; // 파일 사이즈

                            //byte[] xFileData =  new byte[xFileLen]; // 파일 사이즈만큼 byte 배열 설정
                            //xFile.InputStream.Read(xFileData, 0, xFileLen); // 스트림에서 파일 읽어 바이트 배열에 담는 처리

                            //// 파일 생성
                            //string[] xFileNameSector = xFile.FileName.Split(new char[] { '\\' });
                            //string xFileName = xFileNameSector[xFileNameSector.Length - 1];
                            //rParams[6] = xFileName;

                            //xFilePath = rParams[7].ToString() + xFileName;
                            //FileStream xNewFile = new FileStream(xFilePath, FileMode.Create);

                            //xNewFile.Write(xFileData, 0, xFileData.Length); // byte 배열 내용을 파일 쓰는 처리
                            //xNewFile.Close(); // 파일 닫는 처리
                        }

                        bool xUpdate = false; //update 할지 여부 
                        OracleParameter[] xPara = null;

                        if (rParams[10].ToString() == "Y")
                        {
                            //subject id를 확인 하여 temp_save_flg = 'Y' 이면 update
                            //아니면 insert 하기                             
                            xSql = " SELECT TEMP_SAVE_FLG FROM t_contents WHERE contents_id = '" + rParams[0] + "' ";
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

                        string xQID = string.Empty; 
                        if (xUpdate)
                        {
                            xQID = rParams[0].ToString(); 
                            xSql = @" UPDATE T_CONTENTS 
                                        SET
                                            CONTENTS_TYPE = :CONTENTS_TYPE
                                            , CONTENTS_LANG = :CONTENTS_LANG
                                            , CONTENTS_NM = :CONTENTS_NM
                                            , CONTENTS_REMARK = :CONTENTS_REMARK
                                            , CONTENTS_FILE_NM = :CONTENTS_FILE_NM
                                            , UPT_ID = :UPT_ID 
                                            , UPT_DT = SYSDATE 
                                        WHERE CONTENTS_ID = :CONTENTS_ID ";

                            xPara = new OracleParameter[7];
                            xPara[0] = base.AddParam("CONTENTS_TYPE", OracleType.VarChar, rParams[1]);
                            xPara[1] = base.AddParam("CONTENTS_LANG", OracleType.VarChar, rParams[2]);
                            xPara[2] = base.AddParam("CONTENTS_NM", OracleType.VarChar, rParams[3]);
                            xPara[3] = base.AddParam("CONTENTS_REMARK", OracleType.VarChar, rParams[4]);
                            xPara[4] = base.AddParam("CONTENTS_FILE_NM", OracleType.VarChar, rParams[6]);
                            xPara[5] = base.AddParam("UPT_ID", OracleType.VarChar, rParams[8]);
                            xPara[6] = base.AddParam("CONTENTS_ID", OracleType.VarChar, rParams[0]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);

                        }
                        else
                        {
                            xSql = " INSERT INTO t_contents ";
                            xSql += " (contents_id, contents_type, contents_lang, contents_nm, contents_remark, ";
                            xSql += " contents_file_nm, ins_id, INS_DT, UPT_ID, UPT_DT, send_flg, TEMP_SAVE_FLG) ";
                            xSql += " VALUES ( ";
                            xSql += " :contents_id, :contents_type, :contents_lang, :contents_nm, :contents_remark, ";
                            xSql += " :contents_file_nm, :ins_id, SYSDATE, :INS_ID, SYSDATE, :send_flg, :TEMP_SAVE_FLG ";
                            xSql += " ) ";

                            vp_l_common_md com = new vp_l_common_md();
                            xQID = com.GetMaxIDOfTable(new string[] { "T_CONTENTS", "contents_id" });

                            xPara = new OracleParameter[9];
                            xPara[0] = base.AddParam("CONTENTS_TYPE", OracleType.VarChar, rParams[1]);
                            xPara[1] = base.AddParam("CONTENTS_LANG", OracleType.VarChar, rParams[2]);
                            xPara[2] = base.AddParam("CONTENTS_NM", OracleType.VarChar, rParams[3]);
                            xPara[3] = base.AddParam("CONTENTS_REMARK", OracleType.VarChar, rParams[4]);
                            xPara[4] = base.AddParam("CONTENTS_FILE_NM", OracleType.VarChar, rParams[6]);
                            xPara[5] = base.AddParam("INS_ID", OracleType.VarChar, rParams[8]);
                            xPara[6] = base.AddParam("CONTENTS_ID", OracleType.VarChar, xQID);
                            xPara[7] = base.AddParam("send_flg", OracleType.VarChar, rParams[9]);
                            xPara[8] = base.AddParam("TEMP_SAVE_FLG", OracleType.VarChar, rParams[10]);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xPara, xTransLMS);

                            //// 2. DB 저장 (DB 작업 중에 문제발생 시, 저장된 파일 제거)
                            //xSql = " INSERT INTO t_contents ";
                            //xSql += " (contents_id, contents_type, contents_lang, contents_nm, contents_remark, ";
                            //xSql += " contents_file_nm, ins_id, send_flg) ";
                            //xSql += " VALUES ( ";

                            //xSql += string.Format("'{0}', ", rParams[0].ToString()); // contents_id

                            //xSql += string.Format("'{0}', ", rParams[1].ToString()); // contents_type
                            //xSql += string.Format("'{0}', ", rParams[2].ToString()); // lang
                            //xSql += string.Format("'{0}', ", rParams[3].ToString()); // contents_name
                            //xSql += string.Format("'{0}', ", rParams[4].ToString()); // remark
                            //xSql += string.Format("'{0}', ", rParams[6].ToString()); // contents_filename
                            //xSql += string.Format("'{0}', ", rParams[8].ToString()); // ins_id
                            //xSql += string.Format("'{0}' ", rParams[9].ToString()); // send_flg

                            //xSql += " ) ";

                            //xCmdLMS.CommandText = xSql;
                            //base.Execute("LMS", xCmdLMS, xTransLMS);
                        }

                        //temp save  아닐 경우선박으로 발송
                        if (rParams[10].ToString() == "N")
                        {
                            OracleParameter[] oOraParams = null; 
                            oOraParams = new OracleParameter[2];
                            oOraParams[0] = base.AddParam("p_in_table", OracleType.VarChar, "T_CONTENTS");
                            oOraParams[1] = base.AddParam("p_out_table", OracleType.VarChar, "T_LMS_CONTENTS");
                            base.Execute(db, CommandType.StoredProcedure, "pkg_lms_datasync.lms_export", oOraParams, xTransLMS);
                        }

                        xRtn = xQID;
                        xTransLMS.Commit(); //트렌잭션 커밋
                    }
                    catch (Exception ex)
                    {
                        // 파일 제거
                        if (xFilePath.Trim() != "")
                            File.Delete(xFilePath);
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

        /************************************************************
       * Function name : GetContentsMixInfoOfCourse
       * Purpose       : 특정 (등록)과정의 과목 & 컨텐츠 정보를 가져오는 처리
       * Input         : string[] rParams (0: course_id)
       * Output        : DataTable
       *************************************************************/
        public DataTable GetContentsMixInfoOfCourse(string[] rParams)
        {
            try
            {
                string xSql = " SELECT a.course_nm, b.subject_seq, c.subject_nm, e.contents_nm, e.contents_file_nm ";
                xSql += " FROM t_course a, t_course_subject b, t_subject c, t_subject_contents d, t_contents e ";
                xSql += string.Format(" WHERE a.course_id = '{0}'", rParams[0]);
                xSql += " AND a.course_id = b.course_id ";
                xSql += " AND b.subject_id = c.subject_id ";
                xSql += " AND c.subject_id = d.subject_id ";
                xSql += " AND d.contents_id = e.contents_id ";
                xSql += " ORDER BY b.subject_seq, d.contents_seq ";

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
