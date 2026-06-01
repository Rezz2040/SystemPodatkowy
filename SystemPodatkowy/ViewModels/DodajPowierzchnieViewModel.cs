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
    public class DodajPowierzchnieViewModel : BaseViewModel
    {
        private int _podatnikId;

        public Powierzchnia NowaPowierzchnia { get; set; }

        public decimal UdzialProcentowy { get; set; } = 100.00m;

        public ICommand ZapiszCommand { get; }
        public Action ZamknijOkno { get; set; }

        public DodajPowierzchnieViewModel(int podatnikId)
        {
            _podatnikId = podatnikId;
            NowaPowierzchnia = new Powierzchnia
            {
                PodatnikID = podatnikId
            };

            ZapiszCommand = new RelayCommand(ZapiszPowierzchnie, CzyMoznaZapisac);
        }

        private void ZapiszPowierzchnie(object parametr)
        {
            if (string.IsNullOrWhiteSpace(NowaPowierzchnia.NumerKsiegiWieczystej))
                NowaPowierzchnia.NumerKsiegiWieczystej = null;

            if (string.IsNullOrWhiteSpace(NowaPowierzchnia.Adres))
                NowaPowierzchnia.Adres = null;

            using (var context = new SystemPodatkowyContext())
            {
                context.Powierzchnie.Add(NowaPowierzchnia);

                var wspolwlasnosc = new Wspolwlasnosc
                {
                    PodatnikID = _podatnikId,
                    Powierzchnia = NowaPowierzchnia,
                    UdziałProcentowy = UdzialProcentowy
                };

                context.Wspolwlasnosci.Add(wspolwlasnosc);

                context.SaveChanges();
            }

            ZamknijOkno?.Invoke();
        }

        private bool CzyMoznaZapisac(object parametr)
        {
            return !string.IsNullOrWhiteSpace(NowaPowierzchnia.ObrebEwidencyjny) &&
                   !string.IsNullOrWhiteSpace(NowaPowierzchnia.NumerDzialki) &&
                   NowaPowierzchnia.PowCalkowitaM2 > 0 &&
                   UdzialProcentowy > 0 && UdzialProcentowy <= 100;
        }
    }
}
