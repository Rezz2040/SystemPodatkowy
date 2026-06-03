using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemPodatkowy.Models
{
    [Table("Wplaty")]
    public class Payment
    {
        [Key]
        [Column("WplataID")]
        public int PaymentID { get; set; }

        [Column("PodatnikID")]
        public int TaxpayerID { get; set; }

        [Required]
        [Column("DataWplywu")]
        public DateTime PaymentDate { get; set; }

        [Column("KwotaWplaty", TypeName = "money")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("SposobWplaty")]
        public string PaymentMethod { get; set; }

        [ForeignKey(nameof(TaxpayerID))]
        public virtual Taxpayer Taxpayer { get; set; }
    }
}
