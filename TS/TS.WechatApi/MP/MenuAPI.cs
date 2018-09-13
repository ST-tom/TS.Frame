using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.WechatAPI.MP
{
    /// <summary>
    /// 自定义菜单接口
    /// </summary>
    public class MenuAPI : BaseAPI
    {
        public BaseRes DeleteMenu(string accessToken)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";
            url = string.Format(url, accessToken);
            return HttpGet<BaseRes>(url);
        }

        public BaseRes CreateMenu(string accessToken, MenuButton[] buttons)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
            url = string.Format(url, accessToken);
            string post = JsonConvert.SerializeObject(new { button = buttons });

            return HttpPost<BaseRes>(url, post);
        }

    }

    public class MenuButton
    {
        public const string click = "click";
        public const string view = "view";

        public string type { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string url { get; set; }
        public MenuSubButton[] sub_button { get; set; }
    }

    public class MenuSubButton
    {
        public string type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string key { get; set; }
    }
}
