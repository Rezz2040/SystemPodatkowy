using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SystemPodatkowy.Data;
using SystemPodatkowy.Models;
using SystemPodatkowy.MVVM;

namespace SystemPodatkowy.ViewModels
{
    public class DodajWplateViewModel : BaseViewModel
    {
        public Wplata NowaWplata { get; set; }
        public ICommand ZapiszCommand { get; }
        public Action ZamknijOkno { get; set; }

        public DodajWplateViewModel(int podatnikId)
        {
            NowaWplata = new Wplata
            {
                PodatnikID = podatnikId,
                DataWplywu = DateTime.Now,
                SposobWplaty = "Przelew bankowy"
            };

            ZapiszCommand = new RelayCommand(ZapiszWplate, CzyMoznaZapisac);
        }

        private void ZapiszWplate(object parametr)
        {
            using (var context = new SystemPodatkowyContext())
            {
                context.Wplaty.Add(NowaWplata);
                context.SaveChanges();
            }

            ZamknijOkno?.Invoke();
        }

        private bool CzyMoznaZapisac(object parametr)
        {
            return NowaWplata.KwotaWplaty > 0;
        }
    }
}
