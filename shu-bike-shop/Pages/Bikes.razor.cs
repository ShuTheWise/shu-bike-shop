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
        [Inject] private IBikesData bikesData { get; set; }
        [Inject] private IBasketService basket { get; set; }
        [Inject] private IJSRuntime jSRuntime { get; set; }

        private List<BikeModel> bikes;

        protected override async Task OnInitializedAsync()
        {
            bikes = await bikesData.GetBikes();
        }

        private async Task AddProductToBasket(ProductModel productModel)
        {
            try
            {
                var add = await jSRuntime.Confirm($"Add {productModel.Name} to the basket?");

                if (add)
                {
                    await basket.AddProduct(productModel, () => jSRuntime.Confirm("Item already in the basket, add again?"));
                }
            }
            catch (InsufficientProductAmountException ex)
            {
                await jSRuntime.Inform(ex.Message);
            }
        }
    }
}
