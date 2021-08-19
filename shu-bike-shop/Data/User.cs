using System;
using System.Collections.Generic;
using System.Linq;
using Ingenico.Direct.Sdk;

namespace shu_bike_shop
{
    public class User
    {
        public string Name { get; set; }
        public Role Role { get; set; }

        public bool CanPlaceOrder => Role == Role.Users;
    }
}
