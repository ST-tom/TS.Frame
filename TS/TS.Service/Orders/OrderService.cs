using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Core;
using TS.Core.Domain.Orders;
using TS.Data;

namespace TS.Service.Orders
{
    public class OrderService : BaseService
    {
        public OrderService() : base()
        {

        }

        public PagedList<Order> GetPageOrders(int pageIndex,int pageSize,string customerName)
        {
            var lamda = EFRepository.ExpressionTrue<Order>();

            if (!string.IsNullOrWhiteSpace(customerName))
            {
                lamda = lamda.And(e => e.CustomerName.Contains(customerName));
            }

            return EFRepository.GetPagedData(pageIndex, pageSize, lamda, e => e.CreateTime);
        }
    }
}
