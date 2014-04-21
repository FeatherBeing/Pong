using System;
using System.Drawing;

namespace Pong
{
    interface IGameView
    {
        Size Boundaries { get; }
        /// <summary>
        /// This method will draw all game objects. 
        /// </summary>
        /// <param name="ball"></param>
        /// <param name="paddle"></param>
        /// <param name="paddle2"></param>
        void Draw(Ball ball, Paddle paddle, Paddle paddle2);
        void PlayerWon(Player winner);
    }
}
