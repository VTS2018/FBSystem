using System;
using System.Web;
using System.Net;
using System.Text;

namespace FBSystem
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class CharAtExtention
    {
        /// <summary>
        /// 截取字符串指定索引的字符
        /// </summary>
        /// <param name="s">输入的字符串</param>
        /// <param name="index">索引位置</param>
        /// <returns></returns>
        public static string CharAt(this string s, int index)
        {
            if ((index >= s.Length) || (index < 0))
                return "";
            return s.Substring(index, 1);
        }

        public static string Substring2(this string s, int startIndex, int endIndex)
        {
            try
            {
                if (endIndex > startIndex)
                {
                    return s.Substring(startIndex, endIndex - startIndex);
                }
            }
            catch
            {
                return string.Empty;
            }
            return string.Empty;
        }
    }

    public class OpearHTML
    {
        // 主替换函数，直接调用
        public static String replaceStr(String replaceStr, String domain)
        {
            StringBuilder sb = new StringBuilder();
            String[] strArr = replaceStr.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String str in strArr)
            {
                sb.Append(repalceStrAll(str, domain) + "\n");
            }
            return sb.ToString();
        }

        public static String repalceStrAll(String str, String domain)
        {
            StringBuilder sb = new StringBuilder();
            String beforeStr = "";
            String lastStr = "";
            String middleStr = "";
            String afterStr = "";
            int strLen = 0;
            int index = str.IndexOf("<a");
            if (index != -1)
            {
                beforeStr = str.Substring2(0, index);
                // 寻找a标签的>符号
                strLen = str.Length;
                bool find = false;
                for (int i = index + 2; i < strLen; i++)
                {
                    if (str.CharAt(i) == ">") // 该行找到了结束符
                    {
                        find = true;
                        middleStr = str.Substring2(index, i);
                        afterStr = replaceHref(middleStr, domain);

                        lastStr = str.Substring(i);
                        if (lastStr.IndexOf("<a") != -1) // 嵌套循环替换
                        {
                            lastStr = repalceStrAll(lastStr, domain);
                        }
                        break;
                    }
                }
                if (!find)
                {
                    sb.Append(str);
                }
                else
                {
                    sb.Append(beforeStr + afterStr + lastStr);
                }
            }
            else
            {
                sb.Append(str);
            }
            return sb.ToString();

            #region 注释
            //StringBuilder sb = new StringBuilder();
            //String beforeStr = "";
            //String lastStr = "";
            //String middleStr = "";
            //String afterStr = "";
            //int strLen = 0;
            //int index = str.IndexOf("<a");

            //if (index != -1)
            //{
            //    beforeStr = str.Substring2(0, index);
            //    // 寻找a标签的>符号
            //    strLen = str.Length;

            //    for (int i = index + 2; i < strLen; i++)
            //    {
            //        if (str.CharAt(i) == ">") // 刚行找到了结束符
            //        {
            //            middleStr = str.Substring2(index, i);
            //            afterStr = replaceHref(middleStr, domain);

            //            lastStr = str.Substring(i);
            //            if (lastStr.IndexOf("<a") != -1) // 嵌套循环替换
            //            {
            //                lastStr = repalceStrAll(lastStr, domain);
            //            }
            //            break;
            //        }
            //    }
            //    sb.Append(beforeStr + afterStr + lastStr);
            //}
            //else
            //{
            //    sb.Append(str);
            //}
            //return sb.ToString();
            #endregion
        }

        public static String replaceHref(String href, String domain)
        {
            String beforeStr;
            String lastStr;
            String replaceStr;
            String afterStr;
            int index = href.IndexOf("href");
            if (index != -1)
            {
                beforeStr = href.Substring2(0, index);
                int count = 0;
                int beginIndex = 0;
                int endIndex = 0;
                // 读取href后面的引号后面的内容
                for (int i = index + 4; i < href.Length; i++)
                {
                    if (href.CharAt(i) == "\"" || href.CharAt(i) == "\'")
                    {
                        count++;
                        if (count == 1)
                        {
                            beginIndex = i;
                        }
                        if (count == 2)
                        {
                            endIndex = i;
                            break;
                        }
                    }
                }
                lastStr = href.Substring2(endIndex + 1, href.Length);
                replaceStr = href.Substring2(beginIndex + 1, endIndex);
                afterStr = replaceDomain(replaceStr, domain);
                return beforeStr + afterStr + lastStr;
            }
            return href;
        }

        public static String replaceDomain(String str, String domain)
        {
            if (str.IndexOf("javascript") != -1 || str.IndexOf("/") == -1)
            {
                return str;
            }
            int index = 0;
            if (str.StartsWith("http"))
            {
                index = str.IndexOf("http") + 7;
            }
            else if (str.StartsWith("https"))
            {
                index = str.IndexOf("https") + 8;
            }
            // 获取第一个/字符
            String subStr = str.Substring2(index, str.Length);
            //System.out.println(subStr);
            String lastStr = subStr.Substring2(subStr.IndexOf("/"), subStr.Length);
            return "href=\"" + domain + lastStr + "\"";
        }
    }
}