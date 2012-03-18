using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;

namespace towersmash
{
    public enum UnitID
    {
        player,
        tower,
        bullet,
        obstacle,
        unitm
    }

    public class Iid
    {
        public Iid(int nodeId, UnitID unitid)
        {
            _node_ID = nodeId;
            _unit_ID = unitid;
        }
        private int _node_ID;
        public int NodeID
        {
            get { return _node_ID; }
        }

        private UnitID _unit_ID;
        public UnitID UnitID
        {
            get { return _unit_ID; }
        }
    }
}
