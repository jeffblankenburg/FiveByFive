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
        private List<Die> Dice = new List<Die>(5) { new Die(), new Die(), new Die(), new Die(), new Die() };
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
            int x;
            int y;
            
            //RESET ALL HIGHLIGHTED SPACES.
            for (x = 0; x <= 4; x++)
            {
                for (y = 0; y <= 4; y++)
                {
                    if (GameBoard.Spaces[x, y] == 0)
                        GameBoard.Spaces[x, y] = -1;

                    if (GameBoard.Spaces[x, y] == 100)
                        Counters[x] = Counters[x] - (y+1);
                }
            }
            
            //COUNT THE NUMBER OF EACH TYPE OF DICE.
            
            Counters[0] = Counters[0] + Dice.Where(d => d.Value == 1).Count();
            Counters[1] = Counters[1] + Dice.Where(d => d.Value == 2).Count();
            Counters[2] = Counters[2] + Dice.Where(d => d.Value == 3).Count();
            Counters[3] = Counters[3] + Dice.Where(d => d.Value == 4).Count();
            Counters[4] = Counters[4] + Dice.Where(d => d.Value == 5).Count();

            //MARK THE SPACES THAT COULD STILL BE CHECKED.
            for (x = 0; x <= 4; x++)
            {
                if (Counters[x] > 0)
                {
                    for (y = Counters[x] - 1; y >= 0; y--)
                    {
                        if (GameBoard.Spaces[x, y] == -1)
                            GameBoard.Spaces[x, y] = 0;
                    }
                }
                else
                {
                    for (y = 0; y <= 4; y++)
                    {
                        if (GameBoard.Spaces[x, y] == 0)
                            GameBoard.Spaces[x, y] = -1;
                    }
                }
            }
        }

        public void EndTurn()
        {
            int UsedDiceCount = 0;
            
            for (int x = 0; x<=4; x++)
            {
                for (int y = 0; y<=4; y++)
                {
                    if (GameBoard.Spaces[x, y] == 100)
                    {
                        GameBoard.Spaces[x, y] = Players[PlayerIndex].Position;
                        UsedDiceCount = UsedDiceCount + (y + 1);
                    }
                }
                HoldDie(x);
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

        public void SetDieValue(int index, int value)
        {
            Dice[index].Value = value;
        }
    }
}
