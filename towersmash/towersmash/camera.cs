using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace towersmash
{
    public class camera
    {
        public Vector2 position;
        public float rotation;
        public Vector2 origin;
        public float scale;
        public Vector2 screencenter;
        public static Matrix transform;
        public static Matrix translate;
        public float movespeed= .2f;
        public float maxscale= .66f;
        public float minscale=.0001f;
        public float scalespeed = .2f;
        private float viewportheight;
        private float viewportwidth;
        public int offset;

        public camera(Game game, int yoffset) 
        {
            viewportwidth = game.GraphicsDevice.Viewport.Width;
            viewportheight = game.GraphicsDevice.Viewport.Height - yoffset;
            screencenter = new Vector2(viewportwidth / 2, viewportheight / 2);
            scale = 1;
            movespeed = .25f;
        }
        
        public void update(GameTime gametime, master master)
        {            
            //calculate the transform matrix
            transform = math.identity *
                //translate by negative position
                math.createtranslation(-position.X, -position.Y, 0) *
                math.createscale(scale) *
                //rotate about z axis
                //math.createrotationz(1.75f) *
                //translate by the origin amount
                math.createtranslation(viewportwidth / 2, viewportheight / 2, 0);
            translate = math.identity *
                //translate by negative position
                math.createtranslation(-position.X, -position.Y, 0) *
                //rotate about z axis
                //math.createrotationz(1.75f) *
                //translate by the origin amount
                math.createtranslation(viewportwidth / 2, viewportheight / 2, 0);
            //delta so the movement seems smoooth
            float delta = (float)gametime.ElapsedGameTime.TotalSeconds;
            //get a rectangle which contains all the players
            Rectangle players = master.playerspace();
            //pad the rectangle
            players.X -= 500;
            players.Y -= 500;
            players.Width += 1000;
            players.Height += 1000;
            position = math.smoothstep(position, new Vector2(players.Center.X, players.Center.Y), movespeed); 
             
            //create a scale based on the the size of the player rectangle in respect to the original viewport size
            float xscale = viewportwidth / players.Width;
            float yscale = viewportheight/ players.Height;
            //use the largest scaling value, the smaller value scale will still include all the picture from the larger scale
            float newscale = (xscale < yscale) ? xscale : yscale;
            //clamp the scale to the min or max
            newscale = math.clamp(newscale, minscale, maxscale);
            //smooth the scaling
            scale = math.smoothstep(scale, newscale, scalespeed);
            //currently not using the rotation, but it is in there for future reference
        }
    }
}
