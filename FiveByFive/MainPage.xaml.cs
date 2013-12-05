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
            HeldBrush = new SolidColorBrush(HeldColor);
            ClearBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void Number_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Rectangle r = sender as Rectangle;
            string n = r.Name;
            int i = Int32.Parse(n.Replace("Tap", ""));

            if (r.Fill == HeldBrush)
            {
                TextBlock t = FindName("X" + i) as TextBlock;
                if (t.Visibility == Visibility.Collapsed) t.Visibility = Visibility.Visible;
                else if (t.Visibility == Visibility.Visible) t.Visibility = Visibility.Collapsed;
            }


        }

        private void RollButton_Click(object sender, RoutedEventArgs e)
        {
            
            RollResult result = Game.RollDice();
            UpdateBoard(result);
            //if (result.IsLastRoll)
            //{
            //    RollButton.Visibility = Visibility.Collapsed;
            //}

        }

        private void UpdateBoard(RollResult result)
        {
            //DISABLE ROLL BUTTON IF LAST ROLL
            if (result.IsLastRoll) RollButton.IsEnabled = false;

            //SHOW ROLL COUNT
            RollButton.Content = "Roll" + result.Player.Rolls;

            //SHOW NEW DICE VALUES
            Dice0.Text = Game.GetDieValue(0).ToString();
            Dice1.Text = Game.GetDieValue(1).ToString();
            Dice2.Text = Game.GetDieValue(2).ToString();
            Dice3.Text = Game.GetDieValue(3).ToString();
            Dice4.Text = Game.GetDieValue(4).ToString();

            //UPDATE UI TO SHOW AVAILABLE BOXES TO SELECT
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Rectangle r = FindName("Tap" + (i + 1) + "" + (j + 1)) as Rectangle;
                    if ((result.Layout.Spaces[i, j] == true) && (Game.GameBoard.Spaces[i,j] == false))
                    {
                        r.Fill = HeldBrush;
                    }
                    else r.Fill = ClearBrush;
                }
            }

            //UPDATE STRIKES
            StrikeText.Text = result.Player.Strikes + " Strikes";

            //UPDATE PLAYER NAME
            PlayerText.Text = result.Player.Name;
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
            t.Visibility = Visibility.Collapsed;
        }

        

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}