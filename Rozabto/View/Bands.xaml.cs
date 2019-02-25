using Rozabto.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rozabto.View {
    public partial class Bands : UserControl {
        public Bands() {
            InitializeComponent();
            DataContext = MainViewModel.MySongs;
        }

        private void SelectBand(object sender, SelectionChangedEventArgs e) {
            var listbox = sender as ListBox;
            var grid = ((MainWindow)Application.Current.MainWindow).GridPrincipal;
            grid.Children.Clear();
            MainViewModel.ActivateABP(MainViewModel.Collection.Bands[listbox.SelectedIndex]);
            grid.Children.Add(new ABPContent());
        }
    }
}
