using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using PongController;

namespace Pong
{
    public partial class GameForm : Form, IGameView, IDisposable
    {
        public Size Boundaries { get; private set; }
        private bool isGameOver;
        private Bitmap gameObjects;
        private IGameController gameController;
        private readonly Pen pen = new Pen(Color.White, 5);
        private readonly Font myFont = new System.Drawing.Font("Helvetica", 40, FontStyle.Regular);
        private Keys[] inputKeys = new Keys[] { Keys.Up, Keys.Down, Keys.W, Keys.S };

        public GameForm()
        {
            InitializeComponent();
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            Boundaries = new Size(ClientSize.Width, ClientSize.Height); // Always set boundaries to size of the view control
            gameController = new GameController(this, new Player(PongController.Orientation.Left, this), new Player(PongController.Orientation.Right, this));
            gameController.PlayerWin += (this as IGameView).PlayerWon;
            // Start a seperate worker task for game logic
            Task.Factory.StartNew(
                () => 
                {
                    System.Threading.Thread.CurrentThread.Name = "WorkerThread";
                    gameController.Start(); 
                }); 
            gameObjects = new Bitmap(Boundaries.Width, Boundaries.Height);
        }

        void IGameView.PlayerWon(Player winner)
        {
            isGameOver = true;
            MessageBox.Show(String.Concat("Player ", winner.Id.ToString() + " won!"));
        }

        void IGameView.Draw(Ball ball, Paddle paddle, Paddle paddle2)
        {
            // If the game is over we need to cease drawing as that will throw an exception
            // Due to PlayerWin event calling a release of drawing resources in this instance of IGameView
            if (isGameOver) 
            {       
                return;
            }

            //Draw to bitmap
            using (Graphics gameObj = Graphics.FromImage(gameObjects))
            {
                gameObj.Clear(Color.Black); // Clear area to allow redrawing of all game objects
                gameObj.DrawEllipse(pen, new Rectangle(ball.Point, new Size(5, 5))); // Ball
                gameObj.DrawLine(pen, new Point(Size.Width / 2, 0), new Point(Size.Width / 2, Size.Height)); // Net
                gameObj.DrawString(gameController.Players[0].Score.ToString(), myFont, pen.Brush, Boundaries.Width / 2.7f, 0); // Score p1
                gameObj.DrawString(gameController.Players[1].Score.ToString(), myFont, pen.Brush, Boundaries.Width / 1.8f, 0); // Score p2
                gameObj.DrawLine(pen, paddle.Position, new Point(paddle.Position.X, paddle.Position.Y + paddle.Size.Height));
                gameObj.DrawLine(pen, paddle2.Position, new Point(paddle2.Position.X, paddle2.Position.Y + paddle2.Size.Height));
                Invalidate(); // Invalidate to force redraw of game objects      
            }
        }

        void IDisposable.Dispose()
        {
            pen.Dispose();
            myFont.Dispose();
        }

        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.DrawImage(gameObjects, new Point(0, 0));
            // No dispose here because we're using double buffering
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            //Check if pressed key equals any of the player assigned keys.. If so call MovePaddle()
            foreach (Keys key in Keyboard.GetPressedKeys())
            {
                if (inputKeys.Any(entry => entry.Equals(key)))
                {
                    switch (key)
                    {
                        case Keys.Up:
                            gameController.MovePaddle(0, PongController.Move.Up);
                            break;
                        case Keys.Down:
                            gameController.MovePaddle(0, PongController.Move.Down);
                            break;
                        case Keys.W:
                            gameController.MovePaddle(1, PongController.Move.Up);
                            break;
                        case Keys.S:
                            gameController.MovePaddle(1, PongController.Move.Down);
                            break;
                    }
                }
            }           
        }
    }
}
