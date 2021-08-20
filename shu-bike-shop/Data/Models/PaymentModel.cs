using System;
using Ingenico.Direct.Sdk.Domain;

namespace shu_bike_shop
{
    public class PaymentModel
    {
        public string ApiVersion { get; set; }
        public DateTime Created { get; set; }
        public Guid Id { get; set; }
        public string MerchantId { get; set; }
        public string Type { get; set; }
        public PaymentResponse Payment { get; set; }
    }
}
