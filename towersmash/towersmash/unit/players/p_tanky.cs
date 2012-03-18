#region Copyright (c) 2009 Daniel A. Selnick
//This file, p_tank.cs shall adhere to the following license found here at:
//http://creativecommons.org/licenses/by-nc-nd/3.0/us/
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace towersmash
{
    public class p_tanky: player
    {
        public p_tanky(Iid id)
            : base()        
        {
            base.id = id;
        }
    }
}
