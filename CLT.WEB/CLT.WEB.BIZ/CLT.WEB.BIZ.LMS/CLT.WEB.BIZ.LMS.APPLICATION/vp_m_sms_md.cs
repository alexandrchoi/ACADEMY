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
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace CLT.WEB.BIZ.LMS.APPLICATION
{
    /// <summary>
    /// 1. 작업개요 : vp_m_sms_md
    /// 
    /// 2. 주요기능 : SMS전송 관련 BIZ 처리
    ///				  
    /// 3. Class 명 : vp_m_sms_md
    /// 
    /// 4. 작 업 자 : 김민규 / 2011.01.20
    /// 
    /// 5. Revision History : 
    ///    [CHM-201219386] LMS 기능 개선 요청
    ///        *서진한 2012.08.01
    ///        * Source
    ///          vp_m_sms_md
    ///        * Comment 
    ///          Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제따른 변경    ///          영문화 작업 
    /// 
    ///    [CHM-201218484] 국토해양부 로고 추가 및 데이터 정렬 기준 추가
    ///     * 김은정 2012.09.13
    ///     * Source
    ///       : vp_m_sms_md.cs
    ///     * Comment 
    ///       : 교육대상자 선발에서 확정인원만 문자 발송 대상자에 포함되도록 변경 (조건식 Confirm 여부 추가 : '1') 
    /// </summary>  
    /// 
    class vp_m_sms_md : DAC
    {
        /************************************************************
        * Function name : GetSMSListMaster
        * Purpose       : SMS 발송 리스트 조회
        * Input         : string[] rParams
        * Output        : DataTable
        *************************************************************/
        public DataTable GetSMSListMaster(string[] rParams)
        {
            try
            {
                string xSql = " SELECT * FROM ( ";
                xSql += " SELECT rownum rnum, b.* FROM ( ";
                xSql += " SELECT sms_group_no, sent_sms_title, sent_dt, COUNT(seq) totalcount, COUNT(rec_dt) sentcount, ";
                xSql += " COUNT(seq) - COUNT(rec_dt) failcount, ";
                xSql += " COUNT(*) over() totalrecordcount ";
                xSql += " FROM t_sent_sms_box ";
                xSql += string.Format(" WHERE reserved_yn = '{0}' ", rParams[2]); // 전송타입 (예약발송, 일반발송)
                if (!string.IsNullOrEmpty(rParams[3].ToString()))
                    xSql += string.Format("   AND sent_dt >= TO_DATE('{0}','YYYY.MM.DD.HH24MISS') ", rParams[3] + ".000000");
                if (!string.IsNullOrEmpty(rParams[4].ToString()))
                    xSql += string.Format("   AND sent_dt <= TO_DATE('{0}','YYYY.MM.DD.HH24MISS') ", rParams[4] + ".235959");
                xSql += " GROUP BY sms_group_no, sent_sms_title, sent_dt ";
                xSql += " HAVING COUNT(rec_dt) IS NOT NULL ";
                xSql += " ORDER BY sent_dt DESC ";
                xSql += " ) b ";
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


        public DataTable GetSMSListDetail(string[] rParams)
        {
            try
            {
                string xSql = string.Empty;
                xSql += " SELECT seq, sent_sms, rec_nm, rec_mobile_phone, sent_dt, ";
                xSql += " DECODE(rec_dt, null, '실패', '성공') AS result ";
                xSql += " FROM t_sent_sms_box ";
                xSql += string.Format(" WHERE sms_group_no = {0} ", rParams[0]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /************************************************************
        * Function name : GetCourseDate
        * Purpose       : 조회기간별 과정 리스트 조회
        * Input         : string[] rParams
        * Output        : DataTable
        *************************************************************/
        public DataTable GetCourseDate(string[] rParams, CultureInfo rArgCultureInfo)
        {
            try
            {
                string xSql = "SELECT DISTINCT opencourse.course_id course_id ";
                if (rArgCultureInfo.Name.ToLower() == "ko-kr")
                {
                    xSql += ", course.course_nm course_nm ";
                }
                else
                {
                    xSql += ", course.COURSE_NM_ABBR AS course_nm ";
                }
                
                xSql += " FROM t_open_course opencourse, t_course course ";
                xSql += " WHERE opencourse.course_id = course.course_id ";
                xSql += string.Format(" AND opencourse.ins_dt >= TO_DATE('{0}','YYYY.MM.DD') ", rParams[0]); // 조회기간 From
                xSql += string.Format(" AND opencourse.ins_dt <= TO_DATE('{0}','YYYY.MM.DD') ", rParams[1]); // 조회기간 To
                xSql += " ORDER BY course_nm ASC ";
                return base.ExecuteDataTable("LMS", xSql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /************************************************************
        * Function name : GetOpenCourseDate
        * Purpose       : 조회기간별 개설과정 리스트 조회
        * Input         : string[] rParams
        * Output        : DataTable
        *************************************************************/
        public DataTable GetOpenCourseDate(string[] rParams)
        {
            try
            {
                string xSql = "SELECT TO_CHAR(opencourse.course_begin_dt,'YYYY.MM.DD') || ' ~ ' || TO_CHAR(opencourse.course_end_dt, 'YYYY.MM.DD') course_date, ";
                xSql += " opencourse.open_course_id open_course_id ";
                xSql += " FROM t_open_course opencourse ";
                xSql += string.Format(" WHERE course_id = '{0}' ", rParams[0]);
                xSql += string.Format("   AND opencourse.ins_dt >= TO_DATE('{0}', 'YYYY.MM.DD') ", rParams[1]);
                xSql += string.Format("   AND opencourse.ins_dt <= TO_DATE('{0}', 'YYYY.MM.DD') ", rParams[2]);

                return base.ExecuteDataTable("LMS", xSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /************************************************************
        * Function name : GetOpenCourseResultUser
        * Purpose       : 개설과정에 등록된 사용자 List
        * Input         : string[] rParams
        * Output        : DataTable
        *************************************************************/
        public DataTable GetOpenCourseResultUser(string[] rParams)
        {
            try
            {
                string xSql = "SELECT tuser.user_id, ";
                xSql += " tuser.user_nm_kor, tuser.user_nm_eng_first || ' ' || tuser.user_nm_eng_last user_nm_eng, ";
                xSql += " tuser.mobile_phone, tuser.email_id ";
                xSql += " FROM t_course_result result ";
                xSql += " INNER JOIN t_user tuser ";
                xSql += " ON tuser.user_id = result.user_id ";
                
                xSql += string.Format(" WHERE result.open_course_id = '{0}' ", rParams[0]);
                xSql += " AND confirm = '1' ";

                return base.ExecuteDataTable("LMS", xSql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /************************************************************
        * Function name : GetSMSID
        * Purpose       : SMS ID 생성
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public string GetSMSID(string[] rParams, OracleCommand rCmd)
        {
            string xSql = "";
            object xTemp = null;
            string xRtnID = string.Empty; // 테이블별 리턴 ID를 담는 변수

            try
            {
                xSql = string.Format(" SELECT seq_sms_id.NEXTVAL FROM DUAL ");
                //if (!string.IsNullOrEmpty(rParams[1]))
                //    xSql += string.Format(" WHERE {0} = {1} ", rParams[0], rParams[2]);

                rCmd.CommandText = xSql;
                xRtnID = Convert.ToString(rCmd.ExecuteScalar());
                //xTemp = base.ExecuteScalar("LMS", xSql);

                //xRtnID = xTemp.ToString();

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
        * Function name : GetSMSGroupID
        * Purpose       : SMS ID 생성
        * Input         : string[] rParams
        * Output        : string
        *************************************************************/
        public string GetSMSGroupID(string[] rParams)
        {
            string xSql = "";
            object xTemp = null;
            string xRtnID = string.Empty; // 테이블별 리턴 ID를 담는 변수

            try
            {
                xSql = string.Format(" SELECT seq_sms_group_id.NEXTVAL FROM DUAL ");

                xTemp = base.ExecuteScalar("LMS", xSql);

                xRtnID = xTemp.ToString();

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
        * Function name : GetUserDutyStep
        * Purpose       : SMS 전송관련 테이블 Insert
        * Input         : string[] rParams
        * Output        : DataTable
        *************************************************************/
        public DataTable GetUserDutyStep(string rParams)
        {
            try
            {

                string xSql = "SELECT duty_step, company_id FROM t_user ";
                xSql += string.Format("WHERE user_id = '{0}' ", rParams);
                //for (int i = 0; i < rParams.Length; i++)
                //{
                //    if (i < rParams.Length)
                //        xSql += string.Format(" '{0}',  ", rParams[0]);
                //    else
                //        xSql += string.Format(" '{0}' ) ", rParams[0]);
                //}

                return base.ExecuteDataTable("LMS", xSql);

            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }

        /************************************************************
        * Function name : SetSentSMSBoxInsert
        * Purpose       : SMS 전송관련 테이블 Insert
        * Input         : string[] rParams
        * Output        : DataTable
        *************************************************************/
        public string SetSentSMSBoxInsert(params object[] rParams)
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

                string[] rMasterParams = (string[])rParams[0];
                string[,] rDetailParams = (string[,])rParams[1];


                string xSMSGroupCode = GetSMSGroupID(new string[] { "sms_group_no"});
                           
                try
                {
                    int xCount = Convert.ToInt32(rMasterParams[3]);
                    for (int i = 0; i < xCount; i++)
                    {
                        string xSeq = GetSMSID(new string[] { "seq", xSMSGroupCode }, xCmdLMS);
                        DataTable xDt = null;
                        if (rDetailParams[i,0] != "0")
                             xDt = GetUserDutyStep(rDetailParams[i,0]);

                        string xSql = "INSERT INTO t_sent_sms_box ( ";
                        xSql += " seq, ";
                        xSql += " sms_group_no, ";
                        xSql += " sent_sms, ";
                        xSql += " sent_sms_title, ";
                        xSql += " rec_mobile_phone, ";
                        xSql += " rec_nm, ";
                        xSql += " open_course_id, ";
                        xSql += " rec_dt, ";
                        xSql += " sent_user_id, ";
                        xSql += " sent_mobile_phone, ";
                        xSql += " reserved_yn, ";
                        xSql += " reserved_dt, ";
                        //xSql += " sent_dt, ";
                        xSql += " rec_user_id, ";
                        xSql += " rec_duty_step, ";
                        xSql += " company_id) ";
                        xSql += " VALUES ( "; 
                        xSql += string.Format(" {0}, ", Convert.ToInt32(xSeq));  // Seq
                        xSql += string.Format(" {0}, ", Convert.ToInt32(xSMSGroupCode)); // SMS 발송그룹
                        xSql += string.Format(" '{0}', ", rMasterParams[2]);  // SMS 발송 내용
                        xSql += string.Format(" '{0}', ", rMasterParams[0]);  // SMS 제목
                        xSql += string.Format(" '{0}', ", rDetailParams[i,2]);  // 수신자 전화번호
                        xSql += string.Format(" '{0}', ", rDetailParams[i, 1]); // 수신자 이름
                        xSql += string.Format(" '{0}', ", rMasterParams[4]);  // 개설과정 코드
                        xSql += " SYSDATE, ";
                        xSql += string.Format(" '{0}', ", rMasterParams[5]);  // 발신자 ID
                        xSql += string.Format(" '{0}', ", rMasterParams[1]);  // 발신자 회신번호

                        if (rMasterParams[6] == "R")
                        {
                            xSql += string.Format(" '{0}', ", "Y");
                            xSql += string.Format(" TO_DATE({0},'YYYYMMDDHH24MISS'), ", rMasterParams[7]);
                        }
                        else
                        {
                            xSql += string.Format(" '{0}', ", "N");
                            xSql += " null, ";
                        }

                        if (rDetailParams[i, 0] == "0")
                        {
                            xSql += string.Format(" '{0}', ", " ");  // 수신자 ID (비회원일경우 Empty Value)
                            xSql += string.Format(" '{0}', ", " ");  // 수신자 직급코드
                            xSql += string.Format(" '{0}' ", " ");  // 수신자 회사 ID
                        }
                        else
                        {
                            xSql += string.Format(" '{0}', ", rDetailParams[i, 0]); // 수신자 ID
                            xSql += string.Format(" '{0}', ", xDt.Rows[0]["duty_step"].ToString());  // 수신자 직급코드
                            xSql += string.Format(" '{0}' ", xDt.Rows[0]["company_id"].ToString()); // 수신자 회사 ID
                        }
                        xSql += "        ) ";


                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);


                        // DBAgent SMS 발송
                        xSql = string.Empty;
                        xSql += " INSERT INTO tbl_submit_queue ";
                        xSql += " VALUES ( ";
                        xSql += string.Format(" '{0}', ", xSeq);  // 메시지 ID 기본키 (이력테이블로 이동될떄에도 PK)
                        xSql += string.Format(" '{0}', ", xSMSGroupCode); // 메시지 그룹 ID
                        xSql += string.Format(" '{0}', ", "1613");  // SK 브로드밴드 DBAgentID 
                        xSql += string.Format(" '{0}', ", "1");  // 전송위치 구분 : 1 기업
                        xSql += string.Format(" '{0}', ", rMasterParams[8].Trim());  // 메시지 구분코드
                        xSql += string.Format(" '{0}', ", rMasterParams[6].Trim()); // 예약발송 여부 I : 즉시발송, R : 예약발송
                        if (rMasterParams[6] == "I") // 즉시 발송일 경우
                            xSql += " TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS'), "; 
                        else  // 예약발송일 경우
                            xSql += string.Format(" '{0}', ", rMasterParams[7].Trim()); 
                        xSql += " '1', "; // 서버에서의 메시지 저장 여부 0 : 저장하지 않음
                        xSql += string.Format(" '{0}', ", rDetailParams[i, 2].Trim());  // 수신자 전화번호 // 메시지 받는 사람의 전화번호
                        xSql += string.Format(" '{0}', ", rMasterParams[1].Trim());  // 발신자 회신번호
                        xSql += " '', "; // 국가코드 (국제 메시지 발송시에만 사용)
                        xSql += " '00', "; // 특정통신사 가입자 지정
                        xSql += string.Format(" '{0}', ", rMasterParams[2]);  // SMS 발송 내용
                        xSql += " '', ";  //  CallBack URL

                        if (rMasterParams[8] == "00")  // 첨부된 컨텐츠 개수 used_cd가 SMS(00,01,02) 일 경우 0, used_cd가 LMS(10,11) 일 경우 1
                        {
                            xSql += string.Format(" {0}, ", "0"); // 첨부된 컨텐츠 개수 (SMS)
                            xSql += " '', ";
                        }
                        else
                        {
                            xSql += string.Format(" {0}, ", "1"); // 첨부된 컨텐츠 개수 (MAIL)
                            xSql += " 'text/plain', ";
                        }

                        xSql += " '', "; // 첨부된 이미지 경로
                        xSql += " to_char(sysdate, 'YYYYMMDDHH24MISS'), ";// 법인업체에서 SK 브로드밴드로 전송한 시간
                        xSql += " '', "; // 법인업체에서 결과를 통보받은 시간
                        xSql += " '', "; // SK 브로드밴드가 통신사로 메시지를 보낸 시간
                        xSql += " '', "; // 휴대폰 가입자가 메시지를 받은 시간
                        xSql += " '', "; // WebAgent 에서 사용
                        xSql += " '0', "; // 메시지 처리 상태 0 : 전송할 메시지(0으로 설정된 메시지만 발송) 1: 메시지 전송요청 진행중인 상태, 2: 메시지 전송요청 완료상태(전송결과 수신대기), 9: 처리 완료상태(전송결과를 수신 하였거나 오류를 발생하여 전송 하지 못하고 종료된 상태)
                        xSql += " '', "; // 전송 결과값
                        xSql += string.Format(" '{0}', ", rMasterParams[0]); // 메시지 타이틀
                        xSql += " '', "; // 수신자 이통사 코드
                        xSql += " '', ";
                        xSql += " '', ";
                        xSql += " '', ";
                        xSql += " '', ";
                        xSql += " 0, ";
                        xSql += " 0) ";

                        xCmdLMS.CommandText = xSql;
                        base.Execute(db, xCmdLMS, xTransLMS);
                    }

                    xTransLMS.Commit(); // 트랜잭션 커밋
                    xRtn = Boolean.TrueString;
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
    }
}
