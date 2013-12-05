using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveByFiveLogic
{
    public class FiveByFiveGame
    {
        int PlayerIndex = 0;
        public List<Player> Players = new List<Player>();
        List<Die> Dice = new List<Die>(5) { new Die(), new Die(), new Die(), new Die(), new Die() };
        public Board GameBoard = new Board();
        
        
        public FiveByFiveGame()
        {
            

        }

        public void AddPlayer(Player p)
        {
            Players.Add(p);
        }

        public RollResult RollDice()
        {
            RollResult result = new RollResult();
            result.Player = Players[PlayerIndex];
            Random r = new Random();
            
            for (int i = 0; i < 5; i++)
            {
                if (!Dice[i].IsHeld)
                {
                    Dice[i].Value = r.Next(1, 6);
                }
            }

            Players[PlayerIndex].Rolls++;

            //HIGHLIGHT THE BOXES THAT COULD BE ILLUMINATED.  EVERY ROLL.
            result = SetAvailableBoxes(result);

            //CHECK TO SEE IF THIS WAS THE PLAYER'S LAST ROLL.  THIS SHOULD LOCK THE ROLL BUTTON, AND MOVE TO THE NEXT PLAYER.
            if (Players[PlayerIndex].Rolls == 3)
            {
                Players[PlayerIndex].Rolls = 0;
                result.IsLastRoll = true;
                PlayerIndex++;

                if ((PlayerIndex+1) > Players.Count())
                {
                    PlayerIndex = 0;
                }
            }

            return result;
        }

        private RollResult SetAvailableBoxes(RollResult result)
        {
            int[] Counters = new int[]{0,0,0,0,0};

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0;j<5;j++)
                {
                    if (Dice[j].Value == (i+1)) Counters[i]++;
                }

                if (Counters[i] > 0)
                {
                    for (int j = 0; j < Counters[i]; j++)
                    {
                        result.Layout.Spaces[j,i] = true;
                    }
                }
            }



            return result;
        }

        public int GetDieValue(int die)
        {
            return Dice[die].Value;
        }

        public bool HoldDie(int die)
        {
            Dice[die].IsHeld = !Dice[die].IsHeld;
            return Dice[die].IsHeld;        
        }

    }
}
