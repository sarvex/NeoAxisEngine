﻿// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace NeoAxis
{
	/// <summary>
	/// Regulator switch in the scene.
	/// </summary>
	[AddToResourcesWindow( @"Base\Game framework\Regulator Switch", -7998 )]
	[NewObjectDefaultName( "Regulator Switch" )]
	public class RegulatorSwitchInSpace : MeshInSpace, InteractiveObject
	{
		bool changing;
		bool changingForward;

		/////////////////////////////////////////

		/// <summary>
		/// Specifies the range of possible values.
		/// </summary>
		[DefaultValue( "0 1" )]
		public Reference<Range> ValueRange
		{
			get { if( _valueRange.BeginGet() ) ValueRange = _valueRange.Get( this ); return _valueRange.value; }
			set { if( _valueRange.BeginSet( ref value ) ) { try { ValueRangeChanged?.Invoke( this ); } finally { _valueRange.EndSet(); } } }
		}
		public event Action<RegulatorSwitchInSpace> ValueRangeChanged;
		ReferenceField<Range> _valueRange = new Range( 0, 1 );

		/// <summary>
		/// The current position of the switch.
		/// </summary>
		[DefaultValue( 0.0 )]
		public Reference<double> Value
		{
			get { if( _value.BeginGet() ) Value = _value.Get( this ); return _value.value; }
			set { if( _value.BeginSet( ref value ) ) { try { ValueChanged?.Invoke( this ); } finally { _value.EndSet(); } } }
		}
		/// <summary>Occurs when the <see cref="Value"/> property value changes.</summary>
		public event Action<RegulatorSwitchInSpace> ValueChanged;
		ReferenceField<double> _value = 0.0;

		/// <summary>
		/// Whether to allow user interaction with the object.
		/// </summary>
		[DefaultValue( true )]
		public Reference<bool> AllowInteract
		{
			get { if( _allowInteract.BeginGet() ) AllowInteract = _allowInteract.Get( this ); return _allowInteract.value; }
			set { if( _allowInteract.BeginSet( ref value ) ) { try { AllowInteractChanged?.Invoke( this ); } finally { _allowInteract.EndSet(); } } }
		}
		/// <summary>Occurs when the <see cref="AllowInteract"/> property value changes.</summary>
		public event Action<RegulatorSwitchInSpace> AllowInteractChanged;
		ReferenceField<bool> _allowInteract = true;

		/// <summary>
		/// Specifies the angle value of the markers for minimum and maximum value.
		/// </summary>
		[DefaultValue( "-45 45" )]
		[Range( -360, 360 )]
		public Reference<Range> AngleRange
		{
			get { if( _angleRange.BeginGet() ) AngleRange = _angleRange.Get( this ); return _angleRange.value; }
			set { if( _angleRange.BeginSet( ref value ) ) { try { AngleRangeChanged?.Invoke( this ); } finally { _angleRange.EndSet(); } } }
		}
		/// <summary>Occurs when the <see cref="AngleRange"/> property value changes.</summary>
		public event Action<RegulatorSwitchInSpace> AngleRangeChanged;
		ReferenceField<Range> _angleRange = new Range( -45, 45 );

		/// <summary>
		/// Change the value of an object per second.
		/// </summary>
		[DefaultValue( 0.5 )]
		public Reference<double> ChangeSpeed
		{
			get { if( _changeSpeed.BeginGet() ) ChangeSpeed = _changeSpeed.Get( this ); return _changeSpeed.value; }
			set { if( _changeSpeed.BeginSet( ref value ) ) { try { ChangeSpeedChanged?.Invoke( this ); } finally { _changeSpeed.EndSet(); } } }
		}
		/// <summary>Occurs when the <see cref="ChangeSpeed"/> property value changes.</summary>
		public event Action<RegulatorSwitchInSpace> ChangeSpeedChanged;
		ReferenceField<double> _changeSpeed = 0.5;

		/// <summary>
		/// The sound that is played when changing.
		/// </summary>
		[DefaultValueReference( @"Base\UI\Styles\Sounds\ButtonClick.ogg" )]
		public Reference<Sound> SoundTick
		{
			get { if( _soundTick.BeginGet() ) SoundTick = _soundTick.Get( this ); return _soundTick.value; }
			set { if( _soundTick.BeginSet( ref value ) ) { try { SoundTickChanged?.Invoke( this ); } finally { _soundTick.EndSet(); } } }
		}
		/// <summary>Occurs when the <see cref="SoundTick"/> property value changes.</summary>
		public event Action<RegulatorSwitchInSpace> SoundTickChanged;
		ReferenceField<Sound> _soundTick = new Reference<Sound>( null, @"Base\UI\Styles\Sounds\ButtonClick.ogg" );

		/// <summary>
		/// The frequency of tick sounds.
		/// </summary>
		[DefaultValue( 0.05 )]
		public Reference<double> SoundTickFrequency
		{
			get { if( _soundTickFrequency.BeginGet() ) SoundTickFrequency = _soundTickFrequency.Get( this ); return _soundTickFrequency.value; }
			set { if( _soundTickFrequency.BeginSet( ref value ) ) { try { SoundTickFrequencyChanged?.Invoke( this ); } finally { _soundTickFrequency.EndSet(); } } }
		}
		/// <summary>Occurs when the <see cref="SoundTickFrequency"/> property value changes.</summary>
		public event Action<RegulatorSwitchInSpace> SoundTickFrequencyChanged;
		ReferenceField<double> _soundTickFrequency = 0.05;

		/// <summary>
		/// The offset by X from the switch to the center of the valve.
		/// </summary>
		[DefaultValue( 0.1 )]
		public Reference<double> ValveOffset
		{
			get { if( _valveOffset.BeginGet() ) ValveOffset = _valveOffset.Get( this ); return _valveOffset.value; }
			set { if( _valveOffset.BeginSet( ref value ) ) { try { ValveOffsetChanged?.Invoke( this ); } finally { _valveOffset.EndSet(); } } }
		}
		/// <summary>Occurs when the <see cref="ValveOffset"/> property value changes.</summary>
		public event Action<RegulatorSwitchInSpace> ValveOffsetChanged;
		ReferenceField<double> _valveOffset = 0.1;

		/// <summary>
		/// The radius of the valve.
		/// </summary>
		[DefaultValue( 0.1 )]
		public Reference<double> ValveRadius
		{
			get { if( _valveRadius.BeginGet() ) ValveRadius = _valveRadius.Get( this ); return _valveRadius.value; }
			set { if( _valveRadius.BeginSet( ref value ) ) { try { ValveRadiusChanged?.Invoke( this ); } finally { _valveRadius.EndSet(); } } }
		}
		/// <summary>Occurs when the <see cref="ValveRadius"/> property value changes.</summary>
		public event Action<RegulatorSwitchInSpace> ValveRadiusChanged;
		ReferenceField<double> _valveRadius = 0.1;

		/////////////////////////////////////////

		[Browsable( false )]
		public bool Changing
		{
			get { return changing; }
		}

		[Browsable( false )]
		public bool ChangingForward
		{
			get { return changingForward; }
		}

		protected virtual void OnChangingBegin() { }
		protected virtual void OnChangingEnd() { }

		public event Action<RegulatorSwitchInSpace> ChangingBeginEvent;
		public event Action<RegulatorSwitchInSpace> ChangingEndEvent;

		public void ChangingBegin( bool forward )
		{
			if( Changing && ChangingForward == forward )
				return;

			if( Changing && ChangingForward != forward )
				ChangingEnd();

			changing = true;
			changingForward = forward;
			NetworkSendChanging( null );
			OnChangingBegin();
			ChangingBeginEvent?.Invoke( this );
		}

		public void ChangingEnd()
		{
			if( !Changing )
				return;

			changing = false;
			changingForward = false;
			NetworkSendChanging( null );
			OnChangingEnd();
			ChangingEndEvent?.Invoke( this );
		}

		public double GetValueFactor()
		{
			var r = ValueRange.Value;
			if( r.Size != 0 )
				return ( Value.Value - r.Minimum ) / r.Size;
			return 0;
		}

		public double GetValueAngle()
		{
			return MathEx.Lerp( AngleRange.Value.Minimum, AngleRange.Value.Maximum, GetValueFactor() );
		}

		void SimulateTickSound( double oldValue, double newValue )
		{
			bool play = false;

			if( SoundTickFrequency > 0 )
			{
				for( var tickTime = ValueRange.Value.Minimum; tickTime <= ValueRange.Value.Maximum; tickTime += SoundTickFrequency )
				{
					var before = oldValue < tickTime;
					var after = newValue < tickTime;
					if( before != after )
					{
						play = true;
						break;
					}
				}
			}

			if( play )
			{
				SoundPlay( SoundTick );
				if( NetworkIsServer && SoundTick.ReferenceOrValueSpecified )
				{
					BeginNetworkMessageToEveryone( "SoundTick" );
					EndNetworkMessage();
				}
			}
		}

		void Simulate( float delta )
		{
			if( Changing )
			{
				var oldValue = Value.Value;

				//calculate new value
				var newValue = oldValue;
				var step = ChangeSpeed * delta;
				if( ChangingForward )
					newValue += step;
				else
					newValue -= step;
				newValue = MathEx.Clamp( newValue, ValueRange.Value.Minimum, ValueRange.Value.Maximum );

				//update value
				Value = newValue;

				SimulateTickSound( oldValue, newValue );
			}
		}

		public void SumulateRequiredValue( double requiredValue, float delta )
		{
			var oldValue = Value.Value;

			//calculate new value
			var newValue = oldValue;
			var step = ChangeSpeed * delta;
			if( newValue < requiredValue )
			{
				newValue += step;
				if( newValue > requiredValue )
					newValue = requiredValue;
			}
			else
			{
				newValue -= step;
				if( newValue < requiredValue )
					newValue = requiredValue;
			}
			newValue = MathEx.Clamp( newValue, ValueRange.Value.Minimum, ValueRange.Value.Maximum );

			//update value
			Value = newValue;

			SimulateTickSound( oldValue, newValue );
		}

		protected override void OnUpdate( float delta )
		{
			base.OnUpdate( delta );

			//if( EngineApp.IsEditor )
			//	Simulate( delta );
		}

		protected override void OnSimulationStep()
		{
			base.OnSimulationStep();

			Simulate( Time.SimulationDelta );
		}

		public override void NewObjectSetDefaultConfiguration( bool createdFromNewObjectWindow = false )
		{
			if( Components.Count == 0 )
			{
				Color = new ColorValue( 0.6784314, 0.6784314, 0.6784314 );

				//Mesh
				{
					var mesh = CreateComponent<Mesh>();
					mesh.Name = "Mesh";
					Mesh = ReferenceUtility.MakeThisReference( this, mesh );

					var geometry = mesh.CreateComponent<MeshGeometry_Box>();
					geometry.Name = "Mesh Geometry";
					geometry.Dimensions = new Vector3( 0.05, 0.3, 0.3 );
					geometry.Material = ReferenceUtility.MakeReference( "Base\\Materials\\White.material" );
				}

				//Button
				{
					var meshInSpace = CreateComponent<MeshInSpace>();
					meshInSpace.Name = "Button";

					var mesh = meshInSpace.CreateComponent<Mesh>();
					mesh.Name = "Mesh";
					meshInSpace.Mesh = ReferenceUtility.MakeThisReference( this, mesh );

					var geometry = mesh.CreateComponent<MeshGeometry_Cylinder>();
					geometry.Name = "Mesh Geometry";
					geometry.Axis = 0;
					geometry.Radius = 0.08;
					geometry.Height = 0.08;
					geometry.Material = ReferenceUtility.MakeReference( "Base\\Materials\\White.material" );

					meshInSpace.Mesh = ReferenceUtility.MakeThisReference( meshInSpace, mesh );

					var transformOffset = meshInSpace.CreateComponent<TransformOffset>();
					transformOffset.Name = "Attach Transform Offset";
					transformOffset.PositionOffset = new Vector3( 0.05, 0, 0 );
					transformOffset.Source = ReferenceUtility.MakeThisReference( transformOffset, this, "Transform" );

					meshInSpace.Transform = ReferenceUtility.MakeThisReference( meshInSpace, transformOffset, "Result" );
				}

				//Indicator Min
				{
					var meshInSpace = CreateComponent<MeshInSpace>();
					meshInSpace.Name = "Indicator Min";
					meshInSpace.Color = new ColorValue( 1, 0, 0 );

					var mesh = meshInSpace.CreateComponent<Mesh>();
					mesh.Name = "Mesh";
					meshInSpace.Mesh = ReferenceUtility.MakeThisReference( this, mesh );

					var geometry = mesh.CreateComponent<MeshGeometry_Cylinder>();
					geometry.Name = "Mesh Geometry";
					geometry.Axis = 0;
					geometry.Radius = 0.02;
					geometry.Height = 0.02;
					geometry.Segments = 16;
					geometry.Material = ReferenceUtility.MakeReference( "Base\\Materials\\White.material" );

					meshInSpace.Mesh = ReferenceUtility.MakeThisReference( meshInSpace, mesh );

					var transformOffset = meshInSpace.CreateComponent<TransformOffset>();
					transformOffset.Name = "Attach Transform Offset";
					transformOffset.PositionOffset = new Vector3( 0.03, -0.11, 0.11 );
					transformOffset.Source = ReferenceUtility.MakeThisReference( transformOffset, this, "Transform" );

					meshInSpace.Transform = ReferenceUtility.MakeThisReference( meshInSpace, transformOffset, "Result" );
				}

				//Indicator Max
				{
					var meshInSpace = CreateComponent<MeshInSpace>();
					meshInSpace.Name = "Indicator Max";
					meshInSpace.Color = new ColorValue( 0, 1, 0 );

					var mesh = meshInSpace.CreateComponent<Mesh>();
					mesh.Name = "Mesh";
					meshInSpace.Mesh = ReferenceUtility.MakeThisReference( this, mesh );

					var geometry = mesh.CreateComponent<MeshGeometry_Cylinder>();
					geometry.Name = "Mesh Geometry";
					geometry.Axis = 0;
					geometry.Radius = 0.02;
					geometry.Height = 0.02;
					geometry.Segments = 16;
					geometry.Material = ReferenceUtility.MakeReference( "Base\\Materials\\White.material" );

					meshInSpace.Mesh = ReferenceUtility.MakeThisReference( meshInSpace, mesh );

					var transformOffset = meshInSpace.CreateComponent<TransformOffset>();
					transformOffset.Name = "Attach Transform Offset";
					transformOffset.PositionOffset = new Vector3( 0.03, 0.11, 0.11 );
					transformOffset.Source = ReferenceUtility.MakeThisReference( transformOffset, this, "Transform" );

					meshInSpace.Transform = ReferenceUtility.MakeThisReference( meshInSpace, transformOffset, "Result" );
				}

				//Marker Min
				{
					var meshInSpace = CreateComponent<MeshInSpace>();
					meshInSpace.Name = "Marker Min";
					meshInSpace.Color = new ColorValue( 1, 0, 0 );

					var mesh = meshInSpace.CreateComponent<Mesh>();
					mesh.Name = "Mesh";
					meshInSpace.Mesh = ReferenceUtility.MakeThisReference( this, mesh );

					var geometry = mesh.CreateComponent<MeshGeometry_Box>();
					geometry.Name = "Mesh Geometry";
					geometry.Dimensions = new Vector3( 0.05, 0.14, 0.01 );
					geometry.Material = ReferenceUtility.MakeReference( "Base\\Materials\\White.material" );

					meshInSpace.Mesh = ReferenceUtility.MakeThisReference( meshInSpace, mesh );

					var transformOffset = meshInSpace.CreateComponent<TransformOffset>();
					transformOffset.Name = "Attach Transform Offset";
					transformOffset.PositionOffset = new Vector3( 0.01, 0, 0 );
					transformOffset.Source = ReferenceUtility.MakeThisReference( transformOffset, this, "Transform" );

					meshInSpace.Transform = ReferenceUtility.MakeThisReference( meshInSpace, transformOffset, "Result" );
				}

				//Marker Max
				{
					var meshInSpace = CreateComponent<MeshInSpace>();
					meshInSpace.Name = "Marker Max";
					meshInSpace.Color = new ColorValue( 0, 1, 0 );

					var mesh = meshInSpace.CreateComponent<Mesh>();
					mesh.Name = "Mesh";
					meshInSpace.Mesh = ReferenceUtility.MakeThisReference( this, mesh );

					var geometry = mesh.CreateComponent<MeshGeometry_Box>();
					geometry.Name = "Mesh Geometry";
					geometry.Dimensions = new Vector3( 0.05, 0.14, 0.01 );
					geometry.Material = ReferenceUtility.MakeReference( "Base\\Materials\\White.material" );

					meshInSpace.Mesh = ReferenceUtility.MakeThisReference( meshInSpace, mesh );

					var transformOffset = meshInSpace.CreateComponent<TransformOffset>();
					transformOffset.Name = "Attach Transform Offset";
					transformOffset.PositionOffset = new Vector3( 0.01, 0, 0 );
					transformOffset.Source = ReferenceUtility.MakeThisReference( transformOffset, this, "Transform" );

					meshInSpace.Transform = ReferenceUtility.MakeThisReference( meshInSpace, transformOffset, "Result" );
				}

				//Marker Current
				{
					var meshInSpace = CreateComponent<MeshInSpace>();
					meshInSpace.Name = "Marker Current";
					meshInSpace.Color = new ColorValue( 1, 1, 1 );

					var mesh = meshInSpace.CreateComponent<Mesh>();
					mesh.Name = "Mesh";
					meshInSpace.Mesh = ReferenceUtility.MakeThisReference( this, mesh );

					var geometry = mesh.CreateComponent<MeshGeometry_Box>();
					geometry.Name = "Mesh Geometry";
					geometry.Dimensions = new Vector3( 0.05, 0.14, 0.01 );
					geometry.Material = ReferenceUtility.MakeReference( "Base\\Materials\\White.material" );

					meshInSpace.Mesh = ReferenceUtility.MakeThisReference( meshInSpace, mesh );

					var transformOffset = meshInSpace.CreateComponent<TransformOffset>();
					transformOffset.Name = "Attach Transform Offset";
					transformOffset.PositionOffset = new Vector3( 0.06, 0, 0 );
					transformOffset.Source = ReferenceUtility.MakeThisReference( transformOffset, this, "Transform" );

					meshInSpace.Transform = ReferenceUtility.MakeThisReference( meshInSpace, transformOffset, "Result" );
				}

				//C# Script
				{
					var script = CreateComponent<CSharpScript>();
					script.Name = "C# Script";
					script.Code = "public void InteractiveObjectButton_UpdateEvent(NeoAxis.Component sender, float delta)\r\n{\r\n\tvar _this = sender as RegulatorSwitchInSpace;\r\n\tif (_this != null)\r\n\t{\r\n\t\tvar indicatorMin = _this.GetComponent(\"Indicator Min\") as MeshInSpace;\r\n\t\tif (indicatorMin != null)\r\n\t\t\tindicatorMin.Color = _this.Value.Value <= _this.ValueRange.Value.Minimum ? new ColorValue(1, 0, 0) : new ColorValue(0.5, 0.5, 0.5);\r\n\r\n\t\tvar indicatorMax = _this.GetComponent(\"Indicator Max\") as MeshInSpace;\r\n\t\tif (indicatorMax != null)\r\n\t\t\tindicatorMax.Color = _this.Value.Value >= _this.ValueRange.Value.Maximum ? new ColorValue(0, 1, 0) : new ColorValue(0.5, 0.5, 0.5);\r\n\r\n\t\tvar button = _this.GetComponent(\"Button\");\r\n\t\tif (button != null)\r\n\t\t{\r\n\t\t\tvar offset = button.GetComponent<TransformOffset>();\r\n\t\t\tif (offset != null)\r\n\t\t\t{\r\n\t\t\t\tvar angle = _this.GetValueAngle() - 90;\r\n\t\t\t\toffset.RotationOffset = new Angles(angle, 0, 0).ToQuaternion();\r\n\t\t\t}\r\n\t\t}\r\n\r\n\t\tvar markerMin = _this.GetComponent(\"Marker Min\");\r\n\t\tif (markerMin != null)\r\n\t\t{\r\n\t\t\tvar offset = markerMin.GetComponent<TransformOffset>();\r\n\t\t\tif (offset != null)\r\n\t\t\t{\r\n\t\t\t\tvar angle = _this.AngleRange.Value.Minimum - 90;\r\n\t\t\t\tvar angleR = MathEx.DegreeToRadian(angle);\r\n\t\t\t\toffset.PositionOffset = new Vector3(0.01, Math.Cos(angleR) * 0.04, Math.Sin(-angleR) * 0.04);\r\n\t\t\t\toffset.RotationOffset = new Angles(angle, 0, 0).ToQuaternion();\r\n\t\t\t}\r\n\t\t}\r\n\r\n\t\tvar markerMax = _this.GetComponent(\"Marker Max\");\r\n\t\tif (markerMax != null)\r\n\t\t{\r\n\t\t\tvar offset = markerMax.GetComponent<TransformOffset>();\r\n\t\t\tif (offset != null)\r\n\t\t\t{\r\n\t\t\t\tvar angle = _this.AngleRange.Value.Maximum - 90;\r\n\t\t\t\tvar angleR = MathEx.DegreeToRadian(angle);\r\n\t\t\t\toffset.PositionOffset = new Vector3(0.01, Math.Cos(angleR) * 0.04, Math.Sin(-angleR) * 0.04);\r\n\t\t\t\toffset.RotationOffset = new Angles(angle, 0, 0).ToQuaternion();\r\n\t\t\t}\r\n\t\t}\r\n\r\n\t\tvar markerCurrent = _this.GetComponent(\"Marker Current\");\r\n\t\tif (markerCurrent != null)\r\n\t\t{\r\n\t\t\tvar offset = markerCurrent.GetComponent<TransformOffset>();\r\n\t\t\tif (offset != null)\r\n\t\t\t{\r\n\t\t\t\tvar angle = _this.GetValueAngle() - 90;\r\n\t\t\t\tvar angleR = MathEx.DegreeToRadian(angle);\r\n\t\t\t\toffset.PositionOffset = new Vector3(0.06, Math.Cos(angleR) * 0.04, Math.Sin(-angleR) * 0.04);\r\n\t\t\t\toffset.RotationOffset = new Angles(angle, 0, 0).ToQuaternion();\r\n\t\t\t}\r\n\t\t}\r\n\t}\r\n}";

					var handler = script.CreateComponent<EventHandlerComponent>();
					handler.Name = "Event Handler UpdateEvent";
					handler.WhenEnable = EventHandlerComponent.WhenEnableEnum.Editor | EventHandlerComponent.WhenEnableEnum.Simulation | EventHandlerComponent.WhenEnableEnum.Instance;
					handler.Event = ReferenceUtility.MakeReference( "this:..\\..\\event:UpdateEvent" );
					handler.HandlerMethod = ReferenceUtility.MakeReference( "this:..\\method:InteractiveObjectButton_UpdateEvent(NeoAxis.Component,System.Single)" );
				}
			}
		}

		public void SoundPlay( Sound sound )
		{
			ParentScene?.SoundPlay( sound, TransformV.Position );
		}

		public virtual bool ObjectInteractionInputMessage( GameMode gameMode, InputMessage message )
		{
			var mouseDown = message as InputMessageMouseButtonDown;
			if( mouseDown != null )
			{
				if( mouseDown.Button == EMouseButtons.Left || mouseDown.Button == EMouseButtons.Right )
				{
					if( NetworkIsClient )
					{
						var writer = BeginNetworkMessageToServer( "ChangingBegin" );
						if( writer != null )
						{
							writer.Write( mouseDown.Button == EMouseButtons.Left );
							EndNetworkMessage();
						}
					}
					else
						ChangingBegin( mouseDown.Button == EMouseButtons.Left );
					return true;
				}
			}

			var mouseUp = message as InputMessageMouseButtonUp;
			if( mouseUp != null )
			{
				if( mouseUp.Button == EMouseButtons.Left && Changing && ChangingForward )
				{
					if( NetworkIsClient )
					{
						BeginNetworkMessageToServer( "ChangingEnd" );
						EndNetworkMessage();
					}
					else
						ChangingEnd();
					return true;
				}
				if( mouseUp.Button == EMouseButtons.Right && Changing && !ChangingForward )
				{
					if( NetworkIsClient )
					{
						BeginNetworkMessageToServer( "ChangingEnd" );
						EndNetworkMessage();
					}
					else
						ChangingEnd();
					return true;
				}
			}

			return false;
		}

		public void ObjectInteractionGetInfo( GameMode gameMode, ref InteractiveObjectObjectInfo info )
		{
			info = new InteractiveObjectObjectInfo();
			info.AllowInteract = AllowInteract;
			info.SelectionTextInfo.Add( Name );
		}

		public void ObjectInteractionEnter( ObjectInteractionContext context )
		{
		}

		public void ObjectInteractionExit( ObjectInteractionContext context )
		{
			if( NetworkIsClient )
			{
				BeginNetworkMessageToServer( "ChangingEnd" );
				EndNetworkMessage();
			}
			else
				ChangingEnd();
		}

		public void ObjectInteractionUpdate( ObjectInteractionContext context )
		{
		}

		protected override bool OnSpaceBoundsUpdateIncludeChildren()
		{
			return true;
		}

		void NetworkSendChanging( ServerNetworkService_Components.ClientItem client )
		{
			if( NetworkIsServer )
			{
				var writer = client != null ? BeginNetworkMessage( client, "Changing" ) : BeginNetworkMessageToEveryone( "Changing" );
				writer.Write( changing );
				writer.Write( changingForward );
				EndNetworkMessage();
			}
		}

		protected override void OnClientConnectedBeforeRootComponentEnabled( ServerNetworkService_Components.ClientItem client )
		{
			base.OnClientConnectedBeforeRootComponentEnabled( client );

			NetworkSendChanging( client );
		}

		protected override bool OnReceiveNetworkMessageFromServer( string message, ArrayDataReader reader )
		{
			if( !base.OnReceiveNetworkMessageFromServer( message, reader ) )
				return false;

			if( message == "SoundTick" )
				SoundPlay( SoundTick );
			else if( message == "Changing" )
			{
				changing = reader.ReadBoolean();
				changingForward = reader.ReadBoolean();
			}

			return true;
		}

		protected override bool OnReceiveNetworkMessageFromClient( ServerNetworkService_Components.ClientItem client, string message, ArrayDataReader reader )
		{
			if( !base.OnReceiveNetworkMessageFromClient( client, message, reader ) )
				return false;


			//!!!!security check is needed. who interact


			if( message == "ChangingBegin" )
			{
				var forward = reader.ReadBoolean();
				if( !reader.Complete() )
					return false;

				ChangingBegin( forward );
			}
			else if( message == "ChangingEnd" )
			{
				ChangingEnd();
			}

			return true;
		}
	}
}