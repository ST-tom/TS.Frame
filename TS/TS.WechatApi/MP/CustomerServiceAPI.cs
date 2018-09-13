using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.WechatAPI.MP
{
    /// <summary>
    /// 客服消息接口API
    /// </summary>
    public class CustomerServiceAPI : BaseAPI
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public BaseRes SendMessage(string accessToken,string openId,string content)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", accessToken);
            var data = JsonConvert.SerializeObject(new
            {
                touser = openId,
                msgtype = "text",
                text = new
                {
                    content = content,
                }
            });

            return HttpPost<BaseRes>(url, data);
        }

    }
}
