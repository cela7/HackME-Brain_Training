using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;


namespace Mymnastics
{
    internal class MenuManager
    {
        private int location = 0;
        private int slidev = 0;
        private int slideDelay = 0;
        private int textOffset = 0;
        private int spriteN = 0;
        private int backDelay = 1;
        private bool dragNeedsReset = false;
        public int needToStartGame = 0;
        Viewport viewport;
        public MenuManager(Viewport viewport)
        {
            this.viewport = viewport;
        }

        public void Update(TouchCollection touchCollection)
        {
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();
                if (GestureType.HorizontalDrag == gs.GestureType && slideDelay <= 0 && !dragNeedsReset)
                {
                    dragNeedsReset = true;
                    bool flag = false;
                    if (gs.Delta.X > 0 && location > 0)
                    {
                        location--;
                        flag = true;
                    }
                    else if (gs.Delta.X < 0 && location < 4)
                    {
                        location++;
                        flag = true;
                    }
                    int dir = (int)(gs.Delta.X / Math.Abs(gs.Delta.X));
                    if(flag) slideScreen(dir);
                }
                if(GestureType.DragComplete == gs.GestureType)
                {
                    dragNeedsReset = false;
                }
                if(GestureType.Tap == gs.GestureType && location > 0)
                {
                    if(gs.Position.X > viewport.Bounds.Center.X - 128 && gs.Position.X < viewport.Bounds.Center.X + 128 && gs.Position.Y > viewport.Bounds.Center.Y + 200 - 64 && gs.Position.Y < viewport.Bounds.Center.Y + 200 + 64)
                    {
                        startGame(location);
                    }
                }
            }

            if(Math.Abs(textOffset) > 0)
            {
                textOffset = textOffset + slidev;
                if(Math.Abs(slidev) > 0) slidev = (int)((float)slidev / 1.25);
                if (slideDelay > 0) slideDelay--;
                if (Math.Abs(textOffset) < Math.Abs(slidev))
                {
                    textOffset = 0;
                    slideDelay = 0;
                }
            }
        }
        private void startGame(int game)
        {
            needToStartGame = game;
        }
        private void slideScreen(int dir)
        {
            slidev = 115 * dir;
            slideDelay = 30;
            textOffset = -500 * dir;
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, List<Texture2D> backgroundList, Texture2D circles, Texture2D circle2)
        {
            Vector2 size;
            Vector2 origin;
            Vector2 size2;
            Vector2 origin2;

            switch (location)
            {
                case 0:
                    spriteBatch.Draw(backgroundList[0], new Rectangle(0, 0, viewport.Width, viewport.Height), new Rectangle((spriteN - (int)Math.Floor((double)spriteN / 6) * 6) * 256, (int)Math.Floor((double)spriteN / 6)*256, 256, 256), Color.White);
                    size = font.MeasureString("Brain Train");
                    origin = size * 0.5f;
                    size2 = font.MeasureString("Swipe Left");
                    origin2 = size2 * 0.5f;
                    spriteBatch.DrawString(font, "Brain Train", new Vector2(viewport.Bounds.Center.X + textOffset, viewport.Bounds.Center.Y - 200), Color.Black, 0, origin, 1, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, "Swipe Left", new Vector2(viewport.Bounds.Center.X + textOffset, viewport.Bounds.Center.Y + 200), Color.Black, 0, origin2, 1, SpriteEffects.None, 0);
                    break;
                case 1:
                    spriteBatch.Draw(backgroundList[1], new Rectangle(0, 0, viewport.Width, viewport.Height), new Rectangle((spriteN - (int)Math.Floor((double)spriteN / 6) * 6) * 256, (int)Math.Floor((double)spriteN / 6) * 256, 256, 256), Color.White);
                    size = font.MeasureString("Math");
                    origin = size * 0.5f;
                    size2 = font.MeasureString("Play");
                    origin2 = size2 * 0.5f;
                    spriteBatch.DrawString(font, "Math", new Vector2(viewport.Bounds.Center.X + textOffset, viewport.Bounds.Center.Y - 200), Color.Black, 0, origin, 1, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, "Play", new Vector2(viewport.Bounds.Center.X + textOffset, viewport.Bounds.Center.Y + 200), Color.Black, 0, origin2, 1, SpriteEffects.None, 0);
                    break;
                case 2:
                    spriteBatch.Draw(backgroundList[2], new Rectangle(0, 0, viewport.Width, viewport.Height), new Rectangle((spriteN - (int)Math.Floor((double)spriteN / 6) * 6) * 256, (int)Math.Floor((double)spriteN / 6) * 256, 256, 256), Color.White);
                    size = font.MeasureString("Reaction");
                    origin = size * 0.5f;
                    size2 = font.MeasureString("Play");
                    origin2 = size2 * 0.5f;
                    spriteBatch.DrawString(font, "Reaction", new Vector2(viewport.Bounds.Center.X + textOffset, viewport.Bounds.Center.Y - 200), Color.Black, 0, origin, 1, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, "Play", new Vector2(viewport.Bounds.Center.X + textOffset, viewport.Bounds.Center.Y + 200), Color.Black, 0, origin2, 1, SpriteEffects.None, 0);
                    break;
                case 3:
                    spriteBatch.Draw(backgroundList[3], new Rectangle(0, 0, viewport.Width, viewport.Height), new Rectangle((spriteN - (int)Math.Floor((double)spriteN / 6) * 6) * 256, (int)Math.Floor((double)spriteN / 6) * 256, 256, 256), Color.White);
                    size = font.MeasureString("Probability");
                    origin = size * 0.5f;
                    size2 = font.MeasureString("Play");
                    origin2 = size2 * 0.5f;
                    spriteBatch.DrawString(font, "Probability", new Vector2(viewport.Bounds.Center.X + textOffset, viewport.Bounds.Center.Y - 200), Color.Black, 0, origin, 1, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, "Play", new Vector2(viewport.Bounds.Center.X + textOffset, viewport.Bounds.Center.Y + 200), Color.Black, 0, origin2, 1, SpriteEffects.None, 0);
                    break;
                case 4:
                    spriteBatch.Draw(backgroundList[4], new Rectangle(0, 0, viewport.Width, viewport.Height), new Rectangle((spriteN - (int)Math.Floor((double)spriteN / 6) * 6) * 256, (int)Math.Floor((double)spriteN / 6) * 256, 256, 256), Color.White);
                    size = font.MeasureString("About Us:");
                    origin = size * 0.5f;
                    size2 = font.MeasureString("Programmed by: ---");
                    origin2 = size2 * 0.5f;
                    Vector2 size3 = font.MeasureString("Art by: Benjamin Dodge");
                    Vector2 origin3 = size3 * 0.5f;
                    spriteBatch.DrawString(font, "About Us:", new Vector2(viewport.Bounds.Center.X + textOffset, viewport.Bounds.Center.Y - 200), Color.Black, 0, origin, 1, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, "Programmed by:\nConnor LaBonty, \nWilliam Jordan,\n and Josh Mayberry", new Vector2(viewport.Bounds.Center.X + textOffset, viewport.Bounds.Center.Y + 100), Color.Black, 0, origin2, 1, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, "Art by: Benjamin Dodge", new Vector2(viewport.Bounds.Center.X + textOffset, viewport.Bounds.Center.Y + 500), Color.Black, 0, origin3, 1, SpriteEffects.None, 0);
                    break;
                default:
                    break;
            }

            for(int i = 0; i < 5; i++)
            {
                int xoffset = (i - 2) * 64;
                if(i == location)
                {
                    spriteBatch.Draw(circle2, new Rectangle(viewport.Bounds.Center.X + xoffset - 5, viewport.Bounds.Bottom - 64, 32, 32), Color.White);
                }
                else
                {
                    spriteBatch.Draw(circles, new Rectangle(viewport.Bounds.Center.X + xoffset - 5, viewport.Bounds.Bottom - 64, 32, 32), Color.White);
                }
            }

            if (backDelay <= 0)
            {
                spriteN++;
                backDelay = 1;
            }
            else backDelay--;
            if (spriteN > 31) spriteN = 0;
        }
    }
}