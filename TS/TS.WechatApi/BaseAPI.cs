using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TS.WechatAPI
{
    public class BaseAPI
    {
        public static T HttpGet<T>(string url, int? timeout = null, string userAgent = "", CookieCollection cookies = null, string param = "")
            where T : BaseRes
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";

            if (timeout.HasValue)
                myRequest.Timeout = timeout.Value;

            if (!string.IsNullOrEmpty(userAgent))
                myRequest.UserAgent = userAgent;

            if (cookies != null)
            {
                myRequest.CookieContainer = new CookieContainer();
                myRequest.CookieContainer.Add(cookies);
            }

            HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            var res = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            return res;
        }
          
        public static T HttpPost<T>(string url, string post, int? timeout = null, string userAgent = "", CookieCollection cookies = null)
            where T : BaseRes
        {
            Encoding encoding = Encoding.GetEncoding("utf-8");
            byte[] data_array = encoding.GetBytes(post);

            // 准备请求...
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

            if (timeout.HasValue)
                myRequest.Timeout = timeout.Value;

            if (!string.IsNullOrEmpty(userAgent))
                myRequest.UserAgent = userAgent;

            if (cookies != null)
            {
                myRequest.CookieContainer = new CookieContainer();
                myRequest.CookieContainer.Add(cookies);
            }

            Stream newStream = myRequest.GetRequestStream();
            // 发送数据
            newStream.Write(data_array, 0, data_array.Length);
            newStream.Close();

            HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            var res = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            return res;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受       
        }
    }
}
