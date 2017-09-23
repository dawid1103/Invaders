using System;
using System.Drawing;

namespace Invaders
{
    public class Invader
    {
        private const int HORIZONTAL_INTERVAL = 10;
        private const int VERTICAL_INTERVAL = 40;
        private Bitmap image;
        private Bitmap[] images;

        public Point Location { get; private set; }
        public InvaderType InvaderType { get; private set; }
        public Rectangle Area
        {
            get
            {
                return new Rectangle(Location, image.Size);
            }
        }

        public int Score { get; private set; }

        public Invader(InvaderType invaderType, Point location, int score)
        {
            InvaderType = invaderType;
            Location = location;
            Score = score;

            InitializeImages(invaderType);

            image = InvaderImage(0);
        }

        private Bitmap InvaderImage(int imageNumber)
        {
            return images[imageNumber];
        }

        public void Draw(Graphics g, int animCell)
        {
            image = InvaderImage(animCell);
            g.DrawImageUnscaled(image, Location);
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    Location = new Point(Location.X - HORIZONTAL_INTERVAL, Location.Y);
                    break;
                case Direction.Right:
                    Location = new Point(Location.X + HORIZONTAL_INTERVAL, Location.Y);
                    break;
                case Direction.Down:
                    Location = new Point(Location.X, Location.Y + VERTICAL_INTERVAL);
                    break;
                default:
                    break;
            }
        }

        private void InitializeImages(InvaderType invaderType)
        {
            images = new Bitmap[4];

            switch (invaderType)
            {
                case InvaderType.Bug:
                    images[0] = Properties.Resources.bug1;
                    images[1] = Properties.Resources.bug2;
                    images[2] = Properties.Resources.bug3;
                    images[3] = Properties.Resources.bug4;
                    break;
                case InvaderType.Saucer:
                    images[0] = Properties.Resources.flyingsaucer1;
                    images[1] = Properties.Resources.flyingsaucer2;
                    images[2] = Properties.Resources.flyingsaucer3;
                    images[3] = Properties.Resources.flyingsaucer4;
                    break;
                case InvaderType.Satellite:
                    images[0] = Properties.Resources.satellite1;
                    images[1] = Properties.Resources.satellite2;
                    images[2] = Properties.Resources.satellite3;
                    images[3] = Properties.Resources.satellite4;
                    break;
                case InvaderType.Spaceship:
                    images[0] = Properties.Resources.spaceship1;
                    images[1] = Properties.Resources.spaceship2;
                    images[2] = Properties.Resources.spaceship3;
                    images[3] = Properties.Resources.spaceship4;
                    break;
                case InvaderType.Star:
                    images[0] = Properties.Resources.star1;
                    images[1] = Properties.Resources.star2;
                    images[2] = Properties.Resources.star3;
                    images[3] = Properties.Resources.star4;
                    break;
                default:
                    break;
            }
        }
    }
}