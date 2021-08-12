using System;
using System.Collections.Generic;
using System.Linq;

namespace shu_bike_shop
{
    public class User
    {
        public string Name { get; set; }
        public Role Role { get; set; }

        public bool CanPlaceOrder => Role == Role.Users;
    }
}
