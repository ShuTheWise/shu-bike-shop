using Ingenico.Direct.Sdk;
using Ingenico.Direct.Sdk.Domain;
using Ingenico.Direct.Sdk.Merchant;
using System.Threading.Tasks;

namespace PaymentAccessService
{
    public interface IPaymentService
    {
        Task<TestConnection> TestConnection();
        Task<CreatePaymentResponse> CreateTestPayment();
        Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest createPaymentRequest);
        IMerchantClient GetMerchant();
        string MerchantId { get; }
    }
}