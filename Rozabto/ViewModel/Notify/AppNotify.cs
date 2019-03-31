using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Rozabto.ViewModel.Notify {
    public class AppNotify : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public Color BackgroundColor { get; private set; }
        public Color ForegroundColor { get; private set; }

        private bool toWhite;

        public AppNotify() {
            BackgroundColor = Color.FromRgb(0, 0, 0);
            ForegroundColor = Color.FromRgb(255, 255, 255);
        }

        public void ChangeColor() {
            ((App)Application.Current).ChangeTheme(toWhite);
            toWhite = !toWhite;
        }

        public void OnPropertyChanged(string property) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
