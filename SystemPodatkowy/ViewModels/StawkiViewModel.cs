using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SystemPodatkowy.Data;
using SystemPodatkowy.Models;
using SystemPodatkowy.MVVM;

namespace SystemPodatkowy.ViewModels
{
    public class StawkiViewModel : BaseViewModel
    {
        private ObservableCollection<StawkaPodatku> _listaStawek;
        public ObservableCollection<StawkaPodatku> ListaStawek
        {
            get => _listaStawek;
            set { _listaStawek = value; OnPropertyChanged(); }
        }

        private StawkaPodatku _nowaStawka;
        public StawkaPodatku NowaStawka
        {
            get => _nowaStawka;
            set { _nowaStawka = value; OnPropertyChanged(); }
        }

        public ICommand ZapiszStawkeCommand { get; }

        public StawkiViewModel()
        {
            NowaStawka = new StawkaPodatku
            {
                RokPodatkowy = 2026,
                PrzedmiotOpodatkowania = "Budynki mieszkalne"
            };

            ZapiszStawkeCommand = new RelayCommand(ZapiszStawke, CzyMoznaZapisac);
            ZaladujStawki();
        }

        private void ZaladujStawki()
        {
            using (var context = new SystemPodatkowyContext())
            {
                var stawkiZBazy = context.StawkiPodatku
                    .OrderByDescending(s => s.RokPodatkowy)
                    .ToList();

                ListaStawek = new ObservableCollection<StawkaPodatku>(stawkiZBazy);
            }
        }

        private void ZapiszStawke(object parametr)
        {
            using (var context = new SystemPodatkowyContext())
            {
                context.StawkiPodatku.Add(NowaStawka);
                context.SaveChanges();
            }

            ZaladujStawki();

            NowaStawka = new StawkaPodatku
            {
                RokPodatkowy = 2026,
                PrzedmiotOpodatkowania = "Budynki mieszkalne"
            };
        }

        private bool CzyMoznaZapisac(object parametr)
        {
            return NowaStawka.RokPodatkowy >= 2000 &&
                   !string.IsNullOrWhiteSpace(NowaStawka.PrzedmiotOpodatkowania) &&
                   NowaStawka.StawkaZaM2 > 0;
        }
    }
}
