using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerGames.FarseerPhysics.Collisions;

namespace towersmash
{
    public class pvp: GameplayScreen
    {
        public Texture2D arenatexture;
        protected static Rectangle _arenarect;
        public static Rectangle arenarect
        {
            get { return _arenarect; }
        }
        public AABB arenaaabb;
        public int numberofpickups = 3;
        float playeroffset = 200;
        float arenaheight = 7666;
        float arenawidth;
        public int playerslives = 5;
        simplegui gui;
        Texture2D texture;
        public pvp(ScreenManager screenm)
            : base(screenm)
        {
        }

        public override void LoadContent()
        {
            float screen_height = this.game.GraphicsDevice.Viewport.Height;
            screen_height = screen_height - (screen_height * simplegui.gui_size);
            float screen_width = this.game.GraphicsDevice.Viewport.Width;
            float aspectratio = screen_width / screen_height;
            arenawidth = arenaheight * 1f;
            
            //Create the arena
            Vector2 arenaposition = new Vector2(300, 300);
            _arenarect = new Rectangle(
                (int)arenaposition.X, (int)arenaposition.Y, (int)arenawidth, (int)arenaheight);
            Vector2 max = new Vector2
                (arenaposition.X + arenawidth, arenaposition.Y + arenaheight);
            arenaaabb = new AABB
                (ref arenaposition, ref max); 
            arenatexture = base.game.Content.Load<Texture2D>("grid");
            gui = new simplegui(this.game.GraphicsDevice.Viewport.Width, this.game.GraphicsDevice.Viewport.Height, this.master);
            
            base.LoadContent();
            gui.loadcontent();
            //calculateplayerpositions(arenaposition);
            calculateplayerpositions(_arenarect);
            
            //# of players lives determined on the level layer
            foreach(player p in master.players.unitpool)
            {
                p.lives = this.playerslives;
            }
            camera.scale = initialscale;
            texture = game.Content.Load<Texture2D>("default");
            calculateborders();
        }

        AABB[] borderaaabb;
        void calculateborders()
        {
            borders = new Rectangle[4];
            borderaaabb = new AABB[4];
            borders[0] = new Rectangle(_arenarect.Left, _arenarect.Top, border_size, _arenarect.Height);
            borders[1] = new Rectangle(_arenarect.Right - border_size, _arenarect.Top, border_size, _arenarect.Height);
            borders[2] = new Rectangle(_arenarect.Left, _arenarect.Top, _arenarect.Width, border_size);
            borders[3] = new Rectangle(_arenarect.Left, _arenarect.Bottom - 300, _arenarect.Width, border_size);
            for (int i = 0; i < 4; i++)
            {
                Vector2 min = Vector2.Zero;
                Vector2 max = Vector2.Zero;
                min.X = borders[i].Left;
                min.Y = borders[i].Top;
                max.X = borders[i].Right;
                max.Y = borders[i].Bottom;
                borderaaabb[i] = new AABB(ref min, ref max);
            }
        }

        private void calculateplayerpositions(Rectangle arenarect)
        {
            float xoffset = arenarect.Width * .25f;
            float yoffest = arenarect.Height * .25f;
            master.players[0].position = new Vector2(arenarect.Center.X - xoffset,
                arenarect.Center.Y - yoffest);
            master.players[1].position = new Vector2(arenarect.Center.X + xoffset,
                arenarect.Center.Y - yoffest);
            master.players[2].position = new Vector2(arenarect.Center.X - xoffset,
                arenarect.Center.Y + yoffest);
            master.players[3].position = new Vector2(arenarect.Center.X + xoffset,
                arenarect.Center.Y + yoffest);
        }

        /// <summary>
        /// Calculate where to initially put the players
        /// </summary>
        private void calculateplayerpositions(Vector2 arenaposition)
        {
            //top left corner
            master.players.unitpool[0].body.Position = new Vector2(
                arenaposition.X + playeroffset,
                arenaposition.Y + playeroffset);
            //top right corner
            master.players.unitpool[1].body.Position = new Vector2(
                arenaposition.X + arenawidth - playeroffset - master.players.unitpool[1].texture.Width,
                arenaposition.Y + playeroffset);
            //bottom left corner
            master.players.unitpool[2].body.Position = new Vector2(arenaposition.X + playeroffset,
                    arenaposition.Y + arenaheight - playeroffset - master.players.unitpool[2].texture.Height);
            //bottom right corner
            master.players.unitpool[3].body.Position = new Vector2(
                arenaposition.X + arenawidth - playeroffset - master.players.unitpool[3].texture.Width,
                arenaposition.Y + arenaheight - playeroffset - master.players.unitpool[3].texture.Height);
        }

        float initialscale = .00000001f;
        float scaleincrease = .0005f;
        float scalefinal = .1f;
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            foreach (player player in master.players.unitpool)
            {
                if (!AABB.Intersect(ref player.geom.AABB, ref arenaaabb))
                {
                    player.lives = player.lives - 1;
                    if (player.lives < 1)
                        master.remove(player.id);
                    else
                    {
                        master.triggerparticles("explosion", player.geom.Position);
                        //reset the players numbers
                        player.reset();
                        //reset player to the middle of the arena
                        player.position = new Vector2(_arenarect.Center.X, _arenarect.Center.Y);
                    }
                }
            }
            //Check win condition
            Win();
            updateborders();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (initialscale < scalefinal)
            {
                initialscale += scaleincrease;
                camera.scale = initialscale;
            }
        }

        private void updateborders()
        {
            
            foreach (player p in master.players.unitpool)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (AABB.Intersect(ref borderaaabb[i], ref p.geom.AABB))
                    {
                        p.addPush(4f);
                    }
                }
            }
        }

        private void Win()
        {
            //when there is only one player left on the arena
            //the game is over
            if (towersmash.numberofplayers < 2)
            {
                this.ExitScreen();
                int j=0;
                int max = towersmash.maxnumplayers;
                for (int i = 0; i < max; i++)
                {
                    if (towersmash.players[i])
                        j = i;
                }
                j++;
                MessageBoxScreen message = new MessageBoxScreen("Player " + j + " has won the game!");
                message.Accepted += new EventHandler<PlayerIndexEventArgs>(message_Accepted);
                message.Cancelled +=new EventHandler<PlayerIndexEventArgs>(message_Accepted);
                ScreenManager.AddScreen(message, null);
            }
        }

        void message_Accepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }

        Rectangle[] borders;
        int border_size = 1000;
        public override void Draw(GameTime gameTime)
        {
           // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.Black, 0, 0);            
            spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, camera.transform);
            spritebatch.Draw(arenatexture, _arenarect, Color.White);

            Color border_color = Color.Gray;
            //Left boundary
            spritebatch.Draw(texture, borders[0], border_color);
            
            //Right boundary
            spritebatch.Draw(texture, borders[1], border_color);
            
            //Top boundary
            spritebatch.Draw(texture, borders[2], border_color);
            
            //Bottom boundary
            spritebatch.Draw(texture, borders[3], border_color);
            
            base.Draw(gameTime);
            
            if (initialscale < scalefinal)
            
            {
                spritebatch.Begin();
                for (int i = 0; i < 4; i++)
                {
                    if (towersmash.players[i])
                    {
                        PlayerIndex playerindex = (PlayerIndex)i;
                        Vector2 position = Vector2.Zero;
                        switch (playerindex)
                        {
                            case PlayerIndex.One:
                                position.X = towersmash.screen_width * .1f;
                                position.Y = towersmash.screen_height * .1f;
                                break;
                            case PlayerIndex.Two:
                                position.X = towersmash.screen_width - towersmash.screen_width * .20f;
                                position.Y = towersmash.screen_height * .1f;
                                break;
                            case PlayerIndex.Three:
                                position.X = towersmash.screen_width * .1f;
                                position.Y = towersmash.screen_height - towersmash.screen_height * .20f;
                                break;
                            case PlayerIndex.Four:
                                position.X = towersmash.screen_width - towersmash.screen_width * .20f;
                                position.Y = towersmash.screen_height - towersmash.screen_height * .20f;
                                break;
                            default:
                                position = Vector2.One;
                                break;
                        }
                        spritebatch.DrawString(ScreenManager.Font, "Player: " + playerindex, position, Color.Yellow);
                    }
                }
                spritebatch.End();
            }
            else
                gui.draw(this.spritebatch);
        }
    }
}