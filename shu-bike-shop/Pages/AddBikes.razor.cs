using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace shu_bike_shop.Pages
{
    public partial class AddBikes
    {
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private IJSRuntime runtime { get; set; }
        [Inject] private IBikesData bikesData { get; set; }

        private BikeCreateModel model = new();

        private async Task HandleValidSubmit()
        {
            try
            {
                var bike = await bikesData.AddBike(model);
                navigationManager.NavigateTo("/bikes");
            }
            catch
            {
                await runtime.Inform("Something went wrong");
            }
        }
    }
}
