using System;
using System.Collections.Generic;

namespace FunctionalUsersAwards.EfContext
{
    public class Award
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; }
        
        public List<UserAward> Users { get; set; }
    }
}