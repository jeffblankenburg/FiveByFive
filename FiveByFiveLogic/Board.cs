using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveByFiveLogic
{
    public class Board
    {
        //  -1 IS THE DEFAULT STATE.
        //  0 IS A SPACE THAT SHOULD BE HIGHLIGHTED.
        //  1, 2, OR 3 ARE PLAYER VALUES THAT INDICATE IT HAS BEEN MARKED BY A SPECIFIC PLAYER.
        //  100 MEANS THAT A PLAYER HAS MARKED THE SPACE, BUT HAS NOT YET FINISHED THEIR TURN TO COMMIT THAT CHOICE.
        
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
