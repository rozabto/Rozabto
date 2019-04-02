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
    public partial class MyMusic : UserControl {
        public MyMusic() {
            InitializeComponent();
            GridPrincipal.Children.Add(new Bands());
        }

        public void Album(object sender, MouseButtonEventArgs e) {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new Album());
        }

        public void Song(object sender, MouseButtonEventArgs e) {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new Songs());
        }

        public void Band(object sender, MouseButtonEventArgs e) {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new Bands());
        }
    }
}
