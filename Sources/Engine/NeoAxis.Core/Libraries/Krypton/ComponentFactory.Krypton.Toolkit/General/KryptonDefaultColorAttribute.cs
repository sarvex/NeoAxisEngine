#if !DEPLOY
// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2012. All rights reserved.
//	The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 17/267 Nepean Hwy, 
//  Seaford, Vic 3198, Australia and are supplied subject to licence terms.
// 
//
// *****************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;

namespace Internal.ComponentFactory.Krypton.Toolkit
{
    /// <summary>
    /// Create a default value attribute for color property.
    /// </summary>
    public sealed class KryptonDefaultColorAttribute : DefaultValueAttribute
    {
        #region Identity
        /// <summary>
        /// Initialize a new instance of the KryptonDefaultColorAttribute class.
        /// </summary>
        public KryptonDefaultColorAttribute()
            : base(Color.Empty)
        {
        }
        #endregion
    }
}

#endif