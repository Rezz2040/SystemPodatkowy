using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemPodatkowy.Models
{
    [Table("PozycjeDeklaracji")]
    public class DeclarationItem
    {
        [Key]
        [Column("PozycjaID")]
        public int ItemID { get; set; }

        [Column("DeklaracjaID")]
        public int DeclarationID { get; set; }
        [Column("PowierzchniaID")]
        public int AreaID { get; set; }
        [Column("StawkaID")]
        public int TaxRateID { get; set; }

        [Column("PowierzchniaDoOpodatkowaniaM2", TypeName = "decimal(10,2)")]
        public decimal TaxableAreaSqm {  get; set; }

        [Column("WartoscPozycji", TypeName = "money")]
        public decimal ItemValue {  get; set; }

        [ForeignKey(nameof(DeclarationID))]
        public virtual TaxDeclaration TaxDeclaration { get; set; }

        [ForeignKey(nameof(AreaID))]
        public virtual PropertyArea PropertyArea { get; set; }

        [ForeignKey(nameof(TaxRateID))]
        public virtual TaxRate TaxRate { get; set; }
    }
}
