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
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using Internal.ComponentFactory.Krypton.Toolkit;

namespace Internal.ComponentFactory.Krypton.Navigator
{
	/// <summary>
    /// Implements the NavigatorMode.BarCheckButtonGroupOnly mode.
	/// </summary>
    internal class ViewBuilderBarCheckButtonGroupOnly : ViewBuilderBarItemBase
    {
		#region Public
		/// <summary>
		/// Construct the view appropriate for this builder.
		/// </summary>
		/// <param name="navigator">Reference to navigator instance.</param>
		/// <param name="manager">Reference to current manager.</param>
		/// <param name="redirector">Palette redirector.</param>
		public override void Construct(KryptonNavigator navigator, 
									   ViewManager manager,
									   PaletteRedirect redirector)
		{
			// Let base class perform common operations
			base.Construct(navigator, manager, redirector);
		}

        /// <summary>
        /// Gets a value indicating if the mode is a tab strip style mode.
        /// </summary>
        public override bool IsTabStripMode
        {
            get { return true; }
        }

        /// <summary>
        /// User has used the keyboard to select the currently selected page.
        /// </summary>
        public override void KeyPressedPageView()
        {
            // If there is a currently selected page
            if (Navigator.SelectedPage != null)
            {
                // Grab the view for the page
                INavCheckItem checkItem = _pageLookup[Navigator.SelectedPage];

                // If the item also has the focus
                if (checkItem.HasFocus)
                {
                    // Then perform the click action for the button
                    checkItem.PerformClick();
                }
            }
        }

        /// <summary>
        /// Destruct the previously created view.
        /// </summary>
        public override void Destruct()
        {
            // Let base class perform common operations
            base.Destruct();
        }
        #endregion

        #region Protected
        /// <summary>
        /// Create the view hierarchy for this view mode.
        /// </summary>
        protected override void CreateCheckItemView()
        {
            // Create the view element that lays out the check buttons
            _layoutBar = new ViewLayoutBar(Navigator.StateCommon.Bar,
                                           PaletteMetricInt.CheckButtonGap,
                                           Navigator.Bar.ItemSizing,
                                           Navigator.Bar.ItemAlignment,
                                           Navigator.Bar.BarMultiline,
                                           Navigator.Bar.ItemMinimumSize,
                                           Navigator.Bar.ItemMaximumSize,
                                           Navigator.Bar.BarMinimumHeight,
                                           false);

            // Create the scroll spacer that restricts display
            _layoutBarViewport = new ViewLayoutViewport(Navigator.StateCommon.Bar,
                                                        PaletteMetricPadding.BarPaddingOnly,
                                                        PaletteMetricInt.CheckButtonGap,
                                                        Navigator.Bar.BarOrientation,
                                                        Navigator.Bar.ItemAlignment,
                                                        Navigator.Bar.BarAnimation);
            _layoutBarViewport.Add(_layoutBar);

            // Create the button bar area docker
            _layoutBarDocker = new ViewLayoutDocker();
            _layoutBarDocker.Add(_layoutBarViewport, ViewDockStyle.Fill);

            // Add a separators for insetting items
            _layoutBarSeparatorFirst = new ViewLayoutSeparator(0);
            _layoutBarSeparatorLast = new ViewLayoutSeparator(0);
            _layoutBarDocker.Add(_layoutBarSeparatorFirst, ViewDockStyle.Left);
            _layoutBarDocker.Add(_layoutBarSeparatorLast, ViewDockStyle.Right);

            // Create the docker used to layout contents of main panel and fill with group
            _layoutPanelDocker = new ViewLayoutDocker();
            _layoutPanelDocker.Add(new ViewLayoutPageHide(Navigator), ViewDockStyle.Fill);
            _layoutPanelDocker.Add(_layoutBarDocker, ViewDockStyle.Top);

            // Create a canvas for containing the selected page and put old root inside it
            _drawGroup = new ViewDrawCanvas(Navigator.StateNormal.HeaderGroup.Back, Navigator.StateNormal.HeaderGroup.Border, VisualOrientation.Top);
            _drawGroup.Add(_layoutPanelDocker);
            _newRoot = _drawGroup;

            // Must call the base class to perform common actions
            base.CreateCheckItemView();
        }

        /// <summary>
        /// Create a manager for handling the button specifications.
        /// </summary>
        protected override void CreateButtonSpecManager()
        {
            // Let base class create the button spec manager
            base.CreateButtonSpecManager();

            // Modify the way that button specs are remapped
            ButtonSpecNavManagerLayoutBar barManager = (ButtonSpecNavManagerLayoutBar)_buttonManager;

            // Remap the normal color onto the button text
            barManager.RemapTarget = ButtonSpecNavRemap.ButtonSpecRemapTarget.ButtonStandalone;
        }
        #endregion
    }
}

#endif