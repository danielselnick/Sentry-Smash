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
    /// Defines a Modifier which gradually changes the opacity of a Particle over its lifetime.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.OpacityModifierTypeConverter, ProjectMercury.Design")]
#endif
    public sealed class OpacityModifier : Modifier
    {
        private float _initial;

        /// <summary>
        /// Gets or sets the initial opacity of Particles as they are released.
        /// </summary>
        public float Initial
        {
            get { return this._initial; }
            set
            {
                Guard.ArgumentNotFinite("Initial", value);
                Guard.ArgumentLessThan("Initial", value, 0f);
                Guard.ArgumentGreaterThan("Initial", value, 1f);

                this._initial = value;
            }
        }

        private float _ultimate;

        /// <summary>
        /// Gets or sets the ultimate opacity of Particles as they are retired.
        /// </summary>
        public float Ultimate
        {
            get { return this._ultimate; }
            set
            {
                Guard.ArgumentNotFinite("Ultimate", value);
                Guard.ArgumentLessThan("Ultimate", value, 0f);
                Guard.ArgumentGreaterThan("Ultimate", value, 1f);

                this._ultimate = value;
            }
        }

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            return new OpacityModifier
            {
                Initial = this.Initial,
                Ultimate = this.Ultimate
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
            particle->Colour.W = (this.Initial + ((this.Ultimate - this.Initial) * particle->Age));
        }
    }
}