using System.Reflection;
using FunctionalUsersAwards.EfContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FunctionalUsersAwards.EfMigrations
{
    public class UserAwardDesignTimeFactory : IDesignTimeDbContextFactory<UserAwardDbContext>
    {
        public UserAwardDbContext CreateDbContext(string[] args)
        {
            string conString = "Data Source=.;Initial Catalog=UsersAwards;Integrated Security=True";
            var opts = new DbContextOptionsBuilder().UseSqlServer(conString, builder => builder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
            return new UserAwardDbContext(opts.Options);
        }
    }
}