// JScript 파일
//C.Date : Create Date
//U.Date : Update Date

var commPath = "/test/Common/";
var codeMasterPath = "/test/Manager/CodeManager/";

/////////////////////////////////////////////////
//C.Date : 2007-03-14
//C.User : BHJ
//Action : form Reset
//Use As : onClick = "javascript:execReset();"
/////////////////////////////////////////////////
function execReset()
{
    //document.getElementById("<%= form1.ClientID %>").reset();
    aspnetForm.reset();
}

////////////////////////////////////////////////////////////////////////////////////////////////////////
//C.Date : 2007-03-07
//C.User : BHJ
//Action : Row Add OR Del
//param  : objElements - Event Object
//         viewCnt     - Action Count
//         flag        - true(visible) | false(hidden)
//Use As : onClick="javascript:toggleView(document.all.[Ctrl ID].getElementsByTagName([TagName]), 1, true|false)"
//   Ex) : onClick="javascript:toggleView(document.all.fileViewTable.getElementsByTagName('TR'), 1, true)"
// Alert : Already Defined : style="display:<none|block>" In objRow
////////////////////////////////////////////////////////////////////////////////////////////////////////
function toggleView(objElements, viewCnt, flag)
{
    var cnt = 0;
    var chkCnt = 0;
    var i, j;
    
	for(i = 0; i < objElements.length; i++)
	{          
		if(flag == true) //Add row
		{
			if(objElements[i].style.display == "none")
			{
				objElements[i].style.display = "block";
				cnt++;       
			}
		}
		else //Del row
		{
			//If one row remain, do not delete row.
			for(j = 0; j < objElements.length; j++)
			{
				if(objElements[j].style.display == "block")
				{
					chkCnt++;
				}
			}
			
			if(chkCnt == 1)
			{
				cnt = viewCnt;
			}
			else
			{
				if(objElements[i].style.display == "block")
				{
					objElements[i].style.display = "none";                                                                                                                                                                                                                                                                                                                                                                                                                                
					cnt++;
				}
			}
		}                
        
		//지시된 수 만큼 증가 또는 감소 시켰는지 체크하는 함수
		if(cnt == viewCnt)
		{
			return false;
		}
	}
	
}

///////////////////////////////////////////////////////////////
//C.Date : 2007-03-13
//C.User : BHJ
// Action  : 체크박스 전체 선택/해제
// param  : checkingCtrl - onClick작업이 일어나는 checkbox 객체
//사용예 : onClick = "javascript:checkAll(this);"
///////////////////////////////////////////////////////////////
function checkAll(checkingCtrl){

	var oItem = checkingCtrl.children;
	var theBox= (checkingCtrl.type == "checkbox") ? checkingCtrl : checkingCtrl.children.item[0];
	
	xState = theBox.checked;
	elm = theBox.form.elements;

	for(i = 0; i < elm.length; i++)
	{
		if(elm[i].type == "checkbox" && elm[i].id != theBox.id)
		{ 
			if(elm[i].checked != xState) elm[i].click();		       
		}
	}
}

/////////////////////////////////////////////////////////////////////////////
//C.Date : 2007-03-13
//C.User : BHJ
// Action  : 지정된 컨트롤의 disply 스타일 속성을 설정함.
// param  : true | false - 지정값, objID - display 스타일 설정할 컨트롤 객체
//사용   : 
//사용예 : onClick = "javascript:toggleViewCtrl([true|false], [id명]);"
//주의   : 
/////////////////////////////////////////////////////////////////////////////
function toggleViewCtrl(flag, objID)
{
	if(flag)
	{
		document.getElementById(objID).style.display = "block";
	}
	else
	{
		document.getElementById(objID).style.display = "none";
	}
}

/////////////////////////////////////////////////////////////////////////////
//C.Date : 2007-03-13
//C.User : BHJ
// Action  : 지정된 컨트롤의 disabled 설정
// param  : flag : true | false - 지정값, objID - disabled 설정할 컨트롤 객체
//사용   : 
//사용예 : onClick = "javascript:toggleEnableCtrl([true|false], [id명]);"
//주의   : 
/////////////////////////////////////////////////////////////////////////////
function toggleEnableCtrl(flag, objID)
{
	document.getElementById(objID).disabled = flag;
}


////////////////////////////////////////////////////////////////////
//C.Date : 2007-03-14
//C.User : BHJ
// Action  : 지정된 컨트롤의 byte 체크
// param  : ev_obj - 이벤트 발생 객체, maxLen - 허용바이트 수
//사용   : 
//사용예 : onKeyDown = "javascript:lenChk(this, [허용 바이트수 ]);"
//주의   : 
////////////////////////////////////////////////////////////////////
function lenChk(ev_obj, maxLen)
{
    var strByte = (ev_obj.value.length+(escape(ev_obj.value)+"%u").match(/%u/g).length-1);

    if(strByte > maxLen)
    {        
        ev_obj.value = ev_obj.value.substr(0, maxLen);
    }
}


//////////////////////////////////////////
//C.Date : 2007-03-14
//C.User : BHJ
//Action : 공백제거함수
//사용예 : [입력컨트롤이름].value.trim();
//////////////////////////////////////////
String.prototype.trim = function(){
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

//////////////////////////////////////////
//C.Date : 2007-04-11
//C.User : BHJ
//Action : change str1 -> str2
//사용예 : [ctrlName].value.replaceAll("+", "-");
//////////////////////////////////////////
String.prototype.replaceAll = function(str1, str2) {
    var temp_str = "";
    if (this.trim() != "" && str1 != str2) {
        temp_str = this.trim();
        while (temp_str.indexOf(str1) > -1){
            temp_str = temp_str.replace(str1, str2);
        }
    }    
    return temp_str;
}

//////////////////////////////////////////
//C.Date : 2007-03-210
//C.User : BHJ
// Action  : SELECT 태그 동적으로 값 설정
// param  : selName       :: selectBox이름
//		   separation    :: 구분자 예) '|', ',', ':', ';' 등...
//		   selData       :: 표시되는 값
//		   dataVal       :: 실제 value
//		   setSelDataVal :: 기본으로 선택될 값
//적용예제 : //Research//Research_InUp.aspx 등.
//////////////////////////////////////////
function setSelData(selName, separation, selData, dataVal, setSelDataVal)
{
	var arrSelData = selData.split(separation);
	var arrDataVal = dataVal.split(separation);

	for(var i = 0; i < arrSelData.length; i++) //해당 채널의 서브 카테고리 배열 길이만큼 반복
	{
		selName.options[i] = new Option(arrSelData[i], arrDataVal[i]); //카테고리에 해당하는 콤보박스 값을 채움 :: new Option(표시되는 값, 실제 Value)

		//기본으로 선택되어질 초기 값 설정가능
		if(arrDataVal[i] == setSelDataVal) selName.selectedIndex = i;
	}
}

//////////////////////////////////////////////////////////////
//C.Date : 2007-03-22
//C.User : BHJ
// Action  : 지정된 숫자만큼 TR의 수를 생성하고 삭제한다.
// param  : tableName - TR생성하고자하는 테이블명
//         creRowCnt - 생성하고자하는 TR갯수		
//         tdVal     - 생성하고자하는 TD의 값을 담고있는 배열
//         Label     - '질문/보기 등 TD에 들어갈 Label 값
//Remark : ~/Research/Research_InUp.aspx 페이지 참고.
//////////////////////////////////////////////////////////////
function addRow(tableName, creRowCnt, tdVal, Label)		
{	
    var objTB    = document.getElementById(tableName);  // 테이블지정		    
    var trCnt    = objTB.rows.length;				    // 테이블 현재 TR의 개수
    var objTBody = objTB.childNodes[0];				    // table의 첫번째 차일드 즉 tbody를 지정한다.
   
	var row, cell;
			   
    var i, j;
    
	for(i = 0; i < trCnt; i++)
    {
		objTB.rows[i].style.display = "block";
    }
    
    if(creRowCnt > trCnt) //Row  추가
    {	
		for(i = trCnt; i < creRowCnt; i++)
		{	
			row = document.createElement("TR");  // TR을 하나 생성한다.				
			objTBody.appendChild(row);			 // TBODY에 자식노드를 하나 추가한다.						
		    
			cell = document.createElement("TD");//TD를 생성한다.
			row.appendChild(cell);              //TR에 TD를 하나 추가한다.
			cell.innerHTML = Label + " " + (i + 1);
			cell.style.width = "50";			
			cell.style.textAlign = "center";
			
			for(j = 0; j < tdVal.length; j++)
			{					
				cell = document.createElement("TD");//TD를 생성한다.
				row.appendChild(cell);              //TR에 TD를 하나 추가한다.
				cell.innerHTML = tdVal[j];				
			}			
		}
	}
	else if( creRowCnt < trCnt)//Row 삭제
	{				
		var rowIdx = trCnt;
			
		while(rowIdx > creRowCnt)
		{		
			//objTB.deleteRow(--rowIdx);					
			objTB.rows[--rowIdx].style.display = "none";
		}
	}
}

//////////////////////////////////////////////////////////////
//C.Date : 2007-03-22
//C.User : BHJ
//Action : Get idx of event object array
//param  : elements - array of control
//         objEvent - raise event control object
//Remark : this function used [~/Research/Research_InUp.aspx ]
//////////////////////////////////////////////////////////////
function getIdx(elements, objEvent) 
{
	var idx = 0;
	
	for(var j = 0; j < elements.length;j++){
				
		if(elements[j].uniqueID == objEvent.uniqueID){
			
			idx = j;
			
			return idx;
		}
	}
}	

//////////////////////////////////////////////////////////////
//C.Date : 2007-03-22
//C.User : BHJ
//Action : do not Submit when textbox press Enter key
//Remark : action when onKeyPress
//         ex)onkeypress = "return doNotSubmit()"
//////////////////////////////////////////////////////////////
function doNotSubmit()
{	
	if(event.keyCode == 13) return false;
}

/////////////////////////////////////////
//Action : Setting Date format
//remark : align - Left 
//         Number only
//          ex : onkeyup=ChkDate(this);
//MaxLength    : 10
//First Number : 1,2
//Format       : 2006.04.05
/////////////////////////////////////////
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

////////////////////////////////
//Action : Open Calendar
////////////////////////////////
function Cal_YMD(){

	strleft = "dialogleft:" + eval(window.screenLeft + window.event.clientX - 50);
	strtop = ";dialogtop:" + eval(window.screenTop + window.event.clientY + 10);
	
	if(strleft >= 730) {
		strleft = "dialogleft:730";
	}

	if(strtop >= 510) {
		strtop = ";dialogtop:510";
	}
	return window.showModalDialog(commPath + "Calendar.aspx","",strleft +  strtop +";dialogWidth:208px; dialogHeight:240px;scroll:0;status:0;resizable:0;help:0;titlebar:0;");
	//window.open(calPath + "Calendar.aspx","",strleft +  strtop +";dialogWidth:233px; dialogHeight:280px;scroll:no;status:no;titlebar:no;");
}

//null check
function is_null(item_var) {

	if(item_var == "" || item_var == null || item_var == 'undefined' || item_var == " ")
	{
		return true;
	}
	return false;
}
/////////////////////////////////////////////////
//End of Open Calendar Window
/////////////////////////////////////////////////

/////////////////////////////////////////////////
//C.Date : 2007-04-09
//C.User : BHJ
//Action : Input number only
// ex)onKeyUp = CheckInput('both');
//    gubun : both -> Check alpabat and number
//            num  -> Number only
/////////////////////////////////////////////////
function CheckInput(gubun)
{
    var objEv = event.srcElement;

    var reg;
    if(gubun == "both")
    {
		reg = /[^A-Za-z0-9]/;
		
    }
    else if(gubun == "num")
    {    
        reg  = /[^0-9]/;
    }
    
    var strValue = objEv.value;
    var str      = "";

    if(strValue.match(reg)){        
    
        for(var i = 0 ; i <strValue.length;i++ ){
            
            str += strValue.charAt(i).replace(reg, "");
            
        }

        objEv.value = str;
        objEv.focus();
    }
}

/////////////////////////////////////////////////
//C.Date : 2007-04-10
//C.User : BHJ
//Action : Open search address window
//param  : Each controls' Name value
/////////////////////////////////////////////////
function openAddrSearch(zip1, zip2, addr1, addr2)
{
	var url = commPath;
	url += "Addr_Search.aspx";
	url += "?zip1="  + zip1.name;
	url += "&zip2="  + zip2.name;
	url += "&addr1=" + addr1.name;
	url += "&addr2=" + addr2.name;
	
	window.open(url, "Addr_Search", "width=430,height=280,left=430,top=220,scrollbars=yes");
}

/////////////////////////////////////////////////
//C.Date : 2007-04-10
//C.User : BHJ
//Action : Email validation check
/////////////////////////////////////////////////
function CheckEmail(obj){
	
	 var reg = /^((\w|[\-\.])+)@((\w|[\-\.])+)\.([A-Za-z]+)$/;
	 
	 if (obj.search(reg) != -1) {
		return true;
     }
     
     return false;
}

/////////////////////////////////////////////////
//C.Date : 2007-04-10
//C.User : BHJ
//Action : SSN validation check
/////////////////////////////////////////////////
function CheckJumin(obj1, obj2) {
 
	var tmp1,tmp2;
	var t1,t2,t3,t4,t5,t6,t7;
	var ok = true;

	var val1 = obj1.value;
	var val2 = obj2.value;
	
	tmp1 = val1.substring(2,4);
	tmp2 = val1.substring(4);

	if ((tmp1 < "01") || (tmp1 > "12")) return false;
	if ((tmp2 < "01") || (tmp2 > "31")) return false;
		
	t1 = val1.substring(0,1);
	t2 = val1.substring(1,2);
	t3 = val1.substring(2,3);
	t4 = val1.substring(3,4);
	t5 = val1.substring(4,5);
	t6 = val1.substring(5,6);

	t11 = val2.substring(0,1);
	t12 = val2.substring(1,2);
	t13 = val2.substring(2,3);
	t14 = val2.substring(3,4);
	t15 = val2.substring(4,5);
	t16 = val2.substring(5,6);
	t17 = val2.substring(6,7);

	var tot = t1*2 + t2*3 + t3*4 + t4*5 + t5*6 + t6*7;
	tot += t11*8 + t12*9 + t13*2 + t14*3 + t15*4 + t16*5;

	var result = tot%11;
	result = (11 - result)%10;

	if (result != t17) 
	{	
		return false;
	}  
	
    return true;
}

/////////////////////////////////////////////////
// C.Date : 2007-04-13
// C.User : BHJ
// Action : Check Company no.
// return : boolean
// ex) 111111-1111111
/////////////////////////////////////////////////
function CheckCompNo(resno){

    fmt = /^d{6}-d{7}$/;
    
    if(!fmt.test(resno)) return false;
    
    buf = new Array(13);
    
    for (i = 0; i < 6; i++)
    {
		buf[i] = parseInt(resno.charAt(i));
	}
	
    for (i = 6; i < 13; i++)
    {
		buf[i] = parseInt(resno.charAt(i + 1));
	}
 
    multipliers = [1,2,1,2,1,2,1,2,1,2,1,2];
    
    for (i = 0, sum = 0; i < 12; i++)
    {
		sum += (buf[i] *= multipliers[i]);
	}
    
    if(10 - sum.toString().substring(sum.toString().length*1 - 1,sum.toString().length*1)*1 != buf[12])
    {
		return false;
	}
 
    return true;
}

//////////////////////////////////////////////////////////////
//C.Date : 2007-03-27
//C.User : L.J.Y
//Action : Common design module
//////////////////////////////////////////////////////////////
function MM_reloadPage(init) {  //reloads the window if Nav4 resized
  if (init==true) with (navigator) {if ((appName=="Netscape")&&(parseInt(appVersion)==4)) {
    document.MM_pgW=innerWidth; document.MM_pgH=innerHeight; onresize=MM_reloadPage; }}
  else if (innerWidth!=document.MM_pgW || innerHeight!=document.MM_pgH) location.reload();
}
MM_reloadPage(true);

function MM_preloadImages() { //v3.0
  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
    var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
    if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}

function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}

function MM_showHideLayers() { //v6.0
  var i,p,v,obj,args=MM_showHideLayers.arguments;
  for (i=0; i<(args.length-2); i+=3) if ((obj=MM_findObj(args[i]))!=null) { v=args[i+2];
    if (obj.style) { obj=obj.style; v=(v=='show')?'visible':(v=='hide')?'hidden':v; }
    obj.visibility=v; }
}

function MM_swapImgRestore() { //v3.0
  var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
}

function MM_swapImage() { //v3.0
  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
}



//Replay show/hide
function roll_tip(divid) {
	if (document.all[divid].style.display == ""){
	
		document.all[divid].style.display = "none";
		MM_swapImgRestore("tipimg" + divid.substring(3));
		rollFlag = 0;
		return;
	} 
	else 
	{
		if (rollFlag != 0) 
		{
			document.all["tip" + rollFlag].style.display = "none";
			document.all[divid].style.display = "";
			MM_swapImgRestore("tipimg" + rollFlag);
		}

		document.all[divid].style.display = "";
		MM_swapImage("tipimg" + divid.substring(3), "", "", 1);
		rollFlag = divid.substring(3);
	}
}  


// People iframe resize.
function doResize() 
{ 
	container.height = people.document.body.scrollHeight; 
	container.width = people.document.body.scrollWidth; 
} 



// Masterpage language select layer
var show = 0;
function show_menu1(sec) {
	document.all.show_menu1.style.visibility=sec;
}


/////////////////////////////////////////////////
//C.Date : 2007-04-19
//C.User : BHJ
//Action : Open search codeMaster window
//param  : Each controls' Name value
/////////////////////////////////////////////////
function openCodeMasterSearch(code, codeName)
{
	var url = codeMasterPath;
	url += "CodeManager_Pop.aspx";
	url += "?code="  + code.name;
	url += "&codeName="  + codeName.name;
	
	window.open(url, "CodeMaster_Search", "width=540,height=550,left=430,top=220,scrollbars=yes");
}