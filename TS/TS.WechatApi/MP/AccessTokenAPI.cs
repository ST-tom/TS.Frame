using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.WechatAPI.MP
{
    /// <summary>
    /// access token 接口
    /// </summary>
    public class AccessTokenAPI : BaseAPI
    {
        public GetTokenRes GetToken()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            url = string.Format(url, WechatSetting.appId, WechatSetting.appSecret);

            return HttpGet<GetTokenRes>(url);
        }
    }

    public class GetTokenRes : BaseRes
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }
}
