using Ingenico.Direct.Sdk;
using Ingenico.Direct.Sdk.Domain;
using System;

namespace shu_bike_shop
{
    public class BackofficeContext
    {
        private string merchantId = "lkaMerchantWebshopTest";
        private string idempotenceKey = Guid.NewGuid().ToString();

        public void CreatePayment(CreatePaymentRequest createPaymentRequest)
        {
            CommunicatorConfiguration communicatorConfiguration2 = new()
            {
                ApiEndpoint = new Uri(""),
                ApiKeyId = "",
                SecretApiKey = ""
            };

            var client = Factory.CreateClient(communicatorConfiguration2);

            CallContext context = new CallContext().WithIdempotenceKey(idempotenceKey);
            try
            {
                CreatePaymentResponse response = client.WithNewMerchant(merchantId).Payments.CreatePayment(createPaymentRequest).GetAwaiter().GetResult();
            }
            catch (IdempotenceException e)
            {
                // A request with the same idempotenceKey is still in progress, try again after a short pause
                // e.IdempotenceRequestTimestamp contains the value of the
                // X-GCS-Idempotence-Request-Timestamp header
            }
            finally
            {
                long? idempotenceRequestTimestamp = context.IdempotenceRequestTimestamp;
                // idempotenceRequestTimestamp contains the value of the
                // X-GCS-Idempotence-Request-Timestamp header
                // if idempotenceRequestTimestamp is not null this was not the first request
            }
        }
    }
}