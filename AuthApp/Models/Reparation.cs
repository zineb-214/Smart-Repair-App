
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApp.Models
{
    public class Reparation
    {
        [Key] public int Id { get; set; }

        [Display(Name = "Date de reception")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Required]
        public DateTime DateReception { get; set; }

        [Required] public int IdClient { get; set; }

        [Required] public int IdReparateur { get; set; }


        [ForeignKey("IdClient")]
        public virtual Client? Client { get; set; }

        [ForeignKey("IdReparateur")]
        public virtual Reparateur? Reparateur { get; set; }

        public virtual ICollection<DetailsReparation>? DetailsReparations { get; set; }
    }
}
