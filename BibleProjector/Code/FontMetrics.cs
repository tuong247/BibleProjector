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
    public class FontMetrics
    {
        const int EmSize = 2048;
        private const int dpiSize = 20;
        TextBlock txtblk;
        double height;
        double[][] charWidths = new double[256][];

        public FontMetrics(Font font)
        {
            this.Font = font;

            // Create the TextBlock for all measurements
            txtblk = new TextBlock
            {
                FontFamily = this.Font.FontFamily,
                FontStyle = this.Font.FontStyle,
                FontWeight = this.Font.FontWeight,
                FontSize = EmSize
            };

            // Store the character height
            txtblk.Text = " ";
            height = Math.Ceiling(dpiSize * font.FontFamily.LineSpacing);// txtblk.ActualHeight / EmSize;
        }

        public Font Font { protected set; get; }

        public double this[char ch]
        {
            get
            {
                // Break apart the character code
                int upper = (ushort)ch >> 8;
                int lower = (ushort)ch & 0xFF;

                // If there's no array, create one
                if (charWidths[upper] == null)
                {
                    charWidths[upper] = new double[256];

                    for (int i = 0; i < 256; i++)
                        charWidths[upper][i] = -1;
                }

                // If there's no character width, obtain it
                if (charWidths[upper][lower] == -1)
                {
                    txtblk.Text = ch.ToString();
                    charWidths[upper][lower] = txtblk.ActualWidth / EmSize;
                }
                return charWidths[upper][lower];
            }
        }

        public Size MeasureText(string text)
        {
            //double accumWidth = 0;

            //foreach (char ch in text)
            //    accumWidth += this[ch];
            return new Size(GetTextWidth(text, txtblk.FontSize), height);
        }
        private double GetTextWidth(string text, double fontSize)
        {
            TextBlock txtMeasure = new TextBlock();
            txtMeasure.FontSize = fontSize;
            txtMeasure.Text = text;
            double width = txtMeasure.ActualWidth;
            return width;
        }

        public Size MeasureText(string text, int startIndex, int length)
        {
            double accumWidth = 0;

            for (int index = startIndex; index < startIndex + length; index++)
                accumWidth += this[text[index]];

            return new Size(accumWidth, height);
        }
    }
}
