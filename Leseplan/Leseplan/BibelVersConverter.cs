using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Leseplan
{
    public static class BibelVersConverter
    {
        static IDictionary<string, string> _Mapping = new Dictionary<string, string>(StringComparer.Ordinal) 
            {
            {"1.Mose", "GEN"},
            {"2.Mose", "EXO"},
            {"3.Mose", "LEV"},
            {"4.Mose", "NUM"},
            {"5.Mose", "DEU"},
            {"Jos", "JOS"},
            {"Richter", "JDG"},
            {"Rut", "RUT"},
            {"1.Samuel", "1SA"},
            {"2.Samuel", "2SA"},
            {"1.Könige", "1KI"},
            {"2.Könige", "2KI"},
            {"1.Chronik", "1CH"},
            {"2.Chronik", "2CH"},
            {"Esra", "EZR"},
            {"Nehemia", "NEH"},
            {"Esther", "EST"},
            {"Hiob", "JOB"},
            {"Psalm", "PSA"},
            {"Sprüche", "PRO"},
            {"Prediger", "ECC"},
            {"Hoheslied", "SNG"},
            {"Jesaja", "ISA"},
            {"Jeremia", "JER"},
            {"Klagelieder", "LAM"},
            {"Hesekiel", "EZK"},
            {"Daniel", "DAN"},
            {"Hosea", "HOS"},
            {"Joel", "JOL"},
            {"Amos", "AMO"},
            {"Obadja", "OBA"},
            {"Jona", "JON"},
            {"Micha", "MIC"},
            {"Nahum", "NAM"},
            {"Habakuk", "HAB"},
            {"Zefania", "ZEP"},
            {"Haggai", "HAG"},
            {"Sacharja", "ZEC"},
            {"Maleachi", "MAL"},

            {"Matthäus", "MAT"},
            {"Markus", "MRK"},
            {"Lukas", "LUK"},
            {"Johannes", "JHN"},
            {"Apostelgeschichte", "ACT"},
            {"Römer", "ROM"},
            {"1.Korinther", "1CO"},
            {"2.Korinther", "2CO"},
            {"Galater", "GAL"},
            {"Epheser", "EPH"},
            {"Philipper", "PHP"},
            {"Kolosser", "COL"},
            {"1.Thessalonicher", "1TH"},
            {"2.Thessalonicher", "2TH"},
            {"1.Timotheus", "1TI"},
            {"2.Timotheus", "2TI"},
            {"Titus", "TIT"},
            {"Philemon", "PHM"},
            {"Hebräer", "HEB"},
            {"Jakobus", "JAS"},
            {"1.Petrus", "1PE"},
            {"2.Petrus", "2PE"},
            {"1.Johannes", "1JN"},
            {"2.Johannes", "2JN"},
            {"3.Johannes", "3JN"},
            {"Judas", "JUD"},
            {"Offenbarung", "REV"},
            };

        static Regex _RegExBook = new Regex(@"^((\d\.)?(\D+))(.*)$", RegexOptions.Compiled);

        static Regex _RegExVers1 = new Regex(@"^(\d+)((,(\d+))([-](\d+))?)?$", RegexOptions.Compiled);

        public static string ToOsis(string vers)
        {
            var m = _RegExBook.Match(vers);
            if (m.Success)
            {
                var grp = m.Groups[1].Value;
                if (_Mapping.ContainsKey(grp))
                {
                    var appr = _Mapping[grp];
                    var vs = m.Groups[4].Value;
                    if (!string.IsNullOrEmpty(vs))
                    {
                        // Map vers
                        var mv = _RegExVers1.Match(vs);
                        if (mv.Success)
                        {
                            if (!string.IsNullOrEmpty(mv.Groups[4].Value))
                            {
                                if (!string.IsNullOrEmpty(mv.Groups[6].Value))
                                {
                                    var s = appr + "." + mv.Groups[1].Value + "." + mv.Groups[4].Value + "-" + mv.Groups[6].Value;
                                    return s;
                                }
                                var s2 = appr + "." + mv.Groups[1].Value + "." + mv.Groups[4].Value;
                                return s2;
                            }
                            var s3 = appr + "." + mv.Groups[1].Value;
                            return s3;
                        }
                    }
                    return appr;
                }
            }
            return null;
        }
    }
}
