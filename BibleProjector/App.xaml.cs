using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Application = System.Windows.Application;

namespace BibleProjector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static System.Windows.Media.Color Color1 { get; set; }
        public static System.Windows.Media.Color Color2 { get; set; }
        public static string BibileLocation { get; set; }
        public static string MotionLocation { get; set; }
        public static int ProjectorScreen { get; set; }
        public static string CurrentFont { get; set; }
        public static int CurrentSize { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            BibileLocation = @"data\Vietnamese Bible.xml";
            MotionLocation =Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures);
            Color1 = Colors.White;
            Color2 = Colors.Yellow;
            var screens = Screen.AllScreens;
            ProjectorScreen = screens.Count() - 1;
        }

        private void Application_Activated(object sender, EventArgs e)
        {

        }

        private void Application_Deactivated(object sender, EventArgs e)
        {

        }
    }
}
