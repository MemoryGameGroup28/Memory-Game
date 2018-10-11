using System;
using System.Collections.Generic;
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



        //HOW TO USE A CLASS WITHIN THE MAIN WINDOW: 
            //Class name - function name -> Define function with class parameters
            //GameGrid gameGrid = new GameGrid(col, row); 
        
        //-----------------------------------------//

        //Main Game Screen that initializes all game components
        public MainWindow()
        {
            InitializeComponent();
            GameGrid grid = new GameGrid(GameGrid, GridCol, GridRow);
        }

        
        //Give a certain width/height to each column/row item within the game
        private void InitializeGameGrid()
        {
            ColumnDefinition gameColumn = new ColumnDefinition();
            gameColumn.Width = new GridLength(7, GridUnitType.Star);
            GameGrid.ColumnDefinitions.Add(gameColumn);

            ColumnDefinition scoreColumn = new ColumnDefinition();
            scoreColumn.Width = new GridLength(3, GridUnitType.Star);
            GameGrid.ColumnDefinitions.Add(scoreColumn);
        }

    }

}

