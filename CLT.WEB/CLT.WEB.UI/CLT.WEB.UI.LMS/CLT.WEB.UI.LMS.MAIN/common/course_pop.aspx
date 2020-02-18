<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="course_pop.aspx.cs" Inherits="CLT.WEB.UI.LMS.COMMON.course_pop" 
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" language="javascript">

    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container">
        <h3><asp:Label ID="lblMenuTitle" runat="server" meta:resourcekey="lblMenuTitle" /></h3>
    
    <section class="section-board">

        <!-- 검색 -->
        <div class="gm-table plane-table">
            <table>
                <colgroup>
                    <col width="15%">
                    <col width="35%">
                    <col width="15%">
                    <col width="35%">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="col"><asp:Label ID="lblCourseNM" runat="server" meta:resourcekey="lblCourseNM" /></th>
                    <td><asp:TextBox runat="server" ID="txtCourseNM" MaxLength="50" /></td>
                    <th scope="col"><asp:Label ID="lblCourseType" runat="server" meta:resourcekey="lblCourseType" /></th>
                    <td><asp:DropDownList ID="ddlCourseType" runat="server" /></td>
                </tr>
                <tr>
                    <th scope="col"><asp:Label ID="lblCourseLang" runat="server" meta:resourcekey="lblCourseLang" /></th>
                    <td><asp:DropDownList ID="ddlCourseLang" runat="server" /></td>
                    <td colspan="2"><asp:Button ID="btnRetrieve" runat="server" Text="Retrieve" CssClass="button-default blue" OnClick="btnRetrieve_Click" meta:resourcekey="btnRetrieveResource" /></td>
                </tr>
                </tbody>
            </table>
        </div>

        <CLTWebControl:PageInfo ID="PageInfo1" runat="server" Visible="false" />
        <div class="gm-table data-table read-type">
            <C1WebGrid:C1WebGrid ID="C1WebGrid1" runat="server" AllowSorting="True" AllowColSizing="True" AutoGenerateColumns="False"
                    DataKeyField="course_id" OnItemCreated="WebGrid1_ItemCreated">
                <Columns>
                    <C1WebGrid:C1TemplateColumn>
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                        </ItemTemplate>
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                    </C1WebGrid:C1TemplateColumn>
                                
                    <C1WebGrid:C1BoundColumn DataField="course_type" >
                        <ItemStyle Width="15%" HorizontalAlign="Center"/>
                    </C1WebGrid:C1BoundColumn>
                                
                    <C1WebGrid:C1TemplateColumn >
                        <ItemTemplate>
                            <a href="#" onclick="setTextValOfOpener1(self, opener, '<%=ViewState["opener_textbox_id"].ToString() %>', '<%# DataBinder.Eval(Container.DataItem, "course_id")%>', '<%=ViewState["opener_textbox_nm"].ToString() %>', '<%# DataBinder.Eval(Container.DataItem, "course_nm")%>');">
                            <%# DataBinder.Eval(Container.DataItem, "course_nm")%></a>
                        </ItemTemplate>
                        <ItemStyle Width="50%" HorizontalAlign="Center" />
                    </C1WebGrid:C1TemplateColumn>
                                
                    <C1WebGrid:C1BoundColumn DataField="course_lang">
                        <ItemStyle Width="15%" HorizontalAlign="Center"/>                                    
                    </C1WebGrid:C1BoundColumn>
                                
                    <C1WebGrid:C1BoundColumn DataField="ins_dt" DataFormatString="{0:yyyy.MM.dd}">
                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                    </C1WebGrid:C1BoundColumn>
                </Columns>
                <HeaderStyle Font-Bold="true" />
                <ItemStyle  Wrap="true"  />
                <AlternatingItemStyle />
            </C1WebGrid:C1WebGrid>
        </div>
        
        <div class="gm-paging">
            <CLTWebControl:PageNavigator ID="PageNavigator1" runat="server" OnOnPageIndexChanged="PageNavigator1_OnPageIndexChanged" />
        </div>

        <!-- 버튼
        클래스명 button-group : 사방 20px 여백 포함됨.
        -->
        <!--div class="button-group center">
            <a href="#none" class="button-default blue">버튼1</a>
            <button type="button" class="button-default">버튼2</button>
            <input type="button" class="button-default" value="버튼3">
        </div-->

    </section>
        
    </div>
    <!--// 팝업 컨텐츠 끝 -->

</asp:Content>
