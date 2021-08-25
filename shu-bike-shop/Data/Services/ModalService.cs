using System;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public class ModalService
    {
        public event Action<string> OnInform;

        public async Task Inform(string text)
        {
            OnInform(text);
        }
    }
}
