using System;
using System.IO;
using System.Net;

using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CHelper
{
    /// <summary>
    /// 经常使用的一些通用函数
    /// </summary>
    public partial class Common
    {
        #region DisplayTxtContent

        /// <summary>
        /// txt之读取txt文件显示输出
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        public static void DisplayTxtContent(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            fs.Close();
        }

        #endregion

        #region LoadTextToList
        /// <summary>
        /// txt之将txt中的数据变为个List
        /// </summary>
        /// <param name="filePath">文本的路径</param>
        /// <param name="bl">是否去除掉文本中的空格，txt，js html文件</param>
        /// <returns></returns>
        public static List<string> LoadTextToList(string filePath, bool bl)
        {
            List<string> ls = new List<string>();
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
                {

                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (bl)
                        {
                            ls.Add(line.Trim());
                        }
                        else
                        {
                            ls.Add(line);
                        }
                    }
                }
                return ls;
            }
        }

        #endregion

        #region LoadTextToStack

        /// <summary>
        /// txt之txt中的数据加载到Stack里面
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        /// <param name="bl">是否去除掉空格</param>
        /// <returns></returns>
        public Stack<string> LoadTextToStack(string filePath, bool bl)
        {
            Stack<string> stack = new Stack<string>();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (bl)
                        {
                            stack.Push(line.Trim());
                        }
                        else
                        {
                            stack.Push(line);
                        }
                    }
                }
            }
            return stack;
        }

        #endregion

        #region ReadTextToendByDefault

        /// <summary>
        /// 读取一个文件并返回全部的内容
        /// </summary>
        /// <param name="filePath">文本文件的地址</param>
        /// <returns>文本文件的全部内容,使用默认的编码格式</returns>
        public static string ReadTextToendByDefault(string filePath)
        {
            String strContent = string.Empty;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
                    {
                        strContent = sr.ReadToEnd();
                    }
                }
                catch
                {
                    fs.Dispose();
                }
            }
            return strContent;
        }

        #endregion

        #region ReadTextToendByUTF8

        /// <summary>
        /// 以UTF-8的形式读取文件内容并全部返回
        /// </summary>
        /// <param name="strPath">文本的路径</param>
        /// <returns></returns>
        public static string ReadTextToendByUTF8(string strPath)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(strPath))
            {
                //注意使用System.IO.File.OpenText 防止乱码
                System.IO.StreamReader sr = System.IO.File.OpenText(strPath);
                str = sr.ReadToEnd();
            }
            return str;
        }

        #endregion
    }

    public partial class Common
    {
        #region DropHTML

        ///<summary>   
        ///移除HTML标签   
        ///</summary>   
        ///<param name="HTMLStr">HTMLStr</param>   
        public static string ParseTags(string HTMLStr)
        {
            return System.Text.RegularExpressions.Regex.Replace(HTMLStr, "<[^>]*>", "");
        }

        /// <summary>
        /// 清除html脚本文件。稳定的版本
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string DropHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring))
                return "";
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
            //Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }

        /// <summary>   
        /// 将所有HTML标签替换成""   
        /// </summary>   
        /// <param name="strHtml"></param>   
        /// <returns></returns>   
        public static string StripHTML(string strHtml)
        {
            string[] aryReg =
            {   
                 @"<script[^>]*?>.*?</script>",   
                 @"<(///s*)?!?((/w+:)?/w+)(/w+(/s*=?/s*(([""'])(file://[""'tbnr]|[^/7])*?/7|/w+)|.{0})|/s)*?(///s*)?>",   
                 @"([/r/n])[/s]+",   
                 @"&(quot|#34);",   
                 @"&(amp|#38);",   
                 @"&(lt|#60);",   
                 @"&(gt|#62);",    
                 @"&(nbsp|#160);",    
                 @"&(iexcl|#161);",   
                 @"&(cent|#162);",   
                 @"&(pound|#163);",   
                 @"&(copy|#169);",   
                 @"&#(/d+);",   
                 @"-->",   
                 @"<!--.*/n"  
           };

            string[] aryRep = 
            {   
                  "",   
                  "",   
                  "",   
                  "\"",   
                  "&",   
                  "<",   
                  ">",   
                  " ",   
                  "/xa1",//chr(161),   
                  "/xa2",//chr(162),   
                  "/xa3",//chr(163),   
                  "/xa9",//chr(169),   
                  "",   
                  "/r/n",   
                  ""  
             };

            string newReg = aryReg[0];
            string strOutput = strHtml;

            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }
            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("/r/n", "");
            return strOutput;
        }

        #endregion

        #region WipeoffSChar

        /// <summary>
        /// 【文件命名】去除文件命名中的特殊字符
        /// 【在对文件重命名的时候非常有用】
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static string WipeoffSChar(string strInput)
        {
            try
            {
                if (!string.IsNullOrEmpty(strInput))
                {
                    char[] rChar = Path.GetInvalidPathChars();

                    strInput = strInput.Trim();

                    string strChar = "/|$|'|!|:|“|”|’| |‘|'|.|#|(|)|?|,|_|[|]|{|}|@";

                    string[] arrlist = strChar.Split('|');

                    for (int i = 0; i < rChar.Length; i++)
                    {
                        strInput = strInput.Replace(rChar[i], ' ');
                    }

                    for (int j = 0; j < arrlist.Length; j++)
                    {
                        strInput = strInput.Replace(arrlist[j], " ");
                    }

                    strInput = strInput.Replace("  ", " ").Replace(" ", "-");

                    return strInput;
                }
            }
            catch
            {
                return Guid.NewGuid().ToString();
            }
            return string.Empty;
        }

        #region 注销
        //public static void T()
        //{
        //    Console.WriteLine(Convert.ToInt32('a'));
        //    Console.WriteLine(Convert.ToInt32('z'));
        //    Console.WriteLine(Convert.ToInt32('A'));
        //    Console.WriteLine(Convert.ToInt32('Z'));

        //    for (int i = 97; i <= 122; i++)
        //    {
        //        Console.WriteLine(Convert.ToChar(i));
        //    }

        //    for (int i = 65; i <= 90; i++)
        //    {
        //        Console.WriteLine(Convert.ToChar(i));
        //    }


        //    Console.WriteLine(IsCharandDia(' '));
        //    Console.WriteLine(IsCharandDia('a'));
        //    Console.WriteLine(IsCharandDia('A'));
        //    Console.WriteLine(IsCharandDia('1'));
        //    Console.WriteLine(IsCharandDia('~'));
        //    Console.WriteLine(IsCharandDia('%'));
        //    Console.WriteLine(IsCharandDia('`'));
        //    Console.WriteLine(IsCharandDia('발'));
        //} 
        #endregion

        #endregion

        #region FilterString

        /// <summary>
        /// 判断字符是否数据应为字母大小写，十进制数字，空白
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsCharandDia(char ch)
        {
            //要求字符输出为26个英文字母；空白：space；十进制数字0-9
            //bool bl = true;
            //if ((char.ToLower(ch) >= 'a' && char.ToLower(ch) <= 'z') || (Convert.ToInt32(ch) == 32) || char.IsDigit(ch))
            //{
            //    bl = true;
            //}
            //else
            //{
            //    bl = false;
            //}
            //return bl;
            return ((char.ToLower(ch) >= 'a' && char.ToLower(ch) <= 'z') || (Convert.ToInt32(ch) == 32) || char.IsDigit(ch));
        }

        /// <summary>
        /// 【文件命名】-【实用版】获得一个可以用作文件命名的字符串
        /// </summary>
        /// <param name="strInput">要输入的字符串</param>
        /// <param name="ch">连字符</param>
        /// <returns></returns>
        public static string FilterString(string strInput, char chS)
        {
            string strR = string.Empty;

            if (!string.IsNullOrEmpty(strInput))
            {
                strR = strInput.Trim();
                List<char> lsC = new List<char>();
                foreach (char ch in strInput)
                {
                    if (IsCharandDia(ch))
                    {
                        lsC.Add(ch);
                    }
                }
                //strR = new string(lsC.ToArray()).ToString().Replace(' ', '-');
                strR = new string(lsC.ToArray()).ToString().Replace(' ', chS);
            }
            return strR;
        }

        #endregion

        #region StringSplit

        /// <summary>
        /// 功能：将字符串分割成数组
        /// </summary>
        /// <param name="strSource">目标字符串</param>
        /// <param name="strSplit">分隔符</param>
        /// <returns>分割后的字符数组，返回的内容中并不包含所谓的分隔符号</returns>
        public static string[] StringSplit(string strSource, string strSplit)
        {
            /* 
            "asdfasfasdfasdfasdf"
            "sfasdfasdfasdf"
            "sdfasdfasdf"
            "sdfasdf"
            "sdf"
            "sdfasdf"
            "sdfasdfasdf"
            "sfasdfasdfasdf"
            "asdfasfasdfasdfasdf"
            */
            string[] strtmp = new string[1];

            int index = strSource.IndexOf(strSplit, 0);//得到分割符出现的第一个位置

            if (index < 0)//表示没有找到该分隔符
            {
                strtmp[0] = strSource;
                return strtmp;//返回目标的字符
            }
            else
            {
                strtmp[0] = strSource.Substring(0, index);//没什么价值

                return StringSplit(strSource.Substring(index + strSplit.Length), strSplit, strtmp);
            }
        }

        /// <summary>
        /// 功能：采用递归将字符串分割成数组
        /// </summary>
        /// <param name="strSource">目标源</param>
        /// <param name="strSplit">分割符号</param>
        /// <param name="attachArray">附加数组,在最后的结果中出现</param>
        /// <returns></returns>
        public static string[] StringSplit(string strSource, string strSplit, string[] attachArray)
        {
            string[] strtmp = new string[attachArray.Length + 1];//临时

            attachArray.CopyTo(strtmp, 0);

            int index = strSource.IndexOf(strSplit, 0);
            if (index < 0)
            {
                strtmp[attachArray.Length] = strSource;
                return strtmp;
            }
            else
            {
                strtmp[attachArray.Length] = strSource.Substring(0, index);
                return StringSplit(strSource.Substring(index + strSplit.Length), strSplit, strtmp);
            }
        }

        #endregion

        #region StringReplace
        /// <summary>
        /// 功能：替换字符串
        /// </summary>
        /// <param name="strSource">目标字符串</param>
        /// <param name="oldStr">要替换的字符串</param>
        /// <param name="strNew">替换成</param>
        /// <returns>ReplaceString("Hello World!","Hello","Hi")</returns>
        public static string StringReplace(string strSource, string oldStr, string strNew)
        {
            if (!string.IsNullOrEmpty(strSource) && !string.IsNullOrEmpty(oldStr) && !string.IsNullOrEmpty(strNew))
            {
                return strSource.Replace(oldStr, strNew);
            }
            else
            {
                return strSource;
            }
        }

        #endregion

        #region GetHtmlImageUrlList
        /// <summary> 
        /// 获得HTML中所有图片的src地址【比较稳定的一个版本】
        /// </summary> 
        /// <param name="sHtmlText">HTML代码</param> 
        /// <returns>图片的URL列表</returns> 
        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(sHtmlText);
            int i = 0;

            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表 
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            return sUrlList;
        }

        #endregion

        #region GetHtmlImageUrlListV2
        /// <summary> 
        /// 取得HTML中所有图片的 URL。 
        /// </summary> 
        /// <param name="sHtmlText">HTML代码</param> 
        /// <returns>图片的URL列表</returns> 
        public static string GetHtmlImageUrlListV2(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(sHtmlText);

            StringBuilder sbr = new StringBuilder();

            // 取得匹配项列表 
            foreach (Match match in matches)
            {
                sbr.Append(match.Groups["imgUrl"].Value + Environment.NewLine);
            }
            return sbr.ToString();
        }

        #endregion

        #region GetImgUrl

        ///<summary>   
        ///取出文本中的图片地址   
        ///</summary>   
        ///<param name="HTMLStr">HTMLStr</param>   
        public static string GetImgUrl(string HTMLStr)
        {
            string str = string.Empty;
            // string sPattern = @"^<img\s+[^>]*>";
            Regex r = new Regex(@"<img\s+[^>]*\s*src\s*=\s*([']?)(?<url>\S+)'?[^>]*>", RegexOptions.Compiled);
            Match m = r.Match(HTMLStr.ToLower());
            if (m.Success)
            {
                str = m.Result("${url}");
            }
            return str;
        }

        #endregion

        #region GetExtName
        /// <summary>
        /// 获得字符串中的扩展名，获取不到是“.jpg”
        /// </summary>
        /// <param name="strURL"></param>
        /// <returns></returns>
        public static string GetExtName(string strInput)
        {
            string str = string.Empty;
            try
            {
                str = Path.GetExtension(strInput);
            }
            catch
            {
                str = ".jpg";
            }
            return str;
        }

        #endregion

        #region GetExtName
        /// <summary>
        /// 获得字符串中的扩展名，输入默认的类型
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="defalutExt"></param>
        /// <returns></returns>
        public static string GetExtName(string strInput, string defalutExt)
        {
            string str = string.Empty;
            try
            {
                str = Path.GetExtension(strInput);
            }
            catch
            {
                return defalutExt;
            }
            return str;
        }

        #endregion

        #region GetNumber
        /// <summary>
        /// 解析出字符串中的数字
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static string GetNumber(string strInput)
        {
            StringBuilder sbr = new StringBuilder();
            if (strInput != "" && strInput != null)
            {
                char[] achar = strInput.ToCharArray();

                foreach (char item in achar)
                {
                    if (char.IsDigit(item))
                    {
                        sbr.Append(item.ToString());
                    }
                }
            }
            return sbr.ToString();
        }
        #endregion

        #region GenerateFileName
        /// <summary>
        /// 获得以当前日期为文件名字的字符串
        /// </summary>
        /// <returns></returns>
        public static string GenerateFileName()
        {
            string strName = string.Empty;
            strName = System.DateTime.Now.ToString("G").Replace("/", "-").Replace(":", "-");
            return strName;
        }
        #endregion

        #region GenerateDateTimeString
        /// <summary>
        /// 获得日期随机码字符串，用于上传文件重命名文件
        /// </summary>
        /// <returns></returns>
        public static string GenerateDateTimeString()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }
        #endregion

        #region GenerateGuid
        /// <summary>
        /// 获得以Guid方式生成的随机字符串
        /// </summary>
        /// <returns></returns>
        public static string GenerateGuid()
        {
            return Guid.NewGuid().ToString();
        }
        #endregion

        #region GenerateStringID
        /// <summary>
        /// 获得一个不重复的字符串
        /// 据说一亿次数都不重复
        /// </summary>
        /// <returns></returns>
        public static string GenerateStringID()
        {
            long i = 1;
            byte[] barr = Guid.NewGuid().ToByteArray();
            foreach (byte b in barr)
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
        #endregion

        #region GenerateIntID

        /// <summary>
        /// 获得一个不重复的长整型数字
        /// </summary>
        /// <returns></returns>
        public static long GenerateIntID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        #endregion

        #region GenerateString
        /// <summary>
        /// 固定长度的随机字符串
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns>随机串</returns>
        public static string GenerateString(int length)
        {
            char[] charList = {'0','1','2','3','4','5','6','7','8','9',
            'A','B','C','D','E','F','G','H','I','J','K','L','M',
            'N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            'a','b','c','d','e','f','g','h','i','j','k','l','m',
            'n','o','p','q','r','s','t','u','v','w','x','y','z'};
            char[] rev = new char[length];
            Random f = new Random();
            for (int i = 0; i < length; i++)
            {
                rev[i] = charList[Math.Abs(f.Next(127)) % length];
            }
            return new string(rev);
        }
        #endregion

        #region GenerateString
        /// <summary>
        /// 随机字符串生成算法
        /// 移除方式得到随机不重复的字符串
        /// 该算法是字符长度越大随机性就越大【比较好的一个方法】
        /// </summary>
        /// <param name="input">要输入的字符串</param>
        /// <returns></returns>
        public static string GenerateString(string input)
        {
            //字符串中字符长度
            int len = input.Length;

            //将字符串转换为字符数组
            char[] chs = input.ToCharArray();

            //用来保存字符数组下标
            List<int> indexes = new List<int>();

            //保存新字符串作为返回结果
            string result = "";

            for (int i = 0; i < len; i++)
                indexes.Add(i);

            Random rd = new Random();//bug修复的地点就在这个地方不要使用0进行初始化

            while (len > 0)
            {
                int ranNum = rd.Next(len);

                //随机生成一个数字，然后取该数字作为字符数组下标，将该位置的字符取出放到新字符串中
                result += chs[indexes[ranNum]].ToString();

                //字符数组下标中排除已经使用过的下标
                indexes.Remove(indexes[ranNum]);

                len--;
            }

            return result;  //该算法是字符长度越大随机性就越大
        }

        #endregion

        #region GeneratePassWord
        ///<summary>
        ///获得一个用于生成密码的随机字符串
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GeneratePassWord(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;

            //随机字符串生成器的主要功能如下：
            //1、支持自定义字符串长度
            //2、支持自定义是否包含数字
            //3、支持自定义是否包含小写字母
            //4、支持自定义是否包含大写字母
            //5、支持自定义是否包含特殊符号
            //6、支持自定义字符集
        }
        #endregion
    }

    public partial class Common
    {
        #region 注销
        //客户端代码
        //public static void T2()
        //{
        //    //struct本身是一个结构体
        //    //DateTime dt0 = new DateTime();

        //    DateTime dt1 = new DateTime(2012, 8, 14, 10, 54, 55);
        //    DateTime dt2 = new DateTime(2012, 12, 21);//2012-12-21 00:00:00
        //    Console.WriteLine(DateDiff(dt1, dt2));

        //    //我活了多少天了
        //    DateTime dt3 = new DateTime(2013, 12, 25, 20, 49, 00);
        //    DateTime dt4 = new DateTime(1990, 11, 17, 02, 48, 00);//2012-12-21 00:00:00
        //    Console.WriteLine("我活了:" + DateDiff(dt4, dt3));
        //} 
        #endregion

        #region DateDiff
        /// <summary>
        /// DateTime操作计算时间的差值
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);

            TimeSpan ts = ts1.Subtract(ts2).Duration();

            dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
            return dateDiff;

            #region note
            //C#中使用TimeSpan计算两个时间的差值
            //可以反加两个日期之间任何一个时间单位。
            //TimeSpan ts = Date1 - Date2;
            //double dDays = ts.TotalDays;//带小数的天数，比如1天12小时结果就是1.5 
            //int nDays = ts.Days;//整数天数，1天12小时或者1天20小时结果都是1  
            #endregion
        }

        #endregion

        #region DateDiff
        ///<summary>   
        ///返回两个日期之间的时间间隔（y：年份间隔、M：月份间隔、d：天数间隔、h：小时间隔、m：分钟间隔、s：秒钟间隔、ms：微秒间隔）   
        ///</summary>   
        ///<param   name="Date1">开始日期</param>   
        ///<param   name="Date2">结束日期</param>   
        ///<param   name="Interval">间隔标志</param>   
        ///<returns>返回间隔标志指定的时间间隔</returns>   
        public static object DateDiff(System.DateTime Date1, System.DateTime Date2, string Interval)
        {
            double dblYearLen = 365;//年的长度，365天   
            double dblMonthLen = (365 / 12);//每个月平均的天数   

            System.TimeSpan objT;
            objT = Date2.Subtract(Date1);

            //Console.WriteLine(objT.ToString());//获得的差多少时间的文本表示
            //Console.WriteLine(objT.TotalDays); //获得多少天的表示
            //Console.WriteLine(objT.TotalHours);//获得多少小时
            //Console.WriteLine(objT.TotalMilliseconds);
            //Console.WriteLine(objT.TotalMinutes);//整分钟数的表示
            //Console.WriteLine(objT.TotalSeconds);//整秒数

            switch (Interval)
            {
                case "y"://返回日期的年份间隔   
                    return System.Convert.ToInt32(objT.Days / dblYearLen);
                case "M"://返回日期的月份间隔   
                    return System.Convert.ToInt32(objT.Days / dblMonthLen);
                case "d"://返回日期的天数间隔   
                    return objT.Days;
                case "h"://返回日期的小时间隔   
                    return objT.Hours;
                case "m"://返回日期的分钟间隔   
                    return objT.Minutes;
                case "s"://返回日期的秒钟间隔   
                    return objT.Seconds;
                case "ms"://返回时间的微秒间隔   
                    return objT.Milliseconds;
                default:
                    break;
            }
            return 0;
        }

        #endregion
    }

    public partial class Common
    {
        #region SetLog

        /// <summary>
        /// 日志函数
        /// </summary>
        /// <param name="logpath">保存日志的地址</param>
        /// <returns></returns>
        public static bool SetLog(string logPath, string strContent)
        {
            bool bl = true;
            try
            {
                using (System.IO.FileStream fs = new FileStream(logPath, FileMode.Append, FileAccess.Write))
                {
                    using (System.IO.StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(strContent);
                    }
                }
            }
            catch
            {
                bl = false;
            }
            return bl;
        }

        #endregion

        #region TraceLog

        /// <summary>
        /// 跟踪数据
        /// </summary>
        /// <param name="data"></param>
        public static void TraceLog(string LogFile, string data)
        {
            //一般会将这个路劲在构造函数中给出
            try
            {
                //在这个地方做手脚即可
                File.AppendAllText(LogFile, string.Concat(data, Environment.NewLine));
            }
            catch
            {

            }
        }

        #endregion
    }

    public partial class Common
    {
        #region IsFileExist

        /// <summary>
        /// 判断一个文件是否存在
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static bool IsFileExist(string filename)
        {
            return System.IO.File.Exists(filename);
        }

        #endregion

        #region GetDirFile

        /// <summary>
        /// 获取指定目录下的指定类型的文件信息。只支持一级目录下的文件的获取
        /// </summary>
        /// <param name="strDir">目录路径</param>
        /// <param name="strFileType">文件类型【*.txt】</param>
        /// <param name="bl">是否返回完全的路径</param>
        /// <returns></returns>
        public static string[] GetDirFile(string strDir, string strFileType, bool bl)
        {
            if (!string.IsNullOrEmpty(strDir))
            {
                DirectoryInfo dir = new DirectoryInfo(strDir);
                int lenth = dir.GetFiles(strFileType).Length;
                string[] arr = new string[lenth];
                int i = 0;

                foreach (FileInfo dChild in dir.GetFiles(strFileType))
                {
                    if (bl)
                    {
                        arr[i] = dChild.FullName;
                    }
                    else
                    {
                        arr[i] = dChild.Name;
                    }
                    i++;
                }
                return arr;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region GetDir

        /// <summary>
        /// 获得指定目录下所有的目录信息。
        /// </summary>
        /// <param name="strDir">目标目录</param>
        /// <param name="bl">是否获得完整的路径呢？</param>
        /// <returns></returns>
        public static string[] GetDir(string strDir, bool bl)
        {
            if (!string.IsNullOrEmpty(strDir))
            {
                DirectoryInfo dir = new DirectoryInfo(strDir);
                DirectoryInfo[] dirarr = dir.GetDirectories();
                string[] arr = new string[dirarr.Length];

                for (int i = 0; i < dirarr.Length; i++)
                {
                    if (bl)
                    {
                        arr[i] = dirarr[i].FullName;
                    }
                    else
                    {
                        arr[i] = dirarr[i].Name;
                    }
                }
                return arr;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region IsSubDir

        /// <summary>
        /// 判断目录下是否有子目录
        /// </summary>
        /// <param name="strDir"></param>
        /// <returns></returns>
        public static bool IsSubDir(string strDir)
        {
            bool bl = true;
            if (!string.IsNullOrEmpty(strDir))
            {
                if (!Directory.Exists(strDir))
                {
                    bl = false;
                }

                string[] arr = Directory.GetDirectories(strDir);
                if (arr == null || arr.Length == 0)
                {
                    bl = false;
                }
            }
            return bl;
        }

        #endregion

        #region CreateDir

        /// <summary>
        /// 创建一个目录。创建时会删除同名目录，所以目录下不能有文件
        /// </summary>
        /// <param name="directoryPath"></param>
        public static bool CreateDir(string directoryPath)
        {
            bool bl = true;
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    Directory.Delete(directoryPath);
                    Directory.CreateDirectory(directoryPath);
                    bl = true;
                }
                else
                {
                    Directory.CreateDirectory(directoryPath);
                    bl = true;
                }
            }
            catch (Exception)
            {
                bl = false;
            }
            return bl;
        }

        #endregion

        #region DeleteFolder

        /// <summary>
        /// 删除目录下的所有子目录和文件
        /// </summary>
        /// <param name="dir"></param>
        public static bool DeleteFolder(string dir)
        {
            bool bl = true;

            try
            {
                //删除该目录下的文件
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                    {
                        FileInfo fi = new FileInfo(d);

                        //判断是否为只读
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        {
                            //如果包含就把文件的属性设置为正常，即没有设置其他的属性
                            fi.Attributes = FileAttributes.Normal;
                        }
                        //直接删除其中的文件
                        File.Delete(d);
                        //Console.WriteLine(string.Format("{0}已被删除！", d));
                    }
                    else
                    {
                        //递归删除子文件夹
                        DeleteFolder(d);
                        //Console.WriteLine("{0}已被删除！", d);
                    }
                }

                //删除已空文件夹
                Directory.Delete(dir);
                //Console.WriteLine(string.Format("{0}已被删除！", dir));
            }
            catch
            {
                bl = false;
            }
            return bl;
        }

        #endregion

        #region CopyFile

        /// <summary>
        /// 复制移动文件
        /// </summary>
        /// <param name="strSourcefile">源文件路径</param>
        /// <param name="strGoal">保存文件路径，带扩展名</param>
        /// <param name="isMove">是否移动呢？</param>
        /// <returns></returns>
        public static bool CopyFile(string strSourcefile, string strSaveFile, bool isMove)
        {
            bool bl = true;
            try
            {
                //如果源文件存在
                if (File.Exists(strSourcefile))
                {
                    if (isMove)
                    {
                        File.Move(strSourcefile, strSaveFile);
                    }
                    else
                    {
                        File.Copy(strSourcefile, strSaveFile, true);
                    }
                }
                else
                {
                    bl = false;
                }
            }
            catch
            {
                bl = false;
            }
            return bl;
        }

        #endregion

        #region CopyDir

        /// <summary>
        /// 将指定文件夹下面的所有内容copy到目标文件夹下面
        /// </summary>
        /// <param name="srcPath">源目录</param>
        /// <param name="aimPath">目标目录</param>
        public static void CopyDir(string srcPath, string aimPath)
        {
            #region 注销

            #region note
            /*
             * 本文介绍如何将一个目录里面的所有文件复制到目标目录里面。 
                下面介绍几个我们在该例程中将要使用的类： 
                1、Directory：Exposes static methods for creating, moving, and enumerating through directories and subdirectories.
                2、Path：Performs operations on String instances that contain file or directory path information. These operations are performed in a cross-platform manner.
                3、File：Provides static methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of FileStream objects.
                这两个类里都是不可继承的，是从object直接继承来的，被实现成sealed，上面的解释均来自MSDN。 
                下面是实现的代码，代码中的细节不在这里详细描述，请看代码注释： 
             *  嘿嘿！这次给大家说的功能比较简单，适合初学者，希望对初学者有所帮助
              ！如果你需要将该功能使用在Web工程中，那么请设置给ASPNET帐号足够的权限，不过这样做是很危险的，不建议这样做。
             */

            #endregion

            // 检查目标目录是否以目录分割字符结束如果不是则添加之 
            if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
            {
                aimPath += Path.DirectorySeparatorChar;
            }

            // 判断目标目录是否存在如果不存在则新建之 
            if (!Directory.Exists(aimPath))
            {
                Directory.CreateDirectory(aimPath);
            }

            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组 
            // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法 
            // string[] fileList = Directory.GetFiles(srcPath); 

            string[] fileList = Directory.GetFileSystemEntries(srcPath);

            // 遍历所有的文件和目录 
            foreach (string file in fileList)
            {
                // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件 
                if (Directory.Exists(file))
                {
                    if (!file.EndsWith("Backup"))
                    {
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    }
                }
                // 否则直接Copy文件 
                else
                {
                    File.Copy(file, aimPath + Path.GetFileName(file), true);
                    //Microsoft.VisualBasic.FileSystem.FileCopy(file, aimPath + Path.GetFileName(file));
                    //Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(file, aimPath + Path.GetFileName(file));
                }
            }
            #endregion
        }

        /// <summary>
        /// 将指定文件夹下面的所有内容copy到目标文件夹下面
        /// </summary>
        /// <param name="srcPath">源目录</param>
        /// <param name="aimPath">目标目录</param>
        public static void CopyDirectory(string srcPath, string aimPath)
        {
            #region note
            /*
             * 本文介绍如何将一个目录里面的所有文件复制到目标目录里面。 
                下面介绍几个我们在该例程中将要使用的类： 
                1、Directory：Exposes static methods for creating, moving, and enumerating through directories and subdirectories.
                2、Path：Performs operations on String instances that contain file or directory path information. These operations are performed in a cross-platform manner.
                3、File：Provides static methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of FileStream objects.
                这两个类里都是不可继承的，是从object直接继承来的，被实现成sealed，上面的解释均来自MSDN。 
                下面是实现的代码，代码中的细节不在这里详细描述，请看代码注释： 
             *  嘿嘿！这次给大家说的功能比较简单，适合初学者，希望对初学者有所帮助
              ！如果你需要将该功能使用在Web工程中，那么请设置给ASPNET帐号足够的权限，不过这样做是很危险的，不建议这样做。
             */

            #endregion

            // 检查目标目录是否以目录分割字符结束如果不是则添加之 
            if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                aimPath += Path.DirectorySeparatorChar;

            // 判断目标目录是否存在如果不存在则新建之 
            if (!Directory.Exists(aimPath))
                Directory.CreateDirectory(aimPath);

            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组 
            // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法 
            // string[] fileList = Directory.GetFiles(srcPath); 

            string[] fileList = Directory.GetFileSystemEntries(srcPath);

            // 遍历所有的文件和目录 
            foreach (string file in fileList)
            {
                // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件 
                if (Directory.Exists(file))
                {
                    CopyDirectory(file, aimPath + Path.GetFileName(file));
                }
                else
                {
                    File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
        }

        #endregion

    }

    public partial class Common
    {
        #region CreateFile

        /// <summary>
        /// 创建指定路径指定格式的文件：适用于创建txt，css，js文件，创建之前会删除同名的文件的
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static bool CreateFile(string strPath)
        {
            bool bl = true;
            try
            {
                //string strPath = "E:\\m.txt";
                if (File.Exists(strPath))
                {
                    File.Delete(strPath);
                    File.Create(strPath);
                    bl = true;
                }
                else
                {
                    File.Create(strPath);
                    bl = true;
                }
            }
            catch
            {
                bl = false;
            }
            return bl;
        }

        #endregion

        #region CreateFile

        /// <summary>
        /// 创建指定格式的文件并写入内容。如果文件已存在，它将被覆盖。默认的编码
        /// </summary>
        /// <param name="strContent">写入的文件内容</param>
        /// <param name="strGobalPaht">保存的路径</param>
        /// <returns></returns>
        public static bool CreateFile(string strContent, string strGobalPath)
        {
            try
            {
                using (FileStream fs = new FileStream(strGobalPath, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sr = new StreamWriter(fs, Encoding.Default))
                    {
                        sr.WriteLine(strContent);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region CreateFile

        /// <summary>
        /// 创建指定格式的文件并写入内容，指定路径，扩展名和编码格式
        /// </summary>
        /// <param name="strContent">写入的文件内容</param>
        /// <param name="strGobalPaht">保存的路径</param>
        /// <param name="encoding">指定的编码格式</param>
        /// <returns></returns>
        public static bool CreateFile(string strContent, string strGobalPath, Encoding encoding)
        {
            try
            {
                using (FileStream fs = new FileStream(strGobalPath, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sr = new StreamWriter(fs, encoding))
                    {
                        sr.WriteLine(strContent);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region CreateFile

        /// <summary>
        /// 创建指定格式的文件：该函数会自动创建目录.可以指定编码，不指定的话默认编码
        /// </summary>
        /// <param name="strContent">写入的文件内容</param>
        /// <param name="strDir">保存的目录</param>
        /// <param name="strName">保存的文件名</param>
        /// <param name="strType">保存的文件类型</param>
        /// <returns></returns>
        public static bool CreateFile(string strContent, string strDir, string strName, string strType, Encoding encoding)
        {
            try
            {
                if (!Directory.Exists(strDir))
                {
                    Directory.CreateDirectory(strDir);
                }

                Encoding en = (encoding == null ? Encoding.Default : encoding);
                using (FileStream fs = new FileStream(strDir + strName + strType, FileMode.Create, FileAccess.Write))
                {

                    using (StreamWriter sr = new StreamWriter(fs, en))
                    {
                        sr.WriteLine(strContent);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region FileToBinary

        /// <summary>
        /// 将传进来的文件转换成字符串
        /// </summary>
        /// <param name="FilePath">待处理的文件路径(本地或服务器)</param>
        /// <returns></returns>
        public static string FileToBinary(string FilePath)
        {
            using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                //利用新传来的路径实例化一个FileStream对像
                int fileLength = Convert.ToInt32(fs.Length);

                //得到对像大小
                byte[] fileByteArray = new byte[fileLength];

                //声明一个byte数组
                using (BinaryReader br = new BinaryReader(fs))
                {
                    //声明一个读取二进流的BinaryReader对像
                    for (int i = 0; i < fileLength; i++)
                    {
                        //循环数组
                        br.Read(fileByteArray, 0, fileLength);
                        //将数据读取出来放在数组中
                    }
                }
                string strData = Convert.ToBase64String(fileByteArray);

                //装数组转换为String字符串
                return strData;
            }
        }

        #endregion

        #region BinaryToFile

        /// <summary>
        /// 装传进来的字符串保存为文件
        /// </summary>
        /// <param name="path">需要保存的位置路径</param>
        /// <param name="binary">需要转换的字符串</param>
        public static void BinaryToFile(string path, string binary)
        {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            //利用新传来的路径实例化一个FileStream对像
            BinaryWriter bw = new BinaryWriter(fs);
            //实例化一个用于写的BinaryWriter
            bw.Write(Convert.FromBase64String(binary));
            bw.Close();
            fs.Close();
        }

        #endregion
    }

    public partial class Common
    {
        #region DownFile

        /// <summary>
        /// 原名称下载文件的方法
        /// </summary>
        /// <param name="stringUrl">下载文件的URL</param>
        /// <param name="stringDirs">保存的目录[不包含文件名]</param>
        /// <returns></returns>
        public static bool DownFilesWhile(string stringUrl, string savefilePath)
        {
            //string stringUrl = "http://www.meileg.com/beautyleg/photo/big/256-Ruby-81/0001.jpg";
            bool bl = true;
            try
            {
                #region 处理逻辑
                if (!string.IsNullOrEmpty(stringUrl))
                {

                    //根据URL创建请求的实例对象
                    WebRequest httpRequest = WebRequest.Create(stringUrl);

                    //由请求对象获得响应对象
                    WebResponse httpResponse = httpRequest.GetResponse();

                    //获得响应流
                    using (Stream netStream = httpResponse.GetResponseStream())
                    {
                        using (FileStream fileStream = new FileStream(savefilePath, FileMode.Create, FileAccess.Write))
                        {
                            //初始化缓冲区的大小,每次读取时的单位长度
                            //byte[] downloadBuffer = new byte[4096];
                            byte[] downloadBuffer = new byte[60];

                            int bufferSize;//接收每次返回的字节大小
                            int curDownSize = 0;//当前下载数据的大小

                            while ((bufferSize = netStream.Read(downloadBuffer, 0, downloadBuffer.Length)) > 0)
                            {
                                curDownSize += bufferSize;
                                //double speed = (httpResponse.ContentLength - curDownSize);
                                //Console.WriteLine(speed);
                                fileStream.Write(downloadBuffer, 0, bufferSize);
                            }
                        }
                    }
                }
                else
                {
                    bl = false;
                }
                #endregion
            }
            catch (WebException e)
            {
                #region 异常处理
                bl = false;
                e = new WebException("404");
                //Console.WriteLine(e.Message);
                //写入日志
                //CommonSpace.Conmmon.CreateFile(ImgHelper.strLog);
                //WriteLog(ImgHelper.strLog);
                //Console.WriteLine(webe.Status.ToString());
                #endregion
            }
            return bl;
            #region note
            //获取请求文件的类型
            //Console.WriteLine(httpResponse.ContentType);
            //获得请求数据的大小，字节数114072byte
            //Console.WriteLine(httpResponse.ContentLength);
            //从请求响应中获得响应流 
            //获得当前应用程序的所在的更目录
            //Console.WriteLine(Environment.CurrentDirectory);
            //Console.WriteLine(System.Windows.Forms.Application.StartupPath); 
            #endregion
        }

        /// <summary>
        /// 自动命名下载的方法:使用HttpWebRequest对象
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="FileName"></param>
        public static bool DownFilesDo(string Url, string FileName)
        {
            bool bl = true;
            try
            {
                // 1、使用HttpWebRequest/HttpWebResonse和WebClient
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                WebResponse response = request.GetResponse();
                //Stream stream = response.GetResponseStream();

                if (!response.ContentType.ToLower().StartsWith("text/"))
                {
                    //Value = SaveBinaryFile(response, FileName); 
                    byte[] buffer = new byte[4029];
                    Stream outStream = System.IO.File.Create(FileName);
                    Stream inStream = response.GetResponseStream();

                    int l;
                    do
                    {
                        l = inStream.Read(buffer, 0, buffer.Length);
                        if (l > 0)
                            outStream.Write(buffer, 0, l);
                    } while (l > 0);

                    outStream.Close();
                    inStream.Close();
                }
            }
            catch (Exception)
            {
                bl = false;
            }
            return bl;
        }

        #endregion
    }
}