using Microsoft.EntityFrameworkCore;
using TeamCelebrations.Data.Entities;

namespace TeamCelebrations.Data.DataAccess
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<PhoneCode> PhoneCodes { get; set; }
        public DbSet<Administrator>? Administrators { get; set; }
        public DbSet<Unit>? Units { get; set; }
        public DbSet<Employee>? Employees { get; set; }
        public DbSet<Friendship>? Friendships {get; set; }
        public DbSet<Event>? Events { get; set; }
        public DbSet<Message>? Messages { get; set; }
        public DbSet<Notification>? Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Phone Code
            modelBuilder.Entity<PhoneCode>(builder => {
                builder.HasIndex(p => p.Code).IsUnique();

                builder.HasIndex(p => p.CountryName).IsUnique();

                builder.HasIndex(p => p.CountryCode).IsUnique();
            });

            // Administrator
            modelBuilder.Entity<Administrator>(builder => {
                builder.HasIndex(a => new {
                    a.FirstName,
                    a.LastName,
                }).IsUnique();

                builder.HasIndex(a => a.Email).IsUnique();
            });

            // Unit
            modelBuilder.Entity<Unit>(builder => {
                builder.HasIndex(u => u.Name).IsUnique();

                builder.HasIndex(u => u.Acronym).IsUnique();
            });

            // Employee
            modelBuilder.Entity<Employee>(builder =>
            {
                builder.HasIndex(p => p.DNI).IsUnique();    

                builder.HasIndex(e => new
                {
                    e.FirstName,
                    e.LastName,
                }).IsUnique();

                builder.HasIndex(e => e.Email).IsUnique();

                builder.HasIndex(e => new
                {
                    e.PhoneCodeId,
                    e.PhoneNumber,
                }).IsUnique();
            });

            // Friendship
            modelBuilder.Entity<Friendship>(builder => {
                builder.HasIndex(f => new
                {
                    f.EmployeeId1,
                    f.EmployeeId2,
                }).IsUnique();
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
Add-Migration AddAcronymToUnitAndRequired -Project TeamCelebrations.Data -StartupProject TeamCelebrations.WebAPI
Update-Database -Project TeamCelebrations.Data -StartupProject TeamCelebrations.WebAPI
"password": "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg="
"email": "rulbricht@teamcelebrations.com",
*/