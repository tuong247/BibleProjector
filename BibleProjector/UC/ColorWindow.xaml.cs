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
using BibleProjector.Properties;
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
            var userPrefs = new UserPreferences();
            color1.SelectedColor = CommonFunction.ConvertoMediaColor(userPrefs.Color1);
            color2.SelectedColor = CommonFunction.ConvertoMediaColor(userPrefs.Color2);
            bgColor1.SelectedColor = CommonFunction.ConvertoMediaColor(userPrefs.BgColor1);
            bgColor2.SelectedColor = CommonFunction.ConvertoMediaColor(userPrefs.BgColor2);
            SelectedFont.Text = userPrefs.CurrentFont;
            SelectedFontSize.Text = userPrefs.CurrentSize.ToString();

            var screens = Screen.AllScreens;
            foreach (var screen in screens)
            {
                rcboMonitor.Items.Add(new RadComboBoxItem() {Content = screen.DeviceName});
            }
            rcboMonitor.SelectedIndex = screens.Count() - 1;
        }

       

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var userPrefs = new UserPreferences
            {
                Color1 = CommonFunction.ConvertoColor(color1.SelectedColor),
                Color2 = CommonFunction.ConvertoColor(color2.SelectedColor),
                BgColor1 = CommonFunction.ConvertoColor(bgColor1.SelectedColor),
                BgColor2 = CommonFunction.ConvertoColor(bgColor2.SelectedColor)
            };
            if (SelectedFont.SelectedItem != null)
                userPrefs.CurrentFont = SelectedFont.Text;
            if (SelectedFontSize.SelectedItem != null)
                userPrefs.CurrentSize = CommonFunction.ConvertInt(SelectedFontSize.Text);
            if (rcboMonitor.SelectedIndex > 0)
                userPrefs.ProjectorScreen = rcboMonitor.SelectedIndex;
            userPrefs.Save();
            this.Close();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
