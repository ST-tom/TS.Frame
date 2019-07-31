using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Core;
using TS.Core.Domain.Orders;

namespace TS.Service.Orders
{
    public interface IOrderService
    {
        /// <summary>
        /// 获取订单分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="customerName"></param>
        /// <returns></returns>
        PagedList<Order> GetPageOrders(int pageIndex, int pageSize, string customerName);
    }
}
