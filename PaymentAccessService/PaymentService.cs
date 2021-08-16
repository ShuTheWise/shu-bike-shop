using Ingenico.Direct.Sdk;
using Ingenico.Direct.Sdk.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentAccessService
{
    public class PaymentService : IPaymentService
    {
        public string MerchantId => "lkaMerchantWebshopTest";

        private readonly IConfiguration config;
        //private string idempotenceKey = Guid.NewGuid().ToString();

        public PaymentService(IConfiguration config)
        {
            this.config = config;
        }

        public Task<TestConnection> TestConnection()
        {
            return GetClient().WithNewMerchant(MerchantId).Services.TestConnection();
        }

        public Task<CreatePaymentResponse> CreateTestPayment()
        {
            var client = GetClient();
            var body = GetTestCreatePaymentRequest();
            //CallContext context = new CallContext().WithIdempotenceKey(idempotenceKey);
            
            return client.WithNewMerchant(MerchantId).Payments.CreatePayment(body);
            //return GetClient().WithNewMerchant(MerchantId).Services.TestConnection();
        }

        private IClient GetClient()
        {
            var apiKeyId = config["API_KEY_ID"];
            var secretApiKey = config["SECRET_API_KEY"];

            CommunicatorConfiguration communicatorConfiguration = new()
            {
                ApiEndpoint = new Uri("https://payment.preprod.direct.ingenico.com"),
                ApiKeyId = apiKeyId,
                SecretApiKey = secretApiKey
            };

            return Factory.CreateClient(communicatorConfiguration);
        }

        //public CreatePaymentResponse CreatePayment(CreatePaymentRequest createPaymentRequest)
        //{
        //    var client = GetClient();
        //    CallContext context = new CallContext().WithIdempotenceKey(idempotenceKey);
        //    try
        //    {
        //        CreatePaymentResponse response = client.WithNewMerchant(MerchantId).Payments.CreatePayment(createPaymentRequest).GetAwaiter().GetResult();
        //        return response;
        //    }
        //    catch (IdempotenceException e)
        //    {
        //        // A request with the same idempotenceKey is still in progress, try again after a short pause
        //        // e.IdempotenceRequestTimestamp contains the value of the
        //        // X-GCS-Idempotence-Request-Timestamp header
        //    }
        //    finally
        //    {
        //        long? idempotenceRequestTimestamp = context.IdempotenceRequestTimestamp;
        //        // idempotenceRequestTimestamp contains the value of the
        //        // X-GCS-Idempotence-Request-Timestamp header
        //        // if idempotenceRequestTimestamp is not null this was not the first request
        //    }

        //    return null;
        //}

        public CreatePaymentRequest GetTestCreatePaymentRequest()
        {
            Card card = new Card();
            card.CardNumber = "5137009801943438";
            card.CardholderName = "Wile E. Coyote";
            card.Cvv = "123";
            card.ExpiryDate = "1299";

            //AmountOfMoney authenticationAmount = new AmountOfMoney();
            //authenticationAmount.Amount = 20L * 100;
            //authenticationAmount.CurrencyCode = "EUR";

            RedirectionData redirectionData = new RedirectionData();
            redirectionData.ReturnUrl = "https://hostname.myownwebsite.url";

            //ThreeDSecure threeDSecure = new ThreeDSecure();
            ////threeDSecure.AuthenticationAmount = authenticationAmount;
            ////threeDSecure.AuthenticationFlow = "browser";
            //threeDSecure.ChallengeCanvasSize = "600x400";
            //threeDSecure.ChallengeIndicator = "challenge-requested";
            //threeDSecure.ExemptionRequest = "none";
            //threeDSecure.RedirectionData = redirectionData;
            //threeDSecure.SkipAuthentication = true;

            CardPaymentMethodSpecificInput cardPaymentMethodSpecificInput = new CardPaymentMethodSpecificInput();
            cardPaymentMethodSpecificInput.Card = card;
            cardPaymentMethodSpecificInput.IsRecurring = false;
            //cardPaymentMethodSpecificInput.MerchantInitiatedReasonIndicator = "delayedCharges";
            cardPaymentMethodSpecificInput.PaymentProductId = 3;
            cardPaymentMethodSpecificInput.AuthorizationMode = "SALE";

            //disabled 3d
            //cardPaymentMethodSpecificInput.ThreeDSecure = threeDSecure;

            cardPaymentMethodSpecificInput.TransactionChannel = "ECOMMERCE";

            AmountOfMoney amountOfMoney = new AmountOfMoney();
            amountOfMoney.Amount = 20L * 100;
            amountOfMoney.CurrencyCode = "EUR";

            Address billingAddress = new Address();
            billingAddress.AdditionalInfo = "b";
            billingAddress.City = "Monument Valley";
            billingAddress.CountryCode = "US";
            billingAddress.HouseNumber = "13";
            billingAddress.State = "Utah";
            billingAddress.Street = "Desertroad";
            billingAddress.Zip = "84536";

            CompanyInformation companyInformation = new CompanyInformation();
            companyInformation.Name = "Acme Labs";

            ContactDetails contactDetails = new ContactDetails();
            contactDetails.EmailAddress = "wile.e.coyote@acmelabs.com";
            contactDetails.FaxNumber = "+1234567891";
            contactDetails.PhoneNumber = "+1234567890";

            BrowserData browserData = new BrowserData();
            browserData.ColorDepth = 24;
            browserData.JavaEnabled = false;
            browserData.ScreenHeight = "1200";
            browserData.ScreenWidth = "1920";

            CustomerDevice device = new CustomerDevice();
            device.AcceptHeader = "texthtml,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            device.BrowserData = browserData;
            device.IpAddress = "123.123.123.123";
            device.Locale = "en-US";
            device.TimezoneOffsetUtcMinutes = "420";
            device.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_4) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1 Safari/605.1.15";

            PersonalName name = new PersonalName();
            name.FirstName = "Wile";
            name.Surname = "Coyote";
            name.Title = "Mr.";

            PersonalInformation personalInformation = new PersonalInformation();
            personalInformation.DateOfBirth = "19490917";
            personalInformation.Gender = "male";
            personalInformation.Name = name;

            Customer customer = new Customer();
            customer.AccountType = "none";
            customer.BillingAddress = billingAddress;
            customer.CompanyInformation = companyInformation;
            customer.ContactDetails = contactDetails;
            customer.Device = device;
            customer.Locale = "en_US";
            customer.MerchantCustomerId = "1234";
            customer.PersonalInformation = personalInformation;

            //OrderInvoiceData invoiceData = new OrderInvoiceData();
            //invoiceData.InvoiceDate = "20140306191500";
            //invoiceData.InvoiceNumber = "000000123";

            OrderReferences references = new OrderReferences();
            references.Descriptor = "Fast and Furry-ous";
            references.MerchantReference = "AcmeOrder0001";

            PersonalName shippingName = new PersonalName();
            shippingName.FirstName = "Road";
            shippingName.Surname = "Runner";
            shippingName.Title = "Miss";

            AddressPersonal address = new AddressPersonal();
            address.AdditionalInfo = "Suite II";
            address.City = "Monument Valley";
            address.CountryCode = "US";
            address.HouseNumber = "1";
            address.Name = shippingName;
            address.State = "Utah";
            address.Street = "Desertroad";
            address.Zip = "84536";

            Shipping shipping = new Shipping();
            shipping.Address = address;

            IList<LineItem> items = new List<LineItem>();

            AmountOfMoney item1AmountOfMoney = new AmountOfMoney();
            item1AmountOfMoney.Amount = 2500L * 100;
            item1AmountOfMoney.CurrencyCode = "EUR";

            LineItemInvoiceData item1InvoiceData = new LineItemInvoiceData();
            item1InvoiceData.Description = "ACME Super Outfit";
            //item1InvoiceData.NrOfItems = "1";
            //item1InvoiceData.PricePerItem = 2500L;

            LineItem item1 = new LineItem();
            item1.AmountOfMoney = item1AmountOfMoney;
            item1.InvoiceData = item1InvoiceData;

            items.Add(item1);

            AmountOfMoney item2AmountOfMoney = new AmountOfMoney();
            item2AmountOfMoney.Amount = 480L * 100;
            item2AmountOfMoney.CurrencyCode = "EUR";

            LineItemInvoiceData item2InvoiceData = new LineItemInvoiceData();
            item2InvoiceData.Description = "Aspirin";

            LineItem item2 = new LineItem();
            item2.AmountOfMoney = item2AmountOfMoney;
            item2.InvoiceData = item2InvoiceData;

            items.Add(item2);

            ShoppingCart shoppingCart = new ShoppingCart();
            shoppingCart.Items = items;

            Order order = new Order();
            order.AmountOfMoney = amountOfMoney;
            //order.Customer = customer;
            //order.References = references;
            //order.Shipping = shipping;
            //order.ShoppingCart = shoppingCart;

            CreatePaymentRequest body = new CreatePaymentRequest();
            //body.CardPaymentMethodSpecificInput.ThreeDSecure.SkipAuthentication 
            body.CardPaymentMethodSpecificInput = cardPaymentMethodSpecificInput;
            body.Order = order;

            return body;
        }
    }
}