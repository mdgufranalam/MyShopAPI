using ShopAPI.DataAccess.Repository.IRepository;

namespace ShopAPI.DataAccess
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }

        void Save();
    }
}