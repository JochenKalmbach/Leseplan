using System;

namespace Leseplan
{
    public class BibelServer : Bibel
    {
        public override Uri ToUrl(string vers)
        {
            return new Uri($"https://www.bibleserver.com/text/{Short}/{vers}");
        }
    }
}
