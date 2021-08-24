using DataAccessLibrary;
using DataAccessLibrary.Models;
using Ingenico.Direct.Sdk.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PaymentAccessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shu_bike_shop.Pages
{
    public partial class OrderDetails
    {
        [Parameter]
        public int? Id { get; set; }

        public bool IsRecurring { get; set; }
        public string AuthorizationMode { get; set; } = "SALE";

        [Inject] private IOrderData orderData { get; set; }
        [Inject] private ITokenData tokenData { get; set; }
        [Inject] private ISecurityService securityService { get; set; }
        [Inject] private IPaymentService paymentService { get; set; }
        [Inject] private ITransactionsData transactionData { get; set; }
        [Inject] private IJSRuntime jSRuntime { get; set; }

        private CardForm cardForm;
        private User user;
        private List<TokenResponse> userTokens;
        private List<string> userTokenAliases;

        private OrderModel orderModel;
        private bool loaded;
        private int? aliasId;

        private int? AliasId
        {
            get => aliasId;
            set
            {
                aliasId = value;
                if (aliasId.HasValue)
                {
                    cardForm.Load(userTokens[aliasId.Value]);
                }
                else
                {
                    cardForm.Clear();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            user = await securityService.GetCurrentUser();
            await RefreshOrder();

            if (user != null)
            {
                userTokens = await GetUserTokensAsync(user.Name);
                userTokenAliases = userTokens.Select(x => x.Card.Alias).ToList();
            }
        }

        private async Task<List<TokenResponse>> GetUserTokensAsync(string username)
        {
            var tokenCards = new List<TokenResponse>();

            var tokens = await tokenData.GetTokens(username);

            foreach (var token in tokens)
            {
                var tokenResponse = await paymentService.GetMerchant().Tokens.GetToken(token);
                tokenCards.Add(tokenResponse);
            }

            return tokenCards;
        }

        private CreatePaymentRequest CreateRequestWithoutCreditCardInfo()
        {
            var amountOfMoney = long.Parse((orderModel.TotalAmount * 100).ToString());

            var order = new Order()
            {
                AmountOfMoney = new AmountOfMoney()
                {
                    Amount = amountOfMoney,
                    CurrencyCode = "EUR"
                }
            };

            CreatePaymentRequest createPaymentRequest = new CreatePaymentRequest()
            {
                Order = order,
            };

            return createPaymentRequest;
        }

        private async Task PayAsync(CardPaymentMethodSpecificInput cardPaymentMethod)
        {
            var pay = await jSRuntime.Confirm($"Pay for order ?");

            if (!pay)
            {
                return;
            }

            TransactionCreateModel transaction = new TransactionCreateModel
            {
                PaymentProductId = cardPaymentMethod.PaymentProductId.Value,
                Amount = orderModel.TotalAmount,
                Username = user.Name,
                OrderId = orderModel.Id,
                CardholderName = cardPaymentMethod.Card.CardholderName
            };

            string message;

            try
            {
                CreatePaymentRequest createPaymentRequest = CreateRequestWithoutCreditCardInfo();
                createPaymentRequest.CardPaymentMethodSpecificInput = cardPaymentMethod;
                createPaymentRequest.CardPaymentMethodSpecificInput.IsRecurring = IsRecurring;
                createPaymentRequest.CardPaymentMethodSpecificInput.AuthorizationMode = AuthorizationMode;

                var response = await paymentService.CreatePayment(createPaymentRequest);
                var json = response.ToJson();
                transaction.ResponseMessage = json;
                transaction.Status = response.Payment.Status;

                var paymentId = response.Payment.Id.Split('_');
                transaction.PaymentId = long.Parse(paymentId[0]);
                transaction.PaymentIdSuffix = int.Parse(paymentId[1]);
                message = json;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                transaction.ErrorMessage = message;
            }

            //make a call to api controller or sth else so it is not awaited here
            await transactionData.AddTransaction(transaction);
            //await jSRuntime.Inform(message);

            if (AliasId.HasValue)
            {
                await jSRuntime.Inform("Payment sucessful");
                return;
            }
            else
            {
                if (await jSRuntime.Confirm("Payment sucessful, do you want to save card information for future use?"))
                {
                    var createdTokenResponse = await CreateToken(cardPaymentMethod);
                    await tokenData.AddToken(user.Name, createdTokenResponse.Token);

                    string message2 = "Error saving card";
                    if (createdTokenResponse != null)
                    {
                        try
                        {
                            var tokenResponse = await paymentService.GetMerchant().Tokens.GetToken(createdTokenResponse.Token);
                            message2 = $"Sucessfully saved card, alis: {tokenResponse.Card.Alias}";
                        }
                        catch
                        {
                            message2 = "Sucessfully saved card, but could not retrive";
                        }
                    }

                    await jSRuntime.Inform(message2);
                }
            }
        }

        private async Task<CreatedTokenResponse> CreateToken(CardPaymentMethodSpecificInput input)
        {
            try
            {
                var body = new CreateTokenRequest();
                TokenCardSpecificInput tokenCardSpecificInput = new TokenCardSpecificInput();
                tokenCardSpecificInput.Data = new Ingenico.Direct.Sdk.Domain.TokenData();
                tokenCardSpecificInput.Data.Card = input.Card;
                body.PaymentProductId = input.PaymentProductId.Value;
                body.Card = tokenCardSpecificInput;
                return await paymentService.GetMerchant().Tokens.CreateToken(body);
            }
            catch
            {

            }

            return null;
        }

        private async Task RefreshOrder()
        {
            loaded = false;

            if (user != null && Id.HasValue)
            {
                orderModel = await orderData.GetOrder(Id.Value, user.Name);
            }

            loaded = true;
        }

        private bool IsPaid(OrderModel order) => order.PaymentStatus == PaymentStatus.Paid;
    }
}
