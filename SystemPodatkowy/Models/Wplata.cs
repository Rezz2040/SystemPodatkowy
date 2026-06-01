using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemPodatkowy.Models
{
    public class Wplata
    {
        [Key]
        public int WplataID { get; set; }

        public int PodatnikID { get; set; }

        [Required]
        public DateTime DataWplywu { get; set; }

        [Column(TypeName = "money")]
        public decimal KwotaWplaty { get; set; }

        [Required]
        [MaxLength(50)]
        public string SposobWplaty { get; set; }

        [ForeignKey(nameof(PodatnikID))]
        public virtual Podatnik Podatnik { get; set; }
    }
}
