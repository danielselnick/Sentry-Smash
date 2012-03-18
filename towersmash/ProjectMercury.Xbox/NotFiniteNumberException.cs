/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The exception that is thrown when a floating-point value is positive infinity, negative infinity, or
    /// Not-a-Number (NaN).
    /// </summary>
    [ComVisible(true)]
    public class NotFiniteNumberException : ArithmeticException
    {
        private const string EXCEPTION_MESSAGE = "Number encountered was not a finite quantity.";
        private const int    HRESULT           = -2146233048;

        /// <summary>
        /// Gets the invalid number that is a positive infinity, a negative infinity, or Not-a-Number (NaN).
        /// </summary>
        public double OffendingNumber { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFiniteNumberException"/> class.
        /// </summary>
        public NotFiniteNumberException() : base(EXCEPTION_MESSAGE)
        {
            base.HResult = HRESULT;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFiniteNumberException"/> class.
        /// </summary>
        /// <param name="offendingNumber">The offending number.</param>
        public NotFiniteNumberException(double offendingNumber) : this()
        {
            this.OffendingNumber = offendingNumber;
        }
    }
}