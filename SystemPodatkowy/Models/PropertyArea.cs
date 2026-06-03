using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemPodatkowy.Models
{
    [Table("Powierzchnie")]
    public class PropertyArea
    {
        [Key]
        [Column("PowierzchniaID")]
        public int AreaID { get; set; }

        [Column("PodatnikID")]
        public int TaxpayerID { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("ObrebEwidencyjny")]
        public string CadastralDistrict { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("NumerDzialki")]
        public string PlotNumber { get; set; }

        [MaxLength(50)]
        [Column("NumerKsiegiWieczystej")]
        public string? LandRegisteredNumber {  get; set; }

        [MaxLength(100)]
        [Column("Adres")]
        public string? Address { get; set; }

        [Column("PowCalkowitaM2", TypeName = "decimal(10,2)")]
        public decimal TotalAreaSqm { get; set; }

        [ForeignKey(nameof(TaxpayerID))]
        public virtual Taxpayer Taxpayer { get; set; }

        public virtual ICollection<CoOwnership> CoOwnerships{ get; set; }
        public virtual ICollection<DeclarationItem> DeclarationItems { get; set; }
    }
}
