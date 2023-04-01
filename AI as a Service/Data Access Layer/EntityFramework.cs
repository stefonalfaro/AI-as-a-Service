using Microsoft.EntityFrameworkCore;
using AI_as_a_Service.Models;
using AI_as_a_Service.Services.Interfaces;
using static AI_as_a_Service.Models.ChatCompletions;

namespace AI_as_a_Service.Data
{
    //You can easily perform CRUD operations using the injected ApplicationDbContext instance in your controllers. Entity Framework Core provides a set of methods to interact with the database, such as Add, Update, Remove, Find, ToListAsync, etc.
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Plan> Plans { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FineTuning> FineTuning { get; set; }
        public DbSet<Payment> Payments { get; set; }

        // Add other DbSet properties for your models here
    }

    //If you want to implement a generic repository pattern for easy CRUD operations, you can create a Repository class that accepts a generic type and provides common data access methods:
    public class SQLServerRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public SQLServerRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbSet.FindAsync(id) != null;
        }

        public Task<TEntity> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
