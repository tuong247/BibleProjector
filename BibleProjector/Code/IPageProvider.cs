using System.Windows;
using System.Windows.Media;

namespace BibleProjector.Code
{
    public interface IPageProvider
    {
        void SetPageSize(Size size);

        void SetFont(FontFamily fontFamily, double FontSize);

        FrameworkElement GetPage(int chapterIndex, int pageIndex);

        FrameworkElement QueryLastPage(int chapterIndex, out int pageIndex);

        FrameworkElement GetLastPage(int chapterIndex, out int pageIndex);
    }
}
