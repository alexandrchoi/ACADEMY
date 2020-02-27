using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

// 필수 using 문
using System.Globalization;
using CLT.WEB.UI.FX.UTIL;
using CLT.WEB.UI.FX.AGENT;
using CLT.WEB.UI.COMMON.BASE;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using IAMVIRTUALAGENTCOMLib;
using System.Linq;

namespace CLT.WEB.UI.LMS.MAIN
{
    /// <summary>
    /// 1. 작업개요 : Sub Master Class
    /// 
    /// 2. 주요기능 : 웹사이트 레이아웃
    ///				  
    /// 3. Class 명 : MasterSub
    /// 
    /// 4. 작 업 자 : 
    /// </summary>
    public partial class MasterWin : BaseMasterPage
    {
        String iFileNm, iSubNm = "";

        /************************************************************
       * Function name : Page_Load
       * Purpose       : 페이지 로드될 때 처리
       * Input         : void
       * Output        : void
       *************************************************************/
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Language"] != null && Session["Language"].ToString().Trim() != GetCultureName())
            {
                SetDefaultCulture(new CultureInfo(Session["Language"].ToString() == "EN" ? "en-US" : "ko-KR"));
                //Response.Redirect(Request.RawUrl.Replace("Lang=" + Session["Language"], ""));
            }
            
            try
            {
                iFileNm = getFileName();
                iSubNm = getSubName();
                //
                HtmlHead head = (System.Web.UI.HtmlControls.HtmlHead)this.Page.Header;
                head.Controls.Add(new LiteralControl("\r\n"));
                //
                HtmlLink[] xHtmlLnk = new HtmlLink[3];
                string[] xCssFiles = { "css/common/reset", "css/jquery-ui.min", "css/common/layout"}; //, "fontawesome/css/all" 
                for (int i = 0; i < 3; i++)
                {
                    xHtmlLnk[i] = new HtmlLink();
                    xHtmlLnk[i].Attributes.Add("rel", "stylesheet");
                    xHtmlLnk[i].Attributes.Add("type", "text/css");
                    xHtmlLnk[i].Attributes.Add("media", "all");
                    xHtmlLnk[i].Href = ResolveUrl("/asset/" + xCssFiles[i] + ".css"); ;
                    head.Controls.Add(xHtmlLnk[i]);
                    head.Controls.Add(new LiteralControl("\r\n"));
                }
                //
                HtmlGenericControl[] xHtmlGC = new HtmlGenericControl[4];
                string[] xJssFiles = { "jquery.min", "jquery-ui.min", "common/util", "common/popup" };
                for (int i = 0; i < 4; i++)
                {
                    xHtmlGC[i] = new HtmlGenericControl();
                    xHtmlGC[i].TagName = "script";
                    xHtmlGC[i].Attributes.Add("type", "text/javascript");
                    //xHtmlGC[i].Attributes.Add("language", "javascript");
                    xHtmlGC[i].Attributes.Add("src", "/asset/js/" + xJssFiles[i] + ".js");
                    head.Controls.Add(xHtmlGC[i]);
                    head.Controls.Add(new LiteralControl("\r\n"));
                }
                //
                if (Session["USER_ID"] != null && Session["USER_ID"].ToString() != this.GuestUserID)
                {
                }
                // 인증되지 않은 사용자의 경우
                else
                {
                }
                //bottom 영문처리
                if (IsSettingKorean())
                {
                }
                else
                {
                }

                if (!IsPostBack)
                {
                }
                //
                if (Convert.ToString(Session["USER_GROUP"]) != this.GuestUserID)
                {

                }
                else
                {

                }
                //
                string xJScripts = "\r\n"
                    + " function fnShowHide(id, show) " + "\r\n"
                    + " { " + "\r\n"
                    + "     var sView = document.getElementById(id).style.visibility == 'visible' ? 'hidden' : 'visible'; " + "\r\n"
                    + "     if (show != null) sView = show; " + "\r\n"
                    + "     document.getElementById(id).style.visibility = sView; " + "\r\n"
                    + " } " + "\r\n";
                xJScripts += " \r\n" + "\r\n"
                    // 주어진 배열에서 요소 1개를 랜덤하게 골라 반환하는 함수
                    + " function randomItem(a) { " + "\r\n"
                    + "     return a[Math.floor(Math.random() * a.length)]; " + "\r\n"
                    + " } " + "\r\n"
                    + "\r\n"
                    + " $(document).ready(function() { " + "\r\n";

                xJScripts += " \r\n" + "\r\n"
                    //Calendar (fold - 컨트롤 아래로 숨는문제, bounce 떨림 사용X)
                    + "     vAnim = new Array('show', 'slideDown', 'fadeIn', 'blind', 'clip', '', 'slide'); " + "\r\n"
                    + "     anim = randomItem(vAnim); " + "\r\n"
                    + "     $('.datepick').each(function() { " + "\r\n"
                    + "         $(this).datepicker({ " + "\r\n"
                    + "                 showOn: 'both', " + "\r\n" //button
                    + "                 changeMonth: true, " + "\r\n"
                    + "                 changeYear: true, " + "\r\n"
                    + "                 showButtonPanel: true, " + "\r\n"
                    + "                 buttonImage: '/asset/images/icon_cal.gif', " + "\r\n"
                    + "                 buttonImageOnly: true, " + "\r\n"
                    + "                 showMonthAfterYear: true, " + "\r\n"
                    + "                 dateFormat: 'yy.mm.dd', " + "\r\n"
                    + "                 showAnim: anim, " + "\r\n"
                    + "                 yearRange: '-100:+10',  " + "\r\n"
                    + "         }); " + "\r\n"
                    + "     }); " + "\r\n"
                    + " }); " + "\r\n"
                    + "\r\n";

                // <body> 태그 바로 밑에
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", xJScript, true);
                // </body> 태그 바로 위에
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", xJScripts, true);

                xJScripts = "\r\n"
                    + "    // 버튼 박스 자식 체크 " + "\r\n"
                    + "    var $buttonChild = $('.button-box').children().length; " + "\r\n"
                    + "    if ($buttonChild >= 1) { " + "\r\n"
                    + "        $('.button-box').addClass('has-button'); " + "\r\n"
                    + "    } " + "\r\n"
                    + " " + "\r\n"
                    + "    //파일첨부 인풋 기능 구현 " + "\r\n"
                    + "    var fileTarget = $('.file-box .upload-hidden'); " + "\r\n"
                    + "    fileTarget.on('change', function(){ // 값이 변경되면 " + "\r\n"
                    + "        if (window.FileReader){ // modern browser " + "\r\n"
                    + "            var filename = $(this)[0].files[0].name; " + "\r\n"
                    + "        } else { // old IE " + "\r\n"
                    + "            var filename = $(this).val().split('/').pop().split('\\').pop(); // 파일명만 추출 " + "\r\n"
                    + "        } " + "\r\n"
                    + " " + "\r\n"
                    + "        // 추출한 파일명 삽입 " + "\r\n"
                    + "        $(this).siblings('.upload-name').val(filename); " + "\r\n"
                    + "    }); " + "\r\n";
                head.Controls.Add(new LiteralControl("<script type='text/javascript'>" + xJScripts + "</script>"));

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "Propagate Policy");
                if (rethrow) throw;
            }
        }
        private string getFileName()
        {
            string xFileNm = "";
            try
            {
                string xRaw = System.IO.Path.GetFileNameWithoutExtension(Request.RawUrl).ToLower();
                if (xRaw == "index" || xRaw == "default")
                    xRaw = "";
            }
            catch { }
            return xFileNm;
        }
        private string getSubName()
        {

            string xSubNm = "";
            try
            {
                string xDir = System.IO.Path.GetDirectoryName(Request.RawUrl).ToLower();
                if (xDir.Length > 0)
                {
                    try
                    {
                        string[] arrDir = xDir.Split(new char[] { '\\' });

                        xSubNm = arrDir[arrDir.Length - 1];
                    }
                    catch
                    {
                        xSubNm = "";
                    }
                }
            }
            catch { }
            return xSubNm;
        }
        
    }
}
