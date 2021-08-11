using Ingenico.Direct.Sdk.Domain;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public interface IPaymentService
    {
        CreatePaymentResponse CreatePayment(CreatePaymentRequest createPaymentRequest);
        Task<TestConnection> TestConnection();
        Task<CreatePaymentResponse> CreateTestPayment();

        string MerchantId { get; }
    }
}