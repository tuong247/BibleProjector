using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BibleProjector.Code
{
    public class PageProvider : IPageProvider
    {
        const int FirstLineIndent = 3;              // in units of space width
        const double ParagraphSpacing = 1.0 / 3;    // in units of character height
        static readonly Brush BlackBrush = new SolidColorBrush(Colors.Black);

        // Regenerated every time book is loaded
        List<ParagraphInfo> paragraphs = new List<ParagraphInfo>();

        FontMetrics fontMetrics = new FontMetrics(new Font("Cambria"));
        Size pageSize;
        double fontSize;

        // Only public property
        public BookInfo Book { set; get; }

        protected void PreprocessBook(StreamReader stream)
        {
            // Occurs every time program is run
            GenerateParagraphs(stream);

            // Occurs only first time program is run
            if (this.Book.Chapters.Count == 0)
                GenerateChapters();
        }

        private void GenerateParagraphs(StreamReader stream)
        {

            string line = null;
            string prevLine = null;
            int prevIndent = 0;
            var paragraphBuilder = new StringBuilder();

            while (null != (line = stream.ReadLine()))
            {
                // Trim line of all space and find the line indent
                line = line.TrimEnd(' ');
                int indent = line.Length;
                line = line.TrimStart(' ');
                indent -= line.Length;

                // A new paragraph is always implied with a line of zero length
                bool newParagraph = line.Length == 0;

                if (!newParagraph && paragraphBuilder.Length > 0 && prevLine != null)
                {
                    // This is the case for a list ("contents" for example)
                    //  where each line has a different indent
                    if (indent != prevIndent)
                    {
                        newParagraph = true;
                    }

                        // But some indented text needs to be concatenated (epistoles, for example)
                    else if (indent != 0)
                    {
                        string firstWord = line;
                        int spaceIndex = line.IndexOf(' ');

                        if (spaceIndex != -1)
                            firstWord = firstWord.Substring(0, spaceIndex);

                        // This works for some books, but not for "Phineas Finn" where
                        //  at least the first letter has shorter lines than normal
                        // if (indent + prevLine.Length + 1 + firstWord.Length <= 70)
                        //     newParagraph = true;
                    }
                }

                // Check for a possible paragraph termination
                if (newParagraph)
                {
                    if (paragraphBuilder.Length > 0)
                    {
                        SubstituteCharacters(paragraphBuilder);

                        // Create a new ParagraphInfo for the paragraph
                        ParagraphInfo paragraph = new ParagraphInfo
                        {
                            Indent = prevIndent,
                            Text = paragraphBuilder.ToString()
                        };
                        paragraphs.Add(paragraph);

                        // Clear the StringBuilder
                        paragraphBuilder.Remove(0, paragraphBuilder.Length);
                    }

                    if (line.Length > 0)
                    {
                        // Append the line to the paragraph
                        paragraphBuilder.Append(line);
                    }
                    else
                    {
                        // Add the current line that terminates the paragraph
                        ParagraphInfo emptyParagraph = new ParagraphInfo
                        {
                            Indent = 0,
                            Text = line
                        };
                        paragraphs.Add(emptyParagraph);
                    }
                }
                else
                {
                    // Insert a space between appended lines assembled into paragraph
                    if (paragraphBuilder.Length > 0)
                        paragraphBuilder.Append(" ");

                    paragraphBuilder.Append(line);
                }
                prevLine = line;
                prevIndent = indent;
            }

            // Add the last paragraph if there's one in progress
            if (paragraphBuilder.Length > 0)
            {
                SubstituteCharacters(paragraphBuilder);

                ParagraphInfo paragraph = new ParagraphInfo
                {
                    Indent = prevIndent,
                    Text = paragraphBuilder.ToString()
                };
                paragraphs.Add(paragraph);
            }
        }

        // Does fancy quotes and em-dashes
        void SubstituteCharacters(StringBuilder paragraphBuilder)
        {
            // Replace boring double quotes with fancy quotes
            bool isCloseQuote = false;

            for (int i = 0; i < paragraphBuilder.Length; i++)
            {
                if (paragraphBuilder[i] == '"')
                {
                    paragraphBuilder[i] = isCloseQuote ? '\x201D' : '\x201C';
                    isCloseQuote ^= true;
                }
            }

            // Replace double dashes with em dashes separated by spaces
            //  (but only when they're surrounded by non-dashes)
            int index = 0;

            while (index < paragraphBuilder.Length - 1)
            {
                if (paragraphBuilder[index] == '-' &&                       // current is dash
                    paragraphBuilder[index + 1] == '-' &&                   // next one is dash
                    (index == 0 || paragraphBuilder[index - 1] != '-') &&   // there isn't an earlier dash
                    (index == paragraphBuilder.Length - 2 || paragraphBuilder[index + 2] != '-')) // there isn't a later dash
                {
                    if (index == 0 && paragraphBuilder.Length == 2)     // double-dash appears alone
                    {
                        paragraphBuilder.Remove(index, 2);
                        paragraphBuilder.Insert(index, "\x2014");
                    }
                    else if (index == 0)                                // double-dash at beginnning of paragraph
                    {
                        paragraphBuilder.Remove(index, 2);
                        paragraphBuilder.Insert(index, "\x2014 ");
                        index += 1;
                    }
                    else if (index >= paragraphBuilder.Length - 3)      // double-dash not followed by at least two characters
                    {
                        paragraphBuilder.Remove(index, 2);
                        paragraphBuilder.Insert(index, " \x2014");
                        index += 1;
                    }
                    else
                    {
                        paragraphBuilder.Remove(index, 2);
                        paragraphBuilder.Insert(index, " \x2014 ");
                        index += 2;
                    }
                }

                index += 1;
            }
        }

        void GenerateChapters()
        {
            // Set so the beginning of the book becomes the first chapter
            int consecutiveBlankLineCount = 10;
            int accumulatedCharacterCount = 0;
            ChapterInfo chapterInfo = null;

            for (int paragraphIndex = 0; paragraphIndex < paragraphs.Count; paragraphIndex++)
            {
                if (paragraphs[paragraphIndex].Text.Length == 0)
                {
                    consecutiveBlankLineCount += 1;
                }
                else
                {
                    if (consecutiveBlankLineCount > 2)
                    {
                        PageInfo pageInfo = new PageInfo
                        {
                            ParagraphIndex = paragraphIndex,
                            CharacterIndex = 0,
                            AccumulatedCharacterCount = accumulatedCharacterCount
                        };

                        chapterInfo = new ChapterInfo
                        {
                            Title = paragraphs[paragraphIndex].Text.Length < 100 ? paragraphs[paragraphIndex].Text : paragraphs[paragraphIndex].Text.Substring(0, 100),
                            FirstParagraph = paragraphIndex,
                            ParagraphCount = 1
                        };
                        chapterInfo.Pages.Add(pageInfo);
                        this.Book.Chapters.Add(chapterInfo);
                    }
                    else
                    {
                        // Accumulate number of paragraphs in the chapter
                        chapterInfo.ParagraphCount += 1 + consecutiveBlankLineCount;
                    }
                    consecutiveBlankLineCount = 0;
                    accumulatedCharacterCount += paragraphs[paragraphIndex].Text.Length;
                }
            }

            // Flag the last chapter
            this.Book.Chapters[this.Book.Chapters.Count - 1].IsLastChapter = true;
            this.Book.CharacterCount = accumulatedCharacterCount;
        }

        public void SetPageSize(Size size)
        {
            if (pageSize != size)
            {
                pageSize = size;

                // TODO: If the page size can really change,
                //      pagination information needs to be invalidated
            }
        }

        public void SetFont(FontFamily fontFamily, double fontSize)
        {
            fontMetrics = new FontMetrics(new Font(fontFamily.Source));

            // this.fontFamily = fontFamily;
            this.fontSize = fontSize;

            // TODO: If the font and font size can really change,
            //      pagination information needs to be invalidated
        }


        public FrameworkElement QueryLastPage(int chapterIndex, out int pageIndex)
        {
            return GetLastPage(chapterIndex, out pageIndex, false);
        }

        public FrameworkElement GetLastPage(int chapterIndex, out int pageIndex)
        {
            FrameworkElement element = GetLastPage(chapterIndex, out pageIndex, true);
            return element;
        }

        FrameworkElement GetLastPage(int chapterIndex, out int pageIndex, bool paginateIfNecessary)
        {
            if (pageSize.IsEmpty)
            {
                pageIndex = -1;
                return null;
            }

            if (chapterIndex < 0 || chapterIndex > this.Book.Chapters.Count - 1)
            {
                pageIndex = -1;
                return null;
            }

            ChapterInfo chapter = this.Book.Chapters[chapterIndex];
            int lastIndex = chapter.Pages.Count - 1;

            if (chapter.Pages[lastIndex].IsLastPageInChapter &&
                chapter.Pages[lastIndex].IsPaginated)               // IsPaginated should be implied
            {
                pageIndex = lastIndex;
                return BuildPageElement(chapter, chapter.Pages[lastIndex]);
            }

            if (!paginateIfNecessary)
            {
                pageIndex = -1;
                return null;
            }

            while (!chapter.Pages[chapter.Pages.Count - 1].IsLastPageInChapter)
                Paginate(chapterIndex, chapter.Pages.Count - 1);

            pageIndex = chapter.Pages.Count - 1;
            return BuildPageElement(chapter, chapter.Pages[chapter.Pages.Count - 1]);
        }

        public FrameworkElement GetPage(int chapterIndex, int pageIndex)
        {
            if (pageSize.IsEmpty)
                return null;

            if (pageIndex < 0)
                return null;

            if (chapterIndex < 0 || chapterIndex > this.Book.Chapters.Count - 1)
                return null;

            ChapterInfo chapter = this.Book.Chapters[chapterIndex];

            // Check if the last page of the paragraph has already been established, 
            //  and the requested page is greater than that last page
            if (chapter.Pages[chapter.Pages.Count - 1].IsLastPageInChapter && pageIndex > chapter.Pages.Count - 1)
                return null;

            // If the last page of a chapter is requested, and it's already available, get it
            if (pageIndex < chapter.Pages.Count && chapter.Pages[pageIndex].IsLastPageInChapter)
            {
                return BuildPageElement(chapter, chapter.Pages[pageIndex]);
            }

            // Otherwise, if the page has already been paginated, get it
            if (pageIndex + 1 < chapter.Pages.Count && chapter.Pages[pageIndex].IsPaginated)
            {
                return BuildPageElement(chapter, chapter.Pages[pageIndex]);
            }

            if (pageIndex != chapter.Pages.Count - 1)
                throw new Exception(String.Format("GetPage can only paginate one page per call: pageIndex = {0} while chapter.Pages.Count = {1}", pageIndex, chapter.Pages.Count));

            PageInfo pageInfo = chapter.Pages[pageIndex];

            PageInfo nextPageInfo = Paginate(chapter, pageInfo);

            if (nextPageInfo != null)   // ie, if !pageInfo.IsLastPageInChapter
                chapter.Pages.Add(nextPageInfo);

            return BuildPageElement(chapter, pageInfo);
        }

        bool Paginate(int chapterIndex, int pageIndex)
        {
            if (chapterIndex < 0 || chapterIndex > this.Book.Chapters.Count - 1)
                return false;

            ChapterInfo chapter = this.Book.Chapters[chapterIndex];

            if (pageIndex < chapter.Pages.Count && chapter.Pages[pageIndex].IsPaginated)
                return true;

            if (chapter.Pages[chapter.Pages.Count - 1].IsLastPageInChapter)
                return false;

            PageInfo pageInfo = chapter.Pages[pageIndex];
            PageInfo nextPageInfo = Paginate(chapter, pageInfo);

            if (nextPageInfo != null)
                chapter.Pages.Add(nextPageInfo);

            return true;
        }

        // TODO: Could pass chapter and pageIndex here, to obtain pageInfo within, and then can set nextPageInfo
        PageInfo Paginate(ChapterInfo chapter, PageInfo pageInfo)
        {
            // Get dimensions of space character
            Size spaceSize = fontMetrics.MeasureText(" ");
            int lineHeight = (int)Math.Ceiling(fontSize * spaceSize.Height);
            int spaceWidth = (int)Math.Ceiling(fontSize * spaceSize.Width);

            // Initialize a couple variables
            int accumHeight = 0;
            int characterCount = pageInfo.AccumulatedCharacterCount;
            PageInfo nextPageInfo = null;
            int paragraphIndex = pageInfo.ParagraphIndex;
            int characterIndex = pageInfo.CharacterIndex;

            // Loop for lines on page
            while (true)
            {
                ParagraphInfo paragraph = paragraphs[paragraphIndex];

                // For blank paragraphs, leave a gap...
                if (paragraph.Text.Length == 0)
                {
                    // .. but not when it's at the top of the page
                    if (accumHeight > 0)
                        accumHeight += (int)Math.Ceiling(ParagraphSpacing * lineHeight);
                }
                // Normal case for non-blank paragraphs
                else
                {
                    // This is the amount to indent the entire paragraph
                    int indent = spaceWidth * paragraph.Indent;

                    // Apply first line indent if paragraph indent is zero, and it's
                    //  not the first paragraph in the chapter
                    if (indent == 0 &&
                        characterIndex == 0 &&
                        paragraph != paragraphs[chapter.FirstParagraph])
                    {
                        indent = spaceWidth * FirstLineIndent;
                    }

                    // Start off accumulated line width with the indent
                    int accumWidth = indent;

                    // Loop for words within line -- always the same paragraph
                    while (true)
                    {
                        // Find next space in paragraph
                        int spaceIndex = paragraph.Text.IndexOf(' ', characterIndex);
                        int wordCharacters = (spaceIndex == -1 ? paragraph.Text.Length : spaceIndex) - characterIndex;
                        int wordWidth = (int)Math.Ceiling(fontSize * fontMetrics.MeasureText(paragraph.Text, characterIndex, wordCharacters).Width);

                        // Check if the accumulated width exceeds the page width, 
                        //      but make sure to have at least one word.
                        if (accumWidth > indent && accumWidth + wordWidth >= pageSize.Width)
                            break;

                        WordInfo word = new WordInfo
                        {
                            LocationLeft = accumWidth,
                            LocationTop = accumHeight,
                            ParagraphIndex = paragraphIndex,
                            CharacterIndex = characterIndex,
                            CharacterCount = wordCharacters,
                        };
                        pageInfo.Words.Add(word);

                        accumWidth += wordWidth;
                        characterCount += wordCharacters + (spaceIndex == -1 ? 0 : 1);
                        characterIndex += wordCharacters + (spaceIndex == -1 ? 0 : 1);

                        // End of line because it's the end of the paragraph
                        if (spaceIndex == -1)
                        {
                            characterIndex = 0;
                            break;
                        }

                        // End of line because no more words will fit
                        if (accumWidth + spaceWidth >= pageSize.Width)
                            break;

                        accumWidth += spaceWidth;

                        // Otherwise, point to next word
                        characterIndex = spaceIndex + 1;

                        // ready for next word
                    }
                    // Bump up the accumulated height of the page
                    accumHeight += lineHeight;
                }

                // Start a new line, but check first if it's a new paragraph
                if (characterIndex == 0)
                {
                    // If we've been working with the last paragraph in the chapter,
                    //      we're also done with the page
                    if (paragraphIndex + 1 == chapter.FirstParagraph + chapter.ParagraphCount)
                    {
                        pageInfo.IsLastPageInChapter = true;
                        break;
                    }
                    // Otherwise, kick up the paragraphIndex
                    paragraphIndex++;
                }

                // Check if the next paragraph is too much for this page
                if ((paragraphs[paragraphIndex].Text.Length == 0 &&
                            accumHeight + lineHeight / 2 > pageSize.Height) ||
                    accumHeight + lineHeight > pageSize.Height)
                {
                    break;
                }
            }

            pageInfo.IsPaginated = true;

            // Create PageInfo for next page
            if (!pageInfo.IsLastPageInChapter)
            {
                nextPageInfo = new PageInfo
                {
                    ParagraphIndex = paragraphIndex,
                    CharacterIndex = characterIndex,
                    AccumulatedCharacterCount = characterCount
                };
            }

            return nextPageInfo;
        }

        FrameworkElement BuildPageElement(ChapterInfo chapter,
                                          PageInfo pageInfo)
        {
            if (pageInfo.Words.Count == 0)
            {
                Paginate(chapter, pageInfo);
            }

            Canvas canvas = new Canvas();

            foreach (WordInfo word in pageInfo.Words)
            {
                TextBlock txtblk = new TextBlock
                {
                    FontFamily = fontMetrics.Font.FontFamily,
                    FontSize = this.fontSize,
                    Text = paragraphs[word.ParagraphIndex].Text.
                                Substring(word.CharacterIndex,
                                          word.CharacterCount),
                    Tag = word
                };

                Canvas.SetLeft(txtblk, word.LocationLeft);
                Canvas.SetTop(txtblk, word.LocationTop);
                canvas.Children.Add(txtblk);
            }
            return canvas;
        }
    }
}

