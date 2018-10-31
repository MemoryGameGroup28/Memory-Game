using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
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

        //Label vairables
        private Label scorePlayer1; //Stores the current score of Player 1
        private Label scorePlayer2; //Stores the current score of Player 2
        private Label scorePlayer3; //Stores the current score of Player 3
        private Label scorePlayer4; //Stores the current score of Player 4

        //Score variables
        private int currentPlayer = 0; //Checks which player's turn it is
        private int currentScorePlayer1 = 0; //Keeps track of the score of player 1
        private int currentScorePlayer2 = 0; //Keeps track of the score of player 2
        private int currentScorePlayer3 = 0; //Keeps track of the score of player 3
        private int currentScorePlayer4 = 0; //Keeps track of the score of player 4

        private string namePlayer1 = null;
        private string namePlayer2 = null;
        private string namePlayer3 = null;
        private string namePlayer4 = null;
        StreamReader sr = new StreamReader(@"C:\MemoryGame\Names.SAV");
        List<string> lines = new List<string>();

        //Card variables
        private DispatcherTimer aTimer = new DispatcherTimer(); //Time indicating when cards need to flip back if there is no match

        private bool card1Flip = false; //Checks if the user has clicked on a 1st card
        private bool card2Flip = false; //Checks if the user has clicked on a 2nd card
        private bool isRunning = false; //Checks if a progress is running.

        private Uri card1Image = null; //Stores the URI (image location) of card 1
        private Uri card2Image = null; //Stores the URI (image location) of card 2

        private Image card1 = null; //stores which card has been clicked first.
        private Image card2 = null; //stores which card has been clicked second.

        private string xyCard1= null; //stores Row&Column definition of card1
        private string xyCard2 = null; //stores Row&Column definition of card2

        private int currentMatches = 0; //keeps track of the amount of matches to show end game window


        public MemoryGrid(Grid grid, int GridCol, int GridRow)
        {
            this.grid = grid;
            InitializeMemoryGrid(GridCol, GridRow);
            InitializeScoreGrid();
            AddBackgroundImages();
            GetImagesList();
            LoadNames();
            Labels();
            Sounds();
            SetInitialTurn();
        }

        //Generate MemoryGrid with parameters assigned to ImageRows/Cols + GridRow/Col
        private void InitializeMemoryGrid(int GridCol, int GridRow)
        {
            mainGrid = new Grid();
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
            mainGrid.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
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
                Uri source = new Uri("Files/Card" + imageNr + ".png", UriKind.Relative); //Define search parameters
                images.Add(source);
            }
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
            
        }
        

        //On click event when user clicks on a card
        private void CardClick(object sender, MouseButtonEventArgs e)
        {
            var element = (UIElement)e.Source;
            //On click access image URI from List<URI>, convert URI to Bitmap Image and store data from card 1 in card1Image
            if(isRunning == false)
            {
                if (card1Flip == false)
                {
                    Image card = (Image)sender;
                    ImageSource front = new BitmapImage((Uri)card.Tag);
                    card.Source = front;
                    card1Flip = true;
                    xyCard1 = Convert.ToString(Grid.GetColumn(element)) + Convert.ToString(Grid.GetRow(element));
                    this.card1Image = (Uri)card.Tag;
                    this.card1 = card;
                }
                //On click access image URI from List<URI>, convert URI to Bitmap Image and store data from card 2 in card2Image
                else if (card1Flip == true && card2Flip == false)
                {
                    xyCard2 = Convert.ToString(Grid.GetColumn(element)) + Convert.ToString(Grid.GetRow(element));
                    if (xyCard1 != xyCard2)
                    {
                        Image card = (Image)sender;
                        ImageSource front = new BitmapImage((Uri)card.Tag);
                        card.Source = front;
                        card2Flip = true;

                        this.card2Image = (Uri)card.Tag;
                        this.card2 = card;
                        isRunning = true;
                        cardMatch();
                    }
                }
            }
        }

        //Function to check if 2 cards are equal to eachother and have different grid positions
        private void cardMatch()
        {
            if (isRunning == true)
            {
                if (card1Image.Equals(card2Image) && xyCard1 != xyCard2)
                {
                    if (currentPlayer == 1)
                    {
                        currentScorePlayer1++;
                        scorePlayer1.Content = "\n" + namePlayer1 + " :            " + currentScorePlayer1;
                        scored.Load();
                        scored.Play();
                        currentMatches++;
                    }
                    else if (currentPlayer == 2)
                    {
                        currentScorePlayer2++;
                        scorePlayer2.Content = "\n" + namePlayer2 + " :            " + currentScorePlayer2;
                        scored.Load();
                        scored.Play();
                        currentMatches++;
                    }
                    else if (currentPlayer == 3)
                    {
                        currentScorePlayer3++;
                        scorePlayer3.Content = "\n" + namePlayer3 + " :            " + currentScorePlayer3;
                        scored.Load();
                        scored.Play();
                        currentMatches++;
                    }
                    else if (currentPlayer == 4)
                    {
                        currentScorePlayer4++;
                        scorePlayer4.Content = "\n" + namePlayer4 + " :            " + currentScorePlayer4;
                        scored.Load();
                        scored.Play();
                        currentMatches++;
                    }

                    card1Flip = false;
                    card2Flip = false;
                    matches();
                }
                else if (card1Image.Equals(card2Image) && xyCard1 == xyCard2)
                {
                    aTimer = new DispatcherTimer();
                    aTimer.Interval = TimeSpan.FromMilliseconds(1000);
                    aTimer.Tick += timer_Tick;
                    aTimer.Start();
                }
                else if (card1Image != card2Image)
                {
                    //Mouse.OverrideCursor = Cursors.None; //hide cursor
                    //timer to return cards to original position after 1 second.
                    aTimer = new DispatcherTimer();
                    aTimer.Interval = TimeSpan.FromMilliseconds(1000);
                    aTimer.Tick += timer_Tick;
                    aTimer.Start();

                    if (currentPlayer == 1)
                    {
                        if (namePlayer2 != null)
                        {
                            currentPlayer = 2;
                            scorePlayer1.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                            scorePlayer2.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                            scorePlayer1.FontSize = 20;
                            scorePlayer2.FontSize = 25;
                        }
                        else if (namePlayer2 == null)
                        {
                            if (namePlayer3 != null)
                            {
                                currentPlayer = 3;
                                scorePlayer1.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                                scorePlayer3.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                scorePlayer1.FontSize = 20;
                                scorePlayer3.FontSize = 25;
                            }
                            else if (namePlayer3 == null)
                            {
                                if (namePlayer4 != null)
                                {
                                    currentPlayer = 4;
                                    scorePlayer1.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                                    scorePlayer4.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                    scorePlayer1.FontSize = 20;
                                    scorePlayer4.FontSize = 25;
                                }
                                else if (namePlayer4 == null)
                                {
                                    currentPlayer = 1;
                                }
                            }
                        }
                        negative.Load();
                        negative.Play();
                    }
                    else if (currentPlayer == 2)
                    {
                        if (namePlayer3 != null)
                        {
                            currentPlayer = 3;
                            scorePlayer2.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                            scorePlayer3.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                            scorePlayer2.FontSize = 20;
                            scorePlayer3.FontSize = 25;
                        }
                        else if (namePlayer3 == null)
                        {
                            if (namePlayer4 != null)
                            {
                                currentPlayer = 4;
                                scorePlayer2.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                                scorePlayer4.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                scorePlayer2.FontSize = 20;
                                scorePlayer4.FontSize = 25;
                            }
                            else if (namePlayer4 == null)
                            {
                                if (namePlayer1 != null)
                                {
                                    currentPlayer = 1;
                                    scorePlayer2.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                                    scorePlayer1.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                    scorePlayer2.FontSize = 20;
                                    scorePlayer1.FontSize = 25;
                                }
                                else if (namePlayer1 == null)
                                {
                                    currentPlayer = 2;
                                }
                            }
                        }
                        negative.Load();
                        negative.Play();
                    }
                    else if (currentPlayer == 3)
                    {
                        if (namePlayer4 != null)
                        {
                            currentPlayer = 4;
                            scorePlayer3.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                            scorePlayer4.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                            scorePlayer3.FontSize = 20;
                            scorePlayer4.FontSize = 25;
                        }
                        else if (namePlayer4 == null)
                        {
                            if (namePlayer1 != null)
                            {
                                currentPlayer = 1;
                                scorePlayer3.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                                scorePlayer1.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                scorePlayer3.FontSize = 20;
                                scorePlayer1.FontSize = 25;
                            }
                            else if (namePlayer1 == null)
                            {
                                if (namePlayer2 != null)
                                {
                                    currentPlayer = 2;
                                    scorePlayer3.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                                    scorePlayer2.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                    scorePlayer3.FontSize = 20;
                                    scorePlayer2.FontSize = 25;
                                }
                                else if (namePlayer2 == null)
                                {
                                    currentPlayer = 3;
                                }
                            }
                        }
                        negative.Load();
                        negative.Play();
                    }
                    else if (currentPlayer == 4)
                    {
                        if (namePlayer1 != null)
                        {
                            currentPlayer = 1;
                            scorePlayer4.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                            scorePlayer1.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                            scorePlayer4.FontSize = 20;
                            scorePlayer1.FontSize = 25;
                        }
                        else if (namePlayer1 == null)
                        {
                            if (namePlayer2 != null)
                            {
                                currentPlayer = 2;
                                scorePlayer4.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                                scorePlayer2.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                scorePlayer4.FontSize = 20;
                                scorePlayer2.FontSize = 25;
                            }
                            else if (namePlayer2 == null)
                            {
                                if (namePlayer3 != null)
                                {
                                    currentPlayer = 3;
                                    scorePlayer4.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                                    scorePlayer3.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                    scorePlayer4.FontSize = 20;
                                    scorePlayer3.FontSize = 25;
                                }
                                else if (namePlayer3 == null)
                                {
                                    currentPlayer = 4;
                                }
                            }
                        }
                        negative.Load();
                        negative.Play();
                    }
                }
            }
            isRunning = false;
        }

        //After timer reaches 0, perform below action.
        private void timer_Tick(object sender, EventArgs e)
        {
            //Set image of card 1 && 2 = backgroundimag.png
            card1Image = null;
            card2Image = null;
            xyCard1 = null;
            xyCard2 = null;

            card1Flip = false;
            card2Flip = false;
            card1.Source = new BitmapImage(new Uri("Files/cardBackground.png", UriKind.Relative)); //Set background image
            card2.Source = new BitmapImage(new Uri("Files/cardBackground.png", UriKind.Relative)); //Set background image
            Mouse.OverrideCursor = Cursors.Arrow;
            aTimer.Stop();
        }
        
        //Generate new Grid that holds all the score labels
        private void InitializeScoreGrid()
        {
            scoreGrid = new Grid();
            scoreGrid.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            //scoreGrid.ShowGridLines = true;
            for (int i = 0; i < 1; i++)
            {
                scoreGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 6; i++)
            {
                scoreGrid.RowDefinitions.Add(new RowDefinition());

            }

            //Sets image for first 2 rows
            for (int i = 0; i < 2; i++)
            {
                Image scoreImageGrey = new Image();
                scoreImageGrey.Source = new BitmapImage(new Uri("Files/ScoreLabel3.png", UriKind.Relative));
                scoreImageGrey.Stretch = Stretch.Fill;
                Grid.SetRow(scoreImageGrey, i);
                scoreGrid.Children.Add(scoreImageGrey);
            }
            //Sets image for all even rows
            for (int i = 2; i < 6; i++)
            {
                if (i % 2 == 0)
                {
                    Image scoreImageGreen = new Image();
                    scoreImageGreen.Source = new BitmapImage(new Uri("Files/ScoreLabel.png", UriKind.Relative));
                    scoreImageGreen.Stretch = Stretch.Fill;
                    Grid.SetRow(scoreImageGreen, i);
                    scoreGrid.Children.Add(scoreImageGreen);
                }

            }
            //Sets image for all odd rows
            for (int i = 2; i < 6; i++)
            {
                if (i % 2 != 0)
                {
                    Image scoreImageGreen = new Image();
                    scoreImageGreen.Source = new BitmapImage(new Uri("Files/ScoreLabel2.png", UriKind.Relative));
                    scoreImageGreen.Stretch = Stretch.Fill;
                    Grid.SetRow(scoreImageGreen, i);
                    scoreGrid.Children.Add(scoreImageGreen);
                }
            }
            Grid.SetColumn(scoreGrid, 1);
            grid.Children.Add(scoreGrid);
        }
        
        private void matches()
        {
            if (currentMatches == 8)
            {
                MessageBox.Show("Congratulatuons you've found all the matches");
                if (Directory.Exists(@"C:\MemoryGame"))
                {
                    File.Delete(@"C:\MemoryGame\Names.SAV");
                    Directory.Delete(@"C:\MemoryGame");
                }
            }
        }
        
        //Loads the names from the save file created during the name entry. 
        private void LoadNames()
        {
            while (!sr.EndOfStream) //Adds all the lines from the .SAV file if the file has not ended
            {
                lines.Add(sr.ReadLine());
            }

            if (!string.IsNullOrEmpty(lines[0])) //Only adds the name if the line is not empty
            {
                namePlayer1 = lines[0];
            }
            if (!string.IsNullOrEmpty(lines[1])) //Only adds the name if the line is not empty
            {
                namePlayer2 = lines[1];
            }
            if (!string.IsNullOrEmpty(lines[2])) //Only adds the name if the line is not empty
            {
                namePlayer3 = lines[2];
            }
            if (!string.IsNullOrEmpty(lines[3])) //Only adds the name if the line is not empty
            {
                namePlayer4 = lines[3];
            }
            sr.Close(); //closes the streamreader
        }

        //Generate all the labels that will be placed within the scoregrid.
        private void Labels()
        {
            Label title = new Label();
            title.Content = "\nMemory";
            title.FontSize = 30;
            title.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumn(title, 1);
            Grid.SetRow(title, 0);
            scoreGrid.Children.Add(title);

            Label currentScore = new Label();
            currentScore.Content = "\nScores";
            currentScore.FontSize = 20;
            currentScore.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumn(currentScore, 1);
            Grid.SetRow(currentScore, 1);
            scoreGrid.Children.Add(currentScore);

            if(namePlayer1 != null)
            {
                scorePlayer1 = new Label();
                scorePlayer1.Content = "\n" + namePlayer1 + " :            " + currentScorePlayer1;
                scorePlayer1.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                scorePlayer1.FontSize = 25;
                scorePlayer1.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(scorePlayer1, 1);
                Grid.SetRow(scorePlayer1, 2);
                scoreGrid.Children.Add(scorePlayer1);
            }
            if(namePlayer2 != null)
            {
                scorePlayer2 = new Label();
                scorePlayer2.Content = "\n" + namePlayer2 + " :            " + currentScorePlayer2;
                scorePlayer2.FontSize = 20;
                scorePlayer2.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                scorePlayer2.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(scorePlayer2, 1);
                Grid.SetRow(scorePlayer2, 3);
                scoreGrid.Children.Add(scorePlayer2);
            }
            if (namePlayer3 != null)
            {
                scorePlayer3 = new Label();
                scorePlayer3.Content = "\n" + namePlayer3  + " :            " + currentScorePlayer3;
                scorePlayer3.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                scorePlayer3.FontSize = 20;
                scorePlayer3.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(scorePlayer3, 1);
                Grid.SetRow(scorePlayer3, 4);
                scoreGrid.Children.Add(scorePlayer3);
            }
            if (namePlayer4 != null)
            {
                scorePlayer4 = new Label();
                scorePlayer4.Content = "\n" + namePlayer4 + " :            " + currentScorePlayer4;
                scorePlayer4.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                scorePlayer4.FontSize = 20;
                scorePlayer4.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(scorePlayer4, 1);
                Grid.SetRow(scorePlayer4, 5);
                scoreGrid.Children.Add(scorePlayer4);
            }

        }

        //Added sound effects
        private void Sounds()
        {
            SoundPlayer noMatch = new SoundPlayer();
            noMatch.Stream = Properties.Resources.Negative;
            negative = noMatch;

            SoundPlayer Match = new SoundPlayer();
            Match.Stream = Properties.Resources.Scored;
            scored = Match;

            MediaPlayer backgroundMusic = new MediaPlayer();
            backgroundMusic.Open(new Uri("ThemeMusic.wav", UriKind.Relative));
            themeMusic = backgroundMusic;
            themeMusic.MediaEnded += new EventHandler(Media_Ended);
            themeMusic.Play();

        }
        //If the themeMusic ends below function will ensure the music gets looped.
        private void Media_Ended(object sender, EventArgs e)
        {
            themeMusic.Position = TimeSpan.Zero;
            themeMusic.Play();
        }

        //Set the initial turn
        private void SetInitialTurn()
        {
            if (namePlayer1 != null) //If player 1 name is entered, current player is 1
            {
                currentPlayer = 1;
                scorePlayer1.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            else if (namePlayer1 == null)
            {
                if (namePlayer2 != null) //if player 1 name is empty and player 2 name is entered, current player is 2
                {
                    currentPlayer = 2;
                    scorePlayer2.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                }
                else if (namePlayer2 == null)
                {
                    if (namePlayer3 != null) //if player 2 name is empty and player 3 name is entered, current player is 3
                    {
                        currentPlayer = 3;
                        scorePlayer3.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    }
                    else if (namePlayer3 == null)
                    {
                        if (namePlayer4 != null) //if player 3 name is empty and player 4 name is entered, current player is 4
                        {
                            currentPlayer = 4;
                            scorePlayer4.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        }
                        else if (namePlayer4 == null) //if no names have been entered.
                        {
                            MessageBox.Show("No names have been entered, the game will restart.");
                            if (Directory.Exists(@"C:\MemoryGame"))
                            {
                                File.Delete(@"C:\MemoryGame\Names.SAV");
                                Directory.Delete(@"C:\MemoryGame");
                            }
                            Process.Start(Application.ResourceAssembly.Location);
                            Application.Current.Shutdown();
                        }
                    }
                }
            }
        }
    }
}
