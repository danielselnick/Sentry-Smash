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
    /// Defines a Modifier which constrains & deflects particles inside a rectangle.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.RectangleConstraintDeflectorTypeConverter, ProjectMercury.Design")]
#endif
    public sealed class RectangleConstraintDeflector : Modifier
    {
        /// <summary>
        /// Defines the position of the rectangle boundary constraint.
        /// </summary>
        public Vector2 Position;

        private float _width;

        /// <summary>
        /// Gets or sets the width of the rectangle deflector.
        /// </summary>
        /// <value>The width of the rectangle deflector.</value>
        public float Width
        {
            get { return this._width; }
            set
            {
                Guard.ArgumentNotFinite("Width", value);
                Guard.ArgumentLessThan("Width", value, 0f);

                this._width = value;
            }
        }

        private float _height;

        /// <summary>
        /// Gets or sets the height of the rectangle.
        /// </summary>
        /// <value>The height of the rectangle.</value>
        public float Height
        {
            get { return this._height; }
            set
            {
                Guard.ArgumentNotFinite("Height", value);
                Guard.ArgumentLessThan("Height", value, 0f);

                this._height = value;
            }
        }

        private VariableFloat _restitutionCoefficient;

        /// <summary>
        /// Gets or sets the restitution coefficient (bounce factor) of Particles when the hit the deflector.
        /// </summary>
        public VariableFloat RestitutionCoefficient
        {
            get { return this._restitutionCoefficient; }
            set { this._restitutionCoefficient = value; }
        }

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new RectangleConstraintDeflector
            {
                Height = this.Height,
                Position = this.Position,
                RestitutionCoefficient = this.RestitutionCoefficient,
                Width = this.Width
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
            float halfScale = particle->Scale * 0.5f;

            if (this.Constrain(ref particle->Position.X, this.Position.X + halfScale, (this.Position.X + this.Width) - halfScale))
                particle->Momentum.X *= (Calculator.Clamp(this.RestitutionCoefficient, 0f, 1f) * -1f);

            if (this.Constrain(ref particle->Position.Y, this.Position.Y + halfScale, (this.Position.Y + this.Height) - halfScale))
                particle->Momentum.Y *= (Calculator.Clamp(this.RestitutionCoefficient, 0f, 1f) * -1f);
        }

        /// <summary>
        /// Constrains the specified value within the specified range, and returns true if the value was
        /// constrained.
        /// </summary>
        /// <param name="value">The value to be constrained.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>True if the specified value was constrained, else false.</returns>
        private bool Constrain(ref float value, float min, float max)
        {
            if (value < min)
            {
                value = min;

                return true;
            }

            if (value > max)
            {
                value = max;

                return true;
            }

            return false;
        }
    }
}