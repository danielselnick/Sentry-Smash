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
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a Modifier which limits the velocity of Particles to a specified value.
    /// </summary>
    /// <remarks>For best results insert this Modifier after any other Modifiers which may alter
    /// the velocity of the Particle.</remarks>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.VelocityClampModifierTypeConverter, ProjectMercury.Design")]
#endif
    public class VelocityClampModifier : Modifier
    {
        private float _maximumVelocity;

        private float SquareMaximumVelocity;

        /// <summary>
        /// Gets or sets the maximum velocity of Particles..
        /// </summary>
        /// <value>The maximum velocity of Particles..</value>
        public float MaximumVelocity
        {
            get { return this._maximumVelocity; }
            set
            {
                Guard.ArgumentNotFinite("MaximumVelocity", value);
                Guard.ArgumentLessThan("MaximumVelocity", value, 0f);

                this._maximumVelocity = value;

                this.SquareMaximumVelocity = value * value;
            }
        }

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new VelocityClampModifier
            {
                MaximumVelocity = this.MaximumVelocity
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
            float squareVelocity = ((particle->Velocity.X * particle->Velocity.X) + (particle->Velocity.Y * particle->Velocity.Y));

            if (squareVelocity > this.SquareMaximumVelocity)
            {
                float velocity = Calculator.Sqrt(squareVelocity);

                particle->Velocity.X = (particle->Velocity.X / velocity) * this.MaximumVelocity;
                particle->Velocity.Y = (particle->Velocity.Y / velocity) * this.MaximumVelocity;
            }
        }
    }
}