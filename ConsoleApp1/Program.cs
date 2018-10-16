using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using QueueTriggerEFCore.Domain;
using QueueTriggerEFCore.Models;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // This console app simulates an Scheduled Job processes running every 10 minutes,
            // and calling an external service asking for all the users we need to Synchronise
            // this external service can return the same user more than once in the same call
            // For each returned user, we queued a message in the queue, meaning
            // "hey, someone has work to do with User GUID XYZ..."

            // (it´s not the real scenario, but just for simplifying the test.
            // in the real scenario, the User entity contains children entities, i.e: Projects
            // so a User can have different projects, and the same project can be in different users
            // so we can have the same Project in different Messages in the Queue.)

            Console.WriteLine("Feeding demo data in Queue...");

            var correlationId = Guid.NewGuid();

            var users = new List<UserQueueMessage>
            {
                new UserQueueMessage
                {
                    Name = $"Peter",
                    Surname = "Parker",
                    Id = Guid.Parse("ff950f24-3b85-491f-a90f-125b9da15e91"),
                    CorrelationId = correlationId
                },
                new UserQueueMessage
                {
                    Name = $"Bruce",
                    Surname = "Wagner",
                    Id = Guid.Parse("23ed486e-48af-4b52-89a8-6f3c3284c270"),
                    CorrelationId = correlationId
                },
                new UserQueueMessage
                {
                    Name = $"Dr",
                    Surname = "Strange",
                    Id = Guid.Parse("b21481e0-d476-4519-88bd-5dc6958ab09d"),
                    CorrelationId = correlationId
                },
                new UserQueueMessage
                {
                    Name = $"Falcon",
                    Surname = "Eye",
                    Id = Guid.Parse("cbf78f0e-ec85-4a3f-99f5-596567e5b75b"),
                    CorrelationId = correlationId
                }
            }.ToArray();

            var storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            var queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference("user-items");
            queue.CreateIfNotExistsAsync().GetAwaiter().GetResult();


            for (var i = 0; i < 100; i++)
            {
                var random = new Random();
                var randomPosition = random.Next(4);
                var randomUserFromPool = users[randomPosition];

                var message = new CloudQueueMessage(JsonConvert.SerializeObject(randomUserFromPool));
                queue.AddMessageAsync(message).GetAwaiter().GetResult();
            }   
             
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
