using Ingenico.Direct.Sdk.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace shu_bike_shop.Pages
{
    public partial class CardForm
    {
        public delegate Task SubmitDelegate(CardPaymentMethodSpecificInput card);

        [Parameter]
        public SubmitDelegate OnValidSubmit { get; set; }
        public int? PaymentProductId { get; set; } = 1;

        private static readonly Random random = new();

        private readonly int minYear = 2021;
        private readonly int maxYear = 2050;
        private readonly long[] cards = new[] { 4539621432530835, 4207119164078249, 4578973198822589, 4916011604337123, 4024007185178505, 4539475867524734, 4936251254334329, 4916594740987885, 4926089846021729 };
        private readonly Dictionary<string, string> months = new()
        {
            {"01","January" },
            {"02","February"},
            {"03","March"},
            {"04","April"},
            {"05","May"},
            {"06","June"},
            {"07","July"},
            {"08","August"},
            {"09","September"},
            {"10","October"},
            {"11","November"},
            {"12","December" }
        };

        private Card card = new Card { ExpiryDate = "1122" };
        private TokenResponse TokenResponse;
        private EditContext editContext;
        private ValidationMessageStore messageStore;

        private string month = "11";
        private string year = "22";
        private bool paymentViaToken;
        private bool isProcessing;

        protected override void OnInitialized()
        {
            editContext = new(card);
            editContext.OnValidationRequested += HandleValidationRequested;
            messageStore = new(editContext);
        }

        private void Fill()
        {
            card.CardholderName = GenerateName();
            card.Cvv = GenCvv().ToString();
            card.CardNumber = GenCard().ToString();
            year = GenYear().ToString().Substring(2);
            month = GenMonth();
        }

        public void Load(TokenResponse tokenResponse)
        {
            var cardWithoutCvv = tokenResponse.Card.Data.CardWithoutCvv;

            card.CardholderName = cardWithoutCvv.CardholderName;
            card.Cvv = "";
            card.CardNumber = cardWithoutCvv.CardNumber;
            year = cardWithoutCvv.ExpiryDate.Substring(2);
            month = cardWithoutCvv.ExpiryDate.Substring(0, 2);


            TokenResponse = tokenResponse;
            paymentViaToken = true;
        }

        public void Clear()
        {
            card.CardholderName = "";
            card.Cvv = "";
            card.CardNumber = "";
            year = "22";
            month = "01";

            paymentViaToken = false;
            TokenResponse = null;
        }

        private string GenMonth()
        {
            var index = random.Next(0, 12);
            return months.ElementAt(index).Key;
        }

        private long GenCard()
        {
            return cards[random.Next(0, cards.Length - 1)];
        }

        private int GenYear()
        {
            return random.Next(minYear, maxYear);
        }

        private int GenCvv()
        {
            return random.Next(100, 999);
        }

        private void HandleValidationRequested(object sender, ValidationRequestedEventArgs args)
        {
            messageStore.Clear();

            card.ExpiryDate = month + year;

            if (!ValidateCvv(card.Cvv))
            {
                messageStore.Add(() => card.ExpiryDate, "Invalid cvv");
            }

            if (!ValidateCreditCardProvider(card.CardNumber))
            {
                messageStore.Add(() => card.CardNumber, "Only Visa and Mastercard are accepted");
            }

            if (paymentViaToken)
            {
                return;
            }

            if (string.IsNullOrEmpty(card.CardholderName))
            {
                messageStore.Add(() => card.CardNumber, "Invalid card holder");
            }

            if (!ValidateExpiryDate())
            {
                messageStore.Add(() => card.CardNumber, "Invalid expiry date");
            }

            if (!ValidateCreditCardNumber(card.CardNumber))
            {
                messageStore.Add(() => card.CardNumber, "Invalid card number");
            }
        }

        private static bool ValidateCvv(string cvv)
        {
            if (string.IsNullOrEmpty(cvv))
            {
                return false;
            }

            var cvvCheck = new Regex(@"^\d{3}$");

            if (!cvvCheck.IsMatch(cvv))
            {
                // <2>check cvv is valid as "999"
                return false;
            }

            return true;
        }

        private bool ValidateCreditCardProvider(string cardNo)
        {
            PaymentProductId = null;

            if (int.TryParse(cardNo[0].ToString(), out int firstNum))
            {
                if (firstNum == 2 || firstNum == 5)
                {
                    PaymentProductId = 3;
                }
                else if (firstNum == 4)
                {
                    PaymentProductId = 1;
                }
            }

            return PaymentProductId.HasValue;
        }

        private static bool ValidateCreditCardNumber(string cardNo)
        {
            if (string.IsNullOrEmpty(cardNo))
            {
                return false;
            }

            var cardCheck = new Regex(@"(^4[0-9]{12}(?:[0-9]{3})?$)|(^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$)|(3[47][0-9]{13})|(^3(?:0[0-5]|[68][0-9])[0-9]{11}$)|(^6(?:011|5[0-9]{2})[0-9]{12}$)|(^(?:2131|1800|35\d{3})\d{11}$)");

            if (!cardCheck.IsMatch(cardNo))
            {
                return false;
            }

            return true;
        }

        private async Task HandleValidSubmit()
        {
            if (OnValidSubmit != null)
            {
                isProcessing = true;
                this.StateHasChanged();

                CardPaymentMethodSpecificInput cardPaymentMethodSpecificInput = new CardPaymentMethodSpecificInput();
                cardPaymentMethodSpecificInput.Card = card;
                cardPaymentMethodSpecificInput.PaymentProductId = PaymentProductId;

                if (paymentViaToken)
                {
                    cardPaymentMethodSpecificInput.Token = TokenResponse.Id;
                }

                await OnValidSubmit(cardPaymentMethodSpecificInput);

                isProcessing = false;
                this.StateHasChanged();
            }
        }

        private bool ValidateExpiryDate()
        {
            if (year != "21")
            {
                return true;
            }

            if (int.TryParse(month, out int m))
            {
                if (m < DateTime.Now.Month)
                {
                    return false;
                }
            }

            return true;
        }

        public static string GenerateName()
        {
            int len = 10;

            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;
        }

        public void Dispose()
        {
            editContext.OnValidationRequested -= HandleValidationRequested;
        }
    }
}
