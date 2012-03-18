/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Renderers
{
    using System;

    public class BlendModeNotSuportedException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlendModeNotSuportedException"/> class.
        /// </summary>
        public BlendModeNotSuportedException() : base() { }
    }
}