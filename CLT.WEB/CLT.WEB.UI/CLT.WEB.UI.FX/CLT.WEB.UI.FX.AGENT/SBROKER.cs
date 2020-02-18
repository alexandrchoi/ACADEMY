using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;

namespace CLT.WEB.UI.FX.AGENT
{
    public enum LMS_SYSTEM
    {
        MAIN,
        INTRODUCE,
        ADMIN,
        CAPABILITY,
        EDUMANAGEMENT,
        MYPAGE,
        CURRINTRODUCE,
        COMMUNITY,
        MANAGE,
        CURRICULUM,
        OTHERS,
        APPLICATION,
        COMMISSION
    }

    public class SBROKER
    {
        private static Random m_RndGen = new Random();

        static SBROKER()
        {
        }

        public static object CallLocalService(string CLASS_FULL_NAME, string METHOD_NAME, params object[] oParams)
        {
            Type type = null;
            object otmp = null;
            object returnObject = null;

            try
            {
                //type = AccessType(CLASS_FULL_NAME);
                int iClassIndex = 0;
                string BUSINESS_ASSEMBLY = string.Empty;
                iClassIndex = Convert.ToString(CLASS_FULL_NAME).LastIndexOf('.');
                BUSINESS_ASSEMBLY = Convert.ToString(CLASS_FULL_NAME).Substring(0, iClassIndex);
                type = Type.GetType(Convert.ToString(CLASS_FULL_NAME) + "," + BUSINESS_ASSEMBLY + ", Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                otmp = type.Assembly.CreateInstance(CLASS_FULL_NAME);
                returnObject = type.InvokeMember(METHOD_NAME,
                                                    BindingFlags.Default |
                                                    BindingFlags.InvokeMethod,
                                                    null, otmp, oParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                otmp = null;
                type = null;
            }

            return returnObject;
        }

        public static void ExecuteOnly(string CLASS_FULL_NAME, string METHOD_NAME, LMS_SYSTEM EnumVM, string SubProject, params object[] oParams)
        {
            try
            {
                CallLocalService(CLASS_FULL_NAME, METHOD_NAME, oParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetDataSet(string CLASS_FULL_NAME, string METHOD_NAME, LMS_SYSTEM EnumVM, string SubProject, params object[] oParams)
        {
            DataSet dsReturn = null;
            try
            {
                dsReturn = (DataSet)CallLocalService(CLASS_FULL_NAME, METHOD_NAME, oParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsReturn;
        }

        public static DataTable GetTable(string CLASS_FULL_NAME, string METHOD_NAME, LMS_SYSTEM EnumVM, string SubProject, params object[] oParams)
        {
            DataTable dtReturn = null;

            try
            {
                dtReturn = (DataTable)CallLocalService(CLASS_FULL_NAME, METHOD_NAME, oParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dtReturn;
        }

        public static string GetString(string CLASS_FULL_NAME, string METHOD_NAME, LMS_SYSTEM EnumVM, string SubProject, params object[] oParams)
        {
            string sReturn = null;
            try
            {
                sReturn = (string)CallLocalService(CLASS_FULL_NAME, METHOD_NAME, oParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sReturn;
        }

        public static object GetObject(string CLASS_FULL_NAME, string METHOD_NAME, LMS_SYSTEM EnumVM, string SubProject, params object[] oParams)
        {
            object sReturn = null;
            try
            {
                sReturn = CallLocalService(CLASS_FULL_NAME, METHOD_NAME, oParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sReturn;
        }

        //private static Type AccessType(string CLASS_FULL_NAME)
        //{
        //    string BUSINESS_ASSEMBLY = "";
        //    Type type = null;
        //    Assembly assembly = null;

        //    try
        //    {
        //        // Auto Assembly Probing Path : Application Bin Directory
        //        int iClassIndex = CLASS_FULL_NAME.LastIndexOf('.');
        //        BUSINESS_ASSEMBLY = CLASS_FULL_NAME.Substring(0, iClassIndex);
        //        assembly = Assembly.LoadFrom(SystemSettings.GET_BIZASSEM_LOADPATH + BUSINESS_ASSEMBLY + ".dll");

        //        if (assembly == null)
        //        {
        //            throw new Exception("Could not find assembly in BusinessPipe! Assembly: " + BUSINESS_ASSEMBLY);
        //        }

        //        type = assembly.GetType(CLASS_FULL_NAME);

        //        if (type == null)
        //        {
        //            throw new Exception("Could not find type!\nAssembly: " + CLASS_FULL_NAME);
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return type;

        //}

        public enum RETURN_TYPE
        {
            DATASET,
            DATATABLE,
            STRING,
            OBJECT
        }
    }
}
