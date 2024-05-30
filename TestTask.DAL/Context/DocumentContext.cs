using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Domain.Models;

namespace TestTask.DAL.Context
{
    public class DocumentContext : DbContext
    {
        public DocumentContext()
        {
            
        }
        public DocumentContext(DbContextOptions<DocumentContext> options) :base(options)
        {

        }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentStatus> DocumentStatuses { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                
            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = 1, Name = "CREATED" },
                new Status { StatusId = 2, Name = "DELETED" }
            );
        }
    }
}
