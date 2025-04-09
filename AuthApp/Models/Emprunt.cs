using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthApp.Models
{
    public class Emprunt
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Collegue")]
        [Display(Name = "Collègue emprunteur")]
        public int CollegueId { get; set; }

        // Navigation Property
        public virtual Collegue? Collegue { get; set; }

        [Required]
        [Display(Name = "Type d'emprunt")]
        
        public TypeEmprunt TypeEmprunt { get; set; }

        [Required]
        [Display(Name = "Montant")]
        public float Montant { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }

    public enum TypeEmprunt
    {
        Empruntes,        // Borrowed
        Remboursee,       // Reimbursed
        PropreEmpruntes,  // Own borrowed
        PropreRemboursee  // Own reimbursed
    }
}
