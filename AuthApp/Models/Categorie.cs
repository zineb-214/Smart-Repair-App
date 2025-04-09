using System.ComponentModel.DataAnnotations;

namespace AuthApp.Models
{
    public class Categorie
    {

        [Key]
        public int Id { get; set; }
        public required string Nom { get; set; }


    }
}
