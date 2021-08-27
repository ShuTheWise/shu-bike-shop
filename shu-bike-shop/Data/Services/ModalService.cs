using Microsoft.AspNetCore.Components;
using shu_bike_shop.Shared;
using System;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public class ModalService : IModalService
    {
        public event Action<ModalReference> OnModalShow;
        public event Action<ModalReference> OnModalClose;

        public async Task<ModalResult> Show<T>(string message) where T : notnull, IComponent
        {
            ModalReference modalReference = new();
            modalReference.RenderFragment = GetRenderFragment<T>(message, modalReference);

            OnModalShow?.Invoke(modalReference);
            var modalResult = await modalReference.GetModalResult().ConfigureAwait(false);
            OnModalClose?.Invoke(modalReference);

            return modalResult;
        }

        public async Task<bool> Confirm(string message)
        {
            var modalResult = await Show<ConfirmModal>(message);

            return !modalResult.Cancelled;
        }

        public async Task Inform(string message)
        {
            await Show<InformModal>(message);
        }

        private RenderFragment GetRenderFragment<T>(string text, ModalReference modalReference) where T : notnull, IComponent
        {
            return new RenderFragment(builder =>
             {
                 builder.OpenComponent<T>(0);
                 builder.AddAttribute(1, "Text", text);
                 builder.AddComponentReferenceCapture(2, compRef => modalReference.Modal = (IModal)compRef);
                 builder.CloseComponent();
             });
        }
    }
}
