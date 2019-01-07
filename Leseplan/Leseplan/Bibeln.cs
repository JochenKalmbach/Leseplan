using System;
using System.Collections.Generic;
using System.Linq;

namespace Leseplan
{
    public class Bibeln
    {
        public Bibeln()
        {
            Translations.Add(new Bibel { Short = "LUT", Text = "Lutherbibel 2017" });
            Translations.Add(new Bibel { Short = "ELB", Text = "Elberfelder Bibel" });
            Translations.Add(new Bibel { Short = "HFA", Text = "Hoffnung für Alle" });
            Translations.Add(new Bibel { Short = "SLT", Text = "Schlachter 2000" });
            Translations.Add(new Bibel { Short = "ZB", Text = "Zürcher Übersetzung" });
            Translations.Add(new Bibel { Short = "KJV", Text = "King James Version" });
            Translations.Add(new Bibel { Short = "NIV", Text = "New International Version" });
            Translations.Add(new Bibel { Short = "ESV", Text = "English Standard Version" });
        }


        public List<Bibel> Translations { get; set; } = new List<Bibel>();
    }

    public class Bibel
    {
        public string Short { get; set; }
        public string Text { get; set; }

        public Uri ToUrl(string vers)
        {
            return new Uri($"https://www.bibleserver.com/text/{Short}/{vers}");
        }
    }
}
