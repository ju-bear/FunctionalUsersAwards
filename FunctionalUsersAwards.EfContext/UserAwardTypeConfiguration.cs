using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunctionalUsersAwards.EfContext
{
    public class UserAwardTypeConfiguration : IEntityTypeConfiguration<UserAward>
    {
        public void Configure(EntityTypeBuilder<UserAward> builder)
        {
            builder.HasKey(x => new
                                {
                                    x.UserId, 
                                    x.AwardId
                                });
            builder.Property(x => x.UserId).HasColumnName("UserId");
            builder.Property(x => x.AwardId).HasColumnName("AwardId");
        }
    }
}