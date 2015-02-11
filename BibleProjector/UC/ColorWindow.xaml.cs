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
using setting = BibleProjector.Settings;

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
            color1.SelectedColor = CommonFunction.ConvertoMediaColor(setting.Default.Color1);
            color2.SelectedColor = CommonFunction.ConvertoMediaColor(setting.Default.Color2);
            bgColor1.SelectedColor = CommonFunction.ConvertoMediaColor(setting.Default.BgColor1);
            bgColor2.SelectedColor = CommonFunction.ConvertoMediaColor(setting.Default.BgColor2);
            SelectedFont.SelectedValue =setting.Default.CurrentFont;
            SelectedFontSize.SelectedValue = setting.Default.CurrentSize;

            var screens = Screen.AllScreens;
            foreach (var screen in screens)
            {
                rcboMonitor.Items.Add(new RadComboBoxItem() {Content = screen.DeviceName});
            }
            rcboMonitor.SelectedIndex = screens.Count() - 1;
            setting.Default.CurrentSize = rcboMonitor.SelectedIndex;
            setting.Default.Save();
        }

       

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            setting.Default.Color1 = CommonFunction.ConvertoColor(color1.SelectedColor);
            setting.Default.Color2 = CommonFunction.ConvertoColor(color2.SelectedColor);
            setting.Default.BgColor1 = CommonFunction.ConvertoColor(bgColor1.SelectedColor);
            setting.Default.BgColor2 = CommonFunction.ConvertoColor(bgColor2.SelectedColor);
            if (SelectedFont.SelectedItem != null)
                setting.Default.CurrentFont = SelectedFont.Text;
            if (SelectedFontSize.SelectedItem != null)
                setting.Default.CurrentSize = CommonFunction.ConvertInt(SelectedFontSize.Text);
            if (rcboMonitor.SelectedIndex > 0)
                setting.Default.ProjectorScreen = rcboMonitor.SelectedIndex;
            setting.Default.Save();
            this.Close();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
