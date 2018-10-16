using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using QueueTriggerEFCore.Domain;

namespace QueueTriggerEFCore.Data
{
    public sealed class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {

        }

        public UsersContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(
                        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=UsersSynced;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }
    }
}
