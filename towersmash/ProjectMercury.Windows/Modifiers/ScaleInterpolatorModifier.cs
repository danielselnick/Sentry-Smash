/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Modifiers
{
    using System.ComponentModel;

#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.ScaleInterpolatorModifierTypeConverter, ProjectMercury.Design")]
#endif
    public class ScaleInterpolatorModifier : Modifier
    {
        private float _initialScale;

        /// <summary>
        /// Gets or sets the initial scale.
        /// </summary>
        public float InitialScale
        {
            get { return this._initialScale; }
            set
            {
                Guard.ArgumentLessThan("InitialScale", value, 0f);

                this._initialScale = value;
            }
        }

        private float _middleScale;

        /// <summary>
        /// Gets or sets the middle scale.
        /// </summary>
        public float MiddleScale
        {
            get { return this._middleScale; }
            set
            {
                Guard.ArgumentLessThan("MiddleScale", value, 0f);

                this._middleScale = value;
            }
        }

        private float _middlePosition;

        /// <summary>
        /// Gets or sets the middle scale position.
        /// </summary>
        public float MiddlePosition
        {
            get { return this._middlePosition; }
            set
            {
                Guard.ArgumentOutOfRange("MiddlePosition", value, 0f, 1f);

                this._middlePosition = value;
            }
        }

        private float _finalScale;

        /// <summary>
        /// Gets or sets the final scale.
        /// </summary>
        public float FinalScale
        {
            get { return this._finalScale; }
            set
            {
                Guard.ArgumentLessThan("FinalScale", value, 0f);

                this._finalScale = value;
            }
        }

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new ScaleInterpolatorModifier
            {
                InitialScale = this.InitialScale,
                MiddleScale = this.MiddleScale,
                MiddlePosition = this.MiddlePosition,
                FinalScale = this.FinalScale
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
                particle->Scale = this.InitialScale + ((this.MiddleScale - this.InitialScale) * (particle->Age / this.MiddlePosition));

            else
                particle->Scale = this.MiddleScale + ((this.FinalScale - this.MiddleScale) * ((particle->Age - this.MiddlePosition) / (1f - this.MiddlePosition)));
        }
    }
}