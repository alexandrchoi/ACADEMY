<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="company.aspx.cs" Inherits="CLT.WEB.UI.LMS.APPR.company" 
    Culture="auto" UICulture="auto" %>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <script type="text/javascript" language="javascript">
        
    </script>
</asp:Content>



<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-fix">
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="Job competency" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack()"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>
    <p>View 버튼을 클릭하면 직급별/선종별 역량정의 및 역량기준을 확인하실 수 있습니다.</p>

    <section class="section-board">
        <div class="sub-tab">
            <a href="#none" class="current" data-tab="tab-1" id="tab_1">DECK</a>
            <a href="#none" data-tab="tab-2" id="tab_2">ENGINE</a>
        </div>

        <!-- DECK -->
        <div id="tab-1" class="tab-content current">
            <div class="gm-table data-table list-type">
                <table>
                    <caption>Pctc, Cntr, Bulk, Lng, Tanker의 Rank</caption>

                    <thead>
                    <tr>
                        <th scope="col">Rank</th>
                        <th scope="col" colspan="5">Ship type</th>
                    </tr>
                    <tr>
                        <th scope="col"></th>
                        <th scope="col">PCTC</th>
                        <th scope="col">CNTR</th>
                        <th scope="col">BULK</th>
                        <th scope="col">LNG</th>
                        <th scope="col">TANKER</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr>
                        <th scope="row">Capt.</th>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                    </tr>
                    <tr>
                        <th scope="row">C/O</th>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                    </tr>
                    <tr>
                        <th scope="row">2/O</th>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                    </tr>
                    <tr>
                        <th scope="row">3/O</th>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                    </tr>
                    <tr>
                        <th scope="row">M/O</th>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                    </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <!--// DECK -->

        <!-- ENGINE -->
        <div id="tab-2" class="tab-content">
            <div class="gm-table data-table list-type">
                <table>
                    <caption>Pctc, Cntr, Bulk, Lng, Tanker의 Rank</caption>

                    <thead>
                    <tr>
                        <th scope="col">Rank</th>
                        <th scope="col" colspan="5">Ship type</th>
                    </tr>
                    <tr>
                        <th scope="col"></th>
                        <th scope="col">PCTC</th>
                        <th scope="col">CNTR</th>
                        <th scope="col">BULK</th>
                        <th scope="col">LNG</th>
                        <th scope="col">TANKER</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr>
                        <th scope="row">C/E</th>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                    </tr>
                    <tr>
                        <th scope="row">1/E</th>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                    </tr>
                    <tr>
                        <th scope="row">2/E</th>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                    </tr>
                    <tr>
                        <th scope="row">3/E</th>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                        <td><a href="#none" target="_blank" class="button-default blue">View</a></td>
                    </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <!--// DECK -->

    </section>
</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>