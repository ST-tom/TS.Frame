using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TS.Service.Orders;
using TS.Web.Models.Orders;

namespace TS.Web.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Order
        public ActionResult Index()
        {
            return View("OrderList");
        }

        public ActionResult OrderList()
        {
            return View();
        }

        public ActionResult OrderPageList(int pageIndex,int pageSize,OrderListModel model)
        {
            var orders = new OrderService().GetPageOrders(pageIndex, pageSize, model.CustomerName);

            var list = orders.Select(order => {
                return new OrderModel()
                {
                    OrderId = order.Id,
                    CustomerName = order.CustomerName,
                    Price = Math.Round(order.Price, 2).ToString(),
                    CreateTime = order.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                };
            }).ToList();

            return Json(new { result = true , htmlStr = this.RenderPartialViewToString("_Order", list), totalCount = orders.TotalCount },JsonRequestBehavior.AllowGet);
        }
    }
}