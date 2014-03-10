using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using FiveByFiveLogic;

namespace FiveByFive
{
    public partial class GameSetup : PhoneApplicationPage
    {

        List<Player> Players = new List<Player>();
        
        public GameSetup()
        {
            InitializeComponent();
        }

        private void AddNewPlayer_Click(object sender, RoutedEventArgs e)
        {
            PlayerTypes.Visibility = Visibility.Visible;
        }

        private void SavePlayer_Click(object sender, RoutedEventArgs e)
        {
            NameBox.Visibility = Visibility.Collapsed;
            PlayerTypes.Visibility = Visibility.Collapsed;
            StartGameButton.Visibility = Visibility.Visible;

            Player p = new Player { IsHumanPlayer = true, Name=NameInput.Text };
            Players.Add(p);
            PlayerList.ItemsSource = null;
            PlayerList.ItemsSource = Players;

            NameInput.Text = "";

            if (Players.Count() == 3)
            {
                AddPlayerButton.Visibility = Visibility.Collapsed;
            }
        }

        private void HumanPlayer_Click(object sender, RoutedEventArgs e)
        {
            NameBox.Visibility = Visibility.Visible;
            NameInput.Focus();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            string q = "";
            foreach (Player p in Players)
            {
                q += p.Name + "," + p.IsHumanPlayer.ToString() + ",";
            }
            NavigationService.Navigate(new Uri("/MainPage.xaml?q=" + q, UriKind.Relative));
        }
    }
}