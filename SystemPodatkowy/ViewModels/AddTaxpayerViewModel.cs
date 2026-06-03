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
    public class AddTaxpayerViewModel : BaseViewModel
    {
        public Taxpayer NewTaxpayer { get; set; }

        public ICommand SaveCommand { get; }

        public Action CloseAction {  get; set; }

        public AddTaxpayerViewModel()
        {
            NewTaxpayer = new Taxpayer
            {
                EntityType = "Osoba fizyczna"
            };

            SaveCommand = new RelayCommand(SaveTaxpayer, CanSave);
        }

        private void SaveTaxpayer(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NewTaxpayer.TaxIdNumber))
                NewTaxpayer.TaxIdNumber = null;

            if (string.IsNullOrWhiteSpace(NewTaxpayer.PersonalIdNumber))
                NewTaxpayer.PersonalIdNumber = null;

            if(string.IsNullOrWhiteSpace(NewTaxpayer.FirstName))
                NewTaxpayer.FirstName = null;

            using (var context = new TaxSystemContext())
            {
                context.Taxpayers.Add(NewTaxpayer);
                context.SaveChanges();
            }

            CloseAction?.Invoke();
        }

        private bool CanSave(object parameter)
        {
            return !string.IsNullOrWhiteSpace(NewTaxpayer.CompanyNameOrLastName) &&
                   !string.IsNullOrWhiteSpace(NewTaxpayer.City);
        }
    }
}
