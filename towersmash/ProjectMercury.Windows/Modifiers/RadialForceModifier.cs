﻿/*  
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
    /// Defines a Modifier which applies a force to a Particle when it enters a circular area.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.RadialForceModifierTypeConverter, ProjectMercury.Design")]
#endif
    public class RadialForceModifier : Modifier
    {
        /// <summary>
        /// Gets or sets the position of the force.
        /// </summary>
        public Vector2 Position;

        private float _radius;

        /// <summary>
        /// Gets or sets the radius of the force.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the specified value is negetive or zero.
        public float Radius
        {
            get { return this._radius; }
            set
            {
                Guard.ArgumentNotFinite("Radius", value);
                Guard.ArgumentLessThan("Radius", value, float.Epsilon);

                this._radius = value;

                this.SquareRadius = value * value;
            }
        }

        private float SquareRadius;

        /// <summary>
        /// Gets or sets the force vector.
        /// </summary>
        public Vector2 Force;

        /// <summary>
        /// Gets or sets the strength of the force.
        /// </summary>
        public float Strength;

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new RadialForceModifier
            {
                Position = this.Position,
                Radius = this.Radius,
                Force = this.Force,
                Strength = this.Strength
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
            Vector2 distance;

            // Calculate the distance between the Particle and the center of the force...
            distance.X = this.Position.X - particle->Position.X;
            distance.Y = this.Position.Y - particle->Position.Y;

            float squareDistance = ((distance.X * distance.X) + (distance.Y * distance.Y));

            // Check to see if the Particle is within range of the force...
            if (squareDistance < this.SquareRadius)
            {
                // Adjust the force vector based on the strength of the force and the time delta...
                float strengthDelta = this.Strength * dt;

                particle->Velocity.X += (this.Force.X * strengthDelta);
                particle->Velocity.Y += (this.Force.Y * strengthDelta);
            }
        }
    }
}