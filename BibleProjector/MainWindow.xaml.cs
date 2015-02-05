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
using Telerik.Windows;
using BibleProjector.Code;
using BibleProjector.Model;
using BibleProjector.UC;

namespace BibleProjector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Dictionary<string, int> _chapterCount = new Dictionary<string, int>();
        readonly XmlDocument _xmldoc = new XmlDocument();
        ShowCauGoc showCauGoc = new ShowCauGoc();
        public MainWindow()
        {
            InitializeComponent();
            BibleBooksComboBox.SelectedIndex = 0;
            cboDoan.SelectedIndex = 0;

        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            var screens = Screen.AllScreens;
            SetFormLocation( screens[App.ProjectorScreen]);
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
            var colorwindows = new ColorWindow();
            colorwindows.Show();
        }

        private void btnLoadKinhThanh_Click(object sender, RoutedEventArgs e)
        {
            var frm = new ChonKinhThanh();
            frm.Show();
            frm.Closed += frm_Closed;
        }

        void frm_Closed(object sender, EventArgs e)
        {
            var isChange = (sender as ChonKinhThanh).IsChangeData;
            if (!isChange) return;
            this.DataContext = new MainWindowViewModel();
            BibleBooksComboBox.SelectedIndex = 0;
            cboDoan.SelectedIndex = 0;
        }

        private void LoadBackGround_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string result = dialog.SelectedPath;
                App.MotionLocation = result;
                this.DataContext = new MainWindowViewModel();
                BibleBooksComboBox.SelectedIndex = 0;
                cboDoan.SelectedIndex = 0;
            }
        }

        private void btnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
