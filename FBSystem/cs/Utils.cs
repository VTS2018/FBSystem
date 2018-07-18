using System;
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

//1.0
namespace Octopus.Common
{
    public partial class Utils
    {
        #region 系统版本
        ///// <summary>
        ///// 版本信息类
        ///// </summary>
        //public class VersionInfo
        //{
        //    public int FileMajorPart
        //    {
        //        get { return 3; }
        //    }
        //    public int FileMinorPart
        //    {
        //        get { return 0; }
        //    }
        //    public int FileBuildPart
        //    {
        //        get { return 0; }
        //    }
        //    public string ProductName
        //    {
        //        get { return "DTcms"; }
        //    }
        //    public int ProductType
        //    {
        //        get { return 0; }
        //    }
        //}
        //public static string GetVersion()
        //{
        //    return DTKeys.ASSEMBLY_VERSION;
        //}
        #endregion

        #region 获得配置文件节点XML文件的绝对路径
        public static string GetXmlMapPath(string xmlName)
        {
            return GetMapPath(ConfigurationManager.AppSettings[xmlName].ToString());
        }
        #endregion

        #region 获得当前绝对路径
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion

        #region 文件操作

        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        public static bool DeleteFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return false;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除上传的文件(及缩略图)
        /// </summary>
        /// <param name="_filepath"></param>
        public static void DeleteUpFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return;
            }
            string fullpath = GetMapPath(_filepath); //原图
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
            if (_filepath.LastIndexOf("/") >= 0)
            {
                string thumbnailpath = _filepath.Substring(0, _filepath.LastIndexOf("/")) + "mall_" + _filepath.Substring(_filepath.LastIndexOf("/") + 1);
                string fullTPATH = GetMapPath(thumbnailpath); //宿略图
                if (File.Exists(fullTPATH))
                {
                    File.Delete(fullTPATH);
                }
            }
        }

        /// <summary>
        /// 删除指定文件夹
        /// </summary>
        /// <param name="_dirpath">文件相对路径</param>
        public static bool DeleteDirectory(string _dirpath)
        {
            if (string.IsNullOrEmpty(_dirpath))
            {
                return false;
            }
            string fullpath = GetMapPath(_dirpath);
            if (Directory.Exists(fullpath))
            {
                Directory.Delete(fullpath, true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改指定文件夹名称
        /// </summary>
        /// <param name="old_dirpath">旧相对路径</param>
        /// <param name="new_dirpath">新相对路径</param>
        /// <returns>bool</returns>
        public static bool MoveDirectory(string old_dirpath, string new_dirpath)
        {
            if (string.IsNullOrEmpty(old_dirpath))
            {
                return false;
            }
            string fulloldpath = GetMapPath(old_dirpath);
            string fullnewpath = GetMapPath(new_dirpath);
            if (Directory.Exists(fulloldpath))
            {
                Directory.Move(fulloldpath, fullnewpath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 返回文件大小KB
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>int</returns>
        public static int GetFileSize(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return 0;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                FileInfo fileInfo = new FileInfo(fullpath);
                return ((int)fileInfo.Length) / 1024;
            }
            return 0;
        }

        /// <summary>
        /// 返回文件扩展名，不含“.”
        /// </summary>
        /// <param name="_filepath">文件全名称</param>
        /// <returns>string</returns>
        public static string GetFileExt(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return "";
            }
            if (_filepath.LastIndexOf(".") > 0)
            {
                return _filepath.Substring(_filepath.LastIndexOf(".") + 1); //文件扩展名，不含“.”
            }
            return "";
        }

        /// <summary>
        /// 返回文件名，不含路径
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>string</returns>
        public static string GetFileName(string _filepath)
        {
            return _filepath.Substring(_filepath.LastIndexOf(@"/") + 1);
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>bool</returns>
        public static bool FileExists(string _filepath)
        {
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获得远程字符串
        /// </summary>
        public static string GetDomainStr(string key, string uriPath)
        {
            string result = CacheHelper.Get(key) as string;
            if (result == null)
            {
                System.Net.WebClient client = new System.Net.WebClient();
                try
                {
                    client.Encoding = System.Text.Encoding.UTF8;
                    result = client.DownloadString(uriPath);
                }
                catch
                {
                    result = "Temporarily unable to connect!";
                }
                CacheHelper.Insert(key, result, 60);
            }
            return result;
        }

        #endregion

        #region 操作权限菜单
        /// <summary>
        /// 获取操作权限
        /// </summary>
        /// <returns>Dictionary</returns>
        public static Dictionary<string, string> ActionType()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Show", "显示");
            dic.Add("View", "查看");
            dic.Add("Add", "添加");
            dic.Add("Edit", "修改");
            dic.Add("Delete", "删除");
            dic.Add("Audit", "审核");
            dic.Add("Reply", "回复");
            dic.Add("Confirm", "确认");
            dic.Add("Cancel", "取消");
            dic.Add("Invalid", "作废");
            dic.Add("Build", "生成");
            dic.Add("Instal", "安装");
            dic.Add("Unload", "卸载");
            dic.Add("Back", "备份");
            dic.Add("Restore", "还原");
            dic.Add("Replace", "替换");
            return dic;
        }
        #endregion
    }

    public partial class Utils
    {
        #region V1 WriteCookie

        /// <summary>
        /// 写cookie值【会话Cookie】
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        #endregion

        #region V2 WriteCookie

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        #endregion

        #region GetCookie

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return UrlDecode(HttpContext.Current.Request.Cookies[strName].Value.ToString());
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null &&
                HttpContext.Current.Request.Cookies[strName][key] != null)
            {
                return UrlDecode(HttpContext.Current.Request.Cookies[strName][key].ToString());
            }
            else
            {
                return "";
            }
        }

        #endregion
    }

    public partial class Utils
    {
        #region Check Object

        /// <summary>
        /// 检查对象是否为null问题
        /// </summary>
        /// <param name="obj">Null：true</param>
        /// <returns></returns>
        public static bool CheckObject(object obj)
        {
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)) || (ObjectToStr(obj) == ""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Check Int32

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            if (expression != null)
                return IsNumeric(expression.ToString());

            return false;
        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断给定的字符串(strInt)是否是数值型
        /// </summary>
        /// <param name="strInt">要确认的字符串</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsInt(string strInt)
        {
            if (strInt == null || strInt == "")
            {
                return false;
            }
            //return new Regex(@"^([0-9])[0-9]*(\.\w*)?$").IsMatch(strInt);	//整数和小数
            return new Regex(@"^(0|[1-9]\d*)$").IsMatch(strInt);	//正整数
        }

        #endregion

        #region Check Double

        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object expression)
        {
            if (expression != null)
                return Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");

            return false;
        }

        #endregion

        #region Check Email

        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
        }

        public static bool IsValidDoEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        #endregion

        #region Check URL

        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }

        /// <summary>
        /// 是否为httpUrl地址
        /// </summary>
        /// <param name="httpUrl"></param>
        /// <returns></returns>
        public static bool IsHttpUrl(string httpUrl)
        {
            //return Regex.IsMatch(WebUrl, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            //return Regex.IsMatch(WebUrl, @"http://");
            return httpUrl.IndexOf("http://") != -1;
        }
        #endregion

        #region Check IP
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion

        #region Check DateTime
        /// <summary>
        ///  判断字符串是否合法的日期格式
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsData(string value)
        {
            try
            {
                System.DateTime.Parse(value);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region String <====> Array

        /// <summary>
        /// 将字符串转换为数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字符串数组</returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new char[',']);
        }

        /// <summary>
        /// 将数组转换为字符串
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="speater">分隔符</param>
        /// <returns>String</returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        #endregion

        #region  Object <====> Bool

        /// <summary>
        /// object型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool ObjToBool(object expression, bool defValue)
        {
            if (expression != null)
            {
                return StrToBool(expression.ToString(), defValue);
            }
            else
            {
                return defValue;
            }
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            if (expression != null)
            {
                if (string.Compare(expression, "true", true) == 0)
                {
                    return true;
                }
                else if (string.Compare(expression, "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }

        #endregion

        #region Object <====> Int32

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjToInt(object expression, int defValue)
        {
            if (expression != null)
            {
                return StrToInt(expression.ToString(), defValue);
            }
            return defValue;
        }

        /// <summary>
        /// 将字符串转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string expression, int defValue)
        {
            if (string.IsNullOrEmpty(expression) || expression.Trim().Length >= 11 || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
            {
                return defValue;
            }
            int rv;
            if (Int32.TryParse(expression, out rv))
            {
                return rv;
            }
            return Convert.ToInt32(StrToFloat(expression, defValue));
        }

        #endregion

        #region Object <====> Decimal

        /// <summary>
        /// Object型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal ObjToDecimal(object expression, decimal defValue)
        {
            if (expression != null)
            {
                return StrToDecimal(expression.ToString(), defValue);
            }
            return defValue;
        }

        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(string expression, decimal defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            decimal intValue = defValue;
            if (expression != null)
            {
                bool IsDecimal = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsDecimal)
                    decimal.TryParse(expression, out intValue);
            }
            return intValue;
        }

        #endregion

        #region Object <====> Float

        /// <summary>
        /// Object型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjToFloat(object expression, float defValue)
        {
            if (expression != null)
            {
                return StrToFloat(expression.ToString(), defValue);
            }
            return defValue;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string expression, float defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            float intValue = defValue;
            if (expression != null)
            {
                bool IsFloat = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(expression, out intValue);
            }
            return intValue;
        }

        #endregion

        #region Object <====> DateTime

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str, DateTime defValue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime dateTime;
                if (DateTime.TryParse(str, out dateTime))
                    return dateTime;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str)
        {
            return StrToDateTime(str, DateTime.Now);
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj)
        {
            return StrToDateTime(obj.ToString());
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj, DateTime defValue)
        {
            return StrToDateTime(obj.ToString(), defValue);
        }

        #endregion

        #region DateTime <====>中文
        /// <summary>
        /// 将时间换成中文
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <returns></returns>
        public static string DateToChineseString(DateTime datetime)
        {
            TimeSpan ts = DateTime.Now - datetime;
            //    System.Web.HttpContext.Current.Response.Write(ts.TotalDays);
            if ((int)ts.TotalDays >= 365)
            {
                return (int)ts.TotalDays / 365 + "年前";
            }
            if ((int)ts.TotalDays >= 30 && ts.TotalDays <= 365)
            {
                return (int)ts.TotalDays / 30 + "月前";
            }
            if ((int)ts.TotalDays == 1)
            {
                return "昨天";
            }
            if ((int)ts.TotalDays == 2)
            {
                return "前天";
            }
            if ((int)ts.TotalDays >= 3 && ts.TotalDays <= 30)
            {
                return (int)ts.TotalDays + "天前";
            }
            if ((int)ts.TotalDays == 0)
            {
                if ((int)ts.TotalHours != 0)
                {
                    return (int)ts.TotalHours + "小时前";
                }
                else
                {
                    if ((int)ts.TotalMinutes == 0)
                    {
                        return "1分钟前";
                    }
                    else
                    {
                        return (int)ts.TotalMinutes + "分钟前";
                    }
                }
            }
            return datetime.ToString("yyyy年MM月dd日 HH:mm");
        }
        #endregion

        #region Object <====> String

        /// <summary>
        /// 将对象转换为字符串
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的string类型结果</returns>
        public static string ObjectToStr(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            return obj.ToString().Trim();
        }
        #endregion

        #region Base64 <====> String
        public static string StringToBase64(string str)
        {
            byte[] c = Convert.FromBase64String(str);
            return Encoding.Default.GetString(c);
        }

        public static string Base64ToString(string str)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(b);
        }

        #endregion
    }

    public partial class Utils
    {
        #region 上传文件
        public static bool SingleUploadFile(HttpFileCollection files, out string[] fileInfo)
        {
            bool bl = true;
            fileInfo = new string[4] { "", "", "", "" };
            #region MIME
            string[] fileMime = new string[] 
            { 
                "application/octet-stream", 
                "image/bmp", 
                "image/gif", 
                "application/msword",
                "text/plain",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "application/vnd.ms-excel"
            };
            #endregion
            HttpPostedFile hpf = files[0];
            if (hpf.ContentLength > 0)
            {
                #region FileInfo

                //获取客户端上的文件的完全限定名称。
                string clientPath = hpf.FileName;

                //截取文件名称
                string fname = Path.GetFileName(clientPath);

                //文件MiME类型
                string type = hpf.ContentType;

                //"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",// xlsx
                //"application/vnd.ms-excel" //xls

                //验证逻辑 只让用户上传一定范围格式内的文件 对文件的大小也要进行限制
                string savePath = HttpContext.Current.Server.MapPath("~/Upload/" + fname);
                #endregion

                #region SaveFile
                if (Path.GetExtension(fname) != ".exe")
                {
                    if (Array.FindIndex(fileMime, x => x == type) >= 0)
                    {
                        try
                        {
                            hpf.SaveAs(savePath);
                        }
                        catch
                        {
                            bl = false;
                        }

                        fileInfo = string.Concat("文件名称：" + fname, "|",
                            "文件大小：" + hpf.ContentLength, "|",
                            "文件类型：" + hpf.ContentType, "|", savePath).Split(new char[] { '|' });
                    }
                }
                #endregion
            }
            return bl;
        }
        public static bool SingleUploadFile(HttpFileCollection files, string savePath, out string[] fileInfo)
        {
            bool bl = true;
            fileInfo = new string[4] { "", "", "", "" };

            #region MIME
            string[] fileMime = new string[] 
            { 
                "application/octet-stream", 

                "image/bmp", 
                "image/gif", 
                "image/jpeg",
                "image/x-icon",
                "image/png",

                "application/msword",
                "text/plain",
                "text/html",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "application/vnd.ms-excel",
            };
            #endregion

            HttpPostedFile hpf = files[0];
            if (hpf.ContentLength > 0)
            {
                #region FileInfo

                //获取客户端上的文件的完全限定名称。
                string clientPath = hpf.FileName;

                //截取文件名称
                string fname = Path.GetFileName(clientPath);

                //文件MiME类型
                string type = hpf.ContentType;

                //"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",// xlsx
                //"application/vnd.ms-excel" //xls

                //验证逻辑 只让用户上传一定范围格式内的文件 对文件的大小也要进行限制
                //string savePath = HttpContext.Current.Server.MapPath("~/Upload/" + fname);
                #endregion

                #region SaveFile
                if (Path.GetExtension(fname) != ".exe")
                {
                    if (Array.FindIndex(fileMime, x => x == type) >= 0)
                    {
                        try
                        {
                            hpf.SaveAs(savePath + "\\" + fname);
                        }
                        catch
                        {
                            bl = false;
                        }

                        fileInfo = string.Concat("文件名称：" + fname, "|",
                            "文件大小：" + hpf.ContentLength, "|",
                            "文件类型：" + hpf.ContentType, "|",
                            savePath).Split(new char[] { '|' });
                    }
                }
                #endregion
            }

            return bl;
        }
        #endregion
        #region 下载文件
        public static void DownLoadFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            string newFileName = Path.GetFileName(filePath);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + newFileName);
            HttpContext.Current.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.WriteFile(fileInfo.FullName);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        #endregion
    }

    public partial class Utils
    {
        #region 返回字符串真实长度
        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns></returns>
        public static int GetLength(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            return Encoding.Default.GetBytes(str).Length;
        }
        #endregion

        #region 分割字符串 1

        /// <summary>
        /// 分割字符串 1
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                {
                    return new string[] { strContent };
                }
                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
            {
                return new string[0] { };
            }
        }

        #endregion

        #region 分割字符串 2
        /// <summary>
        /// 分割字符串 2
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];
            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }
            return result;
        }
        #endregion

        #region 清除HTML标记
        public static string DropHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring)) return "";
            //删除脚本  
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML  
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }

        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string Content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(Content, regexstr, string.Empty, RegexOptions.IgnoreCase).Trim();
        }
        #endregion

        #region 清除HTML标记且返回相应的长度
        public static string DropHTML(string Htmlstring, int strLen)
        {
            return CutString(DropHTML(Htmlstring), strLen);
        }
        #endregion

        #region 截取字符长度
        /// <summary>
        /// 截取字符长度
        /// </summary>
        /// <param name="inputString">字符</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string CutString(string inputString, int len)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return "";
            }
            inputString = DropHTML(inputString);
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号 
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString += "…";
            return tempString;
        }
        #endregion

        #region TXT代码转换成HTML格式
        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="chr">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把TXT代码转换成HTML格式
        public static String ToHtml(string Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\r\n", "<br />");
            sb.Replace("\n", "<br />");
            sb.Replace("\t", " ");
            //sb.Replace(" ", "&nbsp;");
            return sb.ToString();
        }
        #endregion

        #region HTML代码转换成TXT格式
        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="chr">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把HTML代码转换成TXT格式
        public static String ToTxt(String Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("&nbsp;", " ");
            sb.Replace("<br>", "\r\n");
            sb.Replace("<br>", "\n");
            sb.Replace("<br />", "\n");
            sb.Replace("<br />", "\r\n");
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");
            sb.Replace("&amp;", "&");
            return sb.ToString();
        }
        #endregion

        #region 删除最后结尾的一个逗号
        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            if (str.Length < 1)
            {
                return "";
            }
            return str.Substring(0, str.LastIndexOf(","));
        }
        #endregion

        #region 删除最后结尾的指定字符后的字符

        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        /// <param name="str">源字符</param>
        /// <param name="strchar">结尾的字符</param>
        /// <returns></returns>
        public static string DelLastChar(string str, string strchar)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            if (str.LastIndexOf(strchar) >= 0 && str.LastIndexOf(strchar) == str.Length - 1)
            {
                return str.Substring(0, str.LastIndexOf(strchar));
            }
            return str;
        }

        #endregion

        #region 删除指定字符后面的所有字符串

        /// <summary>
        /// 删除指定字符后面的所有字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strchar"></param>
        /// <returns></returns>
        public static string DelLastStringBychar(string str, string strchar)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            if (str.LastIndexOf(strchar) >= 0)
            {
                return str.Substring(0, str.LastIndexOf(strchar));
            }
            return str;
        }

        #endregion

        #region 替换指定的字符串
        /// <summary>
        /// 替换指定的字符串
        /// </summary>
        /// <param name="originalStr">原字符串</param>
        /// <param name="oldStr">旧字符串</param>
        /// <param name="newStr">新字符串</param>
        /// <returns></returns>
        public static string ReplaceStr(string originalStr, string oldStr, string newStr)
        {
            if (string.IsNullOrEmpty(oldStr))
            {
                return "";
            }
            return originalStr.Replace(oldStr, newStr);
        }
        #endregion
    }

    public partial class Utils
    {
        /*安全处理*/
        #region 处理掉URL特殊字符

        ///<summary>
        ///处理掉URL特殊字符
        ///</summary>
        public static string ClearUrlCode(string str, params string[] strV)
        {
            string repalceValue = "/|$|'|!|:|“|”|’| |‘|'|.|#|(|)|?|,|_|=|+|&|;|├";
            str = DropHTML(str.Trim()).ToLower();
            string[] rvlist = repalceValue.Split('|');
            if (rvlist.Length > 0)
            {
                for (int i = 0; i < rvlist.Length; i++)
                {
                    if (rvlist[i].Length > 0)
                    {
                        str = str.Replace(rvlist[i], "-").Replace("--", "-");
                    }
                }

                if (strV.Length > 0)
                {
                    for (int i = 0; i < strV.Length; i++)
                    {
                        if (strV[i].Length > 0)
                        {
                            str = str.Replace(strV[i], "");
                        }
                    }
                }
            }
            return str;
        }

        #endregion

        #region 检测是否有Sql危险字符

        /// <summary>
        /// 检测是否有Sql危险字符
        /// 没有返回true
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        /// <summary>
        /// 检查危险字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Filter(string sInput)
        {
            if (sInput == null || sInput == "")
                return null;
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                //throw new Exception("字符串中含有非法字符!");
                throw new Exception("unsafe string");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }

        /// <summary> 
        /// 检查过滤设定的危险字符
        /// </summary> 
        /// <param name="InText">要过滤的字符串 </param> 
        /// <returns>如果参数存在不安全字符，则返回true </returns> 
        public static bool SqlFilter(string word, string InText)
        {
            if (InText == null)
                return false;
            foreach (string i in word.Split('|'))
            {
                if ((InText.ToLower().IndexOf(i + " ") > -1) || (InText.ToLower().IndexOf(" " + i) > -1))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 过滤特殊字符
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Htmls(string Input)
        {
            if (Input != string.Empty && Input != null)
            {
                string ihtml = Input.ToLower();
                ihtml = ihtml.Replace("<script", "&lt;script");
                ihtml = ihtml.Replace("script>", "script&gt;");
                ihtml = ihtml.Replace("<%", "&lt;%");
                ihtml = ihtml.Replace("%>", "%&gt;");
                ihtml = ihtml.Replace("<$", "&lt;$");
                ihtml = ihtml.Replace("$>", "$&gt;");
                return ihtml;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region URL字符编码
        /// <summary>
        /// URL字符编码
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.Replace("'", "");
            return HttpContext.Current.Server.UrlEncode(str);
        }
        #endregion

        #region URL字符解码
        /// <summary>
        /// URL字符解码
        /// </summary>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return HttpContext.Current.Server.UrlDecode(str);
        }
        #endregion

        #region 组合URL参数

        /// <summary>
        /// 组合URL参数
        /// </summary>
        /// <param name="_url">页面地址</param>
        /// <param name="_keys">参数名称</param>
        /// <param name="_values">参数值</param>
        /// <returns>String</returns>
        public static string CombUrlTxt(string _url, string _keys, params string[] _values)
        {
            StringBuilder urlParams = new StringBuilder();
            try
            {
                string[] keyArr = _keys.Split(new char[] { '&' });
                for (int i = 0; i < keyArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(_values[i]) && _values[i] != "0")
                    {
                        _values[i] = UrlEncode(_values[i]);
                        urlParams.Append(string.Format(keyArr[i], _values) + "&");
                    }
                }
                if (!string.IsNullOrEmpty(urlParams.ToString()) && _url.IndexOf("?") == -1)
                    urlParams.Insert(0, "?");
            }
            catch
            {
                return _url;
            }
            return _url + DelLastChar(urlParams.ToString(), "&");
        }

        #endregion

        #region 重新组合URL参数并进行编码

        /// <summary>
        /// 重新组合URL参数并进行编码
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static string UrlParmes(string queryString)
        {
            StringBuilder sbr = new StringBuilder();
            if (queryString.Contains("&") || queryString.Contains("="))
            {
                string[] arrp = queryString.Split('&', '=');
                for (int i = 0; i < arrp.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        sbr.Append(arrp[i] + "=");
                    }
                    else
                    {
                        sbr.Append(System.Web.HttpUtility.UrlEncode(arrp[i]) + "&");
                    }
                }
            }
            return Utils.DelLastChar(sbr.ToString(), "&");
        }

        #endregion

        #region 替换URL
        /// <summary>
        /// 替换扩展名
        /// </summary>
        public static string GetUrlExtension(string urlPage, string staticExtension)
        {
            int indexNum = urlPage.LastIndexOf('.');
            if (indexNum > 0)
            {
                return urlPage.Replace(urlPage.Substring(indexNum), "." + staticExtension);
            }
            return urlPage;
        }
        /// <summary>
        /// 替换扩展名，如没有扩展名替换默认首页
        /// </summary>
        public static string GetUrlExtension(string urlPage, string staticExtension, bool defaultVal)
        {
            int indexNum = urlPage.LastIndexOf('.');
            if (indexNum > 0)
            {
                return urlPage.Replace(urlPage.Substring(indexNum), "." + staticExtension);
            }
            if (defaultVal)
            {
                if (urlPage.EndsWith("/"))
                {
                    return urlPage + "index." + staticExtension;
                }
                else
                {
                    return urlPage + "/index." + staticExtension;
                }
            }
            return urlPage;
        }
        #endregion

        #region 返回URL中结尾的文件名
        /// <summary>
        /// 返回URL中结尾的文件名
        /// </summary>		
        public static string GetUrlFileName(string url)
        {
            //	HttpContext.Current.Response.Write( url.IndexOf("?"));

            if (url == null)
            {
                return "";
            }
            //是否有参数
            if (url.IndexOf("?") != -1)
            {
                //去掉参数
                string noquery = url.Substring(0, url.IndexOf("?"));

                //根据/分组
                string[] filenames = noquery.Split(new char[] { '/' });

                //文件名
                string filename = filenames[filenames.Length - 1];

                return filename;
            }
            else
            {
                return System.IO.Path.GetFileName(url);
            }

            //以前的
            //			if (url == null)
            //			{
            //				return "";
            //			}
            //			string[] strs1 = url.Split(new char[]{'/'});
            //			return strs1[strs1.Length - 1].Split(new char[]{'?'})[0];
        }
        #endregion
    }

    public partial class Utils
    {
        /*随即字符*/
        #region 生成随机ID
        /// <summary>
        /// 生成随机ID
        /// 【用于购物车ID和订单号ID】
        /// </summary>
        /// <returns></returns>
        public static string GetRandomID()
        {
            DateTime dt = DateTime.Now.ToUniversalTime();
            Random ran = new Random();
            string RandKey = ran.Next(100, 999).ToString();
            string randomid = dt.ToString("yyyyMMddHHmmssffff") + RandKey;
            return randomid;
        }
        #endregion

        #region 生成指定长度的字符串
        /// <summary>
        /// 生成指定长度的字符串,即生成strLong个str字符串
        /// </summary>
        /// <param name="strLong">生成的长度</param>
        /// <param name="str">以str生成字符串</param>
        /// <returns></returns>
        public static string StringOfChar(int strLong, string str)
        {
            string ReturnStr = "";
            for (int i = 0; i < strLong; i++)
            {
                ReturnStr += str;
            }
            return ReturnStr;
        }
        #endregion

        #region 生成日期随机码
        /// <summary>
        /// 生成日期随机码
        /// </summary>
        /// <returns></returns>
        public static string GetRamCode()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }
        #endregion

        #region 生成随机字母或数字

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns></returns>
        public static string Number(int Length)
        {
            return Number(Length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Number(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }

        /// <summary>
        /// 生成随机字母字符串(数字字母混和)
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        public static string GetCheckCode(int codeCount)
        {
            string str = string.Empty;
            int rep = 0;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        /// <summary>
        /// 根据日期和随机码生成订单号
        /// </summary>
        /// <returns></returns>
        public static string GetOrderNumber()
        {
            string num = DateTime.Now.ToString("yyMMddHHmmss");//yyyyMMddHHmmssms
            return num + Number(2).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numSeeds"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int Next(int numSeeds, int length)
        {
            byte[] buffer = new byte[length];
            System.Security.Cryptography.RNGCryptoServiceProvider Gen = new System.Security.Cryptography.RNGCryptoServiceProvider();
            Gen.GetBytes(buffer);
            uint randomResult = 0x0;//这里用uint作为生成的随机数  
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)buffer[i] << ((length - 1 - i) * 8));
            }
            return (int)(randomResult % numSeeds);
        }

        #endregion

        #region 生成随机码[未检查]

        /// <summary>
        /// 生成随机码[未检查]
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="isLower">是否转化成小写</param>
        /// <returns></returns>
        public static string RandCode(int length, bool isLower)
        {
            char[] arrChar = new char[]
            {
               'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
               '0','1','2','3','4','5','6','7','8','9',
               'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'
            };

            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < length; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            if (isLower)
            {
                return num.ToString().ToLower();
            }
            return num.ToString();
        }

        #endregion
    }

    public partial class Utils
    {
        #region 显示分页

        /// <summary>
        /// 返回分页页码
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="linkUrl">链接地址，__id__代表页码</param>
        /// <param name="centSize">中间页码数量</param>
        /// <returns></returns>
        public static string OutPageList(int pageSize, int pageIndex, int totalCount, string linkUrl, int centSize)
        {
            //计算页数
            if (totalCount < 1 || pageSize < 1)
            {
                return "";
            }
            int pageCount = totalCount / pageSize;
            if (pageCount < 1)
            {
                return "";
            }
            if (totalCount % pageSize > 0)
            {
                pageCount += 1;
            }
            if (pageCount <= 1)
            {
                return "";
            }
            StringBuilder pageStr = new StringBuilder();

            string pageId = "__id__";
            string firstBtn = "<a href=\"" + ReplaceStr(linkUrl, pageId, (pageIndex - 1).ToString()) + "\">«上一页</a>";
            string lastBtn = "<a href=\"" + ReplaceStr(linkUrl, pageId, (pageIndex + 1).ToString()) + "\">下一页»</a>";
            string firstStr = "<a href=\"" + ReplaceStr(linkUrl, pageId, "1") + "\">1</a>";
            string lastStr = "<a href=\"" + ReplaceStr(linkUrl, pageId, pageCount.ToString()) + "\">" + pageCount.ToString() + "</a>";

            if (pageIndex <= 1)
            {
                firstBtn = "<span class=\"disabled\">«上一页</span>";
            }
            if (pageIndex >= pageCount)
            {
                lastBtn = "<span class=\"disabled\">下一页»</span>";
            }
            if (pageIndex == 1)
            {
                firstStr = "<span class=\"current\">1</span>";
            }
            if (pageIndex == pageCount)
            {
                lastStr = "<span class=\"current\">" + pageCount.ToString() + "</span>";
            }
            int firstNum = pageIndex - (centSize / 2); //中间开始的页码
            if (pageIndex < centSize)
                firstNum = 2;
            int lastNum = pageIndex + centSize - ((centSize / 2) + 1); //中间结束的页码
            if (lastNum >= pageCount)
                lastNum = pageCount - 1;
            pageStr.Append("<span>共" + totalCount + "记录</span>");
            pageStr.Append(firstBtn + firstStr);
            if (pageIndex >= centSize)
            {
                pageStr.Append("<span>...</span>\n");
            }
            for (int i = firstNum; i <= lastNum; i++)
            {
                if (i == pageIndex)
                {
                    pageStr.Append("<span class=\"current\">" + i + "</span>");
                }
                else
                {
                    pageStr.Append("<a href=\"" + ReplaceStr(linkUrl, pageId, i.ToString()) + "\">" + i + "</a>");
                }
            }
            if (pageCount - pageIndex > centSize - ((centSize / 2)))
            {
                pageStr.Append("<span>...</span>");
            }
            pageStr.Append(lastStr + lastBtn);
            return pageStr.ToString();
        }

        /// <summary>
        /// 返回分页页码
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="linkUrl">链接地址，__id__代表页码</param>
        /// <param name="centSize">中间页码数量</param>
        /// <returns></returns>
        public static string OutPageList(int pageSize, int pageIndex, int totalCount, string linkUrl, int centSize, params string[] langPage)
        {
            //计算页数
            if (totalCount < 1 || pageSize < 1)
            {
                return "";
            }
            int pageCount = totalCount / pageSize;
            if (pageCount < 1)
            {
                return "";
            }
            if (totalCount % pageSize > 0)
            {
                pageCount += 1;
            }
            if (pageCount <= 1)
            {
                return "";
            }
            /*
             * langPage[0]=首页
             * langPage[1]=上一页
             * langPage[2]=下一页
             * langPage[3]=最后一页
             * langPage[4]=Page
             * langPage[5]=Total
             */
            StringBuilder pageStr = new StringBuilder();

            string pageId = "__id__";
            string firstBtn = "<a href=\"" + ReplaceStr(linkUrl, pageId, (pageIndex - 1).ToString()) + "\">" + langPage[1] + "</a>";
            string lastBtn = "<a href=\"" + ReplaceStr(linkUrl, pageId, (pageIndex + 1).ToString()) + "\">" + langPage[2] + "</a>";
            string firstStr = "<a href=\"" + ReplaceStr(linkUrl, pageId, "1") + "\">1</a>";
            string lastStr = "<a href=\"" + ReplaceStr(linkUrl, pageId, pageCount.ToString()) + "\">" + pageCount.ToString() + "</a>";

            if (pageIndex <= 1)
            {
                firstBtn = "<span class=\"disabled\">" + langPage[1] + "</span>";
            }
            if (pageIndex >= pageCount)
            {
                lastBtn = "<span class=\"disabled\">" + langPage[2] + "</span>";
            }
            if (pageIndex == 1)
            {
                firstStr = "<span class=\"current\">1</span>";
            }
            if (pageIndex == pageCount)
            {
                lastStr = "<span class=\"current\">" + pageCount.ToString() + "</span>";
            }
            int firstNum = pageIndex - (centSize / 2); //中间开始的页码
            if (pageIndex < centSize)
                firstNum = 2;
            int lastNum = pageIndex + centSize - ((centSize / 2) + 1); //中间结束的页码
            if (lastNum >= pageCount)
                lastNum = pageCount - 1;

            //pageStr.Append("<span>共" + totalCount + "记录</span>");
            pageStr.Append(string.Format("<span>" + langPage[5] + "</span>", totalCount));

            pageStr.Append(firstBtn + firstStr);
            if (pageIndex >= centSize)
            {
                pageStr.Append("<span>...</span>\n");
            }
            for (int i = firstNum; i <= lastNum; i++)
            {
                if (i == pageIndex)
                {
                    pageStr.Append("<span class=\"current\">" + i + "</span>");
                }
                else
                {
                    pageStr.Append("<a href=\"" + ReplaceStr(linkUrl, pageId, i.ToString()) + "\">" + i + "</a>");
                }
            }
            if (pageCount - pageIndex > centSize - ((centSize / 2)))
            {
                pageStr.Append("<span>...</span>");
            }
            pageStr.Append(lastStr + lastBtn);
            return pageStr.ToString();
        }

        //增强版：跳转功能+下拉框选择+多语言+展示样式
        public static string OutPageListV2()
        {
            return "";
        }

        #endregion
    }
}