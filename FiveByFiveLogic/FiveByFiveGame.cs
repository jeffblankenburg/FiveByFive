using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveByFiveLogic
{
    public class FiveByFiveGame
    {
        private int PlayerIndex;
        private int RollIndex;
        private List<Player> Players = new List<Player>();
        private List<Die> Dice = new List<Die>(5) { new Die(), new Die(), new Die(), new Die(), new Die() };
        private Board GameBoard = new Board();
        
        internal Random Random { get; set; }
        
        public FiveByFiveGame()
        {
            PlayerIndex = 0;
            RollIndex = -1;

            Random = new Random();
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

                for (int i = 0; i < 5; i++)
                {
                    if (!Dice[i].IsHeld)
                    {
                        Dice[i].Value = Random.Next(1, 6);
                    }
                }
                RollIndex++;
            }
            return result;
        }

        public int[] CountDice()
        {
            int[] Counters = new int[] { 0, 0, 0, 0, 0 };
            //MAKE SURE THE DICE HAVE ACTUALLY BEEN ROLLED BEFORE DOING THIS.
            if (GetDieValue(0) > 0 && GetDieValue(1) > 0 && GetDieValue(2) > 0 && GetDieValue(3) > 0 && GetDieValue(4) > 0)
            {
                for (int x = 0; x <= 4; x++)
                {
                    int y = GetDieValue(x) - 1;
                    Counters[y]++;
                }
            }
            return Counters;
        }

        public int[] CountHeldDice()
        {
            int[] Counters = new int[] { 0, 0, 0, 0, 0 };
            //MAKE SURE THE DICE HAVE ACTUALLY BEEN ROLLED BEFORE DOING THIS.
            if (GetDieValue(0) > 0 && GetDieValue(1) > 0 && GetDieValue(2) > 0 && GetDieValue(3) > 0 && GetDieValue(4) > 0)
            {
                for (int x = 0; x <= 4; x++)
                {
                    if (IsDieHeld(x))
                    {
                        int y = GetDieValue(x) - 1;
                        Counters[y]++;
                    }
                }
            }
            return Counters;
        }

        public void UpdateBoard()
        {
            int[] Counters = CountHeldDice();
            
            //RESET ALL HIGHLIGHTED SPACES.
            for (int x = 0; x <= 4; x++)
            {
                for (int y = 0; y <= 4; y++)
                {
                    if ((GameBoard.Spaces[x, y] == 0) || (GameBoard.Spaces[x, y] == 100))
                    {
                        GameBoard.Spaces[x, y] = -1;
                    }
                }
                //THIS CURRENTLY HIGHLIGHTS THE APPROPRIATE BOX FOR THE DICE YOU HAVE HELD.
                if ((Counters[x]) > 0)
                {
                    if (GetBoardSpaceValue(x, Counters[x] - 1) == -1)
                    {
                        MarkBoard(x, Counters[x] - 1, true, false);
                    }
                }

                //TODO: ALSO VERIFY THAT THE BOARD SELECTIONS THEY MAKE ACTUALLY HOLD THE DICE TOO.
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

        public bool IsDieHeld(int die)
        {
            return Dice[die].IsHeld;
        }

        public bool HoldDie(int die)
        {
            if (Dice[die].IsHeld == true) Dice[die].IsHeld = false;
            else if (Dice[die].IsHeld == false) Dice[die].IsHeld = true;
            UpdateBoard();
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

        public void MarkBoard(int x, int y, bool state, bool shouldsync = true)
        {
            //WE MARK THE SPACES AS "100" WHEN THE USER SELECTS THEM.  WE CHANGE THEM TO THEIR POSITION WHEN THEY COMMIT THE CHOICE.
            if (state == true)
                GameBoard.Spaces[x, y] = 100;
            else if ((state == false) && (GameBoard.Spaces[x, y] == 100))
                GameBoard.Spaces[x, y] = 0;
            if (shouldsync) UpdateBoard();
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

        public bool IsOver()
        {
            // if > 1 players, only one player left
            // if 1 player, when you run out of strikes
            return (Players.Count > 1 && Players.Count(x => x.Strikes < 5) <= 1) 
                || (Players.Count == 1 && Players[0].Strikes >= 5);
        }
    }
}
