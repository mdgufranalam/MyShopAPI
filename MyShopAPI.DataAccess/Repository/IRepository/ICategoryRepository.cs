using ShopAPI.Models;

namespace ShopAPI.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        //void CreateCategory(Category category);
        //IEnumerable<Category> GetCategory();
        //Category GetCategoryById(int id);
        //int DeleteCategory(int id);
        //Category Edit(int id, Category category);
        //void Save();
        public void Update(Category product);
    }
}