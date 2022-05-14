using MyShopAPI.DataAccess.Repository;

namespace MyShopAPI.DataAccess
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }

        void Save();
    }
}