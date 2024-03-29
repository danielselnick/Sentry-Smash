﻿/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Modifiers
{
    using System.ComponentModel;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a Modifier which merges the colour of particles towards a single colour over their lifetime. Works best
    /// when Particles are being released with random colours, where you require the particles to have a uniform colour
    /// at the end of their lifetime.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.ColourMergeModifierTypeConverter, ProjectMercury.Design")]
#endif
    public sealed class ColourMergeModifier : Modifier
    {
        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new ColourMergeModifier
            {
                MergeColour = this.MergeColour
            };
        }

        /// <summary>
        /// The final colour of Particles when they are retired.
        /// </summary>
        public Vector3 MergeColour;

        /// <summary>
        /// Processes the specified Particle.
        /// </summary>
        /// <param name="dt">Elapsed time in whole and fractional seconds.</param>
        /// <param name="particle">The particle to be processed.</param>
        /// <param name="tag">The tag which has been attached to the Particle (or null).</param>
        public override unsafe void Process(float dt, Particle* particle, object tag)
        {
            float a = particle->Age * 0.07f,
                  aInv = 1f - a;

            particle->Colour.X = (particle->Colour.X * aInv) + (this.MergeColour.X * a);
            particle->Colour.Y = (particle->Colour.Y * aInv) + (this.MergeColour.Y * a);
            particle->Colour.Z = (particle->Colour.Z * aInv) + (this.MergeColour.Z * a);
        }
    }
}