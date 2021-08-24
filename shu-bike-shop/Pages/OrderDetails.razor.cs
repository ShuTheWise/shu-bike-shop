using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;

namespace shu_bike_shop.Pages
{
    public partial class OrderDetails
    {
        [Parameter]
        public int? Id { get; set; }

        [Inject] private IOrderData orderData { get; set; }
        [Inject] private ISecurityService securityService { get; set; }

        private bool loaded;
        private User user;
        private OrderModel orderModel;

        public async Task RefreshOrder()
        {
            loaded = false;

            if (user != null && Id.HasValue)
            {
                orderModel = await orderData.GetOrder(Id.Value, user.Name);
            }

            loaded = true;
        }

        protected override async Task OnInitializedAsync()
        {
            user = await securityService.GetCurrentUser();
            await RefreshOrder();
        }
    }
}