using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace TS.Service.Cookie
{
    public partial class CookieHelper
    {
        /// <summary>
        /// 将登录的用户的消息，以票据的形式存入到cookie中
        /// </summary>
        /// <param name="response"></param>
        /// <param name="userName">名称</param>
        /// <param name="userData">返回的用户信息(用户id或guid)</param>
        protected void SetUserLoginCookie(HttpResponseBase response,string userName,string userData)
        {
            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                userName,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                false,
                userData,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { HttpOnly = true };
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取当前cookie存储登录的票据，返回票据的UserData
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string GetUserLoginCookie(HttpRequestBase request)
        {
            var cookie = request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            return ticket.UserData;
        }
    }
}
