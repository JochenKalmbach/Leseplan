using System;

namespace Leseplan
{
    public abstract class Bibel
    {
        public string Short { get; set; }
        public string Text { get; set; }

        public abstract Uri ToUrl(string vers);
    }
}
