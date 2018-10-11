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
    public class GameGrid
    {
        // Create the Grid
        private Grid grid;
        private int ImageRows = 4;
        private int ImageCols = 4;
        private int currentScorePlayer1 = 0;
        private int currentScorePlayer2 = 0;


        private bool card1Flip = false;
        private bool card2Flip = false;
        private BitmapImage card1Image = null;
        private BitmapImage card2Image = null;


        public GameGrid(Grid grid, int GridCol, int GridRow)
        {
            this.grid = grid;
            //this.ImageCols = ImageCols;
            //this.ImageRows = ImageRows;
            InitializeMemoryGrid(GridCol, GridRow);
            AddBackgroundImages();
            GetImagesList();
            TitleLabel();
            ScoreLabel();

        }

        //Generate MemoryGrid with parameters assigned to ImageRows/Cols + GridRow/Col
        private void InitializeMemoryGrid(int GridCol, int GridRow)
        {
            for (int i = 0; i < GridCol; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());

            }
            for (int i = 0; i < GridRow; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
        }

        //Add backgroundimages to the generated grid
        private void AddBackgroundImages()
        {
            List<ImageSource> images = GetImagesList();
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
                    grid.Children.Add(backgroundImage);

                }
            }
        }


        
        //Adding all game images to a list
        private List<ImageSource> GetImagesList()
        {
            List<ImageSource> images = new List<ImageSource>();
            for (int i = 0; i < 16; i++)
            {
                int imageNr = i % 8 + 1;
                ImageSource source = new BitmapImage(new Uri("Files/Card" + imageNr + ".png", UriKind.Relative));
                images.Add(source);

            }
            //IMAGE RANDOMZER COMES HERE



            //------------------------------//
            return images;
        }

        //On click event when user clicks on a card
        private void CardClick(object sender, MouseButtonEventArgs e)
        {
            if (card1Flip == false)
            {
                Image card = (Image)sender;
                ImageSource front = (ImageSource)card.Tag;
                card.Source = front;
                card1Flip = true;
                this.card1Image = front;
            }
            else if (card1Flip == true && card2Flip == false)
            {
                Image card = (Image)sender;
                ImageSource front = (ImageSource)card.Tag;
                card.Source = front;
                card2Flip = true;
            }
            if (card1Flip && card2Flip == true)
            {
                MessageBox.Show("2 flipped cards");
            }

        }
        


        #region Labels
        //Add title Label
        private void TitleLabel()
        {
            Label title = new Label();
            title.Content = "Memory";
            title.FontSize = 30;
            title.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetColumn(title, 4);
            Grid.SetRow(title, 0);
            grid.Children.Add(title);
        }

        //Add labels related to the current score
        private void ScoreLabel()
        {
            Label currentScore = new Label();
            currentScore.Content = "Current Score";
            currentScore.FontSize = 20;
            Grid.SetColumn(currentScore, 4);
            Grid.SetRow(currentScore, 1);
            grid.Children.Add(currentScore);

            Label scorePlayer1 = new Label();
            scorePlayer1.Content = "Player 1: " + currentScorePlayer1;
            scorePlayer1.FontSize = 20;
            Grid.SetColumn(scorePlayer1, 4);
            Grid.SetRow(scorePlayer1, 2);
            grid.Children.Add(scorePlayer1);


            Label scorePlayer2 = new Label();
            scorePlayer2.Content = "Player2: " + currentScorePlayer2;
            scorePlayer2.FontSize = 20;
            Grid.SetColumn(scorePlayer2, 4);
            Grid.SetRow(scorePlayer2, 3);
            grid.Children.Add(scorePlayer2);

        }
        #endregion Labels

    }
}
