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
        protected void SetUserLoginCookie(HttpResponseBase response,dynamic user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                user.Name,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                false,
                user.CustomerGuid.ToString(),
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

        protected string GetUserLoginCookie(HttpRequestBase request)
        {
            var cookie = request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            return ticket.UserData;
        }
    }
}
