using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace shu_bike_shop.Shared
{
    public partial class InformModal : IModal
    {
        [Parameter]
        public string Text { get; set; }

        private ModalResult modalResult;

        public async Task<ModalResult> GetModalResult()
        {
            while (modalResult == null)
            {
                await Task.Yield();
            }

            return modalResult;
        }

        private void ModalOk()
        {
            modalResult = ModalResult.Ok();
        }
    }
}
