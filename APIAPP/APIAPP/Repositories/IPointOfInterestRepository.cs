using APIAPP.Models;
using LanguageExt;
using LanguageExt.Pipes;

namespace APIAPP.Repositories
{
    public interface IPointOfInterestRepository
    {
        Task<List<PointsOfInterestDTO>> TryGetPointsOfInterest();
        Task<bool> TryDeletePointOfInterest(int id, int latitude, int longitude, int radius);
        Task<bool> TryPostPointOfInterest(int id, int latitude, int longitude, int radius);
    }
}
