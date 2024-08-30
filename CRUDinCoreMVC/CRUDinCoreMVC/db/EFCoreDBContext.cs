﻿using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace CRUDinCoreMVC.Models
{
    public class EFCoreDbContext : DbContext
    {
        //Constructor calling the Base DbContext Class Constructor
        public EFCoreDbContext(DbContextOptions<EFCoreDbContext> options) : base(options)
        {
        }

        //OnConfiguring() method is used to select and configure the data source
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Configure the ConnectionString and DbContext class
            builder.Services.AddDbContext<EFCoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("EFCoreDBConnection"));
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        //Adding Domain Classes as DbSet Properties
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}