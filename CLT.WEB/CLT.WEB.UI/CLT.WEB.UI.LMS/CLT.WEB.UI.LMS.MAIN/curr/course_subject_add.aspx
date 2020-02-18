<%@ Page Language="C#" MasterPageFile="/MasterWin.Master" AutoEventWireup="true" CodeBehind="course_subject_add.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.course_subject_add" 
    Culture="auto" UICulture="auto" %>

<%@ Register Assembly="C1.Web.C1WebGrid.2" Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>
<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <script type="text/javascript" language="javascript">
        function setLoad()
        {
            hidSubject = document.getElementById("<%= HidSubject.ClientID %>");
        }
        function setSelectSubject(subject_id)
        {
            hidSubject = document.getElementById("<%= HidSubject.ClientID %>");
            hidSubject.value = subject_id;
            <%= Page.GetPostBackEventReference(LnkBtnSubject) %>;
        }
        function OK()
        {
            var datas = Array();
            datas[0] = "<%= subjectItems %>"; // 형태( 각각의 Row 콤마(凸) 로 구분)
            opener.setSubjectAdd(datas);
            opener.focus();
            self.close();
        }
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <!-- 선택된 subject -->
    <asp:HiddenField runat ="server" ID="HidSubject" />
    <asp:LinkButton ID="LnkBtnSubject" runat = "server" OnClick="LnkBtnSubject_Click" />


    <!-- 팝업 컨텐츠 시작 -->
    <div class="pop-container section-board">
        <h3><asp:Label ID="lblMenuTitle" runat="server" Text="" meta:resourcekey="lblMenuTitle" /></h3>
        <!--<p></p>-->

        <!-- 검색 -->
        <div class="search-box">
            <div class="search-group">
                <!--언어-->
                <asp:Label ID="lblLang" CssClass="title" runat="server" meta:resourcekey="lblLang" />
                <asp:DropDownList ID="ddlLang" runat="server" />
            </div>
            <div class="search-group">
                <!--분류-->
                <asp:Label ID="lblClassification" CssClass="title" runat="server" meta:resourcekey="lblClassification" />
                <asp:DropDownList ID="ddlClassification" runat="server" />
            </div>
            <div class="search-group">
                <!--과목명 -->
                <asp:Label ID="lblSubject" CssClass="title" runat="server" meta:resourcekey="lblSubject" />
                <asp:TextBox ID="txtSubject" runat="server" />
            </div>
            <div class="search-group">
                <!--강사명 -->
                <asp:Label ID="lblInstructor" CssClass="title" runat="server" meta:resourcekey="lblInstructor" />
                <asp:TextBox ID="txtInstructor" runat="server" />
            </div>
            <asp:Button ID="btnRetrieve" runat ="server" Text="Search" CssClass="button-board-search" OnClick="btnRetrieve_Click" />
        </div>
        
        <div class="button-box right">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button-default blue" OnClick="btnAdd_Click" />
        </div>


        <div class="gm-grid column-2">
            <div class="item">
                <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />

                <div class="gm-table data-table list-type">

                    <C1WebGrid:C1WebGrid ID="grd" runat="server" AllowSorting="True"  AllowColSizing="True"  CssClass="grid_main" AutoGenerateColumns="false" OnItemCreated="grd_ItemCreated">
                    <Columns>

                        <C1WebGrid:C1TemplateColumn >
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                            </ItemTemplate>
                            <ItemStyle Width="5%" />
                        </C1WebGrid:C1TemplateColumn>
                        <C1WebGrid:C1BoundColumn DataField="subject_lang">
                            <ItemStyle Width="11%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="subject_kind" >
                            <ItemStyle Width="12%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1TemplateColumn >
                            <ItemTemplate>
                                <a href="#" onclick="setSelectSubject( '<%# DataBinder.Eval(Container.DataItem, "subject_id")%>');">
                                <%# DataBinder.Eval(Container.DataItem, "subject_nm")%></a>
                            </ItemTemplate>
                            <ItemStyle Width="37%" CssClass ="left" />
                        </C1WebGrid:C1TemplateColumn>

                        <C1WebGrid:C1BoundColumn DataField="learning_time" Visible="false" >
                            <ItemStyle Width="8%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="instructor" >
                            <ItemStyle Width="15%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="use_flg" >
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="ins_dt" Visible="false" DataFormatString="{0:yyyy.MM.dd}">
                            <ItemStyle Width="8%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="temp_save_flg" >
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1TemplateColumn>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk" runat = "server"/>
                            </ItemTemplate>
                            <ItemStyle Width="5%" />
                        </C1WebGrid:C1TemplateColumn>

                        <C1WebGrid:C1BoundColumn DataField="subject_id" Visible="false">
                            <ItemStyle Width="0%" />
                        </C1WebGrid:C1BoundColumn>

                        <C1WebGrid:C1BoundColumn DataField="subject_nm" Visible="false">
                            <ItemStyle Width="0%" />
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
            </div>
            
            <div class="item">
                 <CLTWebControl:PageInfo ID="PageInfo2" runat="server" />

                 <div class="gm-table data-table list-type">

                    <C1WebGrid:C1WebGrid ID="grd2" runat="server" AllowSorting="True"  AllowColSizing="True" CssClass="grid_main" AutoGenerateColumns="false"
                        DataKeyField="CONTENTS_ID" OnItemCreated="grd2_ItemCreated">
                        <Columns>

                            <C1WebGrid:C1TemplateColumn>
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                                </ItemTemplate>
                                <ItemStyle Width="5%" />
                            </C1WebGrid:C1TemplateColumn>
                            <C1WebGrid:C1BoundColumn DataField="contents_nm">
                                <ItemStyle Width="23%" CssClass ="left"></ItemStyle>
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="contents_file_nm">
                                <ItemStyle Width="10%" />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="contents_type">
                                <ItemStyle Width="9%" />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="contents_lang">
                                <ItemStyle Width="8%" />
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="contents_remark">
                                <ItemStyle Width="24%" CssClass ="left"></ItemStyle>
                            </C1WebGrid:C1BoundColumn>
                            <C1WebGrid:C1BoundColumn DataField="ins_dt" DataFormatString="{0:yyyy.MM.dd}">
                                <ItemStyle Width="10%" />
                            </C1WebGrid:C1BoundColumn>

                            <C1WebGrid:C1BoundColumn DataField="temp_save_flg" >
                                <ItemStyle Width="9%" />
                            </C1WebGrid:C1BoundColumn>

                            <C1WebGrid:C1BoundColumn DataField="contents_id" Visible="false">
                                <ItemStyle Width="0%" />
                            </C1WebGrid:C1BoundColumn>
                        
                        </Columns>
                        <HeaderStyle Font-Bold="true" />
                        <ItemStyle  Wrap="true"  />
                        <AlternatingItemStyle />
                    </C1WebGrid:C1WebGrid>
            
                </div>
        
                <div class="gm-paging">
                    <CLTWebControl:PageNavigator ID="PageNavigator2" runat="server" OnOnPageIndexChanged="PageNavigator2_OnPageIndexChanged" />
                </div>
            </div>
            
        </div>

    </div>
    <!--// 팝업 컨텐츠 끝 -->

</asp:Content>
