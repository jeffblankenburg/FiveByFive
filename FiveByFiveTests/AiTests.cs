using System.Linq;
using FiveByFiveLogic;
using FiveByFiveLogic.Ai;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FiveByFiveTests
{
    [TestClass]
    public class AiTests
    {
        private FiveByFiveGame _game;
        private Player _player;
        private Ai _ai;

        [TestInitialize]
        public void Setup()
        {
            _game = new FiveByFiveGame();
            _player = new Player {IsHumanPlayer = false, Strikes = 0};
            
            _game.AddPlayer(_player);

            _ai = new EasyAi();
        }

        [TestMethod]
        public void OnEasyScenarioOne()
        {
            // 2x2 x'ed
            _game.AssignBoardSpace(0, 2, 2);

            // 2, 2, 3, 3, 3 up; 0 rolls left
            _game.SetDice(2, 2, 3, 3, 3);
            
            var selections = _ai.GetMoves(_game);

            //  => 1x2, 3x3, 1xX
            Assert.AreEqual(selections.Count(), 2);
            Assert.IsTrue(selections.Any(x => x.X == 2 & x.Y == 1), "Has 2x1");
            Assert.IsTrue(selections.Any(x => x.X == 3 & x.Y == 3), "Has 3x3");
            Assert.AreEqual(4, selections.Sum(x => x.Y), "Only four dice used");
        }
    }
}