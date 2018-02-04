using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using TS.Core.Log;
using TS.Service.Https;

namespace TS.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            LogException(exception);

            //process 404 HTTP errors
            var httpException = exception as HttpException;
            if (httpException != null)                   
            {
                if (httpException.GetHttpCode() == 404)
                {
                    if (!HttpErrorHelper.IsStaticResource(this.Request))
                    {
                        Server.ClearError();
                        Response.TrySkipIisCustomErrors = true;

                        Response.Redirect("~/Common/PageNotFound", true);
                    }
                }   
            }

        }

        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;

            //ignore 404 HTTP errors
            var httpException = exc as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
                return;

            try
            {
                LogHelper.Error(exc.Message, exc);
            }
            catch (Exception)
            {
                //don't throw new exception if occurs
            }
        }
    }
}