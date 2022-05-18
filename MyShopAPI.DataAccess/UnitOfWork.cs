using ShopAPI.DataAccess.Data;
using ShopAPI.DataAccess.Repository;
using ShopAPI.DataAccess.Repository.IRepository;
using ShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAPI.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
            ShoppingCart = new ShopingCartRepository(context);
        }
        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; } 
        public IShopingCartRepository ShoppingCart { get; private set; } 

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
