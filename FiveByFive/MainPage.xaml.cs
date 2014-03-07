using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using FiveByFive.Resources;
using System.Windows.Shapes;
using FiveByFiveLogic;
using System.Windows.Media;

namespace FiveByFive
{
    public partial class MainPage : PhoneApplicationPage
    {
        FiveByFiveGame Game = new FiveByFiveGame();
        Color HeldColor = Color.FromArgb(0x99, 0xFF, 0xFF, 0xFF);
        SolidColorBrush HeldBrush;
        SolidColorBrush BlueBrush;
        SolidColorBrush ClearBrush;
        
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            DataContext = Game;
            //GridRotation.Begin();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BlueBrush = new SolidColorBrush(Colors.Blue);
            HeldBrush = new SolidColorBrush(HeldColor);
            ClearBrush = new SolidColorBrush(Colors.Transparent);
            Game.AddPlayer(new Player { Name="Jeff", IsHumanPlayer=true });
            Game.AddPlayer(new Player { Name = "Travis", IsHumanPlayer = true });
        }

        private void Number_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (Game.GetRollIndex() == 2)
            {
                Rectangle r = sender as Rectangle;
                string n = r.Name;
                int i = Int32.Parse(n.Replace("Tap", ""));
                int x = Int32.Parse((i.ToString()).Substring(0, 1)) - 1;
                int y = Int32.Parse((i.ToString()).Substring(1, 1)) - 1;

                if (Game.CheckPositionForMarking(x, y))
                {
                    Game.MarkBoard(x, y, true);
                    UpdateBoard();
                }
            }
        }

        private void RollButton_Click(object sender, RoutedEventArgs e)
        {
            RollResult r = Game.RollDice();
            
            if (r.DidRoll == false)
            {
                MessageBox.Show("There should definitely be a check here to make sure they want to confirm their move.");
                Game.EndTurn();
                ResetDice();
            }

            UpdateBoard();
        }

        private void ResetDice()
        {
            Dice0.Text = String.Empty;
            Dice1.Text = String.Empty;
            Dice2.Text = String.Empty;
            Dice3.Text = String.Empty;
            Dice4.Text = String.Empty;
            Die0.Fill = ClearBrush;
            Die1.Fill = ClearBrush;
            Die2.Fill = ClearBrush;
            Die3.Fill = ClearBrush;
            Die4.Fill = ClearBrush;
        }

        private void UpdateBoard()
        {
            Game.UpdateBoard();
            
            //SHOW ROLL COUNT
            RollButton.Content = "Roll " + (Game.RollIndex + 1).ToString();

            //SHOW NEW DICE VALUES
            if (Game.RollIndex != -1)
            { 
                Dice0.Text = Game.GetDieValue(0).ToString();
                Dice1.Text = Game.GetDieValue(1).ToString();
                Dice2.Text = Game.GetDieValue(2).ToString();
                Dice3.Text = Game.GetDieValue(3).ToString();
                Dice4.Text = Game.GetDieValue(4).ToString();
            }

            //UPDATE UI TO SHOW AVAILABLE BOXES TO SELECT
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Rectangle r = FindName("Tap" + (i + 1) + "" + (j + 1)) as Rectangle;
                    TextBlock t = FindName("X" + (i + 1) + "" + (j + 1)) as TextBlock;
                    if (Game.GetBoardSpaceValue(i, j) > 0)
                    {
                        t.Foreground = GetPlayerSolidColorBrush(Game.GetBoardSpaceValue(i, j));
                        t.Visibility = Visibility.Visible;
                        r.Fill = ClearBrush;
                    }
                    else if (Game.GetBoardSpaceValue(i, j) == 0)
                    {
                        t.Visibility = Visibility.Collapsed;
                        r.Fill = HeldBrush;
                    }
                    else
                    {
                        t.Visibility = Visibility.Collapsed;
                        r.Fill = ClearBrush;
                    }
                }
            }

            //UPDATE PLAYER BAR
            PlayerBar.Fill = GetPlayerSolidColorBrush(Game.GetPlayerIndex() + 1);

            //UPDATE STRIKES
            StrikeText.Text = Game.GetCurrentStrikes().ToString() + " strikes";

            //UPDATE PLAYER NAME
            PlayerText.Text = Game.GetCurrentPlayerName();
        }

        private SolidColorBrush GetPlayerSolidColorBrush(int i)
        {
            switch (i)
            {
                case 1:
                    return new SolidColorBrush(Colors.Red);
                case 2:
                    return new SolidColorBrush(Colors.Blue);
                case 3:
                    return new SolidColorBrush(Colors.Green);
                default:
                    return GetPlayerSolidColorBrush(Game.GetPlayerIndex() + 1);
            }
        }

        private void Die_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Rectangle r = sender as Rectangle;
            int die = Int32.Parse(r.Name.Replace("Die", ""));
            if (Game.HoldDie(die))
            {
                r.Fill = HeldBrush;
            }
            else r.Fill = ClearBrush;
        }

        private void X_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock t = sender as TextBlock;
            string n = t.Name;
            int i = Int32.Parse(n.Replace("X", ""));
            int x = Int32.Parse((i.ToString()).Substring(0, 1)) - 1;
            int y = Int32.Parse((i.ToString()).Substring(1, 1)) - 1;

            Game.MarkBoard(x, y, false);
            UpdateBoard();
        }
    }
}