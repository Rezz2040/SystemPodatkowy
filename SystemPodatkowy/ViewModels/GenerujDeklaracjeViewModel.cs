using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using SystemPodatkowy.Data;
using SystemPodatkowy.Models;
using SystemPodatkowy.MVVM;

namespace SystemPodatkowy.ViewModels
{
    public class GenerujDeklaracjeViewModel : BaseViewModel
    {
        private int _podatnikId;

        private int _rok = 2026;
        public int Rok 
        { 
            get => _rok;
            set { _rok = value; OnPropertyChanged(); } 
        }

        public ICommand GenerujCommand { get; }
        public Action ZamknijOkno { get; set; }

        public GenerujDeklaracjeViewModel(int podatnikId)
        {
            _podatnikId = podatnikId;
            GenerujCommand = new RelayCommand(GenerujDeklaracje, CanGeneruj);
        }

        private void GenerujDeklaracje(object parametr)
        {
            using (var context = new SystemPodatkowyContext())
            {
                var stawka = context.StawkiPodatku.FirstOrDefault(s => s.RokPodatkowy == Rok);

                if (stawka == null)
                {
                    MessageBox.Show($"Brak zdefiniowanej stawki podatkowej na rok {Rok}. Dodaj ją najpierw w zakładce stawek.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var wspolwlasnosciPodatnika = context.Wspolwlasnosci
                    .Include(w => w.Powierzchnia)
                    .Where(w => w.PodatnikID == _podatnikId)
                    .ToList();

                if (!wspolwlasnosciPodatnika.Any())
                {
                    MessageBox.Show("Ten podatnik nie posiada żadnych zarejestrowanych powierzchni.", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var nowaDeklaracja = new Deklaracja
                {
                    PodatnikID = _podatnikId,
                    Rok = Rok,
                    DataZlozenia = DateTime.Now,
                    TypDokumentu = "IN-1",
                    LacznaKwotaPodatku = 0
                };

                context.Deklaracje.Add(nowaDeklaracja);
                context.SaveChanges();

                decimal lacznaKwota = 0;

                foreach (var wspolwlasnosc in wspolwlasnosciPodatnika)
                {
                    decimal powDoOpodatkowania = wspolwlasnosc.Powierzchnia.PowCalkowitaM2 * (wspolwlasnosc.UdziałProcentowy / 100m);
                    decimal wartoscPodatku = powDoOpodatkowania * stawka.StawkaZaM2;

                    var pozycja = new PozycjaDeklaracji
                    {
                        DeklaracjaID = nowaDeklaracja.DeklaracjaID,
                        PowierzchniaID = wspolwlasnosc.PowierzchniaID,
                        StawkaID = stawka.StawkaID,
                        PowierzchniaDoOpodatkowaniaM2 = powDoOpodatkowania,
                        WartoscPozycji = wartoscPodatku
                    };

                    context.PozycjeDeklaracji.Add(pozycja);
                    lacznaKwota += wartoscPodatku;
                }

                nowaDeklaracja.LacznaKwotaPodatku = lacznaKwota;
                context.SaveChanges();

                MessageBox.Show($"Wygenerowano deklarację. Łączna kwota podatku: {lacznaKwota:C}", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            ZamknijOkno?.Invoke();
        }

        private bool CanGeneruj(object parametr)
        {
            return Rok >= 2000 && Rok <= 2100;
        }
    }
}
