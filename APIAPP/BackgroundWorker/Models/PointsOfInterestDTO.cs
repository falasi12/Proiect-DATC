namespace BackgroundWorker.Models
{
    public class PointsOfInterestDTO
    {
        public int Id { get; set; }
        public int Radius { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public bool Archived { get; set; }

        public PointsOfInterestDTO(int id, int radius, int latitude, int longitude, bool archived = false) {
            Id = id;
            Radius = radius; 
            Latitude = latitude; 
            Longitude = longitude;
            Archived = archived;
        }
    }
}
