using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.WechatAPI
{
    /// <summary>
    /// 配置
    /// </summary>
    public class WechatSetting
    {
        public static string appId = ConfigurationManager.AppSettings["appId"].ToString();
        public static string appSecret = ConfigurationManager.AppSettings["appSecret"].ToString();
        public static string host = ConfigurationManager.AppSettings["host"].ToString();
        public const string wechatOpenId = "wechat_open_id";
        public const string userInfo = "user_info";
    }

    public enum Language
    {
        /// <summary>
        /// 中文简体
        /// </summary>
        zh_CN,
        /// <summary>
        /// 中文繁体
        /// </summary>
        zh_TW,
        /// <summary>
        /// 英文
        /// </summary>
        en
    }
}
