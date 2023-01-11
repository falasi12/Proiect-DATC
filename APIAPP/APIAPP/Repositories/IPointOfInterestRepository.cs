using APIAPP.Models;
using LanguageExt;
using LanguageExt.Pipes;

namespace APIAPP.Repositories
{
    public interface IPointOfInterestRepository
    {
        Task<List<PointsOfInterestDTO>> TryGetPointsOfInterest();
        Task<bool> TryDeletePointOfInterest(int id, double latitude, double longitude, double radius);
        Task<bool> TryPostPointOfInterest(PointsOfInterestDTO poi);
        Task<bool> SimplePostPOI(PointsOfInterestDTO poi);


    }
}
