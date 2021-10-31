using Lendship.Backend.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Lendship.Backend.Models
{
    public class LendshipDbContext : IdentityDbContext<ApplicationUser>
    {
        public LendshipDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Advertisement> Advertisements { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Availability> Availabilites { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<EvaluationAdvertiser> EvaluationAdvertisers { get; set; }

        public DbSet<EvaluationLender> EvaluationLenders { get; set; }

        public DbSet<ClosedGroup> ClosedGroups { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Conversation> Conversation { get; set; }

        public DbSet<SavedAdvertisement> SavedAdvertisements { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Conversation>()
                .Property(e => e.UserIds)
                .HasConversion(
                    v => string.Join(',', v),
                    v => mapStringToGuids(v));

            modelBuilder.Entity<ClosedGroup>()
                .Property(e => e.UserIds)
                .HasConversion(
                    v => string.Join(',', v),
                    v => mapStringToGuids(v));
        }

        private List<Guid> mapStringToGuids(string v)
        {
            var result = new List<Guid>();
            var ids = v.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var id in ids)
            {
                result.Add(new Guid(id));
            }
            return result;
        }
    }
}
