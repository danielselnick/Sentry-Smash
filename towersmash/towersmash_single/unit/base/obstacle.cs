using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics.Collisions;
using Microsoft.Xna.Framework.Graphics;

namespace towersmash
{
	public class obstacle: gunit
	{
        public obstacle()
            : base()
        {
            
        }

        public override void loadcontent(master master)
        {
            base.textureName = "tower";
            base.color = Color.GreenYellow;
            base.loadcontent(master);
            geom.OnCollision += this.oncollision;
            geom.CollisionGroup = 10;
        }

        protected bool oncollision(Geom geom1, Geom geom2, ContactList contact)
        {
            Iid id1 = (Iid)geom1.Tag;
            Iid id2 = (Iid)geom2.Tag;
            if (id1.UnitID == UnitID.obstacle && id2.UnitID == UnitID.player)
            {
                master.players[id2.NodeID].body.ApplyForce(this.rotationV2 * 4333333);
                master.players[id2.NodeID].addPush(4000f);
            }
            return false;
        }
	}
}
