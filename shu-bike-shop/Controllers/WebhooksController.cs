using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DataAccessLibrary;
using PaymentAccessService;
using DataAccessLibrary.Models;
using System;

namespace shu_bike_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhooksController : ControllerBase
    {
        private const int retryCount = 3;

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
        public async Task<IActionResult> Post([FromBody] PaymentModel paymentModel)
        {
            var payment = paymentModel.Payment;
            string paymentId = $"request id: {paymentModel.Id}, payment id: {paymentModel.Payment.Id}";

            await logger.Log($"Received payment response {paymentId} with status {paymentModel.Payment.StatusOutput.StatusCode}");
            try
            {
                int responseId = await responsesData.AddResponse(paymentModel.ToJson(false));

                var paymentIdSplit = payment.Id.Split('_');
                var paymentIdPrefix = long.Parse(paymentIdSplit[0]);
                var paymentIdSuffix = int.Parse(paymentIdSplit[1]);

                TransactionModel transaction = await GetTransaction(paymentId, paymentIdPrefix);

                dynamic parameters = new
                {
                    status = paymentModel.Payment.Status,
                    orderid = transaction.OrderId,
                    paymentidsuffix = paymentIdSuffix
                };

                await transactionsData.UpdateTransactionByPaymentId(paymentIdPrefix, parameters);

                PaymentStatus? status = null;

                switch (paymentModel.Payment.StatusOutput.StatusCode)
                {
                    case 0:
                        status = PaymentStatus.ProcessingPayment;
                        break;
                    case 5:
                        status = PaymentStatus.WaitingForMerchant;
                        break;
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
                return BadRequest();
            }
            return Ok();
        }

        private async Task<TransactionModel> GetTransaction(string paymentId, long paymentIdPrefix)
        {
            TransactionModel transaction = null;
            int i = 1;
            for (; ; )
            {
                try
                {
                    transaction = await transactionsData.GetTransactionByPaymentId(paymentIdPrefix);
                    break;
                }
                catch
                {
                    await logger.Log($"Could not get transaction for {paymentId} on try {i}");
                    await Task.Delay(i * i * 1000);
                    i++;
                    if (i > 3)
                    {
                        throw;
                    }
                }
            }

            return transaction;
        }
    }
}
