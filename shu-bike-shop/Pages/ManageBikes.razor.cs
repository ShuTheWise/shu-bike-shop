using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace shu_bike_shop.Pages
{
    public partial class ManageBikes
    {
        [Inject] private IProductData productData { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private List<BikeModel> bikes;

        protected override async Task OnInitializedAsync()
        {
            bikes = await productData.GetProducts<BikeModel>();
        }

        private async Task DeleteBike(ProductModel productModel)
        {
            await productData.RemoveProduct(productModel.Id);
            await OnInitializedAsync();
        }

        private async Task UpdateBike(BikeModel bikeModel)
        {
            dynamic updateModel = new
            {
                amount = bikeModel.Amount,
                make = bikeModel.Make,
                model = bikeModel.Model,
                price = bikeModel.Price,
                year = bikeModel.Year,
                imageurl = bikeModel.ImageUrl,
            };

            await productData.UpdateProduct<BikeModel>(bikeModel.Id, updateModel);

            await OnInitializedAsync();
        }
    }
}
