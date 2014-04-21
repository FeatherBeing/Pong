using System;
using System.Linq;
using System.Drawing;

namespace PongController
{
    public enum CollisionType
    {
        Paddle, Boundary
    }

    public class Ball
    {
        private readonly IGameView view;
        private readonly IGameController gameController;
        public readonly BallController ballController;
        private Vector velocity;
        private double angle;
        private event CollisionHandler Collision;
        public Point Point { get; private set; }

        public Ball(IGameView view, IGameController controller)
        {
            this.view = view;
            this.gameController = controller;
            ballController = new BallController(this);
        }

        public class BallController
        {
            private Ball ball;
            private Random rng = new Random();
            private int baseMod;

            public BallController(Ball ball)
            {
                this.ball = ball;
                ball.Collision += OnCollision;
            }

            public void Center(Player server)
            {
                ball.Point = new Point(ball.view.Boundaries.Width / 2, ball.view.Boundaries.Height / 2);

                //The ball will start moving from the center Point towards one of the different sides
                ball.angle = (server.Paddle.Orientation.Equals(Orientation.Left)) ? Math.PI : Math.PI * 2;

                //Re-randomize the base velocity of the ball
                baseMod = Math.Max(3, rng.Next(6));
            }

            public void UpdatePosition()
            {
                ball.velocity.X = (ball.angle == Math.PI) ? -5 : 5;
                ball.Point = new Point(ball.Point.X + ball.velocity.X, ball.Point.Y + ball.velocity.Y);


                //Check if the suggested point is beyond the boundaries of the window
                if (ball.Point.X > ball.view.Boundaries.Width || ball.Point.Y > ball.view.Boundaries.Height || ball.Point.X < 0
                    || ball.Point.Y < 0)
                {
                    ball.Collision(CollisionType.Boundary); // If it does raise collision event
                }

                //Check if the new point collides with the hitbox of a player paddle
                if (ball.gameController.Players[0].Paddle.GetHitbox().Any(point => point.Equals(ball.Point)) ||
                    ball.gameController.Players[1].Paddle.GetHitbox().Any(point => point.Equals(ball.Point)))
                {
                    ball.Collision(CollisionType.Paddle);
                }
            }

            public void OnCollision(CollisionType collisionType)
            {
                switch (collisionType)
                {
                    case CollisionType.Paddle:
                        ball.angle = (ball.angle == Math.PI) ? Math.PI * 2 : Math.PI;
                        ball.velocity.Y = (ball.angle == Math.PI) ? 5 : -5;
                        break;
                    case CollisionType.Boundary:
                        // If the collision is with a window boundary check if we need to bounce the ball 
                        // or make a player score
                        if (ball.Point.X > ball.view.Boundaries.Width || ball.Point.X < 0)
                        {
                            // If the angle of the ball of the ball is greater than ½ rad then the left paddle was 
                            // the shooter so he should score 
                            if (ball.angle == Math.PI * 2)
                            {
                                var scoringPlayer = Array.Find(ball.gameController.Players, player => player.Paddle.Orientation.Equals(Orientation.Left));
                                ball.gameController.PlayerScore(scoringPlayer);
                            } else // If not, then it's the right paddle
                            {
                                var scoringPlayer = Array.Find(ball.gameController.Players, player => player.Paddle.Orientation.Equals(Orientation.Right));
                                ball.gameController.PlayerScore(scoringPlayer);
                            }
                        } else
                        {
                            ball.velocity.Y = (ball.angle == Math.PI) ? -5 : 5;
                        }
                        break;
                }
            }
        }
    }
}   
