using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApp.Models
{
    public class Client
    {

        [Key][Display(Name = "ID")] public int Id { get; set; }
        [Required][Display(Name = "Name")] public string Nom { get; set; }
        [Required][Display(Name = "Numero de tele")] public int Tele { get; set; }

        [Display(Name = "Image de Client")] public string? Image { get; set; }

        [Required][Display(Name = "Code de Suivi")] public int Code { get; set; }

        public int ReparateurId { get; set; }
        [ForeignKey("ReparateurId")]
        public Reparateur? Reparateur { get; set; }

        public virtual ICollection<Reparation>? Reparations { get; set; }
    }
}
