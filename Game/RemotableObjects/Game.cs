using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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

        private int mapWidth;
        private int mapHeight;

        


        private Game()
        {
            players = new LinkedList<Player>();
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

        public void setMap(String fileName)
        {
            //TODO: Wczytywanie mapy z pliku to tablicy terrain + ustawienie zmiennych mapWidth i mapHeight
        }

        public void setObjects()
        {
            //TODO: Wczytywanie przedmiotów do tablicy objectsOnMap, chyba najlepiej jakiś random. Rozmiar tablicy powinien odpowiadać rozmiarowi tablicy terrain.
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
            //TODO: Ciało metody analogicznie do moveUp()
        }

        private void moveLeft()
        {
            //TODO: Ciało metody analogicznie do moveUp()
        }

        private void moveRight()
        {
            //TODO: Ciało metody analogicznie do moveUp()
        }

        private void attack()
        {
            //TODO: Najlepiej to chyba zrobić poprzez sprawdzenie czy na polu obok (którym?) znajduje się przeciwnik.
            // Jeśli tak, to zadać mu obrażenia i samemu jakieś otrzymać. Jeśli przeciwnik padnie, to dostaje się 
            // jakiś przedmiot czy coś w tym stylu, do obmyślenia :)
        }

        public bool connectNewPlayer(String name)
        {
            if (!players.Any(p => p.Name == name))
            {
                addPlayer(name);
                Console.WriteLine("New player connected: " + name);
                return true;
            }
            else
            {
                Console.WriteLine("New player rejected because the name already exists: " + name);
                return false;
            }
        }

        private void addPlayer(String name)
        {
            Player player = new Player();
            player.Name = name;
            //TODO: Przydałoby się jeszcze ustawić pozycję początkową (random?) i statystyki
            players.AddLast(player);
        }
    }
}
