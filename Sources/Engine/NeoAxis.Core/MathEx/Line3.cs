// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace NeoAxis
{
	//[TypeConverter( typeof( MathExGeneralTypeConverter<Line3> ) )]
	/// <summary>
	/// Represents a double precision line in three-dimensional space.
	/// </summary>
	[StructLayout( LayoutKind.Sequential )]
	public struct Line3
	{
		[Serialize]
		public Vector3 Start;
		[Serialize]
		public Vector3 End;

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Line3( Line3 source )
		{
			Start = source.Start;
			End = source.End;
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Line3( Vector3 start, Vector3 end )
		{
			this.Start = start;
			this.End = end;
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public override bool Equals( object obj )
		{
			return ( obj is Line3 && this == (Line3)obj );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public override int GetHashCode()
		{
			return ( Start.GetHashCode() ^ End.GetHashCode() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public static bool operator ==( Line3 v1, Line3 v2 )
		{
			return ( v1.Start == v2.Start && v1.End == v2.End );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public static bool operator !=( Line3 v1, Line3 v2 )
		{
			return ( v1.Start != v2.Start || v1.End != v2.End );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public bool Equals( Line3 v, double epsilon )
		{
			if( !Start.Equals( v.Start, epsilon ) )
				return false;
			if( !End.Equals( v.End, epsilon ) )
				return false;
			return true;
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		[AutoConvertType]
		public Line3F ToLineF()
		{
			Line3F result;
			result.Start = Start.ToVector3F();
			result.End = End.ToVector3F();
			return result;
		}

		//!!!!
		//[AutoConvertType]
		//Parse, ToString

	}
}
