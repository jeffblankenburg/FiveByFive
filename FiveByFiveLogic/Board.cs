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
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Spaces[i,j] = -1;
                }
            }
        }

        public bool AssignSpace(int position, int x, int y)
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
