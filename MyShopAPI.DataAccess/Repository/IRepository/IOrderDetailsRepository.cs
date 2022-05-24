using ShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAPI.DataAccess.Repository.IRepository
{
    public interface IOrderDetailsRepository: IRepository<OrderDetail>
    {
        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
    }
}
