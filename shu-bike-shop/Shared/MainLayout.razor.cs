using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace shu_bike_shop.Shared
{
    public partial class MainLayout
    {
        [Inject] private ModalService modelService { get; set; }

        private ConfirmModal confirmModal { get; set; }

        private bool DisplayConfirmModal { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            modelService.OnInform += ModelService_OnInform;
        }

        private void ModelService_OnInform(string text)
        {
            //confirmModal.Text = text;
            DisplayConfirmModal = true;
            StateHasChanged();
        }

        //protected override Task OnInitializedAsync()
        //{
        //    //return base.OnInitializedAsync();
        //}
        //public
    }
}
