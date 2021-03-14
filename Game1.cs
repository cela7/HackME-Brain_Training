using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Mymnastics
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TouchCollection touchCollection;
        GameTemplate gameTemplate;
        MenuManager menuManager;
        Viewport viewport;
        List<Texture2D> backgroundList;
        List<Texture2D> cardList;
        SpriteFont font;
        Texture2D circles;
        Texture2D circle2;
        Texture2D green;
        Song menu;
        Song voltorb;
        Song math;
        Song reaction;

        MathGame mathGame;
        ReactionGame reactionGame;
        ProbabilityGame probabilityGame;

        bool songUpdate = false;

        int GameState = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight | DisplayOrientation.PortraitDown;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferMultiSampling = true;
            TouchPanel.EnabledGestures = GestureType.HorizontalDrag | GestureType.FreeDrag | GestureType.Tap | GestureType.Hold | GestureType.DragComplete;
            
            menuManager = new MenuManager(GraphicsDevice.Viewport);
            backgroundList = new List<Texture2D>();
            cardList = new List<Texture2D>();
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Georgia");
            backgroundList.Add(Content.Load<Texture2D>("CheckeredBackgroundLrWm"));
            backgroundList.Add(Content.Load<Texture2D>("CheckeredBackgroundBbbWm"));
            backgroundList.Add(Content.Load<Texture2D>("CheckeredBackgroundOcgWm"));
            backgroundList.Add(Content.Load<Texture2D>("CheckeredBackgroundSoWm"));
            backgroundList.Add(Content.Load<Texture2D>("CheckeredBackgroundENDMEscreen"));

            cardList.Add(Content.Load<Texture2D>("0-1.png"));
            cardList.Add(Content.Load<Texture2D>("1-1.png"));
            cardList.Add(Content.Load<Texture2D>("2-1.png"));
            cardList.Add(Content.Load<Texture2D>("3-1.png"));
            cardList.Add(Content.Load<Texture2D>("4-1.png"));
            cardList.Add(Content.Load<Texture2D>("5-1.png"));
            cardList.Add(Content.Load<Texture2D>("6-1.png"));
            cardList.Add(Content.Load<Texture2D>("7-1.png"));
            cardList.Add(Content.Load<Texture2D>("8-1.png"));
            cardList.Add(Content.Load<Texture2D>("9-1.png"));
            cardList.Add(Content.Load<Texture2D>("10-1.png"));
            cardList.Add(Content.Load<Texture2D>("11-1.png"));
            cardList.Add(Content.Load<Texture2D>("12-1.png"));
            cardList.Add(Content.Load<Texture2D>("13-1.png"));
            cardList.Add(Content.Load<Texture2D>("14-1.png"));
            cardList.Add(Content.Load<Texture2D>("15-1.png"));
            cardList.Add(Content.Load<Texture2D>("card-1.png"));
            cardList.Add(Content.Load<Texture2D>("Card flip 1"));
            cardList.Add(Content.Load<Texture2D>("Card flip 2"));
            cardList.Add(Content.Load<Texture2D>("Card flip 3"));
            cardList.Add(Content.Load<Texture2D>("bomb"));

            circles = Content.Load<Texture2D>("circles");
            circle2 = Content.Load<Texture2D>("circle2");
            green = Content.Load<Texture2D>("GreenSquare");

            mathGame = new MathGame(font, GraphicsDevice.Viewport);
            reactionGame = new ReactionGame(font, GraphicsDevice.Viewport, green);
            probabilityGame = new ProbabilityGame(font, GraphicsDevice.Viewport, cardList);

            menu = Content.Load<Song>("Main Menu");
            math = Content.Load<Song>("Math");
            reaction = Content.Load<Song>("Reaction");
            voltorb = Content.Load<Song>("Voltorb");

            MediaPlayer.Play(menu);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            foreach(Texture2D t in backgroundList)
            {
                t.Dispose();
            }
            foreach (Texture2D t in cardList)
            {
                t.Dispose();
            }
            circles.Dispose();
            circle2.Dispose();
            green.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            touchCollection = TouchPanel.GetState();

            switch (GameState)
            {
                case 0:
                    menuManager.Update(touchCollection);
                    break;
                case 1:
                    mathGame.Update(touchCollection, gameTime);
                    if (mathGame.backToMenu)
                    {
                        GameState = 0;
                        mathGame.backToMenu = false;
                        MediaPlayer.Stop();
                        MediaPlayer.Play(menu);
                    }
                    break;
                case 2:
                    reactionGame.Update(touchCollection, gameTime);
                    if(reactionGame.backToMenu)
                    {
                        GameState = 0;
                        reactionGame.backToMenu = false;
                        MediaPlayer.Stop();
                        MediaPlayer.Play(menu);
                    }
                    break;
                case 3:
                    probabilityGame.Update(touchCollection, gameTime);
                    if (probabilityGame.backToMenu)
                    {
                        GameState = 0;
                        probabilityGame.backToMenu = false;
                        MediaPlayer.Stop();
                        MediaPlayer.Play(menu);
                    }
                    break;
                default:
                    Console.WriteLine("oops");
                    break;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            if(menuManager.needToStartGame != 0)
            {
                GameState = menuManager.needToStartGame;
                menuManager.needToStartGame = 0;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            switch(GameState)
            {
                case 0:
                    menuManager.Draw(spriteBatch, font, backgroundList, circles, circle2);
                    break;
                case 1:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    mathGame.Draw(graphics, spriteBatch);
                    break;
                case 2:
                    GraphicsDevice.Clear(Color.Red);
                    reactionGame.Draw(graphics, spriteBatch);
                    break;
                case 3:
                    GraphicsDevice.Clear(Color.Orange);
                    probabilityGame.Draw(spriteBatch);
                    break;
                default:
                    Console.WriteLine("oops");
                    break;
            }


            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
