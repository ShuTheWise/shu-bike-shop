using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IBikesData
    {
        Task<List<BikeModel>> GetBikes();
        Task<BikeModel> AddBike(BikeCreateModel model);
        Task RemoveBike(int id);
        Task<BikeModel> UpdateBike(BikeUpdateModel model);
    }
}