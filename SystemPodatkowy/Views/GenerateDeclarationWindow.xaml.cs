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

namespace SystemPodatkowy.Views
{

    public partial class GenerateDeclarationWindow : Window
    {
        public GenerateDeclarationWindow(int taxpayerID)
        {
            InitializeComponent();

            var vm = new GenerateDeclarationViewModel(taxpayerID);
            this.DataContext = vm;

            if(vm.CloseAction == null)
            {
                vm.CloseAction = new Action(this.Close);
            }
        }
    }
}
