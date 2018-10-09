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


        public GameGrid(Grid grid, int GridCol, int GridRow)
        {
            this.grid = grid;
            this.ImageCols = ImageCols;
            this.ImageRows = ImageRows;
            InitializeMemoryGrid(GridCol, GridRow);
            AddImages();
            GetImagesList();

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

        //Add background images to the generated grid
        private void AddImages()
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

        //On click event when user clicks on a card
        private void CardClick(object sender, MouseButtonEventArgs e)
        {
            Image card = (Image)sender;
            ImageSource front = (ImageSource)card.Tag;
            card.Source = front;
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
        






        private void AddLabel()
        {
            Label title = new Label();
            title.Content = "Memory";
            title.FontFamily = new FontFamily("Ariel");
            title.FontSize = 40;
            title.HorizontalAlignment = HorizontalAlignment.Center;

            Grid.SetColumn(title, 1);
            grid.Children.Add(title);
        }



    }
}
