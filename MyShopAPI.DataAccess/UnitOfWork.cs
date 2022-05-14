using MyShopAPI.DataAccess.Data;
using MyShopAPI.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopAPI.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
        }
        public ICategoryRepository Category { get; private set; }




        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
