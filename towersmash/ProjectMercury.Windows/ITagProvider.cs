/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury
{
    /// <summary>
    /// Defines the interface for an object which provides tag data to Particles when they are relased.
    /// </summary>
    public interface ITagProvider
    {
        /// <summary>
        /// Generates a custom data tag for the specified Particle.
        /// </summary>
        object GetTag(ref Particle particle);

        /// <summary>
        /// Called when a custom data tag is no longer needed because the Particle has been retired.
        /// </summary>
        void DisposeTag(object tag);
    }
}