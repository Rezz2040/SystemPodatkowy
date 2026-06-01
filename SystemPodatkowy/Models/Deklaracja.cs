using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemPodatkowy.Models
{
    public class Deklaracja
    {
        [Key]
        public int DeklaracjaID { get; set; }

        public int PodatnikID { get; set; }

        public int Rok {  get; set; }

        [Required]
        public DateTime DataZlozenia { get; set; }

        [Required]
        [MaxLength(20)]
        public string TypDokumentu { get; set; }

        [Column(TypeName = "money")]
        public decimal LacznaKwotaPodatku { get; set; }

        [ForeignKey(nameof(PodatnikID))]
        public virtual Podatnik Podatnik { get; set; }

        public virtual ICollection<PozycjaDeklaracji> Pozycje {  get; set; }
    }
}
