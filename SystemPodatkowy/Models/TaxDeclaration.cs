using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemPodatkowy.Models
{
    [Table("Deklaracje")]
    public class TaxDeclaration
    {
        [Key]
        [Column("DeklaracjaID")]
        public int DeclarationID { get; set; }

        [Column("PodatnikID")]
        public int TaxpayerID { get; set; }

        [Column("Rok")]
        public int Year {  get; set; }

        [Required]
        [Column("DataZlozenia")]
        public DateTime SubmissionDate { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("TypDokumentu")]
        public string DocumentType { get; set; }

        [Column("LacznaKwotaPodatku", TypeName = "money")]
        public decimal TotalTaxAmount { get; set; }

        [ForeignKey(nameof(TaxpayerID))]
        public virtual Taxpayer Taxpayer { get; set; }

        public virtual ICollection<DeclarationItem> DeclarationItems {  get; set; }
    }
}
