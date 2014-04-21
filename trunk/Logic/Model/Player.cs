using System;

namespace PongController
{
    public class Player
    {
        private static int ctr;
        public readonly int Id;
        public int Score { get; set; }
        public Paddle Paddle { get; set; }

        public Player(Orientation orientation, IGameView view)
        {
            Paddle = new Paddle(orientation, view);
            Id = ctr++;
        }
    }
}
