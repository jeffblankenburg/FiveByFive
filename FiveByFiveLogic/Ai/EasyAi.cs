using System.Collections.Generic;
using System.Linq;

namespace FiveByFiveLogic.Ai
{
    public class EasyAi : Ai {
        public IList<RoundSelection> GetMoves(FiveByFiveGame game)
        {
            var results = new List<RoundSelection>();
            var dice = game.GetDice();
            foreach (var num in new[] {1, 2, 3, 4, 5})
            {
                var count = dice.Count(x => x == num);
                if (count == 0)
                    continue;

                var nextProposedMove = new RoundSelection {X = num, Y = count};
                while (!nextProposedMove.IsValidMoveIn(game) && nextProposedMove.Y > 0)
                    nextProposedMove.Y--;

                if (nextProposedMove.Y > 0)
                    results.Add(nextProposedMove);
            }

            return results;
        }
    }
}