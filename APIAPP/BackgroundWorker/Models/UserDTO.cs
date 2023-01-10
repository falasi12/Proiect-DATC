namespace BackgroundWorker.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public int AdminRights { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; }

    }
}
