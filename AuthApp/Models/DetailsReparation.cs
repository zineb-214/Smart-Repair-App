using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApp.Models
{
    public class DetailsReparation
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int AppareilId { get; set; }

        [ForeignKey("AppareilId")]
        public virtual Appareil? Appareil { get; set; }

        [Required]
        public string IMEI { get; set; } = string.Empty;

        [Required]
        public string NumeroSerie { get; set; } = string.Empty;

        [Required]
        public float PrixTotalReparation { get; set; }

        [Required]
        public float PrixAvance { get; set; }

        //[NotMapped] 
        [Required]
        public float PrixReste => PrixTotalReparation - PrixAvance;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime DateFinEstime { get; set; }

        [Required]
        public string Statut { get; set; } = "En attente";

        [Required]
        public int ReparationId { get; set; }

        // Navigation Property
        [ForeignKey("ReparationId")]
        public virtual Reparation? Reparation { get; set; }


    }
}
