using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoughtWorks.QRCode.Codec;

namespace TS.Data.Helper
{
    public class QRCodeHelper
    {
        /// <summary>
        /// 生成二维码,jpg格式
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public string GetQRCode(string url, string savePath, string fileName = "", bool isNamedByDate = false)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 4;
            qrCodeEncoder.QRCodeVersion = 8;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            Image image = qrCodeEncoder.Encode(url);

            savePath = System.Web.Hosting.HostingEnvironment.MapPath(savePath);
            fileName = !isNamedByDate && !string.IsNullOrWhiteSpace(fileName) ? $@"/{fileName}.jpg" : $@"/{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.jpg";
            string filePath = $"{savePath}{fileName}";

            FileStream fs;
            if (System.IO.File.Exists(filePath))
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }
            else
            {
                fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
            image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
            fs.Close();
            image.Dispose();

            return $"{savePath}{fileName}";
        }
    }
}
