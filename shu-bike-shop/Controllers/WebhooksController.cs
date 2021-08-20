using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Ingenico.Direct.Sdk.Domain;
using DataAccessLibrary;
using PaymentAccessService;
using DataAccessLibrary.Models;

namespace shu_bike_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhooksController : ControllerBase
    {
        private readonly ITransactionsData transactionsData;
        private readonly IReceivedCreatePaymentResponseData responsesData;
        private readonly IOrderData orderData;
        private readonly IPaymentService paymentService;
        private readonly ILogger logger;

        public WebhooksController(ITransactionsData transactionsData, IOrderData orderData, IReceivedCreatePaymentResponseData responsesData, IPaymentService paymentService, ILogger logger)
        {
            this.transactionsData = transactionsData;
            this.orderData = orderData;
            this.responsesData = responsesData;
            this.paymentService = paymentService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task Post([FromBody] CreatePaymentResponse createPaymentResponse)
        {
            var payment = createPaymentResponse.Payment;
            string paymentId = createPaymentResponse.Payment.Id;

            await logger.Log($"Received payment response {paymentId}, status {createPaymentResponse.Payment.StatusOutput.StatusCode}");
            try
            {
                int responseId = await responsesData.AddResponse(createPaymentResponse.ToJson(false));

                var paymentIdSplit = payment.Id.Split('_');
                var paymentIdPrefix = long.Parse(paymentIdSplit[0]);
                var paymentIdSuffix = int.Parse(paymentIdSplit[1]);

                TransactionModel transaction = null;
                for (; ; )
                {
                    int i = 0;
                    try
                    {
                        await Task.Delay(3000);
                        transaction = await transactionsData.GetTransactionByPaymentId(paymentIdPrefix);
                        i++;

                        break;
                    }
                    catch
                    {
                        if (i > 5)
                        {
                            throw;
                        }
                    }
                }

                dynamic parameters = new
                {
                    status = createPaymentResponse.Payment.Status,
                    orderid = transaction.OrderId,
                    paymentidsuffix = paymentIdSuffix
                };

                await transactionsData.UpdateTransactionByPaymentId(paymentIdPrefix, parameters);

                PaymentStatus? status = null;

                switch (createPaymentResponse.Payment.StatusOutput.StatusCode)
                {
                    case 5:
                        {
                            status = PaymentStatus.WaitingForMerchant;
                            //var body = new CapturePaymentRequest
                            //{
                            //    Amount = payment.PaymentOutput.AmountOfMoney.Amount,
                            //    IsFinal = true
                            //};
                            //var captureResponse = await paymentService.GetMerchant().Payments.CapturePayment(payment.Id, body);
                            break;
                        }
                    case 9:
                    case 91:
                        status = PaymentStatus.Paid;
                        break;
                }

                if (status.HasValue)
                {
                    await orderData.UpdateOrderByOrderId(transaction.OrderId, new { paymentstatus = status.Value });
                }
                await logger.Log($"Payment {paymentId} processed sucessfully");
            }
            catch (Exception ex)
            {
                await logger.Log($"Error processing payment response {paymentId}, {ex}");
            }
        }
    }
}
