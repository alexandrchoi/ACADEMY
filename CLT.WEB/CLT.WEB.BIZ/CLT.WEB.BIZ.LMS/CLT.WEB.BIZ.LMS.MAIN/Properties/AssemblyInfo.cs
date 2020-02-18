using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 어셈블리의 일반 정보는 다음 특성 집합을 통해 제어됩니다.
// 어셈블리와 관련된 정보를 수정하려면
// 이 특성 값을 변경하십시오.
[assembly: AssemblyTitle("CLT.WEB.BIZ.LMS.MAIN")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("CLT.WEB.BIZ.LMS.MAIN")]
[assembly: AssemblyCopyright("Copyright (C)  2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// ComVisible을 false로 설정하면 이 어셈블리의 형식이 COM 구성 요소에 
// 노출되지 않습니다. COM에서 이 어셈블리의 형식에 액세스하려면 
// 해당 형식에 대해 ComVisible 특성을 true로 설정하십시오.
[assembly: ComVisible(false)]

// 이 프로젝트가 COM에 노출되는 경우 다음 GUID는 typelib의 ID를 나타냅니다.
[assembly: Guid("5114f4f4-c4c4-46f6-9378-e63647fb902a")]

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
// Version : 1.0.0.0
// Date    : 2012.05.25
// Author  : KIM MIN KYU
// comment : Go Live 

/* 1.0.0.1
 * [CHM-201218612] 페이지 권한, 설문조사 대상자 조회, 수강신청 기간 관련
 * 김민규 2012.06.26
 * Source
   : CLT.WEB.BIZ.LMS.MAIN.vp_m_main_md.cs
 * Comment 
   : 페이지 권한 관련 변경작업
 */
//[assembly: AssemblyVersion("1.0.0.2")]
//[assembly: AssemblyFileVersion("1.0.0.2")]
///    [CHM-201219386] LMS 기능 개선 요청
///        *서진한 2012.08.01
///        * Source
///          vp_m_main_md
///        * Comment 
///          * Comment 
///          개설된 과정 중에 최근 N개의 데이터를 현재기준 수강신청 가능일자 포함된는 과정만 조회
///          영문조회시 과정명 영문화 처리

