using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public class CookieService : ICookieService
    {
        private readonly IJSRuntime jsRuntime;

        public CookieService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public Task<T> GetCookie<T>(string name)
        {
            return jsRuntime.InvokeAsync<T>("blazorExtensions.GetCookie", name).AsTask();
        }

        public async Task WriteCookie(string name, object value, int days)
        {
            await jsRuntime.InvokeAsync<object>("blazorExtensions.WriteCookie", name, value, days);
        }
    }
}
