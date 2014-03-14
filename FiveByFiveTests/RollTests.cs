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
        public void InitializeRollTests()
        {
            Game = new FiveByFiveGame();
            Game.AddPlayer(new Player { Name = "Jeff" });
        }
        
        [TestMethod]
        public void OneRollShouldIncreaseRollCount()
        {
            SimulateThisManyRolls(1);
            Assert.AreEqual(0, Game.GetRollIndex());
        }

        [TestMethod]
        public void FourthRollShouldNotBePermittedAndDiceValuesShouldThereforeNotChange()
        {
            SimulateThisManyRolls(3);
            int[] OldDice = RecordDice();
            RollResult r = Game.RollDice();
            Assert.AreEqual(OldDice[0], Game.GetDieValue(0));
            Assert.AreEqual(OldDice[1], Game.GetDieValue(1));
            Assert.AreEqual(OldDice[2], Game.GetDieValue(2));
            Assert.AreEqual(OldDice[3], Game.GetDieValue(3));
            Assert.AreEqual(OldDice[4], Game.GetDieValue(4));
        }

        [TestMethod]
        public void FirstDieIsHeldAndDoesNotChangeValueOnNextRoll()
        {
            SimulateThisManyRolls(1);
            Game.SetDieValue(0, 5);
            Game.HoldDie(0);
            SimulateThisManyRolls(1);
            Assert.AreEqual(5, Game.GetDieValue(0));
        }

        [TestMethod]
        public void FirstDieIsNotHeldAndChangesValueOnNextRoll()
        {
            SimulateThisManyRolls(1);
            Game.SetDieValue(0, 5);
            // force the rolls to return 4's instead of random
            Game.Random = new FakeRandom(4);
            SimulateThisManyRolls(1);
            Assert.AreNotEqual(5, Game.GetDieValue(0));
        }

        public void SimulateThisManyRolls(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Game.RollDice();
            }
        }

        public int[] RecordDice()
        {
            int[] OldDice = new int[5];
            OldDice[0] = Game.GetDieValue(0);
            OldDice[1] = Game.GetDieValue(1);
            OldDice[2] = Game.GetDieValue(2);
            OldDice[3] = Game.GetDieValue(3);
            OldDice[4] = Game.GetDieValue(4);
            return OldDice;
        }
    }

    internal class FakeRandom : Random
    {
        private readonly int _staticValue;

        public FakeRandom(int staticValue)
        {
            _staticValue = staticValue;
        }

        public override int Next(int minValue, int maxValue)
        {
            return Math.Min(maxValue, Math.Max(_staticValue, minValue));
        }
    }
}
