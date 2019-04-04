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

namespace Rozabto.View
{
    public partial class Bands : UserControl
    {
        public string SelectedBandName { get; set; }
        public Bands()
        {
            InitializeComponent();
            DataContext = MainViewModel.MySongs;
        }

        public void SelectBand(object sender, SelectionChangedEventArgs e)
        {
            var listbox = sender as ListBox;
            // Взимаме grid който показва страницата от MainWindow.
            var grid = ((MainWindow)Application.Current.MainWindow).GridPrincipal;
            grid.Children.Clear();
            // Активираме ABP с избраната банда от листа.
            MainViewModel.ActivateABP(MainViewModel.Collection.Bands[listbox.SelectedIndex]);
            grid.Children.Add(new ABPContent());
        }

        public void SelectedBand(object sender, MouseEventArgs e)
        {
            SelectedBandName = ((sender as DockPanel).Children[1] as Label).Content.ToString();
        }

        public void AddToPlayList(object sender, RoutedEventArgs e)
        {
            var add = new AddToPlayList(MainViewModel.GetSongsFromBand(SelectedBandName))
            {
                Owner = (MainWindow)Application.Current.MainWindow
            };
            add.Show();
        }

        public void RemoveBand(object sender, RoutedEventArgs e) 
        {
            MainViewModel.RemoveBand(SelectedBandName);
        }
    }
}
