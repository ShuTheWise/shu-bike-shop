using Microsoft.AspNetCore.Components;
using PaymentAccessService;
using System;
using System.Threading.Tasks;
using Ingenico.Direct.Sdk.Domain;

namespace shu_bike_shop.Pages
{
    public partial class Index
    {
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private IPaymentService paymentService { get; set; }

        protected async Task HostedCheckout()
        {
            try
            {
                CreateHostedCheckoutRequest body = new CreateHostedCheckoutRequest();

                var testRequest = paymentService.GetTestCreatePaymentRequest();

                body.Order = testRequest.Order;

                body.HostedCheckoutSpecificInput = new();
                body.HostedCheckoutSpecificInput.ReturnUrl = "https://www.heroku.com/";
                body.HostedCheckoutSpecificInput.Locale = "en_GB";
                body.HostedCheckoutSpecificInput.CardPaymentMethodSpecificInput = new();
                body.HostedCheckoutSpecificInput.CardPaymentMethodSpecificInput.GroupCards = true;

                var response = await paymentService.GetMerchant().HostedCheckout.CreateHostedCheckout(body);

                navigationManager.NavigateTo($"https://payment.{response.PartialRedirectUrl}");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
