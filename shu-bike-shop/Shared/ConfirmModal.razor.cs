using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shu_bike_shop.Shared
{
    public class ModalResult
    {

    }

    public interface IModal
    {
        event Action<ModalResult> OnClose;
    }

    public partial class ConfirmModal : IModal
    {

        [Parameter]
        public string Text { get; set; } = "Lorem ipsum";

        public event Action<ModalResult> OnClose;

        private void ModalCancel()
        {
            ModalResult m = new();
            OnClose?.Invoke(m);
        }

        private void ModalOk()
        {
            ModalResult m = new();
            OnClose?.Invoke(m);
        }

        //public Task<ModalResult> 
    }
}
