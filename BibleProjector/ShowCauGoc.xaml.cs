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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using BibleProjector.Code;
using BibleProjector.Model;
using BibleProjector.Properties;
using TransitionEffects;

namespace BibleProjector
{
    /// <summary>
    /// Interaction logic for ShowCauGoc.xaml
    /// </summary>
    public partial class ShowCauGoc : Window
    {
        private PhotoSlideShow PhotoSlideShow;
        private DispatcherTimer timer;
       
        private readonly Random _rand = new Random();

        private bool _useOrder = true;
        private int _nextEffect = 0;
        private int _usedTimes = 0;

        private static TransitionEffect[][] transitionEffectsSingle =
        {
            new TransitionEffect[]
            {
                new FadeTransitionEffect(), 
            }, 
            new TransitionEffect[]
            { 
                new FadeTransitionEffect(),
            }, 
        };

        private static TransitionEffect[][] transitionEffects =
        {
            new TransitionEffect[]
            {
                new FadeTransitionEffect(),
                new FadeTransitionEffect(),
            },
        };

        public ShowCauGoc()
        {
            InitializeComponent();
            tblCauGoc.Width = double.NaN;
            tblCauGoc.Height = double.NaN;
            Loaded += Window2_Loaded;
        }

        void Window2_Loaded(object sender, RoutedEventArgs e)
        {
            PhotoSlideShow = new PhotoSlideShow() {};
        }
      
        //private void OnTimerTick(object sender, EventArgs e)

        private void SwapChildren()
        {
            currentChild.Source = PhotoSlideShow.CurrentPhoto;
            oldChild.Effect = null;
        }

        private void ChangePhoto(bool applyTransitionEffect)
        {
            if (PhotoSlideShow != null /*&& !this.oldChild.ImageDownloadInProgress*/ )
            {
                if (applyTransitionEffect)
                {
                    SwapChildren();
                    ApplyTransitionEffect();
                }
                else
                {
                    currentChild.Source = PhotoSlideShow.CurrentPhoto;
                    oldChild.Source = PhotoSlideShow.PreviousPhoto;
                }
            }
        }
       
        private void ApplyTransitionEffect()
        {
            var effectGroup = transitionEffects[_rand.Next(transitionEffects.Length)];
            var effect = effectGroup[_rand.Next(effectGroup.Length)];
            if (_useOrder)
            {
                effectGroup = transitionEffectsSingle[_nextEffect % transitionEffectsSingle.Length];
                effect = effectGroup[0];
                if (++_usedTimes == 2)
                {
                    _usedTimes = 0;
                    _nextEffect++;
                }

                if (_nextEffect == transitionEffectsSingle.Length)
                {
                    _useOrder = false;
                }
            }
            var randEffect = effect as RandomizedTransitionEffect;
            if (randEffect != null)
            {
                randEffect.RandomSeed = _rand.NextDouble();
            }

            var da = new DoubleAnimation(0.0, 1.0, new Duration(TimeSpan.FromSeconds(2.0)), FillBehavior.HoldEnd)
            {
                AccelerationRatio = 0.5,
                DecelerationRatio = 0.5
            };
            da.Completed += TransitionCompleted;
            effect.BeginAnimation(TransitionEffect.ProgressProperty, da);

            var vb = new VisualBrush(oldChild)
            {
                Viewbox = new Rect(0, 0, oldChild.ActualWidth, oldChild.ActualHeight),
                ViewboxUnits = BrushMappingMode.Absolute
            };
            effect.OldImage = vb;
            currentChild.Effect = effect;
        }

        private void TransitionCompleted(object sender, EventArgs e)
        {
            currentChild.Effect = null;
            if (PhotoSlideShow != null)
            {
                oldChild.Source = PhotoSlideShow.CurrentPhoto;
            }
        }

        public void SetCauGoc(string caugoc, bool isShowOtherColor)
        {

            tblCauGoc.Foreground = isShowOtherColor ? new SolidColorBrush(CommonFunction.ConvertoMediaColor(Settings.Default.Color2)) : new SolidColorBrush(CommonFunction.ConvertoMediaColor(Settings.Default.Color1));
            tblCauGoc.FontSize = Settings.Default.CurrentSize > 0 ? Settings.Default.CurrentSize : tblCauGoc.FontSize;
            tblCauGoc.FontFamily = !string.IsNullOrEmpty(Settings.Default.CurrentFont) ? new FontFamily(Settings.Default.CurrentFont) : tblCauGoc.FontFamily;
            tblCauGoc.Text = caugoc;
            dropShadow.Color = isShowOtherColor ? CommonFunction.ConvertoMediaColor(Settings.Default.BgColor2) : CommonFunction.ConvertoMediaColor(Settings.Default.BgColor1);
            tblCauGoc1.Foreground = isShowOtherColor ? new SolidColorBrush(CommonFunction.ConvertoMediaColor(Settings.Default.Color2)) : new SolidColorBrush(CommonFunction.ConvertoMediaColor(Settings.Default.Color1));
            tblCauGoc1.FontSize = Settings.Default.CurrentSize > 0 ? Settings.Default.CurrentSize : tblCauGoc1.FontSize;
            tblCauGoc1.FontFamily = !string.IsNullOrEmpty(Settings.Default.CurrentFont) ? new FontFamily(Settings.Default.CurrentFont) : tblCauGoc1.FontFamily;
            tblCauGoc1.Text = caugoc;
        }

        public void SetMedia(string mediapath)
        {
            currentChild.Source = new Uri(mediapath, UriKind.Absolute);
            PhotoSlideShow.CurrentPhoto = new Uri(mediapath, UriKind.Absolute); 
            if (PhotoSlideShow != null)
            {
                //PhotoSlideShow.MoveNext();
            }

            ChangePhoto(true);
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            currentChild.Position = new TimeSpan(0, 0, 1);
            currentChild.Play();
        }
    }
}
