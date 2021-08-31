using DataAccessLibrary.Models;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public class BasketModalService : IBasketModalService
    {
        private readonly IBasketService basketService;
        private readonly IModalService modalService;

        public BasketModalService(IBasketService basketService, IModalService modalService)
        {
            this.basketService = basketService;
            this.modalService = modalService;
        }

        public async Task AddProductToBasket(ProductModel productModel)
        {
            var add = await modalService.Confirm($"Add {productModel.Name} to the basket?");

            if (add)
            {
                var b = await basketService.AddProduct(productModel);

                if (!b)
                {
                    if (await modalService.Confirm("Item already in the basket, add again?"))
                    {
                        try
                        {
                            await basketService.RaiseQuantity(productModel.Id);
                        }
                        catch (InsufficientProductAmountException ex)
                        {
                            await modalService.Inform(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
