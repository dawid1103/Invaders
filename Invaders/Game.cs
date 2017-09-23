using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Invaders
{
    public class Game
    {
        private const int INVADER_SIZE = 60;

        private int score = 0;
        private int livesLeft = 0;
        private int wave = 0;
        private int framesSkipped = 0;

        private Rectangle boundaries;
        private Random random;

        private Direction invaderDirection;
        private List<Invader> invaders = new List<Invader>();

        private PlayerShip playerShip;
        private List<Shot> invaderShots = new List<Shot>();
        private List<Shot> playerShots = new List<Shot>();
        private Stars stars = new Stars();

        public event EventHandler GameOver;

        public Game(Rectangle boundaries, Random random)
        {
            this.boundaries = boundaries;
            this.random = random;

            playerShip = new PlayerShip(boundaries);
            livesLeft = 3;
            NextWave();
        }

        public void FireShot()
        {
            if (playerShots.Count < 2)
            {
                playerShots.Add(new Shot(new Point(playerShip.Location.X + playerShip.Area.Size.Width / 2, playerShip.Location.Y), Direction.Up, boundaries));
            }
        }

        public void Go()
        {
            if (!playerShip.Alive)
            {
                return;
            }

            foreach (Shot shot in invaderShots.ToList())
            {
                if (!shot.Move())
                {
                    invaderShots.Remove(shot);
                }

            }

            foreach (Shot shot in playerShots.ToList())
            {
                if (!shot.Move())
                {
                    playerShots.Remove(shot);
                }
            }

            MoveInvaders();
            InvaderReturnFire();
            CheckForInvaderTakShot();
            CheckForPlayerTakShot();
            framesSkipped++;
        }

        public void MovePlayer(Direction direction)
        {
            if (playerShip.Alive)
            {
                playerShip.Move(direction);
            }
        }

        public void Draw(Graphics g, int animCell)
        {

            g.FillRectangle(Brushes.Black, boundaries);

            stars.Draw(g);

            foreach (Invader invader in invaders)
            {
                invader.Draw(g, animCell);
            }

            playerShip.Draw(g);

            foreach (Shot shot in invaderShots)
            {
                shot.Draw(g);
            }

            foreach (Shot shot in playerShots)
            {
                shot.Draw(g);
            }

            g.DrawString($"Wynik: {score}", new Font("Tahoma", 15, FontStyle.Bold), Brushes.Yellow, new Point(boundaries.X + 20, boundaries.Y + 20));
            g.DrawString($"Życia: {livesLeft}", new Font("Tahoma", 15, FontStyle.Bold), Brushes.Yellow, new Point(boundaries.Width - 120, boundaries.Y + 20));

            if (!playerShip.Alive)
            {
                g.DrawString($"GAME{Environment.NewLine}OVER", new Font("Tahoma", 100, FontStyle.Bold), Brushes.White, new Point(150, 80));
            }

        }

        public void Twinkle()
        {
            stars.Twinkle();
        }

        private void NextWave()
        {
            wave++;
            framesSkipped = 0;
            invaderDirection = Direction.Right;

            for (int i = 6; i > 0; i--)
            {
                for (int j = 1; j < 6; j++)
                {
                    invaders.Add(new Invader((InvaderType)j, new Point(INVADER_SIZE * i, INVADER_SIZE * j), 60 - (10 * j)));
                }
            }
        }

        private void MoveInvaders()
        {
            if (framesSkipped + wave > 6)
            {
                framesSkipped = 0;

                switch (invaderDirection)
                {
                    case Direction.Left:
                        if (invaders.Any(i => i.Location.X - 100 + i.Area.Width <= 0))
                        {
                            invaderDirection = Direction.Right;
                            InvaderMoveDown();
                        }
                        break;
                    case Direction.Right:
                        if (invaders.Any(i => i.Location.X >= boundaries.Width - 100))
                        {
                            invaderDirection = Direction.Left;
                            InvaderMoveDown();
                        }
                        break;
                }

                foreach (Invader invader in invaders)
                {
                    invader.Move(invaderDirection);
                }
            }
        }

        private void InvaderMoveDown()
        {
            foreach (Invader invader in invaders)
            {
                invader.Move(Direction.Down);
            }
        }

        private void InvaderReturnFire()
        {
            if (invaderShots.Count() < wave + 1 && random.Next(10) > 10 - wave)
            //if (true)
            {
                var groups = invaders.GroupBy(i => i.Location.X).OrderBy(k => k.Key);
                IGrouping<int, Invader> group = groups.ToArray()[random.Next(groups.Count())];
                Invader invader = group.OrderByDescending(i => i.Location.Y).First();

                invaderShots.Add(new Shot(new Point(invader.Area.X + invader.Area.Width / 2, invader.Area.Y), Direction.Down, boundaries));
            }
        }

        private void CheckForPlayerTakShot()
        {
            foreach (Shot shot in invaderShots)
            {
                if (playerShip.Area.Contains(shot.Location))
                {
                    livesLeft--;

                    if (livesLeft < 1)
                    {
                        playerShip.Alive = false;
                    }

                    return;
                }
            }
        }

        private void CheckForInvaderTakShot()
        {
            foreach (Shot shot in playerShots.ToList())
            {
                Invader deathInvader = invaders.FirstOrDefault(i => i.Area.Contains(shot.Location));

                if (deathInvader != null)
                {
                    playerShots.Remove(shot);
                    invaders.Remove(deathInvader);
                    score += deathInvader.Score;
                }
            }

            if (invaders.Count() == 0)
            {
                NextWave();
            }
        }
    }
}