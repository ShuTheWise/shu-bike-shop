using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace shu_bike_shop.Shared
{
    public class ModalResult
    {

    }

    public interface IModal
    {
        Task<ModalResult> GetModalResult();
    }

    public partial class ConfirmModal : IModal
    {
        [Parameter]
        public string Text { get; set; } = "Lorem ipsum";

        public ModalResult modalResult;

        private void ModalCancel()
        {
            ModalResult m = new();
            modalResult = m;
        }

        private void ModalOk()
        {
            ModalResult m = new();
            modalResult = m;
        }

        public Task<ModalResult> GetModalResult()
        {
            while (modalResult == null)
            {

            }
            return Task.FromResult(modalResult);
        }
    }
}