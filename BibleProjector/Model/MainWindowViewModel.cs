using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BibleProjector.Code;

namespace BibleProjector.Model
{
    public class MainWindowViewModel:ViewModel
    {
        #region Item Sources

        public ObservableCollection<Bible> Bibles { get; set; }
        public ObservableCollection<Chapter> Chapters { get; set; }
        public ObservableCollection<Video> Videos { get; set; }
        public ObservableCollection<Chapter> AvailableChapter { get; set; }
        public ObservableCollection<Verse> AvailableVerses { get; set; }

        #endregion

        #region Binding Examples

        // Property backing fields.
        Bible selectedBible;
        Chapter selectedChapter;
        readonly XmlDocument _xmldoc = new XmlDocument();
        readonly Dictionary<string, int> _chapterCount = new Dictionary<string, int>();
        /// <summary>
        /// Gets or sets the shipping destination country.
        /// </summary>
        public Bible SelectedBible
        {
            get { return selectedBible; }
            set
            {
                // Set the local field value.
                selectedBible = value;

                // The lists of available shipping carriers/methods are invalidated now, so we need to update it.
                AvailableChapter.Clear();
                if (value != null)
                {
                    AvailableChapter.Clear();
                    int totalchapter;
                    _chapterCount.TryGetValue(value.Name,out totalchapter);
                    for (var i = 1; i < totalchapter; i++)
                    {
                        AvailableChapter.Add(new Chapter(){ChapterId = i});
                    }
                   
                }
                NotifyPropertyChanged("SelectedBible");
            }
        }

        /// <summary>
        /// Gets or sets the selected shipping carrier.
        /// </summary>
        public Chapter SelectedChapter
        {
            get { return selectedChapter; }
            set
            {
                selectedChapter = value;
                if (selectedChapter != null)
                {
                    AvailableVerses.Clear();
                    //_xmldoc.Load(@"Data\Vietnamese Bible.xml");
                    var nodes = _xmldoc.SelectSingleNode("/XMLBIBLE/BIBLEBOOK[@bname='" + selectedBible.Name + "']/CHAPTER[@cnumber='" + selectedChapter.ChapterId + "']");
                    if (nodes == null) return;
                    foreach (XmlNode i in nodes.ChildNodes)
                    {
                        var doan = "";
                        if (i.Attributes != null)
                        {
                            doan = i.Attributes[0].Value;
                        }
                        string noidung = doan + " " + i.FirstChild.Value;

                        AvailableVerses.Add(new Verse() { Value = noidung, Id = doan });
                    }
                }
               
                // The list of available shipping methods is now invalidated.  Clear the list and populate new shipping methods based
                // on the selected carrier and destination country.
                NotifyPropertyChanged("SelectedChapter");
            }
        }

        #endregion
        #region Constructors

        public MainWindowViewModel()
        {
            // Instantiate our item sources.
            Bibles = new ObservableCollection<Bible>();
            Chapters = new ObservableCollection<Chapter>();
            AvailableChapter = new ObservableCollection<Chapter>();
            AvailableVerses = new ObservableCollection<Verse>();
            Videos = new ObservableCollection<Video>();
            SetBible(App.BibileLocation);
            SetVideos(App.MotionLocation);
        }

        public void SetVideos(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            Videos.Clear();
            var info = dirInfo.GetFiles();
            foreach (var f in info)
            {
                Videos.Add(new Video(){Location = f.FullName, Name = f.Name});
            }
            NotifyPropertyChanged("SetVideos");
        }

        public void SetBible(string path)
        {
            _chapterCount.Clear();
            var list = CommonFunction.GetBibleList().ToList();
            foreach (var bibile in list)
            {
                var id = list.IndexOf(bibile);
                Bibles.Add(new Bible()
                {
                    Id = id,Book = bibile.Key,Name = bibile.Value
                });
            }

            _xmldoc.Load(path);
            var xmlnodes = _xmldoc.SelectNodes("/XMLBIBLE/BIBLEBOOK");
            var bibleBooks = new Dictionary<string, string>();
            if (xmlnodes != null)
                foreach (var xmlnode in xmlnodes.Cast<XmlNode>().Where(xmlnode => xmlnode.Attributes != null))
                {
                    if (xmlnode.Attributes == null) continue;
                    string name = xmlnode.Attributes.GetNamedItem("bname").Value;
                    bibleBooks.Add(name, name);
                    var xmlNodeList = xmlnode.SelectNodes("CHAPTER");
                    if (xmlNodeList == null) continue;
                    if (xmlnode.Attributes != null)
                        _chapterCount.Add(xmlnode.Attributes.GetNamedItem("bname").Value, xmlNodeList.Count);
                }
            
          
        }

        #endregion
    }
}
