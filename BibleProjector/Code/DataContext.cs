using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibleProjector.Model;

namespace BibleProjector.Code
{
    public class DataContext
    {
        public ObservableCollection<Category> DanhMuc { get; set; }
        public ObservableCollection<Bible> KinhThanh { get; set; }
    }
}
