// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
using System;
using System.Collections.Generic;
using System.IO;
using NeoAxis;

namespace Project
{
	public class ScenesWindow : UIWindow
	{
		List<string> fullPaths = new List<string>();
		//static double savedScrollPosition;

		//

		protected override void OnEnabledInSimulation()
		{
			fullPaths.Clear();

			if( Components[ "Button Load" ] != null )
				( (UIButton)Components[ "Button Load" ] ).Click += ButtonLoad_Click;
			if( Components[ "Button Close" ] != null )
				( (UIButton)Components[ "Button Close" ] ).Click += ButtonClose_Click;

			var list = GetComponent<UIList>( "List" );
			if( list != null )
			{
				var files = VirtualDirectory.GetFiles( "", "*.scene", SearchOption.AllDirectories );

				var showOnlyFileNames = SystemSettings.MobileDevice;

				CollectionUtility.MergeSort( files, delegate ( string name1, string name2 )
				{
					var s1 = name1.Replace( "\\", " \\" ).Replace( "/", " /" );
					var s2 = name2.Replace( "\\", " \\" ).Replace( "/", " /" );
					if( showOnlyFileNames )
					{
						s1 = Path.GetFileName( s1 );
						s2 = Path.GetFileName( s2 );
					}
					return string.Compare( s1, s2 );
				} );

				foreach( var file in files )
				{
					fullPaths.Add( file );

					string itemText = showOnlyFileNames ? Path.GetFileName( file ) : file;
					list.Items.Add( itemText );

					if( PlayScreen.Instance != null && string.Compare( PlayScreen.Instance.PlayFileName, file, true ) == 0 )
						list.SelectedIndex = list.Items.Count - 1;
				}

				list.ItemMouseDoubleClick += List_ItemMouseDoubleClick;

				if( list.SelectedIndex != 0 )
					list.EnsureVisible( list.SelectedIndex );

				//// Apply saved scroll position of the list control.
				//if( list.SelectedIndex != 0 && list.GetScrollBar() != null )
				//	list.GetScrollBar().Value = savedScrollPosition;
			}

			list?.Focus();
		}

		protected override void OnDisabledInSimulation()
		{
			//// Save scroll position of the list control.
			//var list = GetComponent<UIList>( "List" );
			//if( list != null && list.GetScrollBar() != null )
			//	savedScrollPosition = list.GetScrollBar().Value;
		}

		void ButtonClose_Click( UIButton sender )
		{
			Dispose();
		}

		void ButtonLoad_Click( UIButton sender )
		{
			var list = GetComponent<UIList>( "List" );
			if( list != null && list.SelectedIndex != -1 )
			{
				var playFile = fullPaths[ list.SelectedIndex ];//list.SelectedItem;
				SimulationApp.PlayFile( playFile );
			}
		}

		private void List_ItemMouseDoubleClick( UIControl sender, EMouseButtons button, ref bool handled )
		{
			ButtonLoad_Click( null );
		}

		protected override bool OnKeyDown( KeyEvent e )
		{
			if( e.Key == EKeys.Escape )
			{
				Dispose();
				return true;
			}

			return base.OnKeyDown( e );
		}
	}
}