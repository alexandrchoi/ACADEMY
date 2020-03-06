<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="issuing.aspx.cs" Inherits="CLT.WEB.UI.LMS.EDUM.issuing" Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <%--<script type="text/javascript" language="javascript" src="/scripts/lmsCommon.js"></script>--%>

    <script language = "javascript" type="text/javascript">
       function OpenReport(strReportAddr, rWidth)
        {   
            var childWindow;
            childWindow = window.open(strReportAddr, "Report", "menubar=0, status=0, toolbar=1, scrollbars=1, width="+rWidth+", height=850");
            childWindow.focus();
        }
        // 체크박스 전체선택/해제 
        function CheckAllNew(checkAllBox, chkName)                         
        {   
            var frm = document.forms[0]                         
            var ChkState = checkAllBox.checked;
            for(i=0;i< frm.length;i++)                         
            {   
                e = frm.elements[i];   
                if (e.type == 'checkbox' && e.name.indexOf(chkName) != -1)
                    if (!e.disabled) {
                        e.checked = ChkState;
                        chk_new_reason(e);
                    }
            }
        } 

        var chk_new_reason = function(el){
            var check = $(el).is(':checked');
            var eltx = $(el).parent().parent().parent().find('input[type=text]').first();
            if (check) {
                $(eltx).val("신규발급");
                $(eltx).attr("readonly",true); 
            }
            else {
                $(eltx).val("");
                $(eltx).removeAttr("readonly"); 
            }
        };
   </script>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="Label1" runat="server" meta:resourcekey="lblMenuTitle" /></h3>
        <!--<p><asp:Label ID="lbl0" runat="server" meta:resourcekey="lblGuide0" Text="" />
           <asp:Label ID="lbl1" runat="server" meta:resourcekey="lblGuide1" Text="" /></p>-->

        <!-- 검색 -->
        <!--div class="message-box default center">
        </div>-->


        <!-- 상단 버튼-->
        <div class="button-box right">
        </div>


        <!-- 내용-->
        <section class="section-board">
            <CLTWebControl:PageInfo ID="PageInfo1" runat="server"  Visible="false" />
            <div class="gm-table data-table list-type">
                <C1WebGrid:C1WebGrid ID="grdList" runat="server" AllowColSizing="True" CssClass="grid_main"
                    AutoGenerateColumns="False" OnItemDataBound="grdUserList_ItemDataBound" DataKeyField="KEYS"
                    OnItemCreated="grdList_ItemCreated">
                    <Columns>

                        <C1WebGrid:C1TemplateColumn>
                            <HeaderTemplate>
                                <input type="checkbox" id="chk_all_sel" name="chk_all_sel" onclick="CheckAll(this, 'chk_sel');" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="checkbox" id="chk_sel" name="chk_sel" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="20px"/>
                        </C1WebGrid:C1TemplateColumn>

                        <C1WebGrid:C1BoundColumn DataField="COMPANY_NM" HeaderText="회사명">
                            <ItemStyle Width="" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="COURSE_NM" HeaderText="과정명">
                            <ItemStyle Width="" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="CERTIFICATE_NO" HeaderText="증서번호">
                            <ItemStyle Width="" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="STEP_NAME" HeaderText="직책">
                            <ItemStyle Width="" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="USER_NM_KOR" HeaderText="성명">
                            <ItemStyle Width="" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="PERSONAL_NO" HeaderText="주민등록번호">
                            <ItemStyle Width="" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="IS_PIC_FILE" HeaderText="사진">
                            <ItemStyle Width="" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1TemplateColumn HeaderText="회원정보수정">
                            <ItemTemplate>
                                <input type="button" value="수정" class="button-default blue" onclick="javascript:openPopWindow('/manage/user_edit.aspx?EDITMODE=EDIT&USER_ID=<%# DataBinder.Eval(Container.DataItem, "user_id")%>&MenuCode=212','user_edit', '1280', '821');" />
                                <asp:Image ID="img_pic_file" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "PIC_FILE")%>' Visible="false" />
                                <asp:FileUpload ID="fileUplaod" runat="server" Width="200px" Visible="false" />
                            </ItemTemplate>
                            <ItemStyle Width="" />
                        </C1WebGrid:C1TemplateColumn>
                        
                        <C1WebGrid:C1TemplateColumn HeaderText="발급사유">
                            <ItemTemplate>
                                <input type="text" id="txtReason" name="txtReason" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "REASON")%>'  />
                                <input type="hidden" id="txtCertKey" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "CERTIFICATE_KEY")%>' />
                                <input type="hidden" id="txtCertName" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "CERTIFICATE_NAME")%>' />
                                <input type="hidden" id="txtDisabled" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "IS_DISABLED")%>' />
                            </ItemTemplate>
                            <ItemStyle Width="120px"/>
                        </C1WebGrid:C1TemplateColumn>

                    </Columns>
                    <HeaderStyle />
                    <ItemStyle Wrap="true" />
                    <AlternatingItemStyle />
                </C1WebGrid:C1WebGrid>
            </div>

             <div class="gm-paging">
                <CLTWebControl:PageNavigator ID="PageNavigator1" runat="server" Visible="false" OnOnPageIndexChanged="PageNavigator1_OnPageIndexChanged" />
            </div>


            <!-- 하단 버튼-->
            <div class="button-group center">
                <!--<asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button-main-rnd lg blue" OnClick="btn_save_Click" OnClientClick="return fnValidateForm();" />-->
                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="button-main-rnd lg blue" OnClick="btnPrint_Click" /> 
            </div>
        </section>
    </div>
</asp:Content>

