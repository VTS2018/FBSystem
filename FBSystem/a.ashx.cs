using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBSystem
{
    /// <summary>
    /// a 的摘要说明
    /// </summary>
    public class a : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            context.Response.Write(NetHelper.HttpGet("http://it.glassvipshop.com/"));
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