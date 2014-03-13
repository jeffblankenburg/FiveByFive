namespace FiveByFiveLogic.Ai
{
    public class RoundSelection
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool IsValidMoveIn(FiveByFiveGame game)
        {
            return !game.CheckPositionForMarking(X, Y);
        }
    }
}