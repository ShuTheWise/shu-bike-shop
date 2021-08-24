using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IProductData
    {
        //Task<List<ProductModel>> GetProducts();
        Task DecrementProductAmount(int id, int amount);
        Task AddProduct<T, U>(U createModel) where U : ProductCreateModel where T : ProductModel;
        Task UpdateProduct<T>(int id, dynamic updateModel) where T : ProductModel;
        Task RemoveProduct(int id);
        Task<ProductModel> GetProduct(int id);
        Task<List<T>> GetProducts<T>() where T : ProductModel;
    }
}