/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Modifiers
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a collection of Modifiers.
    /// </summary>
    public class ModifierCollection : List<Modifier>
    {
        /// <summary>
        /// Returns a deep copy of the ModifierCollection.
        /// </summary>
        /// <returns>A deep copy of the ModifierCollection.</returns>
        public ModifierCollection DeepCopy()
        {
            ModifierCollection modifiers = new ModifierCollection();

            foreach (Modifier mod in this)
                modifiers.Add(mod.DeepCopy());

            return modifiers;
        }

        /// <summary>
        /// Causes all Modifiers in the collection to process the specified Particle.
        /// </summary>
        internal unsafe void RunProcessors(float dt, Particle* particle, object tag)
        {
            for (int i = 0; i < base.Count; i++)
                this[i].Process(dt, particle, tag);
        }
    }
}