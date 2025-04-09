namespace AuthApp.Models
{
    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? UserType { get; set; } // "Admin", "Reparateur", or "Magasin"

    }
}
