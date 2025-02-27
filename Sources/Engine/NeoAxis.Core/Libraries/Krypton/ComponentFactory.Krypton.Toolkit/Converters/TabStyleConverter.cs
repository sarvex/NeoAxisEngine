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

namespace Internal.ComponentFactory.Krypton.Toolkit
{
    /// <summary>
    /// Custom type converter so that TabStyle values appear as neat text at design time.
    /// </summary>
    internal class TabStyleConverter : StringLookupConverter
    {
        #region Static Fields
        private Pair[] _pairs = new Pair[] { new Pair(TabStyle.HighProfile,     "High Profile"),
                                             new Pair(TabStyle.StandardProfile, "Standard Profile"),
                                             new Pair(TabStyle.LowProfile,      "Low Profile"),
                                             new Pair(TabStyle.OneNote,         "OneNote"),
                                             new Pair(TabStyle.Dock,            "Dock"),
                                             new Pair(TabStyle.DockAutoHidden,  "Dock AutoHidden"),
                                             new Pair(TabStyle.Custom1,         "Custom1"),
                                             new Pair(TabStyle.Custom2,         "Custom2"),
                                             new Pair(TabStyle.Custom3,         "Custom3") };
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the TabStyleConverter clas.
        /// </summary>
        public TabStyleConverter()
            : base(typeof(TabStyle))
        {
        }
        #endregion

        #region Protected
        /// <summary>
        /// Gets an array of lookup pairs.
        /// </summary>
        protected override Pair[] Pairs 
        {
            get { return _pairs; }
        }
        #endregion
    }
}

#endif