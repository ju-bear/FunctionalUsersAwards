using Microsoft.EntityFrameworkCore;

namespace FunctionalUsersAwards.EfContext
{
    public class UserAwardDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public DbSet<Award> Awards { get; set; }

        public UserAwardDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserTypeConfiguration())
                        .ApplyConfiguration(new UserAwardTypeConfiguration())
                        .ApplyConfiguration(new AwardTypeConfiguration());
        }
    }
}