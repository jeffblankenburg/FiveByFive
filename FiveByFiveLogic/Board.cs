using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveByFiveLogic
{
    public class Board
    {
        public bool[,] Spaces = new bool[5, 5];

        public Board()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Spaces[i,j] = false;
                }
            }
        }
    }
}
