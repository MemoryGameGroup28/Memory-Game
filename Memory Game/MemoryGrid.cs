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
    public class MemoryGrid
    {
        // Create the Grid
        private Grid grid;
        private Grid mainGrid; //grid containing all memory cards
        private Grid scoreGrid;
        

        private int ImageRows = 4;
        private int ImageCols = 4;

        private int CurrentPlayer = 0;
        private int currentScorePlayer1 = 0;
        private int currentScorePlayer2 = 0;


        private bool card1Flip = false;
        private bool card2Flip = false;
        private Uri card1Image = null;
        private Uri card2Image = null;



        public MemoryGrid(Grid grid, int GridCol, int GridRow)
        {
            this.grid = grid;
            InitializeMemoryGrid(GridCol, GridRow);
            InitializeScoreGrid();
            AddBackgroundImages();
            GetImagesList();
            Labels();
        }

        //Generate MemoryGrid with parameters assigned to ImageRows/Cols + GridRow/Col
        private void InitializeMemoryGrid(int GridCol, int GridRow)
        {
            mainGrid = new Grid();
            mainGrid.ShowGridLines = true;
            //mainGrid.ShowGridLines = true;
            for (int i = 0; i < GridCol; i++)
            {
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());

            }
            for (int i = 0; i < GridRow; i++)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition());
            }
            Grid.SetColumn(mainGrid, 0);
            grid.Children.Add(mainGrid);
        }

        //Add backgroundimages to the generated grid
        private void AddBackgroundImages()
        {
            List<Uri> images = GetImagesList();
            for (int GridRow = 0; GridRow < ImageRows; GridRow++)
            {
                for (int GridCol = 0; GridCol < ImageCols; GridCol++)
                {
                    Image backgroundImage = new Image();
                    backgroundImage.Source = new BitmapImage(new Uri("Files/cardBackground.png", UriKind.Relative)); //Set background image
                    backgroundImage.Tag = images.First();
                    
                    images.RemoveAt(0);
                    backgroundImage.MouseDown += new MouseButtonEventHandler(CardClick);
                    Grid.SetColumn(backgroundImage, GridCol);
                    Grid.SetRow(backgroundImage, GridRow);
                    mainGrid.Children.Add(backgroundImage);

                }
            }
        }

        //Adding all game images to a list
        private List<Uri> GetImagesList()
        {
            List<Uri> images = new List<Uri>();
            for (int i = 0; i < 16; i++)
            {
                int imageNr = i % 8 + 1;
                Uri source = new Uri("Files/Card" + imageNr + ".png", UriKind.Relative);
                images.Add(source);

            }
            //IMAGE RANDOMZER COMES HERE



            //------------------------------//
            return images;
        }

        //On click event when user clicks on a card
        private void CardClick(object sender, MouseButtonEventArgs e)
        {
            CurrentPlayer = 1;
            //On click access image URI from List<URI>, convert URI to Bitmap Image and store data from card 1 in card1Image
            if (card1Flip == false)
            {
                Image card = (Image)sender;
                ImageSource front = new BitmapImage((Uri)card.Tag);
                card.Source = front;
                card1Flip = true;
                this.card1Image = (Uri)card.Tag;
            }
            //On click access image URI from List<URI>, convert URI to Bitmap Image and store data from card 2 in card2Image
            else if (card1Flip == true && card2Flip == false)
            {
                Image card = (Image)sender;
                ImageSource front = new BitmapImage((Uri)card.Tag);

                card.Source = front;
                card2Flip = true;
                this.card2Image = (Uri)card.Tag;
            }
            //Match function
            if (card1Flip && card2Flip == true)
            {
                if(card1Image.Equals(card2Image))
                {
                    if (CurrentPlayer == 1)
                    {
                        currentScorePlayer1 += 2;
                        //MessageBox.Show("point for player 1");
                    }
                    else if (CurrentPlayer ==2)
                    {
                        currentScorePlayer2 += 2;
                        MessageBox.Show("point for player 2");
                    }
                    card1Flip = false;
                    card2Flip = false;
                }

                else if (card1Image != card2Image)
                {
                    card1Flip = false;
                    card2Flip = false;
                    CurrentPlayer = 2;
                }
            }

        }

        //stopover in bangkok


        //Generate new Grid that holds all the score labels
        private void InitializeScoreGrid()
        {
            scoreGrid = new Grid();
            
            scoreGrid.ShowGridLines = true;
            for (int i = 0; i < 1; i++)
            {
                scoreGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 5; i++)
            {
                scoreGrid.RowDefinitions.Add(new RowDefinition());
            }
            Grid.SetColumn(scoreGrid, 1);
            grid.Children.Add(scoreGrid);
        }
        
        //Generate all the labels that will be placed within the scoregrid.
        private void Labels()
        {
            Label title = new Label();
            title.Content = "Memory";
            title.FontSize = 30;
            title.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumn(title, 1);
            Grid.SetRow(title, 0);
            scoreGrid.Children.Add(title);

            Label currentScore = new Label();
            currentScore.Content = "Current Score";
            currentScore.FontSize = 20;
            title.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumn(currentScore, 1);
            Grid.SetRow(currentScore, 1);
            scoreGrid.Children.Add(currentScore);

            Label scorePlayer1 = new Label();
            scorePlayer1.Content =  "Player 1: " + Convert.ToString(currentScorePlayer1);
            scorePlayer1.FontSize = 20;
            title.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumn(scorePlayer1, 1);
            Grid.SetRow(scorePlayer1, 2);
            scoreGrid.Children.Add(scorePlayer1);

            Label scorePlayer2 = new Label();
            scorePlayer2.Content = "Player2: " + Convert.ToString(currentScorePlayer2);
            scorePlayer2.FontSize = 20;
            title.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumn(scorePlayer2, 1);
            Grid.SetRow(scorePlayer2, 3);
            scoreGrid.Children.Add(scorePlayer2);

        }
    }
}
