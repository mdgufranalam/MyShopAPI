using ShopAPI.DataAccess.Data;
using ShopAPI.DataAccess.Repository.IRepository;
using ShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAPI.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        //public void CreateCategory(Category category)
        //{
        //    try
        //    {
                
        //        if(_context.Categories.Where(c => c.Name.ToLower() == category.Name.ToLower()).Any())
        //        {
        //            throw new Exception("Category Already Exist.");
        //        }
        //        _context.Categories.Add(category);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //public Category Edit(int id, Category category)
        //{
        //    try
        //    {
        //        var _item=_context.Categories.Find(id);
        //        if (_item == null)
        //        {
        //            return _item;
        //        }
        //        else
        //        {
        //            _item.Name = category.Name; 
        //            _item.Description = category.Description; 
        //            return _item;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //public int DeleteCategory(int id)
        //{
        //    try
        //    {
        //        var item= _context.Categories.Find(id);
        //        if(item == null)
        //        {
        //            return 0;
        //        }
        //        else
        //        {
        //            _context.Categories.Remove(item); 
        //            return item.Id;
        //        }
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

        //public IEnumerable<Category> GetCategory()
        //{
        //    try
        //    {
        //        return _context.Categories;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public Category GetCategoryById(int id)
        //{
        //    try
        //    {
        //        var category = _context.Categories.FirstOrDefault(c => c.Id == id);                
        //        return category;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public void Update(Category category)
        {
            try
            {
                var _item = _context.Categories.Find(category.Id);
                if (_item == null)
                {
                    throw new Exception("Product not found.");
                }
                else
                {
                    _item.Name = category.Name;
                    _item.Description = category.Description;                 
                    // _item.Category.Id = product.CategoryId;
                    //return _item;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

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
        
    }
}
