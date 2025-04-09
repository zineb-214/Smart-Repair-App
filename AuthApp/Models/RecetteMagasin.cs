using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthApp.Models
{
    public class RecetteMagasin
    {

        [Key]
        public int Id { get; set; }

        [Display(Name = "Recette Totale")]
        [Column(TypeName = "decimal(18,2)")]
        public float RecetteTotaleMagasin { get; set; }

        [Display(Name = "Magasin")]
        [Required]
        public string NomMagasin { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        [Display(Name = "Taxe Totale")]
        [Column(TypeName = "decimal(18,2)")]
        public float TotalTax { get; set; }

        // Clé étrangère vers Magasin
        [ForeignKey("Magasin")] 
        public int MagasinId { get; set; }

        // Navigation
        public Magasin Magasin { get; set; } = null!;

        // Relation 1-n avec RecetteReparateur
        public ICollection<RecetteReparateur> RecettesReparateurs { get; set; } = new List<RecetteReparateur>();


    }
}
