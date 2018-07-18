using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBSystem
{
    public class HttpCusModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }
        private void context_BeginRequest(object sender, EventArgs args)
        {
            HttpApplication app = sender as HttpApplication;
            string url = app.Request.Url.ToString();

            string newUrl = string.Empty;
            if (!url.EndsWith(".js") && !url.EndsWith(".ashx") && !url.EndsWith(".aspx"))
            {
                if (app.Request.UrlReferrer != null)
                {
                    newUrl += "referurl=" + app.Request.UrlReferrer.Host;
                }
                if (!string.IsNullOrEmpty(url))
                {
                    if (!string.IsNullOrEmpty(newUrl))
                    {
                        newUrl += "&";
                    }
                    newUrl += "url=" + url;
                }
                app.Context.RewritePath("~/Default.ashx", null, newUrl);
            }
        }

        public void Dispose()
        {

        }
    }
}