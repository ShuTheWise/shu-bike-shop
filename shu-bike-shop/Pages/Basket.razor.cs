using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shu_bike_shop.Pages
{
    public partial class Basket
    {
        [Inject] private ModalService modalService{ get; set; }
        [Inject] private IJSRuntime jSRuntime { get; set; }
        [Inject] private IBasketService basketService { get; set; }
        [Inject] private IOrderData orderData { get; set; }
        [Inject] private IProductData productData { get; set; }
        [Inject] private ISecurityService securityService { get; set; }

        private User user;
        private bool CanPlaceOrder => user != null && user.CanPlaceOrder;
        private bool BuyingDisabled => !CanPlaceOrder;

        private decimal totalAmount;
        private List<BasketItemModel> basketItems;

        protected override async Task OnInitializedAsync()
        {
            user = await securityService.GetCurrentUser();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RefreshBasket();
                StateHasChanged();
            }
        }

        private async Task RefreshBasket()
        {
            basketItems = await basketService.GetBasketItems();
            RefreshTotalAmount();
        }

        private async Task PlaceOrder()
        {
            user = await securityService.GetCurrentUser();

            var orderModel = await orderData.AddOrder(new OrderCreateModel()
            {
                UserEmail = user.Name,
                TotalAmount = totalAmount,
                Items = basketItems.Select(x => new OrderProductModel
                {
                    ProductId = x.Product.Id,
                    UnitPrice = x.Product.Price,
                    Amount = x.Quantity
                }).ToList()
            });

            foreach (var item in basketItems)
            {
                await productData.DecrementProductAmount(item.Product.Id, item.Quantity);
            }

            await basketService.ClearBasket();
            basketItems = new List<BasketItemModel>();

            await modalService.Inform($"Your order is placed, your Order number is { orderModel.Id }");
            await jSRuntime.Confirm($"Your order is placed, your Order number is {orderModel.Id}");
        }

        private void RefreshTotalAmount()
        {
            totalAmount = 0;
            foreach (var item in basketItems)
            {
                totalAmount += item.Price;
            }
        }

        private async Task RemoveProduct(int id)
        {
            if (await jSRuntime.Confirm("Remove product from the basket?"))
            {
                basketItems.RemoveAll(x => x.Product.Id == id);

                await UpdateBasket();
                await RefreshBasket();
            }
        }

        private async Task SetQuantity(ChangeEventArgs changeEventArgs, BasketItemModel basketItemModel)
        {
            basketItemModel.Quantity = int.Parse(changeEventArgs.Value.ToString());
            RefreshTotalAmount();
            await basketService.SetBasketItems(basketItems);
        }

        private async Task UpdateBasket()
        {
            await basketService.SetBasketItems(basketItems);
        }

        private async Task ClearBasket()
        {
            basketItems = null;
            StateHasChanged();

            await basketService.ClearBasket();
            await RefreshBasket();
        }
    }
}
