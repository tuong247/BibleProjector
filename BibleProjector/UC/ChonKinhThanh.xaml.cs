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
using BibleProjector.Annotations;
using BibleProjector.Model;

namespace BibleProjector.UC
{
    /// <summary>
    /// Interaction logic for ChonKinhThanh.xaml
    /// </summary>
    public partial class ChonKinhThanh : Window
    {
        public bool IsChangeData { get; set; }
        public ChonKinhThanh()
        {
            InitializeComponent();
            rcboBible.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string bible = rcboBible.SelectionBoxItem.ToString();
            string data = "data\\" + bible + ".xml";
            //Settings.Default.BibileLocation = data;
            //Settings.Default.Save();
            IsChangeData = true;
            this.Close();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            IsChangeData = false;
            this.Close();
        }

       
    }
}
