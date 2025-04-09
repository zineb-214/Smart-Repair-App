using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AuthApp.Models
{
    public class Reparateur
    {
        [Key][Display(Name = "ID")] public int reparateurID { get; set; }


        [Required][StringLength(100)] public string Nom { get; set; }


        public bool Role { get; set; }

        [Range(0, 100)] public float TaxPer { get; set; }

        [StringLength(100)] public string UserName { get; set; }

        [StringLength(10)] public string Password { get; set; }

        public int MagasinId { get; set; }
        [ForeignKey("MagasinId")]
        public Magasin? Magasin { get; set; }
        public ICollection<Reparation> Reparations { get; set; }

    }
}
