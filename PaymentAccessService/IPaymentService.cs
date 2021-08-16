using Ingenico.Direct.Sdk.Domain;
using System.Threading.Tasks;

namespace PaymentAccessService
{
    public interface IPaymentService
    {
        Task<TestConnection> TestConnection();
        Task<CreatePaymentResponse> CreateTestPayment();

        string MerchantId { get; }
    }
}