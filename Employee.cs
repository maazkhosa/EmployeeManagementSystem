using System;
using Newtonsoft.Json;

namespace EmployeeManagementSystem
{
    public class Person
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Id: {Id}, Name: {Name}, Address: {Address}");
        }
    }

    public class Employee : Person
    {
        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("salary")]
        public double Salary { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Position: {Position}, Salary: {Salary}, Department: {Department}, PartitionKey: {PartitionKey}");
        }
    }
}
