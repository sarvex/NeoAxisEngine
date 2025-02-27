#if !DEPLOY
//// *****************************************************************************
//// 
////  © Component Factory Pty Ltd 2012. All rights reserved.
////	The software and associated documentation supplied hereunder are the 
////  proprietary information of Component Factory Pty Ltd, 17/267 Nepean Hwy, 
////  Seaford, Vic 3198, Australia and are supplied subject to licence terms.
//// 
////
//// *****************************************************************************

//using System;
//using System.Drawing;
//using System.Windows.Forms;
//using System.ComponentModel;
//using System.Diagnostics;

//namespace Internal.ComponentFactory.Krypton.Toolkit
//{
//    /// <summary>
//    /// Storage for grids palette settings.
//    /// </summary>
//    public class KryptonPaletteGrids : Storage
//    {
//        #region Instance Fields
//        private KryptonPaletteGrid _gridCommon;
//        private KryptonPaletteGrid _gridList;
//        private KryptonPaletteGrid _gridSheet;
//        private KryptonPaletteGrid _gridCustom1;
//        #endregion

//        #region Identity
//        /// <summary>
//        /// Initialize a new instance of the KryptonPaletteGrids class.
//        /// </summary>
//        /// <param name="redirector">Palette redirector for sourcing inherited values.</param>
//        /// <param name="needPaint">Delegate for notifying paint requests.</param>
//        internal KryptonPaletteGrids( PaletteRedirect redirector,
//                                     NeedPaintHandler needPaint )
//        {
//            Debug.Assert( redirector != null );

//            _gridCommon = new KryptonPaletteGrid( redirector, GridStyle.List, needPaint );
//            _gridList = new KryptonPaletteGrid( redirector, GridStyle.List, needPaint );
//            _gridSheet = new KryptonPaletteGrid( redirector, GridStyle.Sheet, needPaint );
//            _gridCustom1 = new KryptonPaletteGrid( redirector, GridStyle.Custom1, needPaint );

//            // Create redirectors for inheriting from style specific to style common
//            PaletteRedirectGrids redirectCommon = new PaletteRedirectGrids( redirector, _gridCommon );

//            // Ensure the specific styles inherit to the common grid style
//            _gridList.SetRedirector( redirectCommon );
//            _gridSheet.SetRedirector( redirectCommon );
//            _gridCustom1.SetRedirector( redirectCommon );
//        }
//        #endregion

//        #region IsDefault
//        /// <summary>
//        /// Gets a value indicating if all values are default.
//        /// </summary>
//        public override bool IsDefault
//        {
//            get
//            {
//                return _gridCommon.IsDefault &&
//                       _gridList.IsDefault &&
//                       _gridSheet.IsDefault &&
//                       _gridCustom1.IsDefault;
//            }
//        }
//        #endregion

//        #region PopulateFromBase
//        /// <summary>
//        /// Populate values from the base palette.
//        /// </summary>
//        /// <param name="common">Reference to common settings.</param>
//        public void PopulateFromBase( KryptonPaletteCommon common )
//        {
//            // Populate only the designated styles
//            _gridList.PopulateFromBase( common, GridStyle.List );
//            _gridSheet.PopulateFromBase( common, GridStyle.Sheet );
//        }
//        #endregion

//        #region GridCommon
//        /// <summary>
//        /// Gets access to the common grid appearance entries.
//        /// </summary>
//        [KryptonPersist]
//        [Category( "Visuals" )]
//        [Description( "Overrides for defining common grid appearance." )]
//        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
//        public KryptonPaletteGrid GridCommon
//        {
//            get { return _gridCommon; }
//        }

//        private bool ShouldSerializeGridCommon()
//        {
//            return !_gridCommon.IsDefault;
//        }
//        #endregion

//        #region GridList
//        /// <summary>
//        /// Gets access to the list grid appearance entries.
//        /// </summary>
//        [KryptonPersist]
//        [Category( "Visuals" )]
//        [Description( "Overrides for defining list grid appearance." )]
//        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
//        public KryptonPaletteGrid GridList
//        {
//            get { return _gridList; }
//        }

//        private bool ShouldSerializeGridList()
//        {
//            return !_gridList.IsDefault;
//        }
//        #endregion

//        #region GridSheet
//        /// <summary>
//        /// Gets access to the sheet grid appearance entries.
//        /// </summary>
//        [KryptonPersist]
//        [Category( "Visuals" )]
//        [Description( "Overrides for defining sheet grid appearance." )]
//        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
//        public KryptonPaletteGrid GridSheet
//        {
//            get { return _gridSheet; }
//        }

//        private bool ShouldSerializeGridSheet()
//        {
//            return !_gridSheet.IsDefault;
//        }
//        #endregion

//        #region GridCustom1
//        /// <summary>
//        /// Gets access to the first custom grid appearance entries.
//        /// </summary>
//        [KryptonPersist]
//        [Category( "Visuals" )]
//        [Description( "Overrides for defining the first custom grid appearance." )]
//        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
//        public KryptonPaletteGrid GridCustom1
//        {
//            get { return _gridCustom1; }
//        }

//        private bool ShouldSerializeGridCustom1()
//        {
//            return !_gridCustom1.IsDefault;
//        }
//        #endregion
//    }
//}

#endif