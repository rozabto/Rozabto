using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rozabto.Model;
using Rozabto.View;

namespace UnitTesting_Rozabto
{
    [TestClass]
    public class ViewTests
    {
        [TestMethod]
        public void TestMethod_Songs()
        {
            var songs = new Songs();
        }

        [TestMethod]
        public void TestMethod_Album()
        {
            var album = new Rozabto.View.Album();
        }

        [TestMethod]
        public void TestMethod_PlayLists()
        {
            var playLists = new Rozabto.View.Playlists();
        }

        [TestMethod]
        public void TestMethod_Band()
        {
            var band = new Rozabto.View.Bands();
        }

    }
}
