using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.WechatAPI.MP
{
    /// <summary>
    /// 模板消息接口
    /// </summary>
    public class TempMsgAPI : BaseAPI
    {
        public BaseRes Send(string accessToken,string openId, string template_id, Dictionary<string, TempMsgData> data, string turnUrl = "", string appId = "", string pagePath = "")
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
            url = string.Format(url, accessToken);

            var post = JsonConvert.SerializeObject(new
            {
                touser = openId,
                template_id = template_id,
                data = data,
                url = turnUrl,
                miniprogram = new
                {
                    appid = appId,
                    pagePath = pagePath
                }
            });
            return HttpPost<BaseRes>(url, post);
        }
    }

    public class TempMsgData
    {
        public string value;
        public string color;
    }
}
