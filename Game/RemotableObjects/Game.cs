using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RemotableObjects
{
    public class Game : MarshalByRefObject, IGame
    {

        public enum TileType
        {
            Grass,
            Water,
            Sand
        };

        private static Game instance;
        private int movesCounter = 0;
        private readonly int movesPerTurn = 10;

        private LinkedList<Player> players;
        private LinkedList<Mob> mobs;

        private TileType[,] terrain;
        private GameObject[,] objectsOnMap;

        private String[][] map;

        private int mapWidth;
        private int mapHeight;

        


        private Game()
        {
            players = new LinkedList<Player>();
            setObjects();
            //setMap();
        }

        public static Game Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Game();
                }
                return instance;
            }
        }

        public void setMap()
        {
            map = new String[32][];
            for (int i = 0; i < 32; ++i) map[i] = new String[40];
            map = File.ReadLines(@"../../resources/map.csv").Select(x => x.Split(',')).ToArray();
        }

        public void setObjects()
        {
            objectsOnMap = new GameObject[40,32];
            mapHeight = 32;
            mapWidth = 40;
        }

        public void setMobs()
        {
            //TODO: Ustawienie pozycji, statystyk i posiadanych przedmiotów przez potworki na mapie
        }

        public override object InitializeLifetimeService() { return (null); }

        public Player getCurrentPlayer()
        {
            int currentPlayerIndex = movesCounter / movesPerTurn % players.Count;
            return players.ElementAt(currentPlayerIndex);
        }

        public String getCurrentPlayerName()
        {
            Player currentPlayer = getCurrentPlayer();
            if (currentPlayer != null)
            {
                return currentPlayer.Name;
            }
            return null;
        }

        public void makeMove(String playerName, MoveType moveType)
        {        
            if (getCurrentPlayerName() == playerName)
            {
                switch(moveType)
                {
                    case MoveType.Up:
                        moveUp();
                        break;
                    case MoveType.Down:
                        moveDown();
                        break;
                    case MoveType.Left:
                        moveLeft();
                        break;
                    case MoveType.Right:
                        moveRight();
                        break;
                    case MoveType.Attack:
                        attack();
                        break;
                }
            }
        }

        private void moveUp()
        {
            Player player = getCurrentPlayer();
            Point position = player.Position;

            //TODO: Sprawdzenie, czy na drodze nie ma przeciwnika
            if (player.Position.Y == 0)
            {
                Console.WriteLine("Player " + player.Name + "trying to move out of bounds");
                return;
            }

            GameObject gameObject = objectsOnMap[position.X, position.Y - 1];
            if (gameObject != null && gameObject.isSolid())
            {
                Console.WriteLine("Player " + player.Name + "trying to move into obstacle");
            }
            else if (gameObject != null && !gameObject.isSolid() && gameObject.GetType() == typeof(Item))
            {
                Console.WriteLine("Player " + player.Name + "collected item");
                player.collectItem((Item)gameObject);
                objectsOnMap[position.X, position.Y - 1] = null;
            }
            else
            {
                player.Position = new Point(position.X, position.Y - 1);
                Console.WriteLine("Move made by player: " + player.Name);
                ++movesCounter;
            } 
        }

        private void moveDown()
        {
            Player player = getCurrentPlayer();
            Point position = player.Position;
            
            //TODO: Sprawdzenie, czy na drodze nie ma przeciwnika

            if (player.Position.Y == mapHeight - 1)
            {
                Console.WriteLine("Player " + player.Name + "trying to move out of bounds");
                return;
            }

            GameObject gameObject = objectsOnMap[position.X, position.Y + 1];
            if (gameObject != null && gameObject.isSolid())
            {
                Console.WriteLine("Player " + player.Name + "trying to move into obstacle");
            }
            else if (gameObject != null && !gameObject.isSolid() && gameObject.GetType() == typeof(Item))
            {
                Console.WriteLine("Player " + player.Name + "collected item");
                player.collectItem((Item)gameObject);
                objectsOnMap[position.X, position.Y + 1] = null;
            }
            else
            {
                player.Position = new Point(position.X, position.Y + 1);
                Console.WriteLine("Move made by player: " + player.Name);
                ++movesCounter;
            }
        }

        private void moveLeft()
        {
            Player player = getCurrentPlayer();
            Point position = player.Position;
            
            //TODO: Sprawdzenie, czy na drodze nie ma przeciwnika
            if (player.Position.X == 0)
            {
                Console.WriteLine("Player " + player.Name + "trying to move out of bounds");
                return;
            }

            GameObject gameObject = objectsOnMap[position.X - 1, position.Y];
            if (gameObject != null && gameObject.isSolid())
            {
                Console.WriteLine("Player " + player.Name + "trying to move into obstacle");
            }
            else if (gameObject != null && !gameObject.isSolid() && gameObject.GetType() == typeof(Item))
            {
                Console.WriteLine("Player " + player.Name + "collected item");
                player.collectItem((Item)gameObject);
                objectsOnMap[position.X - 1, position.Y] = null;
            }
            else
            {
                player.Position = new Point(position.X - 1, position.Y);
                Console.WriteLine("Move made by player: " + player.Name);
                ++movesCounter;
            }
        }

        private void moveRight()
        {
            Player player = getCurrentPlayer();
            Point position = player.Position;

            //TODO: Sprawdzenie, czy na drodze nie ma przeciwnika
            if (player.Position.X == mapWidth - 1)
            {
                Console.WriteLine("Player " + player.Name + "trying to move out of bounds");
                return;
            }

            GameObject gameObject = objectsOnMap[position.X + 1, position.Y];
            if (gameObject != null && gameObject.isSolid())
            {
                Console.WriteLine("Player " + player.Name + "trying to move into obstacle");
            }
            else if (gameObject != null && !gameObject.isSolid() && gameObject.GetType() == typeof(Item))
            {
                Console.WriteLine("Player " + player.Name + "collected item");
                player.collectItem((Item)gameObject);
                objectsOnMap[position.X + 1, position.Y] = null;
            }
            else
            {
                player.Position = new Point(position.X + 1, position.Y);
                Console.WriteLine("Move made by player: " + player.Name);
                ++movesCounter;
            }
        }

        private void attack()
        {
            //TODO: Najlepiej to chyba zrobić poprzez sprawdzenie czy na polu obok (którym?) znajduje się przeciwnik.
            // Jeśli tak, to zadać mu obrażenia i samemu jakieś otrzymać. Jeśli przeciwnik padnie, to dostaje się 
            // jakiś przedmiot czy coś w tym stylu, do obmyślenia :)
        }

        public String connectNewPlayer()
        {
            String name = "Player " + (players.Count + 1);
            if (!players.Any(p => p.Name == name))
            {
                addPlayer(name);
                Console.WriteLine("New player connected: " + name);
                return name;
            }
            else
            {
                Console.WriteLine("New player rejected because the name already exists: " + name);
                return null;
            }
            
        }

        public Player getPlayer(String name)
        {
            return players.FirstOrDefault(p => p.Name == name);
        }

        public LinkedList<Player> getAllPlayers()
        {
            return players;
        }

        public String[][] GetMap()
        {
            return map;
        }

        private void addPlayer(String name)
        {
            Player player = new Player();
            player.Name = name;
            player.Position = new Point(players.Count + 5, players.Count + 5);
            //TODO: Przydałoby się LEPIEJ ustawić pozycję początkową (random?) i statystyki
            players.AddLast(player);
        }
    }
}
