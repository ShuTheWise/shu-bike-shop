using Microsoft.AspNetCore.Components;

namespace shu_bike_shop.Shared
{
    public partial class Modal
    {
        [Inject] private ModalService modalService { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            modalService.OnModalShow += ModalService_OnShowModal;
            modalService.OnModalClose += ModalService_OnModalClose;
        }

        private async void ModalService_OnModalClose(ModalReference obj)
        {
            ChildContent = null;
            await InvokeAsync(StateHasChanged);
        }

        private async void ModalService_OnShowModal(ModalReference obj)
        {
            ChildContent = obj.RenderFragment;
            await InvokeAsync(StateHasChanged);
        }
    }
}
