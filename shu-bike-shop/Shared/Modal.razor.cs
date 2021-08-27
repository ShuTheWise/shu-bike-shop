using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace shu_bike_shop.Shared
{
    public partial class Modal
    {
        [Inject] private IModalService modalService { get; set; }
        [Inject] private IJSRuntime jsRuntime { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        private bool scrollLockActive;

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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (ChildContent != null && !scrollLockActive)
            {
                await jsRuntime.InvokeAsync<object>("blazorExtensions.ScrollLock", true);
                scrollLockActive = true;
            }
            else if (ChildContent == null && scrollLockActive)
            {
                await jsRuntime.InvokeAsync<object>("blazorExtensions.ScrollLock", false);
                scrollLockActive = false;
            }
        }
    }
}
