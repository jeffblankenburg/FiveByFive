using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveByFiveLogic
{
    public class RollResult
    {
        public bool IsLastRoll { get; set; }
        public bool DidRoll { get; set; }
        public Board Layout { get; set; }
        public Player Player { get; set; }

        public RollResult()
        {
            IsLastRoll = false;
            Layout = new Board();
        }

        public RollResult(Player p)
        {
            IsLastRoll = false;
            Layout = new Board();
            Player = p;
        }
    }
}
