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
    public class AddPaymentViewModel : BaseViewModel
    {
        public Payment NewPayment { get; set; }
        public ICommand SaveCommand { get; }
        public Action CloseAction { get; set; }

        public AddPaymentViewModel(int taxpayerID)
        {
            NewPayment = new Payment
            {
                TaxpayerID = taxpayerID,
                PaymentDate = DateTime.Now,
                PaymentMethod = "Przelew bankowy"
            };

            SaveCommand = new RelayCommand(SavePayment, CanSave);
        }

        private void SavePayment(object parameter)
        {
            using (var context = new TaxSystemContext())
            {
                context.Payments.Add(NewPayment);
                context.SaveChanges();
            }

            CloseAction?.Invoke();
        }

        private bool CanSave(object parameter)
        {
            return NewPayment.Amount > 0;
        }
    }
}
