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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BibleProjector.Code;

namespace BibleProjector.UC
{
    /// <summary>
    /// Interaction logic for ViewerControl.xaml
    /// </summary>
    public partial class ViewerControl : UserControl
    {
        public ViewerControl()
        {
            InitializeComponent();
            pageHosts[0] = pageHost0;
            pageHosts[1] = pageHost1;
            pageHosts[2] = pageHost2;
        }
        public IPageProvider PageProvider
        {
            set { SetValue(PageProviderProperty, value); }
            get { return (IPageProvider)GetValue(PageProviderProperty); }
        }

        public PageTransition PageTransition
        {
            set { SetValue(PageTransitionProperty, value); }
            get { return (PageTransition)GetValue(PageTransitionProperty); }
        }

        public int ChapterIndex
        {
            protected set { SetValue(ChapterIndexProperty, value); }
            get { return (int)GetValue(ChapterIndexProperty); }
        }

        public int PageIndex
        {
            protected set { SetValue(PageIndexProperty, value); }
            get { return (int)GetValue(PageIndexProperty); }
        }

        public bool JumpToPage(int chapterIndex, int pageIndex)
        {
            // Save chapter and page indices in public properties
            this.ChapterIndex = chapterIndex;
            this.PageIndex = pageIndex;

            if (this.PageProvider != null)
            {
                this.PageProvider.SetPageSize(new Size(pageHost0.ActualWidth, pageHost0.ActualHeight));
                this.PageProvider.SetFont(this.FontFamily, 30);
                FrameworkElement element = this.PageProvider.GetPage(chapterIndex, pageIndex);

                if (element != null)
                {
                    // Set pageHosts for this page and the next page
                    pageHosts[pageHostBaseIndex].Child = element;
                    //SetUpNextPage();
                    //SetUpPreviousPage();

                    // Set the FractionlBaseIndex for the PageTransition
                    this.PageTransition.FractionalBaseIndex = pageHostBaseIndex;
                    return true;
                }
            }
            return false;
        }
        Border[] pageHosts = new Border[3];
        int pageHostBaseIndex = 0;          // current page
        double dragFactor;

        int nextPageChapterIndex;
        int nextPagePageIndex;
        int prevPageChapterIndex;
        int prevPagePageIndex;

        bool animationInProgress, gestureInProgress, gestureHadFlick, canGoNextPage, canGoPrevPage;

        public static readonly DependencyProperty PageProviderProperty =
            DependencyProperty.Register("PageProvider",
                typeof(IPageProvider),
                typeof(ViewerControl),
                new PropertyMetadata(OnPageProviderChanged));

        public static readonly DependencyProperty PageTransitionProperty =
            DependencyProperty.Register("PageTransition",
                typeof(PageTransition),
                typeof(ViewerControl),
                new PropertyMetadata(OnPageTransitionChanged));

        public static readonly DependencyProperty ChapterIndexProperty =
            DependencyProperty.Register("ChapterIndex",
                typeof(int),
                typeof(ViewerControl),
                new PropertyMetadata(0, OnPageChanged));

        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex",
                typeof(int),
                typeof(ViewerControl),
                new PropertyMetadata(0, OnPageChanged));

        public event EventHandler PageChanged;

        Duration CalculateFractionalDuration(double fractionalBaseIndexDestination)
        {
            double fraction = Math.Abs(fractionalBaseIndexDestination -
                                       this.PageTransition.FractionalBaseIndex);
            double msec = fraction * this.PageTransition.AnimationDuration;
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(msec);
            return new Duration(timeSpan);
        }

        static void OnPageProviderChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
        }

        static void OnPageTransitionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as ViewerControl).OnPageTransitionChanged((PageTransition)args.OldValue, (PageTransition)args.NewValue);
        }

        void OnPageTransitionChanged(PageTransition oldPageTransition, PageTransition newPageTransition)
        {
            double fractionalBaseIndex = -1;

            if (oldPageTransition != null)
            {
                fractionalBaseIndex = oldPageTransition.FractionalBaseIndex;
                oldPageTransition.Detach();
            }
            if (newPageTransition != null)
            {
                newPageTransition.Attach(LayoutRoot, pageContainer0, pageContainer1, pageContainer2);
                newPageTransition.FractionalBaseIndex = fractionalBaseIndex;
            }
        }

        static void OnPageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ViewerControl bookViewer = obj as ViewerControl;

            if (bookViewer.PageChanged != null)
                bookViewer.PageChanged(bookViewer, EventArgs.Empty);
        }
    }
}
