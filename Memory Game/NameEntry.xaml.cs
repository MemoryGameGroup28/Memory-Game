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
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        // player 2 name
        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        // Start game click
        private void Button_Click(object sender, RoutedEventArgs e)
        {          
                new MainWindow().Show();
                this.Close();          
        }
    }



}
