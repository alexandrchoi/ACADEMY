using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 어셈블리의 일반 정보는 다음 특성 집합을 통해 제어됩니다.
// 어셈블리와 관련된 정보를 수정하려면
// 이 특성 값을 변경하십시오.
[assembly: AssemblyTitle("CLT.WEB.BIZ.LMS.COMMUNITY")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("CLT.WEB.BIZ.LMS.COMMUNITY")]
[assembly: AssemblyCopyright("Copyright (C)  2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// ComVisible을 false로 설정하면 이 어셈블리의 형식이 COM 구성 요소에 
// 노출되지 않습니다. COM에서 이 어셈블리의 형식에 액세스하려면 
// 해당 형식에 대해 ComVisible 특성을 true로 설정하십시오.
[assembly: ComVisible(false)]

// 이 프로젝트가 COM에 노출되는 경우 다음 GUID는 typelib의 ID를 나타냅니다.
[assembly: Guid("d229b832-5db4-4d16-b993-2dc7f65417b9")]

// 어셈블리의 버전 정보는 다음 네 가지 값으로 구성됩니다.
//
//      주 버전
//      부 버전 
//      빌드 번호
//      수정 버전
//
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]
// Version : 1.0.0.0
// Date    : 2012.05.25
// Author  : KIM MIN KYU
// comment : Go Live 

/* 1.0.0.1
* [CHM-201218479] LMS 공지사항 게시, 설문조사 항목 유효성 체크
* 김민규 2012.06.18
* Source
  : CLT.WEB.BIZ.LMS.MANAGE.vp_y_community_notice_md.cs
* Comment 
  :  공지사항 게시 관련 수정
*/

//[assembly: AssemblyVersion("1.0.0.2")]
//[assembly: AssemblyFileVersion("1.0.0.2")]
///    [CHM-201219386] LMS 기능 개선 요청
///        *서진한 2012.08.01
///        * Source
///          vp_y_community_data_md
///          vp_y_community_faq_md
///          vp_y_community_notice_md
///          vp_y_community_qna_md
///        * Comment 
///          * Comment 
///          Execute(string DBAlias, OracleCommand command, OracleTransaction transaction) 삭제따른 변경
///          영문화 작업
///          

//[assembly: AssemblyVersion("1.0.0.3")]
//[assembly: AssemblyFileVersion("1.0.0.3")]
//[CHM-201327273] 커뮤니티 게시판 내역조회 로직 보완
// *서진욱 2013.10.22
// * Source
// vp_y_community_qna_md.cs
//* Comment 
// 삭제한 내역은 관리자만 조회되도록 수정