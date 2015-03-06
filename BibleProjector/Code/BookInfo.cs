using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector.Code
{
    public class BookInfo
    {
        public BookInfo()
        {
            this.Chapters = new List<ChapterInfo>();
        }

        public int CurrentChapterIndex { set; get; }

        public int CurrentPageIndex { set; get; }

        public int CharacterCount { set; get; }

        public List<ChapterInfo> Chapters { set; get; }
    }
}
