using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerGames.FarseerPhysics.Collisions;
using Microsoft.Xna.Framework.Content;

namespace towersmash
{
    public interface iunit
    {
        void loadcontent(master master);
        void update(GameTime gameTime);
        void draw(SpriteBatch spriteBatch);
        void clone(gunit unit);
        Iid id
        { get; set; }
        int playernumber { get; set; }
    }
}
