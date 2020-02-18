<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="qna_detail.aspx.cs" Inherits="CLT.WEB.UI.LMS.COMMUNITY.qna_detail" 
    Culture="auto" UICulture="auto" %>

<%@ Register TagPrefix="CLTWebControl" Namespace="CLT.WEB.UI.COMMON.CONTROL" Assembly="CLT.WEB.UI.COMMON.CONTROL" %>
<%@ Register Assembly="C1.Web.C1WebGrid.2, Version=2.1.20063.84, Culture=neutral, PublicKeyToken=589f1fc067ff4031"
    Namespace="C1.Web.C1WebGrid" TagPrefix="C1WebGrid" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" language="javascript">
        
    </script>
</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-fix">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="Q&A" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack();return false;"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>

    <section class="section-board board-reply">

        <div class="board-list gm-table data-table read-type">
            <table>
                <colgroup>
                    <col width="10%">
                    <col width="*">
                </colgroup>
                <tbody>
                <tr>
                    <th scope="row">
                        <asp:Label ID="lblSubjectTitle" runat="server" Text="제목" meta:resourcekey="lblSubjectTitle" />
                    </th>
                    <td>
                        <asp:Label ID="lblSubject" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <dl class="read-info">
                            <dt>
                                <asp:Label ID="lblCreatedByTitle" runat="server" Text="작성자" meta:resourcekey="lblCreatedByTitle" />
                            </dt>
                            <dd>
                                <asp:Label ID="lblCreatedBy" runat="server" />
                            </dd>
                            <dt>
                                <asp:Label ID="lblCreatedTitle" runat="server" Text="작성일자" meta:resourcekey="lblCreatedTitle" />
                            </dt>
                            <dd>
                                <asp:Label ID="lblCreated" runat="server" />
                            </dd>
                            <dt>
                                <asp:Label ID="lblHitCntTitle" runat="server" Text="조회수" meta:resourcekey="lblHitCntTitle" />
                            </dt>
                            <dd>
                                <asp:Label ID="lblHitCnt" runat="server" />
                            </dd>
                            <dt id="dtAttFile" runat="server" Visible="false">
                                <asp:Label ID="lblAttFile" runat="server" Text="첨부파일" meta:resourcekey="lblAttFile" />
                            </dt>
                            <dd id="ddAttFile" runat="server" Visible="false" class="attach-list">
                                <asp:Label ID="lblDown1" runat="server" />&nbsp
                                <asp:LinkButton ID="btnDown1" runat = "server" OnClick="btnDown_Click" />
                                &nbsp;&nbsp;
                                <asp:Label ID="lblDown2" runat="server" />&nbsp
                                <asp:LinkButton ID="btnDown2" runat = "server" OnClick="btnDown_Click" />
                                &nbsp;&nbsp;
                                <asp:Label ID="lblDown3" runat="server" />&nbsp
                                <asp:LinkButton ID="btnDown3" runat = "server" OnClick="btnDown_Click" />
                            </dd>
                        </dl>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="read-content">
                            <asp:Label ID="lblContent" runat="server" />
                        </div>
                    </td>
                </tr>

                <!-- 댓글 목록 -->
                <tr>
                    <td colspan="2">
                        <h4 class="title-comment">Comment</h4>
                        <ul class="comment-list">
                            <asp:Repeater runat="server" ID="rPtReply">
                                <ItemTemplate>
                                    <li>
                                        <div class="writer-info">
                                            <span class="writer-name"><%# DataBinder.Eval(Container.DataItem, "user_nm_kor") %></span>
                                            <span class="write-date"><%# DataBinder.Eval(Container.DataItem, "ins_dt") %></span>
                                        </div>
                                        <div class="comment-show">
                                            <%# DataBinder.Eval(Container.DataItem, "rep_content") %>
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <!--ul class="comment-list" id="">
                            <li>
                                <div class="writer-info">
                                    <span class="writer-name">홍길동</span>
                                    <span class="write-date">2019.12.30</span>
                                </div>
                                <div class="comment-show">
                                    안녕하십니까, 유수에스엠 운항훈련원 우진주입니다.

                                    2016년 8월 교육 훈련 일정표를 알려드리니 휴가 계획 세우시는데 참고하시기 바랍니다.
                                    무더운 여름, 가족과 친구들과 건강하고 즐거운 휴가 보내시고 저는 교육 때 뵙겠습니다.
                                </div>
                            </li>
                            <li>
                                <div class="writer-info">
                                    <span class="writer-name">홍길동</span>
                                    <span class="write-date">2019.12.30</span>
                                </div>
                                <div class="comment-show">
                                    안녕하십니까, 유수에스엠 운항훈련원 우진주입니다.
                                </div>
                            </li>
                        </ul-->
                    </td>
                </tr>
                <!--// 댓글 목록 -->

                <!-- 댓글 작성 -->
                <tr>
                    <td colspan="2">
                        <h4 class="title-comment"><asp:Label ID="lblWriteReply" runat="server" Text="댓글 작성" meta:resourcekey="lblWriteReply" /></h4>
                        <div class="comment-write">
                            <asp:TextBox ID="txtReplace" runat="server" TextMode="MultiLine" />
                            <asp:Button ID="btnReplaceSave" runat="server" Text="Save" CssClass="button-default blue" OnClick="btnReplaceSave_OnClick" />
                        </div>
                    </td>
                </tr>
                <!--// 댓글 작성 -->
                </tbody>
            </table>
        </div>

        <div class="button-box right">
            <asp:Button ID="btnList" runat="server" Text="List" CssClass="button-default blue" OnClick="button_Click" meta:resourcekey="btnListResource" />
            <asp:Button ID="btnModify" runat="server" Text="Modify" CssClass="button-default" OnClick="button_Click" meta:resourcekey="btnModifyResource" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button-default" OnClick="button_Click" Visible="False" meta:resourcekey="btnDeleteResource" />
            <asp:Button ID="btnRestore" runat="server" Text="Restore" CssClass="button-default" OnClick="button_Click" Visible="False" meta:resourcekey="btnRestoreResource" />
        </div>
    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>