using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mymnastics
{
    internal class ProbabilityGame
    {
        private SpriteFont font;
        public bool backToMenu = false;
        private int[,] grid = new int[7, 7];
        private bool[,] flipped = new bool[5, 5];
        private int counter = 0;
        private int pointsCounter = 0;
        private bool win = false;
        private bool done = false;
        private int delay = 0;
        private int points = 1;
        private int gameCounter = 0;
        private List<Texture2D> cardList;
        Random random = new Random();
        Viewport viewport;

        public ProbabilityGame(SpriteFont font, Viewport viewport, List<Texture2D> cardList)
        {
            this.font = font;
            this.viewport = viewport;
            this.cardList = cardList;
            setFlipped();
            startGame();
        }

        private void setFlipped()
        {
            for(int y = 0; y < 5; y++)
            {
                for(int x = 0; x < 5; x++)
                {
                    flipped[y, x] = false;
                }
            }
        }

        private void startGame()
        {
            for(int r = 0; r < grid.GetLength(0) - 2; r++)
            {
                for(int c = 0; c < grid.GetLength(1) - 2; c++)
                {
                    grid[r,c] = (int)(random.Next(1, 4));
                }
            }

            while(true)
            {
                grid[(int)random.Next(0, 5), (int)random.Next(0, 5)] = 0;

                for(int r = 0; r < grid.GetLength(0) - 2; r++)
                {
                    for(int c = 0; c < grid.GetLength(1) - 2; c++)
                    {
                        if(grid[r,c] == 0)
                        {
                            counter++;
                        }
                    }
                }
                if (counter == 5) break;
                counter = 0;
            }

            for(int r = 0; r < grid.GetLength(0); r++)
            {
                for(int c = 0; c < grid.GetLength(1); c++)
                {
                    if(grid[r,c] == 2 || grid[r,c] == 3)
                    {
                        pointsCounter++;
                    }
                }
            }

            int bombCounter = 0;
            for(int r = 0; r < grid.GetLength(0) - 2; r++)
            {
                for(int c = 0; c < grid.GetLength(1) - 2; c++)
                {
                    if(grid[r, c] == 0)
                    {
                        bombCounter++;
                    }
                }
                grid[r, 5] = bombCounter;
                bombCounter = 0;
            }

            for(int c = 0; c < grid.GetLength(0) - 2; c++)
            {
                for(int r = 0; r < grid.GetLength(1) - 2; r++)
                {
                    if(grid[r,c] == 0)
                    {
                        bombCounter++;
                    }
                }
                grid[5, c] = bombCounter;
                bombCounter = 0;
            }

            int rowTotal = 0;
            for (int r = 0; r < grid.GetLength(0) - 2; r++)
            {
                for (int c = 0; c < grid.GetLength(1) - 2; c++)
                {
                    rowTotal += grid[r, c];
                }
                grid[r, 6] = rowTotal;
                rowTotal = 0;
            }

            for (int c = 0; c < grid.GetLength(0) - 2; c++)
            {
                for (int r = 0; r < grid.GetLength(1) - 2; r++)
                {
                    rowTotal += grid[r, c];
                }
                grid[6, c] = rowTotal;
                rowTotal = 0;
            }
        }

        private void checkButtons(TouchCollection touchCollection)
        {
            if(touchCollection.Count > 0 && touchCollection[0].State == TouchLocationState.Pressed && touchCollection[0].Position.Y <= 1008)
            {
                int x = (int)Math.Floor((double)touchCollection[0].Position.X / 144);
                int y = (int)Math.Floor((double)touchCollection[0].Position.Y / 144);
                if(x < 5 && y < 5) flipped[y, x] = true;

                if (grid[y, x] == 0)
                {
                    done = true;
                    delay = 120;
                }
                else
                {
                    points *= grid[y, x];
                }
            }
        }

        public void Update(TouchCollection touchCollection, GameTime gameTime)
        {
            if(!done) checkButtons(touchCollection);
            Console.WriteLine(pointsCounter + ", " + gameCounter);
            if(gameCounter >= pointsCounter)
            {
                done = true;
                win = true;
                delay = 120;
            }
            if (delay > 0) delay--;

            if(delay <= 0 && done)
            {
                grid = new int[7, 7];
                flipped = new bool[5, 5];
                counter = 0;
                pointsCounter = 0;
                win = false;
                done = false;
                delay = 0;
                points = 1;
                gameCounter = 0;
                setFlipped();
                startGame();
                backToMenu = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for(int y = 0; y < 7; y++)
            {
                for(int x = 0; x < 7; x++)
                {
                    if(x < 5 && y < 5)
                    {
                        if(flipped[y, x])
                        {
                            if(grid[y, x] == 0) spriteBatch.Draw(cardList[20], new Rectangle(x * 144, y * 144, 144, 144), Color.White);
                            else spriteBatch.Draw(cardList[grid[y, x]], new Rectangle(x*144, y*144, 144, 144), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(cardList[16], new Rectangle(x*144, y*144, 144, 144), Color.White);
                        }
                    }
                    else
                    {
                        if(x < 5 || y < 5)
                        spriteBatch.Draw(cardList[grid[y, x]], new Rectangle(x * 144, y * 144, 144, 144), Color.White);
                    }
                }
            }
            Vector2 size = font.MeasureString("Points: 00");
            Vector2 origin = size * 0.5f;
            spriteBatch.DrawString(font, "Points: " + points, new Vector2(viewport.Bounds.Center.X, viewport.Bounds.Center.Y + 600), Color.White, 0, origin, 1, SpriteEffects.None, 0);

            if(done && win)
            {
                Vector2 size2 = font.MeasureString("You win!");
                Vector2 origin2 = size * 0.5f;
                spriteBatch.DrawString(font, "You win!", new Vector2(viewport.Bounds.Center.X, viewport.Bounds.Center.Y + 400), Color.White, 0, origin, 1, SpriteEffects.None, 0);
            }
            if(done && !win)
            {
                Vector2 size2 = font.MeasureString("You lose");
                Vector2 origin2 = size * 0.5f;
                spriteBatch.DrawString(font, "You lose", new Vector2(viewport.Bounds.Center.X, viewport.Bounds.Center.Y + 400), Color.White, 0, origin, 1, SpriteEffects.None, 0);
            }
        }
    }
}