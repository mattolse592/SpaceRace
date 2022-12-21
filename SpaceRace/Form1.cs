//Matthew Olsen
//Dec 20 2022
//Space race game

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace SpaceRace
{
    public partial class Form1 : Form
    {
        Random randGen = new Random();
        int time = 800;

        Rectangle hero2 = new Rectangle(150, 770, 20, 30);
        Rectangle hero1 = new Rectangle(450, 770, 20, 30);
        int heroSpeed = 4;

        int hero1Score = 0;
        int hero2Score = 0;

        int randValue;

        List<Rectangle> balls = new List<Rectangle>();
        int ballSize = 10;
        List<int> ballSpeed = new List<int>();

        List<Rectangle> balls2 = new List<Rectangle>();
        int ballSize2 = 10;
        List<int> ballSpeed2 = new List<int>();

        bool upDown = false;
        bool downDown = false;
        bool wDown = false;
        bool sDown = false;

        SolidBrush orangeBrush = new SolidBrush(Color.Orange);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush redBrush = new SolidBrush(Color.Red);

        SoundPlayer crashPlayer = new SoundPlayer(Properties.Resources.crash);
        SoundPlayer gameOverPlayer = new SoundPlayer(Properties.Resources.gameOver);
        SoundPlayer scorePlayer = new SoundPlayer(Properties.Resources.score);
        SoundPlayer clickPlayer = new SoundPlayer(Properties.Resources.click);
        string gameState = "waiting";
        public Form1()
        {
            InitializeComponent();
        }
        public void GameSetup()
        {
            gameState = "running";

            titleLabel.Text = "";
            subtitleLabel.Text = "";

            gameLoop.Enabled = true;

            hero1.Y = 770;
            hero2.Y = 770;

            hero1Score = 0;
            hero2Score = 0;
            score1.Visible = true;
            score2.Visible = true;
            balls.Clear();

            time = 950;

            Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = true;
                    break;
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.Down:
                    downDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        clickPlayer.Play();
                        GameSetup();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        clickPlayer.Play();
                        this.Close();
                    }
                    break;
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Down:
                    downDown = false;
                    break;
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
            }
        }
        //game tick event 50 tps
        private void gameLoop_Tick(object sender, EventArgs e)
        {
            if (upDown == true && hero1.Y > 0 && time < 800)
            {
                hero1.Y -= heroSpeed;
            }
            if (downDown == true && hero1.Y < 770 && time < 800)
            {
                hero1.Y += heroSpeed;
            }
            if (wDown == true && hero2.Y > 0 && time < 800)
            {
                hero2.Y -= heroSpeed;
            }
            if (sDown == true && hero2.Y < 770 && time < 800)
            {
                hero2.Y += heroSpeed;
            }

            for (int i = 0; i < balls.Count; i++)
            {
                int x = balls[i].X + ballSpeed[i];
                balls[i] = new Rectangle(x, balls[i].Y, balls[i].Width, balls[i].Height);
            }
            for (int i = 0; i < balls2.Count; i++)
            {
                int x = balls2[i].X - ballSpeed2[i];
                balls2[i] = new Rectangle(x, balls2[i].Y, balls2[i].Width, balls2[i].Height);
            }

            randValue = randGen.Next(1, 101);
            ballSize = randGen.Next(10, 20);
            ballSize2 = randGen.Next(10, 20);

            if (randValue < 10)
            {
                ballSize = randGen.Next(5, 10);
                balls.Add(new Rectangle(0, randGen.Next(0, this.Height - 40), ballSize * 3, ballSize));
                ballSpeed.Add(randGen.Next(2, 6));
            }
            if (randValue > 90)
            {
                ballSize2 = randGen.Next(5, 10);
                balls2.Add(new Rectangle(650, randGen.Next(0, this.Height - 40), ballSize2 * 3, ballSize2));
                ballSpeed2.Add(randGen.Next(2, 6));
            }

            for (int i = 0; i < balls.Count; i++)
            {
                if (balls[i].X >= this.Width)
                {
                    balls.Remove(balls[i]);
                    ballSpeed.Remove(ballSpeed[i]);
                }
            }
            for (int i = 0; i < balls2.Count; i++)
            {
                if (balls2[i].X < 0)
                {
                    balls2.Remove(balls2[i]);
                    ballSpeed2.Remove(ballSpeed2[i]);
                }
            }

            for (int i = 0; i < balls.Count; i++)
            {
                if (balls[i].IntersectsWith(hero1))
                {
                    hero1.Y = 770;
                    crashPlayer.Play();
                }
                if (balls[i].IntersectsWith(hero2))
                {
                    hero2.Y = 770;
                    crashPlayer.Play();
                }
            }
            for (int i = 0; i < balls2.Count; i++)
            {
                if (balls2[i].IntersectsWith(hero1))
                {
                    hero1.Y = 770;
                    crashPlayer.Play();
                }
                if (balls2[i].IntersectsWith(hero2))
                {
                    hero2.Y = 770;
                    crashPlayer.Play();
                }
            }

            if (hero1.Y < 0)
            {
                hero1.Y = 770;
                hero1Score++;
                scorePlayer.Play();
            }
            if (hero2.Y < 0)
            {
                hero2.Y = 770;
                hero2Score++;
                scorePlayer.Play();
            }

            if (time < 1)
            {
                gameLoop.Enabled = false;
                gameOverPlayer.Play();
                gameState = "over";
            }

            time--;
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle timer = new Rectangle(298, 800 - time, 5, time);
            if (gameState == "waiting")
            {
                titleLabel.Text = "Space Race";
                subtitleLabel.Text = "Press Space to Start or Esc to Exit";
            }
            else if (gameState == "running")
            {
                e.Graphics.DrawImage(Properties.Resources.rocketShipLogo, hero1);
                e.Graphics.DrawImage(Properties.Resources.rocketShipLogo, hero2);

                if (time < 800 && time > 150)
                {
                    e.Graphics.FillRectangle(whiteBrush, timer);
                }
                else if (time < 150)
                {
                    e.Graphics.FillRectangle(redBrush, timer);
                }
                for (int i = 0; i < balls.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, balls[i]);
                }
                for (int i = 0; i < balls2.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, balls2[i]);
                }

                if (wDown == true)
                {
                    Rectangle flame1 = new Rectangle(hero2.X, hero2.Y + 20, 20, 20);
                    e.Graphics.DrawImage(Properties.Resources.flames, flame1);
                }
                if (upDown == true)
                {
                    Rectangle flame2 = new Rectangle(hero1.X, hero1.Y + 20, 20, 20);
                    e.Graphics.DrawImage(Properties.Resources.flames, flame2);
                }
                score2.Text = $"{hero1Score}";
                score1.Text = $"{hero2Score}";

            }
            else if (gameState == "over")
            {
                if (hero1Score > hero2Score)
                {
                    titleLabel.Text = "Player 2 wins!";
                }
                else if (hero2Score > hero1Score)
                {
                    titleLabel.Text = "Player 1 wins!";
                }
                else
                {
                    titleLabel.Text = "It's a tie!";
                }

                subtitleLabel.Text = "Press Space to Start or Esc to Exit";
            }
        }
    }
}
