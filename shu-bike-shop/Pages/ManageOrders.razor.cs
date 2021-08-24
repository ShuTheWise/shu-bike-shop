using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace shu_bike_shop.Pages
{
    public partial class ManageOrders
    {
        [Inject] private IOrderData orderData { get; set; }
        [Inject] private ITransactionsData transactionData { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private ISecurityService securityService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }

        private List<OrderModel> orders;

        protected override async Task OnInitializedAsync()
        {
            await RefreshOrders();
        }

        private async Task RefreshOrders()
        {
            orders = await orderData.GetOrders();
        }

        private async Task UpdateOrder(OrderModel orderModel)
        {
            dynamic model = new
            {
                orderstatus = orderModel.OrderStatus,
                paymentstatus = orderModel.PaymentStatus
            };

            await orderData.UpdateOrderByOrderId(orderModel.Id, model);

            await RefreshOrders();
        }
    }
}
