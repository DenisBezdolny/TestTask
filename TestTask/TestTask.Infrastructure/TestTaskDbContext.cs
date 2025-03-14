﻿using Microsoft.EntityFrameworkCore;
using TestTask.Domain.Entities;
using TestTask.Infrastructure.Configurations;

namespace TestTask.Infrastructure
{
    // Represents the EF Core database context for the TestTask application.
    public class TestTaskDbContext : DbContext 
    {
        public DbSet<Employee?> Employees { get; set; }

        public TestTaskDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }
                
    }
}
