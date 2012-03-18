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
    /// Defines a Modifier which gradually changes the colour of a Particle over the course of its lifetime.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.ColourModifierTypeConverter, ProjectMercury.Design")]
#endif
    public sealed class ColourModifier : Modifier
    {
        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new ColourModifier
            {
                InitialColour = this.InitialColour,
                UltimateColour = this.UltimateColour
            };
        }

        /// <summary>
        /// The initial colour of Particles when they are released.
        /// </summary>
        public Vector3 InitialColour;

        /// <summary>
        /// The ultimate colour of Particles when they are retired.
        /// </summary>
        public Vector3 UltimateColour;

        /// <summary>
        /// Processes the specified Particle.
        /// </summary>
        /// <param name="dt">Elapsed time in whole and fractional seconds.</param>
        /// <param name="particle">The particle to be processed.</param>
        /// <param name="tag">The tag which has been attached to the Particle (or null).</param>
        public override unsafe void Process(float dt, Particle* particle, object tag)
        {
            particle->Colour.X = (this.InitialColour.X + ((this.UltimateColour.X - this.InitialColour.X) * particle->Age));
            particle->Colour.Y = (this.InitialColour.Y + ((this.UltimateColour.Y - this.InitialColour.Y) * particle->Age));
            particle->Colour.Z = (this.InitialColour.Z + ((this.UltimateColour.Z - this.InitialColour.Z) * particle->Age));
        }
    }
}
