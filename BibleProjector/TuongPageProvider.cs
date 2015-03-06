using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibleProjector.Code;

namespace BibleProjector
{
   public class TuongPageProvider: PageProvider
    {
        public TuongPageProvider()
        {
            //this.Book = (Application.Current as App).AppSettings.Book;
            this.Book = new BookInfo();
            using (var stream = new StreamReader(@"Data\pg18000.txt"))
            {
                PreprocessBook(stream);
            }
        }
    }
   
}
