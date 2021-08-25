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
        [Inject] private IBasketService basketService { get; set; }
        [Inject] private IJSRuntime jSRuntime { get; set; }

        private List<BikeModel> bikes;

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(500);
            bikes = await productData.GetProducts<BikeModel>();
        }

        private async Task AddProductToBasket(ProductModel productModel)
        {
            var add = await jSRuntime.Confirm($"Add {productModel.Name} to the basket?");

            if (add)
            {
                var b = await basketService.AddProduct(productModel);

                if (!b)
                {
                    if (await jSRuntime.Confirm("Item already in the basket, add again?"))
                    {
                        try
                        {
                            await basketService.RaiseQuantity(productModel.Id);
                        }
                        catch (InsufficientProductAmountException ex)
                        {
                            await jSRuntime.Inform(ex.Message);
                        }
                    }
                }
            }
        }
    }
}