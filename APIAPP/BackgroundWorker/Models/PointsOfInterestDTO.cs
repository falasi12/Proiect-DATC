namespace BackgroundWorker.Models
{
    public class PointsOfInterestDTO
    {
        public int Id { get; set; }
        public double Radius { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Archived { get; set; }

        public PointsOfInterestDTO(int id, double radius, double latitude, double longitude, bool archived = false) {
            Id = id;
            Radius = radius; 
            Latitude = latitude; 
            Longitude = longitude;
            Archived = archived;
        }
    }
}
