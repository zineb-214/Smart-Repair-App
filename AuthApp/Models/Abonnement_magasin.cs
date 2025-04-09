using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthApp.Models
{
    public class Abonnement_magasin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Id_magasin { get; set; }

        [ForeignKey("Id_magasin")]
        public required Magasin Magasin { get; set; }

        [Required]
        public DateTime Date_debut { get; set; }

        public DateTime Date_expiration { get; private set; }

        [Required]
        [MaxLength(20)] // Limite la longueur de la chaîne
        public string Statut { get; set; } = "Actif"; // Valeur par défaut
        public Abonnement_magasin()
        {
            Date_debut = DateTime.Now;
            Date_expiration = Date_debut.AddMonths(1);
        }
    }
}
