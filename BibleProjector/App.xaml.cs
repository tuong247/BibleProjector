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

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //ProjectorScreen = screens.Count() - 1;
        }

        private void Application_Activated(object sender, EventArgs e)
        {

        }

        private void Application_Deactivated(object sender, EventArgs e)
        {

        }
    }
}
