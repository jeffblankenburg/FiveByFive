using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FiveByFiveLogic;

namespace FiveByFiveTests
{
    [TestClass]
    public class Tests
    {
        FiveByFiveGame Game;
        
        public void TwoHumanPlayerSetup()
        {
            Game = new FiveByFiveGame();
            Player p = new Player{ IsHumanPlayer=true, Name = "Jeff", Rolls=0, Strikes = 0};
            Player q = new Player { IsHumanPlayer = true, Name = "Travis", Rolls = 0, Strikes = 0 };
            Game.AddPlayer(p);
            Game.AddPlayer(q);
        }

        [TestMethod]
        public void PlayerOneTakesFirstRoll()
        {
            TwoHumanPlayerSetup();
            Game.RollDice();
            Assert.AreEqual(1, Game.Players[0].Rolls);
        }

        [TestMethod]
        public void FiveRollsShouldEndUpOnTheSecondRollForSecondPlayer()
        {
            TwoHumanPlayerSetup();
            Game.RollDice();
            Game.RollDice();
            Game.RollDice();
            Game.RollDice();
            Game.RollDice();
            Assert.AreEqual(2, Game.Players[1].Rolls);
        }

        [TestMethod]
        public void SevenRollsShouldEndUpOnTheFirstRollForFirstPlayer()
        {
            TwoHumanPlayerSetup();
            Game.RollDice();
            Game.RollDice();
            Game.RollDice();
            Game.RollDice();
            Game.RollDice();
            Game.RollDice();
            Game.RollDice();
            Assert.AreEqual(1, Game.Players[0].Rolls);
        }
    }
}
