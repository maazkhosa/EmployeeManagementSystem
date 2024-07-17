using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;

namespace EmployeeManagementSystem
{
    public static class EmployeeFunctions
    {
        [FunctionName("CreateEmployee")]
        public static async Task<IActionResult> CreateEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "employee")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Creating a new employee.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var employee = JsonConvert.DeserializeObject<Employee>(requestBody);

            // Ensure PartitionKey is set correctly
            if (string.IsNullOrEmpty(employee.PartitionKey))
            {
                employee.PartitionKey = employee.Id; // Set partition key if missing
            }

            await CosmosDbService.AddEmployeeAsync(employee);

            return new OkObjectResult(employee);
        }

        [FunctionName("GetEmployee")]
        public static async Task<IActionResult> GetEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "employee/{id}")] HttpRequest req,
            string id,
            ILogger log)
        {
            log.LogInformation($"Getting employee with id: {id}");

            var employee = await CosmosDbService.GetEmployeeAsync(id);
            if (employee == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(employee);
        }
    }
}