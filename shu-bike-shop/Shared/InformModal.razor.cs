using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shu_bike_shop.Shared
{
    public partial class InformModal
    {
        [Parameter]
        public string Text { get; set; } = "Lorem ipsum";

        public EventCallback OnClose { get; set; }

        private Task ModalOk()
        {
            return OnClose.InvokeAsync();
        }
    }
}
