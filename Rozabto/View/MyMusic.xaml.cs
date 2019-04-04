using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rozabto.View
{
    public partial class MyMusic : UserControl
    {
        public MyMusic()
        {
            InitializeComponent();
            GridPrincipal.Children.Add(new Bands());
        }

        public void Album(object sender, MouseButtonEventArgs e)
        {
            SelectedBlock.Margin = new Thickness(-200, 0, 0, 0);
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new Album());
        }

        public void Song(object sender, MouseButtonEventArgs e)
        {
            SelectedBlock.Margin = new Thickness(0, 0, 0, 0);
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new Songs());
        }

        public void Band(object sender, MouseButtonEventArgs e)
        {
            SelectedBlock.Margin = new Thickness(200, 0, 0, 0);
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new Bands());
        }
    }
}
