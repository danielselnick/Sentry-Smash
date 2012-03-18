/*  
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
    /// Defines a Modifier which adjusts the scale of a Particle over its lifetime.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.ScaleModifierTypeConverter, ProjectMercury.Design")]
#endif
    public class ScaleModifier : Modifier
    {
        /// <summary>
        /// The initial scale of the Particle in pixels.
        /// </summary>
        public float InitialScale;

        /// <summary>
        /// The ultimate scale of the Particle in pixels.
        /// </summary>
        public float UltimateScale;

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new ScaleModifier
            {
                InitialScale = this.InitialScale,
                UltimateScale = this.UltimateScale
            };
        }

        /// <summary>
        /// Processes the specified Particle.
        /// </summary>
        /// <param name="dt">Elapsed time in whole and fractional seconds.</param>
        /// <param name="particle">The particle to be processed.</param>
        /// <param name="tag">The tag which has been attached to the Particle (or null).</param>
        public override unsafe void Process(float dt, Particle* particle, object tag)
        {
            particle->Scale = (this.InitialScale + ((this.UltimateScale - this.InitialScale) * particle->Age));
        }
    }
}