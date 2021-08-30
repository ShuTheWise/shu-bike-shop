using DataAccessLibrary;
using Ingenico.Direct.Sdk.Domain;
using Microsoft.AspNetCore.Components;
using PaymentAccessService;
using System.Threading.Tasks;

namespace shu_bike_shop.Pages
{
    public partial class HostedCheckout
    {
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private IPaymentService paymentService { get; set; }
        [Inject] private IModalService modalSerivce { get; set; }
        
        [Parameter] public OrderDetails parent { get; set; }

        public async Task GoToHostedCheckoutPage()
        {
            try
            {
                var response = await CreateHostedCheckout();
                navigationManager.NavigateTo($"https://payment.{response.PartialRedirectUrl}");
            }
            catch
            {
                await modalSerivce.Inform("Something went wrong...");
            }
        }

        private async Task<CreateHostedCheckoutResponse> CreateHostedCheckout()
        {
            CreateHostedCheckoutRequest body = new();

            body.Order = parent.GetOrder();
            body.HostedCheckoutSpecificInput = new();
            body.HostedCheckoutSpecificInput.ReturnUrl = $"{navigationManager.BaseUri}/payment/{parent.OrderModel.Id}";
            body.HostedCheckoutSpecificInput.Locale = "en_GB";
            body.HostedCheckoutSpecificInput.CardPaymentMethodSpecificInput = new();
            body.HostedCheckoutSpecificInput.CardPaymentMethodSpecificInput.GroupCards = true;

            var response = await paymentService.GetMerchant().HostedCheckout.CreateHostedCheckout(body);
            return response;
        }
    }
}
