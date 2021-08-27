using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace shu_bike_shop.Shared
{
    public partial class ConfirmModal : IModal
    {
        [Parameter]
        public string Text { get; set; }

        private ModalResult modalResult;

        private void ModalCancel()
        {
            modalResult = ModalResult.Cancel();
        }

        private void ModalOk()
        {
            modalResult = ModalResult.Ok();
        }

        public async Task<ModalResult> GetModalResult()
        {
            while (modalResult == null)
            {
                await Task.Yield();
            }
            return await Task.FromResult(modalResult);
        }
    }
}