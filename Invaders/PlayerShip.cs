using System;
using System.Drawing;

namespace Invaders
{
    public class PlayerShip
    {
        private const int MOVE_INTERVAL = 10;
        public bool Alive { get; set; } = true;
        public Point Location { get; private set; }
        public Rectangle Area
        {
            get
            {
                return new Rectangle(Location, imageSize);
            }
        }

        private Rectangle boundaries;
        private Size imageSize;

        public PlayerShip(Rectangle boundaries)
        {
            this.boundaries = boundaries;
            imageSize = Properties.Resources.player.Size;
            Location = new Point(boundaries.Width / 2, boundaries.Height - imageSize.Height);
        }

        public void Draw(Graphics g)
        {
            g.DrawImageUnscaled(Properties.Resources.player, Location);
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    if (Location.X - MOVE_INTERVAL < 0)
                    {
                        return;
                    }

                    Location = new Point(Location.X - MOVE_INTERVAL, Location.Y);
                    break;
                case Direction.Right:
                    if (Location.X + MOVE_INTERVAL + Area.Width > boundaries.Width)
                    {
                        return;
                    }

                    Location = new Point(Location.X + MOVE_INTERVAL, Location.Y);
                    break;
                default:
                    break;
            }
        }
    }
}