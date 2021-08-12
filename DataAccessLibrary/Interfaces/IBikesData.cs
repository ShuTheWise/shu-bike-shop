using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IBikesData
    {
        Task<List<BikeModel>> GetBikes();
        Task<BikeModel> AddBike(NewBikeModel model);
    }
}