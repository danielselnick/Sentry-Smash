using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace towersmash
{
    public enum arenaposition
    {
        top,
        bottom,
        left,
        right
    }

    public class obstaclem: unitm<obstacle>
    {
        public obstaclem(Rectangle arenarect, master master): base(numberofobstacles, UnitID.obstacle, master)
        {
                       
        }
        static int numberofobstacles = 32;
        Rectangle arenarectangle;
        Random _random = new Random();
        float scale = .25f;
        int width;
        int height;

        public override void loadcontent(master master)
        {
            base.loadcontent(master);
            arenarectangle = pvp.arenarect;
            width = (int)(arenarectangle.Width * scale);
            height = (int)(arenarectangle.Height * scale); 
            for (int j = 0; j < 4; j++)
                for (int i = 0; i < 4; i++)
                    createobstacle((arenaposition)i);  
        }

        void createobstacle(arenaposition position)
        {
            obstacle newobstactle = base.unitpool.Retrieve().Item;
            Vector2 location = Vector2.Zero; Vector2 direction = Vector2.Zero;
            int xmin = arenarectangle.Left;
            int xmax = arenarectangle.Right;
            int ymin = arenarectangle.Top;
            int ymax = arenarectangle.Bottom;
            
            switch (position)
            {
                case arenaposition.bottom:
                    ymin = ymax - height;
                    break;
                case arenaposition.left:
                    xmax = xmin + width;
                    break;
                case arenaposition.right:
                    xmin = xmax - width;
                    break;
                case arenaposition.top:
                    ymax = ymin + height;
                    break;                    
            }
            location.X = _random.Next(xmin, xmax);
            location.Y = _random.Next(ymin, ymax);
            newobstactle.position = location;
            direction.X = _random.Next(-2, 2);
            direction.Y = _random.Next(-2, 2);
            newobstactle.rotationV2 = direction;
            newobstactle.add(master.physics);
        }
    }
}