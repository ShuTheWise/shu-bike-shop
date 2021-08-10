using DataAccessLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public class BasketService : IBasketService
    {
        private List<BasketItem> Products { get; } = new List<BasketItem>();

        public async Task AddProduct(ProductModel productModel, Func<Task<bool>> raiseQuantity = null)
        {
            var basketItem = await GetBasketItem(productModel);

            if (basketItem != null)
            {
                if (raiseQuantity != null)
                {
                    bool result = await raiseQuantity();

                    if (result)
                    {
                        await RaiseQuantity(productModel);
                    }
                }
            }
            else
            {
                Products.Add(new BasketItem() { Product = productModel });
            }
        }

        public async Task<BasketItem> GetBasketItem(ProductModel productModel)
        {
            var basketItem = Products.FirstOrDefault(x => x.Product == productModel);
            return basketItem;
        }

        public async Task RemoveProduct(ProductModel productModel)
        {
            var basketItem = await GetBasketItem(productModel);
            Products.Remove(basketItem);
        }

        public async Task RaiseQuantity(ProductModel productModel)
        {
            var basketItem = Products.FirstOrDefault(x => x.Product == productModel);

            if (basketItem.Quantity + 1 > basketItem.Product.Amount)
            {
                throw new InsufficientProductAmountException("Insufficient product amount");
            }

            basketItem.Quantity++;
        }

        public Task<List<BasketItem>> GetBasketItems()
        {
            return Task.FromResult(Products);
        }
    }
}
