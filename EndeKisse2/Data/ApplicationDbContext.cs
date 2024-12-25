using EndeKissie.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EndeKisse2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<ImageStore> ImageStore { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<ProjectStatus> ProjectStatus { get; set; }
        public DbSet<Progress> Progress { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //// One-to-Many: ApplicationUser → Project
            //modelBuilder.Entity<Project>()
            //    .HasOne(p => p.User)
            //    .WithMany(u => u.Projects)
            //    .HasForeignKey(p => p.UserId)
            //    .OnDelete(DeleteBehavior.NoAction); // Optional: Cascading deletes

            //// One-to-Many: ApplicationUser → Comment
            //modelBuilder.Entity<Comment>()
            //    .HasOne(c => c.User)
            //    .WithMany(u => u.Comments)
            //    .HasForeignKey(c => c.UserId)
            //    .OnDelete(DeleteBehavior.NoAction); // Optional: Cascading deletes

            //// One-to-Many: Project → Comment
            //modelBuilder.Entity<Comment>()
            //    .HasOne(c => c.Project)
            //    .WithMany(p => p.Comments)
            //    .HasForeignKey(c => c.ProjectId)
            //    .OnDelete(DeleteBehavior.NoAction); // Optional: Cascading deletes
            base.OnModelCreating(modelBuilder);
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
