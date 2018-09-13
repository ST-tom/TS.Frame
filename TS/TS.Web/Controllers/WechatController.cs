using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TS.WechatAPI;
using TS.WechatAPI.MP;

namespace TS.Web.Controllers
{
    public class WechatController : Controller
    {
        #region 回调函数

        /// <summary>
        /// 接受微信转发过来的消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("CallBack")]
        public string MPCallBack()
        {
            try
            {
                StreamReader reader = new StreamReader(Request.InputStream, Encoding.UTF8);
                var recv = XmlDecode.DecodeXML(reader);

                if (recv.MsgType == MessageRecv._event)
                {
                    //模板消息发送结果通知
                    if (recv.Event == MessageRecv.TEMPLATESENDJOBFINISH)
                        return string.Empty;

                    //如果是订阅事件(关注服务号)
                    if (recv.Event == MessageRecv.subscribe)
                    {

                    }
                }

                if (recv.MsgType == MessageRecv.text)
                {

                }

                if (recv.Event == MessageRecv.CLICK)
                {
                    if (recv.EventKey == "button_003_003")
                    {

                    }
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region 微信OAuth验证

        /// <summary>
        /// 拼接链接，跳转获取oauth token的地址
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult GetOAuthToken(string returnUrl)
        {
            var guid = Guid.NewGuid().ToString();
            returnUrl = HttpUtility.UrlEncode(returnUrl);
            var callbackUrl = WechatSetting.host + Url.Action("WechatOAuthCallback", "Wechat", new { url = returnUrl });
            var wechatUrl = new OAuthAPI().GetAuthorizeUrl(WechatSetting.appId, callbackUrl, guid, OAuthScope.snsapi_base);
            return Redirect(wechatUrl);
        }

        /// <summary>
        /// 获取oauth token后的回调函数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="host"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult WechatOAuthCallBack(string url, string host, string code, string state)
        {
            var oauthService = new OAuthAPI();

            if (string.IsNullOrEmpty(code))
            {
                var callbackUrl = host + Url.Action("WechatOAuthCallback", "Wechat", new { url = url });
                var wechatUrl = oauthService.GetAuthorizeUrl(WechatSetting.appId, callbackUrl, Guid.NewGuid().ToString(), OAuthScope.snsapi_base);

                return Redirect(wechatUrl);
            }

            var auth = oauthService.GetAccessToken(WechatSetting.appId, WechatSetting.appSecret, code);

            Session[WechatSetting.wechatOpenId] = auth.openid;

            url = HttpUtility.UrlDecode(url);

            return Redirect(url);
        }

        #endregion
    }
}