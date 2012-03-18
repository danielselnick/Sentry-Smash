using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Dynamics;

namespace towersmash
{
    /// <summary>
    /// Clone physics objects since I don't want to put these into the physics library
    /// </summary>
    public static class cloner
    {
        public static void clonegeom(ref Geom newgeom, ref Geom oldgeom)
        {
            newgeom.Id = Geom.GetNextId();
            newgeom.RestitutionCoefficient = oldgeom.RestitutionCoefficient;
            newgeom.FrictionCoefficient = oldgeom.FrictionCoefficient;
            newgeom.GridCellSize = oldgeom.GridCellSize;
            newgeom.CollisionGroup = oldgeom.CollisionGroup;
            newgeom.CollisionEnabled = oldgeom.CollisionEnabled;
            newgeom.CollisionResponseEnabled = oldgeom.CollisionResponseEnabled;
            newgeom.CollisionCategories = oldgeom.CollisionCategories;
            newgeom.CollidesWith = oldgeom.CollidesWith;
            newgeom.SetVertices(oldgeom.LocalVertices);
            DistanceGrid.Instance.CreateDistanceGrid(newgeom);
        }

        public static void clonebody(ref Body newbody, ref Body oldbody)
        {
            newbody.Mass = oldbody.Mass;
            newbody.MomentOfInertia = oldbody.MomentOfInertia;
            newbody.LinearDragCoefficient = oldbody.LinearDragCoefficient;
            newbody.RotationalDragCoefficient = oldbody.RotationalDragCoefficient;
            newbody.IsQuadraticDragEnabled = oldbody.IsQuadraticDragEnabled;
            newbody.QuadraticDragCoefficient = oldbody.QuadraticDragCoefficient;
            newbody.Enabled = oldbody.Enabled;
            newbody.Tag = oldbody.Tag;
            newbody.IgnoreGravity = oldbody.IgnoreGravity;
            newbody.IsStatic = oldbody.IsStatic;
        }
    }
}
