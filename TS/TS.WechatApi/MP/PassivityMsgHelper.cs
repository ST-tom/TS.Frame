using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.WechatAPI.MP
{
    /// <summary>
    /// 被动回复工具类
    /// </summary>
    public class PassivityMsgHelper
    {
        /// <summary>
        /// 被动回复图文消息
        /// </summary>
        /// <param name="recv"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string SendNewsMessage(MessageRecv recv, List<WX_NewsMsg> list)
        {
            string res = string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>" + list.Count.ToString() + "</ArticleCount><Articles>",
            recv.FromUserName, recv.ToUserName, DateTime.Now.ToShortTimeString());

            foreach (WX_NewsMsg news in list)
            {
                res += "<item>"
                    + "<Title><![CDATA[" + news.Title + "]]></Title>"
                    + "<Description><![CDATA[" + news.Description + "]]></Description>"
                    + "<PicUrl><![CDATA[" + news.PicUrl + "]]></PicUrl>"
                    + "<Url><![CDATA[" + news.Url + "]]></Url>"
                    + "</item>";

            }

            res += "</Articles></xml>";

            return res;
        }

        /// <summary>
        /// 被动回复文本消息
        /// </summary>
        /// <param name="recv"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string SendTextMessage(MessageRecv recv,string content)
        {
            return string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[{3}]]></Content></xml>",
           recv.FromUserName, recv.ToUserName, DateTime.Now.ToShortTimeString(),content);
        }

        /// <summary>
        /// 被动回复转发给客服
        /// </summary>
        /// <param name="recv"></param>
        /// <returns></returns>
        public static string TranferToCustomerService(MessageRecv recv)
        {
            return string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>", recv.FromUserName, recv.ToUserName, DateTime.Now.ToShortTimeString());
        }

        /// <summary>
        /// 被动回复返回错误提示
        /// </summary>
        /// <param name="recv"></param>
        /// <returns></returns>
        public static string SendErrMessage(MessageRecv recv)
        {
            return string.Format("<xml> <ToUserName>< ![CDATA[{0}] ]></ToUserName> <FromUserName>< ![CDATA[{1}] ]></FromUserName> <CreateTime>{2}</CreateTime> <MsgType>< ![CDATA[text] ]></MsgType> <Content>< ![CDATA[{3}] ]></Content> </xml>",
         recv.FromUserName, recv.ToUserName, DateTime.Now.ToShortTimeString(), "非常抱歉，系统出错请稍后尝试");
        }
    }

    /// <summary>
    /// 被动回复图文消息
    /// </summary>
    public class WX_NewsMsg
    {
        public string Title;
        public string Description;
        public string PicUrl;
        public string Url;
    }
}
