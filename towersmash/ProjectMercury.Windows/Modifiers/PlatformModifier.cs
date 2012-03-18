/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Modifiers
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a Modifier which freezes a particle when it comes into contact with a bounding box.
    /// </summary>
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.Modifiers.PlatformModifierTypeConverter, ProjectMercury.Design")]
#endif
    public sealed class PlatformModifier : Modifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformModifier"/> class.
        /// </summary>
        public PlatformModifier()
        {
            this.Platforms = new List<BoundingBox>();
        }

        /// <summary>
        /// The list of platforms.
        /// </summary>
        public List<BoundingBox> Platforms { get; set; }

        /// <summary>
        /// Returns a deep copy of the Modifier implementation.
        /// </summary>
        /// <returns></returns>
        public override Modifier DeepCopy()
        {
            PlatformModifier modifier = new PlatformModifier();

            modifier.Platforms.AddRange(this.Platforms);

            return modifier;
        }

        /// <summary>
        /// Processes the specified Particle.
        /// </summary>
        /// <param name="dt">Elapsed time in whole and fractional seconds.</param>
        /// <param name="particle">The particle to be processed.</param>
        /// <param name="tag">The tag which has been attached to the Particle (or null).</param>
        public override unsafe void Process(float dt, Particle* particle, object tag)
        {
            var position = new Vector3(particle->Position, 0);

            for (int i = 0; i < this.Platforms.Count; i++)
            {
                ContainmentType result;

                this.Platforms[i].Contains(ref position, out result);

                if (result == ContainmentType.Contains)
                {
                    particle->Momentum = Vector2.Zero;

                    return;
                }
            }
        }
    }
}
