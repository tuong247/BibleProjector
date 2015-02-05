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
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using BibleProjector.Code;

namespace BibleProjector.UC
{
    /// <summary>
    /// Interaction logic for ColorWindow.xaml
    /// </summary>
    public partial class ColorWindow : Window
    {
        public ColorWindow()
        {
            InitializeComponent();
            color1.SelectedColor = App.Color1!=null? App.Color1: Colors.White;
            color2.SelectedColor = App.Color2 != null ? App.Color2 : Colors.Yellow;
            SelectedFont.SelectedValue = App.CurrentFont;
            SelectedFontSize.SelectedValue = App.CurrentSize;

            var screens = Screen.AllScreens;
            foreach (var screen in screens)
            {
                rcboMonitor.Items.Add(new RadComboBoxItem() {Content = screen.DeviceName});
            }
            rcboMonitor.SelectedIndex = screens.Count() - 1;

        }

        private void color2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void color1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            App.Color1 = color1.SelectedColor;
            App.Color2 = color2.SelectedColor;
            if (SelectedFont.SelectedItem != null)
                App.CurrentFont = SelectedFont.Text;
            if (SelectedFontSize.SelectedItem != null)
                App.CurrentSize = CommonFunction.ConvertInt(SelectedFontSize.Text);
            if (rcboMonitor.SelectedIndex > 0)
                App.ProjectorScreen = rcboMonitor.SelectedIndex;
            this.Close();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
