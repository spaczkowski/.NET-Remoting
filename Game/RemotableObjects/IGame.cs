using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotableObjects
{
    public interface IGame
    {
        void makeMove(String player);
        bool connectNewPlayer(String name);
        String getCurrentPlayer();
    }
}
