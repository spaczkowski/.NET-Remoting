using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotableObjects
{
    //TODO: Może będą jakieś inne możliwości ruchu?
    public enum MoveType
    {
        Up,
        Down,
        Left,
        Right,
        Attack
    };

    public interface IGame
    {
        void makeMove(String playerName, MoveType moveType);
        bool connectNewPlayer(String name);
        Player getCurrentPlayer();
    }
}
