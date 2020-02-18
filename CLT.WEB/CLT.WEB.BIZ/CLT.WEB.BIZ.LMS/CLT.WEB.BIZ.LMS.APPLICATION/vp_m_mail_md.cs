using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

// 필수 using 문
using System.Data;
using System.Data.OracleClient;
using CLT.WEB.BIZ.FX.BIZBASE;
using System.Collections;
using CLT.WEB.BIZ.LMS.COMMON;
using System.Web.UI.HtmlControls;
using System.Web.Mail;
using System.IO;
using System.Net;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace CLT.WEB.BIZ.LMS.APPLICATION
{
    /// <summary>
    /// 1. 작업개요 : 메일 처리 
    /// 
    /// 2. 주요기능 : 메일 처리
    ///				  
    /// 3. Class 명 : vp_m_mail_md
    /// 
    /// 4. 작 업 자 : 
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_m_mail_md
    ///        * Comment 
    ///          Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제따른 변경
    ///          영문화 작업 

    /// </summary>
    class vp_m_mail_md : DAC
    {
        /************************************************************
        * Function name : GetMailListMaster
        * Purpose       : MAIL List 조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public DataTable GetMailListMaster(string[] rParams)
        {
            try
            {
                string xSql = " SELECT * FROM ( ";
                xSql += " SELECT rownum rnum, b.* FROM ( ";
                xSql += " SELECT mail.seq seq, (SELECT count(*) FROM t_sent_mail_box_att WHERE mail.seq = seq) attcount, ";
                xSql += "        mail_sub, user_id, rec_count, sent_dt, ";
                xSql += "        count(*) over() totalrecordcount ";
                xSql += "   FROM t_sent_mail_box mail ";
                xSql += "  WHERE mail.seq IS NOT NULL ";

                if (!string.IsNullOrEmpty(rParams[2]))
                {
                    xSql += string.Format("   AND sent_dt >= TO_DATE('{0}','YYYY.MM.DD.HH24MISS') ", rParams[2] + ".000000");
                    xSql += string.Format("   AND sent_dt <= TO_DATE('{0}','YYYY.MM.DD.HH24MISS') ", rParams[3] + ".235959");
                }
                xSql += "   ORDER BY mail.ins_dt DESC ";
                xSql += " ) b";
                xSql += " ) ";
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
        * Function name : GetMailListDetail
        * Purpose       : MAIL List 조회
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public DataTable GetMailListDetail(string rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                string xSql = " SELECT mail_sub ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", tuser.user_nm_kor ";
                }
                else
                {
                    xSql += ", tuser.USER_NM_ENG_FIRST || ' ' || USER_NM_ENG_LAST AS user_nm_kor ";
                }
                
                xSql += ", email_id, sent_dt, rec_email, sent_mail, att.att_file ";
                xSql += " FROM t_sent_mail_box mail, t_user tuser, t_sent_mail_box_att att ";
                xSql += " WHERE mail.user_id = tuser.user_id ";
                xSql += " AND mail.seq = att.seq(+) ";
                xSql += string.Format(" AND mail.seq = {0} ", rParams);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        /************************************************************
        * Function name : GetMaxIDOfCode
        * Purpose       : MAIL ID 생성
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public string GetMaxIDOfCode(string[] rParams, OracleCommand rCmd)
        {
            string xSql = "";
            object xTemp = null;
            string xRtnID = string.Empty; // 테이블별 리턴 ID를 담는 변수

            try
            {
                xSql = string.Format(" SELECT NVL(MAX({0}+1),0) id FROM t_sent_mail_box ", rParams[0]);

                rCmd.CommandText = xSql;
                xRtnID = Convert.ToString(rCmd.ExecuteScalar());

                if (xRtnID == "0")
                    xRtnID = "1";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return xRtnID;
        }

        

        /************************************************************
        * Function name : SetMailSend
        * Purpose       : Mail 전송관련 테이블 Insert, Mail 발송
        * Input         : string[] rParams
        * Output        : String
        *************************************************************/
        public string SetMailSend(params object[] rParams)
        {
            string xRtn = Boolean.FalseString;

            Database db = base.GetDataBase("LMS");

            OracleConnection xCnnLMS = (OracleConnection)db.CreateConnection();
            xCnnLMS.Open();
            OracleTransaction xTransLMS = null;
            OracleCommand xCmdLMS = null;
            try
            {
                string xFilePath = string.Empty;

                xTransLMS = xCnnLMS.BeginTransaction();  // 트랜잭션 시작
                xCmdLMS = base.GetSqlCommand(db); 
                xCmdLMS.Connection = xCnnLMS;
                xCmdLMS.Transaction = xTransLMS;

                string xSeq = GetMaxIDOfCode(new string[] {"seq"}, xCmdLMS);

                string[] rMasterParams = (string[])rParams[0];
                object[] rDetailParams = (object[])rParams[1];

                try
                {
                    string xSql = "INSERT INTO t_sent_mail_box ( ";
                    xSql += " seq, mail_sub, sent_mail, rec_count, rec_email, open_course_id, user_id, sent_duty_step, sent_dt, ins_id, ins_dt) ";
                    xSql += " VALUES ( ";
                    xSql += string.Format(" {0}, ", xSeq);  // SEQ (순번)
                    xSql += string.Format(" '{0}', ", rMasterParams[0].ToString());  // mail_sub (메일제목)
                    xSql += string.Format(" '{0}', ", rMasterParams[1].ToString());  // sent_mail (메일내용)
                    xSql += string.Format(" {0}, ", Convert.ToInt32(rMasterParams[2]));  // rec_count (발신자 총원)
                    xSql += string.Format(" '{0}', ", rMasterParams[3].ToString());  // rec_email 수신자 메일 주소 (구분자는 ';')
                    xSql += string.Format(" '{0}', ", rMasterParams[4].ToString());  // open_course_id (개설과정 ID)
                    xSql += string.Format(" '{0}', ", rMasterParams[5].ToString());  // user_id (발신자 ID)
                    xSql += string.Format(" '{0}', ", rMasterParams[6].ToString());  // sent_duty_stpe 발신자 직급
                    xSql += string.Format(" {0}, ", "SYSDATE");  // SENT_DT 발송일자
                    xSql += string.Format(" '{0}', ", rMasterParams[5].ToString());  // 등록자 ID
                    xSql += string.Format(" {0}) ", "SYSDATE");

                    xCmdLMS.CommandText = xSql;
                    base.Execute(db, xCmdLMS, xTransLMS);


                    Rebex.Mail.MailMessage xMsg;
                    xMsg = new Rebex.Mail.MailMessage();
                    xMsg.From = "HANJIN SM" + "<" + rMasterParams[8] + ">";

                    if (rMasterParams[9] == Boolean.TrueString)
                    {
                        xMsg.Bcc = rMasterParams[3];
                    }
                    else
                    {
                        xMsg.To = rMasterParams[3];
                    }
                    
                    xMsg.Subject = rMasterParams[0];
                    xMsg.BodyText = rMasterParams[1];

                        

                    if (rDetailParams.Length != 0)
                    {
                        foreach (object obj in rDetailParams)
                        {

                            HttpPostedFile xFile = (HttpPostedFile)obj;

                            byte[] xFileData = new byte[xFile.ContentLength];
                            xFile.InputStream.Read(xFileData, 0, xFile.ContentLength);

                            string[] xFileNameSector = xFile.FileName.Split(new char[] { '\\' });
                            string xFileName = xFileNameSector[xFileNameSector.Length - 1];

                            xFilePath = rMasterParams[7].ToString() + xFileName;
                            FileStream xNewFile = new FileStream(xFilePath, FileMode.Create);

                            xNewFile.Write(xFileData, 0, xFileData.Length); // byte 배열 내용을 파일 쓰는 처리
                            xNewFile.Close(); // 파일 닫는 처리
                            

                            
                            Rebex.Mail.Attachment att = new Rebex.Mail.Attachment(xFilePath);
                            xMsg.Attachments.Add(att);


                            xSql = string.Empty;
                            xSql += " INSERT INTO t_sent_mail_box_att ( ";
                            xSql += " seq, att_file) ";
                            xSql += " VALUES ( ";
                            xSql += string.Format(" {0}, ", xSeq);
                            xSql += string.Format(" '{0}') ", xFileName);

                            xCmdLMS.CommandText = xSql;
                            base.Execute(db, xCmdLMS, xTransLMS);
                        }
                    }

                    
                    // Live
                    Rebex.Net.Smtp.Send(xMsg, "smtp.hanjin.com");
                    // Test
                    //Rebex.Net.Smtp.Send(xMsg, "testmail.hanjin.com");
                    

                    //Rebex.Net.Smtp.SendDirect(xMsg);
                    //xMsg.Send(xMsg);
                    

                    //Rebex.Net.Smtp smtp = new Rebex.Net.Smtp();
                    //smtp.Connect("col380b.cyberlogitec.com");
                    //smtp.Login("kmk", "kims1203");
                    //smtp.Send(xMsg);
                    //Rebex.Net.Smtp.Send(xMsg, "col380b.cyberlogitec.com");
                    

                    xTransLMS.Commit(); // 트랜잭션 커밋
                    xRtn = Boolean.TrueString;
                }
                catch (Exception ex)
                {
                    // 파일 제거
                    if (xFilePath.Trim() != "")
                        File.Delete(xFilePath);

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
    }
}
