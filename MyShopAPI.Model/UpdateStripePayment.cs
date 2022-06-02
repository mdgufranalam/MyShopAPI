using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAPI.Models
{
    public class UpdateStripePayment
    {
        public int id { get; set; }
        public string session { get; set; }
        public string paymentItentId { get; set; }
    }
}
