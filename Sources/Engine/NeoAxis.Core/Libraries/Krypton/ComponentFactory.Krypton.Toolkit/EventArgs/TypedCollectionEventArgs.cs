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
using System.Diagnostics;

namespace Internal.ComponentFactory.Krypton.Toolkit
{
    /// <summary>
    /// Delegate used for hooking into TypedCollection events.
    /// </summary>
    /// <typeparam name="T">Type of the item inside the TypedCollection.</typeparam>
    public delegate void TypedHandler<T>(object sender, TypedCollectionEventArgs<T> e)  where T : class;

	/// <summary>
	/// Details for typed collection related events.
	/// </summary>
	public class TypedCollectionEventArgs<T> : EventArgs where T : class
	{
		#region Instance Fields
		private T _item;
		private int _index;
		#endregion

		#region Identity
		/// <summary>
        /// Initialize a new instance of the TypedCollectionEventArgs class.
		/// </summary>
        /// <param name="item">Item effected by event.</param>
		/// <param name="index">Index of page in the owning collection.</param>
        public TypedCollectionEventArgs(T item, int index)
		{
			// Remember parameter details
            _item = item;
			_index = index;
		}
		#endregion

		#region Public
		/// <summary>
		/// Gets the item associated with the event.
		/// </summary>
        public T Item
		{
            get { return _item; }
		}

		/// <summary>
		/// Gets the index of the item associated with the event.
		/// </summary>
		public int Index
		{
			get { return _index; }
		}
		#endregion
	}
}

#endif