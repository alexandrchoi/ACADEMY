<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="course_pop_assess.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.course_pop_assess" 
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <script type="text/javascript" language="javascript">
            function OK()
            {
                var datas = Array();

                datas[0] = "<%= assessItems %>"; // 형태( 각각의 Row 콤마(凸) 로 구분)

                opener.setAssessAdd(datas);
                opener.focus();
                self.close();
            }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="" meta:resourcekey="lblMenuTitle" /></h3>
        <!--<p></p>-->

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <asp:Label ID="lblClassification" CssClass="title" runat="server" meta:resourcekey="lblClassification" />
                <asp:DropDownList ID="ddlClassification" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblLang" CssClass="title" runat="server" meta:resourcekey="lblLang" />
                <asp:DropDownList ID="ddlLang" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblExamType" CssClass="title" runat="server" meta:resourcekey="lblExamType" />
                <asp:DropDownList ID="ddlExamType" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblGroup" CssClass="title" runat="server" meta:resourcekey="lblGroup" />
                <asp:DropDownList ID="ddlGroup" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblField" CssClass="title" runat="server" meta:resourcekey="lblField" />
                <asp:DropDownList ID="ddlField" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblQuestion" CssClass="title" runat="server" meta:resourcekey="lblQuestion" />
                <asp:TextBox ID="txtQuestion" runat="server" />
            </div>
            <div class="search-group">
                <asp:Label ID="lblCourse" CssClass="title" runat="server" meta:resourcekey="lblCourse" />
                <input type="text" id="txtCourseNM" runat="server" readonly="readonly" />
                <input type="text" id="txtCourseID" runat="server" readonly="readonly"/>
                <a href="#" class="button-board-search" onclick="openPopWindow('/common/course_pop.aspx?opener_textbox_id=<%=txtCourseID.ClientID %>&opener_textbox_nm=<%=txtCourseNM.ClientID %>&MenuCode=<%=Session["MENU_CODE"]%>', 'vp_l_course_pop_win', '1024', '790');"></a>
            </div>
            <div class="search-group">
                <asp:Label ID="lblSubject" CssClass="title" runat="server" meta:resourcekey="lblSubject" />
                <input type="text" id="txtSubjectNm" runat="server" readonly="readonly" />
                <input type="text" id="txtSubjectId" runat="server" readonly="readonly"  />
                <a href="#" class="button-board-search" onclick="openPopWindow('/curr/course_pop_subject.aspx?subject_id=<%=txtSubjectId.ClientID %>&subject_nm=<%=txtSubjectNm.ClientID %>&MenuCode=<%=Session["MENU_CODE"]%>', 'course_pop_subject', '1024', '790');"></a>
            </div>
            <asp:Button ID="btnRetrieve" runat="server" Text="Search" CssClass="button-default blue" OnClick="btnRetrieve_Click" />
        </div>
        <div class="button-box right">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button-default blue" OnClick="btnAdd_Click" />
        </div>


        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
        <div class="gm-table data-table list-type mt10">
            <C1WebGrid:C1WebGrid ID="grd" runat="server" AllowSorting="True"  AllowColSizing="True" CssClass="grid_main" AutoGenerateColumns="false" OnItemCreated="grd_ItemCreated">
                <Columns>

                    <C1WebGrid:C1TemplateColumn >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1TemplateColumn>
                    <C1WebGrid:C1BoundColumn DataField="question_kind">
                        <ItemStyle Width="7%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="course_group" >
                        <ItemStyle Width="12%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="course_field" >
                        <ItemStyle Width="16%" />
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="question_content" >
                        <ItemStyle Width="30%" CssClass ="left" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="question_type" >
                        <ItemStyle Width="8%" />
                    </C1WebGrid:C1BoundColumn>
                    <C1WebGrid:C1BoundColumn DataField="ins_dt" DataFormatString="{0:yyyy.MM.dd}">
                        <ItemStyle Width="10%" />
                    </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1BoundColumn DataField="temp_save_flg" >
                            <ItemStyle Width="7%" />
                        </C1WebGrid:C1BoundColumn>

                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                            <asp:CheckBox ID="chk" runat = "server"/>
                        </ItemTemplate>
                        <ItemStyle Width="5%" />
                    </C1WebGrid:C1TemplateColumn>

                    <C1WebGrid:C1BoundColumn DataField="question_id" Visible ="false" >
                        <ItemStyle Width="0%" />
                    </C1WebGrid:C1BoundColumn>

                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>
        </div>

        <div class="gm-paging">
            <CLTWebControl:PageNavigator ID="PageNavigator1" runat="server" OnOnPageIndexChanged="PageNavigator1_OnPageIndexChanged" />
        </div>

    </div>
    <!--// 팝업 컨텐츠 끝 -->


</asp:Content>
