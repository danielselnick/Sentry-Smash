using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace towersmash
{
    public class t_machinegun: tower
    {
        public t_machinegun()
            : base()
        { }

        public override void loadcontent(master master)
        {
            //Define which type of weapon for this tower
            base.t_weapon = new w_machinegun();
            //Have the tower load itself
            base.loadcontent(master);
            //Custom values for the tower
            base.interval = 5000;
            //Custom values for the towers weapon  
        }
    }
}
