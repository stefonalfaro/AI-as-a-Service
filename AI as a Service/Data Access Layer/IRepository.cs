namespace AI_as_a_Service.Services.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(Guid id); //CosmosDB uses Guids
        Task<TEntity> GetByIdAsync(int id); //SQL Server uses Integers
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id); //CosmosDB uses Guids
        Task DeleteAsync(int id); //SQL Server uses Integers
    }
}
