using Microsoft.AspNetCore.Components;
using PaymentAccessService;
using System;
using System.Threading.Tasks;

namespace shu_bike_shop.Pages
{
    public partial class ConnectionTest
    {
        [Inject] private IPaymentService paymentService { get; set; }

        private string connectionTestResult;
        private string testPaymentResult;
        private bool testPaymentPerformed;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                testPaymentPerformed = false;
                var testConnection = await paymentService.TestConnection();
                connectionTestResult = testConnection.Result;
            }
            catch (Exception ex)
            {
                connectionTestResult = ex.ToString();
            }
        }

        protected async Task CreateTestPayment()
        {
            testPaymentPerformed = true;
            testPaymentResult = "";

            try
            {
                var paymentResponse = await paymentService.CreateTestPayment();
                testPaymentResult = paymentResponse.ToJson();
            }
            catch (Exception ex)
            {
                testPaymentResult = ex.ToString();
            }
        }
    }
}
