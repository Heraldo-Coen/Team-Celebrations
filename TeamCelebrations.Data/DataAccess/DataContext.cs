using Microsoft.EntityFrameworkCore;
using TeamCelebrations.Data.Entities;

namespace TeamCelebrations.Data.DataAccess
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<Employee>? Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employee
            modelBuilder.Entity<Employee>(builder =>
            {
                builder.HasIndex(s => new
                {
                    s.FirstName,
                    s.LastName,
                }).IsUnique();

                builder.HasIndex(e => e.Email).IsUnique();

                builder.HasIndex(e => e.PhoneNumber).IsUnique();
            });
        }
    }
}