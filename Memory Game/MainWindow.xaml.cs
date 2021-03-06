﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Memory_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int GridRow = 4;
        private int GridCol = 4;

        private int ScoreRow = 5;
        private int ScoreCol = 1;



        //HOW TO USE A CLASS WITHIN THE MAIN WINDOW: 
            //Class name - function name -> Define function with class parameters
            //GameGrid gameGrid = new GameGrid(col, row); 
        
        //-----------------------------------------//

        //Main Game Screen that initializes all game components
        public MainWindow()
        {
            InitializeComponent();
            InitializeGameGrid();

            MemoryGrid grid = new MemoryGrid(GameGrid, GridCol, GridRow);
            //GameGrid.Background = new SolidColorBrush(Color.FromRgb(0,0,0));
            
        }


        //Give a certain width/height to each column/row item within the game grid defined in the MainWindow.xaml
        private void InitializeGameGrid()
        {
            ColumnDefinition gameColumn = new ColumnDefinition();
            gameColumn.Width = new GridLength(7.5, GridUnitType.Star);
            GameGrid.ColumnDefinitions.Add(gameColumn);
            
            ColumnDefinition scoreColumn = new ColumnDefinition();
            scoreColumn.Width = new GridLength(2.5, GridUnitType.Star);
            GameGrid.ColumnDefinitions.Add(scoreColumn);

        }
        // menu controls
        private void Instructions_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.eduplace.com/ss/act/rules.html");
        }
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }

}

