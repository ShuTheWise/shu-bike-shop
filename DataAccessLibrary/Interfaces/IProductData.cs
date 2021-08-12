using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IProductData
    {
        Task<List<ProductModel>> GetProducts();
        Task DecrementProductAmount(int id, int amount);
        Task<ProductModel> AddProduct(ProductCreateModel model);
        Task<ProductModel> UpdateProduct(ProductUpdateModel model);
        Task RemoveProduct(int id);
    }
}