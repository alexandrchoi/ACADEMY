<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="sms_send.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPLICATION.sms_send" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" src="/asset/smarteditor/js/service/HuskyEZCreator.js"></script>
    <script type="text/javascript" language="javascript">
        function btnCheck()
        {
            subject = document.getElementById("<%= txtContent.ClientID %>").value;
            
            if(subject == "")
            {
                alert('내용을 입력하세요');
                return false;
            }
        }
        function setLoad()
	    {
		    document.getElementById("<%= txtContent.ClientID %>").value=document.getElementById("IMOTICONTENT").value;
	    }
        // 이모티콘 문자 바이트 체크
        function CheckLen(str)
        {
            var temp;
            var memocount;
            var co;
            var mess;
            
            memocount = 0;
            mess = 0;
            co = 0;
            
            document.getElementById("<%=txtContent.ClientID%>").value=str;
            len = document.getElementById("<%=txtContent.ClientID%>").value.length;
            
            for(k=0;k<len;k++)
            {
                temp = document.getElementById("<%=txtContent.ClientID%>").value.charAt(k);
                if(escape(temp).length > 4)
                {
                    memocount += 2;
                }
                else
                {
                    memocount++;
                }
            }
            
            //memocount 내요을 COUNTBYTE 와 HiddenField 에 기억 한다.
            document.getElementById("COUNTBYTE").value = memocount;
            document.getElementById("<%=HiddenBYTE.ClientID %>").value = memocount;

            // 80 바이이트가 넘으면 메시지 뿌려주고 2통으로 표시
            // 160 바이트가 넘으면 입력 안됨..
            if (memocount >80)
            {	
                coa = document.getElementById("<%= HiddenAlert.ClientID %>").value;
                
                if(coa == 0)
                {
                    alert("Input Into 80Byte Excess 2 Messages");
                    document.getElementById("<%=HiddenAlert.ClientID %>").value = co + 1;
                }
                
                mess = mess + 1;
                document.getElementById("MESS").value = mess + 1;
                document.getElementById("Input").value ="/ 160Byte";
                document.getElementById("<%=HiddenMESS.ClientID %>").value = document.getElementById("MESS").value;
                document.getElementById("<%=HiddenBYTE2.ClientID %>").value = "/ 160Byte";
                
                if(memocount > 160)
                {
                    alert("Message can input into 160 byte only.");
                    document.getElementById("<%= txtContent.ClientID %>").value = oldText;
                    document.getElementById("COUNTBYTE").value = oldCount;
                    return;
                }
                else
                {
                    oldText = document.getElementById("<%=txtContent.ClientID%>").value;	
                    oldCount = memocount;
                } 
                
                return ; 
            }
            else
            {
                oldText = document.getElementById("<%=txtContent.ClientID%>").value;	
                oldCount = memocount;
            }
             
            if(str!="")
            {
                document.getElementById("COUNTBYTE").value=memocount;
            }
            else
            {
                return memocount;
            }
        }
        
        //직접 입력한 문자 바이트 체크
        function CheckLen1()
        {
            var temp;
            var memocount;
            var mess;
            var co;
                    
            memocount = 0;
            mess = 0;
            co = 0;
            len = document.getElementById("<%=txtContent.ClientID%>").value.length;

            for(k=0;k<len;k++)
            {
                temp = document.getElementById("<%=txtContent.ClientID%>").value.charAt(k);

                if(escape(temp).length > 4)
                {
                    memocount += 2;
                }
                else
                {
                    memocount++;
                }
            }
            
            // memocount를 COUNTBYTE 와 HiddenField 에 기억한다.
            document.getElementById("COUNTBYTE").value = memocount;
            document.getElementById("<%=HiddenBYTE.ClientID %>").value = memocount;
                    
            // 80 바이트가 넘으면 메시지를 뿌려주고 2통으로 입력된다.
            //160 바이트가 넘으면 입력안된다.                 
            if (memocount >80)
            {	
                coa = document.getElementById("<%=HiddenAlert.ClientID %>").value;
                
                if(coa == 0)
                {
                    alert("Input Into 80Byte Excess 2 Messages");
                    document.getElementById("<%=HiddenAlert.ClientID %>").value = co + 1;
                }
                
                mess = mess + 1;
                document.getElementById("MESS").value = "MAIL"
                document.getElementById("Input").value = "/ 160Byte";
                document.getElementById("<%=HiddenMESS.ClientID %>").value = document.getElementById("MESS").value;
                document.getElementById("<%=HiddenBYTE2.ClientID %>").value = "/ 160Byte";
                
                if (memocount >160)
                {	
                    alert("Message can input into 160 byte only.");
                    document.getElementById("<%=txtContent.ClientID%>").value = oldText;		
                    document.getElementById("COUNTBYTE").value = oldCount;
                    return;
                }
                else
                {
                    oldText = document.getElementById("<%=txtContent.ClientID%>").value;	
                    oldCount = memocount;
                } 
           
                return ; 
            }
            else
            {
                oldText = document.getElementById("<%=txtContent.ClientID%>").value;	
                oldCount = memocount;
                document.getElementById("MESS").value = "SMS";
                document.getElementById("Input").value = "/ 80Byte";
            }
                       
            return memocount;
        }
        //화면 포커를 기억하는 자바스크립트
        function f_chk()
        {
            HiddenField1 = document.getElementById("<%=HiddenField1.ClientID %>").value;
            HiddenField2 = document.getElementById("<%=HiddenField2.ClientID %>").value;
            HiddenField3 = document.getElementById("<%=HiddenField3.ClientID %>").value;
            HiddenField4 = document.getElementById("<%=HiddenField4.ClientID %>").value;
                      
            if(HiddenField1 == "1")
            { 
                check.style.display="";
                minus.style.display="";
                plus.style.display="none";         
            }
            else 
            {
                check.style.display="none";
                minus.style.display="none";
                plus.style.display="";            
            }
        
            if(HiddenField2 == "1")
            { 
                reject.style.display="";
                minus2.style.display="";
                plus2.style.display="none";         
            }
            else 
            {
                reject.style.display="none";
                minus2.style.display="none";
                plus2.style.display="";
            }
            
            if(HiddenField3 == "1")
            {
                document.getElementById("GROUP").style.display = "block";
                document.getElementById("TOPNOIMG").style.display="none";
		    	document.getElementById("TOPNO").style.display="none";
            }
            else
            {
                document.getElementById("GROUP").style.display = "none";
                document.getElementById("TOPNOIMG").style.display="block";
		    	document.getElementById("TOPNO").style.display="block";
            }
            
            if(HiddenField4 == "1")
            {
                document.getElementById("View").style.display="none"; 
                document.getElementById("ExcelView").style.display="block";
                document.getElementById("MESS").value = document.getElementById("<%=HiddenMESS.ClientID %>").value;
                document.getElementById("COUNTBYTE").value = document.getElementById("<%=HiddenBYTE.ClientID %>").value;
                document.getElementById("Input").value = document.getElementById("<%=HiddenBYTE2.ClientID %>").value;
            }
            else
            {
                document.getElementById("View").style.display="block"; 
                document.getElementById("ExcelView").style.display="none";
            }
        }
        //창접고 펼치기 
        ok=0;     
        function show(what)
        {  
            HiddenField1 = document.getElementById("<%=HiddenField1.ClientID %>");
            
            if(ok)
            {
                ok=0;
                what.style.display="none";
                minus.style.display="none";
                plus.style.display="";
                HiddenField1.value="0";
            }    
            else
            {
                ok=1;
                what.style.display="";
                minus.style.display="";
                plus.style.display="none";
                HiddenField1.value="1";
            }
        }
        ok2=0; 
        function show2(what2)
        {
            HiddenField2 = document.getElementById("<%=HiddenField2.ClientID %>");
            
            if(ok2)
            {
                ok2=0;
                what2.style.display="none";
                minus2.style.display="none";
                plus2.style.display="";
                HiddenField2.value="0";
            }
            else
            {
                ok2=2;
                what2.style.display="";
                minus2.style.display="";
                plus2.style.display="none";
                HiddenField2.value="0";
            }              
        }
        //숫자체크
        function addDash(objNumBox)
        {
           var numBoxValue = objNumBox.value;
            
            for(var i=0; i<numBoxValue.length; i++)
            {
                if(isNaN(numBoxValue.charAt(i)))
                {
                    alert("숫자만 입력해주세요.");
                    
                    objNumBox.value = '';
                    
                    for(var j=0; j<i; j++)
                    {
                        objNumBox.value += numBoxValue.charAt(j);
                    }
                    
                    return;
                }
            }
        }
        // 해당 폼필드의 커서위치를 저장한다(기억).
        function storeCaret(oTextElement)
        {
	        if (oTextElement == null)
	        {
		        oTextElement = document.getElementById("<%=txtContent.ClientID%>");
	            
	            if (oTextElement.isTextEdit)
	            {
		            oTextElement.caretPos = document.selection.createRange();
		        }
		    }
        }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <asp:HiddenField ID="HiddenTOID" runat="server" />
    <asp:HiddenField ID="HiddenTONAME" runat="server" />
    <asp:HiddenField ID="HiddenTOPNO" runat="server" />
    <asp:HiddenField ID="HiddinTOCOMPANY" runat="server" />
    <asp:HiddenField ID="HiddenDEST" runat="server" />
    <asp:HiddenField ID="HiddenTODEPT" runat="server" />
    <asp:HiddenField ID="HiddenAlert" runat="server" Value="0" />
    <asp:HiddenField ID="HiddenMESS" runat="server" Value="1" />
    <asp:HiddenField ID="HiddenBYTE" runat="server" />
    <asp:HiddenField ID="HiddenBYTE2" runat="server" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="HiddenField2" runat="server" />
    <asp:HiddenField ID="HiddenField3" runat="server" />
    <asp:HiddenField ID="HiddenField4" runat="server" />
    <asp:HiddenField ID="HiddenField5" runat="server" />
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="SMS 발송" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board sms-send">

        <!-- SMS 발송항목 부분 시작 !-->
        <div class="notice-text right"><asp:Label ID="lblRequired" runat="server" Text="필수입력항목" meta:resourcekey="lblRequired" /><span class="required"></span></div>
        <div class="board-list gm-table data-table read-type">

            <table>
                <caption>표정보 - 대상조건, 받는사람, 제목, 내용 등을 제공합니다.</caption>
                <colgroup>
                    <col width="20%">
                    <col width="20%">
                    <col width="*">
                </colgroup>

                <tbody>
                <tr>
                    <th scope="row" rowspan="3"><asp:Label ID="lblTarget" runat="server" Text="대상조건 선택" meta:resourcekey="lblTarget" /></th>
                    <th scope="row"><asp:Label ID="lblPeriod" Text="조회기간" runat="server" meta:resourcekey="lblPeriod" /></th>
                    <td>
                        <asp:TextBox ID="txtCus_From" runat="server" MaxLength="10" CssClass="datepick w180" />
                        <span class="gm-text2">~</span>
                        <asp:TextBox ID="txtCus_To" runat="server" MaxLength="10" CssClass="datepick w180" />
                        <asp:Button ID="btnSearch" runat="server" Text="검색" CssClass="button-default blue" OnClick="btnSearch_Click" meta:resourcekey="btnSearchResource" />
                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblCourseName" Text="과정명" runat="server" meta:resourcekey="lblCourseName" /></th>
                    <td>
                        <asp:DropDownList ID="ddlCus_NM" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCus_NM_OnSelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblCusDate" runat="server" Text="교육기간" meta:resourcekey="lblCusDate" /></th>
                    <td>
                        <asp:DropDownList ID="ddlCus_Date" runat="server" />
                        <asp:Button ID="btnAdd" runat="server" CssClass="button-underline" Text="List Add" OnClick="btnAdd_OnClick" meta:resourcekey="btnAddResource" />
                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblSMSTitle" runat="server" Text="제목" meta:resourcekey="lblSMSTitle" /></th>
                    <td colspan="2">
                        <asp:TextBox ID="txtSMS_Title" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblRec_MobilePhone" runat="server" Text="회신번호" meta:resourcekey="lblRec_MobilePhone" /></th>
                    <td colspan="2">
                        <asp:TextBox ID="txtRec_MobilePhone" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblDatetosend" runat="server" Text="발송방법" meta:resourcekey="lblDatetosend" /></th>
                    <td colspan="2">
                        <label class="radio-box">
                            <asp:RadioButton ID="rbSentType" GroupName="SMSType" Text="지금보내기" meta:resourcekey="rbSentType" Checked="true" AutoPostBack="true" OnCheckedChanged="RadioButton_OnCheckedChanged" runat="server" />
                            <span class="radiomark"></span>
                        </label>

                        <label class="radio-box">
                            <asp:RadioButton ID="rbBooking" GroupName="SMSType" Text="보내기 예약" meta:resourcekey="rbBooking" Checked="false" AutoPostBack="true" OnCheckedChanged="RadioButton_OnCheckedChanged" runat="server" />
                            <span class="radiomark"></span>
                        </label>
                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblTime" runat="server" Text="보낼시간" meta:resourcekey="lblTime" /></th>
                    <td colspan="2" class="many-select">
                        <asp:DropDownList ID="ddlYear" runat="server" />
                        <asp:Label ID="lblYear" text="년" meta:resourcekey="lblYear" runat="server" />
                        <asp:DropDownList ID="ddlMonth" runat="server" />
                        <asp:Label ID="lblMonth" text="월" meta:resourcekey="lblMonth" runat="server" />
                        <asp:DropDownList ID="ddlDay" runat="server" />
                        <asp:Label ID="lblDay" text="일" meta:resourcekey="lblDay" runat="server" />
                        <asp:DropDownList ID="ddlHour" runat="server" />
                        <asp:Label ID="lblHour" text="시" meta:resourcekey="lblHour" runat="server" />
                        <asp:DropDownList ID="ddlMinute" runat="server" />
                        <asp:Label ID="lblMinute" text="분" meta:resourcekey="lblMinute" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblNameTitle" runat="server" Text="수신자 이름" meta:resourcekey="lblNameTitle" /></th>
                    <td colspan="2">
                        <asp:TextBox ID="txtName" runat="server" CssClass="w50per" ToolTip="수신자 이름" Text="" placeholder="수신자의 이름을 입력하세요" />
                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblPhoneTitle" runat="server" Text="수신자 전화번호" meta:resourcekey="lblPhoneTitle" /></th>
                    <td colspan="2">
                        <asp:TextBox ID="txtMobilePhone" runat="server" CssClass="w50per" ToolTip="전화번호(지역번호 포함)" Text="" placeholder="수신자의 전화번호를 입력하세요" />
                        <asp:Button ID="btnPlus" runat="server" CssClass="button-underline" Text="Add" OnClick="btn_Plus_OnClick" meta:resourcekey="btnPlusResource" />
                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblTo" runat="server" Text="받는사람" meta:resourcekey="lblTo" /></th>
                    <td colspan="2">
                        <div class="button-group right">
                            <asp:Label ID="lblTotal1" runat="server" Text="총" CssClass="recipient-list" meta:resourcekey="lblTotal1" />
                            <strong><asp:Label ID="lblSendCnt" runat="server" Text="0" CssClass="recipient-list" /></strong>
                            <asp:Label ID="lblTotal2" runat="server" Text="명" CssClass="recipient-list" meta:resourcekey="lblTotal2" />
                            <asp:Button ID="btnDelete" Text="Delete" CssClass="button-default" OnClick="Delete_OnClick" runat="server" meta:resourcekey="btnDeleteResource" />
                            <asp:Button ID="btnAllDelete" Text="Clear" CssClass="button-default" OnClick="Delete_OnClick" runat="server" meta:resourcekey="btnClearResource" />
                        </div>
                        <asp:ListBox ID="lbSentlist" SelectionMode="Multiple" Width="100%" Height="150" runat="server" />

                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblSMSContent" runat="server" Text="내용" meta:resourcekey="lblSMSContent" /></th>
                    <td colspan="2">
                        <textarea onkeydown="storeCaret(this)" onkeyup="CheckLen1()" onclick="storeCaret(this)"
                                  onselect="storeCaret(this)" rows="10" id="txtContent"  runat="server"></textarea>
                        <br />
                        <input name="MESS" id="MESS" readonly="readonly" style="border-top-width: 0px; border-left-width: 0px;
                                                        font-size: 13px; border-left-color: #8eaffc; background: none transparent scroll repeat 0% 0%;
                                                        border-bottom-width: 0px; border-bottom-color: #8eaffc; width: 27px; border-top-color: #8eaffc;
                                                        font-family: 돋움; height: 18px; border-right-width: 0px; border-right-color: #8eaffc; left: 1px; position: relative;"
                                                        value="SMS" /> 
                        &nbsp
                        <input onfocus="setLoad()"style="border-top-width: 0px;
                                                     border-left-width: 0px; font-size: 13px; border-left-color: #ffffff; background: none transparent scroll repeat 0% 0%;
                                                     border-bottom-width: 0px; border-bottom-color: #ffffff; width: 18px; border-top-color: #ffffff;
                                                     font-family: 돋움; height: 18px; border-right-width: 0px; border-right-color: #ffffff;
                                                     border-bottom-style: none" size="2" value="0" name="COUNTBYTE" id="COUNTBYTE" readonly="readonly" />
                        &nbsp;
                        <input name="Input" id="Input" onkeyup="getByteLen(this.value)" readonly="readonly" style="border-top-width: 0px;
                                                     border-left-width: 0px; font-size: 13px; border-left-color: #ffffff; background: none transparent scroll repeat 0% 0%;
                                                     border-bottom-width: 0px; border-bottom-color: #ffffff; width: 53px; border-top-color: #ffffff;
                                                     font-family: 돋움; height: 18px; border-right-width: 0px; border-right-color: #ffffff;
                                                     border-bottom-style: none" value="/ 80Byte" />
                    </td>
                </tr>
                <tr>
                    <th scope="row"><asp:Label ID="lblSMSAttach" runat="server" Text="Import SMS 목록" meta:resourcekey="lblSMSAttach" /></th>
                    <td colspan="2">
                        <div class="file-box">
                           <input class="upload-name" value="File Name" disabled="disabled">
                           <label for="<%=fu_excel.ClientID %>"><asp:Label ID="lblSearchFile" runat="server" Text="찾아보기" meta:resourcekey="lblSearchFile" /></label>
                           <asp:FileUpload ID="fu_excel" runat="server" CssClass="upload-hidden" />
                        </div>
                        
                        <!-- 파일 첨부 인풋 -->
                        <asp:Button ID="btnExcelUpload" runat="server" OnClick="btnExcelUpload_OnClick" CssClass="button-default" Text="Upload" meta:resourcekey="btnExcelUploadResource" />
                        <asp:Button ID="btnExcelTemplate" runat="server" OnClientClick="javascript:location.href='/file/download/sms.xls'" CssClass="button-default" Text="Excel Template" meta:resourcekey="btnExcelTemplateResource" />      
                    </td>
                </tr>
                </tbody>
            </table>

        </div>
        <!-- SMS 발송항목 부분 끝 !-->

        <div class="button-group right">
		    <asp:Button ID="btnSend" runat="server" CssClass="button-default blue" Text="Send" OnClick="btnSend_OnClick" meta:resourcekey="btnSendResource" />
		    <asp:Button ID="btnCancel" runat="server" CssClass="button-default" Text="Cancel" OnClick="btnCancel_Click" meta:resourcekey="btnCancelResource" />
        </div>
    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>