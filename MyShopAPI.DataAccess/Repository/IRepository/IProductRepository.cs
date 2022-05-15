using ShopAPI.Models;

namespace ShopAPI.DataAccess.Repository.IRepository
{
    //public interface IProductRepository
    //{
    //    void CreateProduct(Product product);
    //    int DeleteProduct(int id);
    //    Product Edit(int id, Product product);
    //    IEnumerable<Product> GetProduct();
    //    Product GetProductById(int id);
    //    void Save();

        
    //}
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
    }
}