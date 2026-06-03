using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemPodatkowy.Models
{
    [Table("StawkiPodatku")]
    public class TaxRate
    {
        [Key]
        [Column("StawkaID")]
        public int TaxRateID { get; set; }

        [Column("RokPodatkowy")]
        public int TaxYear {  get; set; }

        [Required]
        [MaxLength(100)]
        [Column("PrzedmiotOpodatkowania")]
        public string SubjectOfTaxation { get; set; }

        [Column("StawkaZaM2", TypeName = "money")]
        public decimal RatePerSqm { get; set; }

        public virtual ICollection<DeclarationItem> DeclarationItem {  get; set; }
    }
}
