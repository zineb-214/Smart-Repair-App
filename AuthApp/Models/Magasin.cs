using System.ComponentModel.DataAnnotations;

namespace AuthApp.Models
{
    public class Magasin
    {
        [Key][Display(Name = "ID")] public int Id { get; set; }
        [Required][Display(Name = "Name")] public string? Nom { get; set; }
        [StringLength(100)] public string? UserName { get; set; }

        [StringLength(10)] public string? Password { get; set; }
        [Required][Display(Name = "Adresse")] public string? Adresse { get; set; }
        [Required][Display(Name = "Ville")] public string? Ville { get; set; }
        [Required][Display(Name = "Patent")] public string? Patent { get; set; }

        public DateTime Created_at { get; set; }

    }
}
