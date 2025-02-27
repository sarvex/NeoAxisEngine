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
using System.Drawing;
using System.Diagnostics;
using Internal.ComponentFactory.Krypton.Toolkit;
using Internal.ComponentFactory.Krypton.Workspace;

namespace Internal.ComponentFactory.Krypton.Docking
{
	/// <summary>
    /// Event arguments for events that need to provide a unique name but can be cancelled.
	/// </summary>
    public class CancelUniqueNameEventArgs : UniqueNameEventArgs
	{
		#region Instance Fields
        private bool _cancel;
		#endregion

		#region Identity
		/// <summary>
        /// Initialize a new instance of the CancelUniqueNameEventArgs class.
		/// </summary>
        /// <param name="uniqueName">Unique name of page.</param>
        /// <param name="cancel">Initial value for the cancel property.</param>
        public CancelUniqueNameEventArgs(string uniqueName, bool cancel)
            : base(uniqueName)
		{
            _cancel = cancel;
		}
        #endregion

		#region Public
        /// <summary>
        /// Gets and sets a value indicating if the event action should be cancelled.
        /// </summary>
        public bool Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }
        #endregion
	}
}

#endif