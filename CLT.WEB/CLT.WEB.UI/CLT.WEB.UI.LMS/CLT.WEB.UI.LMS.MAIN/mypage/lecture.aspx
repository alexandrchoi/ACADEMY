<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="lecture.aspx.cs" Inherits="CLT.WEB.UI.LMS.MYPAGE.lecture" 
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.Command.2" Namespace="C1.Web.Command" TagPrefix="c1c" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
<script type="text/javascript" language="javascript">

</script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-full">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="나의 강의실" meta:resourcekey="lblMenuTitle" />

        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board">

        <div class="sub-tab">
            <asp:linkbutton id="linkLecturing" runat="server" Text="Taking Course" OnClick="tabctrl_Click" CssClass="current" />
            <asp:linkbutton id="linkLectured" runat="server" Text="Completed Course" OnClick="tabctrl_Click" CssClass="" />
        </div>

        
        <!-- TAB 1 - Lecturing -->
        <asp:Panel ID="pnlLecturing" runat="server" CssClass="tab-content current" Visible="true">

            <p class="headline black center"><asp:Label ID="lbl0" runat="server" meta:resourcekey="lblGuide0" /></p>
        
            <CLTWebControl:PageInfo ID="PageInfo1" runat="server" />
            <div class="gm-table data-table list-type">
                <C1WebGrid:C1WebGrid ID="grd1" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemCreated="grd1_ItemCreated" OnItemDataBound="grd1_ItemDataBound">
                    <Columns>
                    
                        <C1WebGrid:C1BoundColumn DataField="open_course_id" Visible="false">
                            <ItemStyle Width="0%" />                                    
                        </C1WebGrid:C1BoundColumn>     
                           
                        <C1WebGrid:C1BoundColumn DataField="COURSE_TYPE_KEY" Visible="false">
                            <ItemStyle Width="0%" />                                    
                        </C1WebGrid:C1BoundColumn>     
                            
                        <C1WebGrid:C1TemplateColumn >
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                            </ItemTemplate>                                                                         
                            <ItemStyle Width="5%" />
                        </C1WebGrid:C1TemplateColumn>                                                                                                                           
                                                           
                        <C1WebGrid:C1BoundColumn DataField="course_type">
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="course_nm" >
                            <ItemStyle Width="35%" />
                        </C1WebGrid:C1BoundColumn>
                                
                        <C1WebGrid:C1BoundColumn DataField="progress_rate">
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="assess_score">
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="report_score">
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="total_score">
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>
                                
                        <C1WebGrid:C1TemplateColumn>
                            <ItemTemplate>
                                <asp:Button ID="btnPrint" CssClass="button-default" Text="Study" runat="server" OnClick="grd1btn_Click" />                                        
                            </ItemTemplate>
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1TemplateColumn>

                    </Columns>
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle Wrap="true"  />
                    <AlternatingItemStyle />
                </C1WebGrid:C1WebGrid>
            </div>

            <div class="gm-paging">
                <CLTWebControl:PageNavigator ID="PageNavigator1" runat="server" OnOnPageIndexChanged="PageNavigator1_OnPageIndexChanged" />
            </div>

        </asp:Panel>
        
        <!-- TAB 2 - Lectured -->
        <asp:Panel ID="pnlLectured" runat="server" CssClass="tab-content" Visible="false">
            
            <p class="headline black center"><asp:Label ID="lbl1" runat="server" meta:resourcekey="lblGuide1" /></p>
        
            <CLTWebControl:PageInfo ID="PageInfo2" runat="server" />
            <div class="gm-table data-table list-type">
                <C1WebGrid:C1WebGrid ID="grd2" AllowSorting="True" AllowColSizing="True" runat="server" AutoGenerateColumns="false" OnItemCreated="grd2_ItemCreated" OnItemDataBound="grd2_ItemDataBound">
                    <Columns>
                    
                        <C1WebGrid:C1BoundColumn DataField="open_course_id" Visible="false">
                            <ItemStyle Width="0%" />                                    
                        </C1WebGrid:C1BoundColumn>     
                           
                        <C1WebGrid:C1BoundColumn DataField="COURSE_TYPE_KEY" Visible="false">
                            <ItemStyle Width="0%" />                                    
                        </C1WebGrid:C1BoundColumn>     
                            
                        <C1WebGrid:C1TemplateColumn >
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 + this.PageSize * (this.CurrentPageIndex - 1)%>
                            </ItemTemplate>                                                                         
                            <ItemStyle Width="5%" />
                        </C1WebGrid:C1TemplateColumn>                                                                                                                           
                                                           
                        <C1WebGrid:C1BoundColumn DataField="course_type">
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="course_nm" >
                            <ItemStyle Width="35%" />
                        </C1WebGrid:C1BoundColumn>
                                
                        <C1WebGrid:C1BoundColumn DataField="progress_rate">
                            <ItemStyle Width="8%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="assess_score">
                            <ItemStyle Width="8%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="report_score">
                            <ItemStyle Width="8%" />
                        </C1WebGrid:C1BoundColumn>
                        <C1WebGrid:C1BoundColumn DataField="total_score">
                            <ItemStyle Width="8%" />
                        </C1WebGrid:C1BoundColumn>
                                
                        <C1WebGrid:C1BoundColumn DataField="pass_flg">
                            <ItemStyle Width="8%" />
                        </C1WebGrid:C1BoundColumn>
                                
                        <C1WebGrid:C1TemplateColumn>
                            <ItemTemplate>
                                <asp:Button ID="btnPrint2" CssClass="button-default" Text="Study" runat="server" OnClick="grd2btn_Click" />                                                                                
                            </ItemTemplate>
                            <ItemStyle Width="10%" />
                        </C1WebGrid:C1TemplateColumn>

                    </Columns>
                    <HeaderStyle Font-Bold="true" />
                    <ItemStyle Wrap="true"  />
                    <AlternatingItemStyle />
                </C1WebGrid:C1WebGrid>
            </div>

            <div class="gm-paging">
                <CLTWebControl:PageNavigator ID="PageNavigator2" runat="server" OnOnPageIndexChanged="PageNavigator1_OnPageIndexChanged" />
            </div>
        
        </asp:Panel >


    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>