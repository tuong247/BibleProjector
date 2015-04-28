using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using BibleProjector.Properties;
using Telerik.Windows;
using BibleProjector.Code;
using BibleProjector.Model;
using BibleProjector.UC;
using Application = System.Windows.Application;

namespace BibleProjector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Dictionary<string, int> _chapterCount = new Dictionary<string, int>();
        readonly XmlDocument _xmldoc = new XmlDocument();
        int currentBook { get; set; }
        int currentChapter { get; set; }
        ShowCauGoc showCauGoc = new ShowCauGoc();
        private  ColorWindow colorwindows ;

        public MainWindow()
        {
            InitializeComponent();
            BibleBooksComboBox.SelectedIndex = 0;
            cboDoan.SelectedIndex = 0;

        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            var screens = Screen.AllScreens;
            SetFormLocation( screens[Settings.Default.ProjectorScreen]);
        }

        private void btnHide_Click(object sender, RoutedEventArgs e)
        {
            if (showCauGoc != null)
                showCauGoc.Close();
        }

        private void SetFormLocation( Screen screen)
        {
            if (showCauGoc!=null)
                showCauGoc.Close();
            showCauGoc = new ShowCauGoc();
            if (screen == null) throw new ArgumentNullException("screen");
            var mainWindowPresentationSource = PresentationSource.FromVisual(this);
            if (mainWindowPresentationSource != null)
            {
                if (mainWindowPresentationSource.CompositionTarget != null)
                {
                    var m = mainWindowPresentationSource.CompositionTarget.TransformToDevice;
                    var dpiWidthFactor = m.M11;
                    var dpiHeightFactor = m.M22;
                    showCauGoc.Top = screen.WorkingArea.Top / dpiHeightFactor;
                    showCauGoc.Left = screen.WorkingArea.Left / dpiWidthFactor;
                    showCauGoc.Width = screen.WorkingArea.Width / dpiWidthFactor;
                    showCauGoc.Height = screen.WorkingArea.Height / dpiHeightFactor;
                }
            }
            showCauGoc.WindowState = WindowState.Normal;
            showCauGoc.Show();
            caugoc_SelectionChanged(null, null);
        }

        private void caugoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = caugoc.SelectedItem as Verse;
            if (item != null)
            {
                bool isShowOtherColor = CommonFunction.ConvertInt(item.Id)%2 == 0;
                showCauGoc.SetCauGoc(item.Value, isShowOtherColor);
              
            }
        }

        private void Videos_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = videos.SelectedItem as Video;
            if (item != null) showCauGoc.SetMedia(item.Location);
        }

        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            if (colorwindows == null)
            {
                colorwindows = new ColorWindow();
                colorwindows.Show();
                colorwindows.Closed += colorwindows_Closed;
            }
            else
            {

                colorwindows.Show();
            }
           
        }

        void colorwindows_Closed(object sender, EventArgs e)
        {
            colorwindows = null;
            var isChange = (sender as ColorWindow).IsChangeData;
            if (!isChange) return;
            var dataContext = (MainWindowViewModel)this.DataContext;
            var userPrefs = new UserPreferences();
            dataContext.SetBible(userPrefs.BibileLocation);
            

            //BibleBooksComboBox.SelectedIndex = BibleBooksComboBox.SelectedIndex;
            //cboDoan.SelectedIndex = cboDoan.SelectedIndex;
        }

        private void btnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

       

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

      
    }
}
