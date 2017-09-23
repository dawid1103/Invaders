using System;
using System.Drawing;

namespace Invaders
{
    internal class Shot
    {
        private const int MOVE_INTERVAL = 20;
        private const int WIDTH = 5;
        private const int HEIGHT = 15;
        private Direction direction;
        private Rectangle boundaries;

        public Point Location { get; private set; }

        public Shot(Point location, Direction direction, Rectangle boundaries)
        {
            Location = new Point(location.X - WIDTH / 2, location.Y);
            this.direction = direction;
            this.boundaries = boundaries;
        }

        public bool Move()
        {
            if (boundaries.Contains(Location))
            {
                switch (direction)
                {
                    case Direction.Up:
                        Location = new Point(Location.X, Location.Y - MOVE_INTERVAL);
                        break;
                    case Direction.Down:
                        Location = new Point(Location.X, Location.Y + MOVE_INTERVAL);
                        break;
                    default:
                        break;
                }

                return true;
            }

            return false;
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Yellow, new Rectangle(Location, new Size(WIDTH, HEIGHT)));
        }
    }
}