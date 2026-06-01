using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemPodatkowy.Models
{
    public class PozycjaDeklaracji
    {
        [Key]
        public int PozycjaID { get; set; }

        public int DeklaracjaID { get; set; }
        public int PowierzchniaID { get; set; }
        public int StawkaID { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PowierzchniaDoOpodatkowaniaM2 {  get; set; }

        [Column(TypeName = "money")]
        public decimal WartoscPozycji {  get; set; }

        [ForeignKey(nameof(DeklaracjaID))]
        public virtual Deklaracja Deklaracja { get; set; }

        [ForeignKey(nameof(PowierzchniaID))]
        public virtual Powierzchnia Powierzchnia { get; set; }

        [ForeignKey(nameof(StawkaID))]
        public virtual StawkaPodatku StawkaPodaktu { get; set; }
    }
}
