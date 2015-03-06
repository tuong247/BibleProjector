using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector
{
    public class UserPreferences
    {
        #region Member Variables

        private System.Drawing.Color _Color1;
        private System.Drawing.Color _Color2;
        private System.Drawing.Color _BgColor1;
        private System.Drawing.Color _BgColor2;
        private string _MotionLocation;
        private string _CurrentFont;
        private int _CurrentSize;
        private int _ProjectorScreen;
        private string _BibileLocation;


        #endregion //Member Variables

        #region Public Properties

        public System.Drawing.Color Color1
        {
            get { return _Color1; }
            set { _Color1 = value; }
        }

        public System.Drawing.Color Color2
        {
            get { return _Color2; }
            set { _Color2 = value; }
        }

        public System.Drawing.Color BgColor1
        {
            get { return _BgColor1; }
            set { _BgColor1 = value; }
        }

        public System.Drawing.Color BgColor2
        {
            get { return _BgColor2; }
            set { _BgColor2 = value; }
        }
        public string MotionLocation
        {
            get { return _MotionLocation; }
            set { _MotionLocation = value; }
        }
        public string CurrentFont
        {
            get { return _CurrentFont; }
            set { _CurrentFont = value; }
        }
        public int CurrentSize
        {
            get { return _CurrentSize; }
            set { _CurrentSize = value; }
        }
        public int ProjectorScreen
        {
            get { return _ProjectorScreen; }
            set { _ProjectorScreen = value; }
        }
        public string BibileLocation
        {
            get { return _BibileLocation; }
            set { _BibileLocation = value; }
        }

        #endregion //Public Properties

        #region Constructor

        public UserPreferences()
        {
            //Load the settings
            Load();

            //Size it to fit the current screen
            SizeToFit();

            //Move the window at least partially into view
            MoveIntoView();
        }

        #endregion //Constructor

        #region Functions

        /// <summary>
        /// If the saved window dimensions are larger than the current screen shrink the
        /// window to fit.
        /// </summary>
        public void SizeToFit()
        {
            if (_CurrentSize == 0)
            {
                _CurrentSize = 60;
            }
        }

        /// <summary>
        /// If the window is more than half off of the screen move it up and to the left 
        /// so half the height and half the width are visible.
        /// </summary>
        public void MoveIntoView()
        {
            
        }

        private void Load()
        {
            _Color1 = Properties.Settings.Default.Color1;
            _Color2 = Properties.Settings.Default.Color2;
            _BgColor1 = Properties.Settings.Default.BgColor1;
            _BgColor2 = Properties.Settings.Default.BgColor2;
            _BibileLocation = Properties.Settings.Default.BibileLocation;
            _CurrentFont = Properties.Settings.Default.CurrentFont;
            _CurrentSize = Properties.Settings.Default.CurrentSize;
            _ProjectorScreen = Properties.Settings.Default.ProjectorScreen;
        }

        public void Save()
        {
            Properties.Settings.Default.Color1 = _Color1;
            Properties.Settings.Default.Color2 = _Color2;
            Properties.Settings.Default.BgColor1 = _BgColor1;
            Properties.Settings.Default.BgColor2 = _BgColor2;
            Properties.Settings.Default.BibileLocation = _BibileLocation;
            Properties.Settings.Default.CurrentFont = _CurrentFont;
            Properties.Settings.Default.CurrentSize = _CurrentSize;
            Properties.Settings.Default.ProjectorScreen = _ProjectorScreen;
            Properties.Settings.Default.Save();
        }

        #endregion //Functions

    }
}
