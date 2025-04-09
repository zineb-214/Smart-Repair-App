using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApp.Models
{
 
        public class Paiement
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [ForeignKey("Magasin")]
            public int MagasinId { get; set; }

            public Magasin? Magasin { get; set; }

            [Required]
            [StringLength(16, MinimumLength = 13, ErrorMessage = "Le numéro de carte doit contenir entre 13 et 16 chiffres.")]
            public string NumeroCarte { get; set; } = string.Empty;

            [Required]
            [StringLength(4, MinimumLength = 3, ErrorMessage = "CVV doit contenir 3 ou 4 chiffres.")]
            public string CVV { get; set; } = string.Empty;

            [Required]
            [StringLength(50)]
            public string MethodePaiement { get; set; } = string.Empty;

            [Required]
            public DateTime DatePaiement { get; set; } = DateTime.Now;
        }
    

}
