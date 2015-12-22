using RemotableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace DebugClient
{
    class Program
    {
        private static String serverAddress = "localhost";
        private static int tcpPort = 1357;

        static void connectionTest()
        {
            TcpChannel chan = new TcpChannel();
            ChannelServices.RegisterChannel(chan, false);

            IGame game = (Game)Activator.GetObject(typeof(Game), "tcp://" + serverAddress + ":" + tcpPort + "/Game");

            int i = 0;
            while (true)
            {
                i++;
                Console.ReadLine();
                game.connectNewPlayer("Player" + i);
            }
        }

        static void playerManagementTest()
        {
            IGame game = Game.Instance;
            game.connectNewPlayer("A");
            game.connectNewPlayer("B");
            game.connectNewPlayer("C");
            game.connectNewPlayer("B");
            game.connectNewPlayer("A");

            for (int i = 0; i < 50; ++i)
            {
                game.makeMove("A");
            }

            for (int i = 0; i < 50; ++i)
            {
                game.makeMove("B");
            }

            for (int i = 0; i < 50; ++i)
            {
                game.makeMove("C");
            }

            for (int i = 0; i < 50; ++i)
            {
                game.makeMove("B");
            }

            for (int i = 0; i < 50; ++i)
            {
                game.makeMove("A");
            }
        }

        static void Main(string[] args)
        {
            playerManagementTest();
        }
    }
}
