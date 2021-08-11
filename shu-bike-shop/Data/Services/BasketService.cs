using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public class BasketService : IBasketService
    {
        private List<BasketItemModel> Products { get; set; } = new List<BasketItemModel>();

        public async Task AddProduct(ProductModel productModel, Func<Task<bool>> raiseQuantity = null)
        {
            var productId = productModel.Id;
            var basketItem = await GetBasketItem(productId);

            if (basketItem != null)
            {
                if (raiseQuantity != null)
                {
                    bool result = await raiseQuantity();

                    if (result)
                    {
                        await RaiseQuantity(productId);
                    }
                }
            }
            else
            {
                Products.Add(new BasketItemModel() { Product = productModel });
            }
        }

        public async Task<BasketItemModel> GetBasketItem(int productId)
        {
            var basketItem = Products.FirstOrDefault(x => x.Product.Id == productId);
            return basketItem;
        }

        public async Task RemoveProduct(int productId)
        {
            var basketItem = await GetBasketItem(productId);
            Products.Remove(basketItem);
        }

        public async Task RaiseQuantity(int productId)
        {
            var basketItem = await GetBasketItem(productId);

            if (basketItem.Quantity + 1 > basketItem.Product.Amount)
            {
                throw new InsufficientProductAmountException("Insufficient product amount");
            }

            basketItem.Quantity++;
        }

        public Task<List<BasketItemModel>> GetBasketItems()
        {
            return Task.FromResult(Products);
        }

        public Task ClearBasket()
        {
            Products = new List<BasketItemModel>();
            return Task.FromResult(true);
        }
    }
}
