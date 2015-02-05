using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibleProjector.Model;

namespace BibleProjector.Code
{
    public class CommonFunction
    {
        private static readonly char[] _delimiterPost = { ',' };
        private static readonly char[] _delimiterslash = { '/' };
        private static DateTime MinValue = new DateTime(1900, 1, 1);

        public static string HumanReadFile(string fileName)
        {
            var file = fileName.Trim();
            if (!string.IsNullOrEmpty(file) && file.Length > 0 && file.IndexOf(@"\", StringComparison.Ordinal) > 0)
            {
                file = file.Substring(file.LastIndexOf(@"\") + 1, file.Length - (file.LastIndexOf(@"\") + 1));
            }
            if (!string.IsNullOrEmpty(file) && file.Length > 0 && file.IndexOf(@"/", StringComparison.Ordinal) > 0)
            {
                file = file.Substring(file.LastIndexOf(@"/") + 1, file.Length - (file.LastIndexOf(@"/") + 1));
            }
            return file;
        }

        public static double ConvertDouble(string str)
        {
            double res;
            double.TryParse(str, out res);
            return res;
        }

        /// <summary>
        /// On case user only input year, or year month or full year and month
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ConvertCustomDateTime(string str)
        {
            var res = DateTime.MinValue;
            try
            {
                if (!string.IsNullOrEmpty(str) && str.IndexOf("/", System.StringComparison.Ordinal) > -1)
                {
                    var split = str.Split(_delimiterslash);

                    switch (split.Count())
                    {

                        case 1:
                            {
                                res = new DateTime(ConvertInt(split[0]), 2, 1);
                                break;
                            }
                        case 2:
                            {
                                res = new DateTime(ConvertInt(split[1]), ConvertInt(split[0]), 1);
                            }
                            break;
                        case 3:
                            {
                                res = new DateTime(ConvertInt(split[2]), ConvertInt(split[1]), ConvertInt(split[0]));
                            }
                            break;
                        default:
                            DateTime.TryParse(str, new CultureInfo("vi-VN"), DateTimeStyles.None, out res);
                            break;
                    }
                }
                else
                {
                    res = new DateTime(ConvertInt(str), 1, 1);
                }
            }
            catch (Exception e)
            {
                res = DateTime.MinValue;
            }
            return res;

        }


        public static DateTime ConvertDateTime(string str)
        {
            DateTime res;
            DateTime.TryParse(str, new CultureInfo("vi-VN"), DateTimeStyles.None, out res);
            return res;
        }

        public static DateTime ConvertDateTime(string str, bool isVietNam)
        {
            DateTime res;
            DateTime.TryParse(str, new CultureInfo("vi-VN"), DateTimeStyles.None, out res);
            return res;
        }

        public static int ConvertInt(long str)
        {
            int res;
            int.TryParse(str.ToString(CultureInfo.InvariantCulture), out res);
            return res;
        }
        public static int ConvertInt(string str)
        {
            int res;
            int.TryParse(str, out res);
            return res;
        }

        public static Dictionary<string, string> GetBibleList()
        {
            var result = new Dictionary<string, string>
            {
                {"Sáng thế ký", "Genesis"},
                {"Xuất Ê-díp-tô ký", "Exodus"},
                {"Lê-vi ký", "Leviticus"},
                {"Dân số ký", "Numbers"},
                {"Phục truyền luật lệ ký", "Deuteronomy"},
                {"Giô suê", "Joshua"},
                {"Các Quan Xét", "Judges"},
                {"Ru-tơ", "Ruth"},
                {"I Sa-mu-ên", "1 Samuel"},
                {"II Sa-mu-ên", "2 Samuel"},
                {"I Các vua", "1 Kings"},
                {"II Các vua", "2 Kings"},
                {"I Sử ký", "1 Chronicles"},
                {"II Sử ký", "2 Chronicles"},
                {"E-xơ-ra", "Ezra"},
                {"Nê-hê-mi", "Nehemiah"},
                {"Ê-xơ-tê", "Esther"},
                {"Gióp", "Job"},
                {"Thi Thiên", "Psalm"},
                {"Châm ngôn", "Proverbs"},
                {"Truyền đạo", "Ecclesiastes"},
                {"Nhã ca", "Song of Solomon"},
                {"Ê-sai", "Isaiah"},
                {"Giê-rê-mi", "Jeremiah"},
                {"Ca thương", "Lamentations"},
                {"Ê-xê-chi-ên", "Ezekiel"},
                {"Đa-ni-ên", "Daniel"},
                {"Ô-sê", "Hosea"},
                {"Giô-ên", "Joel"},
                {"A-mốt", "Amos"},
                {"Áp-đia", "Obadiah"},
                {"Mi-chê", "Micah"},
                {"Giô-na", "Jonah"},
                {"Na-hum", "Nahum"},
                {"Ha-ba-cúc", "Habakkuk"},
                {"Sô-phô-ni", "Zephaniah"},
                {"A-ghê", "Haggai"},
                {"Xa-cha-ri", "Zechariah"},
                {"Ma-la-chi", "Malachi"},
                {"Ma-thi-ơ", "Matthew"},
                {"Mác", "Mark"},
                {"Luc-ca", "Luke"},
                {"Giăng", "John"},
                {"Công vụ các sứ đồ", "Acts"},
                {"Rô-ma", "Romans"},
                {"I Cô-rinh-tô", "1 Corinthians"},
                {"II Cô-rinh-tô", "2 Corinthians"},
                {"Ga-la-ti", "Galatians"},
                {"Ê-phê-sô", "Ephesians"},
                {"Phi-líp", "Philippians"},
                {"Cô-lô-se", "Colossians"},
                {"I Tê-sa-lô-ni-ca", "1 Thessalonians"},
                {"II Tê-sa-lô-ni-ca", "2 Thessalonians"},
                {"I Ti-mô-thê", "1 Timothy"},
                {"II Ti-mô-thê", "2 Timothy"},
                {"Tít ", "Titus"},
                {"Phi-lê-môn", "Philemon"},
                {"Hê-bơ-rơ", "Hebrews"},
                {"Gia-cơ", "James"},
                {"I Phi-e-rơ", "1 Peter"},
                {"II Phi-e-rơ", "2 Peter"},
                {"I Giăng", "1 John"},
                {"II Giăng", "2 John"},
                {"III Giăng", "3 John"},
                {"Giu-đe", "Jude"},
                {"Khải huyền", "Revelation"}
            };
            return result;
        }

       
    }
}
