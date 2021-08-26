using Microsoft.AspNetCore.Components;
using shu_bike_shop.Shared;
using System;

namespace shu_bike_shop
{
    public class ModalReference
    {
        public event Action<IModal> OnCompRefSet;

        private IModal compRef;

        public IModal CompRef
        {
            get => compRef; 
            set
            {
                if (value != null && compRef != value && OnCompRefSet != null)
                {
                    OnCompRefSet(value);
                }
                compRef = value;
            }
        }

        public RenderFragment RenderFragment { get; set; }
    }

    public class ModalService
    {
        //internal event Action<ModalReference> OnModalClose;
        internal event Action<ModalReference> OnModalShow;

        public bool Confirm(string message)
        {
            ModalReference modalReference = new();
            modalReference.RenderFragment = GetRenderFragment<ConfirmModal>(message, modalReference);
            OnModalShow?.Invoke(modalReference);

            return false;
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

        public void ModalClosed(ModalReference modalRefernce, ModalResult modalResult)
        {

        }
    }
}
