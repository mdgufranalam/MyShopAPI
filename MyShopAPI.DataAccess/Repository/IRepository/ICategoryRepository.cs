using MyShopAPI.Models;

namespace MyShopAPI.DataAccess.Repository
{
    public interface ICategoryRepository
    {
        void CreateCategory(Category category);
        IEnumerable<Category> GetCategory();
        Category GetCategoryById(int id);
        int DeleteCategory(int id);
        Category Edit(int id, Category category);
        void Save();
    }
}