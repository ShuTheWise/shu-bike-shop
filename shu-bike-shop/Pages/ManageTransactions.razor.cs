using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace shu_bike_shop.Pages
{
    public partial class ManageTransactions
    {
        [Inject] private ITransactionsData transactionData { get; set; }

        private List<TransactionModel> transactions;

        protected override async Task OnInitializedAsync()
        {
            await RefreshTransactions();
        }

        private async Task RefreshTransactions()
        {
            transactions = await transactionData.GetTransactions();
        }
    }
}
