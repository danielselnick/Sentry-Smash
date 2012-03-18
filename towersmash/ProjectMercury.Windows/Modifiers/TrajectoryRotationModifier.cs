/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Modifiers
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Defines a Modifier which adjusts the rotation of a Particle to follow its trajectory.
    /// </summary>
    /// <remarks>Ideally this modifier should be added *after* any other physics modifiers.</remarks>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.TrajectoryRotationModifierTypeConverter, ProjectMercury.Design")]
#endif
    public sealed class TrajectoryRotationModifier : Modifier
    {
        /// <summary>
        /// The rotation offset to add to the calculated trajectory rotation.
        /// </summary>
        public float RotationOffset;

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new TrajectoryRotationModifier
            {
                RotationOffset = this.RotationOffset
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
            float rads = Calculator.Atan2(particle->Momentum.Y, particle->Momentum.X);

            particle->Rotation = rads + this.RotationOffset;
        }
    }
}