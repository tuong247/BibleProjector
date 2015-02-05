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

namespace BibleProjector
{
    /// <summary>
    /// Interaction logic for ShowCauGoc.xaml
    /// </summary>
    public partial class ShowCauGoc : Window
    {
        public ShowCauGoc()
        {
            InitializeComponent();
            tblCauGoc.Width = double.NaN;
            tblCauGoc.Height = double.NaN;
        }

        public void SetCauGoc(string caugoc, bool isShowOtherColor)
        {
            tblCauGoc.Foreground = isShowOtherColor ? new SolidColorBrush(App.Color2) : new SolidColorBrush(App.Color1);
            tblCauGoc.FontSize = App.CurrentSize > 0 ? App.CurrentSize : tblCauGoc.FontSize;
            tblCauGoc.FontFamily = !string.IsNullOrEmpty(App.CurrentFont) ? new FontFamily(App.CurrentFont)  : tblCauGoc.FontFamily;
            tblCauGoc.Text = caugoc;
            tblCauGoc1.Foreground = isShowOtherColor ? new SolidColorBrush(App.Color2) : new SolidColorBrush(App.Color1);
            tblCauGoc1.FontSize = App.CurrentSize > 0 ? App.CurrentSize : tblCauGoc1.FontSize;
            tblCauGoc1.FontFamily = !string.IsNullOrEmpty(App.CurrentFont) ? new FontFamily(App.CurrentFont) : tblCauGoc1.FontFamily;
            tblCauGoc1.Text = caugoc;
        }

        public void SetMedia(string mediapath)
        {
            media.Source = new Uri(mediapath,UriKind.Absolute);
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            media.Position = new TimeSpan(0, 0, 1);
            media.Play();
        }
    }
}
