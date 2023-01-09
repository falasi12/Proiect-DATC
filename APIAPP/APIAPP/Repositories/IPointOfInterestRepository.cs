using APIAPP.Models;
using LanguageExt;

namespace APIAPP.Repositories
{
    public interface IPointOfInterestRepository
    {
        Task<List<PointsOfInterestDTO>> TryGetPointsOfInterest();
        Task<bool> TryDeletePointOfInterest(int id, int latitude, int longitude, int radius);
    }
}
