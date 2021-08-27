using DataAccessLibrary;
using Ingenico.Direct.Sdk.Domain;
using Microsoft.AspNetCore.Components;
using PaymentAccessService;
using System.Threading.Tasks;
using System.Runtime;
using System;
using System.Web;
using DataAccessLibrary.Models;

namespace shu_bike_shop.Pages
{
    public partial class HostedCheckout
    {
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private IPaymentService paymentService { get; set; }
        [Inject] private ITransactionsData transactionsData { get; set; }
        [Inject] private IModalService modalSerivce { get; set; }

        [Parameter] public OrderDetails parent { get; set; }

        private string responseId;

        public async Task GoToHostedCheckoutPage()
        {
            try
            {
                var response = await GetResponseAsync();
                navigationManager.NavigateTo($"https://payment.{response.PartialRedirectUrl}");
                responseId = response.HostedCheckoutId;



            }
            catch
            {
                await modalSerivce.Inform("Something went wrong...");
            }
        }

        private async Task<CreateHostedCheckoutResponse> GetResponseAsync()
        {
            CreateHostedCheckoutRequest body = new CreateHostedCheckoutRequest();

            body.Order = parent.GetOrder();
            body.HostedCheckoutSpecificInput = new();
            body.HostedCheckoutSpecificInput.ReturnUrl = navigationManager.Uri;
            body.HostedCheckoutSpecificInput.Locale = "en_GB";
            body.HostedCheckoutSpecificInput.CardPaymentMethodSpecificInput = new();
            body.HostedCheckoutSpecificInput.CardPaymentMethodSpecificInput.GroupCards = true;

            var response = await paymentService.GetMerchant().HostedCheckout.CreateHostedCheckout(body);

            return response;
        }

        protected override async Task OnInitializedAsync()
        {
            var uri = navigationManager.Uri;

            Uri myUri = new Uri(uri);
            string hostedCheckoutId = HttpUtility.ParseQueryString(myUri.Query).Get("hostedCheckoutId");

            if (!string.IsNullOrEmpty(hostedCheckoutId))
            {
                var hostedCheckout = await paymentService.GetMerchant().HostedCheckout.GetHostedCheckout(hostedCheckoutId);

                if (hostedCheckout.CreatedPaymentOutput.Payment != null)
                {
                    var paymentId = hostedCheckout.CreatedPaymentOutput.Payment.Id.Split("_");
                    var paymentIdPrefix = long.Parse(paymentId[0]);
                    var paymentIdSuffix = int.Parse(paymentId[1]);

                    try
                    {
                        var t = await transactionsData.GetTransactionByPaymentId(paymentIdPrefix);
                    }
                    catch
                    {
                        TransactionCreateModel transaction = new TransactionCreateModel
                        {
                            PaymentId = paymentIdPrefix,
                            PaymentIdSuffix = paymentIdSuffix,
                            Amount = parent.orderModel.TotalAmount,
                            Username = parent.user.Name,
                            OrderId = parent.orderModel.Id,
                            Status = hostedCheckout.CreatedPaymentOutput.Payment.StatusOutput.StatusCategory
                        };

                        await transactionsData.AddTransaction(transaction);
                    }
                }
            }
        }
    }
}
