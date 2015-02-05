using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector.Model
{
    public class Bible
    {
        public int Id { get; set; }
        public string Book { get; set; }
        public string Name { get; set; }
        public string Translate { get; set; }
        public int TotalChapter { get; set; }
    }
}
