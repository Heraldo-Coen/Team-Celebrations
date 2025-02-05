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
                builder.HasIndex(e => new
                {
                    e.FirstName,
                    e.LastName,
                }).IsUnique();

                builder.HasIndex(e => e.Email).IsUnique();

                builder.HasIndex(e => e.PhoneNumber).IsUnique();
            });

            // Event
            modelBuilder.Entity<Event>(builder =>
            {
                builder.HasIndex(e => e.Title).IsUnique();
            });

            // Message
            modelBuilder.Entity<Message>(builder =>
            {
                builder.HasIndex(m => new
                {
                    m.SenderId,
                    m.RecipientId,
                    m.EventId,
                }).IsUnique();
            });

            // Notification
            modelBuilder.Entity<Notification>(builder =>
            {
            });
        }
    }
}

/*
Add-Migration UpdateEmployee -Project TeamCelebrations.Data -StartupProject TeamCelebrations.WebAPI
Update-Database -Project TeamCelebrations.Data -StartupProject TeamCelebrations.WebAPI
*/