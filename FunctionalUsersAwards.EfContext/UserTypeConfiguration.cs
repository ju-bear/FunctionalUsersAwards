using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunctionalUsersAwards.EfContext
{
    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("UserId");
            builder.Property(x => x.Username).HasColumnName("Username").IsRequired().HasMaxLength(255);

            builder.HasMany(x => x.Awards).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }
    }
}