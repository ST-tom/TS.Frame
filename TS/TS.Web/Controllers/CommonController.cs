using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}