using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveByFiveLogic
{
    public class Player
    {
        public int Position { get; set; }
        public string Name { get; set; }
        public bool IsHumanPlayer { get; set; }
        public int Strikes { get; set; }

        public Player()
        {
            Strikes = 0;
        }
        
        public bool AddStrikes(int count)
        {
            Strikes = Strikes + count;
            if (Strikes >= 5) return false;
            return true;
        }
    }
}
