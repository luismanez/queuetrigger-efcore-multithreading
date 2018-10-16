using System;
using System.Collections.Generic;
using System.Text;

namespace QueueTriggerEFCore.Models
{
    public class UserQueueMessage
    {
        public Guid CorrelationId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }  
    }
}
