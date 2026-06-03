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
    public class AddPropertyAreaViewModel : BaseViewModel
    {
        private int _taxpayerID;

        public PropertyArea NewPropertyArea { get; set; }

        public decimal PercentageShare { get; set; } = 100.00m;

        public ICommand SaveCommand { get; }
        public Action CloseAction { get; set; }

        public AddPropertyAreaViewModel(int taxpayerID)
        {
            _taxpayerID = taxpayerID;
            NewPropertyArea = new PropertyArea
            {
                TaxpayerID = taxpayerID
            };

            SaveCommand = new RelayCommand(SavePropertyArea, CanSave);
        }

        private void SavePropertyArea(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NewPropertyArea.LandRegisteredNumber))
                NewPropertyArea.LandRegisteredNumber = null;

            if (string.IsNullOrWhiteSpace(NewPropertyArea.Address))
                NewPropertyArea.Address = null;

            using (var context = new TaxSystemContext())
            {
                context.PropertyAreas.Add(NewPropertyArea);

                var coOwnership = new CoOwnership
                {
                    TaxpayerID = _taxpayerID,
                    PropertyArea = NewPropertyArea,
                    PercentageShare = PercentageShare
                };

                context.CoOwnerships.Add(coOwnership);

                context.SaveChanges();
            }

            CloseAction?.Invoke();
        }

        private bool CanSave(object parameter)
        {
            return !string.IsNullOrWhiteSpace(NewPropertyArea.CadastralDistrict) &&
                   !string.IsNullOrWhiteSpace(NewPropertyArea.PlotNumber) &&
                   NewPropertyArea.TotalAreaSqm > 0 &&
                   PercentageShare > 0 && PercentageShare <= 100;
        }
    }
}
