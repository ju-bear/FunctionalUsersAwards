using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunctionalUsersAwards.EfContext
{
    public class AwardTypeConfiguration : IEntityTypeConfiguration<Award>
    {
        public void Configure(EntityTypeBuilder<Award> builder)
        {
            builder.ToTable("Award");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("AwardId");
            builder.Property(x => x.Title).HasColumnName("Title").HasMaxLength(255);

            builder.HasMany(x => x.Users).WithOne(x => x.Award).HasForeignKey(x => x.AwardId);
        }
    }
}