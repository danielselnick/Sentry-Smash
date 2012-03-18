using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics.Collisions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FarseerGames.FarseerPhysics;

namespace towersmash
{
    public class unitm<unitT>: iunit where unitT : gunit, new()
    {
        public spool<unitT> unitpool;
        public master master;
        private Iid poolID;
        public int playernumber { get; set; }
        public unitT this[int index]
        {
            get { return unitpool[index]; }
            set { unitpool[index] = value; }
        }

        public unitm(int numberofunits, UnitID unitID, master master)
        {
            poolID = new Iid(1, unitID);  
            unitpool = new spool<unitT>(numberofunits, unitID);
            this.master = master;
        }

        public virtual void loadcontent(master master)
        {            
            int j = unitpool.Capacity;
            for (int i = 0; i < j; i++)
            {
                unitpool[i].loadcontent(master);
            }
        }

        public virtual void update(GameTime gameTime)
        {
            foreach (unitT u in unitpool)
            {
                u.update(gameTime);
            }
            unitpool.Update();
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            foreach (unitT u in unitpool)
            {
                u.draw(spriteBatch);
            }
        }

        public void add(unitT unit)
        {
            unitT newunit = unitpool.Retrieve().Item;
            newunit.clone(unit);
            newunit.add(master.physics);
        }

        public void remove(Iid id)
        {
            if(id.UnitID != poolID.UnitID)
                throw new Exception("Wrong unit pool!");
            unitT unit = unitpool.Get(id.NodeID).Item;
            unit.remove(master.physics);
            unitpool.Remove(id.NodeID);
        }

        public void clone(gunit unit)
        {
            throw new NotImplementedException();
        }
        public Iid id
        {
            get
            {
                return poolID;
            }
            set
            {
                id = value;
            }
        }
    }
}
