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
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace Internal.ComponentFactory.Krypton.Toolkit
{
	/// <summary>
	/// Redirects requests for context menu images from the ContextMenuImages instance.
	/// </summary>
    public class PaletteRedirectContextMenu : PaletteRedirect
    {
        #region Instance Fields
        private ContextMenuImages _images;
		#endregion

		#region Identity
		/// <summary>
        /// Initialize a new instance of the PaletteRedirectContextMenu class.
		/// </summary>
        /// <param name="target">Initial palette target for redirection.</param>
        /// <param name="images">Reference to source of context menu images.</param>
        public PaletteRedirectContextMenu(IPalette target,
                                          ContextMenuImages images)
            : base(target)
		{
            Debug.Assert(images != null);

            // Remember incoming target
            _images = images;
		}
		#endregion

        #region Images
        /// <summary>
        /// Gets a checked image appropriate for a context menu item.
        /// </summary>
        /// <returns>Appropriate image for drawing; otherwise null.</returns>
        public override Image GetContextMenuCheckedImage()
        {
            Image retImage = _images.Checked;

            // Not found, then inherit from target
            if (retImage == null)
                retImage = Target.GetContextMenuCheckedImage();

            return retImage;
        }

        /// <summary>
        /// Gets a indeterminate image appropriate for a context menu item.
        /// </summary>
        /// <returns>Appropriate image for drawing; otherwise null.</returns>
        public override Image GetContextMenuIndeterminateImage()
        {
            Image retImage = _images.Indeterminate;

            // Not found, then inherit from target
            if (retImage == null)
                retImage = Target.GetContextMenuIndeterminateImage();

            return retImage;
        }

        /// <summary>
        /// Gets an image indicating a sub-menu on a context menu item.
        /// </summary>
        /// <returns>Appropriate image for drawing; otherwise null.</returns>
        public override Image GetContextMenuSubMenuImage()
        {
            Image retImage = _images.SubMenu;

            // Not found, then inherit from target
            if (retImage == null)
                retImage = Target.GetContextMenuSubMenuImage();

            return retImage;
        }
        #endregion
    }
}

#endif