<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="user_excel.aspx.cs" Inherits="CLT.WEB.UI.LMS.MANAGE.user_excel" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script language="javascript" type="text/javascript">
    function chkJumin(fldId1, error_msg) 
    {
        var jumin = fldId1.value.replace(/\s/gi, "");
        
        // check JuminNumber-type and sex_digit 
        fmt = /^\d{6}-[1234]\d{6}$/;
        if (!fmt.test(jumin)) 
        {
            alert(error_msg);
            fldId1.focus() ;
            return true;
        }

        // check date-type
        birthYear = (jumin.charAt(7) <= "2") ? "19" : "20";
        birthYear += jumin.substr(0, 2);
        birthMonth = jumin.substr(2, 2) - 1;
        birthDate = jumin.substr(4, 2);
        birth = new Date(birthYear, birthMonth, birthDate);

        if ( birth.getYear() % 100 != jumin.substr(0, 2) ||
        birth.getMonth() != birthMonth ||
        birth.getDate() != birthDate) 
        {
            alert(error_msg);
            fldId1.focus();     
            return true;
        }

        // Check Sum code
        buf = new Array(13);
        for (i = 0; i < 6; i++) buf[i] = parseInt(jumin.charAt(i));
        for (i = 6; i < 13; i++) buf[i] = parseInt(jumin.charAt(i + 1));

        multipliers = [2,3,4,5,6,7,8,9,2,3,4,5];
        for (i = 0, sum = 0; i < 12; i++) sum += (buf[i] *= multipliers[i]);

        if ((11 - (sum % 11)) % 10 != buf[12]) 
        {
            alert(error_msg);
            fldId1.focus();        
            return true;
        }

        return false;
    }
    
    function fnValidateForm()
    {   
        var chkCnt = 1;
        var chks = new Array(); 
        var gridview = document.getElementById('<%=grdItem.ClientID %>'); 
        chks = gridview.getElementsByTagName('input'); 
        
        for (var i = 0; i < chks.length; i++) { 
            if (chks.item(i).className == "chk_sel" && chks.item(i).checked)
            { 
                var grdIds = chks.item(i).id.split('_');
                var grdItemCtl = grdIds[0] + '_' + grdIds[1] + '_' + grdIds[2] + '_' + grdIds[3];
                //alert(grdItemCtl);
                
                if (isEmpty(document.getElementById(grdItemCtl+'_txtUserNMKor'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "이름" }, new string[] { "Name" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if (isEmpty(document.getElementById(grdItemCtl+'_txtPesonalNo'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "주민등록번호" }, new string[] { "Registration" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if (isEmpty(document.getElementById(grdItemCtl+'_txtUserNMEngFirst'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "영문명(First)" }, new string[] { "English Name(First)" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if (isEmpty(document.getElementById(grdItemCtl+'_txtUserNMEngLast'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "영문명(Last)" }, new string[] { "English Name(Last)" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                //if (isEmpty(document.getElementById(grdItemCtl+'_txtZipCode'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "우편번호" }, new string[] { "Postal Code" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                //if (isEmpty(document.getElementById(grdItemCtl+'_txtAddress1'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "주소" }, new string[] { "Address" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                //if (isEmpty(document.getElementById(grdItemCtl+'_txtPhone'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "연락처" }, new string[] { "Phone" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if (isEmpty(document.getElementById(grdItemCtl+'_txtMobilePhone'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A012", new string[] { "휴대폰" }, new string[] { "Mobile Phone" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;	            
	            
                //if (isSelect(document.getElementById(grdItemCtl+'_ddlDutyStep'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { "직급" }, new string[] { "Grade" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                //if (isSelect(document.getElementById(grdItemCtl+'_ddlTraineeClass'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { "훈련생 구분" }, new string[] { "Trainee Classification" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                
            	// 주민번호 체크
	            if (chkJumin(document.getElementById(grdItemCtl+'_txtPesonalNo'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A016", new string[] { "주민등록번호" }, new string[] { "Registration" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                
	            // 길이체크
                if(isMaxLenth(document.getElementById(grdItemCtl+'_txtUserNMKor'),75,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { "이름","75","50" }, new string[] { "Name","75","50" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if(isMaxLenth(document.getElementById(grdItemCtl+'_txtUserNMEngFirst'),50,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { "영문명(First)","50","30" }, new string[] { "Eng First Name","50","30" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if(isMaxLenth(document.getElementById(grdItemCtl+'_txtUserNMEngLast'),50,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { "영문명(Last)","50","30" }, new string[] { "Eng First Name","50","30" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
   	            //if(isMaxLenth(document.getElementById(grdItemCtl+'_txtAddress1'),100,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { "주소","100","66" }, new string[] { "Address","100","66" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	            //if(isMaxLenth(document.getElementById(grdItemCtl+'_txtAddress2'),100,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { "주소","100","66" }, new string[] { "Address","100","66" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
	            //if(isMaxLenth(document.getElementById(grdItemCtl+'_txtPhone'),20,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { "전화번호","20","13" }, new string[] { "전화번호","20","13" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if(isMaxLenth(document.getElementById(grdItemCtl+'_txtEMail'),50,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { "E-Mail","50","30" }, new string[] { "E-Mail","50","30" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                if(isMaxLenth(document.getElementById(grdItemCtl+'_txtMobilePhone'),20,'<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { "휴대폰 번호","20","13" }, new string[] { "Mobile Phone","20","13" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
                
                chkCnt++;
            } 
            else if(chks.item(i).className == "chk_sel")
            {
                chkCnt++;
            }
        }
        
        return true;
    }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <asp:HiddenField ID="hidCompany_id" runat="server" /> 
    

    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">

        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="개인회원 엑셀 업로드" meta:resourcekey="lblMenuTitle" /></h3>

        <!-- 검색 -->
        <div class="message-box default center">
            
        <table>
            <tr>
                <td>

                    <!-- 파일 첨부 인풋 -->
                    <div class="file-box">
                        <input class="upload-name" value="" disabled="disabled" />
                        <label for="<%=fu_excel.ClientID %>"><asp:Label ID="lblSearchFile" runat="server" Text="찾아보기" meta:resourcekey="lblSearchFile" /></label>
                        <asp:FileUpload ID="fu_excel" runat="server" CssClass="upload-hidden" />
                    </div>
                    <!-- 파일 첨부 인풋 -->

                </td>
            </tr>
            <tr id="trMESSAGE" runat="server">
                <td align="left" style="margin: 15px">

                    <p><asp:Label ID="lbl0" runat="server" meta:resourcekey="lblGuide0" Text="&nbsp;1. Excel Template : [ Excel Template ]버튼 클릭하여, EXCEL파일 서식을 다운로드 합니다." /></p>
                    <p><asp:Label ID="lbl1" runat="server" meta:resourcekey="lblGuide1" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;작성방법" /></p>
                    <p><asp:Label ID="lbl2" runat="server" meta:resourcekey="lblGuide2" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;①  주민등록번호 : 13자리 입력 '-' 없이 입력" /></p>
                    <p><asp:Label ID="lbl3" runat="server" meta:resourcekey="lblGuide3" Text="&nbsp;" /></p>
                    <p><asp:Label ID="lbl4" runat="server" meta:resourcekey="lblGuide4" Text="&nbsp;2. [찾아보기] 버튼을 클릭하여, 작성한 EXCEL파일을 선택합니다." /></p>
                    <p><asp:Label ID="lbl5" runat="server" meta:resourcekey="lblGuide5" Text="&nbsp;" /></p>
                    <p><asp:Label ID="lbl6" runat="server" meta:resourcekey="lblGuide6" Text="&nbsp;3. [Upload]버튼을 클릭합니다." /></p>
                    <p><asp:Label ID="lbl7" runat="server" meta:resourcekey="lblGuide7" Text="&nbsp;" /></p>
                    <p><asp:Label ID="lbl8" runat="server" meta:resourcekey="lblGuide8" Text="&nbsp;4. [Save] 버튼을 클릭하여 개인회원을 등록합니다." /></p>

                </td>
            </tr>
        </table>
                
        </div>
    
        <div class="button-box right">
            
            <asp:Button ID="btnExcel" runat="server" Text="Upload" CssClass="button-default blue" OnClick="btnExcel_Click" meta:resourcekey="btnExcelResource" /> 
            <asp:Button ID="btnSend" runat="server" Text="Save" CssClass="button-default" OnClick="btnSend_Click" OnClientClick="return fnValidateForm();" meta:resourcekey="btnSendResource" /> 

            <input type="button" name="btnExcelTemplate" value="Excel Template" onclick="location.href='/file/download/user_excel.xls';" id="btnExcelTemplate" class="button-default" />

        </div>

        <div class="gm-table data-table list-type">
            <C1WebGrid:C1WebGrid ID="grdItem" runat="server" AllowSorting="True" AllowColSizing="False" AutoGenerateColumns="False" OnItemDataBound="grdItem_ItemDataBound" OnItemCreated="grdItem_ItemCreated">
                <Columns>
                    
                    <C1WebGrid:C1TemplateColumn>
                        <HeaderTemplate>
                            <input type="checkbox" id="chk_all_sel" name="chk_all_sel" onclick="CheckAll(this, 'chk_sel');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" id="chk_sel" name="chk_sel" runat="server" class="chk_sel"/>
                        </ItemTemplate>
                        <ItemStyle Width="25px" />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1TemplateColumn>
                        <HeaderTemplate>
                            <asp:Label ID="lblUserNMKor" runat="server" text="이름" />
                            <span class="required"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="txtUserNMKor" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "이름")%>' Width="60px" ></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1TemplateColumn>
                        <HeaderTemplate>
                            <asp:Label ID="lblPesonalNo" runat="server" text="주민등록번호(-포함)" />
                            <span class="required"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="txtPesonalNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "주민등록번호")%>' Width="110px" ></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1TemplateColumn>
                        <HeaderTemplate>
                            <asp:Label ID="lblUserNMEngFirst" runat="server" text="영문(성)" />
                            <span class="required"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="txtUserNMEngFirst" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "영문 First")%>' Width="60px" ></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1TemplateColumn>
                        <HeaderTemplate>
                            <asp:Label ID="lblUserNMEngLast" runat="server" text="영문(이름)" />
                            <span class="required"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="txtUserNMEngLast" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "영문 Last")%>' Width="70px"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>     
                    
                    <C1WebGrid:C1TemplateColumn HeaderText="우편번호(-포함)">
                        <ItemTemplate>
                            <asp:TextBox ID="txtZipCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "우편번호")%>' Width="85px"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1TemplateColumn HeaderText="주소">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAddress1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "주소")%>' Width="165px"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1TemplateColumn HeaderText="직급">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlDutyStep" runat="server" Width="80px"></asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1TemplateColumn HeaderText="연락처(-포함)">
                        <ItemTemplate>
                            <asp:TextBox ID="txtPhone" runat="server" Width="90px" Text='<%# DataBinder.Eval(Container.DataItem, "연락처")%>'/>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1TemplateColumn HeaderText="E-Mail">
                        <ItemTemplate>
                            <asp:TextBox ID="txtEMail" runat="server" Width="110px" Text='<%# DataBinder.Eval(Container.DataItem, "E-Mail")%>'/>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>           
                    
                    <C1WebGrid:C1TemplateColumn>
                        <HeaderTemplate>
                            <asp:Label ID="lblMobilePhone" runat="server" text="휴대폰(-포함)" />
                            <span class="required"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="txtMobilePhone" runat="server" Width="90px" Text='<%# DataBinder.Eval(Container.DataItem, "휴대폰")%>'/>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>           
                    
                    <C1WebGrid:C1TemplateColumn HeaderText="훈련생 구분">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlTraineeClass" runat="server" Width="110px" ></asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>
                    
                    <C1WebGrid:C1TemplateColumn HeaderText="고용보험 취득일">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAcquisition" runat="server" MaxLength="10" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem, "고용보험취득일")%>'/>
                        </ItemTemplate>
                        <ItemStyle />
                    </C1WebGrid:C1TemplateColumn>

                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle  Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>
        </div>
        
        <div class="gm-paging">

        </div>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->


</asp:Content>