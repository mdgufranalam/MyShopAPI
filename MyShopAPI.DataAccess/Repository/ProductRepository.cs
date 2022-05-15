using Microsoft.EntityFrameworkCore;
using ShopAPI.DataAccess.Data;
using ShopAPI.DataAccess.Repository.IRepository;
using ShopAPI.Models;

namespace ShopAPI.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        #region Commented
        //public void CreateProduct(Product product)
        //{
        //    try
        //    {

        //        if (_context.Products.Where(c => c.Title.ToLower() == product.Title.ToLower()).Any())
        //        {
        //            throw new Exception("Product Already Exist.");
        //        }
        //        _context.Products.Add(product);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //public Product Edit(int id, Product product)
        //{
        //    try
        //    {
        //        var _item = _context.Products.Find(id);
        //        if (_item == null)
        //        {
        //            return _item;
        //        }
        //        else
        //        {
        //            _item.Title = product.Title;
        //            _item.Description = product.Description;
        //            _item.ListPrice = product.ListPrice;
        //            _item.Price = product.Price;
        //            _item.Price50 = product.Price50;
        //            _item.ImageUrl = product.ImageUrl;
        //            _item.LastUpdateDate = DateTime.Now;
        //            _item.CategoryId = product.CategoryId;
        //            // _item.Category.Id = product.CategoryId;
        //            return _item;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //public int DeleteProduct(int id)
        //{
        //    try
        //    {
        //        var item = _context.Products.Find(id);
        //        if (item == null)
        //        {
        //            return 0;
        //        }
        //        else
        //        {
        //            _context.Products.Remove(item);
        //            return item.Id;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}      

        //public IEnumerable<Product> GetProduct()
        //{
        //    try
        //    {
        //        return _context.Products.Include("Category"); ;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public Product GetProductById(int id)
        //{
        //    try
        //    {
        //        var product = _context.Products.FirstOrDefault(c => c.Id == id);
        //        return product;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //public void Save()
        //{
        //    _context.SaveChanges();
        //}
        #endregion

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Update(Product product)
        {
            try
            {
                var _item = _context.Products.Find(product.Id);
                if (_item == null)
                {
                    throw new Exception("Product not found.");
                }
                else
                {
                    _item.Title = product.Title;
                    _item.Description = product.Description;
                    _item.ListPrice = product.ListPrice;
                    _item.Price = product.Price;                   
                    _item.ImageUrl = product.ImageUrl;
                    _item.LastUpdateDate = DateTime.Now;
                    _item.CategoryId = product.CategoryId;
                    // _item.Category.Id = product.CategoryId;
                    //return _item;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
