using System;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public interface IModalService
    {
        event Action<ModalReference> OnModalShow;
        event Action<ModalReference> OnModalClose;

        Task<bool> Confirm(string message);
        Task Inform(string message);
        Task<ModalResult> Show<T>(string message) where T : notnull, IComponent;
    }
}