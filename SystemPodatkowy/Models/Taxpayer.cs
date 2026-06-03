using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemPodatkowy.Models
{
    [Table("Podatnicy")]
    public class Taxpayer
    {
        [Key]
        [Column("PodatnikID")]
        public int TaxpayerID { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("TypPodmiotu")]
        public string EntityType { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("NazwaWlasna_Nazwisko")]
        public string CompanyNameOrLastName { get; set; }

        [MaxLength(50)]
        [Column("Imie")]
        public string? FirstName {  get; set; }

        [MaxLength(10)]
        [Column("NIP", TypeName ="char(10)")]
        public string? TaxIdNumber {  get; set; }

        [MaxLength(11)]
        [Column("PESEL", TypeName = "char(11)")]
        public string? PersonalIdNumber { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("Miejscowosc")]
        public string City { get; set; }

        [Required]
        [MaxLength(6)]
        [Column("KodPocztowy", TypeName = "char(6)")]
        public string PostalCode { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("Ulica")]
        public string Street { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("NumerDomuLokal")]
        public string HouseAndFlatNumber {  get; set; }

        [Required]
        [Column("CzyUsuniety")]
        public bool IsDeleted { get ; set; }


        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<TaxDeclaration> TaxDeclarations { get; set; }
        public virtual ICollection<PropertyArea> PropertyAreas { get; set; }
        public virtual ICollection<CoOwnership> CoOwnerships { get; set; }
    }
}
