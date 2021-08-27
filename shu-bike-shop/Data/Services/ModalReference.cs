using Microsoft.AspNetCore.Components;
using shu_bike_shop.Shared;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public class ModalReference
    {
        public IModal Modal { get; set; }

        public RenderFragment RenderFragment { get; set; }

        public async Task<ModalResult> GetModalResult()
        {
            while (Modal == null)
            {
                await Task.Yield();
            }
            return await Modal.GetModalResult();
        }
    }
}
