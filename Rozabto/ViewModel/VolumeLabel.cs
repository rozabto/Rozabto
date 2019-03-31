using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rozabto.ViewModel {
    /// <summary>
    /// Слагаме етикет на силата на звука.
    /// </summary>
    public static class VolumeLabel {
        private static int pos = int.MaxValue;

        /// <summary>
        /// Създаваме етикет и го добавяме към grid.
        /// </summary>
        public static void Show(Grid volumeGrid, double value) {
            var label = new Label {
                Height = 24,
                Width = 16 + (value < 10 ? 0 : value == 100 ? 13 : 7),
                Foreground = Brushes.LightSteelBlue,
                Background = Brushes.Black,
                BorderBrush = Brushes.LightSteelBlue,
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = value,
                Margin = new Thickness(0, 0, -(value / 1.35) - 15, -30),
                FontSize = 10
            };
            pos = volumeGrid.Children.Add(label);
        }

        /// <summary>
        /// Променяме позицията и числото в етиката.
        /// </summary>
        public static void Changed(Grid volumeGrid, double value) {
            if (volumeGrid.Children.Count <= pos || !(volumeGrid.Children[pos] is Label)) return;
            var label = volumeGrid.Children[pos] as Label;
            label.Width = 16 + (value < 10 ? 0 : value == 100 ? 13 : 7);
            label.Margin = new Thickness(0, 0, -(value / 1.35) - 15, -30);
            label.Content = value;
            volumeGrid.Children[pos] = label;
        }

        /// <summary>
        /// Премахваме етикета.
        /// </summary>
        public static void Hide(Grid volumeGrid) {
            volumeGrid.Children.RemoveAt(pos);
        }
    }
}
