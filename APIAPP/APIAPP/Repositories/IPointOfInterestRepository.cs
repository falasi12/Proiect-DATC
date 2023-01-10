using APIAPP.Models;
using LanguageExt;
using LanguageExt.Pipes;

namespace APIAPP.Repositories
{
    public interface IPointOfInterestRepository
    {
        Task<List<PointsOfInterestDTO>> TryGetPointsOfInterest();
        Task<bool> TryDeletePointOfInterest(int id, double latitude, double longitude, double radius);
        Task<bool> TryPostPointOfInterest(double latitude, double longitude, double radius);
        Task<bool> SimplePostPOI(double latitude, double longitude, double radius);


    }
}
