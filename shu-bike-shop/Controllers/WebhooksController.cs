using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ingenico.Direct.Sdk;
using Ingenico.Direct.Sdk.Domain;
using DataAccessLibrary;
using PaymentAccessService;
using DataAccessLibrary.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace shu_bike_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhooksController : ControllerBase
    {
        private ITransactionsData transactionsData;
        private IReceivedCreatePaymentResponseData responsesData;
        private IOrderData orderData;
        private IPaymentService paymentService;

        public WebhooksController(ITransactionsData transactionsData, IOrderData orderData, IReceivedCreatePaymentResponseData responsesData, IPaymentService paymentService)
        {
            this.transactionsData = transactionsData;
            this.orderData = orderData;
            this.responsesData = responsesData;
            this.paymentService = paymentService;
        }

        // POST api/<FooController>
        [HttpPost]
        public async Task Post([FromBody] CreatePaymentResponse createPaymentResponse)
        {
            int responseId = await responsesData.AddResponse(createPaymentResponse.ToJson(false));

            var payment = createPaymentResponse.Payment;
            var paymentIdStr = payment.Id.Split('_');
            var paymentId = long.Parse(paymentIdStr[0]);
            var paymentIdSuffix = int.Parse(paymentIdStr[1]);

            var transaction = await transactionsData.GetTransactionByPaymentId(paymentId);

            dynamic parameters = new
            {
                status = createPaymentResponse.Payment.Status,
                orderid = transaction.OrderId,
                paymentidsuffix = paymentIdSuffix
            };

            await transactionsData.UpdateTransactionByPaymentId(paymentId, parameters);

            PaymentStatus? status = null;

            if (createPaymentResponse.Payment.StatusOutput.StatusCode == 5)
            {
                status = PaymentStatus.WaitingForMerchant;
            }
            else if (createPaymentResponse.Payment.StatusOutput.StatusCode == 9)
            {
                status = PaymentStatus.Paid;
            }
            else if (createPaymentResponse.Payment.StatusOutput.StatusCode == 91)
            {
                ///
                //paymentService.CreateTestPayment
            }

            //CaptureResponse response = client.merchant("merchantId").payments().capture("paymentId", body);

            if (status.HasValue)
            {
                await orderData.UpdateOrderByOrderId(transaction.OrderId, new { paymentstatus = status.Value });
            }
        }
    }
}
