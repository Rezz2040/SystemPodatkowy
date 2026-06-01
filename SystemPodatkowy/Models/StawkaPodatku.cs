using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemPodatkowy.Models
{
    public class StawkaPodatku
    {
        [Key]
        public int StawkaID { get; set; }

        public int RokPodatkowy {  get; set; }

        [Required]
        [MaxLength(100)]
        public string PrzedmiotOpodatkowania { get; set; }

        [Column(TypeName = "money")]
        public decimal StawkaZaM2 { get; set; }

        public virtual ICollection<PozycjaDeklaracji> Pozycje {  get; set; }
    }
}
