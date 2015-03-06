using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BibleProjector.Code
{
    public class SlideTransition : PageTransition
    {
        FrameworkElement[] pageContainers = new FrameworkElement[3];
        TranslateTransform[] translateTransforms = new TranslateTransform[3];

        public override double AnimationDuration
        {
            get { return 500; }
        }

        public override void Attach(Panel containerPanel,
                                    FrameworkElement pageContainer0,
                                    FrameworkElement pageContainer1,
                                    FrameworkElement pageContainer2)
        {
            pageContainers[0] = pageContainer0;
            pageContainers[1] = pageContainer1;
            pageContainers[2] = pageContainer2;

            for (int i = 0; i < 3; i++)
            {
                translateTransforms[i] = new TranslateTransform();
                pageContainers[i].RenderTransform = translateTransforms[i];
            }
        }

        public override void Detach()
        {
            foreach (FrameworkElement pageContainer in pageContainers)
                pageContainer.RenderTransform = null;
        }

        protected override void ShowPageTransition(int baseIndex, double fraction)
        {
            int nextIndex = (baseIndex + 1) % 3;
            int prevIndex = (baseIndex + 2) % 3;

            translateTransforms[baseIndex].X = -fraction * pageContainers[prevIndex].ActualWidth;
            translateTransforms[nextIndex].X = translateTransforms[baseIndex].X + pageContainers[baseIndex].ActualWidth;
            translateTransforms[prevIndex].X = translateTransforms[baseIndex].X - pageContainers[prevIndex].ActualWidth;
        }
    }
}