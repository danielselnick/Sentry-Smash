using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace towersmash
{
    public class tower: gunit
    {
        
        public weapon t_weapon;
        private Vector2 _target;
        public Vector2 target
        {
            get { return _target; }
            set
            {
                _target = value;
                rotationV2 = target - this.position;
            }
        }

        public Vector2 targetdirection
        {
            get { return _target; }
            set
            {
                _target = value;
                rotationV2 = target;
            }
        }

        public new int playernumber
        {
            get { return base.playernumber; }
            set
            {
                base.playernumber = value;
                t_weapon.playernumber = value;
            }
        }
            
        public tower()
            : base()
        {
            t_weapon = new weapon();
        }

        public override void clone(gunit unit)
        {
            tower towern = (tower)unit;
            //Clone the weapon
            t_weapon.clone(towern.t_weapon);
            base.clone(unit);
        }

        public override void loadcontent(master master)
        {
            base.textureName = "tower";
            base.loadcontent(master);
            t_weapon.loadcontent(master);
            t_weapon.location = this.geom.AABB.Position;
        }

        public override void update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.update(gameTime);
            t_weapon.update(gameTime, this.geom.AABB.Position);            
            //Logic to point at the nearest enemy player
            //Iterate through all the other players, and point at the closest one
            /*
            float distance = t_weapon.range;
            //used in the iterator
            foreach (player p in master.players.unitpool)
            {
                //if it doesn't belong to this player
                if (p.playernumber != base.playernumber)
                {
                    float newdistance = Vector2.Distance(p.position, this.position);
                    if (newdistance < distance)
                    {
                        target = p.position;
                        distance = newdistance;
                    }
                }
            }
            if (distance < t_weapon.range)
            {
                t_weapon.shoot(this.target);
            }
             */
            //logic for end of tower
            t_weapon.shoot(this.target);
            if (base.istimer)
            {
                    master.remove(this.id);
            }
        }
    }
}