using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace shu_bike_shop.Pages
{
    public partial class ManageBikes
    {
        [Inject] private IBikesData bikesData { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private List<BikeModel> bikes;

        protected override async Task OnInitializedAsync()
        {
            bikes = await bikesData.GetBikes();
        }

        private async Task DeleteBike(ProductModel productModel)
        {
            await bikesData.RemoveBike(productModel.Id);
        }

        private async Task UpdateBike(BikeModel bikeModel)
        {
            var updateModel = new BikeUpdateModel()
            {
                Id = bikeModel.Id,
                Amount = bikeModel.Amount,
                Make = bikeModel.Make,
                Model = bikeModel.Model,
                Price = bikeModel.Price,
                Year = bikeModel.Year
            };

            await bikesData.UpdateBike(updateModel);

            NavigationManager.NavigateTo("/bikes");
        }
    }
}
