using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;
using PaymentAccessService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace shu_bike_shop.Pages
{
    public partial class Product
    {
        [Parameter]
        public int Id { get; set; }

        [Inject] private IProductData productData { get; set; }
        [Inject] private IBasketModalService basketModalService { get; set; }

        private Dictionary<string, object> descProps;

        private ProductModel productModel;

        protected override async Task OnInitializedAsync()
        {
            productModel = await productData.GetProduct(Id);
            descProps = productModel.GetDescFields();
        }
    }
}
