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
    public class GenerateDeclarationViewModel : BaseViewModel
    {
        private int _taxpayerID;

        private int _year = 2026;
        public int Year 
        { 
            get => _year;
            set { _year = value; OnPropertyChanged(); } 
        }

        public ICommand GenerateCommand { get; }
        public Action CloseAction { get; set; }

        public GenerateDeclarationViewModel(int taxpayerID)
        {
            _taxpayerID = taxpayerID;
            GenerateCommand = new RelayCommand(GenerateDeclaration, CanGenerate);
        }

        private void GenerateDeclaration(object parameter)
        {
            using (var context = new TaxSystemContext())
            {
                var taxRate = context.TaxRates.FirstOrDefault(s => s.TaxYear == Year);

                if (taxRate == null)
                {
                    MessageBox.Show($"Brak zdefiniowanej stawki podatkowej na rok {Year}. Dodaj ją najpierw w zakładce stawek.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var taxpayerCoOwnership = context.CoOwnerships
                    .Include(w => w.PropertyArea)
                    .Where(w => w.TaxpayerID == _taxpayerID)
                    .ToList();

                if (!taxpayerCoOwnership.Any())
                {
                    MessageBox.Show("Ten podatnik nie posiada żadnych zarejestrowanych powierzchni.", "Brak danych", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var newDeclaration = new TaxDeclaration
                {
                    TaxpayerID = _taxpayerID,
                    Year = Year,
                    SubmissionDate = DateTime.Now,
                    DocumentType = "IN-1",
                    TotalTaxAmount = 0
                };

                context.Declarations.Add(newDeclaration);
                context.SaveChanges();

                decimal totalAmount = 0;

                foreach (var coOwnership in taxpayerCoOwnership)
                {
                    decimal taxableArea = coOwnership.PropertyArea.TotalAreaSqm * (coOwnership.PercentageShare/ 100m);
                    decimal taxValue = taxableArea * taxRate.RatePerSqm;

                    var declarationItem = new DeclarationItem
                    {
                        DeclarationID = newDeclaration.DeclarationID,
                        AreaID = coOwnership.AreaID,
                        TaxRateID = taxRate.TaxRateID,
                        TaxableAreaSqm = taxableArea,
                        ItemValue = taxValue
                    };

                    context.DeclarationItems.Add(declarationItem);
                    totalAmount += taxValue;
                }

                newDeclaration.TotalTaxAmount = totalAmount;
                context.SaveChanges();

                MessageBox.Show($"Wygenerowano deklarację. Łączna kwota podatku: {totalAmount:C}", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            CloseAction?.Invoke();
        }

        private bool CanGenerate(object parameter)
        {
            return Year >= 2000 && Year <= 2100;
        }
    }
}
