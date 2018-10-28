using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Memory_Game
{
    /// <summary>
    /// Interaction logic for NameEntry.xaml
    /// </summary>
    public partial class NameEntry : Window
    {      
        public NameEntry()
        {
            InitializeComponent();
        }

        



        // Menu bar clicks.

        private void Exit_Game_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Restart_Game_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
        
        private void Highscores_Click(object sender, RoutedEventArgs e)
        {
            new Highscores().Show();
            this.Close();
        }

        // player 1 name
        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string Player1Name = txtPlayer1.Text;
        }

        // player 2 name
        public void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            string Player2Name = txtPlayer2.Text;
        }

        // player 3 name
        public void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {
            string Player3Name = txtPlayer3.Text;
        }

        // player 4 name
        public void TextBox_TextChanged_3(object sender, TextChangedEventArgs e)
        {
            string Player4Name = txtPlayer4.Text;
        }


        // Start game click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Saving of player names to user settings
            Memory_Game.Properties.Settings.Default.Player1Name = txtPlayer1.Text;
            Memory_Game.Properties.Settings.Default.Player2Name = txtPlayer2.Text;
            Memory_Game.Properties.Settings.Default.Player3Name = txtPlayer3.Text;
            Memory_Game.Properties.Settings.Default.Player4Name = txtPlayer4.Text;
            Memory_Game.Properties.Settings.Default.Save();

            new MainWindow().Show();
                this.Close();          
        }
    }



}
