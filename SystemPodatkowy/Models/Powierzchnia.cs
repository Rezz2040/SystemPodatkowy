using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemPodatkowy.Models
{
    public class Powierzchnia
    {
        [Key]
        public int PowierzchniaID { get; set; }

        public int PodatnikID { get; set; }

        [Required]
        [MaxLength(50)]
        public string ObrebEwidencyjny { get; set; }

        [Required]
        [MaxLength(20)]
        public string NumerDzialki { get; set; }

        [MaxLength(50)]
        public string? NumerKsiegiWieczystej {  get; set; }

        [MaxLength(100)]
        public string? Adres { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PowCalkowitaM2 { get; set; }

        [ForeignKey(nameof(PodatnikID))]
        public virtual Podatnik Podatnik { get; set; }

        public virtual ICollection<Wspolwlasnosc> Wspolwlasnosci { get; set; }
        public virtual ICollection<PozycjaDeklaracji> Pozycje { get; set; }
    }
}
