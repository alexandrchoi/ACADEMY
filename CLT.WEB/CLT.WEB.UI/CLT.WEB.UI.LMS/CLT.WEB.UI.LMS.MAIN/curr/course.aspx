<%@ Page Language="C#" MasterPageFile="/MasterSub.Master" AutoEventWireup="true" CodeBehind="course.aspx.cs" Inherits="CLT.WEB.UI.LMS.CURR.course" 
    Culture="auto" UICulture="auto" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    

<!-- 서브 컨텐츠 시작 -->
<div class="section-fix">
    
    <h2 class="page-title"><asp:Label ID="lblMenuTitle" runat="server" Text="교육과정" meta:resourcekey="lblMenuTitle" />
        <!-- 모바일 뒤로 가기 -->
        <span class="goback">
			<button onclick="goBack()"><asp:Label ID="txtGoback" runat="server" CssClass="off-screen" Text="뒤로가기" meta:resourcekey="txtGobackResource" /></button>
		</span>
    </h2>
    
    <section class="section-board">

        <!-- Data Table - List type -->
        <div class="gm-table data-table list-type">
            <table>
                <caption>표정보 - 번호, 운영, 과정명, 대상자 등을 제공합니다.</caption>
                <colgroup>
                    <col width="5%">
                    <col width="5%">
                    <col width="*">
                    <col width="20%">
                    <col width="5%">
                    <col width="15%">
                </colgroup>
                <thead>
                <tr>
                    <th scope="col">번호</th>
                    <th scope="col">운영</th>
                    <th scope="col">과정명</th>
                    <th scope="col">대상자</th>
                    <th scope="col">일수</th>
                    <th scope="col">비고</th>
                </tr>
                </thead>

                <tbody>
                <tr>
                    <td>1</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box pink">컨소시엄</span>자동차운반선직무</td>
                    <td>PCTC 승선예정자</td>
                    <td>2</td>
                    <td><a href="/file/curr/01_car_carry.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>2</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box pink">컨소시엄</span>자동차운반선화재안전</td>
                    <td>PCTC 전 승무원</td>
                    <td>1</td>
                    <td><a href="/file/curr/02_car_fire.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>3</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box pink">컨소시엄</span>자동차운반선장비운용</td>
                    <td>3항사, 2항사 직무예정자</td>
                    <td>1</td>
                    <td><a href="/file/curr/03_car_gear.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>4</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box pink">컨소시엄</span>선박조종기술숙련</td>
                    <td>전 항해사관</td>
                    <td>3</td>
                    <td><a href="/file/curr/04_ship_plan.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>5</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box pink">컨소시엄</span>항해계획운용실무</td>
                    <td>전 항해사관</td>
                    <td>2</td>
                    <td><a href="/file/curr/05_sail_plan.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>6</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box pink">컨소시엄</span>전자해도장치운용실무</td>
                    <td>전 항해사관</td>
                    <td>2</td>
                    <td><a href="/file/curr/06_elec_navi.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>7</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box pink">컨소시엄</span>선박위험물운송관리</td>
                    <td>선장, 3항사</td>
                    <td>1</td>
                    <td><a href="/file/curr/07_danger_carry.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>8</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box pink">컨소시엄</span>전자엔진제어시스템</td>
                    <td>초급 기관사관</td>
                    <td>2</td>
                    <td><a href="/file/curr/08_elec_engine.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>9</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box pink">컨소시엄</span>선상의료관리와응급처치</td>
                    <td>해운종사자</td>
                    <td>1</td>
                    <td><a href="/file/curr/09_medical.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>10</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box pink">컨소시엄</span>선박사고조사자전문</td>
                    <td>고급사관</td>
                    <td>2</td>
                    <td><a href="/file/curr/10_accident_report.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>11</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box pink">컨소시엄</span>선교도선관리기술</td>
                    <td>전 항해사관</td>
                    <td>1</td>
                    <td><a href="/file/curr/11_road_manage.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>12</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box pink">컨소시엄</span>선박조종기술숙련(향상)</td>
                    <td>전 항해사관</td>
                    <td>3</td>
                    <td><a href="/file/curr/12_ship_plan_high.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>13</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box pink">컨소시엄</span>항해계획운용실무(향상)</td>
                    <td>전 항해사관</td>
                    <td>2</td>
                    <td><a href="/file/curr/13_sail_plan_high.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>14</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box pink">컨소시엄</span>전자해도장치운용실무(향상)</td>
                    <td>전 항해사관</td>
                    <td>2</td>
                    <td><a href="/file/curr/14_elec_navi_high.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>15</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box blue">위탁</span>ECDIS(전자해도장치) 교육</td>
                    <td>전 항해사관</td>
                    <td>5</td>
                    <td><a href="/file/curr/15_ecdis_edu.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>16</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box blue">위탁</span>ERS(엔진룸 시뮬레이터) 교육</td>
                    <td>초임 3기사</td>
                    <td>2</td>
                    <td><a href="/file/curr/16_ers_edu.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>17</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box blue">위탁</span>선박모의조종 및 선교 팀워크 교육</td>
                    <td>전 항해사관</td>
                    <td>5</td>
                    <td><a href="/file/curr/17_simulation.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>18</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box blue">위탁</span>리더십 및 팀워크 교육</td>
                    <td>전 사관</td>
                    <td>3</td>
                    <td><a href="/file/curr/18_leadership_teamwork.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>19</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box blue">위탁</span>리더십 및 관리기술 직무교육</td>
                    <td>전 고급사관</td>
                    <td>3</td>
                    <td><a href="/file/curr/19_leadership_manage.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>20</td>
                    <td>집체</td>
                    <td class="left"><span class="text-box blue">위탁</span>SHS(선박조종시뮬레이터) 교육</td>
                    <td>전 항해사관</td>
                    <td>3</td>
                    <td><a href="/file/curr/20_shs_edu.pdf" class="button-underline" target="_blank">상세보기</a></td>
                </tr>
                <tr>
                    <td>21</td>
                    <td>집체</td>
                    <td class="left">초임사관 입사오리엔테이션</td>
                    <td>전 초임사관</td>
                    <td>5</td>
                    <td></td>
                </tr>
                <tr>
                    <td>22</td>
                    <td>집체</td>
                    <td class="left">3항사직무과정</td>
                    <td>초임 3항사</td>
                    <td>5</td>
                    <td></td>
                </tr>
                <tr>
                    <td>23</td>
                    <td>집체</td>
                    <td class="left">GMDSS과정</td>
                    <td>초임 3항사</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>24</td>
                    <td>집체</td>
                    <td class="left">3기사직무과정</td>
                    <td>초임 3기사</td>
                    <td>5</td>
                    <td></td>
                </tr>
                <tr>
                    <td>25</td>
                    <td>집체</td>
                    <td class="left">전기전자과정</td>
                    <td>초임 3기사</td>
                    <td>3</td>
                    <td></td>
                </tr>
                <tr>
                    <td>26</td>
                    <td>집체</td>
                    <td class="left">VM과정(Vessel Manager)</td>
                    <td>전 초임사관</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>27</td>
                    <td>집체</td>
                    <td class="left">경력사관입사과정</td>
                    <td>[채용]경력사관</td>
                    <td>3</td>
                    <td></td>
                </tr>
                <tr>
                    <td>28</td>
                    <td>집체</td>
                    <td class="left">경력부원입사과정</td>
                    <td>[채용]경력부원</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>29</td>
                    <td>집체</td>
                    <td class="left">실습생오리엔테이션</td>
                    <td>실습사관</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>30</td>
                    <td>집체</td>
                    <td class="left">사관 안전교육</td>
                    <td>전 사관</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>31</td>
                    <td>집체</td>
                    <td class="left">사관워크숍</td>
                    <td>전 사관</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>32</td>
                    <td>집체</td>
                    <td class="left">리더십교육</td>
                    <td>전 사관</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>33</td>
                    <td>집체</td>
                    <td class="left">부원 안전교육</td>
                    <td>전 부원</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>34</td>
                    <td>집체</td>
                    <td class="left">PSC 대응실무</td>
                    <td>고급 항해사관(BULK)</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>35</td>
                    <td>집체</td>
                    <td class="left">SAFETY OFFICER과정</td>
                    <td>전 고급사관</td>
                    <td>2</td>
                    <td></td>
                </tr>
                <tr>
                    <td>36</td>
                    <td>집체</td>
                    <td class="left">선박관리능력향상과정</td>
                    <td>선기장 직무 예정자</td>
                    <td>2</td>
                    <td></td>
                </tr>
                <tr>
                    <td>37</td>
                    <td>집체</td>
                    <td class="left">1항사직무과정</td>
                    <td>1항사 직무 예정자</td>
                    <td>3</td>
                    <td></td>
                </tr>
                <tr>
                    <td>38</td>
                    <td>집체</td>
                    <td class="left">1기사직무과정</td>
                    <td>1기사 직무 예정자</td>
                    <td>2</td>
                    <td></td>
                </tr>
                <tr>
                    <td>39</td>
                    <td>집체</td>
                    <td class="left">CNTR선직무과정</td>
                    <td>전 항해사관(CNTR)</td>
                    <td>2</td>
                    <td></td>
                </tr>
                <tr>
                    <td>40</td>
                    <td>집체</td>
                    <td class="left">선박위험물관리교육</td>
                    <td>전 항해사관</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>41</td>
                    <td>집체</td>
                    <td class="left">BULK선직무과정</td>
                    <td>고급 항해사관(BULK)</td>
                    <td>2</td>
                    <td></td>
                </tr>
                <tr>
                    <td>42</td>
                    <td>집체</td>
                    <td class="left">PCTC 직무과정(항해)</td>
                    <td>전 항해사관</td>
                    <td>2</td>
                    <td></td>
                </tr>
                <tr>
                    <td>43</td>
                    <td>집체</td>
                    <td class="left">PCTC 직무과정(기관)</td>
                    <td>전 기관사관</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>44</td>
                    <td>집체</td>
                    <td class="left">PCTC 화재안전교육</td>
                    <td>PCTC 전 선원</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>45</td>
                    <td>집체</td>
                    <td class="left">PCTC 장비운용 심화과정</td>
                    <td>3항사(PCTC)</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>46</td>
                    <td>위탁</td>
                    <td class="left">FORK LIFT 운용</td>
                    <td>초급 항해사(PCTC)</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>47</td>
                    <td>집체</td>
                    <td class="left">SHS특별과정(REFRESH)</td>
                    <td>선장</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>48</td>
                    <td>집체</td>
                    <td class="left">ECDIS REFRESH과정</td>
                    <td>선장, 3항사</td>
                    <td>2</td>
                    <td></td>
                </tr>
                <tr>
                    <td>49</td>
                    <td>집체</td>
                    <td class="left">JRC ECDIS TYPE SPECIFIC과정(신)</td>
                    <td>전 항해사관</td>
                    <td>2</td>
                    <td></td>
                </tr>
                <tr>
                    <td>50</td>
                    <td>집체</td>
                    <td class="left">JRC ECDIS TYPE SPECIFIC과정(구)</td>
                    <td>전 항해사관</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>51</td>
                    <td>집체</td>
                    <td class="left">기관사관직무심화과정(운항급)</td>
                    <td>초급 기관사관</td>
                    <td>2</td>
                    <td></td>
                </tr>
                <tr>
                    <td>52</td>
                    <td>집체</td>
                    <td class="left">RT-FLEX 친숙화 교육</td>
                    <td>RT-FLEX 미경험 기관사관</td>
                    <td>1</td>
                    <td></td>
                </tr>
                <tr>
                    <td>53</td>
                    <td>위탁</td>
                    <td class="left">ME-C MAKER교육</td>
                    <td>고급 기관사관</td>
                    <td>5</td>
                    <td></td>
                </tr>
                <tr>
                    <td>54</td>
                    <td>집체</td>
                    <td class="left">ME-C 친숙화 교육</td>
                    <td>초급 기관사관</td>
                    <td>2</td>
                    <td></td>
                </tr>
                </tbody>
            </table>
        </div>
        <!--// Data Table - List type -->

    </section>

</div>
<!--// 서브 컨텐츠 끝 -->


</asp:Content>