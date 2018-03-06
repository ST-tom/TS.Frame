using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Core.Domain.Orders
{
    [Table("Order")]
    public partial class Order : BaseEntity
    {
        public string CustomerName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
