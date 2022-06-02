using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAPI.Models
{
    public class UpdateOrderHeader
    {
        public int Id { get; set; }
        public string OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
    }
}
