using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Security;

using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using CLT.WEB.UI.FX.UTIL;

namespace CLT.WEB.UI.COMMON.BASE
{
    /// <summary>
    /// 1. 작업개요 : BaseMasterPage Class
    /// 
    /// 2. 주요기능 : Master Page의 공통 기능 정의
    ///				  
    /// 3. Class 명 : BaseMasterPage
    /// 
    /// 4. 작 업 자 : 김양도 / 2011.12.08
    /// </summary>
    public class BaseMasterPage : System.Web.UI.MasterPage
    {
        #region 언어 설정
        
        protected string GetCultureName()
        {
            try
            {
                return System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToUpper();
            }
            catch
            {
                return "KO";
            }
        }

        /************************************************************
        * Function name : SetCultureSettings
        * Purpose       : 선택된 이미지(Korean, English)에 따라 언어 설정 처리
        * Input         : HtmlImage rImage
        * Output        : void
        *************************************************************/
        protected void SetCultureSettings(HtmlImage rImage)
        {
            // Session["Language"] 값이 있으면, 
            // 브라우저 언어 설정을 무시하고, 해당 Page의 언어설정을 변경하여 변경된 언어 설정의 Resource 값을 사용한다.
            if (Session["Language"] != null && Session["Language"].ToString().Trim() != string.Empty)
            {
                switch (Session["Language"].ToString().ToUpper())
                {
                    case "KO":
                        rImage.Src = "/images/tt_korean1.gif";
                        break;
                    case "EN":
                        rImage.Src = "/images/tt_english1.gif";
                        break;
                }
            }
            // Session["Language"] 값이 있으면, 브라우저 언어 설정의 Resource 값을 사용한다.
            else
            {
                CultureInfo xCi = System.Threading.Thread.CurrentThread.CurrentCulture;

                switch (xCi.TwoLetterISOLanguageName.ToUpper())
                {
                    case "KO":
                        rImage.Src = "/images/tt_korean1.gif";
                        break;
                    case "EN":
                        rImage.Src = "/images/tt_english1.gif";
                        break;
                }
            }
        }
        protected void SetDefaultCulture(CultureInfo culture)
        {
            Type type = typeof(CultureInfo);
            try
            {
                type.InvokeMember("s_userDefaultCulture",
                                    System.Reflection.BindingFlags.SetField | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static,
                                    null,
                                    culture,
                                    new object[] { culture });

                type.InvokeMember("s_userDefaultUICulture",
                                    System.Reflection.BindingFlags.SetField | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static,
                                    null,
                                    culture,
                                    new object[] { culture });
            }
            catch { }

            try
            {
                type.InvokeMember("m_userDefaultCulture",
                                    System.Reflection.BindingFlags.SetField | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static,
                                    null,
                                    culture,
                                    new object[] { culture });

                type.InvokeMember("m_userDefaultUICulture",
                                    System.Reflection.BindingFlags.SetField | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static,
                                    null,
                                    culture,
                                    new object[] { culture });
            }
            catch { }

            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            }
            catch { }
        }
        
        /************************************************************
        * Function name : IsSettingKorean
        * Purpose       : 현재 설정이 한국어인지 여부 확인 처리
        * Input         : void
        * Output        : bool
        *************************************************************/
        protected bool IsSettingKorean()
        {
            CultureInfo xCi = System.Threading.Thread.CurrentThread.CurrentCulture;

            if (xCi.Name.ToLower() == "ko-kr")
                return true;
            else
                return false;
        }

        #endregion

        #region 인증 처리

        // 인증되지 않은 사용자의 권한 처리를 위한 사용자 ID
        protected string GuestUserID
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["GuestUserID"].ToString();
            }
        }

        /************************************************************
        * Function name : IsAuthenticated
        * Purpose       : 현재 로그인된 사용자가 로그인이 되어있는지 여부 확인
        * Input         : void
        * Output        : void
        *************************************************************/
        protected bool IsAuthenticated()
        {
            if (Page.User.Identity.IsAuthenticated)
                return true;
            else
                return false;
        }

        /************************************************************
        * Function name : SignOut
        * Purpose       : 로그아웃 처리
        * Input         : void
        * Output        : void
        *************************************************************/
        protected void SignOut()
        {
            Page.Session.Abandon();
            FormsAuthentication.SignOut();
        }

        #endregion

        #region 경로 정의

        // excel 파일 다운로드 등과 같이 
        // 임시로 생성되어 삭제되는 파일들을 관리할 때 사용하는 경로
        protected string TempFilePath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["TempFilePath"].ToString();
            }
        }

        // LMS 컨텐츠 보관 파일 경로
        protected string ContentsFilePath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ContentsFilePath"].ToString();
            }
        }

        // Viewer 등 시스템에서 필요한 파일을 Download 하기 위한 경로
        protected string DownloadFilePath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DownloadFilePath"].ToString();
            }
        }

        #endregion
    }
}
