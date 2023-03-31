using AI_as_a_Service.Data;
using System.Collections.Concurrent;
using System.ComponentModel;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AI_as_a_Service.Services.Interfaces;
using Container = Microsoft.Azure.Cosmos.Container;

namespace AI_as_a_Service.Helpers
{
    public class CosmosDbSettings
    {
        public string AccountEndpoint { get; set; }
        public string AccountKey { get; set; }
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
    }

    public class CosmosDbRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public CosmosDbRepository(CosmosDbSettings settings)
        {
            _cosmosClient = new CosmosClient(settings.AccountEndpoint, settings.AccountKey);
            _container = _cosmosClient.GetContainer(settings.DatabaseName, settings.ContainerName);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var query = new QueryDefinition("SELECT * FROM c");
            var queryIterator = _container.GetItemQueryIterator<TEntity>(query);

            List<TEntity> results = new List<TEntity>();
            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        //public async Task<TEntity> GetByIdAsync(Guid id)
        //{
        //    try
        //    {
        //        ItemResponse<TEntity> response = await _container.ReadItemAsync<TEntity>(id.ToString(), new PartitionKey(id.ToString()));
        //        return response.Value;
        //    }
        //    catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        //    {
        //        return null;
        //    }
        //}

        public async Task AddAsync(TEntity entity)
        {
            await _container.CreateItemAsync(entity, new PartitionKey(Guid.NewGuid().ToString()));
        }

        public async Task UpdateAsync(Guid id, TEntity entity)
        {
            await _container.ReplaceItemAsync(entity, id.ToString(), new PartitionKey(id.ToString()));
        }

        public async Task DeleteAsync(Guid id)
        {
            await _container.DeleteItemAsync<TEntity>(id.ToString(), new PartitionKey(id.ToString()));
        }

        Task<TEntity> IRepository<TEntity>.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task IRepository<TEntity>.UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        Task<TEntity> IRepository<TEntity>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task IRepository<TEntity>.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
