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

#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.ColourInterpolatorModifierTypeConverter, ProjectMercury.Design")]
#endif
    public class ColourInterpolatorModifier : Modifier
    {
        /// <summary>
        /// Gets or sets the initial colour.
        /// </summary>
        /// <value>The initial colour.</value>
        public Vector3 InitialColour { get; set; }

        /// <summary>
        /// Gets or sets the middle colour.
        /// </summary>
        /// <value>The middle colour.</value>
        public Vector3 MiddleColour { get; set; }

        private float _middlePosition;

        /// <summary>
        /// Gets or sets the middle colour position.
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

        /// <summary>
        /// Gets or sets the final colour.
        /// </summary>
        /// <value>The final colour.</value>
        public Vector3 FinalColour { get; set; }

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new ColourInterpolatorModifier
            {
                FinalColour = this.FinalColour,
                InitialColour = this.InitialColour,
                MiddleColour = this.MiddleColour,
                MiddlePosition = this.MiddlePosition,
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
            {
                particle->Colour.X = this.InitialColour.X + ((this.MiddleColour.X - this.InitialColour.X) * (particle->Age / this.MiddlePosition));
                particle->Colour.Y = this.InitialColour.Y + ((this.MiddleColour.Y - this.InitialColour.Y) * (particle->Age / this.MiddlePosition));
                particle->Colour.Z = this.InitialColour.Z + ((this.MiddleColour.Z - this.InitialColour.Z) * (particle->Age / this.MiddlePosition));
            }
            else
            {
                particle->Colour.X = this.MiddleColour.X + ((this.FinalColour.X - this.MiddleColour.X) * ((particle->Age - this.MiddlePosition) / (1f - this.MiddlePosition)));
                particle->Colour.Y = this.MiddleColour.Y + ((this.FinalColour.Y - this.MiddleColour.Y) * ((particle->Age - this.MiddlePosition) / (1f - this.MiddlePosition)));
                particle->Colour.Z = this.MiddleColour.Z + ((this.FinalColour.Z - this.MiddleColour.Z) * ((particle->Age - this.MiddlePosition) / (1f - this.MiddlePosition)));
            }
        }
    }
}