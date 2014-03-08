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
        public void CreateOnePlayerGame()
        {
            CreateGameWithThisManyPlayers(1);
            Assert.AreEqual(1, Game.Players[0].Position);
        }

        [TestMethod]
        public void CreateTwoPlayerGame()
        {
            CreateGameWithThisManyPlayers(2);
            Assert.AreEqual(2, Game.Players[1].Position);
        }

        [TestMethod]
        public void CreateThreePlayerGame()
        {
            CreateGameWithThisManyPlayers(3);
            Assert.AreEqual(3, Game.Players[2].Position);
        }

        [TestMethod]
        public void SelectingAnOpenBoardPositionReturnsTrue()
        {
            CreateGameWithThisManyPlayers(1);
            bool success = Game.GameBoard.AssignSpace(1, 0, 0);
            Assert.AreEqual(true, success);
        }

        [TestMethod]
        public void SelectingABoardPositionThatIsAlreadyTakenReturnsFalse()
        {
            CreateGameWithThisManyPlayers(1);
            Game.GameBoard.AssignSpace(1, 0, 0);
            bool success = Game.GameBoard.AssignSpace(1, 0, 0);
            Assert.AreEqual(false, success);
        }

        [TestMethod]
        public void PlayerMarksFirstThreeSpots()
        {
            CreateGameWithThisManyPlayers(1);
            Game.GameBoard.AssignSpace(1, 0, 0);
            Game.GameBoard.AssignSpace(1, 0, 1);
            Game.GameBoard.AssignSpace(1, 0, 2);
            Assert.AreEqual(1, Game.GameBoard.Spaces[0, 0]);
        }

        [TestMethod]
        public void BoardShouldHighlightAllFive5SpacesWhenThereAreFive5sRolled()
        {
            CreateGameWithThisManyPlayers(1);
            Game.Dice[0].Value = 5;
            Game.Dice[1].Value = 5;
            Game.Dice[2].Value = 5;
            Game.Dice[3].Value = 5;
            Game.Dice[4].Value = 5;
            Game.UpdateBoard();
            Assert.AreEqual(0, Game.GameBoard.Spaces[0, 4]);
            Assert.AreEqual(0, Game.GameBoard.Spaces[1, 4]);
            Assert.AreEqual(0, Game.GameBoard.Spaces[2, 4]);
            Assert.AreEqual(0, Game.GameBoard.Spaces[3, 4]);
            Assert.AreEqual(0, Game.GameBoard.Spaces[4, 4]);
        }

        [TestMethod]
        public void PlayerShouldHaveTwoStrikesWhenNotUsingTwoOfTheirDice()
        {
            CreateGameWithThisManyPlayers(1);
            Game.GameBoard.Spaces[2, 0] = 100;
            Game.EndTurn();
            Assert.AreEqual(2, Game.GetPlayerStrikes(0));
        }

        [TestMethod]
        public void PlayerShouldHaveThreeStrikesWhenNotUsingThreeOfTheirDice()
        {
            CreateGameWithThisManyPlayers(1);
            Game.GameBoard.Spaces[0, 0] = 100;
            Game.GameBoard.Spaces[0, 4] = 100;
            Game.EndTurn();
            Assert.AreEqual(3, Game.GetPlayerStrikes(0));
        }

        [TestMethod]
        public void PlayerShouldHaveOneStrikeWhenNotUsingOneDie()
        {
            CreateGameWithThisManyPlayers(1);
            Game.GameBoard.Spaces[3, 2] = 100;
            Game.EndTurn();
            Assert.AreEqual(1, Game.GetPlayerStrikes(0));
        }

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
