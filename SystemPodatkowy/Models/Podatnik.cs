using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemPodatkowy.Models
{
    public class Podatnik
    {
        [Key]
        public int PodatnikID { get; set; }

        [Required]
        [MaxLength(50)]
        public string TypPodmiotu { get; set; }

        [Required]
        [MaxLength(100)]
        public string NazwaWlasna_Nazwisko { get; set; }

        [MaxLength(50)]
        public string? Imie {  get; set; }

        [MaxLength(10)]
        [Column(TypeName = "char(10)")]
        public string? NIP {  get; set; }

        [MaxLength(11)]
        [Column(TypeName = "char(11)")]
        public string? PESEL { get; set; }

        [Required]
        [MaxLength(50)]
        public string Miejscowosc { get; set; }
        [Required]
        [MaxLength(6)]
        [Column(TypeName = "char(6)")]
        public string KodPocztowy { get; set; }

        [Required]
        [MaxLength(50)]
        public string Ulica { get; set; }

        [Required]
        [MaxLength(20)]
        public string NumerDomuLokal {  get; set; }

        public virtual ICollection<Wplata> Wplaty { get; set; }
        public virtual ICollection<Deklaracja> Deklaracje { get; set; }
        public virtual ICollection<Powierzchnia> Powierzchnie { get; set; }
        public virtual ICollection<Wspolwlasnosc> Wspolwlasnosci { get; set; }
    }
}
