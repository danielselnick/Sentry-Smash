using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace towersmash
{
    public class w_machinegun: weapon
    {
        public w_machinegun()
            : base()
        { }

        public override void loadcontent(master master)
        {
            //Create the bullet type
            //base.bulletp = new bullet();
            //Have the weapon load itself
            base.loadcontent(master);
            //Custom values for the weapon
            base.modifyfirerate(2);
            //Custom values for the bullet
            base.w_bullet.body.Mass = .01f;
            base.w_bullet.modifyShootingSpeed(1); 
        }

        public override void clone(weapon weapon)
        {
            base.clone(weapon);
            //clone any values written for this weapon
        }
    }
}
