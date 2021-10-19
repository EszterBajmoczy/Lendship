using Lendship.Backend.Authentication;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Lendship.Backend.Models
{
    public class DbContext : IdentityDbContext<ApplicationUser>
    {
        public DbContext() : base("LendshipMaster") { }

        public DbSet<Advertisement> Advertisements { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Availability> Availabilites { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<EvaluationAdvertiser> EvaluationAdvertisers { get; set; }

        public DbSet<EvaluationLender> EvaluationLenders { get; set; }

        public DbSet<ClosedGroup> ClosedGroups { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Conversation> Conversation { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Advertisement>()
                    .HasRequired<ApplicationUser>(s => s.User)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Availability>()
                    .HasRequired<Advertisement>(s => s.Advertisement)
                    .WithMany()
                    .WillCascadeOnDelete(false);
            
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Reservation>()
                    .HasRequired<Advertisement>(s => s.Advertisement)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reservation>()
                    .HasRequired<ApplicationUser>(s => s.User)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<EvaluationAdvertiser>()
                    .HasRequired<Advertisement>(s => s.Advertisement)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<EvaluationAdvertiser>()
                    .HasRequired<ApplicationUser>(s => s.UserFrom)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<EvaluationAdvertiser>()
                    .HasRequired<ApplicationUser>(s => s.UserTo)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<EvaluationLender>()
                    .HasRequired<Advertisement>(s => s.Advertisement)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<EvaluationLender>()
                    .HasRequired<ApplicationUser>(s => s.UserFrom)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<EvaluationLender>()
                    .HasRequired<ApplicationUser>(s => s.UserTo)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClosedGroup>()
                    .HasRequired<Advertisement>(s => s.Advertisement)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                    .HasRequired<ApplicationUser>(s => s.UserFrom)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Conversation>()
                    .HasRequired<Advertisement>(s => s.Advertisement)
                    .WithMany()
                    .WillCascadeOnDelete(false);

        }
    }
}
