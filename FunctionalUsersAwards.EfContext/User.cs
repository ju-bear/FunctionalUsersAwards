using System;
using System.Collections.Generic;

namespace FunctionalUsersAwards.EfContext
{
    public class User
    {
        public Guid Id { get; set; }
        
        public string Username { get; set; }
        
        public List<UserAward> Awards { get; set; }
    }
}