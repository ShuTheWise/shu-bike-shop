using System;

namespace shu_bike_shop
{
    public class InsufficientProductAmountException : Exception
    {
        public InsufficientProductAmountException(string message) : base(message)
        {
        }
    }
}
