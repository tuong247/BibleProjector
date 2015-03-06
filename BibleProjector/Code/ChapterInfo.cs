using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector.Code
{
    public class ChapterInfo
    {
        // TODO: Need PageSize, FontFamily, and FontSize used to paginate the chapter

        public ChapterInfo()
        {
            this.Pages = new List<PageInfo>();
        }

        public string Title { set; get; }

        public int FirstParagraph { set; get; }

        public int ParagraphCount { set; get; }

        public bool IsLastChapter { set; get; }

        public List<PageInfo> Pages { set; get; }
    }
}
