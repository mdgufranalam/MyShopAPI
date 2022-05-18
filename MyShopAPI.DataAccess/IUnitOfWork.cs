using ShopAPI.DataAccess.Repository;
using ShopAPI.DataAccess.Repository.IRepository;

namespace ShopAPI.DataAccess
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IShopingCartRepository ShoppingCart { get; }
        void Save();
    }
}