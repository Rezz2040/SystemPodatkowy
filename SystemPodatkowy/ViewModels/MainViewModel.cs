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
using SystemPodatkowy.Views;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using SystemPodatkowy.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace SystemPodatkowy.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public readonly ITaxpayerRepository _repository;
        private readonly IServiceProvider _serviceProvider;

        private ObservableCollection<Taxpayer> _taxpayers;

        public ObservableCollection<Taxpayer> Taxpayers
        {
            get => _taxpayers;
            set
            {
                _taxpayers = value;
                OnPropertyChanged();
            }
        }

        private Taxpayer _selectedTaxpayer;
        public Taxpayer SelectedTaxpayer
        {
            get => _selectedTaxpayer;
            set
            {
                _selectedTaxpayer = value;
                OnPropertyChanged();
            }
        }

        private SeriesCollection _paymentPieChartData;
        public SeriesCollection PaymentPieChartData
        {
            get => _paymentPieChartData;
            set
            {
                _paymentPieChartData = value;
                OnPropertyChanged();
            }
        }

        private bool _showDeletedTaxpayers;
        public bool ShowDeletedTaxpayers
        {
            get => _showDeletedTaxpayers;
            set
            {
                if(_showDeletedTaxpayers != value)
                {
                    _showDeletedTaxpayers = value;
                    OnPropertyChanged();
                    LoadTaxpayers();
                }
            }
        }

        public ICommand LoadTaxpayersCommand { get; }
        public ICommand AddTaxpayerCommand { get; }
        public ICommand AddPaymentCommand { get; }
        public ICommand AddPropertyAreaCommand { get; }
        public ICommand OpenTaxRatesCommand { get; }
        public ICommand GenerateDeclarationCommand { get; }
        public ICommand SaveInlineEditCommand { get; }
        public ICommand DeleteTaxpayerCommand { get; }


        
        public MainViewModel(ITaxpayerRepository repository, IServiceProvider serviceProvider)
        {
            _repository = repository;
            _serviceProvider = serviceProvider;

            LoadTaxpayersCommand = new RelayCommand(LoadFromButton);
            AddTaxpayerCommand = new RelayCommand(OpenAddWindow);
            AddPaymentCommand = new RelayCommand(OpenPaymentWindow, CanOpenActionWindow);
            AddPropertyAreaCommand = new RelayCommand(OpenPropertyAreaWindow, CanOpenActionWindow);
            OpenTaxRatesCommand = new RelayCommand(OpenTaxRatesWindow);
            GenerateDeclarationCommand = new RelayCommand(OpenDeclarationWindow, CanOpenActionWindow);
            SaveInlineEditCommand = new RelayCommand(SaveInlineEdit, CanOpenActionWindow);
            DeleteTaxpayerCommand = new RelayCommand(DeleteTaxpayer);
        }

        private void LoadFromButton(Object parameter)
        {
            LoadTaxpayers();
        }

        private void LoadTaxpayers()
        {
            var listFromDb = ShowDeletedTaxpayers
                ? _repository.GetAllDeleted()
                : _repository.GetAllActive();

            Taxpayers = new ObservableCollection<Taxpayer>(listFromDb);
            UpdateChart();
        }

        private void OpenAddWindow(object parameter)
        {
            var addWindow = _serviceProvider.GetRequiredService<AddTaxpayerWindow>();
            addWindow.ShowDialog();

            LoadTaxpayers();
        }

        private bool CanOpenActionWindow(object parameter)
        {
            return SelectedTaxpayer != null;
        }

        private void OpenPaymentWindow(object parameter)
        {
            var paymentRepo = _serviceProvider.GetRequiredService<IGenericRepository<Payment>>();

            var vm = new AddPaymentViewModel(SelectedTaxpayer.TaxpayerID, paymentRepo);
            var paymentWindow = new AddPaymentWindow(vm);

            paymentWindow.ShowDialog();
            LoadTaxpayers();
        }

        private void OpenPropertyAreaWindow(object parameter)
        {
            var propertyWindow = new AddPropertyAreaWindow(SelectedTaxpayer.TaxpayerID);
            propertyWindow.ShowDialog();
            LoadTaxpayers();
        }

        private void OpenTaxRatesWindow(object parameter)
        {
            var ratesWindow = _serviceProvider.GetRequiredService<TaxRatesWindow>();
            ratesWindow.ShowDialog();
        }

        private void OpenDeclarationWindow(object parameter)
        {
            var declarationWindow = new GenerateDeclarationWindow(SelectedTaxpayer.TaxpayerID);
            declarationWindow.ShowDialog();
            LoadTaxpayers();
        }
        private void SaveInlineEdit(object parameter)
        {
            _repository.Update(SelectedTaxpayer);

            MessageBox.Show("Zmiany zostały poprawnie zapisane w bazie danych.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadTaxpayers();
        }

        private void DeleteTaxpayer(object parameter)
        {
            if(parameter is Taxpayer taxpayerToToggle)
            {
                string actionName = taxpayerToToggle.IsDeleted ? "przywrócić" : "usunąć";

                var result = MessageBox.Show($"Czy na pewno chcesz {actionName} podatnika {taxpayerToToggle.CompanyNameOrLastName}?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                
                if(result == MessageBoxResult.Yes)
                {
                    if (taxpayerToToggle.IsDeleted)
                    {
                        _repository.Restore(taxpayerToToggle);
                    }
                    else
                    {
                        _repository.Delete(taxpayerToToggle);
                    }

                    LoadTaxpayers();
                }
            }
        }

        private void UpdateChart()
        {
            if (Taxpayers == null || !Taxpayers.Any()) return;

            var taxpayersWithPayments = Taxpayers
                .Select(t => new
                {
                    Name = t.CompanyNameOrLastName,
                    TotalPaid = t.Payments?.Sum(p => p.Amount) ?? 0
                })
                .Where(t => t.TotalPaid > 0)
                .OrderByDescending(t => t.TotalPaid)
                .ToList();

            var top5 = taxpayersWithPayments.Take(5).ToList();
            var others = taxpayersWithPayments.Skip(5).ToList();

            var series = new SeriesCollection();

            foreach( var item in top5)
            {
                series.Add(new PieSeries
                {
                    Title = item.Name,
                    Values = new ChartValues<decimal> { item.TotalPaid },
                    DataLabels = true
                });
            }

            if (others.Any())
            {
                series.Add(new PieSeries
                {
                    Title = "Pozostali",
                    Values = new ChartValues<decimal> { others.Sum(x => x.TotalPaid) },
                    DataLabels = true
                });
            }

            PaymentPieChartData = series;
        }

    }
}
