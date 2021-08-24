﻿using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace shu_bike_shop.Pages
{
    public partial class OrderDetails
    {
        [Parameter] public int? Id { get; set; }

        public OrderModel orderModel { get; private set; }
        public User user { get; private set; }

        [Inject] private IOrderData orderData { get; set; }
        [Inject] private IProductData productData { get; set; }
        [Inject] private ISecurityService securityService { get; set; }

        private List<OrderProductDetailsModel> orderProductDetailsModel;
        private bool loaded;

        public async Task RefreshOrder()
        {
            loaded = false;

            if (user != null && Id.HasValue)
            {
                orderModel = await orderData.GetOrder(Id.Value, user.Name);

                if (orderModel != null)
                {
                    var orderProducts = await orderData.GetOrderProducts(orderModel.Id);

                    orderProductDetailsModel = new List<OrderProductDetailsModel>();
                    foreach (var item in orderProducts)
                    {
                        var product = await productData.GetProduct(item.ProductId);

                        orderProductDetailsModel.Add(new OrderProductDetailsModel()
                        {
                            ProductModel = product,
                            OrderProduct = item
                        });
                    }
                }
            }

            loaded = true;

            //StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            user = await securityService.GetCurrentUser();
            await RefreshOrder();
        }
    }
}
