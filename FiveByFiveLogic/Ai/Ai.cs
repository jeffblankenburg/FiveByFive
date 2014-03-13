using System.Collections.Generic;

namespace FiveByFiveLogic.Ai
{
    public interface Ai
    {
        IList<RoundSelection> GetMoves(FiveByFiveGame game);
    }
}