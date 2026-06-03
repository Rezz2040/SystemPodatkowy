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
    public class TaxRatesViewModel : BaseViewModel
    {
        private ObservableCollection<TaxRate> _taxRatesList;
        public ObservableCollection<TaxRate> TaxRatesList
        {
            get => _taxRatesList;
            set { _taxRatesList = value; OnPropertyChanged(); }
        }

        private TaxRate _newTaxRate;
        public TaxRate NewTaxRate
        {
            get => _newTaxRate;
            set { _newTaxRate = value; OnPropertyChanged(); }
        }

        public ICommand SaveTaxRateCommand { get; }

        public TaxRatesViewModel()
        {
            NewTaxRate = new TaxRate
            {
                TaxYear = 2026,
                SubjectOfTaxation = "Budynki mieszkalne"
            };

            SaveTaxRateCommand = new RelayCommand(SaveTaxRates, CanSave);
            LoadTaxRates();
        }

        private void LoadTaxRates()
        {
            using (var context = new TaxSystemContext())
            {
                var ratesFromDb = context.TaxRates
                    .OrderByDescending(s => s.TaxYear)
                    .ToList();

                TaxRatesList = new ObservableCollection<TaxRate>(ratesFromDb);
            }
        }

        private void SaveTaxRates(object parameter)
        {
            using (var context = new TaxSystemContext())
            {
                context.TaxRates.Add(NewTaxRate);
                context.SaveChanges();
            }

            LoadTaxRates();

            NewTaxRate = new TaxRate
            {
                TaxYear = 2026,
                SubjectOfTaxation = "Budynki mieszkalne"
            };
        }

        private bool CanSave(object parameter)
        {
            return NewTaxRate.TaxYear >= 2000 &&
                   !string.IsNullOrWhiteSpace(NewTaxRate.SubjectOfTaxation) &&
                   NewTaxRate.RatePerSqm > 0;
        }
    }
}
