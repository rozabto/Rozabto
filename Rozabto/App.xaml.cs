using Rozabto.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Rozabto {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private void App_OnStartup(object sender, StartupEventArgs e) {
            (Current.Resources["BackgroundColour"] as SolidColorBrush).Color = MainViewModel.AppXaml.BackgroundColor;
            (Current.Resources["ForegroudColour"] as SolidColorBrush).Color = MainViewModel.AppXaml.ForegroundColor;
            var mainWindow = new MainWindow();
            mainWindow.ShowDialog();
        }

        public void ChangeTheme(bool toWhite) {
            var background = Resources.MergedDictionaries.FirstOrDefault(f => f.Keys.Cast<string>().FirstOrDefault(t => t == "ForegroundColour") != null);
            var foreground = Resources.MergedDictionaries.FirstOrDefault(f => f.Keys.Cast<string>().FirstOrDefault(t => t == "BackgroundColour") != null);
            foreground.Clear();
            background.Clear();
            if (toWhite) {
                foreground.Add("ForegroundColour", new SolidColorBrush(Color.FromRgb(255, 255, 255)));
                background.Add("BackgroundColour", new SolidColorBrush(Color.FromRgb(0, 0, 0)));
            }
            else {
                background.Add("BackgroundColour", new SolidColorBrush(Color.FromRgb(255, 255, 255)));
                foreground.Add("ForegroudColour", new SolidColorBrush(Color.FromRgb(0, 0, 0)));
            }
        }
    }
}
