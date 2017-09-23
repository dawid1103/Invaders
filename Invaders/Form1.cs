using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Invaders
{
    public partial class Form1 : Form
    {
        private int frame = 0;
        private int cell = 0;
        private Game game;
        private List<Keys> keysPressed = new List<Keys>();
        private bool gameOver = false;

        public Form1()
        {
            InitializeComponent();

            game = new Game(this.ClientRectangle, new Random());
            game.GameOver += Game_GameOver;
        }

        private void Game_GameOver(object sender, EventArgs e)
        {
            gameOver = true;
        }

        private void Animate()
        {
            frame++;
            if (frame >= 6)
                frame = 0;
            switch (frame)
            {
                case 0:
                    cell = 0;
                    break;
                case 1:
                    cell = 1;
                    break;
                case 2:
                    cell = 2;
                    break;
                case 3:
                    cell = 3;
                    break;
                case 4:
                    cell = 2;
                    break;
                case 5:
                    cell = 1;
                    break;
                default:
                    cell = 0;
                    break;
            }

            game.Twinkle();
        }

        private void animationTimer_Tick(object sender, EventArgs e)
        {
            Animate();
            this.Invalidate();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (gameOver)
                return;

            game.Go();

            foreach (Keys key in keysPressed)
            {
                if (key == Keys.Left)
                {
                    game.MovePlayer(Direction.Left);
                    return;
                }

                if (key == Keys.Right)
                {
                    game.MovePlayer(Direction.Right);
                    return;
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            game.Draw(e.Graphics, cell);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys pressedKey = e.KeyCode;

            if (pressedKey == Keys.Q)
            {
                Application.Exit();
            }

            if (gameOver)
            {
                if (pressedKey == Keys.S)
                {
                    //kod do ponownego uruchomienia i restatru zegarów
                }
            }

            if (pressedKey == Keys.Space)
            {
                game.FireShot();
            }

            if (keysPressed.Contains(pressedKey))
            {
                keysPressed.Remove(pressedKey);
            }

            keysPressed.Add(pressedKey);

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (keysPressed.Contains(e.KeyCode))
            {
                keysPressed.Remove(e.KeyCode);
            }
        }
    }
}
