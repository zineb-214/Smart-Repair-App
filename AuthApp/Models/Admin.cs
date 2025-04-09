// Models/Admin.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthApp.Models
{
    public class Admin
    {
        [Key]
        [JsonIgnore]  // Hide in API responses
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [JsonIgnore]  // Never return password in responses
        public string Password { get; set; }
    }
}