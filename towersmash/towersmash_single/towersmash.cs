using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using FarseerGames.FarseerPhysics;
using GameStateManagement;

namespace towersmash
{
    public enum playertype
    {
        bashy,
        shifty,
        tanky
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class towersmash : Game
    {
        #region Game Constants
        public static int numberofplayers = 0;
        public static readonly int maxnumplayers = 4;
        public static Texture2D controls;
        public static bool[] players;
        public static float screen_width = 1280;

        public static float screen_height = 720;

        public static playertype[] characters;
        #endregion 
        public static GraphicsDeviceManager graphics;
        ScreenManager screenmanager;
        public static void updatenumberofplayers()
        {
            int counter = 0;
            for (int i = 0; i < towersmash.maxnumplayers; i++)
            {
                if (towersmash.players[i])
                {
                    counter++;
                }
            }
            towersmash.numberofplayers = counter;
        }
        public towersmash()
        {
            players = new bool[maxnumplayers];
            for (int i = 0; i < maxnumplayers; i++)
            {
                players[i] = false;
            }
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            screenmanager = new ScreenManager(this);
            graphics.PreferredBackBufferWidth = (int)screen_width;
            graphics.PreferredBackBufferHeight = (int)screen_height;
            Components.Add(screenmanager);
            screenmanager.AddScreen(new BackgroundScreen(), null);
            screenmanager.AddScreen(new MainMenuScreen(), null);
            this.IsMouseVisible = true;
            characters = new playertype[4];            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            controls = this.Content.Load<Texture2D>("controls");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
