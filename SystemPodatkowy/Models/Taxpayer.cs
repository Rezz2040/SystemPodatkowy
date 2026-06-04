using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SystemPodatkowy.MVVM;

namespace SystemPodatkowy.Models
{
    [Table("Podatnicy")]
    public class Taxpayer : BaseViewModel, IDataErrorInfo
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

        [NotMapped]
        public string Error => null;

        [NotMapped]
        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case nameof(CompanyNameOrLastName):
                        if (string.IsNullOrWhiteSpace(CompanyNameOrLastName))
                            result = "Nazwa/Nazwisko jest wymagane.";
                        else if (CompanyNameOrLastName.Length < 2)
                            result = "Nazwa musi mieć co najmniej 2 znaki.";
                        break;

                    case nameof(City):
                        if (string.IsNullOrWhiteSpace(City))
                            result = "Miejscowość jest wymagana.";
                        break;

                    case nameof(PersonalIdNumber):
                        if (!string.IsNullOrWhiteSpace(PersonalIdNumber))
                        {
                            if (PersonalIdNumber.Length != 11)
                                result = "PESEL musi składać się z 11 znaków.";
                            else if (!long.TryParse(PersonalIdNumber, out _))
                                result = "PESEL może zawierać tylko cyfry.";
                        }
                        break;

                    case nameof(TaxIdNumber):
                        if (!string.IsNullOrWhiteSpace(TaxIdNumber) && TaxIdNumber.Length != 10)
                            result = "NIP musi składać się dokładnie z 10 znaków.";
                        break;

                    case nameof(PostalCode):
                        ;
                        if (string.IsNullOrWhiteSpace(PostalCode))
                            result = "Kod pocztowy jest wymagany.";
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(PostalCode, @"^\d{2}-\d{3}$"))
                            result = "Kod pocztowy musi być w formacie XX-XXX (np. 30-001).";
                        break;
                }

                return result;
            }
        }

        [NotMapped]
        public bool IsValid
        {
            get
            {
                return string.IsNullOrEmpty(this[nameof(CompanyNameOrLastName)]) &&
                       string.IsNullOrEmpty(this[nameof(City)]) &&
                       string.IsNullOrEmpty(this[nameof(PersonalIdNumber)]) &&
                       string.IsNullOrEmpty(this[nameof(TaxIdNumber)]) &&
                       string.IsNullOrEmpty(this[nameof(PostalCode)]);
            }
        }
    }
}
