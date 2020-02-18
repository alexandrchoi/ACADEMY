using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

namespace CLT.WEB.UI.FX.UTIL
{
    public class StrTable
    {
        public StrTable()
        { }

        public static byte[] STR2BLOB(string blobString)
        {
            Byte[] result = null;

            if (blobString != null)
            {
                string[] strings = blobString.Split(new char[] { '♠' });
                result = new byte[strings.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = byte.Parse(strings[i]);
                }
            }

            return result;
        }
        public static string BLOB2STR(Byte[] bytes)
        {
            if (bytes == null) return null;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytes.Length - 1; i++)
            {
                sb.Append(bytes[i].ToString());
                sb.Append("♠");
            }

            sb.Append(bytes[bytes.Length - 1].ToString());

            return sb.ToString();
        }
        public static string MD2ArrayList2STR(ArrayList rArrayList)
        {

            if (rArrayList == null) return null;

            StringBuilder sb = new StringBuilder();
            string[] xArrCols = null;
            for (int i = 0; i < rArrayList.Count; i++)
            {
                xArrCols = (string[])rArrayList[i];
                for (int j = 0; j < xArrCols.Length; j++)
                {
                    sb.Append(xArrCols[j]);
                    sb.Append('│');
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append('┼');
            }
            if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        //object array 반환될수 있도록 추가 [2011.07.29 choon] 
        public static string MD2ArrayList2STRObj(ArrayList rArrayList)
        {

            if (rArrayList == null) return null;

            StringBuilder sb = new StringBuilder();
            object[] xArrCols = null;
            for (int i = 0; i < rArrayList.Count; i++)
            {
                xArrCols = (object[])rArrayList[i];
                for (int j = 0; j < xArrCols.Length; j++)
                {
                    sb.Append(xArrCols[j]);
                    sb.Append('│');
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append('┼');
            }
            if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }
        public static ArrayList STR2MD2ArrayList(string r2DMArrayListStr)
        {
            if (r2DMArrayListStr == null) return null;
            ArrayList xArrReturn = new ArrayList();
            string[] xArrCols = null;
            string[] xArrRows = r2DMArrayListStr.Split(new char[] { '┼' });
            for (int i = 0; i < xArrRows.Length; i++)
            {
                xArrCols = xArrRows[i].Split(new char[] { '│' });
                xArrReturn.Add(xArrCols);
            }
            return xArrReturn;
        }
        public static string MD2Array2STR(string[,] r2DMArray)
        {
            if (r2DMArray == null) return string.Empty;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < r2DMArray.GetLength(0); i++)
            {
                for (int j = 0; j < r2DMArray.GetLength(1); j++)
                {
                    sb.Append(r2DMArray[i, j]);
                    sb.Append('│');
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append('┼');
            }
            if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        public static string[,] STR2MD2Array(string r2DMArrayStr)
        {
            if (r2DMArrayStr == null || r2DMArrayStr == "") return null;
            string[,] xArrReturn = null;
            string[] xArrCols = null;
            string[] xArrRows = r2DMArrayStr.Split(new char[] { '┼' });
            for (int i = 0; i < xArrRows.Length; i++)
            {
                xArrCols = xArrRows[i].Split(new char[] { '│' });
                if (i == 0)
                {
                    xArrReturn = new string[xArrRows.Length, xArrCols.Length];
                }
                for (int j = 0; j < xArrCols.Length; j++)
                {
                    xArrReturn[i, j] = xArrCols[j];
                }
            }
            return xArrReturn;
        }
        //        public static string[,] 2DMArray2STR(string[,] r2DMArray)
        //{

        //}

        #region string --> DataTable, DataTable --> string
        /// <summary>
        /// 수정번호: 1
        /// 
        /// DataTable을 String으로 변환합니다.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DT2Str(DataTable dt)
        {
            StringBuilder s = new StringBuilder();
            #region DataTable Schema 정보 저장 0 - Col Name 1 - ColType 2 ~ N Data
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                s.Append(dt.Columns[i].ColumnName);
                s.Append('│');
            }
            s.Remove(s.Length - 1, 1);
            s.Append('┼');

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                s.Append(dt.Columns[i].DataType.ToString());
                s.Append('│');
            }
            s.Remove(s.Length - 1, 1);
            s.Append('┼');
            #endregion

            #region DataTable Data 저장

            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    object obj = dt.Rows[r][dt.Columns[c].ColumnName];

                    if (obj == DBNull.Value)
                    {
                        s.Append("┴"); //DBNull지정 문자
                    }
                    else
                    {
                        ///
                        ///수정번호: 4
                        if (dt.Columns[c].DataType.ToString() == "System.Byte[]")
                        {
                            s.Append(BLOB2STR(obj as byte[]));
                        }
                        else
                        {
                            s.Append(obj.ToString());
                        }
                    }

                    s.Append('│');
                }
                s.Remove(s.Length - 1, 1);
                s.Append('┼');
            }
            s.Remove(s.Length - 1, 1);
            #endregion

            return s.ToString();
        }

        /// <summary>
        /// 수정번호: 1
        /// 
        /// string을 DataTable로 변환합니다.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DataTable Str2DT(string s)
        {
            DataTable dt = new DataTable();
            string[] data = s.Split(new char[] { '┼' });

            string[] colNames = data[0].Split(new char[] { '│' });
            string[] colTypes = data[1].Split(new char[] { '│' });
            int colCount = colNames.Length;

            Type colType = null;
            DataColumn col = null;

            for (int i = 0; i < colCount; i++)
            {
                colType = Type.GetType(colTypes[i]);
                col = new DataColumn(colNames[i], colType);
                dt.Columns.Add(col);
            }

            string[] colValues = null;
            DataRow row = null;

            for (int r = 2; r < data.Length; r++)
            {
                colValues = data[r].Split(new char[] { '│' });
                row = dt.NewRow();
                for (int c = 0; c < colCount; c++)
                {
                    //전달된 문자열을 이용해 Table을 생성할 때 DateTime객체인 경우 ""이면 캐스팅에러 발생
                    if (colValues[c] == "┴")  //DBNull지정 문자
                    {
                        row[c] = System.DBNull.Value;
                    }
                    else
                    {
                        if (dt.Columns[c].DataType.ToString() == "System.Byte[]")
                        {
                            row[c] = STR2BLOB(colValues[c]);
                        }
                        else
                        {
                            row[c] = colValues[c];
                        }
                    }
                }

                dt.Rows.Add(row);
            }
            return dt;
        }
        #endregion
    }

}
