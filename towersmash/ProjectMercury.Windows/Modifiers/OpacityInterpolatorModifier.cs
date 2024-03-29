﻿/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Modifiers
{
    using System.ComponentModel;

#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.OpacityInterpolatorModifierTypeConverter, ProjectMercury.Design")]
#endif
    public class OpacityInterpolatorModifier : Modifier
    {
        private float _initialOpacity;

        /// <summary>
        /// Gets or sets the initial opacity.
        /// </summary>
        /// <value>The initial opacity.</value>
        public float InitialOpacity
        {
            get { return this._initialOpacity; }
            set
            {
                Guard.ArgumentOutOfRange("InitialOpacity", value, 0f, 1f);

                this._initialOpacity = value;
            }
        }

        private float _middleOpacity;

        /// <summary>
        /// Gets or sets the middle opacity.
        /// </summary>
        /// <value>The middle opacity.</value>
        public float MiddleOpacity
        {
            get { return this._middleOpacity; }
            set
            {
                Guard.ArgumentOutOfRange("MiddleOpacity", value, 0f, 1f);

                this._middleOpacity = value;
            }
        }

        private float _middlePosition;

        /// <summary>
        /// Gets or sets the middle opacity position.
        /// </summary>
        /// <value>The middle position.</value>
        public float MiddlePosition
        {
            get { return this._middlePosition; }
            set
            {
                Guard.ArgumentOutOfRange("MiddlePosition", value, 0f, 1f);

                this._middlePosition = value;
            }
        }

        private float _finalOpacity;

        /// <summary>
        /// Gets or sets the final opacity.
        /// </summary>
        /// <value>The final opacity.</value>
        public float FinalOpacity
        {
            get { return this._finalOpacity; }
            set
            {
                Guard.ArgumentOutOfRange("FinalOpacity", value, 0f, 1f);

                this._finalOpacity = value;
            }
        }

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new OpacityInterpolatorModifier
            {
                InitialOpacity = this.InitialOpacity,
                MiddleOpacity = this.MiddleOpacity,
                MiddlePosition = this.MiddlePosition,
                FinalOpacity = this.FinalOpacity,
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
            if (particle->Age < this.MiddlePosition)
                particle->Colour.W = this.InitialOpacity + ((this.MiddleOpacity - this.InitialOpacity) * (particle->Age / this.MiddlePosition));

            else
                particle->Colour.W = this.MiddleOpacity + ((this.FinalOpacity - this.MiddleOpacity) * ((particle->Age - this.MiddlePosition) / (1f - this.MiddlePosition)));
        }
    }
}