using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rozabto.Migrations;

namespace UnitTesting_Rozabto
{
    [TestClass]
    public class Migration
    {
        [TestMethod]
        public void TestMethod_test()
        {
            var start = new StartOfDataBase();
            start.Down();
            start.Up();
            var changed = new ChangedABP();
            changed.Down();
            changed.Up();
            var volume = new Volume();
            volume.Down();
            volume.Up();
        }
    }
}
