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
        [Inject] private IModalService modalService { get; set; }

        private List<BikeModel> bikes;

        protected override async Task OnInitializedAsync()
        {
            bikes = await productData.GetProducts<BikeModel>();
        }

        private async Task AddProductToBasket(ProductModel productModel)
        {
            var add = await modalService.Confirm($"Add {productModel.Name} to the basket?");

            if (add)
            {
                var b = await basketService.AddProduct(productModel);

                if (!b)
                {
                    if (await modalService.Confirm("Item already in the basket, add again?"))
                    {
                        try
                        {
                            await basketService.RaiseQuantity(productModel.Id);
                        }
                        catch (InsufficientProductAmountException ex)
                        {
                            await modalService.Inform(ex.Message);
                        }
                    }
                }
            }
        }
    }
}