using System;
using System.Web;
using System.Net;
using System.Text;
using System.IO;

namespace FBSystem
{
    public class NetHelper
    {
        /// <summary>
        /// 向直接URL地址POST值并获取结果:PostVars 为JSON的参数个数
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string Bost_PostUrl(string url, string postVarsValue)
        {
            string sRemoteInfo = string.Empty;
            System.Net.WebClient WebClientObj = new System.Net.WebClient();
            System.Collections.Specialized.NameValueCollection PostVars = new System.Collections.Specialized.NameValueCollection();
            if (postVarsValue != string.Empty)
                PostVars.Add("postVars", postVarsValue);
            try
            {
                byte[] byRemoteInfo = WebClientObj.UploadValues(url, "POST", PostVars);
                sRemoteInfo = System.Text.Encoding.UTF8.GetString(byRemoteInfo);
            }
            catch
            {
                sRemoteInfo = "error";
            }
            return sRemoteInfo;
        }

        #region HTTP GET方式请求数据
        /// <summary>
        /// HTTP GET方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            WebResponse response = null;
            string responseStr = null;

            try
            {
                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                response = null;
            }
            return responseStr;
        }
        #endregion
    }
}
