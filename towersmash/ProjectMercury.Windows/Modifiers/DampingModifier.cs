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
    /// Defines a Modifier which applies a damping force to a Particle over its lifetime.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.DampingModifierTypeConverter, ProjectMercury.Design")]
#endif
    public sealed class DampingModifier : Modifier
    {
        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new DampingModifier
            {
                DampingCoefficient = this.DampingCoefficient
            };
        }

        /// <summary>
        /// The damping coefficient.
        /// </summary>
        public float DampingCoefficient;

        /// <summary>
        /// Processes the specified Particle.
        /// </summary>
        /// <param name="dt">Elapsed time in whole and fractional seconds.</param>
        /// <param name="particle">The particle to be processed.</param>
        /// <param name="tag">The tag which has been attached to the Particle (or null).</param>
        public override unsafe void Process(float dt, Particle* particle, object tag)
        {
            float inverseCoefficientDelta = ((this.DampingCoefficient * dt) * -1f);

            particle->Velocity.X += particle->Momentum.X * inverseCoefficientDelta;
            particle->Velocity.Y += particle->Momentum.Y * inverseCoefficientDelta;
        }
    }
}
