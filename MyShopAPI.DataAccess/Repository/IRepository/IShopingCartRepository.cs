using ShopAPI.Models;

namespace ShopAPI.DataAccess.Repository.IRepository
{
    public interface IShopingCartRepository : IRepository<ShoppingCart>
    {
        int DecrementCount(ShoppingCart shoppingCart, int count);


        int IncrementCount(ShoppingCart shoppingCart, int count);
    }
}