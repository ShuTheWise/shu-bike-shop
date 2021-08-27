using Microsoft.AspNetCore.Components;
using shu_bike_shop.Shared;
using System;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public class ModalReference
    {
        private IModal compRef;

        public IModal CompRef
        {
            get => compRef;
            set
            {
                compRef = value;
            }
        }

        public RenderFragment RenderFragment { get; set; }

        public Task<ModalResult> GetModalResult()
        {
            while (compRef == null)
            {

            }
            //await Task.Delay(1000);
            return compRef.GetModalResult();
        }
    }

    public class ModalService
    {
        internal event Action<ModalReference> OnModalShow;
        internal event Action<ModalReference> OnModalClose;

        public async Task<bool> Confirm(string message)
        {
            ModalReference modalReference = new();
            modalReference.RenderFragment = GetRenderFragment<ConfirmModal>(message, modalReference);

            OnModalShow?.Invoke(modalReference);
            var modalResult = await modalReference.GetModalResult().ConfigureAwait(false);
            OnModalClose?.Invoke(modalReference);

            return true;
        }

        private RenderFragment GetRenderFragment<T>(string text, ModalReference modalReference) where T : notnull, IComponent
        {
            return new RenderFragment(builder =>
             {
                 builder.OpenComponent<T>(0);
                 builder.AddAttribute(1, "Text", text);
                 builder.AddComponentReferenceCapture(2, compRef => modalReference.CompRef = (IModal)compRef);
                 builder.CloseComponent();
             });
        }
    }
}
