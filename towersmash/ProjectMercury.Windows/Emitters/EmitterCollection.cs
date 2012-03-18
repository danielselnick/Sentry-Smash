/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Emitters
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class EmitterCollection : List<Emitter>
    {
        /// <summary>
        /// Returns a deep copy of the Emitter collection.
        /// </summary>
        /// <returns>A deep copy of the Emitter collection.</returns>
        public EmitterCollection DeepCopy()
        {
            EmitterCollection copy = new EmitterCollection();

            foreach (Emitter emitter in this)
                copy.Add(emitter.DeepCopy());

            return copy;
        }

        /// <summary>
        /// Gets the element with the specified name.
        /// </summary>
        /// <param name="name">The name of the Emitter to fetch.</param>
        /// <returns>The first Emitter whose name matches the specified name.</returns>
        public Emitter this[string name]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                    if (base[i].Name.Equals(name))
                        return base[i];

                throw new KeyNotFoundException();
            }
        }
    }
}