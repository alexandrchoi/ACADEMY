using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 어셈블리의 일반 정보는 다음 특성 집합을 통해 제어됩니다.
// 어셈블리와 관련된 정보를 수정하려면
// 이 특성 값을 변경하십시오.
[assembly: AssemblyTitle("CLT.WEB.BIZ.LMS.EDUM")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("CLT.WEB.BIZ.LMS.EDUM")]
[assembly: AssemblyCopyright("Copyright (C)  2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// ComVisible을 false로 설정하면 이 어셈블리의 형식이 COM 구성 요소에 
// 노출되지 않습니다. COM에서 이 어셈블리의 형식에 액세스하려면 
// 해당 형식에 대해 ComVisible 특성을 true로 설정하십시오.
[assembly: ComVisible(false)]

// 이 프로젝트가 COM에 노출되는 경우 다음 GUID는 typelib의 ID를 나타냅니다.
[assembly: Guid("78e6579f-1ee1-4495-82ea-38a3e4c1cd0c")]

// 어셈블리의 버전 정보는 다음 네 가지 값으로 구성됩니다.
//
//      주 버전
//      부 버전 
//      빌드 번호
//      수정 버전
//
// 모든 값을 지정하거나 아래와 같이 '*'를 사용하여 빌드 번호 및 수정 버전이 자동으로
// 지정되도록 할 수 있습니다.
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]

// Go Live 2012.05.25 (1.0.0.0)

//[assembly: AssemblyVersion("1.0.0.1")]
//[assembly: AssemblyFileVersion("1.0.0.1")]
/*
[CHM-201218387] LMS 교육대상자 선발 관련
* 박정호 2012.06.14
* Source
  vp_a_edumng_md.cs
* Comment 
  교육 대상자 선발 추가
*/

// 1.0.0.2
//[CHM-201218410] LMS 영문페이지 영문화 작업 (언어 선택에 따른 조회 내용 국문/영문 변경 작업 )
//* 김은정 2012.06.15
//* Source 
//  vp_a_edumng_md
//* Comment 
//   : LMS 영문페이지 영문화 작업 (Evaluation results registration/Inquiry, Job Competency Evluation inquiry,  Register Training Record)

// 1.0.0.3
//[CHM-201218490] - ShipSchool  엑셀출력 컬럼명 변경
//* 박정호 2012.06.15
//* Source
//    vp_a_edumng_md.cs
//* Comment
//*   step_name -> duty_work_name 으로 변경

// 1.0.0.4
//[CHM-201218642] - [LMS] 미수료 현황 조건변경, 비용 수급 사업장 번호 고용보험 번호로 변경
//* 박정호 2012.06.29
//* Source
//    vp_a_edumng_md.cs
//* Comment
//*   미수료 현황 list 변경, 비용 수급 사업장 엑셀 출력 변경

//[assembly: AssemblyVersion("1.0.0.5")]
//[assembly: AssemblyFileVersion("1.0.0.5")]
//[CHM-201219386] LMS 기능 개선 요청
//*서진한 2012.08.01
//* Source
//  vp_a_edumng_md
//* Comment 
//  Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제따른 변경//  교육대상자 선발 

//[assembly: AssemblyVersion("1.0.0.6")]
//[assembly: AssemblyFileVersion("1.0.0.6")]
//[CHM-201219386] LMS 기능 개선 요청
//* 김은정 2012.08.08
//* Source
//  vp_a_edumng_md
//* Comment 
//  교육 대상자 선발 시 타사 교육이력 대상자도 포함하여 선정 / 직책, 이름, 사번 정렬 적용

//[assembly: AssemblyVersion("1.0.0.7")]
//[assembly: AssemblyFileVersion("1.0.0.7")]
///[CHM-201218484] 국토해양부 로고 추가 및 데이터 정렬 기준 추가
///* 김은정 2012.09.14
///* Source
///   vp_a_edumng_md.GetEduIssuingUserReport(string[] rParams), GetEduListNew(string[] rParams, CultureInfo rArgCultureInfo)
///* Comment 
///   교육 대상자 선발 시 자사+타사 교육 이력 조회 시 기존 현재사번이 아닌 공통사번을 기준으로 대상자 제외하도록 변경
///   국토해양부 과정일 경우 국토해양부 Logo 표시
///   
//[assembly: AssemblyVersion("1.0.0.8")]
//[assembly: AssemblyFileVersion("1.0.0.8")]
///[CHM-201218248] LMS 영문페이지 내용 수정 요청
/// * 김은정 2012.12.10
/// * Source
///   vp_a_edumng_md.GetEduTrainigRecordList, GetEduTrainigRecord, GetEduTrainigRecordDelete
/// * Comment 
///   교육 결과 입력 시 키값 COURSE_ID 까지 지정하여 조회, 저장, 삭제 가능하도록 변경

//[assembly: AssemblyVersion("1.0.0.9")]
//[assembly: AssemblyFileVersion("1.0.0.9")]
///[CHM-201325393] 변경요청
/// * 서진욱 2013.07.15
/// * Source
///   vp_a_edumng_md.cs
/// * Comment 
///   교육 이수(수료)처리시 교육이수 증서번호 자동 발급 기능 추가(T_COURSE_RESULT에 필드 추가) 
///   
//[assembly: AssemblyVersion("1.0.1.0")]
//[assembly: AssemblyFileVersion("1.0.1.0")]
//[CHM-201325394] LMS 기능 개선
// * 서진욱 2013.03.19
// * Source
//   : vp_a_edumng_md.cs
//* Comment 
//   : 교육대상자 선발시 100명씩 보이도록
//   : 직무기술역량평가에 완료여부 컬럼추가


//[assembly: AssemblyVersion("1.0.1.1")]
//[assembly: AssemblyFileVersion("1.0.1.1")]
//[CHM-201327719] 수료현황 교육기간 표시 날짜 수정
// * 김진일 2013.11.20
// * Source
//   : vp_a_edumng_md.cs
//* Comment 
//   : 수료현황 조회시 교육 기간이 개설 과정의 시작일~마감일이 나오던 것을 각 개인이 실제로 받았던 날짜로 나오도록 수정(GetEduResultList)
//   : 미수료현황 조회시 교육 기간이 개설 과정의 시작일~마감일이 나오던 것을 각 개인이 실제로 받았던 날짜로 나오도록 수정(GetEduResultNonPassList)