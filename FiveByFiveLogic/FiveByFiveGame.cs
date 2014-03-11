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
        private List<Player> Players = new List<Player>();
        public List<Die> Dice = new List<Die>(5) { new Die(), new Die(), new Die(), new Die(), new Die() };
        private Board GameBoard = new Board();
        
        
        public FiveByFiveGame()
        {
            PlayerIndex = 0;
            RollIndex = -1;
        }

        public void AddPlayer(Player p)
        {
            p.Position = Players.Count() + 1;
            Players.Add(p);
        }

        public RollResult RollDice()
        {
            RollResult result = new RollResult { DidRoll = false };

            if (RollIndex < 2)
            {
                result = new RollResult { DidRoll = true };

                Random r = new Random();
                for (int i = 0; i < 5; i++)
                {
                    if (!Dice[i].IsHeld)
                    {
                        Dice[i].Value = r.Next(1, 6);
                    }
                }
                RollIndex++;
            }
            return result;
        }

        public void UpdateBoard()
        {
            int[] Counters = new int[] {0,0,0,0,0};
            
            //RESET ALL HIGHLIGHTED SPACES.
            for (int k = 0; k <= 4; k++)
            {
                for (int l = 0; l <= 4; l++)
                {
                    if (GameBoard.Spaces[l, k] == 0)
                        GameBoard.Spaces[l, k] = -1;

                    if (GameBoard.Spaces[l, k] == 100)
                        Counters[l] = Counters[l] - (k+1);
                }
            }
            
            //COUNT THE NUMBER OF EACH TYPE OF DICE.
            
            Counters[0] = Counters[0] + Dice.Where(x => x.Value == 1).Count();
            Counters[1] = Counters[1] + Dice.Where(x => x.Value == 2).Count();
            Counters[2] = Counters[2] + Dice.Where(x => x.Value == 3).Count();
            Counters[3] = Counters[3] + Dice.Where(x => x.Value == 4).Count();
            Counters[4] = Counters[4] + Dice.Where(x => x.Value == 5).Count();

            //MARK THE SPACES THAT COULD STILL BE CHECKED.
            for (int i = 0; i <= 4; i++)
            {
                if (Counters[i] > 0)
                {
                    for (int j = Counters[i] - 1; j >= 0; j--)
                    {
                        if (GameBoard.Spaces[i, j] == -1)
                            GameBoard.Spaces[i, j] = 0;
                    }
                }
                else
                {
                    for (int p = 0; p <= 4; p++)
                    {
                        if (GameBoard.Spaces[i, p] == 0)
                            GameBoard.Spaces[i, p] = -1;
                    }
                }
            }
        }

        public void EndTurn()
        {
            int UsedDiceCount = 0;
            
            for (int i = 0; i<=4;i++)
            {
                for (int j = 0; j<=4; j++)
                {
                    if (GameBoard.Spaces[i, j] == 100)
                    {
                        GameBoard.Spaces[i, j] = Players[PlayerIndex].Position;
                        UsedDiceCount = UsedDiceCount + (i + 1);
                    }
                }
                HoldDie(i);
            }

            //THIS NEEDS TO MAKE SURE THE CURRENT PLAYER IS ASSIGNED A STRIKE FOR EVERY DIE THEY DIDN'T USE.
            Players[PlayerIndex].AddStrikes(5-UsedDiceCount);

            ResetDice();

            PlayerIndex++;
            RollIndex = -1;
            if (PlayerIndex == Players.Count()) PlayerIndex = 0;

            UpdateBoard();
        }

        private void ResetDice()
        {
            Dice = new List<Die>(5) { new Die(), new Die(), new Die(), new Die(), new Die() };
        }

        public int GetDieValue(int die)
        {
            return Dice[die].Value;
        }

        public bool HoldDie(int die)
        {
            if (Dice[die].IsHeld == true) Dice[die].IsHeld = false;
            else if (Dice[die].IsHeld == false) Dice[die].IsHeld = true;
            return Dice[die].IsHeld;        
        }

        public int GetRollIndex()
        {
            return RollIndex;
        }

        public int GetPlayerStrikes()
        {
            return Players[PlayerIndex].Strikes;
        }

        public int GetPlayerStrikes(int player)
        {
            if ((player < Players.Count()) && (player >= 0))
                return Players[player].Strikes;
            else return 0;
        }

        public int GetBoardSpaceValue(int x, int y)
        {
            return GameBoard.Spaces[x, y];
        }

        public string GetPlayerName()
        {
            return Players[PlayerIndex].Name;
        }

        public string GetPlayerName(int player)
        {
            if ((player < Players.Count()) && (player >= 0))
                return Players[player].Name;
            else
                return String.Empty;
        }

        public void MarkBoard(int x, int y, bool state)
        {
            //WE MARK THE SPACES AS "100" WHEN THE USER SELECTS THEM.  WE CHANGE THEM TO THEIR POSITION WHEN THEY COMMIT THE CHOICE.
            if (state == true)
                GameBoard.Spaces[x, y] = 100;
            else if ((state == false) && (GameBoard.Spaces[x, y] == 100))
                GameBoard.Spaces[x, y] = 0;
        }

        public bool CheckPositionForMarking(int x, int y)
        {
            return GameBoard.Spaces[x, y] == 0;
        }

        public int GetPlayerIndex()
        {
            return PlayerIndex;
        }

        public int GetPlayerCount()
        {
            return Players.Count();
        }

        public void AddStrikesToPlayer(int playerIndex, int strikes)
        {
            Players[playerIndex].AddStrikes(strikes);
        }

        public bool AssignBoardSpace(int position, int x, int y)
        {
            return GameBoard.AssignSpace(position, x, y);
        }
    }
}
