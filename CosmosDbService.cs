using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading.Tasks;


namespace EmployeeManagementSystem
{
    public static class CosmosDbService
    {
        private static readonly string EndpointUri = Environment.GetEnvironmentVariable("CosmosDBEndpointUri");
        private static readonly string PrimaryKey = Environment.GetEnvironmentVariable("CosmosDBPrimaryKey");
        private static CosmosClient cosmosClient;
        private static Database database;
        private static Microsoft.Azure.Cosmos.Container container;

        static CosmosDbService()
        {
            cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            database = cosmosClient.GetDatabase(Environment.GetEnvironmentVariable("CosmosDBDatabaseName"));
            container = database.GetContainer(Environment.GetEnvironmentVariable("CosmosDBContainerName"));
        }

        public static async Task AddEmployeeAsync(Employee employee)
        {
            // Ensure PartitionKey is set correctly
            if (string.IsNullOrEmpty(employee.PartitionKey))
            {
                employee.PartitionKey = employee.Id; // Use Id or other appropriate value for partition key
            }

            await container.CreateItemAsync(employee, new PartitionKey(employee.PartitionKey));
        }

        public static async Task<Employee> GetEmployeeAsync(string id)
        {
            try
            {
                // Assuming id is used as partition key here; adjust if needed
                ItemResponse<Employee> response = await container.ReadItemAsync<Employee>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}