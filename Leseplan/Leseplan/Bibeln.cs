using System.Collections.Generic;
using System.Linq;

namespace Leseplan
{
    public class Bibeln
    {
        public Bibeln()
        {
            Translations.Add(new BibelServer { Short = "LUT", Text = "BS: Lutherbibel 2017" });
            Translations.Add(new BibelServer { Short = "ELB", Text = "BS: Elberfelder Bibel" });
            Translations.Add(new BibelServer { Short = "HFA", Text = "BS: Hoffnung für Alle" });
            Translations.Add(new BibelServer { Short = "SLT", Text = "BS: Schlachter 2000" });
            Translations.Add(new BibelServer { Short = "ZB", Text = "BS: Zürcher Übersetzung" });
            Translations.Add(new BibelServer { Short = "KJV", Text = "BS: King James Version" });
            Translations.Add(new BibelServer { Short = "NIV", Text = "BS: New International Version" });
            Translations.Add(new BibelServer { Short = "ESV", Text = "BS: English Standard Version" });
            Translations.Add(new BibelServer { Short = "ESV", Text = "BS: English Standard Version" });

            Translations.Add(new BibelCom { Short = "B-DELUT", Trans = "51", Text = "B: Luther 1912" });
            Translations.Add(new BibelCom { Short = "B-HFA", Trans = "73", Text = "B: Hoffnung für alle" });
            Translations.Add(new BibelCom { Short = "B-ELB", Trans = "57", Text = "B: Elberfelder 1905" });
            Translations.Add(new BibelCom { Short = "B-SCH2000", Trans = "157", Text = "B: Schlachter 2000" });
            Translations.Add(new BibelCom { Short = "B-NGU2011", Trans = "108", Text = "B: Neue Genfer Übersetzung" });
        }


        public List<Bibel> Translations { get; set; } = new List<Bibel>();
    }
}
