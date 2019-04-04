using Rozabto.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Rozabto.View
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            ColorText.Text = MainViewModel.Theme ? "Светло" : "Тъмно";
            ButtonColor.IsChecked = MainViewModel.Theme;
        }

        public void ChangeColor(object sender, RoutedEventArgs e)
        {
            var dirs = Application.Current.Resources.MergedDictionaries;
            dirs.RemoveAt(dirs.Count - 1);
            if (MainViewModel.Theme)
            {
                ColorText.Text = "Тъмно";
                dirs.Add(new ResourceDictionary
                {
                    Source = new Uri("/Rozabto;component/View/Theme/DarkTheme.xaml", UriKind.Relative)
                });
            }
            else
            {
                ColorText.Text = "Светло";
                dirs.Add(new ResourceDictionary
                {
                    Source = new Uri("/Rozabto;component/View/Theme/LightTheme.xaml", UriKind.Relative)
                });
            }
            MainViewModel.Theme = !MainViewModel.Theme;
        }
    }
}
