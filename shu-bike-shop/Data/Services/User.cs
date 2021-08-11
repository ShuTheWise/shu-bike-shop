using DataAccessLibrary;

namespace shu_bike_shop
{
    public class User
    {
        public string Email { get; set; }
        public Role Role { get; set; }

        public bool CanBuy => Role == Role.User;
    }
}