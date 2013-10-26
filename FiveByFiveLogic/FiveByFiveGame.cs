using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveByFiveLogic
{
    public class FiveByFiveGame
    {
        List<Player> Players = new List<Player>();
        List<Die> Dice = new List<Die>(5) { new Die(), new Die(), new Die(), new Die(), new Die() };
        bool[,] Board = new Boolean[5, 5] { { false, false, false, false, false }, { false, false, false, false, false }, { false, false, false, false, false }, { false, false, false, false, false }, { false, false, false, false, false } };
        
        
        public FiveByFiveGame()
        {

        }

        public bool RollDice()
        {
            Random r = new Random();
            
            for (int i = 0; i < 5; i++)
            {
                if (!Dice[i].IsHeld)
                {
                    Dice[i].Value = r.Next(1, 6);
                }
            }

            return true;
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
