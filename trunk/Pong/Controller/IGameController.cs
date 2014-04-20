using System;

namespace Pong
{
    internal enum Move
    {
        Up, Down
    }

    interface IGameController
    {
        Player[] Players { get; }
        PeriodicTick GameTicker { get; }
        /// <summary>
        /// Initializes this instance of the IGameController.
        /// </summary>
        void Start();
        void Refresh();
        void PlayerScore(Player player);
        void MovePaddle(int playerId, Move move);
        event PlayerWonHandler PlayerWin;
    }
}
