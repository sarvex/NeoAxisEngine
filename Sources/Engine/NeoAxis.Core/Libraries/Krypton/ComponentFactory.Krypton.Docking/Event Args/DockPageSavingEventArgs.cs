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
using System.IO;
using System.Xml;
using System.Drawing;
using System.Diagnostics;
using Internal.ComponentFactory.Krypton.Navigator;

namespace Internal.ComponentFactory.Krypton.Docking
{
	/// <summary>
    /// Event data for saving docking page configuration.
	/// </summary>
    public class DockPageSavingEventArgs : DockGlobalSavingEventArgs
	{
		#region Instance Fields
        private KryptonPage _page;
		#endregion

		#region Identity
		/// <summary>
        /// Initialize a new instance of the DockPageSavingEventArgs class.
		/// </summary>
        /// <param name="manager">Reference to owning docking manager instance.</param>
        /// <param name="xmlWriter">Xml writer for persisting custom data.</param>
        /// <param name="page">Reference to page being saved.</param>
        public DockPageSavingEventArgs(KryptonDockingManager manager,
                                       XmlWriter xmlWriter,
                                       KryptonPage page)
            : base(manager, xmlWriter)
		{
            _page = page;
		}
		#endregion

		#region Public
        /// <summary>
        /// Gets the saving page reference.
        /// </summary>
        public KryptonPage Page
        {
            get { return _page; }
        }
        #endregion
	}
}

#endif