using Rozabto.ViewModel;
using Rozabto.ViewModel.Notify;
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
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl {
        public Settings() {
            InitializeComponent();
            ColorText.Text = MainViewModel.Theme ? "Светло" : "Тъмно";
        }

        private void ChangeColor(object sender, RoutedEventArgs e) {
            var dirs = Application.Current.Resources.MergedDictionaries;
            dirs.RemoveAt(dirs.Count - 1);
            if (MainViewModel.Theme) {
                ColorText.Text = "Тъмно";
                dirs.Add(new ResourceDictionary {
                    Source = new Uri("/Rozabto;component/View/Theme/DarkTheme.xaml", UriKind.Relative)
                });
            }
            else {
                ColorText.Text = "Светло";
                dirs.Add(new ResourceDictionary {
                    Source = new Uri("/Rozabto;component/View/Theme/LightTheme.xaml", UriKind.Relative)
                });
            }
            MainViewModel.Theme = !MainViewModel.Theme;
        }
    }
}
