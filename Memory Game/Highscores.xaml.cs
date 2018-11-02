using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for Highscores.xaml
    /// </summary>
    public partial class Highscores : Window
    {

        // reads from highscore saves
        StreamReader HighscoreSave1;
        StreamReader HighscoreSave2;
        StreamReader HighscoreSave3;
        StreamReader HighscoreSave4;
        // empty lists for highscore saves
        List<string> highscoresave1 = new List<string>();
        List<string> highscoresave2 = new List<string>();
        List<string> highscoresave3 = new List<string>();
        List<string> highscoresave4 = new List<string>();



        public Highscores()
        {
            InitializeComponent();
            loadhighscores();
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
            new Window1().Show();
            this.Close();
        }

        private void loadhighscores()
        {
            

            if (File.Exists(@"C:\MemoryGame\Highscores\Highscore1.SAV"))
            {
                HighscoreSave1 = new StreamReader(@"C:\MemoryGame\Highscores\Highscore1.SAV");

                while (!HighscoreSave1.EndOfStream)
                {
                    highscoresave1.Add(HighscoreSave1.ReadLine());
                }
                if (!string.IsNullOrEmpty(highscoresave1[0]))
                {
                    Dateblock1.Text = highscoresave1[0];
                }
                if (!string.IsNullOrEmpty(highscoresave1[1]))
                {
                    Player1.Text = highscoresave1[1];
                }
                if (!string.IsNullOrEmpty(highscoresave1[3]))
                {
                    Player2.Text = highscoresave1[3];
                }
                if (!string.IsNullOrEmpty(highscoresave1[5]))
                {
                    Player3.Text = highscoresave1[5];
                }
                if (!string.IsNullOrEmpty(highscoresave1[7]))
                {
                    Player4.Text = highscoresave1[7];
                }
                if (!string.IsNullOrEmpty(highscoresave1[2]))
                {
                    Score1.Text = highscoresave1[2];
                }
                if (!string.IsNullOrEmpty(highscoresave1[4]))
                {
                    Score2.Text = highscoresave1[4];
                }
                if (!string.IsNullOrEmpty(highscoresave1[6]))
                {
                    Score3.Text = highscoresave1[6];
                }
                if (!string.IsNullOrEmpty(highscoresave1[8]))
                {
                    Score4.Text = highscoresave1[8];
                }

            }

            if (File.Exists(@"C:\MemoryGame\Highscores\Highscore2.SAV"))
            {
                HighscoreSave2 = new StreamReader(@"C:\MemoryGame\Highscores\Highscore2.SAV");
                while (!HighscoreSave2.EndOfStream)
                {
                    highscoresave2.Add(HighscoreSave2.ReadLine());
                }
                if (!string.IsNullOrEmpty(highscoresave2[0]))
                {
                    Dateblock2.Text = highscoresave2[0];
                }
                if (!string.IsNullOrEmpty(highscoresave2[1]))
                {
                    Player1_2.Text = highscoresave2[1];
                }
                if (!string.IsNullOrEmpty(highscoresave2[3]))
                {
                    Player2_2.Text = highscoresave2[3];
                }
                if (!string.IsNullOrEmpty(highscoresave2[5]))
                {
                    Player3_2.Text = highscoresave2[5];
                }
                if (!string.IsNullOrEmpty(highscoresave2[7]))
                {
                    Player4_2.Text = highscoresave2[7];
                }
                if (!string.IsNullOrEmpty(highscoresave2[2]))
                {
                    Score1_2.Text = highscoresave2[2];
                }
                if (!string.IsNullOrEmpty(highscoresave2[4]))
                {
                    Score2_2.Text = highscoresave2[4];
                }
                if (!string.IsNullOrEmpty(highscoresave2[6]))
                {
                    Score3_2.Text = highscoresave2[6];
                }
                if (!string.IsNullOrEmpty(highscoresave2[8]))
                {
                    Score4_2.Text = highscoresave2[8];
                }
            }

            if (File.Exists(@"C:\MemoryGame\Highscores\Highscore3.SAV"))
            {
                HighscoreSave3 = new StreamReader(@"C:\MemoryGame\Highscores\Highscore3.SAV");
                while (!HighscoreSave3.EndOfStream)
                {
                    highscoresave3.Add(HighscoreSave3.ReadLine());
                }
                if (!string.IsNullOrEmpty(highscoresave3[0]))
                {
                    Dateblock3.Text = highscoresave3[0];
                }
                if (!string.IsNullOrEmpty(highscoresave3[1]))
                {
                    Player1_3.Text = highscoresave3[1];
                }
                if (!string.IsNullOrEmpty(highscoresave3[3]))
                {
                    Player2_3.Text = highscoresave3[3];
                }
                if (!string.IsNullOrEmpty(highscoresave3[5]))
                {
                    Player3_3.Text = highscoresave3[5];
                }
                if (!string.IsNullOrEmpty(highscoresave3[7]))
                {
                    Player4_3.Text = highscoresave3[7];
                }
                if (!string.IsNullOrEmpty(highscoresave3[2]))
                {
                    Score1_3.Text = highscoresave3[2];
                }
                if (!string.IsNullOrEmpty(highscoresave3[4]))
                {
                    Score2_3.Text = highscoresave3[4];
                }
                if (!string.IsNullOrEmpty(highscoresave3[6]))
                {
                    Score3_3.Text = highscoresave3[6];
                }
                if (!string.IsNullOrEmpty(highscoresave3[8]))
                {
                    Score4_3.Text = highscoresave3[8];
                }
            }

            if (File.Exists(@"C:\MemoryGame\Highscores\Highscore4.SAV"))
            {
                HighscoreSave4 = new StreamReader(@"C:\MemoryGame\Highscores\Highscore4.SAV");
                while (!HighscoreSave4.EndOfStream)
                {
                    highscoresave4.Add(HighscoreSave4.ReadLine());
                }
                if (!string.IsNullOrEmpty(highscoresave4[0]))
                {
                    Dateblock4.Text = highscoresave4[0];
                }
                if (!string.IsNullOrEmpty(highscoresave4[1]))
                {
                    Player1_4.Text = highscoresave4[1];
                }
                if (!string.IsNullOrEmpty(highscoresave4[3]))
                {
                    Player2_4.Text = highscoresave4[3];
                }
                if (!string.IsNullOrEmpty(highscoresave4[5]))
                {
                    Player3_4.Text = highscoresave4[5];
                }
                if (!string.IsNullOrEmpty(highscoresave4[7]))
                {
                    Player4_4.Text = highscoresave4[7];
                }
                if (!string.IsNullOrEmpty(highscoresave4[2]))
                {
                    Score1_4.Text = highscoresave4[2];
                }
                if (!string.IsNullOrEmpty(highscoresave4[4]))
                {
                    Score2_4.Text = highscoresave4[4];
                }
                if (!string.IsNullOrEmpty(highscoresave4[6]))
                {
                    Score3_4.Text = highscoresave4[6];
                }
                if (!string.IsNullOrEmpty(highscoresave4[8]))
                {
                    Score4_4.Text = highscoresave4[8];
                }
            }


        }



        
        
        
        





    }

   
}

