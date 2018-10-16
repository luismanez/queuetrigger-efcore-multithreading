using System;
using System.Collections.Generic;
using System.Text;

namespace QueueTriggerEFCore.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
}
