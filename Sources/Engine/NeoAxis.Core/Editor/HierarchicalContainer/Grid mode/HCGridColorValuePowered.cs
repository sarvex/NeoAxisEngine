﻿#if !DEPLOY
// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Internal.ComponentFactory.Krypton.Toolkit;

namespace NeoAxis.Editor
{
	public partial class HCGridColorValuePowered : EUserControl, IHCColorValuePowered
	{
		public HCGridColorValuePowered()
		{
			InitializeComponent();

			textBox1.Location = new Point( 0, DpiHelper.Default.ScaleValue( 3 ) );
			textBox1.AutoSize = false;
			textBox1.Height = DpiHelper.Default.ScaleValue( 18 );

			previewButton.Location = new Point( previewButton.Location.X, DpiHelper.Default.ScaleValue( 3 ) );
		}

		public EngineTextBox TextBox
		{
			get { return textBox1; }
		}

		public HCColorPreviewButton PreviewButton
		{
			get { return previewButton; }
		}

		public KryptonTrackBar TrackBarPower
		{
			get { return trackBarPower; }
		}
	}
}

#endif