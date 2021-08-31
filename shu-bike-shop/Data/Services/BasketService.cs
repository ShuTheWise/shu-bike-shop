using DataAccessLibrary;
using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public class BasketService : IBasketService
    {
        private readonly IProductData productData;
        private readonly ICookieService cookieService;

        private const string ServiceBusyCookie = "ServiceBusy";
        private const string ProductsCookieName = "BasketProducts";

        public BasketService(ICookieService cookieService, IProductData productData)
        {
            this.cookieService = cookieService;
            this.productData = productData;
        }

        public async Task<bool> AddProduct(ProductModel productModel)
        {

            var basketItem = await GetBasketItem(productModel.Id);

            var canAdd = basketItem == null;
            if (canAdd)
            {
                var products = await GetBasketItems();
                products.Add(new BasketItemModel() { Product = productModel, Quantity = 1 });

                await UpdateCookie(products);
            }

            return canAdd;
        }

        public async Task<BasketItemModel> GetBasketItem(int productId)
        {
            var basketItems = await GetBasketItems();
            var basketItem = basketItems.FirstOrDefault(x => x.Product.Id == productId);
            return basketItem;
        }

        public async Task RemoveProduct(int productId)
        {
            var basketItems = await GetBasketItems();
            basketItems.RemoveAll(x => x.Product.Id == productId);
            await UpdateCookie(basketItems);
        }

        public async Task RaiseQuantity(int productId)
        {
            var basketItems = await GetBasketItems();
            var basketItem = basketItems.FirstOrDefault(x => x.Product.Id == productId);

            if (basketItem.Quantity + 1 > basketItem.Product.Amount)
            {
                throw new InsufficientProductAmountException("Insufficient product amount");
            }

            basketItem.Quantity++;

            await UpdateCookie(basketItems);
        }

        public async Task ClearBasket()
        {
            await UpdateCookie(new List<BasketItemModel>());
        }

        private async Task UpdateCookie(List<BasketItemModel> items)
        {
            await SetBusy(true);
            var list = items.Select(x => new ProductCookieModel() { ProductId = x.Product.Id, Quantity = x.Quantity }).ToList();
            await cookieService.WriteCookie(ProductsCookieName, list.ToBson(), 10);
            await SetBusy(false);
        }

        private async Task SetBusy(bool b)
        {
            await cookieService.WriteCookie(ServiceBusyCookie, b, 1);
        }

        private async Task<bool> IsBusy()
        {
            var busy = await cookieService.GetCookie<string>(ServiceBusyCookie);

            if (string.IsNullOrEmpty(busy))
            {
                return false;
            }

            return bool.Parse(busy);
        }

        private async Task<List<BasketItemModel>> GetCookie()
        {
            var readCookie = await cookieService.GetCookie<string>(ProductsCookieName);
            try
            {
                var basketItems = new List<BasketItemModel>();

                if (!string.IsNullOrEmpty(readCookie))
                {
                    var productCookies = SerializationExtensions.FromBson<ProductCookieModel>(readCookie);
                    foreach (var cookie in productCookies)
                    {
                        try
                        {
                            BasketItemModel basketItem = new()
                            {
                                Product = await productData.GetProduct(cookie.ProductId),
                                Quantity = cookie.Quantity
                            };

                            basketItems.Add(basketItem);
                        }
                        catch
                        {

                        }
                    }

                    return basketItems;
                }
            }
            catch
            {
                await ClearBasket();
            }

            return new List<BasketItemModel>();
        }

        public async Task<List<BasketItemModel>> GetBasketItems()
        {
            while (await IsBusy())
            {
                await Task.Delay(250);
            }

            return await GetCookie();
        }

        public Task SetBasketItems(List<BasketItemModel> basketItems)
        {
            return UpdateCookie(basketItems);
        }
    }
}
