using Microsoft.EntityFrameworkCore;
using ValiBot.Entities;

namespace ValiBot
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Operation> Operations { get; set; }
        
        
        public DbSet<RegisterForm> RegisterForms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.RegisterForms)
                .WithOne(f => f.AppUser)
                .HasForeignKey(f => f.AppUserId);
        }
    }
}