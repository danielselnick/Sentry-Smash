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
    /// Defines a Modifier which applies a force to a Particle when it enters a rectangular area.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.RectangleForceModifierTypeConverter, ProjectMercury.Design")]
#endif
    public sealed class RectangleForceModifier : Modifier
    {
        /// <summary>
        /// Gets or sets the position of the centre of the rectangular force area.
        /// </summary>
        public Vector2 Position;

        private float HalfWidth;

        /// <summary>
        /// Gets or sets the width of the rectangular force area.
        /// </summary>
        public float Width
        {
            get { return this.HalfWidth + this.HalfWidth; }
            set
            {
                Guard.ArgumentNotFinite("Width", value);
                Guard.ArgumentLessThan("Width", value, 0f);

                this.HalfWidth = value * 0.5f;
            }
        }

        private float HalfHeight;

        /// <summary>
        /// Gets or sets the height of the rectangular force area.
        /// </summary>
        public float Height
        {
            get { return this.HalfHeight + this.HalfHeight; }
            set
            {
                Guard.ArgumentNotFinite("Height", value);
                Guard.ArgumentLessThan("Height", value, 0f);

                this.HalfHeight = value * 0.5f;
            }
        }

        /// <summary>
        /// Gets or sets the force vector.
        /// </summary>
        public Vector2 Force;

        /// <summary>
        /// Gets or sets the strength of the force.
        /// </summary>
        public float Strength;

        /// <summary>
        /// Gets the position of the left edge of the rectangle.
        /// </summary>
        public float Left
        {
            get { return this.Position.X - this.HalfWidth; }
        }

        /// <summary>
        /// Gets the position of the right edge of the rectangle.
        /// </summary>
        public float Right
        {
            get { return this.Position.X + this.HalfWidth; }
        }

        /// <summary>
        /// Gets the position of the top edge of the rectangle.
        /// </summary>
        public float Top
        {
            get { return this.Position.Y - this.HalfHeight; }
        }

        /// <summary>
        /// Gets the position of the bottom edge of the rectangle.
        /// </summary>
        public float Bottom
        {
            get { return this.Position.Y + this.HalfHeight; }
        }

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new RectangleForceModifier
            {
                Position = this.Position,
                Width = this.Width,
                Height = this.Height,
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
            if (particle->Position.X > this.Left)
                if (particle->Position.X < this.Right)
                    if (particle->Position.Y > this.Top)
                        if (particle->Position.Y < this.Bottom)
                        {
                            float deltaStrength = this.Strength * dt;

                            particle->Velocity.X += this.Force.X * deltaStrength;
                            particle->Velocity.Y += this.Force.Y * deltaStrength;
                        }
        }
    }
}