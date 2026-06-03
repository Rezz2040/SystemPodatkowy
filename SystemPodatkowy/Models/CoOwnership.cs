using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemPodatkowy.Models
{
    [Table("Wspolwlasnosc")]
    public class CoOwnership
    {
        [Column("PodatnikID")]
        public int TaxpayerID { get; set; }
        [Column("PowierzchniaID")]
        public int AreaID { get; set; }

        [Column("UdzialProcentowy", TypeName = "decimal(10,2)")]
        public decimal PercentageShare { get; set; }

        [ForeignKey(nameof(TaxpayerID))]
        public virtual Taxpayer Taxpayer { get; set; }

        [ForeignKey(nameof(AreaID))]
        public virtual PropertyArea PropertyArea { get; set; }
    }
}
