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

        public async Task<Category[]> GetAllCategoriesAsync(bool includeProducts)
        {
            IQueryable<Category> query = _context.Categories;
            if (includeProducts)
            {
                query = query.Include(c => c.Products);
            }
            query = query.AsNoTracking().OrderBy(c => c.Id);
            return await query.ToArrayAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(bool includeCategory, int? limit)
        {
            IQueryable<Product> query = _context.Products;
            if (includeCategory)
            {
                query = query.Include(p => p.Category);
            }
            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }
            query = query.AsNoTracking().OrderBy(p => p.Id);
            return await query.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            IQueryable<Category> query = _context.Categories;
            query = query.AsNoTracking().OrderBy(c => c.Id).Where(c => c.Id == categoryId);
#pragma warning disable CS8603
            return await query.FirstOrDefaultAsync();
#pragma warning restore CS8603
        }

        public async Task<Product> GetProductByIdAsync(int productId, bool includeCategory)
        {
            IQueryable<Product> query = _context.Products;
            if (includeCategory)
            {
                query = query.Include(p => p.Category);
            }
            query = query.AsNoTracking().OrderBy(p => p.Id).Where(p => p.Id == productId);
#pragma warning disable CS8603
            return await query.FirstOrDefaultAsync();
#pragma warning restore CS8603
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public async Task<float> GetAveragePriceAsync()
        {
            if (!await _context.Products.AnyAsync())
            {
                return 0; // Return 0 if not found any product
            }
            return await _context.Products.AverageAsync(p => p.Price);
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm, bool includeCategory)
        {
            IQueryable<Product> query = _context.Products;

            if (includeCategory)
            {
                query = query.Include(p => p.Category);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Name!.Contains(searchTerm));
            }

            return await query.ToListAsync();
        }
    }
}
