using System;
using System.Collections.Generic;
using System.Drawing;

namespace Pong
{
    internal enum Orientation
    {
        Left, Right
    }

    class Paddle
    {
        public Point Position;
        public readonly Orientation Orientation;
        public readonly Size Size = new Size(0, 20);

        public Paddle(Orientation orientation, IGameView view) 
        {
            Orientation = orientation;
            // Paddle starting position should be in the center Y-axis with differing X values
            int x = (orientation.Equals(Orientation.Left)) ? view.Boundaries.Width - (view.Boundaries.Width / 20) : view.Boundaries.Width / 20; 
            Position = new Point(x, view.Boundaries.Height / 2);
        }

        public Point[] GetHitbox()
        {
            var hitLocations = new List<Point>();

            for (int i = 0; i < this.Size.Height; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (this.Orientation.Equals(Orientation.Right))
                    {
                        hitLocations.Add(new Point(this.Position.X + j, this.Position.Y + i));
                    } else
                    {
                        hitLocations.Add(new Point(this.Position.X - j, this.Position.Y + i));
                    }
                }
            }
            return hitLocations.ToArray();
        }
    }
}
