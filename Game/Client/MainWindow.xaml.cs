using RemotableObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading;
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
        ImageSource shield;

        private static String serverAddress = "localhost";
        private static int tcpPort = 1357;

        private String playerName;
        private Game game;

        private String connectionTest()
        {
            TcpChannel chan = new TcpChannel();
            ChannelServices.RegisterChannel(chan, false);

            game = (Game)Activator.GetObject(typeof(Game), "tcp://" + serverAddress + ":" + tcpPort + "/Game");
            
            return game.connectNewPlayer();
        }

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

        public delegate void UpdatePlayers();
        public delegate void UpdateGui();

        private void UpdatingThread()
        {
            while(true)
            {
                Thread.Sleep(100);
                Terrain.Dispatcher.Invoke(new UpdatePlayers(RenderPlayers));
                Terrain.Dispatcher.Invoke(new UpdatePlayers(UpdateObjects));
                Gui.Dispatcher.Invoke(new UpdateGui(RenderGui));
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

        LinkedList<Image> heroes;
        LinkedList<Image> itemImages;

        void DrawPlayer(Player player, bool isRightFaced, int skin, Image hero)
        {
            if (!player.IsKilled)
            {
                var knightUri = new Uri(@"resources/knight" + (player.Id + 1) + ".png", UriKind.Relative);
                knight = new BitmapImage(knightUri);

                //Player player = game.getPlayer(playerName);
                Point position = new Point(player.Position.X, player.Position.Y);
                Terrain.Children.Remove(hero);

                hero.Source = knight;
                hero.Stretch = Stretch.Fill;
                hero.Margin = new Thickness(20 * position.X - 10, 20 * position.Y - 20, 0, 0);
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
            else
            {
                Terrain.Children.Remove(hero);

                //Player player = game.getPlayer(playerName);

                /* Point position = new Point(player.Position.X, player.Position.Y);
                 Terrain.Children.Remove(hero);

                 hero.Source = shield;
                 hero.Stretch = Stretch.Fill;
                 hero.Margin = new Thickness(20 * position.X - 10, 20 * position.Y - 20, 0, 0);
                 hero.Width = 40;
                 hero.Height = 40;
                 Terrain.Children.Add(hero); */
            }
        }

        void UpdateObjects()
        {
            var shieldUri = new Uri(@"resources/shield.png", UriKind.Relative);
            shield = new BitmapImage(shieldUri);

            LinkedList<System.Drawing.Point> items = game.getObjectsPositions("Shield");

            foreach (Image item in itemImages)
            {
                Terrain.Children.Remove(item);
            }

            foreach (System.Drawing.Point itemPosition in items)
            {
                Point position = new Point(itemPosition.X, itemPosition.Y);
                Image item = new Image();
                item.Source = shield;
                item.Stretch = Stretch.Fill;
                item.Margin = new Thickness(20 * position.X - 10, 20 * position.Y - 20, 0, 0);
                item.Width = 40;
                item.Height = 40;
                itemImages.AddLast(item);
                Terrain.Children.Add(item);
            }
        }

        bool rightFaced;

        public MainWindow()
        {
            InitializeComponent();
            DrawMap();
            playerName = connectionTest();
            heroes = new LinkedList<Image>();
            itemImages = new LinkedList<Image>();
            for (int i = 0; i < 32; ++i)
            {
                heroes.AddLast(new Image());
            }


            RenderPlayers();
            Thread test = new Thread(new ThreadStart(UpdatingThread));

            Random r = new Random(1993);
            for (int i = 0; i < 60; ++i) {
                DrawTree(r.Next(7,20), r.Next(5,27));
            }

            UpdateObjects();

            lifeBarBorders = new Dictionary<Character, Rectangle>();
            lifeBars = new Dictionary<Character, Rectangle>();

            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            test.Start();
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key) 
            {
                case Key.Left:
                    game.makeMove(playerName, MoveType.Left);
                    rightFaced = false;
                    break;
                case Key.Right:
                    game.makeMove(playerName, MoveType.Right);
                    rightFaced = true;
                    break;
                case Key.Up:
                    game.makeMove(playerName, MoveType.Up);
                    break;
                case Key.Down:
                    game.makeMove(playerName, MoveType.Down);
                    break;
            }
            
            RenderPlayers();
            RenderGui();
            UpdateObjects();            
        }


        void RenderPlayers()
        {
            LinkedList<Player> players = game.getAllPlayers();
            int i = 0;
            foreach (var player in players)
            {
                DrawPlayer(player, false, i + 1, heroes.ElementAt(player.Id));
                ++i;
            }
        }

        void RenderGui()
        {
            Gui.Children.Clear();
            LinkedList<Player> players = game.getAllPlayers();
            foreach (var player in players)
            {
                if (!lifeBarBorders.ContainsKey(player))
                {
                    lifeBarBorders.Add(player,new Rectangle());
                    lifeBars.Add(player, new Rectangle());
                }
                if (player.Name.Equals(playerName))
                    DrawPlayerGui(player);
                else if(!player.IsKilled)
                    DrawOpponentGui(player);
            }
        }

        private Dictionary<Character, Rectangle> lifeBars;
        private Dictionary<Character, Rectangle> lifeBarBorders;

        public void DrawPlayerGui(Player player)
        {
            Rectangle lifeBarBorder = lifeBarBorders[player];

            lifeBarBorder = new Rectangle();
            lifeBarBorder.Width = 210;
            lifeBarBorder.Height = 20;
            SolidColorBrush borderBrush = new SolidColorBrush();
            borderBrush.Color = Color.FromArgb(127, 0, 0, 0);
            lifeBarBorder.Fill = borderBrush;
            Gui.Children.Add(lifeBarBorder);

            Canvas.SetTop(lifeBarBorder, 10);
            Canvas.SetLeft(lifeBarBorder, 10);

            Rectangle lifeBar = lifeBars[player];
            lifeBar = new Rectangle();
            lifeBar.Width = (player.HealthPoints / 100.0f) * 210.0f;
            lifeBar.Height = 20;
            SolidColorBrush barBrush = new SolidColorBrush();
            barBrush.Color = Color.FromArgb(255, 255, 0, 0);
            lifeBar.Fill = barBrush;
            Gui.Children.Add(lifeBar);

            Canvas.SetTop(lifeBar, 10);
            Canvas.SetLeft(lifeBar, 10);
        }

        public void DrawOpponentGui(Character opponent)
        {
            Rectangle lifeBarBorder = lifeBarBorders[opponent];
            
            lifeBarBorder.Width = 40;
            lifeBarBorder.Height = 4;
            SolidColorBrush borderBrush = new SolidColorBrush();
            borderBrush.Color = Color.FromArgb(127, 0, 0, 0);
            lifeBarBorder.Fill = borderBrush;
            Gui.Children.Add(lifeBarBorder);

            Canvas.SetTop(lifeBarBorder, 20 * opponent.Position.Y - 30);
            Canvas.SetLeft(lifeBarBorder, 20 * opponent.Position.X - 10);

            Rectangle lifeBar = lifeBars[opponent];

            lifeBar.Width = (opponent.HealthPoints / 100.0f) * 40.0f;
            lifeBar.Height = 4;
            SolidColorBrush barBrush = new SolidColorBrush();
            barBrush.Color = Color.FromArgb(255, 255, 0, 0);
            lifeBar.Fill = barBrush;
            Gui.Children.Add(lifeBar);

            Canvas.SetTop(lifeBar, 20 * opponent.Position.Y - 30);
            Canvas.SetLeft(lifeBar, 20 * opponent.Position.X - 10);
        }

    }
}
