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
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Internal.ComponentFactory.Krypton.Navigator
{
	/// <summary>
	/// Details for an cancellable event that provides pages associated with a page dragging event.
	/// </summary>
	public class PageDragCancelEventArgs : CancelEventArgs
	{
		#region Instance Fields
        private Point _screenPoint;
        private Point _elementOffset;
        private Control _c;
        private KryptonPageCollection _pages;
        #endregion

		#region Identity
		/// <summary>
        /// Initialize a new instance of the PageDragCancelEventArgs class.
		/// </summary>
        /// <param name="elementOffset">Offset from the top left of the element.</param>
        /// <param name="screenPoint">Screen point of the mouse.</param>
        /// <param name="c">Control that started the drag operation.</param>
        /// <param name="pages">Array of event associated pages.</param>
        public PageDragCancelEventArgs(Point screenPoint,
                                       Point elementOffset,
                                       Control c,
                                       KryptonPage[] pages)
		{
            _screenPoint = screenPoint;
            _elementOffset = elementOffset;
            _c = c;
            _pages = new KryptonPageCollection();

            if (pages != null)
                _pages.AddRange(pages);
		}

        /// <summary>
        /// Initialize a new instance of the PageDragCancelEventArgs class.
        /// </summary>
        /// <param name="screenPoint">Screen point of the mouse.</param>
        /// <param name="elementOffset">Offset from the top left of the element.</param>
        /// <param name="c">Control that started the drag operation.</param>
        /// <param name="pages">Collection of event associated pages.</param>
        public PageDragCancelEventArgs(Point screenPoint,
                                       Point elementOffset,
                                       Control c,
                                       KryptonPageCollection pages)
        {
            _screenPoint = screenPoint;
            _elementOffset = elementOffset;
            _c = c;
            _pages = pages;
        }
        #endregion

        #region ScreenPoint
        /// <summary>
        /// Gets access to the associated screen point.
        /// </summary>
        public Point ScreenPoint
        {
            get { return _screenPoint; }
        }
        #endregion

        #region ElementOffset
        /// <summary>
        /// Gets access to the associated element offset.
        /// </summary>
        public Point ElementOffset
        {
            get { return _elementOffset; }
        }
        #endregion

        #region ElementOffset
        /// <summary>
        /// Gets access to the control that started the drag operation.
        /// </summary>
        public Control Control
        {
            get { return _c; }
        }
        #endregion

        #region Pages
        /// <summary>
        /// Gets access to the collection of pages.
        /// </summary>
        public KryptonPageCollection Pages
        {
            get { return _pages; }
        }
        #endregion
    }
}

#endif