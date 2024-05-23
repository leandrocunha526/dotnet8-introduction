using System.Threading.Tasks;
using dotnet8_introduction.Model;

namespace dotnet8_introduction.Data
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        Task<Product[]> GetAllProductsAsync(bool includeCategory);
        Task<Product> GetProductByIdAsync(int productId, bool includeCategory);

        Task<Category[]> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int categoryId);
    }
}
