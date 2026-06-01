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
    public partial class DodajPowierzchnieWindow : Window
    {
        public DodajPowierzchnieWindow(int podatnikId)
        {
            InitializeComponent();

            var vm = new DodajPowierzchnieViewModel(podatnikId);
            this.DataContext = vm;

            if(vm.ZamknijOkno == null)
            {
                vm.ZamknijOkno = new System.Action(this.Close);
            }
        }
    }
}
