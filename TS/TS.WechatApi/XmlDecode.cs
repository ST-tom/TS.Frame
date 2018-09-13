using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TS.WechatAPI
{
    public class XmlDecode
    {
        #region 解码微信推送来的消息
        /// <summary>
        /// 解码微信推送来的消息
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static MessageRecv DecodeXML(StreamReader reader)
        {
            XDocument doc = XDocument.Load(reader);
            XElement root = doc.Root;

            MessageRecv recv = new MessageRecv();
            recv.ToUserName = ReadElementString(root, "ToUserName");
            recv.FromUserName = ReadElementString(root, "FromUserName");
            recv.CreateTime = Convert.ToInt64(ReadElementString(root, "CreateTime"));
            recv.MsgType = ReadElementString(root, "MsgType");
            recv.Content = ReadElementString(root, "Content");
            recv.MediaId = ReadElementString(root, "MediaId");
            recv.PicUrl = ReadElementString(root, "PicUrl");
            recv.Format = ReadElementString(root, "Format");
            recv.ThumbMediaId = ReadElementString(root, "ThumbMediaId");
            recv.Location_X = ReadElementString(root, "Location_X");
            recv.Location_Y = ReadElementString(root, "Location_Y");
            var Scale = ReadElementString(root, "Scale");
            recv.Scale = string.IsNullOrEmpty(Scale) ? 0 : Convert.ToInt32(Scale);
            recv.Label = ReadElementString(root, "Label");
            recv.Event = ReadElementString(root, "Event");
            recv.Latitude = ReadElementString(root, "Latitude");
            recv.Longitude = ReadElementString(root, "Longitude");
            recv.Precision = ReadElementString(root, "Precision");
            recv.EventKey = ReadElementString(root, "EventKey");
            recv.MsgId = ReadElementString(root, "MsgId");
            try
            {
                recv.AgentID = Convert.ToInt32(ReadElementString(root, "AgentID"));
            }
            catch { }
            var ScanCodeInfoNode = root.Element("ScanCodeInfo");
            if (ScanCodeInfoNode != null)
            {
                var scanCodeInfo = new ScanCodeInfo();
                scanCodeInfo.ScanType = ReadElementString(ScanCodeInfoNode, "ScanType");
                scanCodeInfo.ScanResult = ReadElementString(ScanCodeInfoNode, "ScanResult");
                recv.ScanCodeInfo = scanCodeInfo;
            }

            var SendPicsInfoNode = root.Element("SendPicsInfo");
            if (SendPicsInfoNode != null)
            {
                var sendPicsInfo = new SendPicsInfo();
                sendPicsInfo.Count = Convert.ToInt32(ReadElementString(SendPicsInfoNode, "Count"));
                sendPicsInfo.PicList = new List<SendPicsInfoItem>();
                var PicListNode = SendPicsInfoNode.Element("PicList");
                foreach (var el in PicListNode.Elements("item"))
                {
                    var item = new SendPicsInfoItem() { PicMd5Sum = el.Element("PicMd5Sum").Value };
                    sendPicsInfo.PicList.Add(item);
                }
                recv.SendPicsInfo = sendPicsInfo;
            }

            var SendLocationInfoNode = root.Element("SendLocationInfo");
            if (SendLocationInfoNode != null)
            {
                var sendLocationInfo = new SendLocationInfo();
                sendLocationInfo = new SendLocationInfo();
                sendLocationInfo.Location_X = ReadElementString(SendLocationInfoNode, "Location_X");
                sendLocationInfo.Location_Y = ReadElementString(SendLocationInfoNode, "Location_Y");
                sendLocationInfo.Scale = Convert.ToInt32(ReadElementString(SendLocationInfoNode, "Scale"));
                sendLocationInfo.Label = ReadElementString(SendLocationInfoNode, "Label");
                sendLocationInfo.Poiname = ReadElementString(SendLocationInfoNode, "Poiname");
                recv.SendLocationInfo = sendLocationInfo;
            }

            var BatchJobNode = root.Element("BatchJob");
            if (BatchJobNode != null)
            {
                var batchJob = new BatchJob();
                batchJob.JobId = ReadElementString(BatchJobNode, "JobId");
                batchJob.JobType = ReadElementString(BatchJobNode, "JobType");
                batchJob.ErrCode = Convert.ToInt32(ReadElementString(BatchJobNode, "ErrCode"));
                batchJob.ErrMsg = ReadElementString(BatchJobNode, "ErrMsg");
                recv.BatchJob = batchJob;
            }

            return recv;
        }
        #endregion

        #region 解码微信推送来的消息(简报）
        /// <summary>
        /// 解码微信推送来的消息(简报）
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static MessageRecv DecodeXMLForBrief(string xml)
        {
            XDocument doc = XDocument.Load(new StringReader(xml));
            XElement root = doc.Root;

            MessageRecv recv = new MessageRecv();
            recv.ToUserName = ReadElementString(root, "ToUserName");
            recv.FromUserName = ReadElementString(root, "FromUserName");
            recv.CreateTime = Convert.ToInt64(ReadElementString(root, "CreateTime"));
            recv.MsgType = ReadElementString(root, "MsgType");
            recv.Event = ReadElementString(root, "Event");
            recv.MsgId = ReadElementString(root, "MsgId");

            return recv;
        }
        #endregion

        private static string ReadElementString(XElement root, string elementName)
        {
            var elementNode = root.Element(elementName);
            return elementNode == null ? string.Empty : elementNode.Value;
        }
    }

    public partial class MessageRecv
    {
        public DateTime CreateDate { get; set; }
        public string UserName { get; set; }
        public string FileName { get; set; }
        public string ThumbMediaFileName { get; set; }
        public int IsRead { get; set; }
        public int IsRecv { get; set; }
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public long? CreateTime { get; set; }
        public string MsgType { get; set; }
        public string Content { get; set; }
        public string MediaId { get; set; }
        public string PicUrl { get; set; }
        public string Format { get; set; }
        public string ThumbMediaId { get; set; }
        public string Location_X { get; set; }
        public string Location_Y { get; set; }
        public int? Scale { get; set; }
        public string Label { get; set; }
        public string Event { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Precision { get; set; }
        public string EventKey { get; set; }
        public string CScanCodeInfo { get; set; }
        public string CSendPicsInfo { get; set; }
        public string CSendLocationInfo { get; set; }
        public string CBatchJob { get; set; }
        public string MsgId { get; set; }
        public int AgentID { get; set; }
        public string UserAgent { get; set; }

        #region MsgType
        public const string text = "text";
        public const string image = "image";
        public const string voice = "voice";
        public const string video = "video";
        public const string file = "file";
        public const string shortvideo = "shortvideo";
        public const string location = "location";
        public const string _event = "event";
        #endregion

        #region Event
        public const string subscribe = "subscribe";
        public const string unsubscribe = "unsubscribe";
        public const string LOCATION = "LOCATION";
        public const string CLICK = "CLICK";
        public const string VIEW = "VIEW";
        public const string scancode_push = "scancode_push";
        public const string scancode_waitmsg = "scancode_waitmsg";
        public const string pic_sysphoto = "pic_sysphoto";
        public const string pic_photo_or_album = "pic_photo_or_album";
        public const string pic_weixin = "pic_weixin";
        public const string location_select = "location_select";
        public const string enter_agent = "enter_agent";
        public const string batch_job_result = "batch_job_result";
        public const string TEMPLATESENDJOBFINISH = "TEMPLATESENDJOBFINISH";
        #endregion

        #region JobType
        public const string sync_user = "sync_user";
        public const string replace_user = "replace_user";
        public const string invite_user = "invite_user";
        public const string replace_party = "replace_party";
        #endregion

        #region 扫码推事件的事件推送, 扫码推事件且弹出“消息接收中”提示框的事件推送
        public ScanCodeInfo ScanCodeInfo { get; set; }
        #endregion

        #region 弹出系统拍照发图的事件推送， 弹出拍照或者相册发图的事件推送， 弹出微信相册发图器的事件推送
        public SendPicsInfo SendPicsInfo { get; set; }
        #endregion

        #region 弹出地理位置选择器的事件推送
        public SendLocationInfo SendLocationInfo { get; set; }
        #endregion

        #region 异步任务完成事件推送
        public BatchJob BatchJob { get; set; }
        #endregion

        public string QyAppName { get; set; }

        #region 取得非事件消息简报
        /// <summary>
        /// 取得非事件消息简报
        /// </summary>
        /// <returns></returns>
        public string GetContent()
        {
            switch (this.MsgType)
            {
                case text:
                    return this.Content;
                case image:
                    return "[图片消息]";
                case voice:
                    return "[语音消息]";
                case video:
                    return "[视频消息]";
                case shortvideo:
                    return "[小视频消息]";
                case location:
                    return this.Label;
            }
            return "[未知消息]";
        }
        #endregion

    }

    /// <summary>
    /// 弹出系统拍照发图的事件推送
    /// </summary>
    public class SendPicsInfo
    {
        public int Count { get; set; }
        public List<SendPicsInfoItem> PicList { get; set; }
    }

    /// <summary>
    /// 图片列表元素
    /// </summary>
    public class SendPicsInfoItem
    {
        public string PicMd5Sum { get; set; }
    }

    /// <summary>
    /// 扫码消息
    /// </summary>
    public class ScanCodeInfo
    {
        public string ScanType { get; set; }
        public string ScanResult { get; set; }
    }

    /// <summary>
    /// 弹出地理位置选择器的事件推送
    /// </summary>
    public class SendLocationInfo
    {
        public string Location_X { get; set; }
        public string Location_Y { get; set; }
        public int Scale { get; set; }
        public string Label { get; set; }
        public string Poiname { get; set; }
    }

    /// <summary>
    /// 异步任务完成事件推送
    /// </summary>
    public class BatchJob
    {
        public string JobId { get; set; }
        public string JobType { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }
    }
}
