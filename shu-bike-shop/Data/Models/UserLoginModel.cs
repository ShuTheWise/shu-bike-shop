using System.Collections.Generic;
using System.Linq;

namespace shu_bike_shop
{
    public class UserLoginModel
    {
        public string EmailAddress { get; set; }
        public string EncryptedPassword { get; set; }
    }
}