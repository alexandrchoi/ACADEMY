
/*******************************************
*  기능 :  날짜 체크, 날짜 컨트롤에서 사용 *
*  parameter : obj                         *
********************************************/
function ChkDate(obj)
{
    // Delete one without condition on backspace key press.
    if(event.keyCode == 8)
    {
        return;
    }

    var str = obj.value;

    // Only number input
    if (str.search( /[^0-9]/ ) != -1)
    {
        if(str.length > 5)
        {
            if(str.substr(5,1).search( /[^0-1]/ ) != -1)
            {
                obj.value = str.substr(0, str.length - 1);
                return;
            }
            else if(str.substr(6,1).search( /[^0-9]/ ) != -1)
            {
                obj.value = str.substr(0, str.length - 1);
                return;
            }
            else if(str.substr(5,2) > 12)
            {
                obj.value = str.substr(0, str.length - 1);
                return;
            }
            else if(str.substr(8,1).search( /[^0-3]/ ) != -1)
            {
                obj.value = str.substr(0, str.length - 1);
                return;
            }
            else if(str.substr(9,1).search( /[^0-9]/ ) != -1)
            {
                obj.value = str.substr(0, str.length - 1);
                return;
            }
            else if(str.length > 9)
            {
                var iYear = str.substr(0, 4);
                var iMonth = str.substr(5, 2);
                var iDay = str.substr(8, 2);
				
				//Get last day of current month
                var EDay = new Date(iYear, iMonth, 0).getDate();

                if(iDay > EDay)
                {
                    if(str.substr(5,2) == "02")
                    {
                        obj.value = str.substr(0, str.length - 2);
                    }
                    else
                    {
                        obj.value = str.substr(0,str.length - 1);
                    } 
                }
            }
        }
        else
        {
            obj.value = str.substr(0, str.length - 2);
            return;
        }
    }

    if(str.length == 4)
    {
        obj.value = obj.value + ".";
    }
    else if(str.length == 7)
    {
        obj.value = obj.value + ".";
    }
}

/*********************************
*  기능 :  isEmpty 및 공백 처리  *
*  parameter : fldId, error_msg  *
**********************************/
function isEmpty(fldId, error_msg)
{   
     var fldValue = fldId.value.replace(/\s/gi, "");
     
     if(fldId == "undefined")
        return true;
     
     if(error_msg == "") 
     {
          if(fldValue == "")
              return true;              
          else
              return false;
     } 
     else 
     {
          if(fldValue == "")
          {
               alert(error_msg);
               fldId.focus() ;
               return true;
          }
          else
          {      
               return false;
          }
     }
}

/*********************************
*  기능 :  값이 있는 경우 체크
*  parameter : fldId, error_msg  *
**********************************/
function isNotEmpty(fldId, error_msg)
{   
     var fldValue = fldId.value.replace(/\s/gi, "");
     
     if(fldId == "undefined")
        return true;
     
     if(error_msg == "") 
     {
          if(fldValue == "")
              return false;              
          else
              return true;
     } 
     else 
     {    
          if(fldValue == "")
          {    
               return false;
          }
          else
          {
               alert(error_msg);
               fldId.focus();      
               return true;
          }
     }
}

/******************************************
* 함수명    : 시작 날짜와 끝날짜 비교하는 함수
* 기능      : compareDate에서 사용
* parameter : fromdt, todt
*******************************************/
function cfCompareDate(frmId, toId){
    var fromdt = frmId.value.replace(/\./gi, "").replace(/\./gi, "").replace(/\s/gi, "");
    var todt   = toId.value.replace(/\./gi, "").replace(/\./gi, "").replace(/\s/gi, "");
    
    fromdt = parseInt(fromdt,10);
    todt = parseInt(todt,10);
    
    if(fromdt > todt){
      return false;
    }else{
      return true;
    }
}

/******************************************
* 함수명    : 날짜 체크 함수
* 기능      : compareDate에서 사용
* parameter : strId
*******************************************/
function isDate(fldId) { 
    if(isEmpty(fldId, ""))
	    return true;
	    
    var fldValue = fldId.value;
    var r = new RegExp("(19|20)\\d{2}\\.(0[1-9]|1[0-2])\\.(0[1-9]|[12][0-9]|3[01])"); 
    return r.test(fldValue); 
} 

/******************************************
* 함수명    : isDateChk
* 기능      : 날짜체크 함수
* parameter : strId
*******************************************/
function isDateChk(fldId, error_msg) { 

    if(!isDate(fldId))
    {   
        alert(error_msg);
        fldId.focus();
        return false;
    }
    
    return true;
} 


/******************************************
* 함수명    : 시작날짜, 끝날짜 함수
* 기능      : 
* parameter : frmId, toId, langKind
*******************************************/
function compareDate(frmId, toId, langKind){
    
    var fromdt = frmId.value;
    var todt   = toId.value;

    if(isEmpty(frmId, ""))
    {
        if(langKind == "ko-KR")
            alert("시작일자는 필수 항목입니다.");
        else
            alert("From date is Null.");
        frmId.focus();
        return false;
    }
    else if(isEmpty(toId, ""))
    {
        if(langKind == "ko-KR")
            alert("끝일자는 필수 항목입니다.");
        else
            alert("End date is Null.");
        toId.focus();
        return false;
    }
    
    if(!isDate(frmId))
    {
        if(langKind == "ko-KR")
            alert("시작일자가 잘못 되었습니다.");
        else
            alert("From date is abnormal formation.");
        frmId.focus();
        return false;
    }
    else if(!isDate(toId))
    {
        if(langKind == "ko-KR")
            alert("끝일자가 잘못 되었습니다.");
        else
            alert("End date is abnormal formation.");
        toId.focus();
        return false;
    }
    
    if(!cfCompareDate(frmId, toId))
    {
        if(langKind == "ko-KR")
            alert("시작일자가 끝일자보다 클 수 없습니다.");
        else
            alert("From date is abnormal formation.");
        frmId.focus();
        return false;
    }

    return true;
}

/******************************************
* 함수명    : 시작날짜, 끝날짜 함수
* 기능      : Null 입력 가능
* parameter : frmId, toId, langKind
*******************************************/
function compareDateNull(frmId, toId, langKind){
    
    var fromdt = frmId.value;
    var todt   = toId.value;
       
    if(!isDate(frmId))
    {
        if(langKind == "ko-KR")
            alert("시작일자가 잘못 되었습니다.");
        else
            alert("From date is abnormal formation.");
        frmId.focus();
        return false;
    }
    else if(!isDate(toId))
    {
        if(langKind == "ko-KR")
            alert("끝일자가 잘못 되었습니다.");
        else
            alert("End date is abnormal formation.");
        toId.focus();
        return false;
    }
    
    if(!cfCompareDate(frmId, toId))
    {
        if(langKind == "ko-KR")
            alert("시작일자가 끝일자보다 클 수 없습니다.");
        else
            alert("From date is abnormal formation.");
        frmId.focus();
        return false;
    }

    return true;
}

/******************************************
* 함수명    : isNumber
* 기능      : 숫자체크
* parameter : fldId, error_msg
*******************************************/
function isNumber(fldId, error_msg)
{   
	if(isEmpty(fldId, ""))
	    return true;
	    
	var fldValue = fldId.value;
	var pattern = /^[0-9]+$/gi;
	if (pattern.test(fldValue)) 
	{   
		return true;
    }
	else
	{	
	    alert(error_msg);
        fldId.focus();
	    return false;
	}
	return false;
}

/******************************************
* 함수명    : isUpperCase
* 기능      : 영문대문자 체크
* parameter : fldId, error_msg
*******************************************/
function isUpperCase(fldId, error_msg)
{   
    if(isEmpty(fldId, ""))
	    return true;
	    
	var fldValue = fldId.value;
	var pattern = /^[A-Z]+$/g;
	
	if (pattern.test(fldValue)) 
	{   
		return true;
    }
	else
	{	
	    alert(error_msg);
        fldId.focus();
	    return false;
	}
	return false;
}

/******************************************
* 함수명    : strLength
* 기능      : 문자열 길이구하기 isMaxLenth에서 사용           
* parameter : isMaxLenth 
*******************************************/
function strLength(fldId)
{
       var fldValue = fldId.value;
       var Length = 0;
       var Nav = navigator.appName;
       var Ver = navigator.appVersion;
       var IsExplorer = false;
       var ch;
       
       for(var i = 0 ; i < fldValue.length; i++)
       {
          ch = fldValue.charAt(i);
          if ((ch == "\n") || ((ch >= "ㅏ") && (ch <= "히")) ||
             ((ch >="ㄱ") && (ch <="ㅎ")))
          {
              Length += 3;
          } 
          else
          {
              Length += 1;
          }
       }
       
       return Length;
}

/************************************************************
 *  함수명    : isMaxLenth
 *  함수내용  : 주어진 문자열의 자릿수보다 작은지 검사
 *              isUniCode - false 일땐 1바이트로 계산, 
 *  parameter : fldId, maxLenth, isUniCode, error_msg
 ************************************************************/
function isMaxLenth(fldId, maxLenth, error_msg) //, isUniCode, error_msg)
{
    //if(isUniCode)
    //  maxLenngth = maxLenth / 3;
    
     // 바이트계산길이
     if(strLength(fldId) > maxLenth){
        alert(error_msg);
        fldId.focus();
        return true;
     }
     else{
        return false;
     }
}

/******************************************
* 함수명    : isOutOfRange
* 기능      : 문자열 길이제한                 
* parameter : fldId, min, max

function isOutOfRange(fldId, min, max, langKind, error_msg)
{   
     if(strLength(fldId) < min || strLength(fldId) > max)
        return false;
     else
        return true;
}
*******************************************/

function menuClick(idx)
{	
    var objTRSUB;
    	
	if(parseInt(idx) < 1) 
	    idx = 1;
	
	for(i = 1; i < 30; i++)
	{
	    objTRSUB = document.getElementById("TRSUB_" + i);
		
		if(objTRSUB != null)
		{	
			if(idx == i)
				objTRSUB.style.display = "block";
			else
				objTRSUB.style.display = "none";
		}
		else
			return;
	}
}

// 중분류 메뉴에 마우스가 올려져 있을때(mouseover) 처리되는 메소드
function hoverMiddleMenu(obj)
{
    obj.style.backgroundColor = "#f0f0f0";
}

// 중분류 메뉴에서 마우스가 나갈때(mouseout) 처리되는 메소드
function unhoverMiddleMenu(obj)
{
    obj.style.backgroundColor = "#ffffff";
}

// 소분류 메뉴에 마우스가 올려져 있을때(mouseover) 처리되는 메소드
function hoverBottomMenu(obj)
{
    obj.style.backgroundColor = "#f0f0f0";
}

// 소분류 메뉴에서 마우스가 나갈때(mouseout) 처리되는 메소드
function unhoverBottomMenu(obj)
{
    obj.style.backgroundColor = "#ffffff";
}


// 현재창에서 클릭한 항목의 값을 Opener에 포함된 TextBox(Text)에 바인딩 처리하는 함수
// openerTextControlID, val => 사용자에게 보여주기 위한 파라미터(컨트롤ID와 값)
// openerKeyControlID, key  => 내부적으로 사용하기 위한 파라미터(컨트롤ID와 값)
function setTextValOfOpener(self, opener, openerTextControlID, val, openerKeyControlID, key)
{

    if((opener != null) && (opener != undefined))
    {
        if((openerTextControlID != null) && (openerTextControlID != undefined))
        {
            opener.document.getElementById(openerTextControlID).value = val;
        }
        
        if((openerKeyControlID != null) && (openerKeyControlID != undefined))
        {
            opener.document.getElementById(openerKeyControlID).value = key;
        }
                
        opener.focus();
        self.close();
    }
    else
    {
        alert('잘못된 경로로 접근하셨습니다.');
        self.close();
    }
}

// text에 보여지는 값이랑 내부적으로 key로 사용해야 하는 값이 동일할 경우 사용.
// 그렇지 않고, 서로 달라야 할 경우, setTextValOfOpener() 사용
function setTextValOfOpener1(self, opener, opener_textbox_id, opener_textbox_id_val, opener_textbox_nm, opener_textbox_nm_val)
{
    if((opener != null) && (opener != undefined))
    {
        if((opener_textbox_id != null) && (opener_textbox_id != undefined))
            opener.document.getElementById(opener_textbox_id).value = opener_textbox_id_val;
         
        //스크립트로 넘어오는 opener_textbox_nm_val 중에 ' 를 포함하는 텍스트는
        //정상적으로 동작하지 않으므로 임의로 §로 변경 후 사용자에게 보여줄 때 다시 ' 로 변경하여 보여줌
        if((opener_textbox_nm != null) && (opener_textbox_nm != undefined))
            opener.document.getElementById(opener_textbox_nm).value = opener_textbox_nm_val.replace('§', '\'');
         
        //opener.__doPostBack('ctl00$ContentPlaceHolderMain$btnSelect', '');

        opener.focus();
        self.close();
    }
    else
    {
        alert('잘못된 경로로 접근하셨습니다.');
        self.close();
    }
}


function openAddrSearch(url, zip1, zip2, addr1, addr2)
{
	var w = "520";
	var h = "400";	
	var opt = "scrollbars=no"
	var urlpath = commPath;
	
	urlpath += url
	urlpath += "?zip1="  + zip1.name;
	urlpath += "&zip2="  + zip2.name;
	urlpath += "&addr1=" + addr1.name;
	urlpath += "&addr2=" + addr2.name;
	
	openPopCenter(urlpath, "Addr_Search", w, h, opt); 	
}

function openPopWindow(url, name, width, height)
{
    var left = (screen.width - width)/2;
	var top = (screen.height - height)/2;		
	
    //	w = "700";
    //	h = "248";
    // url = "./Content_InUp_Pop.aspx?mode=" + gubun + "&seq=" + seq;
	// popName = "CourseCode";			
	
	var Win = window.open(url, name, "width=" + width + ",height=" + height + ",left=" + left + ",top=" + top + ",status=no,directories=no,location=no,menubar=no,scrollbars=yes,resizable=1");
	
	Win.focus();
}

function OpenPopFixedWindow(url, name, width, height)
{
    var left = (screen.width - width)/2;
	var top = (screen.height - height)/2;				
	
	var Win = window.open(url, name, "width=" + width + ",height=" + height + ",left=" + left + ",top=" + top + ",status=no,directories=no,location=no,menubar=no,scrollbars=yes,resizable=0");
	
	Win.focus();
}


// 현재창에서 클릭한 항목의 값을 Opener에 포함된 TextBox(Text)에 바인딩 처리하는 함수
// openerTextControlID, val => 사용자에게 보여주기 위한 파라미터(컨트롤ID와 값)
// openerKeyControlID, key  => 내부적으로 사용하기 위한 파라미터(컨트롤ID와 값)
function setTextValOfOpener(self, opener, openerTextControlID, val, openerKeyControlID, key)
{

    if((opener != null) && (opener != undefined))
    {
        if((openerTextControlID != null) && (openerTextControlID != undefined))
        {
            opener.document.getElementById(openerTextControlID).value = val;
        }

        if((openerKeyControlID != null) && (openerKeyControlID != undefined))
        {
            opener.document.getElementById(openerKeyControlID).value = key;
        }
                
        opener.focus();
        self.close();
    }
    else
    {
        alert('잘못된 경로로 접근하셨습니다.');
        self.close();
    }
}


function flash(c,d,e) {
    var flash_tag = "";
    flash_tag = '<OBJECT classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" ';
    flash_tag +='codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0" ';
    flash_tag +='WIDTH="'+c+'" HEIGHT="'+d+'" >';
    flash_tag +='<param name="movie" value="'+e+'">';
    flash_tag +='<param name="quality" value="high">';
    flash_tag +='<param name="wMode" value="transparent">';
    flash_tag +='<embed src="'+e+'" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" ';
    flash_tag +='type="application/x-shockwave-flash" wMode="Transparent" WIDTH="'+c+'" HEIGHT="'+d+'"></embed></object>'

    document.write(flash_tag);

}

function keyConfirm(obj, message)
{
  if(obj.value.trim() == "")
  {
	alert(message);
	return false;
  }			
}

function getFileNM(controlID, filepath)
{
  
  var xControl = document.getElementById(controlID);
  var xFileNameSector = filepath.split("\\");
  var xFileName = xFileNameSector[xFileNameSector.length - 1];
  xControl.value = xFileName;
}


function topMenuMouseOverEvt(obj, menucode, subMenuStr, subMenuUrl, subPosLeft)
{
    //alert(menucode); alert(subMenuStr); alert(subMenuUrl);
    obj.style.color = "#99e7e9"; 
	obj.style.fontWeight = "bold";
	obj.style.cursor = "pointer";
	
	var menucode1 = menucode.substr(0,1);
	//var menucode2 = menucode.substr(1,1);
	//var menucode3 = menucode.substr(2,1);
	
	var submenu = document.getElementById("submenu");
	submenu.style.left = subPosLeft;
	
	var arr = subMenuStr.split('凸'); // 메뉴명
	var arr1 = subMenuUrl.split('凸'); // 메뉴에 따른 url
	
	submenu.innerHTML = "";
	for(var i=0; i<arr.length; i++)
	{
        if(i == arr.length-1)
	        submenu.innerHTML += "<span class=\"submenu\" onmouseover=\"subMenuMouseOverEvt(this);\" onmouseout=\"subMenuMouseOutEvt(this);\" onclick=\"saveMenuCD('" + menucode1 + "', '" + (i+1) + "', '0', '" + arr1[i] + "');\">" + arr[i] + "</span>";
	    else
	        submenu.innerHTML += "<span class=\"submenu\" onmouseover=\"subMenuMouseOverEvt(this);\" onmouseout=\"subMenuMouseOutEvt(this);\" onclick=\"saveMenuCD('" + menucode1 + "', '" + (i+1) + "', '0', '" + arr1[i] + "');\">" + arr[i] + "</span>&nbsp;|&nbsp;";
	     
	}
}

 
// 대분류 메뉴(상단) MouseOver 이벤트 발생 시 처리
//function topMenuMouseOverEvt(obj, subMenuStr, subMenuUrl, subPosLeft)
//{  
//	obj.style.color = "#99e7e9"; 
//	obj.style.fontWeight = "bold";
//	obj.style.cursor = "pointer";
//	
//	var submenu = document.getElementById("submenu");
//	submenu.style.left = subPosLeft;
//	
//	var arr = subMenuStr.split('凸');
//	var arr1 = subMenuUrl.split('凸');
//	
//	submenu.innerHTML = "";
//	for(var i=0; i<arr.length; i++)
//	{
//	    if(i == arr.length-1)
//	        submenu.innerHTML += "<a href='" + arr1[i] + "'><span class='submenu' onmouseover='subMenuMouseOverEvt(this);' onmouseout='subMenuMouseOutEvt(this);'>" + arr[i] + "</span></a>";
//	    else
//	        submenu.innerHTML += "<a href='" + arr1[i] + "'><span class='submenu' onmouseover='subMenuMouseOverEvt(this);' onmouseout='subMenuMouseOutEvt(this);'>" + arr[i] + "</span></a>&nbsp;|&nbsp;";
//	    
//	}
//}

// 대분류 메뉴(상단) MouseOut 이벤트 발생 시 처리
function topMenuMouseOutEvt(obj)
{
	obj.style.color = "#ffffff"
	obj.style.fontWeight = "bold";
}

// 중분류 메뉴(상단) MouseOver 이벤트 발생 시 처리
function subMenuMouseOverEvt(obj)
{
	obj.style.color = "#F6931C";
	obj.style.fontWeight = "bold";
	obj.style.cursor = "pointer";
}

// 중분류 메뉴(상단) MouseOut 이벤트 발생 시 처리
function subMenuMouseOutEvt(obj)
{
    obj.style.color = "#000000";
	obj.style.fontWeight = "normal";
}

function show_menu1(sec) 
{
	document.all.show_menu1.style.visibility=sec;
}

function MM_swapImgRestore() 
{ //v3.0
  var i,x,a=document.MM_sr; 
  for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) 
  { 
    x.src=x.oSrc;
  }
}

function MM_swapImage() 
{ //v3.0
   var i,j=0,x,a=MM_swapImage.arguments; 
   document.MM_sr=new Array; 
   
   for(i=0;i<(a.length-2);i+=3)
   {
       if ((x=MM_findObj(a[i]))!=null)
       {
         document.MM_sr[j++]=x; 
         if(!x.oSrc) 
           x.oSrc=x.src; x.src=a[i+2];
       }
   }
}

function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; 
  
  if((p=n.indexOf("?"))>0&&parent.frames.length) 
  {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);
  }
  
  if(!(x=d[n])&&d.all) 
    x=d.all[n]; 
  
  for (i=0;!x&&i<d.forms.length;i++)
  {
    x=d.forms[i][n];
  }
  
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) 
  { 
    x=MM_findObj(n,d.layers[i].document);
  }
  
  if(!x && d.getElementById) x=d.getElementById(n); 
    return x;
}

 // 체크박스 전체선택/해제 
 function CheckAll(checkAllBox, chkName)                         
 {   
  var frm = document.forms[0]                         
  var ChkState = checkAllBox.checked;
  for(i=0;i< frm.length;i++)                         
  {   
   e = frm.elements[i];   
   if(e.type=='checkbox' && e.name.indexOf(chkName) != -1)
    e.checked= ChkState ;                       
  }
 } 
 
 
 
 
 
     /*********************************
    *  기능 :  입력된 비밀번호 체크  *
    *  parameter : pass1, pass1  *
    **********************************/
    function isPassChk(pass1, pass2, error_msg)
    {   
         var passValue1 = pass1.value.replace(/\s/gi, "");
         var passValue2 = pass2.value.replace(/\s/gi, "");
                  
         if(error_msg == "") 
         {
              if(passValue1 != passValue2)
                  return true;              
              else
                  return false;
         } 
         else 
         {
              if(passValue1 != passValue2)
              {
                   alert(error_msg);
                   pass1.focus();
                   return true;
              }
              else
              {      
                   return false;
              }
         }
    }
    
        
    
    /*********************************
    *  기능 :  입력된 전화번호 체크  *
    *  parameter : phone  *
    **********************************/
	function fnPhoneChk(phone, error_msg)
	{    
	     var valid_reg = /^[0-9]{1,}\-[0-9]{1,}\-[0-9]{1,}$/ 
	     
         if ( !valid_reg.test( phone.value ) ) 
         { 
          alert(error_msg); 
          phone.focus();
          return true; 
         } 

         return false; 
	}
		
	
    /*********************************
    *  기능 :  입력된 E-Mail 체크  *
    *  parameter : email  *
    *  체크사항 
     - @가 2개이상일 경우 
     - .이 붙어서 나오는 경우 
     -  @.나  .@이 존재하는 경우 
     - 맨처음이.인 경우 
     - @이전에 하나이상의 문자가 있어야 함 
     - @가 하나있어야 함 
     - Domain명에 .이 하나 이상 있어야 함 
     - Domain명의 마지막 문자는 영문자 2~4개이어야 함    
    **********************************/
    function CheckMail(strMail, error_msg) 
    { 
        var check1 = /(@.*@)|(\.\.)|(@\.)|(\.@)|(^\.)/;  

        var check2 = /^[a-zA-Z0-9\-\.\_]+\@[a-zA-Z0-9\-\.]+\.([a-zA-Z]{2,4})$/; 
         
        if ( !check1.test(strMail.value) && check2.test(strMail.value) ) 
        { 
            return false; 
        } 
        else 
        { 
            alert(error_msg); 
            return true; 
        } 
    }

    /*********************************
    *  기능 :  입력된 ID (영문) 체크  *
    *  parameter : str  *
    **********************************/    
    function CheckChar2(str, error_msg)
    {  
        strarr = new Array(str.value.length); 
        var flag = true; 
        for (i=0; i<str.value.length; i++) 
        { 
            strarr[i] = str.value.charAt(i) 
            if (!((strarr[i]>='a')&&(strarr[i]<='z'))) 
            {
                alert(error_msg);  
                flag = true; 
            } 
        } 
        if (flag) 
        { 
            return false; 
        }
        else 
        { 
            alert(error_msg);
            return true; 
        } 
    }      	
    
    
    /*********************************
    *  기능 :  isSelect 선택된값이 * 이면 메세지 출력  *
    *  parameter : fldId, error_msg  *
    **********************************/
    function isSelect(fldId, error_msg)
    {   
         var fldValue = fldId.value;
         
         if(error_msg == "") 
         {
              if(fldValue == "*")
                  return true;              
              else
                  return false;
         } 
         else 
         {
              if(fldValue == "*")
              {
                   alert(error_msg);
                   fldId.focus() ;
                   return true;
              }
              else
              {      
                   return false;
              }
         }
    }
    
/*********************************
*  기능 :  메뉴세팅하고 Url 이동
*  parameter : 	menu1 : 대메뉴, menu2 : 중메뉴, menu3 : 소메뉴 goUrl : 메뉴관련 URL
**********************************/
function menuUrl(menu1, menu2, menu3, gourl){		
    saveMenuCD(menu1, menu2, menu3, gourl);
}    
