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
    public class DodajPodatnikaViewModel : BaseViewModel
    {
        public Podatnik NowyPodatnik { get; set; }

        public ICommand ZapiszCommand { get; }

        public Action ZamknijOkno {  get; set; }

        public DodajPodatnikaViewModel()
        {
            NowyPodatnik = new Podatnik
            {
                TypPodmiotu = "Osoba fizyczna"
            };

            ZapiszCommand = new RelayCommand(ZapiszPodatnika, CzyMoznaZapisac);
        }

        private void ZapiszPodatnika(object parametr)
        {
            if (string.IsNullOrWhiteSpace(NowyPodatnik.NIP))
                NowyPodatnik.NIP = null;

            if (string.IsNullOrWhiteSpace(NowyPodatnik.PESEL))
                NowyPodatnik.PESEL = null;

            if(string.IsNullOrWhiteSpace(NowyPodatnik.Imie))
                NowyPodatnik.Imie = null;

            using (var context = new SystemPodatkowyContext())
            {
                context.Podatnicy.Add(NowyPodatnik);
                context.SaveChanges();
            }

            ZamknijOkno?.Invoke();
        }

        private bool CzyMoznaZapisac(object parametr)
        {
            return !string.IsNullOrWhiteSpace(NowyPodatnik.NazwaWlasna_Nazwisko) &&
                   !string.IsNullOrWhiteSpace(NowyPodatnik.Miejscowosc);
        }
    }
}
