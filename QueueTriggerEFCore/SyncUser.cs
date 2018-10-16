using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QueueTriggerEFCore.Data;
using QueueTriggerEFCore.Domain;
using QueueTriggerEFCore.Models;

namespace QueueTriggerEFCore
{
    public static class SyncUser
    {

        [FunctionName("SyncUser")]
        public static async Task Run([QueueTrigger("user-items", Connection = "")]UserQueueMessage userQueueMessage, ILogger log)
        {
            log.LogInformation($"SyncUser started. User Id: {userQueueMessage.Id}");

            // Here we´ll do different calls to external services / APIs to get all the required info
            // and do an aggregate in our User entity
            // User PK is a Guid that is coming from one of the external services

            var user = new User
            {
                Id = userQueueMessage.Id,
                DisplayName = $"{userQueueMessage.Name} {userQueueMessage.Surname}"
            };

            // Here we need to persist the aggregated User data into a SQL Db
            // As this is a QueueTrigger Azure function, different processes are running in parallel
            // so we have a multi threading problem with EF DbContext, and is easy to get a 
            // PK violation exception, as we can have different messages in the queue for the same User id
            // so different Function instances can run at the same time processing the same User so:
            // Timestamp 1: Function Instance 1: Check if User GUID 10 is in DB -> is not
            // Timestamp 1: Function Instance 2: Check if User GUID 10 is in DB -> is not
            // Timestamp 2: Function Instance 1: Insert User GUID 10 in DB
            // Timestamp 3: Function Instance 2: Tries to insert User ID 10 in DB -> PK Violation exception

            using (var db = new UsersContext())
            {
                var userInDb = await db.Users.FirstOrDefaultAsync(u => u.Id.Equals(userQueueMessage.Id));
                if (userInDb == null)
                {
                    db.Users.Add(user);
                }
                else
                {
                    userInDb.DisplayName = user.DisplayName;
                    db.Users.Update(userInDb);
                }

                try
                {
                    db.SaveChanges();
                    log.LogInformation($"SyncUser done. User Id: {userQueueMessage.Id}");
                }
                catch (Exception ex)
                {
                    log.LogError(ex, "================== ERROR ================");
                }
            }            
        }
    }
}
