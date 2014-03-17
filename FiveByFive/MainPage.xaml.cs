using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
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
            //DataContext = Game;
            //GridRotation.Begin();
        }

        private void AddPlayersToGame(string q)
        {
            string[] split = q.Split(new char[] { ',' });
            for (int i = 0; i < split.Length-1; i++)
            {
                Game.AddPlayer(new Player { Name = split[i], IsHumanPlayer = Convert.ToBoolean(split[i + 1]) });
                i++;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("q"))
            {
                if (Game.GetPlayerCount() == 0)
                { 
                    string q = NavigationContext.QueryString["q"].ToString();
                    AddPlayersToGame(q);
                }
            }
            
            BlueBrush = new SolidColorBrush(Colors.Blue);
            HeldBrush = new SolidColorBrush(HeldColor);
            ClearBrush = new SolidColorBrush(Colors.Transparent);
            UpdateBoard();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {

        }

        private void Number_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //if (Game.GetRollIndex() == 2)
            //{
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
            //}
        }

        private void RollButton_Click(object sender, RoutedEventArgs e)
        {
            RollResult r = Game.RollDice();
            
            if (r.DidRoll == false)
            {
                EndTurn();
            }
            UpdateBoard();
        }

        private void EndTurn()
        {
            //MessageBox.Show("There should definitely be a check here to make sure they want to confirm their move.");
            Game.EndTurn();
            if (Game.IsOver())
            {
                MessageBox.Show("Game Over!");
                NavigationService.Navigate(new Uri("/StartScreen.xaml", UriKind.Relative));
            }
            else
            {
                ResetDice();
                UpdateBoard();
            }
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
            SetButtonContent();

            //SHOW NEW DICE VALUES
            if (Game.GetRollIndex() != -1)
            { 
                Dice0.Text = Game.GetDieValue(0).ToString();
                Dice1.Text = Game.GetDieValue(1).ToString();
                Dice2.Text = Game.GetDieValue(2).ToString();
                Dice3.Text = Game.GetDieValue(3).ToString();
                Dice4.Text = Game.GetDieValue(4).ToString();
            }

            //UPDATE UI TO SHOW AVAILABLE BOXES TO SELECT
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    Rectangle r = FindName("Tap" + (x + 1) + "" + (y + 1)) as Rectangle;
                    TextBlock t = FindName("X" + (x + 1) + "" + (y + 1)) as TextBlock;
                    if (Game.GetBoardSpaceValue(x, y) > 0)
                    {
                        t.Foreground = GetPlayerSolidColorBrush(Game.GetBoardSpaceValue(x, y));
                        t.Visibility = Visibility.Visible;
                        r.Fill = ClearBrush;
                    }
                    else if (Game.GetBoardSpaceValue(x, y) == 0)
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
            StrikeText.Text = Game.GetPlayerStrikes().ToString() + " strikes";

            //UPDATE PLAYER NAME
            PlayerText.Text = Game.GetPlayerName();

            //UPDATE SCOREBOARD
            UpdateScoreboard();
        }

        private void UpdateScoreboard()
        {
            Scoreboard.Children.Clear();
            Scoreboard.ColumnDefinitions.Clear();
            
            for (int i = 0; i < Game.GetPlayerCount(); i++)
            {
                Scoreboard.ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(1, GridUnitType.Star)});
                
                Rectangle r = new Rectangle();
                r.Fill = GetPlayerSolidColorBrush(i+1);
                Grid.SetColumn(r, i);
                Scoreboard.Children.Add(r);

                StackPanel s = new StackPanel {Orientation = System.Windows.Controls.Orientation.Vertical};
                    TextBlock t = new TextBlock { TextAlignment=TextAlignment.Center, VerticalAlignment=VerticalAlignment.Center };
                    t.Text = Game.GetPlayerName(i);
                    t.Margin = new Thickness(0, 0, 0, 7);
                    s.Children.Add(t);

                    Grid boxes = new Grid { HorizontalAlignment=HorizontalAlignment.Center };
                    for (int j = 0; j<=4; j++)
                    {
                        boxes.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(25) });
                        TextBlock box = new TextBlock { Text="c", FontFamily=new FontFamily("Webdings") };
                        Grid.SetColumn(box, j);
                        boxes.Children.Add(box);
                        if (j < Game.GetPlayerStrikes(i))
                        {
                            TextBlock x = new TextBlock { Text = "x", FontFamily = new FontFamily("Arial"), FontSize = 28, Margin = new Thickness(3, -9, 0, 0) };
                            Grid.SetColumn(x, j);
                            boxes.Children.Add(x);
                        }
                    }
                    s.Children.Add(boxes);

                Grid.SetColumn(s, i);
                Scoreboard.Children.Add(s);
            }
        }

        private void SetButtonContent()
        {
            switch (Game.GetRollIndex())
            {
                case -1:
                    EndTurnButton.IsEnabled = false;
                    RollButton.IsEnabled = true;
                    RollButton.Content = "Roll";
                    break;
                case 0:
                    EndTurnButton.IsEnabled = false;
                    RollButton.IsEnabled = true;
                    RollButton.Content = "Roll 2";
                    break;
                case 1:
                    EndTurnButton.IsEnabled = false;
                    RollButton.IsEnabled = true;
                    RollButton.Content = "Roll 3";
                    break;
                case 2:
                    EndTurnButton.IsEnabled = true;
                    RollButton.IsEnabled = false;
                    RollButton.Content = "Save Choices";
                    break;
            }
            //RollButton.Content = "Roll " + (Game.GetRollIndex() + 1).ToString();
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
            if (Game.GetDieValue(die) > 0)
            { 
                if (Game.HoldDie(die))
                {
                    r.Fill = HeldBrush;
                }
                else r.Fill = ClearBrush;
            }
            UpdateBoard();
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

        private void EndTurnButton_Click(object sender, RoutedEventArgs e)
        {
            EndTurn();
        }
    }
}