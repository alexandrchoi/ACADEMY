<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="course_edit.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.course_edit"  Culture="auto" UICulture="auto" MasterPageFile="/MasterSub.Master" %>

<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.Command.2" Namespace="C1.Web.Command" TagPrefix="c1c" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">

</script>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

    <!-- Subject 정보 선택 했을때 가져온 hidden 필드 및 link 필드 -->
    <asp:HiddenField runat ="server" ID="HidSubjectAdd" />
    <asp:LinkButton ID="LnkBtnSubjectAdd" runat="server" OnClick="LnkBtnSubjectAdd_Click" />

    <asp:HiddenField runat ="server" ID="HidAssessAdd" />
    <asp:LinkButton ID="LnkBtnAssessAdd" runat="server" OnClick="LnkBtnAssessAdd_Click" />

    <asp:HiddenField runat ="server" ID="HidTextbookAdd" />
    <asp:LinkButton ID="LnkBtnTextbookAdd" runat="server" OnClick="LnkBtnTextbookAdd_Click" />



<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="" meta:resourcekey="lblMenuTitle" />

        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>
    <!--p>설명</p-->
    <!--<asp:Label ID="lblMenuDscr" runat="server" meta:resourcekey="lblMenuDscr" />-->

    <section class="section-board">
        
        <!---------------------------------------------------------------------------------------------------------------------------------->
        <!-- TAB -->
        <div class="sub-tab">
            <asp:LinkButton id="tabCourse" runat="server" CssClass="" OnClick="tabctrl_Click" Text="Course" data-tab="pnlCourse" />
            <asp:LinkButton id="tabSubject" runat="server" CssClass="" OnClick="tabctrl_Click" Text="Subject" data-tab="pnlSubject" />
            <asp:LinkButton id="tabAssess" runat="server" CssClass="" OnClick="tabctrl_Click" Text="Assess" data-tab="pnlAssess" />
            <asp:LinkButton id="tabTextBook" runat="server" CssClass="" OnClick="tabctrl_Click" Text="TextBook" data-tab="pnlTextBook" />
        </div>
        <!---------------------------------------------------------------------------------------------------------------------------------->


        <!---------------------------------------------------------------------------------------------------------------------------------->
        <asp:Panel ID="pnlCourse" runat="server" CssClass="tab-content current">
            
            <!-- 버튼 -->
            <div class="button-box right">
                <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="button-default blue" OnClick="btnSend_Click" OnClientClick="return fnValidation();" />
                <asp:Button ID="btnTemp" runat="server" Text="Temp Save" CssClass="button-default" OnClick="btnTemp_Click" OnClientClick="return fnValidation();" />
                <!--<asp:Button ID="btnRewrite" runat="server" Text="Rewrite" CssClass="button-default" OnClick="btnRewrite_Click" />-->
            </div>
            
            <!--<div class="notice-text right"><asp:Label ID="lblRequired" runat="server" Text="필수입력항목" meta:resourcekey="lblRequired" /><span class="required"></span></div>-->
            <div class="board-list gm-table data-table read-type">
                <table>
                    <colgroup>
                        <col width="15%">
                        <col width="35%">
                        <col width="15%">
                        <col width="35%">
                    </colgroup>
                    <tbody>
                    <tr>
                        <th scope="row">
                            <!-- 언어, 고용보험대상여부 -->
                            <asp:Label ID="lblLang" runat="server" meta:resourcekey="lblLang" />
                            <span class="required"></span>
                        </th>
                        <td>
                            <asp:DropDownList ID="ddlLang" runat="server" />
                        </td>
                        <th scope="row">
                            <asp:Label ID="lblInsu" runat="server" meta:resourcekey="lblInsu" />
                        </th>
                        <td>
                            <asp:RadioButtonList ID="rdoInsu" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" Width="100%">
                                <asp:ListItem Value="Y"> Yes &nbsp;&nbsp; </asp:ListItem>
                                <asp:ListItem Value="N"> No &nbsp;&nbsp; </asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">
                            <!-- 과정명(한글), 과정명(영어) -->
                            <asp:Label ID="lblCourseNm" runat="server" meta:resourcekey="lblCourseNm" />
                            <span class="required"></span>
                        </th>
                        <td>
                            <asp:TextBox ID="txtCourseNm" runat="server" MaxLength="200" />
                        </td>
                        <th scope="row">
                            <asp:Label ID="lblCourseNmE" runat="server" meta:resourcekey="lblCourseNmE" />
                        </th>
                        <td>
                            <asp:TextBox ID="txtCourseNmE" runat="server" MaxLength="50" />
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">
                            <!-- 과정그룹, 과정유형 -->
                            <asp:Label ID="lblGroup" runat="server" meta:resourcekey="lblGroup" />
                            <span class="required"></span>
                        </th>
                        <td>
                            <asp:DropDownList ID="ddlGroup" runat="server" />
                        </td>
                        <th scope="row">
                            <asp:Label ID="lblType" runat="server" meta:resourcekey="lblType" />
                            <span class="required"></span>
                        </th>
                        <td>
                            <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged ="ddlType_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">
                            <!-- 과정분야, 과정분류-->
                            <asp:Label ID="lblField" runat="server" meta:resourcekey="lblField" />
                            <span class="required"></span>
                        </th>
                        <td>
                            <asp:DropDownList ID="ddlField" runat="server" />
                        </td>
                        <th scope="row">
                            <asp:Label ID="lblKind" runat="server" meta:resourcekey="lblKind" />
                            <span class="required"></span>
                        </th>
                        <td>
                            <asp:DropDownList ID="ddlKind" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">
                            <!-- 시험유형, 직급-->
                            <asp:Label ID="lblExam" runat="server" meta:resourcekey="lblExam" />
                        </th>
                        <td>
                            <asp:RadioButtonList ID="rdoExam" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" Width="100%">
                                <asp:ListItem Value="000001"> None &nbsp;&nbsp; </asp:ListItem>
                                <asp:ListItem Value="000002"> Random &nbsp;&nbsp; </asp:ListItem>
                                <asp:ListItem Value="000003" Enabled="false"> Sheet &nbsp;&nbsp; </asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <th scope="row">
                            <asp:Label ID="lblGrade" runat="server" meta:resourcekey="lblGrade" />
                        </th>
                        <td>
                            <asp:Panel runat="server" Height="70px" ScrollBars="Vertical"  >
                                <asp:DataList ID="dtlStep" runat="server"  ShowFooter="False" ShowHeader="False"
                                    ItemStyle-BorderStyle="None" ItemStyle-BorderWidth="0px" ItemStyle-HorizontalAlign="Center"
                                    HorizontalAlign="Center" BorderStyle="None" CellPadding="0" CellSpacing="0" BorderWidth="0" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkStep" runat="server" Checked='<%# Eval("stepchk") %>' />
                                        <asp:Label ID="lblStepNm" runat="server" Text='<%# Eval("stepnm") %>' />
                                        <asp:Label ID="lblStepid" runat="server" Visible="false" Text='<%# Eval("stepid") %>' />
                                    </ItemTemplate>
                                </asp:DataList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">
                            <!-- 선종, 직책-->
                            <asp:Label ID="lblVslType" runat="server" meta:resourcekey="lblVslType" />
                        </th>
                        <td>
                            <asp:DataList ID="dtlVslType" runat="server" ShowFooter="False" ShowHeader="False"
                                ItemStyle-BorderStyle="None" ItemStyle-BorderWidth="0px" ItemStyle-HorizontalAlign="Center"
                                HorizontalAlign="Center" BorderStyle="None" CellPadding="0" CellSpacing="0" BorderWidth="0" >
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkVslType" runat="server" Checked='<%# Eval("vsltypechk") %>' />
                                    <asp:Label ID="lblVslTypeNm" runat="server" Text='<%# Eval("vsltypenm") %>' />
                                    <asp:Label ID="lblVslTypeId" runat="server" Visible="false" Text='<%# Eval("vsltypeid") %>' />
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                        <th scope="row">
                            <asp:Label ID="lblRank" runat="server" meta:resourcekey="lblRank" />
                        </th>
                        <td>
                            <asp:Panel runat="server" Height="70px" ScrollBars="Vertical"  >
                                <asp:DataList ID="dtlRank" runat="server"  ShowFooter="False" ShowHeader="False"
                                    ItemStyle-BorderStyle="None" ItemStyle-BorderWidth="0px" ItemStyle-HorizontalAlign="Center"
                                    HorizontalAlign="Center" BorderStyle="None" CellPadding="0" CellSpacing="0" BorderWidth="0" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRank" runat="server" Checked='<%# Eval("rankchk") %>' />
                                        <asp:Label ID="lblRankNm" runat="server" Text='<%# Eval("ranknm") %>' />
                                        <asp:Label ID="lblRankid" runat="server" Visible="false" Text='<%# Eval("rankid") %>' />
                                    </ItemTemplate>
                                </asp:DataList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">
                            <!-- 과정소개 -->
                            <asp:Label ID="lblIntro" runat="server" meta:resourcekey="lblIntro" />
                            <span class="required"></span>
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="txtIntro" runat="server" Height="70px" TextMode="MultiLine" MaxLength="300" />
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">
                            <!-- 학습목표 -->
                            <asp:Label ID="lblGoal" runat="server" meta:resourcekey="lblGoal" />
                            <span class="required"></span>
                        </th>
                        <td colspan="3">
                            <asp:TextBox ID="txtGoal" runat="server" Height="70px" TextMode="MultiLine" MaxLength="300" />
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">
                            <!-- 정원, 유효기간 -->
                            <asp:Label ID="lblCapa" runat="server" meta:resourcekey="lblCapa" />
                            <span class="required"></span>
                        </th>
                        <td>
                            <asp:TextBox ID="txtCapa" runat="server" MaxLength="5" CssClass="w180" />
                            &nbsp;Person(s)
                        </td>
                        <th scope="row">
                            <asp:Label ID="lblExpired" runat="server" meta:resourcekey="lblExpired" />
                        </th>
                        <td>
                            <asp:TextBox ID="txtExpired" runat="server" MaxLength="5" CssClass="w180" />
                            &nbsp;Year(s)
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">
                            <!-- 교육일수, 교육시간-->
                            <asp:Label ID="lblDays" runat="server" meta:resourcekey="lblDays" />
                        </th>
                        <td>
                            <asp:TextBox ID="txtDays" runat="server" MaxLength="5" CssClass="w180" />
                            &nbsp;Day(s)
                        </td>
                        <th scope="row">
                            <asp:Label ID="lblHours" runat="server" meta:resourcekey="lblHours" />
                        </th>
                        <td>
                            <asp:TextBox ID="txtHours" runat="server" MaxLength="5" CssClass="w180" />
                            &nbsp;Hour(s)
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">
                            <!-- 사용여부 -->
                            <asp:Label ID="lblUsage" runat="server" meta:resourcekey="lblUsage" />
                            <span class="required"></span>
                        </th>
                        <td>
                            <asp:RadioButtonList ID="rdoUsage" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" Width="100%">
                                <asp:ListItem Value="Y"> Yes &nbsp; &nbsp; </asp:ListItem>
                                <asp:ListItem Value="N"> No &nbsp; &nbsp; </asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <th scope="row">
                            <asp:Label ID="lblManager" runat="server" meta:resourcekey="lblManager" />
                        </th>
                        <td>
                            <asp:TextBox ID="txtManager" runat="server" MaxLength="50" />
                        </td>
                    </tr>
                    </tbody>
                </table>
        
            </div>

        </asp:Panel>
        <!---------------------------------------------------------------------------------------------------------------------------------->


        <!---------------------------------------------------------------------------------------------------------------------------------->
        <asp:Panel ID="pnlSubject" runat="server" CssClass="tab-content">
            
            <!-- 버튼 -->
            <div class="button-box right">
                <asp:Button ID="btnTemp2" runat="server" Text="Temp Save" CssClass="button-default blue" OnClick="btnTemp2_Click" />
                <input type="button" value="Add" class="button-default blue" onclick="javascript:openPopWindow('/curr/course_subject_add.aspx','course_subject_add', '1200', '870');" />
                <asp:Button ID="btnDelete2" runat="server" Text="Delete" CssClass="button-default" OnClick="btnDelete2_Click" />
                <asp:Button ID="btnSort" runat="server" Text="Sort" CssClass="button-default" OnClick="btnSort_Click" />
                <input type="button" value="New" class="button-default" onclick="javascript:openPopWindow('/curr/course_subject_new.aspx?MenuCode=<%=Session["MENU_CODE"]%>','course_subject_new', '800', '650');" />
                <input type="button" id ="btnSubject2" value="Subject" class="button-default" onclick="javascript:openPopWindow('/curr/subject_edit.aspx?subject_id=NEW&temp_flg=Y&MenuCode=<%=Session["MENU_CODE"]%>','subject_edit', '800', '750');" />
                <input type="button" value="Contents" class="button-default" onclick="javascript:openPopWindow('/curr/content_edit.aspx?contents_id=NEW&temp_flg=Y&MenuCode=<%=Session["MENU_CODE"]%>','content_edit', '700', '420');" />
            </div>

            <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
            <CLTWebControl:PageInfo ID="PageInfo2" runat="server" />
            <div class="gm-table data-table list-type">
                <C1WebGrid:C1WebGrid ID="grd2" runat="server" CssClass="grid_main" AutoGenerateColumns="false" OnItemCreated="grd2_ItemCreated" >
                    <Columns>

                        <C1WebGrid:C1TemplateColumn >
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 + 15 * (this.CurrentPageIndex - 1)%>
                            </ItemTemplate>
                            <ItemStyle Width="5%" />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1BoundColumn DataField="subject_lang">
                            <ItemStyle Width="8%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="subject_kind" >
                            <ItemStyle Width="8%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1TemplateColumn>
                            <ItemTemplate>
                                <a href="#" onclick="javascript:openPopWindow('/curr/subject_edit.aspx?subject_id=<%# DataBinder.Eval(Container.DataItem, "SUBJECT_ID")%>&TEMP_FLG=<%# DataBinder.Eval(Container.DataItem, "temp_save_flg")%>','subject_edit_win', '800', '750');"><%# DataBinder.Eval(Container.DataItem, "subject_nm")%></a>
                            </ItemTemplate>
                            <ItemStyle Width="35%" CssClass="left" />
                        </C1WebGrid:C1TemplateColumn>

                        <C1WebGrid:C1BoundColumn DataField="learning_time">
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="instructor" >
                            <ItemStyle Width="12%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="use_flg" >
                            <ItemStyle Width="8%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="ins_dt" DataFormatString="{0:yyyy.MM.dd}">
                            <ItemStyle Width="8%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="subject_id" Visible="false">
                            <ItemStyle Width="0%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="subject_nm" Visible="false">
                            <ItemStyle Width="0%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1TemplateColumn>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"/>
                            </ItemTemplate>
                            <ItemStyle Width="5%" />
                        </C1WebGrid:C1TemplateColumn>

                        <C1WebGrid:C1BoundColumn DataField="temp_save_flg" Visible="false">
                            <ItemStyle Width="0%" />
                        </C1WebGrid:C1BoundColumn>
                        
                    </Columns>
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle Wrap="true" />
                    <AlternatingItemStyle />
                </C1WebGrid:C1WebGrid>
            </div>

            <div class="gm-paging">
                <CLTWebControl:PageNavigator ID="PageNavigator2" runat="server" OnOnPageIndexChanged="PageNavigator2_OnPageIndexChanged" />
            </div>
            <!-- 개발자 수정 영역 3 - Grid 추가 영역 End-->


            <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
            <CLTWebControl:PageInfo ID="PageInfo2c" runat="server" />
            <div class="gm-table data-table list-type">
                <C1WebGrid:C1WebGrid ID="grd2c" runat="server" CssClass="grid_main" AutoGenerateColumns="false" OnItemCreated="grd2c_ItemCreated">
                    <Columns>

                        <C1WebGrid:C1TemplateColumn>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                            </ItemTemplate>
                            <ItemStyle Width="5%" />
                        </C1WebGrid:C1TemplateColumn>

                        <C1WebGrid:C1BoundColumn DataField="subject_kind" >
                            <ItemStyle Width="8%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="subject_nm" >
                            <ItemStyle Width="18%" CssClass="left" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1TemplateColumn>
                            <ItemTemplate>
                                <a href="#" onclick="openPopWindow('/curr/content_edit.aspx?contents_id=<%# DataBinder.Eval(Container.DataItem, "CONTENTS_ID")%>&TEMP_FLG=<%# DataBinder.Eval(Container.DataItem, "temp_save_flg")%>','content_edit_win', '700', '420');">
                                    <%# DataBinder.Eval(Container.DataItem, "contents_nm")%>
                                </a>
                            </ItemTemplate>
                            <ItemStyle Width="18%" CssClass="left" />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1BoundColumn DataField="contents_file_nm">
                            <ItemStyle Width="8%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="contents_type">
                            <ItemStyle Width="7%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="contents_lang">
                            <ItemStyle Width="7%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="contents_remark">
                            <ItemStyle Width="20%" CssClass="left" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="ins_dt" DataFormatString="{0:yyyy.MM.dd}">
                            <ItemStyle Width="9%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="CONTENTS_ID" Visible="false">
                            <ItemStyle Width="0%"  CssClass="left" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="temp_save_flg" Visible="false">
                            <ItemStyle Width="0%" />
                        </C1WebGrid:C1BoundColumn>
                        
                    </Columns>
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle Wrap="true" />
                    <AlternatingItemStyle />
                </C1WebGrid:C1WebGrid>
            </div>

            <div class="gm-paging">
                <CLTWebControl:PageNavigator ID="PageNavigator2c" runat="server" OnOnPageIndexChanged="PageNavigator2c_OnPageIndexChanged" />
            </div>
            <!-- 개발자 수정 영역 3 - Grid 추가 영역 End-->

        </asp:Panel>
        <!---------------------------------------------------------------------------------------------------------------------------------->
        

        <!---------------------------------------------------------------------------------------------------------------------------------->
        <asp:Panel ID="pnlAssess" runat="server" CssClass="tab-content">
            
            <!-- 버튼 -->
            <div class="button-box right">
                <asp:Button ID="btnTemp3" runat="server" Text="Temp Save" CssClass="button-default" OnClick="btnTemp3_Click" />
                <input type="button" value="Add" class="button-default blue" onclick="javascript:openPopWindow('/curr/course_pop_assess.aspx','course_pop_assess', '1280', '790');" />

                <asp:Button ID="btnDelete3" runat="server" Text="Delete" CssClass="button-default" OnClick="btnDelete3_Click" />
                <input type="button" value="Assess" class="button-default" onclick="javascript:openPopWindow('/curr/assess_edit.aspx?temp_flg=Y&MenuCode=<%=Session["MENU_CODE"]%>','assess_edit', '800', '840');" />
            </div>

            <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
            <CLTWebControl:PageInfo ID="PageInfo3" runat="server" />
            <div class="gm-table data-table list-type">
                <C1WebGrid:C1WebGrid ID="grd3" runat="server" CssClass="grid_main" AutoGenerateColumns="false" OnItemCreated="grd3_ItemCreated">
                    <Columns>

                        <C1WebGrid:C1TemplateColumn >
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                            </ItemTemplate>
                            <ItemStyle Width="5%" />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1BoundColumn DataField="question_kind">
                            <ItemStyle Width="8%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="course_group" >
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="course_field" >
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1TemplateColumn>
                            <ItemTemplate>
                                <a href="#" onclick="javascript:openPopWindow('/curr/assess_edit.aspx?question_id=<%# DataBinder.Eval(Container.DataItem, "QUESTION_ID")%>&TEMP_FLG=<%# DataBinder.Eval(Container.DataItem, "temp_save_flg")%>','assess_edit_win', '800', '840');"><%# DataBinder.Eval(Container.DataItem, "question_content")%></a>
                            </ItemTemplate>
                            <ItemStyle Width="32%" CssClass="left" />
                        </C1WebGrid:C1TemplateColumn>

                        <C1WebGrid:C1BoundColumn DataField="question_type" >
                            <ItemStyle Width="15%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="ins_dt" DataFormatString="{0:yyyy.MM.dd}">
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="question_id" Visible="false">
                            <ItemStyle Width="0%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="question_content" Visible="false">
                            <ItemStyle Width="0%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1TemplateColumn>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"/>
                            </ItemTemplate>
                            <ItemStyle Width="5%" />
                        </C1WebGrid:C1TemplateColumn>

                        <C1WebGrid:C1BoundColumn DataField="temp_save_flg" Visible="false">
                            <ItemStyle Width="0%" />
                        </C1WebGrid:C1BoundColumn>
                        
                    </Columns>
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle Wrap="true" />
                    <AlternatingItemStyle />
                </C1WebGrid:C1WebGrid>
            </div>

            <div class="gm-paging">
                <CLTWebControl:PageNavigator ID="PageNavigator3" runat="server" OnOnPageIndexChanged="PageNavigator3_OnPageIndexChanged" />
            </div>
            <!-- 개발자 수정 영역 3 - Grid 추가 영역 End-->

        </asp:Panel>
        <!---------------------------------------------------------------------------------------------------------------------------------->
        

        <!---------------------------------------------------------------------------------------------------------------------------------->
        <asp:Panel ID="pnlTextBook" runat="server" CssClass="tab-content">
            
            <!-- 버튼 -->
            <div class="button-box right">
                <asp:Button ID="btnTemp4" runat="server" Text="Temp Save" CssClass="button-default" OnClick="btnTemp4_Click" />
                <input type="button" value="Add" class="button-default blue" onclick="javascript:openPopWindow('/curr/course_pop_textbook.aspx','course_pop_textbook', '1024', '790');" />
                <asp:Button ID="btnDelete4" runat="server" Text="Delete" CssClass="button-default" OnClick="btnDelete4_Click" />
                <input type="button" value="Textbook" class="button-default" onclick="javascript:openPopWindow('/curr/textbook_edit.aspx?temp_flg=Y&MenuCode=<%=Session["MENU_CODE"]%>','textbook_edit', '940', '500');" />
            </div>

            <!-- 개발자 수정 영역 3 - Grid 추가 영역 Start -->
            <CLTWebControl:PageInfo ID="PageInfo4" runat="server"/>
            <div class="gm-table data-table list-type">
                <C1WebGrid:C1WebGrid ID="grd4" runat="server" CssClass="grid_main" AutoGenerateColumns="false" DataKeyField="TEXTBOOK_ID" OnItemCreated="grd4_ItemCreated">
                    <Columns>

                        <C1WebGrid:C1TemplateColumn>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                            </ItemTemplate>
                            <ItemStyle Width="5%" />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1BoundColumn DataField="textbook_type">
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1TemplateColumn>
                            <ItemTemplate>
                                <a href="#" onclick="openPopWindow('/curr/textbook_edit.aspx?textbook_id=<%# DataBinder.Eval(Container.DataItem, "TEXTBOOK_ID")%>&TEMP_FLG=<%# DataBinder.Eval(Container.DataItem, "temp_save_flg")%>','textbook_edit_win', '940', '500');">
                                    <%# DataBinder.Eval(Container.DataItem, "textbook_nm")%>
                                </a>
                            </ItemTemplate>
                            <ItemStyle />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1BoundColumn DataField="author">
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="publisher">
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="textbook_lang">
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="user_nm_kor">
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="ins_dt" DataFormatString="{0:yyyy.MM.dd}">
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="use_flg">
                            <ItemStyle Width="8%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="textbook_id" Visible="false">
                            <ItemStyle Width="0%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="textbook_nm" Visible="false">
                            <ItemStyle Width="0%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1TemplateColumn>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDel" runat="server"/>
                            </ItemTemplate>
                            <ItemStyle Width="7%" />
                        </C1WebGrid:C1TemplateColumn>

                        <C1WebGrid:C1BoundColumn DataField="temp_save_flg" Visible="false">
                            <ItemStyle Width="0%" />
                        </C1WebGrid:C1BoundColumn>
                    
                    </Columns>
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle Wrap="true" />
                    <AlternatingItemStyle />
                </C1WebGrid:C1WebGrid>
            </div>

            <div class="gm-paging">
                <CLTWebControl:PageNavigator ID="PageNavigator4" runat="server" OnOnPageIndexChanged="PageNavigator4_OnPageIndexChanged" />
            </div>
            <!-- 개발자 수정 영역 3 - Grid 추가 영역 End -->

        </asp:Panel>
        <!---------------------------------------------------------------------------------------------------------------------------------->

    </section>


    <script language="javascript" type="text/javascript">
        function setSubjectAdd(datas)
        {
            hidSubjectAdd = document.getElementById("<%=HidSubjectAdd.ClientID %>");
            hidSubjectAdd.value = datas[0];
            <%= Page.GetPostBackEventReference(LnkBtnSubjectAdd) %>;
        }
        function setAssessAdd(datas)
        {
            //debugger;
            hidAssessAdd = document.getElementById("<%=HidAssessAdd.ClientID %>");
            hidAssessAdd.value = datas[0];
            <%= Page.GetPostBackEventReference(LnkBtnAssessAdd) %>;
        }
        function setTextbookAdd(datas)
        {
            hidTextbookAdd = document.getElementById("<%=HidTextbookAdd.ClientID %>");
            hidTextbookAdd.value = datas[0];
            <%= Page.GetPostBackEventReference(LnkBtnTextbookAdd) %>;
        }
        function fnValidation()
        {
            //null check
            if(isEmpty(document.getElementById('<%=ddlLang.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblLang.Text }, new string[] { lblLang.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtCourseNm.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCourseNm.Text }, new string[] { lblCourseNm.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=ddlGroup.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblGroup.Text }, new string[] { lblGroup.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=ddlType.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblType.Text }, new string[] { lblType.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=ddlField.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblField.Text }, new string[] { lblField.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=ddlKind.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A003", new string[] { lblKind.Text }, new string[] { lblKind.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtIntro.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblIntro.Text }, new string[] { lblIntro.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtGoal.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblGoal.Text }, new string[] { lblGoal.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isEmpty(document.getElementById('<%=txtCapa.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A004", new string[] { lblCapa.Text }, new string[] { lblCapa.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

            //size check
            if(isMaxLenth(document.getElementById('<%=txtCourseNm.ClientID %>'), 300, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblCourseNm.Text, "100", "200" }, new string[] { lblCourseNm.Text, "100", "200" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isMaxLenth(document.getElementById('<%=txtCourseNmE.ClientID %>'), 80, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblCourseNmE.Text, "25", "50" }, new string[] { lblCourseNmE.Text, "25", "50" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isMaxLenth(document.getElementById('<%=txtIntro.ClientID %>'), 500, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblIntro.Text, "150", "300" }, new string[] { lblIntro.Text, "150", "300" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isMaxLenth(document.getElementById('<%=txtGoal.ClientID %>'), 500, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A005", new string[] { lblGoal.Text, "150", "300" }, new string[] { lblGoal.Text, "150", "300" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

            if(isMaxLenth(document.getElementById('<%=txtCapa.ClientID %>'), 5, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A011", new string[] { lblCapa.Text, "5" }, new string[] { lblCapa.Text, "5" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isMaxLenth(document.getElementById('<%=txtExpired.ClientID %>'), 5, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A011", new string[] { lblExpired.Text, "5" }, new string[] { lblExpired.Text, "5" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isMaxLenth(document.getElementById('<%=txtDays.ClientID %>'), 5, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A011", new string[] { lblDays.Text, "5" }, new string[] { lblDays.Text, "5" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;
            if(isMaxLenth(document.getElementById('<%=txtHours.ClientID %>'), 5, '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A011", new string[] { lblHours.Text, "5" }, new string[] { lblHours.Text, "5" }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')) return false;

            // 숫자여부체크
            if(isNumber(document.getElementById('<%=txtCapa.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblCapa.Text }, new string[] { lblCapa.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')==false) return false;
            if(isNumber(document.getElementById('<%=txtExpired.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblExpired.Text }, new string[] { lblExpired.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')==false) return false;
            if(isNumber(document.getElementById('<%=txtDays.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblDays.Text }, new string[] { lblDays.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')==false) return false;
            if(isNumber(document.getElementById('<%=txtHours.ClientID %>'), '<%=CLT.WEB.UI.COMMON.BASE.MsgInfo.GetMsg("A010", new string[] { lblHours.Text }, new string[] { lblHours.Text }, System.Threading.Thread.CurrentThread.CurrentCulture) %>')==false) return false;
        }
   </script>

</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>



