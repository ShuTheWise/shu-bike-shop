using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public static class ConfirmExtensions
    {
        public static Task<bool> Confirm(this IJSRuntime jsRuntime, string message)
        {
            return jsRuntime.InvokeAsync<bool>("confirm", message).AsTask();
        }
        public static Task Inform(this IJSRuntime jsRuntime, string message)
        {
            return jsRuntime.InvokeVoidAsync("alert", message).AsTask();
        }
    }
}
