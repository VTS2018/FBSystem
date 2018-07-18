using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using FBSystem.MyPageCode;

namespace FBSystem
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            APISoapClient cl = new APISoapClient();

            string scode = "8CE58A07A257433aB8D5F5EA25E1A303";
            string sResult = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("http://www.baidu.com" + scode, "MD5");
            string pageHtml = cl.GetPageCode("http://www.baidu.com", sResult);
            context.Response.Write(pageHtml);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}