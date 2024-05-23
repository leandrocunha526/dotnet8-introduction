using dotnet8_introduction.Model;
using Microsoft.EntityFrameworkCore;

namespace dotnet8_introduction.Data
{
    public class Repository : IRepository
    {
        public DataContext _context { get; }
        public Repository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Category[]> GetAllCategoriesAsync()
        {
            IQueryable<Category> query = _context.Categories;
            query = query.AsNoTracking().OrderBy(c => c.Id);
            return await query.ToArrayAsync();
        }

        public async Task<Product[]> GetAllProductsAsync(bool includeCategory)
        {
            IQueryable<Product> query = _context.Products;
            if (includeCategory)
            {
                query = query.Include(p => p.category);
            }
            query = query.AsNoTracking().OrderBy(p => p.Id);
            return await query.ToArrayAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            IQueryable<Category> query = _context.Categories;
            query = query.AsNoTracking().OrderBy(c => c.Id).Where(c => c.Id == categoryId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId, bool includeCategory)
        {
            IQueryable<Product> query = _context.Products;
            if (includeCategory)
            {
                query = query.Include(p => p.category);
            }
            query = query.AsNoTracking().OrderBy(p => p.Id).Where(p => p.Id == productId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
    }
}