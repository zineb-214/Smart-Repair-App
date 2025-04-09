using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthApp.Models
{
    public class Appareil
    {

        [Key]
        public int Id { get; set; }

        public required string Nom { get; set; }

        // Clé étrangère vers Categorie
        public int CategorieId { get; set; }

        // Navigation vers Categorie
        [ForeignKey("CategorieId")]
        public Categorie? Categorie { get; set; }


    }
}
