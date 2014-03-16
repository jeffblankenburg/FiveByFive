using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

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
            
            Counters[0] = Counters[0] + Dice.Count(d => d.Value == 1);
            Counters[1] = Counters[1] + Dice.Count(d => d.Value == 2);
            Counters[2] = Counters[2] + Dice.Count(d => d.Value == 3);
            Counters[3] = Counters[3] + Dice.Count(d => d.Value == 4);
            Counters[4] = Counters[4] + Dice.Count(d => d.Value == 5);

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

        public bool IsOver()
        {
            // if > 1 players, only one player left
            // if 1 player, when you run out of strikes
            return (Players.Count > 1 && Players.Count(x => x.Strikes < 5) <= 1) 
                || (Players.Count == 1 && Players[0].Strikes >= 5);
        }

        public int ScoreBoard()
        {
            var score = 0;
            // I feel like I should be able to LINQ across a 2d array, but I don't think I can :(
            for (var x = 0; x < GameBoard.Spaces.GetLength(0); x++)
                for (var y = 0; y < GameBoard.Spaces.GetLength(1); y++)
                    if (GameBoard.Spaces[x, y] == 1)
                        score += (y + 1) * (y + 1);
            
            return score;
        }

        public int GetSelectedSpots()
        {
            return GameBoard.Spaces.ToDictionary().Count(x => x.Value == 100);
        }
    }

    public static class Extensions
    {
        public static IDictionary<Tuple<int, int>, int> ToDictionary(this int[,] data)
        {
            var dict = new Dictionary<Tuple<int, int>, int>();
            for (var x = 0; x < data.GetLength(0); x++)
                for (var y = 0; y < data.GetLength(1); y++)
                    dict.Add(new Tuple<int, int>(x, y), data[x,y]);
            return dict;
        }
    }
}
