using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TS.Core.Log;

namespace TS.Web.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class HttpExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public virtual void OnException(ExceptionContext filterContext)
        {
            //IIS管理的应用，异常将返回ErrorPage页面，所以添加过滤器对ajax请求特殊处理，返回500
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
      
                LogHelper.Error("ajax请求错误", filterContext.Exception);

                //filterContext.Result = new JsonResult
                //{
                //    Data = new { result = false, errmsg = filterContext.Exception.Message },
                //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                //};

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.StatusCode = 500;
            }
        }
    }
}