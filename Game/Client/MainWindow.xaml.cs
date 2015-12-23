using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ImageSource grass;
        ImageSource sand;
        ImageSource water;

        ImageSource knight;

        void DrawMap()
        {
            //Uri do plików z teksturami
            var grassUri = new Uri(@"resources/grass.jpg", UriKind.Relative);
            var sandUri = new Uri(@"resources/sand.jpg", UriKind.Relative);
            var waterUri = new Uri(@"resources/water.jpg", UriKind.Relative);

            

            //tutaj brzydko podana ścieżka, ale to i tak będzie pobierane z serwera więc walić póki co
            var map = File.ReadLines(@"../../resources/map.csv").Select(x => x.Split(',')).ToArray();

            grass = new BitmapImage(grassUri);
            sand = new BitmapImage(sandUri);
            water = new BitmapImage(waterUri);
            

            Image[,] terrain = new Image[40, 32];

            for (int i = 0; i < 40; ++i)
            {
                for (int j = 0; j < 32; ++j)
                {
                    terrain[i, j] = new Image();

                    terrain[i, j].Margin = new Thickness(i * 20, j * 20, 0, 0);

                    terrain[i, j].Height = 20;
                    terrain[i, j].Width = 20;

                    switch (map[j][i])
                    {
                        case "G":
                            terrain[i, j].Source = grass;
                            break;
                        case "W":
                            terrain[i, j].Source = water;
                            break;
                        case "S":
                            terrain[i, j].Source = sand;
                            break;
                        default:
                            terrain[i, j].Source = grass;
                            break;
                    }

                    terrain[i, j].Stretch = Stretch.Fill;

                    Terrain.Children.Add(terrain[i, j]);
                }
            }
        }

        void DrawTree(int x, int y)
        {
            var treeUri = new Uri(@"resources/tree.png", UriKind.Relative);
            var treeBmp = new BitmapImage(treeUri);

            var tree = new Image();
            tree.Source = treeBmp;
            tree.Stretch = Stretch.Fill;
            tree.Margin = new Thickness(20*x-10, 20*y-20, 0, 0);
            tree.Width = 40;
            tree.Height = 40;
            Terrain.Children.Add(tree);
        }

        Image hero;

        void DrawPlayer(int x, int y, bool isRightFaced)
        {
            var knightUri = new Uri(@"resources/knight.png", UriKind.Relative);
            knight = new BitmapImage(knightUri);

            hero = new Image();
            hero.Source = knight;
            hero.Stretch = Stretch.Fill;
            hero.Margin = new Thickness(20 * x - 10, 20 * y - 10, 0, 0);
            hero.Width = 40;
            hero.Height = 40;
            Terrain.Children.Add(hero);

            if (isRightFaced)
            {
                hero.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform flipTrans = new ScaleTransform();
                flipTrans.ScaleX = -1;
                hero.RenderTransform = flipTrans;
            }
        }

        int x, y;
        bool rightFaced;

        public MainWindow()
        {
            InitializeComponent();
            DrawMap();

            x = 12;
            y = 7;
            
            DrawPlayer(12, 7, false);
            
            Random r = new Random(1993);
            for (int i = 0; i < 60; ++i) {
                DrawTree(r.Next(7,20), r.Next(5,27));
            }

            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key) 
            {
                case Key.Left:
                    x--;
                    rightFaced = false;
                    break;
                case Key.Right:
                    x++;
                    rightFaced = true;
                    break;
                case Key.Up:
                    y--;
                    break;
                case Key.Down:
                    y++;
                    break;
            }

            if (x < 0) x = 0;
            if (x > 39) x = 39;
            if (y < 0) y = 0;
            if (y > 31) y = 31;

            Terrain.Children.Remove(hero);
            DrawPlayer(x, y, rightFaced);
        }

    }
}
