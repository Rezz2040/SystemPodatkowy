using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using SystemPodatkowy.ViewModels;

namespace SystemPodatkowy.Views
{
    public partial class AddTaxpayerWindow : Window
    {
        public AddTaxpayerWindow(AddTaxpayerViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;

            if(vm.CloseAction == null)
            {
                vm.CloseAction = new System.Action(this.Close);
            }
        }

        private void NumberValidationTextBox(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
