using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Leseplan.Tests
{
    [TestClass]
    public class BibelVersConverterTests
    {
        [TestMethod]
        public void ToOsis1()
        {
            var lpd = MainPage.ReadLeseplanData();

            foreach(var i in lpd.Entries.SelectMany(p => p.Items))
            {
                var vers = i.Ref;
                var osis = BibelVersConverter.ToOsis(vers);
                Assert.IsNotNull(osis);
            }
         }

        [TestMethod]
        [DataRow("1.Mose1", "GEN.1")]
        [DataRow("Psalm102", "PSA.102")]
        [DataRow("1.Samuel19,19-24", "1SA.19.19-24")]
        [DataRow("Psalm56,142", "PSA.56.142")]
        [DataRow("Obadja", "OBA")]
        public void ToOsis2(string vers, string osis)
        {
            var osis2 = BibelVersConverter.ToOsis(vers);
            Assert.AreEqual(osis, osis2);
        }
    }
}
