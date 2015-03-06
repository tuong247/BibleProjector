using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BibleProjector.Code;

namespace BibleProjector
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage : Window
    {
        public TestPage()
        {
            InitializeComponent();
           var  book = new BookInfo();
            book.CurrentChapterIndex = 0;
            book.CurrentPageIndex = 0;
            Loaded += (object sender, RoutedEventArgs args) =>
            {
                bookViewer1.JumpToPage(book.CurrentChapterIndex, book.CurrentPageIndex);
            };
        }
    }
}
