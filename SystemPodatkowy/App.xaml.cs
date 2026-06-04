using System.Configuration;
using System.Data;
using System.Windows;
using System.Globalization;
using System.Windows.Markup;
using Microsoft.Extensions.DependencyInjection;
using SystemPodatkowy.Models;
using SystemPodatkowy.Repositories;
using SystemPodatkowy.Data;
using SystemPodatkowy.ViewModels;
using SystemPodatkowy.Views;

namespace SystemPodatkowy
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<TaxSystemContext>();

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ITaxpayerRepository, TaxpayerRepository>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<AddTaxpayerViewModel>();
            services.AddTransient<TaxRatesViewModel>();

            services.AddTransient<MainWindow>();
            services.AddTransient<AddTaxpayerWindow>();
            services.AddTransient<TaxRatesWindow>();
        }
    }

}
