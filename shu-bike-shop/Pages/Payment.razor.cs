using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;
using PaymentAccessService;
using System;
using System.Threading.Tasks;
using System.Web;

namespace shu_bike_shop.Pages
{
    public partial class Payment
    {
        [Parameter] public int? Id { get; set; }

        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private IPaymentService paymentService { get; set; }
        [Inject] private ISecurityService securityService { get; set; }
        [Inject] private IOrderData orderData { get; set; }
        [Inject] private ITransactionsData transactionsData { get; set; }

        private string message;
        private bool? success;
        private OrderModel orderModel;

        private void BackToOrder()
        {
            navigationManager.NavigateTo($"/orders/{Id.Value}", true);
        }

        private async Task RetriveOrder()
        {
            var user = await securityService.GetCurrentUser();

            if (user != null)
            {
                orderModel = await orderData.GetOrder(Id.Value, user.Name);
            }
        }

        private async Task WaitForMerchant(params PaymentStatus[] paymentStatuses)
        {
            for (; ; )
            {
                await Task.Delay(1000);
                await RetriveOrder();

                if (orderModel == null)
                {
                    break;
                }

                bool cont = false;

                foreach (var item in paymentStatuses)
                {
                    if (orderModel.PaymentStatus == item)
                    {
                        cont = true;
                        break;
                    }
                }

                if (cont)
                {
                    continue;
                }

                break;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await WaitForMerchant(PaymentStatus.WaitingForMerchant, PaymentStatus.ProcessingPayment);

            if (orderModel == null)
            {
                success = false;
                message = "Wrong page";
                return;
            }

            if (orderModel.PaymentStatus == PaymentStatus.Paid)
            {
                success = true;
                message = $"Order {orderModel.Id} is successfully paid for.";
                return;
            }

            var uri = navigationManager.Uri;
            string hostedCheckoutId = HttpUtility.ParseQueryString(new Uri(uri).Query).Get("hostedCheckoutId");

            if (string.IsNullOrEmpty(hostedCheckoutId))
            {
                success = false;
                message = "Wrong page";
                return;
            }

            var hostedCheckout = await paymentService.GetMerchant().HostedCheckout.GetHostedCheckout(hostedCheckoutId);
            var payment = hostedCheckout.CreatedPaymentOutput?.Payment;
            if (payment == null || payment.StatusOutput.StatusCode != 5)
            {
                message = "Payment failed";
                success = false;
                return;
            }

            var paymentId = hostedCheckout.CreatedPaymentOutput.Payment.Id.Split("_");
            var paymentIdPrefix = long.Parse(paymentId[0]);
            var paymentIdSuffix = int.Parse(paymentId[1]);

            TransactionModel transaction = await transactionsData.GetTransactionByPaymentId(paymentIdPrefix);

            if (transaction == null)
            {
                TransactionCreateModel transactionCreateModel = new()
                {
                    PaymentId = paymentIdPrefix,
                    PaymentIdSuffix = paymentIdSuffix,
                    Amount = orderModel.TotalAmount,
                    Username = orderModel.UserEmail,
                    OrderId = orderModel.Id,
                    Status = payment.StatusOutput.StatusCategory,
                    StatusCode = payment.StatusOutput.StatusCode.GetValueOrDefault()
                };

                await transactionsData.AddTransaction(transactionCreateModel);
            }

            await WaitForMerchant(PaymentStatus.NotPaid, PaymentStatus.WaitingForMerchant, PaymentStatus.ProcessingPayment);

            message = $"Payment successful";
            success = true;
        }
    }
}
