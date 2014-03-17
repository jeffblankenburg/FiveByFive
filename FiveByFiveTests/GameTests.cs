using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FiveByFiveLogic;

namespace FiveByFiveTests
{
    [TestClass]
    public class GameTests
    {
        public FiveByFiveGame Game;

        [TestMethod]
        public void SelectingAnOpenBoardPositionReturnsTrue()
        {
            CreateGameWithThisManyPlayers(1);
            bool success = Game.AssignBoardSpace(1, 0, 0);
            Assert.AreEqual(true, success);
        }

        [TestMethod]
        public void SelectingABoardPositionThatIsAlreadyTakenReturnsFalse()
        {
            CreateGameWithThisManyPlayers(1);
            Game.AssignBoardSpace(1, 0, 0);
            bool success = Game.AssignBoardSpace(1, 0, 0);
            Assert.AreEqual(false, success);
        }

        [TestMethod]
        public void PlayerMarksFirstThreeSpots()
        {
            CreateGameWithThisManyPlayers(1);
            Game.AssignBoardSpace(1, 0, 0);
            Game.AssignBoardSpace(1, 0, 1);
            Game.AssignBoardSpace(1, 0, 2);
            Assert.AreEqual(1, Game.GetBoardSpaceValue(0,0));
        }

        //[TestMethod]
        //public void BoardShouldHighlightAllFive5SpacesWhenThereAreFive5sRolled()
        //{
        //    CreateGameWithThisManyPlayers(1);
        //    Game.SetDieValue(0, 5);
        //    Game.SetDieValue(1, 5);
        //    Game.SetDieValue(2, 5);
        //    Game.SetDieValue(3, 5);
        //    Game.SetDieValue(4, 5);
        //    Game.UpdateBoard();

        //    Assert.AreEqual(0, Game.GetBoardSpaceValue(4, 0));
        //    Assert.AreEqual(0, Game.GetBoardSpaceValue(4, 1));
        //    Assert.AreEqual(0, Game.GetBoardSpaceValue(4, 2));
        //    Assert.AreEqual(0, Game.GetBoardSpaceValue(4, 3));
        //    Assert.AreEqual(0, Game.GetBoardSpaceValue(4, 4));
        //}

        [TestMethod]
        public void PlayerShouldHaveFourStrikesWhenNotUsingFourOfTheirDice()
        {
            CreateGameWithThisManyPlayers(1);
            Game.AssignBoardSpace(100, 4, 0);
            Game.EndTurn();
            Assert.AreEqual(4, Game.GetPlayerStrikes(0));
        }

        [TestMethod]
        public void PlayerShouldHaveTwoStrikesWhenNotUsingTwoOfTheirDice()
        {
            CreateGameWithThisManyPlayers(1);
            Game.AssignBoardSpace(100, 0, 2);
            Game.EndTurn();
            Assert.AreEqual(2, Game.GetPlayerStrikes(0));
        }

        [TestMethod]
        public void PlayerShouldHaveThreeStrikesWhenNotUsingThreeOfTheirDice()
        {
            CreateGameWithThisManyPlayers(1);
            Game.AssignBoardSpace(100, 0, 0);
            Game.AssignBoardSpace(100, 4, 0);
            Game.EndTurn();
            Assert.AreEqual(3, Game.GetPlayerStrikes(0));
        }

        [TestMethod]
        public void PlayerShouldHaveOneStrikeWhenNotUsingOneDie()
        {
            CreateGameWithThisManyPlayers(1);
            Game.AssignBoardSpace(100, 2, 3);
            Game.EndTurn();
            Assert.AreEqual(1, Game.GetPlayerStrikes(0));
        }

        [TestMethod]
        public void PlayerShouldHaveZeroStrikesWhenUsingAllOfTheirDice()
        {
            CreateGameWithThisManyPlayers(1);
            Game.AssignBoardSpace(100, 2, 4);
            Game.EndTurn();
            Assert.AreEqual(0, Game.GetPlayerStrikes(0));
        }

        [TestMethod]
        public void HavingThreeDiceWithTheValueOfTwoShouldReturnADiceCountOfThreeForTheTwosPosition()
        {
            CreateGameWithThisManyPlayers(1);
            Game.SetDieValue(0, 1);
            Game.SetDieValue(1, 2);
            Game.SetDieValue(2, 2);
            Game.SetDieValue(3, 4);
            Game.SetDieValue(4, 2);
            int[] Counters = Game.CountDice();
            Assert.AreEqual(3, Counters[1]);
        }

        [TestMethod]
        public void HoldingTwo2sShouldAlsoHighlightTheTwo2sBoxOnTheBoard()
        {
            CreateGameWithThisManyPlayers(1);
            Game.SetDieValue(0, 2);
            Game.SetDieValue(1, 2);
            Game.SetDieValue(2, 2);
            Game.SetDieValue(3, 2);
            Game.SetDieValue(4, 2);
            Game.HoldDie(0);
            Game.HoldDie(4);
            Assert.AreEqual(100, Game.GetBoardSpaceValue(1, 1));
        }

        //[TestMethod]
        //public void HoldingTwo2sShouldAlsoHighlightTheTwo2sBoxOnTheBoard()
        //{
        //    CreateGameWithThisManyPlayers(1);
        //    Game.SetDieValue(0, 2);
        //    Game.SetDieValue(1, 2);
        //    Game.SetDieValue(2, 2);
        //    Game.SetDieValue(3, 2);
        //    Game.SetDieValue(4, 2);
        //    Game.HoldDie(0);
        //    Game.HoldDie(4);
        //    Assert.AreEqual(100, Game.GetBoardSpaceValue(1, 1));
        //}

        public void CreateGameWithThisManyPlayers(int playercount)
        {
            Game = new FiveByFiveGame();
            switch (playercount)
            {
                case 1:
                    Player p = new Player { IsHumanPlayer = true, Name = "Jeff" };
                    Game.AddPlayer(p);
                    break;
                case 2:
                    Player q = new Player { IsHumanPlayer = true, Name = "Jeff" };
                    Game.AddPlayer(q);
                    Player r = new Player { IsHumanPlayer = true, Name = "Travis" };
                    Game.AddPlayer(r);
                    break;
                case 3:
                    Player s = new Player { IsHumanPlayer = true, Name = "Jeff" };
                    Game.AddPlayer(s);
                    Player t = new Player { IsHumanPlayer = true, Name = "Travis" };
                    Game.AddPlayer(t);
                    Player u = new Player { IsHumanPlayer = true, Name = "Steve" };
                    Game.AddPlayer(u);
                    break;
            }
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
