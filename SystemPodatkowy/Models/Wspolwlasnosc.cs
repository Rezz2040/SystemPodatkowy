using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemPodatkowy.Models
{
    public class Wspolwlasnosc
    {
        public int PodatnikID { get; set; }
        public int PowierzchniaID { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal UdziałProcentowy { get; set; }

        [ForeignKey(nameof(PodatnikID))]
        public virtual Podatnik Podatnik { get; set; }

        [ForeignKey(nameof(PowierzchniaID))]
        public virtual Powierzchnia Powierzchnia { get; set; }
    }
}
