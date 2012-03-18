using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace towersmash
{
    /// <summary>
    /// Class used to shoot a bullet
    /// Used moreso like a script
    /// </summary>
    public class weapon
    {
        public weapon()
        {
            w_bullet = new bullet();
        }
        /// <summary>
        /// the bullet definition which this weapon shoots
        /// </summary>
        protected bullet w_bullet;
        protected master master;
        protected Vector2 _location;
        /// <summary>
        /// The location of the weapon
        /// The object to which this weapon is equipped to must update the location
        /// </summary>
        public Vector2 location
        {
            get { return _location; }
            set
            {
                _location = value;
                //_location.X += this.w_bullet.geom.AABB.Width / 2;
                //_location.Y += this.w_bullet.geom.AABB.Height / 2;
            }
        }

        public Vector2 direction;
        public const double firerate = 150;
        private double _firerate = firerate;
        public double getFirerate()
        { return _firerate; }
        public void modifyfirerate(double modifier)
        { _firerate *= modifier; }

        private int _playernumber;
        public int playernumber
        {
            get { return _playernumber; }
            set
            {
                _playernumber = value;
                w_bullet.playernumber = value;
            }
        }
        public double timer = 0;
       
        public virtual void clone(weapon _weapon)
        {
            w_bullet.clone(_weapon.w_bullet);
            //Copy all values which are unique to this class
            this._firerate = _weapon._firerate;
            this.playernumber = _weapon.playernumber;
        }

        public void shoot(Vector2 direction)
        {
            if (isTimer)
            {
                this.direction = direction;
                master.shootbullet(location, direction, this.w_bullet);
            }
        }

        public virtual void loadcontent(master master)
        {
            this.master = master;            
            w_bullet.loadcontent(master);
        }

        public virtual void update(GameTime gameTime, Vector2 location)
        {
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;
            this.location = location;          
        }

        protected bool isTimer
        {
            get
            {
                if (timer > firerate)
                {
                    timer = 0;
                    return true;
                }
                else
                    return false;
            }
        }
    }   
}
