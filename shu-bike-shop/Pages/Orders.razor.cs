using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace shu_bike_shop.Pages
{
    public partial class Orders
    {
        [Inject] private IOrderData orderData { get; set; }
        [Inject] private ISecurityService securityService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }

        private User user;
        private List<OrderModel> orders;

        protected override async Task OnInitializedAsync()
        {
            user = await securityService.GetCurrentUser();
            if (user != null)
            {
                orders = await orderData.GetOrders(user.Name);
            }
        }

        private void GoToOrder(OrderModel orderModel)
        {
            navigationManager.NavigateTo($"orders/{orderModel.Id}");
        }
    }
}
