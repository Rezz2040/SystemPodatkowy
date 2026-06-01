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
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace SystemPodatkowy.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<Podatnik> _podatnicy;

        public ObservableCollection<Podatnik> Podatnicy
        {
            get => _podatnicy;
            set
            {
                _podatnicy = value;
                OnPropertyChanged();
            }
        }

        private Podatnik _wybranyPodatnik;
        public Podatnik WybranyPodatnik
        {
            get => _wybranyPodatnik;
            set
            {
                _wybranyPodatnik = value;
                OnPropertyChanged();
            }
        }

        public ICommand WczytajPodatnikowCommand { get; }
        public ICommand DodajPodatnikaCommand { get; }
        public ICommand DodajWplateCommand { get; }
        public ICommand DodajPowierzchnieCommand { get; }
        public ICommand OtworzStawkiCommand { get; }
        public ICommand GenerujDeklaracjeCommand { get; }


        public MainViewModel()
        {
            WczytajPodatnikowCommand = new RelayCommand(WczytajZPrzycisk);
            DodajPodatnikaCommand = new RelayCommand(OtworzOknoDodawania);
            DodajWplateCommand = new RelayCommand(OtworzOknoWplaty, CanOtworzOknoWplaty);
            DodajPowierzchnieCommand = new RelayCommand(OtworzOknoPowierzchni, CanOtworzOknoWplaty);
            OtworzStawkiCommand = new RelayCommand(OtworzOknoStawek);
            GenerujDeklaracjeCommand = new RelayCommand(OtworzOknoDeklaracji, CanOtworzOknoWplaty);
        }

        private void WczytajZPrzycisk(Object parametr)
        {
            ZaladujPodatnikow();
        }

        private void ZaladujPodatnikow()
        {
            using (var context = new SystemPodatkowyContext())
            {
                context.Database.EnsureCreated();

                var listaZBazy = context.Podatnicy
                    .Include(p => p.Wplaty)
                    .Include(p => p.Deklaracje)
                    .Include(p => p.Powierzchnie)
                    .ToList();

                Podatnicy = new ObservableCollection<Podatnik>(listaZBazy);
            }
        }

        private void OtworzOknoDodawania(object parametr)
        {
            var oknoDodawania = new DodajPodatnikaWindow();
            oknoDodawania.ShowDialog();

            ZaladujPodatnikow();
        }

        private bool CanOtworzOknoWplaty(object parametr)
        {
            return WybranyPodatnik != null;
        }

        private void OtworzOknoWplaty(object parametr)
        {
            var oknoWplaty = new DodajWplateWindow(WybranyPodatnik.PodatnikID);
            oknoWplaty.ShowDialog();

            ZaladujPodatnikow();
        }

        private void OtworzOknoPowierzchni(object parametr)
        {
            var oknoPow = new DodajPowierzchnieWindow(WybranyPodatnik.PodatnikID);
            oknoPow.ShowDialog();
            ZaladujPodatnikow();
        }

        private void OtworzOknoStawek(object parametr)
        {
            var oknoStawek = new StawkiWindow();
            oknoStawek.ShowDialog();
        }

        private void OtworzOknoDeklaracji(object parametr)
        {
            var oknoDeklaracji = new GenerujDeklaracjeWindow(WybranyPodatnik.PodatnikID);
            oknoDeklaracji.ShowDialog();
            ZaladujPodatnikow();
        }
    }
}
