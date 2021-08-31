using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace shu_bike_shop.Pages
{
    public partial class Bikes
    {
        [Inject] private IProductData productData { get; set; }
        [Inject] private IBasketModalService basketModalService { get; set; }

        private List<BikeModel> bikes;

        protected override async Task OnInitializedAsync()
        {
            bikes = await productData.GetProducts<BikeModel>();
        }

        private async Task AddProductToBasket(ProductModel productModel)
        {
            await basketModalService.AddProductToBasket(productModel);
        }
    }
}