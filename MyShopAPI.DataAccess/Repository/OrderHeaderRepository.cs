using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopAPI.DataAccess.Data;
using ShopAPI.DataAccess.Repository.IRepository;
using ShopAPI.Models;

namespace ShopAPI.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        ApplicationDbContext _context;

        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _context.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (paymentStatus != null)
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }
        public void Update(OrderHeader obj)
        {
            
            _context.OrderHeaders.Update(obj);
        }

        public void UpdateStripePaymentID(int id, string sessionId, string paymentItentId)
        {
            var orderFromDb = _context.OrderHeaders.FirstOrDefault(u => u.Id == id);
            orderFromDb.PaymentDate = DateTime.Now;
            orderFromDb.SessionId = sessionId;
            orderFromDb.PaymentIntentId = paymentItentId;
        }


    }
}
