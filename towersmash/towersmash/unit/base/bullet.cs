using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics.Collisions;
using Microsoft.Xna.Framework;

namespace towersmash
{
    public class bullet: gunit
    {
        public const float shootingspeed = 1024;
        private float _shootingspeed = shootingspeed;
        public float getShootingSpeed()
        { return _shootingspeed; }
        public void modifyShootingSpeed(float shootingspeedmodifier)
        { _shootingspeed *= shootingspeedmodifier; }

        public string shootingsound = "bulletsound";
        public Vector2 startinglocation = Vector2.Zero;

        public const float range = 2000;
        private float _range = range;
        public float getRange()
        {            return _range;        }
        public void modifyRange(float rangemodifier)
        {            _range *= rangemodifier;        }
        public bullet()
        { }
        public override void clone(gunit unit)
        {
            bullet _bullet = (bullet)unit;
            _shootingspeed = _bullet._shootingspeed;
            base.clone(unit);
        }

        public override void update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.update(gameTime);
            if (Vector2.Distance(startinglocation, position) > range)
                master.remove(this.id);
        }

        public override void loadcontent(master master)
        {
            base.textureName = "bullet";
            base.loadcontent(master);
            base.texture = master.texturebullet;
            this.geom.OnCollision += oncollide;
            this.body.LinearDragCoefficient = .00001f;
            this.body.Mass = .001f;
        }

        public bool oncollide(Geom geom1, Geom geom2, ContactList contactlist)
        {
            //remove itself
            master.remove(this.id);
            return false;
        }
    }
}
