#if !DEPLOY
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Internal.Aga.Controls.Tree
{
	internal interface IRowLayout
	{
		int PreferredRowHeight
		{
			get;
			set;
		}

		int VisiblePageRowCount
		{
			get;
		}

		int CurrentPageSize
		{
			get;
		}

		Rectangle GetRowBounds(int rowNo);

		int GetRowAt(Point point);

		int GetFirstRow(int lastPageRow);

		void ClearCache();
	}
}

#endif