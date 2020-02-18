<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="opencourse_edit.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.opencourse_edit" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <script type="text/javascript" language="javascript">
    function fnValidation()
    {
        /* null 체크 */
        if(isEmpty(document.getElementById('<%=txtCourseNM.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblCourseNm.Text }, new string[] { lblCourseNm.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
//        if(isEmpty(document.getElementById('<%=ddlYear.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblYear.Text }, new string[] { lblYear.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isEmpty(document.getElementById('<%=ddlInstitution.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblInstitution.Text }, new string[] { lblInstitution.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isEmpty(document.getElementById('<%=txtScore.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblScore.Text }, new string[] { lblScore.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isEmpty(document.getElementById('<%=txtApplyBeginDt.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblApplyDt.Text }, new string[] { lblApplyDt.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isEmpty(document.getElementById('<%=txtApplyEndDt.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblApplyDt.Text }, new string[] { lblApplyDt.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isEmpty(document.getElementById('<%=txtBeginDt.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblBeginDt.Text }, new string[] { lblBeginDt.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isEmpty(document.getElementById('<%=txtEndDt.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblBeginDt.Text }, new string[] { lblBeginDt.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isEmpty(document.getElementById('<%=txtProgressRate.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblProgressRate.Text }, new string[] { lblProgressRate.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isEmpty(document.getElementById('<%=txtFinalTest.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblFinalTest.Text }, new string[] { lblFinalTest.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isEmpty(document.getElementById('<%=txtMinCount.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblMinCount.Text }, new string[] { lblMinCount.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isEmpty(document.getElementById('<%=txtMaxCount.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblMaxCount.Text }, new string[] { lblMaxCount.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isEmpty(document.getElementById('<%=txtMaxCount.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblMaxCount.Text }, new string[] { lblMaxCount.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

        /* 문자열 size 체크 */
        if(isMaxLenth(document.getElementById('<%=txtEduFee.ClientID %>'), 10, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A011", new string[] { lblEduFee.Text, "10" }, new string[] { lblEduFee.Text, "10" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtScore.ClientID %>'), 3, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A011", new string[] { lblScore.Text, "3" }, new string[] { lblScore.Text, "3" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtSupportFee.ClientID %>'), 10, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A011", new string[] { lblSupportFee.Text, "10" }, new string[] { lblSupportFee.Text, "10" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtSupportCompFee.ClientID %>'), 10, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A011", new string[] { lblSupportCompFee.Text, "10" }, new string[] { lblSupportCompFee.Text, "10" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtMinCount.ClientID %>'), 4, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A011", new string[] { lblMinCount.Text, "4" }, new string[] { lblMinCount.Text, "4" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtMaxCount.ClientID %>'), 4, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A011", new string[] { lblMaxCount.Text, "4" }, new string[] { lblMaxCount.Text, "4" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtProgressRate.ClientID %>'), 3, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A011", new string[] { lblProgressRate.Text, "3" }, new string[] { lblProgressRate.Text, "3" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
        if(isMaxLenth(document.getElementById('<%=txtFinalTest.ClientID %>'), 3, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A011", new string[] { lblFinalTest.Text, "3" }, new string[] { lblFinalTest.Text, "3" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

        /* 숫자인지 여부 체크 */
        if(isNumber(document.getElementById('<%=txtEduFee.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblEduFee.Text }, new string[] { lblEduFee.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')==false) return false;
        if(isNumber(document.getElementById('<%=txtScore.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblScore.Text }, new string[] { lblScore.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')==false) return false;
        if(isNumber(document.getElementById('<%=txtSupportFee.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblSupportFee.Text }, new string[] { lblSupportFee.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')==false) return false;
        if(isNumber(document.getElementById('<%=txtSupportCompFee.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblSupportCompFee.Text }, new string[] { lblSupportCompFee.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')==false) return false;
        if(isNumber(document.getElementById('<%=txtMinCount.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblMinCount.Text }, new string[] { lblMinCount.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')==false) return false;
        if(isNumber(document.getElementById('<%=txtMaxCount.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblMaxCount.Text }, new string[] { lblMaxCount.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')==false) return false;
        if(isNumber(document.getElementById('<%=txtProgressRate.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblProgressRate.Text }, new string[] { lblProgressRate.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')==false) return false;
        if(isNumber(document.getElementById('<%=txtFinalTest.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblFinalTest.Text }, new string[] { lblFinalTest.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')==false) return false;

        /* 일자인지 여부 체크 */
        if(compareDate(document.getElementById('<%=txtApplyBeginDt.ClientID %>'), document.getElementById('<%=txtApplyEndDt.ClientID %>'), '<%=System.Threading.Thread.CurrentThread.CurrentCulture %>')==false) return false;
        if(compareDate(document.getElementById('<%=txtBeginDt.ClientID %>'), document.getElementById('<%=txtEndDt.ClientID %>'), '<%=System.Threading.Thread.CurrentThread.CurrentCulture %>')==false) return false;
        if(compareDateNull(document.getElementById('<%=txtResBeginDt.ClientID %>'), document.getElementById('<%=txtResEndDt.ClientID %>'), '<%=System.Threading.Thread.CurrentThread.CurrentCulture %>')==false) return false;

        return true;
    }
    function OK()
    {
        self.close();
        opener.__doPostBack('ctl00$ContentPlaceHolderMain$btnRetrieve', '');
    }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="lblMenuTitle" runat="server" meta:resourcekey="lblMenuTitle" /></h3>
        <!--<p><asp:Label ID="lbl0" runat="server" meta:resourcekey="lblGuide0" Text="" />
           <asp:Label ID="lbl1" runat="server" meta:resourcekey="lblGuide1" Text="" /></p>-->

        <!-- 검색 -->
        <!--div class="message-box default center">
        </div>-->


        <!-- 상단 버튼-->
        <div class="button-box right">
        </div>


        <!-- 내용-->
        <div class="notice-text right"><asp:Label ID="lblRequired" runat="server" Text="필수입력항목" meta:resourcekey="lblRequired" /><span class="required"></span></div>
        <div class="gm-table data-table read-type">
                
            <table>
                <colgroup>
                    <col width="20%">
                    <col width="30%">
                    <col width="20%">
                    <col width="30%">
                </colgroup>
                <tbody>

                <!-- Course Name(Course Code)-->
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblCourseNm" runat="server" meta:resourcekey="lblCourseNm" />
                        <span class="required"></span>
                    </th>
                    <td colspan ="3">
                        <input type="text" id="txtCourseNM" runat="server" readonly="readonly" style="width:50%; background-color:#dcdcdc" />
                        <input type="text" id="txtCourseID" runat="server" readonly="readonly" style="width:15%;  background-color:#dcdcdc" />
                        <a href="#" class="button-board-search" onclick="openPopWindow('/common/course_pop.aspx?opener_textbox_id=<%=txtCourseID.ClientID %>&opener_textbox_nm=<%=txtCourseNM.ClientID %>&MenuCode=<%=Session["MENU_CODE"]%>', 'course_pop_win', '600', '650');"></a>
                    </td>
                </tr>

                <!-- 교육년도, 차수 -->
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblYear" runat="server" meta:resourcekey="lblYear" />
                        <%-- <span class="required"></span>   --%>
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblSeq" runat="server" meta:resourcekey="lblSeq" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtSeq" runat="server" ReadOnly="true" MaxLength="4" />
                    </td>
                </tr>

                <!-- 교육기관, 교육장소  -->
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblInstitution" runat="server" meta:resourcekey="lblInstitution" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlInstitution" runat="server" />
                    </td>
                </tr>

                <!-- 교육기관, 교육장소  -->
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblInOut" runat="server" meta:resourcekey="lblInOut" />
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlInOut" runat="server" AutoPostBack="true" OnSelectedIndexChanged ="ddlInOut_SelectIndexChange" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblPlace" runat="server" meta:resourcekey="lblPlace" />
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlPlace" runat="server"  />
                    </td>
                </tr>


                <!-- 교육비, VAT  -->
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblEduFee" runat="server" meta:resourcekey="lblEduFee" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtEduFee" runat="server" MaxLength="10" AutoPostBack = "true" OnTextChanged="txtEduFee_TextChanged"  />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblVat" runat="server" meta:resourcekey="lblVat" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtVat" runat="server" ReadOnly="True" BackColor="Gainsboro" />
                    </td>
                </tr>

                <!-- 교육구분, 수료점수  -->
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblClassification" runat="server" meta:resourcekey="lblClassification" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:Panel ID="Panel1" runat = "server" Height="70px" ScrollBars="auto"  >
                            <asp:DataList ID="dtlClassification" runat="server" ShowFooter="False" ShowHeader="False" BorderStyle="None">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkClass" runat="server" Style="height: 20px;" Checked='<%# Eval("check") %>' />
                                    <asp:Label ID="lblClassNm" runat="server" Style="height: 20px;padding-top: 7px" Text='<%# Eval("nm") %>' />
                                    <asp:Label ID="lblClassId" runat="server" Style= "visibility:hidden" Text='<%# Eval("id") %>' />
                                </ItemTemplate>
                            </asp:DataList>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblScore" runat="server" meta:resourcekey="lblScore" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:TextBox ID="txtScore" runat="server" MaxLength="3" />
                    </td>
                </tr>

                <!-- 지원금액(우선), 지원금액(대기업)  -->
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblSupportFee" runat="server" meta:resourcekey="lblSupportFee" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtSupportFee" runat="server" MaxLength="10" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblSupportCompFee" runat="server" meta:resourcekey="lblSupportCompFee" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtSupportCompFee" runat="server" MaxLength="10" />
                    </td>
                </tr>
                <!-- 수강신청기간  -->
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblApplyDt" runat="server" meta:resourcekey="lblApplyDt" />
                        <span class="required"></span>
                    </th>
                    <td  colspan ="3">
                        <asp:TextBox ID="txtApplyBeginDt" runat="server" MaxLength="10" CssClass="datepick w180" />
                        <span class="gm-text2">~</span>
                        <asp:TextBox ID="txtApplyEndDt" runat="server" MaxLength="10" CssClass="datepick w180" />
                    </td>
                </tr>

                <!-- 교육기간  -->
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblBeginDt" runat="server" meta:resourcekey="lblBeginDt" />
                        <span class="required"></span>
                    </th>
                    <td colspan ="3">
                        <asp:TextBox ID="txtBeginDt" runat="server"  MaxLength="10" CssClass="datepick w180" />
                        <span class="gm-text2">~</span>
                        <asp:TextBox ID="txtEndDt" runat="server"  MaxLength="10" CssClass="datepick w180" />
                    </td>
                </tr>

                <!-- 설문조사기간  -->
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblResDt" runat="server" meta:resourcekey="lblResDt" />
                    </th>
                    <td colspan ="3">
                        <asp:TextBox ID="txtResBeginDt" runat="server" MaxLength="10" CssClass="datepick w180" />
                        <span class="gm-text2">~</span>
                        <asp:TextBox ID="txtResEndDt" runat="server"  MaxLength="10" CssClass="datepick w180" />
                    </td>
                </tr>

                <!-- 최소인원, 최대인원 -->
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblMinCount" runat="server" meta:resourcekey="lblMinCount" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtMinCount" runat="server" MaxLength="4" />
                    </td>
                    <th scope="row">
                        <asp:Label ID="lblMaxCount" runat="server" meta:resourcekey="lblMaxCount" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:TextBox ID="txtMaxCount" runat="server" MaxLength="4" />
                    </td>
                </tr>

                <!-- 적용회사 -->
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblCompanyAccept" runat="server" meta:resourcekey="lblCompanyAccept" />
                    </th>
                    <td colspan = "3">
                        <asp:Panel ID="Panel2" runat = "server" Height="70px" ScrollBars="auto"  >
                            <asp:DataList ID="dtlCompany" runat="server" ShowFooter="False" ShowHeader="False" BorderStyle="None">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCompany" runat="server" Style="height: 20px;" Checked='<%# Eval("check") %>' />
                                    <asp:Label ID="lblCompanyNm" runat="server" Style="height: 20px;padding-top: 7px" Text='<%# Eval("nm") %>' />
                                    <asp:Label ID="lblCompanyId" runat="server" Style= "visibility:hidden" Text='<%# Eval("id") %>' />
                                </ItemTemplate>
                            </asp:DataList>
                        </asp:Panel>
                    </td>
                </tr>

                <!-- 이수기준 -->
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblTScore" runat="server" meta:resourcekey="lblTScore" />
                        <span class="required"></span>
                    </th>
                    <td colspan="3">
                        <asp:Label ID="lblTotalScore" runat="server" meta:resourcekey="lblTotalScore"   />
                        <asp:TextBox ID="txtTotalScore" runat="server" Width="10%" ReadOnly="True" MaxLength="3" BackColor="Gainsboro" />&nbsp;&nbsp
                        <asp:Label ID="lblProgressRate" runat="server" meta:resourcekey="lblProgressRate"   />
                        <asp:TextBox ID="txtProgressRate" runat="server" Width="10%"  AutoPostBack = "true" OnTextChanged="txtProgressRate_TextChanged" MaxLength="3" />%&nbsp;&nbsp;
                        <asp:Label ID="lblFinalTest" runat="server" meta:resourcekey="lblFinalTest"  />
                        <asp:TextBox ID="txtFinalTest" runat="server" Width="10%" MaxLength="3"  AutoPostBack ="true" OnTextChanged="txtProgressRate_TextChanged" />%
                    </td>
                </tr>

                <!--  설문조사 -->
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblResNm" runat="server" meta:resourcekey="lblResNm" />
                    </th>
                    <td colspan ="3">
                        <input type="text" id="txtResNm" runat="server" readonly="readonly" style="width:50%; background-color:#dcdcdc" />
                        <input type="text" id="txtResId" runat="server" readonly="readonly" style="width:15%;  background-color:#dcdcdc" />
                        <a href="#" class="button-board-search" onclick="openPopWindow('/curr/opencourse_pop_survey.aspx?res_no=<%=txtResId.ClientID %>&res_sub=<%=txtResNm.ClientID %>&Menucode=<%=Session["menu_code"] %>', 'opencourse_pop_survey_win', '1024', '625');"></a>
                    </td>
                </tr>

                <!-- usage -->
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblUsage" runat="server" meta:resourcekey="lblUsage" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:RadioButtonList ID="rdoUsage" runat="server" Width="15%" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                            <asp:ListItem Value="N">No</asp:ListItem>
                        </asp:RadioButtonList></td>
                    <th scope="row">
                        <asp:Label ID="lblCourseGubun" runat="server" meta:resourcekey="lblCourseGubun" />
                        <span class="required"></span>
                    </th>
                    <td>
                        <asp:RadioButtonList ID="rdoCourseGubun" runat="server" Width="15%" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                            <asp:ListItem Value="N">No</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            
                </tbody>
            </table>
        
        </div>


        <!-- 하단 버튼-->
        <div class="button-group center">  
            <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="button-main-rnd lg blue" OnClick="btnSend_Click" OnClientClick="return fnValidation();"  />
            <!--<asp:Button ID="btnRewrite" runat="server" Text="Rewrite" CssClass="btn_del" OnClick="btnRewrite_Click" />-->
        </div>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->

</asp:Content>