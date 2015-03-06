using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BibleProjector.Code
{
    public class PageInfo
    {
        public PageInfo()
        {
            this.Words = new List<WordInfo>();
        }

        public int ParagraphIndex { set; get; }

        public int CharacterIndex { set; get; }

        public bool IsLastPageInChapter { set; get; }

        public bool IsPaginated { set; get; }

        public int AccumulatedCharacterCount { set; get; }

        [XmlIgnore]
        public List<WordInfo> Words { set; get; }
    }
}
