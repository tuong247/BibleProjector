using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BibleProjector.Model
{
    public class PhotoSlideShow
    {
        public PhotoSlideShow()
        {
            //PreviousPhoto = new Uri("");
            //CurrentPhoto = new Uri("");
        }
        public Uri PreviousPhoto { get; set; }
        public Uri CurrentPhoto { get; set; }
    } 
}
