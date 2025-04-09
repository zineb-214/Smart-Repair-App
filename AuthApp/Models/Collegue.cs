using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthApp.Models
{
    public class Collegue
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Required]
        [Display(Name = "Numéro de téléphone")]
        public string NumeroTelephone { get; set; }

        // Foreign Key for Reparateur
        [Required]
        [ForeignKey("Reparateur")]
        [Display(Name = "Réparateur")]
        public int ReparateurId { get; set; }

        // Navigation Property for Reparateur
        public virtual Reparateur? Reparateur { get; set; }

        // Navigation Property: One Collegue has many Emprunts
        public virtual ICollection<Emprunt>? Emprunts { get; set; } = new List<Emprunt>();

        // Computed Property: Total Emprunt (Σ Montant for each Collegue)
        [NotMapped]
        public float TotalEmprunt => Emprunts?.Sum(e => e.Montant) ?? 0;
    }
}
