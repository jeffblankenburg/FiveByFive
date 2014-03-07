using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveByFiveLogic
{
    public class FiveByFiveGame
    {
        public int PlayerIndex;
        public int RollIndex;
        public List<Player> Players = new List<Player>();
        public List<Die> Dice = new List<Die>(5) { new Die(), new Die(), new Die(), new Die(), new Die() };
        public Board GameBoard = new Board();
        
        
        public FiveByFiveGame()
        {
            PlayerIndex = 1;
            RollIndex = 0;
        }

        public void AddPlayer(Player p)
        {
            p.Position = Players.Count() + 1;
            Players.Add(p);
        }

        public RollResult RollDice()
        {
            RollResult result = new RollResult { DidRoll = true };

            Random r = new Random();
            for (int i = 0; i < 5; i++)
            {
                if (!Dice[i].IsHeld)
                {
                    Dice[i].Value = r.Next(1, 6);
                }
            }

            if (RollIndex == 2)
                result.DidRoll = false;
            else
                RollIndex++;

            ////HIGHLIGHT THE BOXES THAT COULD BE ILLUMINATED.  EVERY ROLL.
            //result = SetAvailableBoxes(result);

            ////CHECK TO SEE IF THIS WAS THE PLAYER'S LAST ROLL.  THIS SHOULD LOCK THE ROLL BUTTON, AND MOVE TO THE NEXT PLAYER.
            if (RollIndex == 2)
            {
                result.IsLastRoll = true;
            }

            return result;
        }

        private RollResult SetAvailableBoxes(RollResult result)
        {
            //int[] Counters = new int[]{0,0,0,0,0};

            //for (int i = 0; i < 5; i++)
            //{
            //    for (int j = 0;j<5;j++)
            //    {
            //        if (Dice[j].Value == (i+1)) Counters[i]++;
            //    }

            //    if (Counters[i] > 0)
            //    {
            //        for (int j = 0; j < Counters[i]; j++)
            //        {
            //            result.Layout.Spaces[j,i] = true;
            //        }
            //    }
            //}



            return result;
        }

        public int GetDieValue(int die)
        {
            //return Dice[die].Value;

            return 0;
        }

        public bool HoldDie(int die)
        {
            //Dice[die].IsHeld = !Dice[die].IsHeld;
            //return Dice[die].IsHeld;        

            return false;
        }

    }
}
