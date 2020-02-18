using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;
using System.Data;

namespace CLT.WEB.UI.FX.UTIL
{
    public class WebControlHelper
    {
        /**************************************************
	     * ComboType:			Combo Type 정의 
	     *							NullAble - Null 값이 들어가는 Combo
	     *							NotNullAble - Null 값이 들어가지 않는 Combo
	     *							All - * 가 들어가는 Combo 
	     **************************************************/
        public enum ComboType
        {
            All,
            NotNullAble,
            NullAble
        }

        #region DropDownList

        /************************************************************
        * Function name : SetSelectItem_DropDownList
        * Purpose       : DropDownList의 특정 Item이 선택되게 하는 처리
        * Input         : DropDownList rDropDownList, string rVal
        * Output        : void
        *************************************************************/
        public static void SetSelectItem_DropDownList(DropDownList rDropDownList, string rVal)
        {
            for (int i = 0; i < rDropDownList.Items.Count; i++)
            {
                if (rDropDownList.Items[i].Value.ToUpper() == rVal.ToUpper())
                    rDropDownList.Items[i].Selected = true;
                else
                    rDropDownList.Items[i].Selected = false;
            }
        }

        /************************************************************
        * Function name : SetSelectText_DropDownList
        * Purpose       : DropDownList의 특정 Item이 선택되게 하는 처리
        * Input         : DropDownList rDropDownList, string rVal
        * Output        : void
        *************************************************************/
        public static void SetSelectText_DropDownList(DropDownList rDropDownList, string rVal)
        {
            for (int i = 0; i < rDropDownList.Items.Count; i++)
            {
                if (rDropDownList.Items[i].Text.ToUpper() == rVal.ToUpper())
                    rDropDownList.Items[i].Selected = true;
                else
                    rDropDownList.Items[i].Selected = false;
            }
        }

        /************************************************************
        * Function name : SetDropDownList
        * Purpose       : DropDownList에 2차원 배열(Text, Value)을 바인딩 처리
        * Input         : DropDownList rDropDownList, string[][] rItems
        * Output        : void
        *************************************************************/
        public static void SetDropDownList(DropDownList rDropDownList, string[][] rItems)
        {
            rDropDownList.Items.Clear();
            rDropDownList.Items.Add(new ListItem("*"));

            for (int i = 0; i < rItems[0].Length; i++)
            {
                rDropDownList.Items.Add(new ListItem(rItems[i][0], rItems[i][1]));
            }
        }

        /************************************************************
        * Function name : SetDropDownList
        * Purpose       : DropDownList에 2차원 배열(Text, Value)을 바인딩 처리
        * Input         : DropDownList rDropDownList, string[][] rItems
        * Output        : void
        *************************************************************/
        public static void SetDropDownList(DropDownList rDropDownList, string[,] rItems)
        {
            rDropDownList.Items.Clear();
            rDropDownList.Items.Add(new ListItem("*"));

            for (int i = 0; i < rItems.GetLength(0); i++)
            {
                rDropDownList.Items.Add(new ListItem(rItems[i,0], rItems[i,1]));
            }
        }

        /************************************************************
        * Function name : SetDropDownList
        * Purpose       : DropDownList에 DataTable을 바인딩 처리
        * Input         : DropDownList rDropDownList, DataTable rItems
        * Output        : void
        *************************************************************/
        public static void SetDropDownList(DropDownList rDropDownList, DataTable rItems, ComboType rType)
        {
            rDropDownList.Items.Clear();
            switch (rType)
            {
                case ComboType.All:
                    rDropDownList.Items.Add(new ListItem("*"));
                    break;
                case ComboType.NullAble:
                    rDropDownList.Items.Add(new ListItem(""));
                    break;
            }

            for (int i = 0; i < rItems.Rows.Count; i++)
            {
                rDropDownList.Items.Add(new ListItem(rItems.Rows[i]["d_knm"].ToString(), rItems.Rows[i]["d_cd"].ToString()));
            }
        }

        /************************************************************
        * Function name : SetDropDownList
        * Purpose       : DropDownList에 DataTable을 바인딩 처리
        * Input         : DropDownList rDropDownList, DataTable rItems
        * Output        : void
        *************************************************************/
        public static void SetDropDownList(DropDownList rDropDownList, DataTable rItems)
        {
            SetDropDownList(rDropDownList, rItems, ComboType.All);
        }

        /************************************************************
       * Function name : SetDropDownList
       * Purpose       : DropDownList에 DataTable을 바인딩 처리
       * Input         : DropDownList rDropDownList, DataTable rItems, string rText, string rValue
       * Output        : void
       *************************************************************/
        public static void SetDropDownList(DropDownList rDropDownList, DataTable rItems, string rText, string rValue)
        {
            SetDropDownList(rDropDownList, rItems, rText, rValue, ComboType.All);
        }

        /************************************************************
       * Function name : SetDropDownList
       * Purpose       : DropDownList에 DataTable을 바인딩 처리
       * Input         : DropDownList rDropDownList, DataTable rItems, string rText, string rValue, ComboType rType
       * Output        : void
       *************************************************************/
        public static void SetDropDownList(DropDownList rDropDownList, DataTable rItems, string rText, string rValue, ComboType rType)
        {
            rDropDownList.Items.Clear();
            switch (rType)
            {
                case ComboType.All:
                    rDropDownList.Items.Add(new ListItem("*"));
                    break;
                case ComboType.NullAble:
                    rDropDownList.Items.Add(new ListItem(""));
                    break;
            }

            for (int i = 0; i < rItems.Rows.Count; i++)
            {
                rDropDownList.Items.Add(new ListItem(rItems.Rows[i][rText].ToString(), rItems.Rows[i][rValue].ToString()));
            }
        }

        /************************************************************
        * Function name : SetListBox
        * Purpose       : ListBox 공통코드용 DataTable을 바인딩 처리
        * Input         : ListBox rListBox, DataTable rItems
        * Output        : void
        *************************************************************/
        public static void SetListBox(ListBox rListBox, DataTable rItems)
        {

            rListBox.Items.Clear();
            rListBox.Items.Add(new ListItem("*"));

            for (int i=0; i < rItems.Rows.Count; i++)
            {
                rListBox.Items.Add(new ListItem(rItems.Rows[i]["d_knm"].ToString(), rItems.Rows[i]["d_cd"].ToString()));
            }
        }

        /************************************************************
        * Function name : SetListBox
        * Purpose       : ListBox 공통코드용 DataTable을 바인딩 처리
        * Input         : ListBox rListBox, DataTable rItems
        * Output        : void
        *************************************************************/
        public static void SetListBox(ListBox rListBox, DataTable rItems, bool bAll)
        {

            rListBox.Items.Clear();
            if (bAll == true)
                rListBox.Items.Add(new ListItem("*"));

            for (int i = 0; i < rItems.Rows.Count; i++)
            {
                rListBox.Items.Add(new ListItem(rItems.Rows[i]["d_knm"].ToString(), rItems.Rows[i]["d_cd"].ToString()));
            }
        }

        /************************************************************
        * Function name : SetListBox
        * Purpose       : ListBox DataTable을 바인딩 처리
        * Input         : ListBox rListBox, DataTable rItems, string rText, string rValue
        * Output        : void
        *************************************************************/
        public static void SetListBox(ListBox rListBox, DataTable rItems, string rText, string rValue)
        {

            rListBox.Items.Clear();
            rListBox.Items.Add(new ListItem("*"));

            for (int i = 0; i < rItems.Rows.Count; i++)
            {
                rListBox.Items.Add(new ListItem(rItems.Rows[i][rText].ToString(), rItems.Rows[i][rValue].ToString()));
            }
        }

        /************************************************************
        * Function name : SetListBox
        * Purpose       : ListBox DataTable을 바인딩 처리
        * Input         : ListBox rListBox, DataTable rItems, string rText, string rValue
        * Output        : void
        *************************************************************/
        public static void SetListBox(ListBox rListBox, DataTable rItems, string rText, string rValue, bool bAll)
        {

            rListBox.Items.Clear();
            if (bAll == true)
                rListBox.Items.Add(new ListItem("*"));

            for (int i = 0; i < rItems.Rows.Count; i++)
            {
                rListBox.Items.Add(new ListItem(rItems.Rows[i][rText].ToString(), rItems.Rows[i][rValue].ToString()));
            }
        }

        /************************************************************
        * Function name : SetDropDownList
        * Purpose       : DropDownList에 DataTable을 바인딩할때 지정된 Index가 선택되도록 처리
        * Input         : DropDownList rDropDownList, DataTable rItems, int rSelectedIndex
        * Output        : void
        *************************************************************/
        public static void SetDropDownList(DropDownList rDropDownList, DataTable rItems, int rSelectedIndex)
        {
            rDropDownList.Items.Clear();
            rDropDownList.Items.Add(new ListItem("*"));

            for (int i = 0; i < rItems.Rows.Count; i++)
            {
                rDropDownList.Items.Add(new ListItem(rItems.Rows[i]["d_knm"].ToString(), rItems.Rows[i]["d_cd"].ToString()));
                if (i == rSelectedIndex)
                    rDropDownList.Items[i].Selected = true;
                else
                    rDropDownList.Items[i].Selected = false;
            }
        }


        /************************************************************
        * Function name : SetDropDownList
        * Purpose       : DropDownList에 DataTable을 바인딩할때 지정된 Index가 선택되도록 처리
        * Input         : DropDownList rDropDownList, DataTable rItems, int rSelectedIndex
        * Output        : void
        *************************************************************/
        public static void SetDropDownList(DropDownList rDropDownList, DataTable rItems, int rSelectedIndex, bool rSelected)
        {
            rDropDownList.Items.Clear();
            
            if (rSelected == true)
                rDropDownList.Items.Add(new ListItem("*"));

            for (int i = 0; i < rItems.Rows.Count; i++)
            {
                rDropDownList.Items.Add(new ListItem(rItems.Rows[i]["d_knm"].ToString(), rItems.Rows[i]["d_cd"].ToString()));
                if (i == rSelectedIndex)
                    rDropDownList.Items[i].Selected = true;
                else
                    rDropDownList.Items[i].Selected = false;
            }
        }

        /************************************************************
        * Function name : SetDropDownList
        * Purpose       : DropDownList에 DataTable을 바인딩 처리
        * Input         : DropDownList rDropDownList, DataTable rItems
        * Output        : void
        *************************************************************/
        public static void SetYearDropDownList(DropDownList rDropDownList)
        {
            rDropDownList.Items.Clear();
            rDropDownList.Items.Add(new ListItem("*"));

            int xPrevYear = DateTime.Now.AddYears(-12).Year;
            int xNextYear = DateTime.Now.AddYears(2).Year;

            for (int i = xPrevYear; i <= xNextYear; i++)
            {
                rDropDownList.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        /************************************************************
        * Function name : SetDropDownList
        * Purpose       : DropDownList에 DataTable을 바인딩 처리
        * Input         : DropDownList rDropDownList, DataTable rItems
        * Output        : void
        *************************************************************/
        public static void SetYearDropDownList(DropDownList rDropDownList, ComboType rType)
        {
            rDropDownList.Items.Clear();

            //rDropDownList.Items.Add(new ListItem("*"));
            switch (rType)
            {
                case ComboType.All:
                    rDropDownList.Items.Add(new ListItem("*"));
                    break;
                case ComboType.NullAble:
                    rDropDownList.Items.Add(new ListItem(""));
                    break;
            }

            int xPrevYear = DateTime.Now.AddYears(-12).Year;
            int xNextYear = DateTime.Now.AddYears(5).Year;

            for (int i = xPrevYear; i <= xNextYear; i++)
            {
                rDropDownList.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }


        #endregion
    }
}
