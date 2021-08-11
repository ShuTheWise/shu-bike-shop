﻿using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public interface IBasketService
    {
        Task AddProduct(ProductModel productModel, Func<Task<bool>> raiseQuantity);
        Task ClearBasket();
        Task<List<BasketItemModel>> GetBasketItems();
        Task RemoveProduct(ProductModel productModel);
    }
}