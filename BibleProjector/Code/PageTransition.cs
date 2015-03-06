using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BibleProjector.Code
{
    public abstract class PageTransition : DependencyObject
    {
        public static readonly DependencyProperty FractionalBaseIndexProperty =
            DependencyProperty.Register("FractionalBaseIndex",
                typeof(double),
                typeof(PageTransition),
                new PropertyMetadata(-1.0, OnTransitionChanged));

        public double FractionalBaseIndex
        {
            set { SetValue(FractionalBaseIndexProperty, value); }
            get { return (double)GetValue(FractionalBaseIndexProperty); }
        }

        public virtual double AnimationDuration
        {
            get { return 1000; }
        }

        static void OnTransitionChanged(DependencyObject obj,
                                        DependencyPropertyChangedEventArgs args)
        {
            (obj as PageTransition).OnTransitionChanged(args);
        }

        void OnTransitionChanged(DependencyPropertyChangedEventArgs args)
        {
            double fraction = (3 + this.FractionalBaseIndex) % 1;
            int baseIndex = (int)(3 + this.FractionalBaseIndex - fraction) % 3;
            ShowPageTransition(baseIndex, fraction);
        }

        public abstract void Attach(Panel containerPanel,
                                    FrameworkElement pageContainer0,
                                    FrameworkElement pageContainer1,
                                    FrameworkElement pageContainer2);

        public abstract void Detach();

        protected abstract void ShowPageTransition(int baseIndex, double fraction);
    }
}
