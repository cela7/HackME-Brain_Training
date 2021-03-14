using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Timers;


namespace Mymnastics
{

    internal class MathGame
    {
        public static int amountRight = 0;
        public static int amountWrong = 0;
        private String answerLoc = "";
        private int answerx = 0;
        private int wanswerx = 0;
        private double x = 0;
        private double y = 0;
        private double x2 = 0;
        private double y2 = 0;
        private double m = 0;
        private double b = 0;
        private int Operator = 0;
        private String equation = "";
        bool answeredProblem = true;
        float currentTime = 0f;
        public String elapsedTime;
        public bool answered = true;
        private SpriteFont font;
        private double answer;
        private double wrongAnswer;
        private Viewport viewport;
        private Stopwatch stopWatch = new Stopwatch();
        private bool timeUp = false;
        private int waitTimer = 0;
        public bool backToMenu = false;

        public MathGame(SpriteFont font, Viewport viewport)
        {
            this.font = font;
            this.viewport = viewport;
        }
        

        private void generateProblem()
        {
            Random random = new Random();
            x = random.Next(1, 10);
            y = random.Next(1, 10);
            y2 = random.Next(1, 10);
            x2 = random.Next(1, 10);
            if (x == x2) x = x2 + 1;
            m = (y2 - y) / (x2 - x);
            b = y / (m * x);
            Operator = random.Next(0, 3);
            int t = random.Next(0, 10);
            if (Operator == 0)
            {
                answer = (int)(Math.Pow((double)x, 2) * (double)y);
                equation = "Solve the Equation: x^2 * y";
            }
            else if (Operator == 1)
            {
                answer = (int)(Math.Pow((double)x, 3) * Math.Pow((double)y, 2));
                equation = "Solve the Equation: x^3 * y^2";
            }
            else if (Operator == 2)
            {
                answer = m;
                equation = "Find the Slope of a Line\nfrom Two Points:";
            }
            else if (Operator == 3)
            {
                answer = (int)((double)random.Next(0, 50) * t + (double)random.Next(0, 50));
                equation = "?";
            }
            wrongAnswer = answer + random.Next(-20, 20);
            if (wrongAnswer == answer) wrongAnswer++;
            if(random.Next(0, 2) == 0)
            {
                answerx = viewport.Bounds.Center.X - 200;
                wanswerx = viewport.Bounds.Center.X + 200;
                answerLoc = "left";
            }
            else
            {
                answerx = viewport.Bounds.Center.X + 200;
                wanswerx = viewport.Bounds.Center.X - 200;
                answerLoc = "right";
            }
        }

        public void Init()
        {

        }
        public void Dispose()
        {

        }
        public void Update(TouchCollection touchCollection, GameTime gameTime)
        {
            if (answeredProblem)
            {
                generateProblem();
                answeredProblem = false;
            }
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(120 - currentTime <= 0 && !timeUp)
            {
                timeUp = true;
                waitTimer = 120;
            }

            if (waitTimer > 0) waitTimer--;
            if(waitTimer <= 0 && timeUp)
            {
                timeUp = false;
                currentTime = 0f;
                answeredProblem = true;
                amountRight = 0;
                amountWrong = 0;
                backToMenu = true;
            }

            if(touchCollection.Count > 0)
            {
                String dir = "";
                if(touchCollection[0].State == TouchLocationState.Released && touchCollection[0].Position.X > viewport.Bounds.Center.X - 200 - 200 && touchCollection[0].Position.X < viewport.Bounds.Center.X - 200 + 200 && touchCollection[0].Position.Y > viewport.Bounds.Center.Y + 400 - 200 && touchCollection[0].Position.Y < viewport.Bounds.Center.Y + 400 + 200)
                {
                    dir = "left";
                    checkAnswer(dir);
                }
                else if (touchCollection[0].State == TouchLocationState.Released && touchCollection[0].Position.X > viewport.Bounds.Center.X + 200 - 200 && touchCollection[0].Position.X < viewport.Bounds.Center.X + 200 + 200 && touchCollection[0].Position.Y > viewport.Bounds.Center.Y + 400 - 200 && touchCollection[0].Position.Y < viewport.Bounds.Center.Y + 400 + 200)
                {
                    dir = "right";
                    checkAnswer(dir);
                }
            }
        }
        private void checkAnswer(String dir)
        {
            if(dir == answerLoc)
            {
                amountRight++;
                generateProblem();
            }
            else
            {
                amountWrong++;
                generateProblem();
            }
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            if(!timeUp)
            {
                Vector2 esize = font.MeasureString(currentTime.ToString(equation));
                Vector2 eorigin = esize * 0.5f;
                spriteBatch.DrawString(font, answer.ToString("0.00"), new Vector2(answerx - 100, viewport.Bounds.Center.Y + 400), Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, wrongAnswer.ToString("0.00"), new Vector2(wanswerx - 100, viewport.Bounds.Center.Y + 400), Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, (120 - currentTime).ToString("0"), new Vector2(viewport.Bounds.Center.X - 100, viewport.Bounds.Top + 128), Color.Black, 0, new Vector2(0, 0) /*origin*/, 2, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, equation, new Vector2(viewport.Bounds.Center.X, viewport.Bounds.Center.Y - 200), Color.White, 0, eorigin, 1, SpriteEffects.None, 0);
                switch (Operator)
                {
                    case 0:
                        spriteBatch.DrawString(font, "x = " + x.ToString("0"), new Vector2(viewport.Bounds.Center.X - 200 - 100, viewport.Bounds.Center.Y + 100), Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, "y = " + y.ToString("0"), new Vector2(viewport.Bounds.Center.X + 200 - 100, viewport.Bounds.Center.Y + 100), Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                        break;
                    case 1:
                        spriteBatch.DrawString(font, "x = " + x.ToString("0"), new Vector2(viewport.Bounds.Center.X - 200 - 100, viewport.Bounds.Center.Y + 100), Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, "y = " + y.ToString("0"), new Vector2(viewport.Bounds.Center.X + 200 - 100, viewport.Bounds.Center.Y + 100), Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                        break;
                    case 2:
                        spriteBatch.DrawString(font, "x1 = " + x.ToString("0"), new Vector2(viewport.Bounds.Center.X - 200 - 100, viewport.Bounds.Center.Y + 100), Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, "y1 = " + y.ToString("0"), new Vector2(viewport.Bounds.Center.X + 200 - 100, viewport.Bounds.Center.Y + 100), Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, "x2 = " + x2.ToString("0"), new Vector2(viewport.Bounds.Center.X - 200 - 100, viewport.Bounds.Center.Y + 200), Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, "y2 = " + y2.ToString("0"), new Vector2(viewport.Bounds.Center.X + 200 - 100, viewport.Bounds.Center.Y + 200), Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                        break;
                    default:
                        Console.WriteLine("oops");
                        break;
                }
            }
            else
            {
                Vector2 size = font.MeasureString(currentTime.ToString("Time Up!"));
                Vector2 origin = size * 0.5f;
                Vector2 size2 = font.MeasureString(amountWrong.ToString("0") + " Answers Right");
                Vector2 origin2 = size * 0.5f;
                spriteBatch.DrawString(font, "Time Up!", new Vector2(viewport.Bounds.Center.X, viewport.Bounds.Center.Y), Color.Black, 0, origin, 2, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, amountRight + " Answers Right", new Vector2(viewport.Bounds.Center.X, viewport.Bounds.Center.Y + 100), Color.Black, 0, origin2, 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, amountWrong + " Answers Wrong", new Vector2(viewport.Bounds.Center.X, viewport.Bounds.Center.Y + 200), Color.Black, 0, origin2, 1, SpriteEffects.None, 0);
            }
        }
    }
}