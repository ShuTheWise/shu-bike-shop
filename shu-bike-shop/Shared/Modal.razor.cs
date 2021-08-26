using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shu_bike_shop.Shared
{
    public partial class Modal
    {
        private ModalReference modalReference;

        [Inject] private ModalService modalService { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            modalService.OnModalShow += ModalService_OnShowModal;
        }

        private async void ModalService_OnShowModal(ModalReference obj)
        {
            ChildContent = obj.RenderFragment;
            await InvokeAsync(StateHasChanged); 
            modalReference = obj;
            modalReference.OnCompRefSet += ModalReference_OnCompRefChanged;
        }

        private void ModalReference_OnCompRefChanged(IModal obj)
        {
            obj.OnClose += Modal_OnClose;
        }

        private void Modal_OnClose(ModalResult obj)
        {
            modalService.ModalClosed(modalReference, obj);

            ChildContent = null;
            modalReference = null;
            StateHasChanged();
        }
    }
}
