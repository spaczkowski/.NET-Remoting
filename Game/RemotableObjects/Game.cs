using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotableObjects
{
    public class Game : MarshalByRefObject, IGame
    {
        private static Game instance;
        private int movesCounter = 0;
        private readonly int movesPerTurn = 10;
        private LinkedList<String> players;


        private Game()
        {
            players = new LinkedList<String>();
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

        public override object InitializeLifetimeService() { return (null); }

        public String getCurrentPlayer()
        {
            int currentPlayerIndex = movesCounter / movesPerTurn % players.Count;
            return players.ElementAt(currentPlayerIndex);
        }

        public void makeMove(String player)
        {        
            if (getCurrentPlayer() == player)
            {
                Console.WriteLine("Move made by player: " + player);
                ++movesCounter;
            }
        }

        public bool connectNewPlayer(String name)
        {
            if (!players.Contains(name))
            {
                players.AddLast(name);
                Console.WriteLine("New player connected: " + name);
                return true;
            }
            else
            {
                Console.WriteLine("New player rejected because the name already exists: " + name);
                return false;
            }
        }
    }
}
