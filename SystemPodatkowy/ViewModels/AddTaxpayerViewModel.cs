using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SystemPodatkowy.Data;
using SystemPodatkowy.Models;
using SystemPodatkowy.MVVM;
using SystemPodatkowy.Repositories;

namespace SystemPodatkowy.ViewModels
{
    public class AddTaxpayerViewModel : BaseViewModel
    {
        private readonly ITaxpayerRepository _repository;

        public Taxpayer NewTaxpayer { get; set; }
        public ICommand SaveCommand { get; }
        public Action CloseAction {  get; set; }

        public AddTaxpayerViewModel(ITaxpayerRepository repository)
        {
            _repository = repository;
            NewTaxpayer = new Taxpayer { EntityType = "Osoba fizyczna" };
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

            _repository.Add(NewTaxpayer);

            CloseAction?.Invoke();
        }

        private bool CanSave(object parameter)
        {
            return NewTaxpayer != null && NewTaxpayer.IsValid;
        }
    }
}
