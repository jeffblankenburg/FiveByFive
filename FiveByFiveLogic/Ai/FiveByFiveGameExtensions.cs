namespace FiveByFiveLogic.Ai
{
    public static class FiveByFiveGameExtensions
    {
        public static void SetDice(this FiveByFiveGame game, int die0, int die1, int die2, int die3, int die4)
        {
            game.SetDieValue(0, die0);
            game.SetDieValue(1, die1);
            game.SetDieValue(2, die2);
            game.SetDieValue(3, die3);
            game.SetDieValue(4, die4);
        }

        public static int[] GetDice(this FiveByFiveGame game)
        {
            return new[]
            {
                game.GetDieValue(0),
                game.GetDieValue(1),
                game.GetDieValue(2),
                game.GetDieValue(3),
                game.GetDieValue(4)                
            };
        }
    }
}