using System;

namespace Leseplan
{
    public class BibelCom : Bibel
    {
        public string Trans {get;set;}

        public override Uri ToUrl(string vers)
        {
            return new Uri($"https://www.bible.com/bible/{Trans}/{BibelVersConverter.ToOsis(vers)}");
        }
    }
}
