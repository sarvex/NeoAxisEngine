#if !DEPLOY
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Internal.Aga.Controls.Tree.NodeControls
{
	public class DrawIconEventArgs : DrawEventArgs
	{
		//betauser
		//private ColorMatrix _iconMatrix;
		//public ColorMatrix IconColorMatrix
		//{
		//	get { return _iconMatrix; }
		//	set { _iconMatrix = value; }
		//}

		public DrawIconEventArgs(TreeNodeAdv node, NodeControl control, DrawContext context) : base(node, control, context) {}
	}
}

#endif