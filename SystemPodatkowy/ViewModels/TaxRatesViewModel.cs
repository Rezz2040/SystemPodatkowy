using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using SystemPodatkowy.Data;
using SystemPodatkowy.Models;
using SystemPodatkowy.MVVM;
using SystemPodatkowy.Repositories;

namespace SystemPodatkowy.ViewModels
{
    public class TaxRatesViewModel : BaseViewModel
    {
        private readonly IGenericRepository<TaxRate> _repository;

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

        public TaxRatesViewModel(IGenericRepository<TaxRate> repository)
        {
            _repository = repository;
            
            NewTaxRate = new TaxRate { TaxYear = 2026, SubjectOfTaxation = "Budynki mieszkalne" };
            SaveTaxRateCommand = new RelayCommand(SaveTaxRates, CanSave);

            LoadTaxRates();
        }

        private void LoadTaxRates()
        {
            var ratesFromDb = _repository.GetAll().OrderByDescending(S => S.TaxYear).ToList();
            TaxRatesList = new ObservableCollection<TaxRate>(ratesFromDb);
        }

        private void SaveTaxRates(object parameter)
        {
            bool alreadyExists = _repository.GetAll()
                .Any(r => r.TaxYear == NewTaxRate.TaxYear &&
                          r.SubjectOfTaxation.ToLower() == NewTaxRate.SubjectOfTaxation.ToLower());

            if(alreadyExists)
            {
                MessageBox.Show($"Staka dla {NewTaxRate.SubjectOfTaxation} na rok {NewTaxRate.TaxYear} już istnieje w bazie!",
                                "Błąd duplikatu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _repository.Add(NewTaxRate);

            LoadTaxRates();

            NewTaxRate = new TaxRate { 
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
