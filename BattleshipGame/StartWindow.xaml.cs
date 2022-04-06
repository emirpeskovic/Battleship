using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BattleshipGame
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        private bool validConfiguration = false;

        private List<string> funnyNames = new List<string>()
        {
            "horseshoe",
            "carl",
            "isotope",
            "clocked",
            "radiant"
        };
        
        public StartWindow()
        {
            // For random name generation
            if (File.Exists("funnyNames.txt"))
            {
                var names = File.ReadAllLines("funnyNames.txt");
                foreach (var name in names)
                {
                    funnyNames.Add(name);
                }
            }
            
            // Apply settings to textboxes when application is ready
            Loaded += delegate
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.recentServerIp))
                {
                    serverIpTextBox.Text = Properties.Settings.Default.recentServerIp;
                }

                if (!string.IsNullOrEmpty(Properties.Settings.Default.profileName))
                {
                    profileNameTextBox.Text = Properties.Settings.Default.profileName;
                }
                else
                {
                    // Create a random name based on 2 random strings from the funnyNames list
                    // Separate each random string with a dash
                    // Append a random number from 1 to 100 at the end of the name to make it somewhat unique
                    profileNameTextBox.Text = funnyNames[new Random().Next(0, funnyNames.Count)] + "-" + funnyNames[new Random().Next(0, funnyNames.Count)] + "-" + new Random().Next(1, 100);
                }
            };

            InitializeComponent();
        }

        private void serverIpTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            string ip = serverIpTextBox.Text;

            if (!IPAddress.TryParse(ip, out var address))
            {
                validConfiguration = false;
                serverIpTextBox.BorderBrush = Brushes.Red;
                return;
            }

            validConfiguration = true;
        }

        private void profileNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string name = profileNameTextBox.Text;

            // Use regex to check if the name is valid
            // Name must be at least 4 characters long and max 24 characters long
            // Name must only contain letters, numbers, and dashes
            // Name must not start or end with a dash
            // Name must not contain more than 2 dashes in a row
            // Name must not contain more than 1 dash at the beginning or end of the name

            if (name.Length < 4 || name.Length > 24)
            {
                validConfiguration = false;
                profileNameTextBox.BorderBrush = Brushes.Red;
                return;
            }

            if (!Regex.IsMatch(name, @"^[a-zA-Z0-9-]*$"))
            {
                validConfiguration = false;
                profileNameTextBox.BorderBrush = Brushes.Red;
                return;
            }

            if (name.StartsWith("-") || name.EndsWith("-"))
            {
                validConfiguration = false;
                profileNameTextBox.BorderBrush = Brushes.Red;
                return;
            }

            if (name.Count(x => x == '-') > 2)
            {
                validConfiguration = false;
                profileNameTextBox.BorderBrush = Brushes.Red;
                return;
            }

            validConfiguration = true;
        }

        private void joinGameButton_Click(object sender, RoutedEventArgs e)
        {
            // If invalid, show message box
            if (!validConfiguration)
            {
                MessageBox.Show("Invalid configuration. Please check your settings.");
                return;
            }

            // TODO: Check if the server is online
        }
    }
}
