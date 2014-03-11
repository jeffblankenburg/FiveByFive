using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveByFiveLogic
{
    public class Board
    {
        public int[,] Spaces = new int[5, 5];

        public Board()
        {
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    Spaces[x, y] = -1;
                }
            }
        }

        internal bool AssignSpace(int position, int x, int y)
        {
            if (Spaces[x,y] == -1)
            {
                Spaces[x, y] = position;
                return true;
            }

            return false;
        }
    }
}
