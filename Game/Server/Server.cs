using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemotableObjects;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting;


namespace Server
{
    class Server
    {
        Game game;

        Server()
        {
            game = Game.Instance;
        }

        static void Main(string[] args)
        {
            TcpChannel channel = new TcpChannel(1357);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(Game), "Game", WellKnownObjectMode.Singleton);
            Console.WriteLine("Game server up and running. Waiting for clients...");

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
