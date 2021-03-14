using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mymnastics
{
    internal class ReactionGame
    {
        private SpriteFont font;
        private Viewport viewport;
        private int trial = 0;
        private int timeUntil = 200;
        private double averageReaction = 0;
        private List<double> reactionTimes = new List<double>();
        private int delay = 0;
        private bool showingGreen = false;
        private bool gameOver = false;
        private int endDelay = 0;
        public bool backToMenu = false;
        private bool tapped = false;
        private float currentTime = 0f;
        private Texture2D greenSquare;

        public ReactionGame(SpriteFont font, Viewport viewport, Texture2D greenSquare)
        {
            this.font = font;
            this.viewport = viewport;
            this.greenSquare = greenSquare;
        }

        public void Init()
        {

        }
        public void Dispose()
        {

        }
        public void Update(TouchCollection touchCollection, GameTime gameTime)
        {
            if(timeUntil > 0)
            {
                timeUntil--;
            }
            if(timeUntil <= 0 && delay <= 0)
            {
                showingGreen = true;
            }
            if (showingGreen) currentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (delay > 0) delay--;

            if(delay <= 0 && tapped)
            {
                resetGame();
                tapped = false;
                currentTime = 0;
                trial++;
            }
            if(trial == 5)
            {
                gameOver = true;
                endDelay = 120;
                double sum = 0;
                foreach(double d in reactionTimes)
                {
                    sum += d;
                }
                averageReaction = sum / reactionTimes.Count;
                trial = 6;
            }
            if (endDelay > 0) endDelay--;

            if(endDelay == 0 && gameOver)
            {
                trial = 0;
                reactionTimes = new List<double>();
                showingGreen = false;
                delay = 0;
                timeUntil = 200;
                gameOver = false;
                endDelay = 0;
                tapped = false;
                currentTime = 0f;
                averageReaction = 0;
                backToMenu = true;
            }

            if(touchCollection.Count > 0 && trial < 5 && showingGreen)
            {
                if(touchCollection[0].State == TouchLocationState.Pressed)
                {
                    tapped = true;
                    showingGreen = false;
                    reactionTimes.Add((double)currentTime);
                    delay = 120;
                }
            }
        }
        private void resetGame()
        {
            Random random = new Random();
            timeUntil = random.Next(50, 250);
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            if(gameOver)
            {
                Vector2 size = font.MeasureString("Average Reaction Time: 0.00");
                Vector2 origin = size * 0.5f;
                spriteBatch.DrawString(font, "Average Reaction Time: " + averageReaction.ToString("0.00"), new Vector2(viewport.Bounds.Center.X, viewport.Bounds.Center.Y), Color.Black, 0, origin, 1, SpriteEffects.None, 0);
            }
            else
            {
                if(!showingGreen && !tapped)
                {
                    Vector2 size = font.MeasureString("Tap when the screen");
                    Vector2 origin = size * 0.5f;
                    spriteBatch.DrawString(font, "Tap when the screen\n turns green", new Vector2(viewport.Bounds.Center.X, viewport.Bounds.Center.Y), Color.Black, 0, origin, 1, SpriteEffects.None, 0);
                }
                else if(showingGreen && !tapped)
                {
                    spriteBatch.Draw(greenSquare, viewport.Bounds, Color.White);
                    Vector2 size = font.MeasureString("Tap Now!");
                    Vector2 origin = size * 0.5f;
                    spriteBatch.DrawString(font, "Tap Now!", new Vector2(viewport.Bounds.Center.X, viewport.Bounds.Center.Y), Color.Black, 0, origin, 1, SpriteEffects.None, 0);
                }
                else if(!showingGreen && tapped)
                {
                    Vector2 size = font.MeasureString("Your Time: 000.00 ms");
                    Vector2 origin = size * 0.5f;
                    spriteBatch.DrawString(font, "Your Time: " + reactionTimes[trial].ToString("0.00") + "ms", new Vector2(viewport.Bounds.Center.X, viewport.Bounds.Center.Y), Color.Black, 0, origin, 1, SpriteEffects.None, 0);
                }
            }
        }
    }
}