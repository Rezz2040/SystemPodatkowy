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
using SystemPodatkowy.ViewModels;

namespace SystemPodatkowy
{
    public partial class TaxRatesWindow : Window
    {
        public TaxRatesWindow()
        {
            InitializeComponent();
            this.DataContext = new TaxRatesViewModel();
        }

        private void DecimalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                string fixedText = textBox.Text.Replace(".", separator).Replace(",", separator);

                if(textBox.Text != fixedText)
                {
                    int caretIndex = textBox.CaretIndex;
                    textBox.Text = fixedText;
                    textBox.CaretIndex = caretIndex;
                }
            }
        }
    }
}
