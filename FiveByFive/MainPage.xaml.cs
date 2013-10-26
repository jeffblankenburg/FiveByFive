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
            //DataContext = Game;
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
            string s = r.Name;
        }

        private void RollButton_Click(object sender, RoutedEventArgs e)
        {
            if (Game.RollDice())
            {
                for (int i = 0;i<5;i++)
                {
                    TextBlock t = FindName("Dice" + i) as TextBlock;
                    t.Text = Game.GetDieValue(i).ToString();
                }
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