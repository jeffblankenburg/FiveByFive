using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FiveByFiveLogic;

namespace FiveByFiveTests
{
    [TestClass]
    public class RollTests
    {
        public FiveByFiveGame Game;

        [TestInitialize]
        public void InitializePlayerTests()
        {
            Game = new FiveByFiveGame();
            Game.AddPlayer(new Player { Name = "Jeff" });
        }
        
        [TestMethod]
        public void OneRollShouldIncreaseRollCount()
        {
            SimulateThisManyRolls(1);
            Assert.AreEqual(1, Game.RollIndex);
        }

        [TestMethod]
        public void ThreeRollsShouldIndicateItIsTheLastRoll()
        {
            SimulateThisManyRolls(3);
            RollResult r = Game.RollDice();
            Assert.AreEqual(true, r.IsLastRoll);
        }

        [TestMethod]
        public void FourthRollShouldNotBePermitted()
        {
            SimulateThisManyRolls(4);
            RollResult r = Game.RollDice();
            Assert.AreEqual(false, r.DidRoll);
        }

        [TestMethod]
        public void FirstDieIsHeldAndDoesNotChangeValueOnNextRoll()
        {
            SimulateThisManyRolls(1);
            Game.Dice[0] = new Die(5, true);
            SimulateThisManyRolls(1);
            Assert.AreEqual(5, Game.Dice[0].Value);
        }

        [TestMethod]
        public void FirstDieIsNOTHeldAndChangesValueOnNextRoll()
        {
            SimulateThisManyRolls(1);
            Game.Dice[0] = new Die(5, false);
            SimulateThisManyRolls(1);
            Assert.AreNotEqual(5, Game.Dice[0].Value);
        }

        public void SimulateThisManyRolls(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Game.RollDice();
            }
        }
    }
}
