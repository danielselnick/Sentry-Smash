/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury
{
    public enum BlendMode
    {
        /// <summary>
        /// No blending.
        /// </summary>
        None = 4,

        /// <summary>
        /// Additive blending.
        /// </summary>
        Add = 0,

        /// <summary>
        /// Alpha blending.
        /// </summary>
        Alpha = 1,

        /// <summary>
        /// Subtractive blending.
        /// </summary>
        Subtract = 2,

        /// <summary>
        /// Multiplicative blending.
        /// </summary>
        Multiply = 3
    }
}