using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector.Code
{
    public class WordInfo
    {
        public int LocationLeft { set; get; }

        public int LocationTop { set; get; }

        public int ParagraphIndex { set; get; }

        public int CharacterIndex { set; get; }

        public int CharacterCount { set; get; }
    }
}
