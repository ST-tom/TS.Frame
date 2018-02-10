using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TS.Service.Files;

namespace TS.Web.Controllers
{
    public class CommonController : BaseController
    {
        // GET: Common
        public ActionResult Index()
        {
            return RedirectToAction("PageNotFound");
        }

        public ActionResult PageNotFound()
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;
            this.Response.ContentType = "text/html";

            return View();
        }

        #region Demo Excel

        public void Export()
        {
            var cellHeard = new Dictionary<string, string>();
            cellHeard.Add("test1", "Name");
            cellHeard.Add("test2", "Sex");
            cellHeard.Add("test3", "Name");
            XSSFWorkbook xk = new ExcelHelper().Export("sheet1", cellHeard, new List<dynamic>().AsQueryable());

            //写入到客户端
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                var fileName = "测试excel"; 
                if (HttpContext.Request.Browser.Browser == "IE")
                    fileName = HttpUtility.UrlEncode(fileName);

                xk.Write(ms);
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", fileName));
                Response.BinaryWrite(ms.ToArray());
            }
        }

        [HttpPost]
        public ActionResult Import()
        {
            var postFile = Request.Files[0];
            StringBuilder errmsg = new StringBuilder();
            if (postFile != null)
            {
                string exName = postFile.FileName.Split('.')[1];
                if (exName == ".xlsx")
                {
                    if (postFile.ContentLength / 1024 <= 1024 * 5) //5MB
                    {
                        //如果文件较大应该先保存在本地，再进行读写操作
                        XSSFWorkbook xb = new XSSFWorkbook(postFile.InputStream);
                        var list = new ExcelHelper().Import<dynamic>(xb, new Dictionary<string, string>(), out errmsg);
                    }
                    else
                    {
                        errmsg.Append("文件过大");
                    }
                }
                else
                {
                    errmsg.Append("文件格式有误");
                }
            }
            else
            {
                errmsg.Append("未上传文件");
            }

            return Json(new { result = errmsg.Length == 0, errmsg = errmsg });
        }

        #endregion
    }
}