using Microsoft.EntityFrameworkCore;
using shu_bike_shop.Data;

namespace shu_bike_shop
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options)
                : base(options)
        {
        }

        public DbSet<Bike> Bikes { get; set; }
    }

    public class User
    {
        public string Username { get; set; }
    }

    public class Account
    {

    }
}
