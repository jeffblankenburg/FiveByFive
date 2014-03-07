﻿using System;
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
            Assert.AreEqual(1, Game.RollIndex);
        }

        [TestMethod]
        public void FourthRollShouldNotBePermittedAndDiceValuesShouldThereforeNotChange()
        {
            SimulateThisManyRolls(3);
            int[] OldDice = RecordDice();
            RollResult r = Game.RollDice();
            Assert.AreEqual(OldDice[0], Game.Dice[0].Value);
            Assert.AreEqual(OldDice[1], Game.Dice[1].Value);
            Assert.AreEqual(OldDice[2], Game.Dice[2].Value);
            Assert.AreEqual(OldDice[3], Game.Dice[3].Value);
            Assert.AreEqual(OldDice[4], Game.Dice[4].Value);
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

        public int[] RecordDice()
        {
            int[] OldDice = new int[5];
            OldDice[0] = Game.Dice[0].Value;
            OldDice[1] = Game.Dice[1].Value;
            OldDice[2] = Game.Dice[2].Value;
            OldDice[3] = Game.Dice[3].Value;
            OldDice[4] = Game.Dice[4].Value;
            return OldDice;
        }
    }
}
