using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector.Model
{
    public class Chapter
    {
        public int BookId { get; set; }
        public int ChapterId { get; set; }
        public int TotalVerse { get; set; }
        public List<string> Verses { get; set; }

    }

    public class Verse
    {
        public int BookId { get; set; }
        public string Id { get; set; }
        public int ChapterId { get; set; }
        public string Value { get; set; }

    }

    public class Video
    {
        public string Location { get; set; }
        public string Name { get; set; }

    }
}
