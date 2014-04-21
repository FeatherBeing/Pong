using System;

namespace Pong
{
    class GameController : IGameController
    {
        private const int WINNING_SCORE = 10;
        private readonly Ball ball;
        public readonly IGameView view;
        public event PlayerWonHandler PlayerWin;
        public Player[] Players { get; private set; }
        public PeriodicTick GameTicker { get; private set; }
        public bool IsGameOver { get; private set; }

        public GameController(IGameView view, Player player, Player player2)
        {
            this.view = view;
            Players = new Player[] { player, player2 };
            ball = new Ball(view, this);
        }

        public void Start()
        {
            const int REFRESH_RATE = 25; // TIME IN MILLISECONDS IN WHICH THE GAME RECALCULATES ALL OBJECT POSITIONS
                                         // CHANGING THIS CAN DRAMATICALLY AFFECT GAME PERFORMANCE!!!!
            GameTicker = new PeriodicTick(REFRESH_RATE);
            GameTicker.Tick += Refresh; // Each time our PeriodicTick ticks we will refresh the ball position and angle

            //Start the ticker...
            GameTicker.Start();

            //...then get the ball moving
            ball.ballController.Center(Players[new Random().Next(Players.Length)]);
        }

        public void Refresh()
        {
            //Update positions of ball here...
            ball.ballController.UpdatePosition();

            //.. Then call Draw() in the IGameView
            view.Draw(ball, Players[0].Paddle, Players[1].Paddle);
        }

        public void MovePaddle(int playerId, Move move)
        {
            const int MOVE_MODIFIER = 15; // THIS DEFINES HOW MANY PIXELS TO MOVE THE PADDLE PER MOVE CALL
            int id = playerId;

            if (move.Equals(Move.Up))
            {
                Players[id].Paddle.Position.Y = Math.Max(Players[id].Paddle.Position.Y - MOVE_MODIFIER, 0);
            } 
            else
            {
                // We must add the length of the paddle here because it will transgress the boundary on it's other side
                Players[id].Paddle.Position.Y = Math.Min(
                    Players[id].Paddle.Position.Y + MOVE_MODIFIER, view.Boundaries.Height - Players[id].Paddle.Size.Height);
            }
        }

        public void PlayerScore(Player player)
        {
            if (++player.Score == (WINNING_SCORE)) // Check if player wins if so...
            {
                // ... Raise PlayerWin event, cancel the GameTicker 
                //     and release all resources used by this instance of IGameView
                PlayerWin(player);                                    
                GameTicker.CancellationTokenSrc.Cancel(); 
                IsGameOver = true;
                (view as IDisposable).Dispose();
            }

            ball.ballController.Center(player); // Recenter the ball
        }
    }
}
