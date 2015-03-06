using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using FontStyle = System.Windows.FontStyle;

namespace BibleProjector.Code
{
    public class Font : IEquatable<Font>
    {
        public Font()
            : this("")
        {
        }

        public Font(string fontFamily)
            : this(fontFamily, FontStyles.Normal, FontWeights.Normal)
        {
        }

        public Font(string fontFamily, FontStyle fontStyle, FontWeight fontWeight)
        {
            this.FontFamily = new FontFamily(fontFamily);
            this.FontStyle = fontStyle;
            this.FontWeight = fontWeight;
        }

        public System.Windows.Media.FontFamily FontFamily
        {
            private set;
            get;
        }

        public FontStyle FontStyle { protected set; get; }

        public FontWeight FontWeight { protected set; get; }

        public bool Equals(Font that)
        {
            return this.FontFamily.Equals(that.FontFamily) &&
                   this.FontStyle.Equals(that.FontStyle) &&
                   this.FontWeight.Equals(that.FontWeight);
        }
    }
}
