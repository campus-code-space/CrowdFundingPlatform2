using EndeKissie2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
            base.OnModelCreating(modelBuilder);
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
