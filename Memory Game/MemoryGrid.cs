using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
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
using System.Timers;
using System.Media;



namespace Memory_Game
{
    public class MemoryGrid
    {
        // Create the Grid
        private Grid grid; //main grid containing memory card and score label grids
        private Grid mainGrid; //grid containing all memory cards
        private Grid scoreGrid; //grid containing score labels
        
        //Variables defining width and height of mainGrid
        private int ImageRows = 4;
        private int ImageCols = 4;

        //Sound variables
        private SoundPlayer negative = new SoundPlayer();
        private SoundPlayer scored = new SoundPlayer();
        private MediaPlayer themeMusic = new MediaPlayer();


        //private SoundPlayer themeMusic = new SoundPlayer();

        //Label vairables
        private Label scorePlayer1;
        private Label scorePlayer2;
        
        //Score variables
        private int currentPlayer = 0; //Checks which player's turn it is
        private int currentScorePlayer1 = 0; //Keeps track of the score of player 1
        private int currentScorePlayer2 = 0; //Keeps track of the score of player 2
        private string namePlayer1 = null; //Stores the name of player 1 filled in from the main menu
        private string namePlayer2 = null; //Stores the name of player 2 filled in from the main menu

        //Card variables
        private DispatcherTimer aTimer = new DispatcherTimer(); //Time indicating when cards need to flip back if there is no match
        private bool card1Flip = false; //Checks if the user has clicked on a 1st card
        private bool card2Flip = false; //Checks if the user has clicked on a 2nd card
        private Uri card1Image = null; //Stores the URI (image location) of card 1
        private Uri card2Image = null; //Stores the URI (image location) of card 2
        private Image card1 = null; //stores which card has been clicked first (location on the board / location in the list)
        private Image card2 = null; //stores which card has been clicked second (location on the board / location in the list)



        public MemoryGrid(Grid grid, int GridCol, int GridRow)
        {
            this.grid = grid;
            currentPlayer = 1;
            InitializeMemoryGrid(GridCol, GridRow);
            InitializeScoreGrid();
            AddBackgroundImages();
            GetImagesList();    
            Labels();
            Sounds();
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

            List<Uri> randomList = new List<Uri>();

            Random r = new Random();
            int randomIndex = 0;
            while (images.Count > 0)
            {
                randomIndex = r.Next(0, images.Count); //Choose a random object in the list
                randomList.Add(images[randomIndex]); //add it to the new, random list
                images.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list

            //------------------------------//
            
        }

        //On click event when user clicks on a card
        private void CardClick(object sender, MouseButtonEventArgs e)
        {
            //On click access image URI from List<URI>, convert URI to Bitmap Image and store data from card 1 in card1Image
            if (card1Flip == false)
            {
                Image card = (Image)sender;
                ImageSource front = new BitmapImage((Uri)card.Tag);
                card.Source = front;
                card1Flip = true;
                this.card1Image = (Uri)card.Tag;
                this.card1 = card;
            }
            //On click access image URI from List<URI>, convert URI to Bitmap Image and store data from card 2 in card2Image
            else if (card1Flip == true && card2Flip == false)
            {
                Image card = (Image)sender;
                ImageSource front = new BitmapImage((Uri)card.Tag);

                card.Source = front;
                card2Flip = true;
                this.card2Image = (Uri)card.Tag;
                this.card2 = card;
            }
            //Match function
            if (card1Flip && card2Flip == true)
            {
                if(card1Image.Equals(card2Image))
                {
                    if (currentPlayer == 1)
                    {
                        currentScorePlayer1++;
                        scorePlayer1.Content = "Player 1: " + currentScorePlayer1;
                        scored.Load();
                        scored.Play();
                    }
                    else if (currentPlayer == 2)
                    {
                        currentScorePlayer2++;
                        scorePlayer2.Content = "Player 2: " + currentScorePlayer2;
                        scored.Load();
                        scored.Play();
                    }
                    card1Flip = false;
                    card2Flip = false;
                }

                else if (card1Image != card2Image)
                {
                    //timer to return cards to original position after 1 second.
                    aTimer = new DispatcherTimer();
                    aTimer.Interval = TimeSpan.FromMilliseconds(1000);
                    aTimer.Tick += timer_Tick;
                    aTimer.Start();
                    if (currentPlayer == 1)
                    {
                        currentPlayer = 2;
                        scorePlayer1.Foreground = Brushes.Black;
                        scorePlayer2.Foreground = Brushes.Green;
                        negative.Load();
                        negative.Play();

                    }
                    else if (currentPlayer == 2)
                    {
                        currentPlayer = 1;
                        scorePlayer2.Foreground = Brushes.Black;
                        scorePlayer1.Foreground = Brushes.Green;
                        negative.Load();
                        negative.Play();
                    }


                }
            }

        }
        //After timer reaches 0, perform below action.
        private void timer_Tick(object sender, EventArgs e)
        {
            //Set image of card 1 && 2 = backgroundimag.png
            card1Flip = false;
            card2Flip = false;
            card1.Source = new BitmapImage(new Uri("Files/cardBackground.png", UriKind.Relative)); //Set background image
            card2.Source = new BitmapImage(new Uri("Files/cardBackground.png", UriKind.Relative)); //Set background image
            aTimer.Stop();
        }
        

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
            currentScore.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumn(currentScore, 1);
            Grid.SetRow(currentScore, 1);
            scoreGrid.Children.Add(currentScore);

            scorePlayer1 = new Label();
            scorePlayer1.Content = "Player1: " + currentScorePlayer1;
            scorePlayer1.Foreground = Brushes.Green;
            scorePlayer1.FontSize = 20;
            scorePlayer1.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumn(scorePlayer1, 1);
            Grid.SetRow(scorePlayer1, 2);
            scoreGrid.Children.Add(scorePlayer1);
            

            scorePlayer2 = new Label();
            scorePlayer2.Content = "Player2: " + currentScorePlayer2;
            scorePlayer2.FontSize = 20;
            scorePlayer2.Foreground = Brushes.Black;
            scorePlayer2.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumn(scorePlayer2, 1);
            Grid.SetRow(scorePlayer2, 3);
            scoreGrid.Children.Add(scorePlayer2);
        }

        private void Sounds()
        {
            SoundPlayer noMatch = new SoundPlayer();
            noMatch.Stream = Properties.Resources.Negative;
            negative = noMatch;

            SoundPlayer Match = new SoundPlayer();
            Match.Stream = Properties.Resources.Scored;
            scored = Match;

            MediaPlayer backgroundMusic = new MediaPlayer();
            backgroundMusic.Open(new Uri(@"D:\School\Memory-Game-Project-Group28\Memory-Game\Memory Game\Files\ThemeMusic.wav"));
            themeMusic = backgroundMusic;
            themeMusic.Position = TimeSpan.FromMilliseconds(1);
            themeMusic.Play();
        }



    }
}
