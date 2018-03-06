using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TS.Web.Models.Orders
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Price { get; set; }
        public string CreateTime { get; set; }
    }
}