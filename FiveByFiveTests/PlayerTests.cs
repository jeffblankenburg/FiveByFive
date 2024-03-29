﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FiveByFiveLogic;

namespace FiveByFiveTests
{
    [TestClass]
    public class PlayerTests
    {
        public FiveByFiveGame Game;

        [TestInitialize]
        public void InitializePlayerTests()
        {
            Game = new FiveByFiveGame();
            Game.AddPlayer(new Player { Name="Jeff" });
        }

        [TestMethod]
        public void AddTwoStrikesToFirstPlayer()
        {
            Game.AddStrikesToPlayer(0, 2);
            Assert.AreEqual(2, Game.GetPlayerStrikes(0));
        }
    }
}
