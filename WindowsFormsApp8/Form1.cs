using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        public Form1()
        {
            InitializeComponent();
            new Settings();
          
            Gametimer.Interval = 1000 / Settings.Speed;
            Gametimer.Tick += UpdateScreen;
            Gametimer.Start();
            StartGame();
        }
        private void StartGame()
        {
            new Settings();
            Snake.Clear();
            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);

            label2.Text = Settings.Score.ToString();
            GenrateFood();
        }
        private void GenrateFood()
        {
            int maxXPos = pictureBox1.Width / Settings.Width;
            int maxYPos = pictureBox1.Height / Settings.Height;
            Random random = new Random();
            food = new Circle();
            food.X = random.Next(0, maxXPos);
            food.Y = random.Next(0, maxYPos);

        }
        public void UpdateScreen(object sender,EventArgs e)
        {
            if (Settings.GameOver == true)
            {
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                {
                    Settings.direction = Direction.Rigth;
                }
               else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Rigth)
                {
                    Settings.direction = Direction.Left;
                }
               else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                {
                    Settings.direction = Direction.Up;
                }
               else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                {
                    Settings.direction = Direction.Down;
                }
                MovePlayer();
            }
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            if (Settings.GameOver!=true)
            {
                Brush snakeColor;

                for (int i = 0; i < Snake.Count; i++) 
                {

                    if (i == 0) { snakeColor = Brushes.Black; }
                    else { snakeColor = Brushes.Green; }


                    //rysowanie wensza
                    canvas.FillEllipse(snakeColor,
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width,Settings.Height));

                    //rysowanie fooda
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                                      food.Y * Settings.Height,
                                      Settings.Width, Settings.Height));


                }
            }
            else
            {
                string gameOver = "Game Over \nYour final score is: " + Settings.Score +"\nPress enter to continue ";
                label3.Text = gameOver;
                label3.Visible = true;
                if (Settings.Score > Settings.MaxScore) { Settings.MaxScore = Settings.Score;}
                string maxScore = Settings.MaxScore.ToString();
                label5.Text = maxScore;
            }
        }

       private void MovePlayer()
        {
            for(int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Rigth:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }

                    int maxXPos = pictureBox1.Width / Settings.Width;
                    int maxYPos = pictureBox1.Height / Settings.Height;

                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }

                    for(int j = 1; j < Snake.Count; j++)
                    {
                        if(Snake[i].X==Snake[j].X &&
                            Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    if(Snake[0].X==food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }
        private void Die()
        {
            Settings.GameOver = true;
            
        }
        private void Eat()
        {
            Circle food = new Circle();
            

            Snake.Add(food);

            Settings.Score += Settings.Points;
            label2.Text = Settings.Score.ToString();
            GenrateFood();
        }

       
    }
}
 