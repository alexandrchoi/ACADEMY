<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="zipcode.aspx.cs" Inherits="CLT.WEB.UI.LMS.COMMON.zipcode" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.BASE" Assembly="CLT.WEB.UI.COMMON.BASE" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>▒ Address Search ▒</title>    
	<link href="/asset/css/layout.css" rel="stylesheet" type="text/css" />
<% 
    string inputYn = Request.Form["inputYn"];
    string roadFullAddr = Request.Form["roadFullAddr"];
    string roadAddrPart1 = Request.Form["roadAddrPart1"];
    string roadAddrPart2 = Request.Form["roadAddrPart2"];
    string engAddr = Request.Form["engAddr"];
    string jibunAddr = Request.Form["jibunAddr"];
    string zipNo = Request.Form["zipNo"];
    string addrDetail = Request.Form["addrDetail"];
    //string admCd = Request.Form["admCd"];
    //string rnMgtSn = Request.Form["rnMgtSn"];
    //string bdMgtSn = Request.Form["bdMgtSn"];
    //string detBdNmList = Request.Form["detBdNmList"];
    //**2017년 2월 추가 제공 **/
    //string bdNm = Request.Form["bdNm"];
    //string bdKdcd = Request.Form["bdKdcd"];
    //string siNm = Request.Form["siNm"];
    //string sggNm = Request.Form["sggNm"];
    //string emdNm = Request.Form["emdNm"];
    //string liNm = Request.Form["liNm"];
    //string rn = Request.Form["rn"];
    //string udrtYn = Request.Form["udrtYn"];
    //string buldMnnm = Request.Form["buldMnnm"];
    //string buldSlno = Request.Form["buldSlno"];
    //string mtYn = Request.Form["mtYn"];
    //string lnbrMnnm = Request.Form["lnbrMnnm"];
    //string lnbrSlno = Request.Form["lnbrSlno"];
    //**2017년 3월 추가 제공 **/
    //string emdNo = Request.Form["emdNo"];
%>
    <script language="javascript">
    // opener관련 오류가 발생하는 경우 아래 주석을 해지하고, 사용자의 도메인정보를 입력합니다. ("주소입력화면 소스"도 동일하게 적용시켜야 합니다.)
    document.domain = "academy-t.gmarineservice.com";

    /*
			    모의 해킹 테스트 시 팝업API를 호출하시면 IP가 차단 될 수 있습니다. 
			    주소팝업API를 제외하시고 테스트 하시기 바랍니다.
    */
    function init(){
	    var url = location.href;
	    var confmKey = "devU01TX0FVVEgyMDIwMDIwNTEwMDM0ODEwOTQzODA=";
	    var resultType = "4"; // 도로명주소 검색결과 화면 출력내용, 1 : 도로명, 2 : 도로명+지번, 3 : 도로명+상세건물명, 4 : 도로명+지번+상세건물명
	    var inputYn= "<%=inputYn%>";
	    if(inputYn != "Y"){
		    document.form.confmKey.value = confmKey;
		    document.form.returnUrl.value = url;
		    document.form.resultType.value = resultType;
		    document.form.action="http://www.juso.go.kr/addrlink/addrLinkUrl.do"; //인터넷망
		    //document.form.action="http://www.juso.go.kr/addrlink/addrMobileLinkUrl.do"; //모바일 웹인 경우, 인터넷망
		    document.form.submit();
	    }else{
		    opener.jusoCallBack("<%=roadFullAddr%>","<%=roadAddrPart1%>","<%=addrDetail%>","<%=roadAddrPart2%>","<%=engAddr%>","<%=jibunAddr%>","<%=zipNo%>");
		    window.close();
	    }
    }
    </script>
	
</head>
<body onload="init()">
    <form id="form" runat="server" method="post">   
		<input type="hidden" id="confmKey" name="confmKey" value=""/>
		<input type="hidden" id="returnUrl" name="returnUrl" value=""/>
		<input type="hidden" id="resultType" name="resultType" value=""/>
		<!-- 해당시스템의 인코딩타입이 EUC-KR일경우에만 추가 START-->
		<!--input type="hidden" id="encodingType" name="encodingType" value="EUC-KR"/-->
		<!-- 해당시스템의 인코딩타입이 EUC-KR일경우에만 추가 END-->
    </form>         
</body>
</html>
