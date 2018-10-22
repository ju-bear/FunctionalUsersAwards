using System;

namespace FunctionalUsersAwards.EfContext
{
    public class UserAward
    {
        public Guid UserId { get; set; }
        
        public User User { get; set; }
        
        public Guid AwardId { get; set; }
        
        public Award Award { get; set; }
    }
}