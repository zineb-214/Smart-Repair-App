using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApp.Models
{
    public class RecetteReparateur
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public float TotalReparations { get; set; } = 0; // Accumulates total repairs

        [Required]
        public float TotalEmprunts { get; set; } = 0; // Sum of all colleagues' loans

        [Required]
        public float RecetteTotale { get; set; } = 0; // Final revenue

        [Required]
        public float TaxReparateur { get; set; } = 0; // Calculated tax

        [Required]
        public DateTime Date { get; set; } = DateTime.Now; // Daily record

        [Required]
        [ForeignKey("Reparateur")]
        public int ReparateurId { get; set; }
        public Reparateur Reparateur { get; set; }

        public int? RecetteMagasinId { get; set; }
        public RecetteMagasin? RecetteMagasin { get; set; }

        // Method to calculate values correctly
        public void CalculerValeurs(float prixTotalReparation, float totalEmpruntCollegue, float pourcentageTaxe)
        {
            // Accumulate total repairs for the day
            TotalReparations = prixTotalReparation; // Set or update total repairs

            // Update loans from colleagues
            TotalEmprunts = totalEmpruntCollegue;

            // Calculate the total revenue (RecetteTotale) based on repairs and loans
            RecetteTotale = TotalReparations ;

            // Calculate tax based on the total revenue and the tax percentage
            TaxReparateur = RecetteTotale * (pourcentageTaxe / 100);
        }
    }
}
