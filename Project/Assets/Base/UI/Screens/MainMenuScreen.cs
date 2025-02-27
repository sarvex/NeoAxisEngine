// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using NeoAxis;

namespace Project
{
	public class MainMenuScreen : UIControl
	{
		static MainMenuScreen instance;

		bool currentDisplayBackgroundSceneOption;
		Scene scene;
		Viewport sceneViewport;

		bool firstRender = true;
		double fadeInTimer;

		UIWindow scenesWindow;
		UIWindow optionsWindow;
		UIWindow multiplayerCreateWindow;
		UIWindow multiplayerJoinWindow;

		///////////////////////////////////////////

		[DefaultValue( null )]
		[Serialize]
		public Reference<ReferenceValueType_Resource> BackgroundScene
		{
			get { if( _backgroundScene.BeginGet() ) BackgroundScene = _backgroundScene.Get( this ); return _backgroundScene.value; }
			set { if( _backgroundScene.BeginSet( ref value ) ) { try { BackgroundSceneChanged?.Invoke( this ); } finally { _backgroundScene.EndSet(); } } }
		}
		public event Action<MainMenuScreen> BackgroundSceneChanged;
		ReferenceField<ReferenceValueType_Resource> _backgroundScene;

		[DefaultValue( null )]
		[Serialize]
		public Reference<ReferenceValueType_Resource> BackgroundSceneLimitedDevice
		{
			get { if( _backgroundSceneLimitedDevice.BeginGet() ) BackgroundSceneLimitedDevice = _backgroundSceneLimitedDevice.Get( this ); return _backgroundSceneLimitedDevice.value; }
			set { if( _backgroundSceneLimitedDevice.BeginSet( ref value ) ) { try { BackgroundSceneLimitedDeviceChanged?.Invoke( this ); } finally { _backgroundSceneLimitedDevice.EndSet(); } } }
		}
		public event Action<MainMenuScreen> BackgroundSceneLimitedDeviceChanged;
		ReferenceField<ReferenceValueType_Resource> _backgroundSceneLimitedDevice;

		///////////////////////////////////////////

		UIButton GetButtonScenes() { return GetComponent<UIButton>( "Button Scenes" ); }
		UIButton GetButtonOptions() { return GetComponent<UIButton>( "Button Options" ); }
		UIButton GetButtonExit() { return GetComponent<UIButton>( "Button Exit" ); }
		UIButton GetButtonMultiplayerCreate() { return GetComponent<UIButton>( "Button Multiplayer Create" ); }
		UIButton GetButtonMultiplayerJoin() { return GetComponent<UIButton>( "Button Multiplayer Join" ); }

		///////////////////////////////////////////

		public static MainMenuScreen Instance
		{
			get { return instance; }
		}

		protected override void OnEnabledInSimulation()
		{
			instance = this;

			if( GetButtonScenes() != null )
			{
				var button = GetButtonScenes();
				button.Click += ButtonScenes_Click;
				button.ReadOnly = SimulationAppClient.Created;
			}
			if( GetButtonOptions() != null )
				GetButtonOptions().Click += ButtonOptions_Click;
			if( GetButtonExit() != null )
				GetButtonExit().Click += ButtonExit_Click;

			//play buttons
			if( Components[ "Button Play City Demo" ] != null )
			{
				var button = (UIButton)Components[ "Button Play City Demo" ];
				var fileName = @"Samples\City Demo\City Demo.scene";
				button.AnyData = fileName;
				button.Click += ButtonPlay_Click;
				if( button.Visible )
					button.ReadOnly = !VirtualFile.Exists( fileName ) || SimulationAppClient.Created;
			}
			if( Components[ "Button Play Nature Demo" ] != null )
			{
				var button = (UIButton)Components[ "Button Play Nature Demo" ];
				var fileName = @"Samples\Nature Demo\Nature Demo.scene";
				button.AnyData = fileName;
				button.Click += ButtonPlay_Click;
				if( button.Visible )
					button.ReadOnly = !VirtualFile.Exists( fileName ) || SimulationAppClient.Created;
			}
			if( Components[ "Button Play Battle Demo" ] != null )
			{
				var button = (UIButton)Components[ "Button Play Battle Demo" ];
				var fileName = @"Samples\Battle Demo\Battle Demo.scene";
				button.AnyData = fileName;
				button.Click += ButtonPlay_Click;
				if( button.Visible )
					button.ReadOnly = !VirtualFile.Exists( fileName ) || SimulationAppClient.Created;
			}
			if( Components[ "Button Play Simple Game" ] != null )
			{
				var button = (UIButton)Components[ "Button Play Simple Game" ];
				var fileName = @"Samples\Simple Game\SimpleGameLevel1.scene";
				button.AnyData = fileName;
				button.Click += ButtonPlay_Click;
				if( button.Visible )
					button.ReadOnly = !VirtualFile.Exists( fileName ) || SimulationAppClient.Created;
			}
			if( Components[ "Button Play Character Scene" ] != null )
			{
				var button = (UIButton)Components[ "Button Play Character Scene" ];
				var fileName = @"Samples\Starter Content\Scenes\Character.scene";
				button.AnyData = fileName;
				button.Click += ButtonPlay_Click;
				if( button.Visible )
					button.ReadOnly = !VirtualFile.Exists( fileName ) || SimulationAppClient.Created;
			}
			if( Components[ "Button Play Spaceship Game" ] != null )
			{
				var button = (UIButton)Components[ "Button Play Spaceship Game" ];
				var fileName = @"Samples\Spaceship Game\Spaceship Game.scene";
				button.AnyData = fileName;
				button.Click += ButtonPlay_Click;
				if( button.Visible )
					button.ReadOnly = !VirtualFile.Exists( fileName ) || SimulationAppClient.Created;
			}
			if( Components[ "Button Play Platform Game" ] != null )
			{
				var button = (UIButton)Components[ "Button Play Platform Game" ];
				var fileName = @"Samples\Platform Game\Platform Game.scene";
				button.AnyData = fileName;
				button.Click += ButtonPlay_Click;
				if( button.Visible )
					button.ReadOnly = !VirtualFile.Exists( fileName ) || SimulationAppClient.Created;
			}

			var textConsole = Components[ "Text Console" ] as UIControl;
			if( textConsole != null && SystemSettings.MobileDevice )
				textConsole.Visible = false;

			// Update sound listener.
			SoundWorld.SetListenerReset();

			// Load background scene.
			currentDisplayBackgroundSceneOption = SimulationApp.DisplayBackgroundScene;
			if( currentDisplayBackgroundSceneOption && EngineApp.IsSimulation )
			{
				var fileName = SystemSettings.LimitedDevice ? BackgroundSceneLimitedDevice.GetByReference : BackgroundScene.GetByReference;
				if( !string.IsNullOrEmpty( fileName ) && VirtualFile.Exists( fileName ) )
					LoadScene( fileName );
				else
					LoadScene( "" );
			}
			else
				LoadScene( "" );
		}

		protected override void OnDisabledInSimulation()
		{
			DestroyScene();

			if( instance == this )
				instance = null;
		}

		void ButtonScenes_Click( UIButton sender )
		{
			if( scenesWindow != null && scenesWindow.Disposed )
				scenesWindow = null;

			if( scenesWindow == null )
			{
				scenesWindow = ResourceManager.LoadSeparateInstance<UIWindow>( @"Base\UI\Screens\ScenesWindow.ui", false, true );
				if( scenesWindow != null )
					AddComponent( scenesWindow );
			}
			else
			{
				scenesWindow.Dispose();
				scenesWindow = null;
			}
		}

		void ButtonOptions_Click( UIButton sender )
		{
			if( optionsWindow != null && optionsWindow.Disposed )
				optionsWindow = null;

			if( optionsWindow == null )
			{
				optionsWindow = ResourceManager.LoadSeparateInstance<UIWindow>( @"Base\UI\Screens\OptionsWindow.ui", false, true );
				if( optionsWindow != null )
					AddComponent( optionsWindow );
			}
			else
			{
				optionsWindow.Dispose();
				optionsWindow = null;
			}
		}

		void ButtonExit_Click( UIButton sender )
		{
			EngineApp.NeedExit = true;
		}

		protected override void OnUpdate( float delta )
		{
			base.OnUpdate( delta );

			if( EngineApp.IsSimulation )
			{
				// Update background scene.
				if( currentDisplayBackgroundSceneOption != SimulationApp.DisplayBackgroundScene )
				{
					currentDisplayBackgroundSceneOption = SimulationApp.DisplayBackgroundScene;

					if( currentDisplayBackgroundSceneOption && EngineApp.IsSimulation )
					{
						var fileName = SystemSettings.LimitedDevice ? BackgroundSceneLimitedDevice.GetByReference : BackgroundScene.GetByReference;
						if( !string.IsNullOrEmpty( fileName ) && VirtualFile.Exists( fileName ) )
							LoadScene( fileName );
						else
							LoadScene( "" );
					}
					else
						LoadScene( "" );
				}

				// Update sound listener.
				if( scene != null && sceneViewport != null )
				{
					var settings = sceneViewport.CameraSettings;
					SoundWorld.SetListener( scene, settings.Position, Vector3.Zero, settings.Rotation );
				}
				else
					SoundWorld.SetListenerReset();

				// Scene simulation.
				scene?.HierarchyController?.PerformSimulationSteps();
				ParentRoot.HierarchyController?.PerformSimulationSteps();

				if( !firstRender )
					fadeInTimer += delta;

				if( GetButtonMultiplayerCreate() != null )
				{
					GetButtonMultiplayerCreate().Highlighted = RunServer.Running;
					GetButtonMultiplayerCreate().ReadOnly = SystemSettings.CurrentPlatform != SystemSettings.Platform.Windows;
				}
				if( GetButtonMultiplayerJoin() != null )
					GetButtonMultiplayerJoin().ReadOnly = SystemSettings.CurrentPlatform != SystemSettings.Platform.Windows;
			}
		}

		double GetFadeInAlpha()
		{
			var curve = new CurveLine();
			curve.AddPoint( 0, new Vector3( 1, 0, 0 ) );
			curve.AddPoint( 1.0, new Vector3( 1, 0, 0 ) );
			curve.AddPoint( 1.0 + 1.0, new Vector3( 0, 0, 0 ) );

			var value = curve.CalculateValueByTime( fadeInTimer );
			return MathEx.Saturate( value.X );
		}

		protected override void OnRenderUI( CanvasRenderer renderer )
		{
			base.OnRenderUI( renderer );
		}

		protected override void OnAfterRenderUIWithChildren( CanvasRenderer renderer )
		{
			base.OnAfterRenderUIWithChildren( renderer );

			//fade in
			if( EngineApp.IsSimulation )
			{
				var alpha = GetFadeInAlpha();
				if( alpha != 0 )
					renderer.AddQuad( new Rectangle( 0, 0, 1, 1 ), new Rectangle( 0, 0, 1, 1 ), null, new ColorValue( 0, 0, 0, alpha ) );
			}

			firstRender = false;
		}

		public void LoadScene( string fileName )
		{
			DestroyScene();

			if( SimulationAppClient.Created )
				return;

			if( !string.IsNullOrEmpty( fileName ) )
				scene = ResourceManager.LoadSeparateInstance<Scene>( fileName, true, null );

			if( scene == null )
			{
				scene = ComponentUtility.CreateComponent<Scene>( null, true, true );
				scene.BackgroundColor = new ColorValue( 0.4, 0.4, 0.4 );
			}

			sceneViewport = ParentContainer.Viewport;
			scene.ViewportUpdateBegin += Scene_ViewportUpdateBegin;
			scene.ViewportUpdateGetCameraSettings += Scene_ViewportUpdateGetCameraSettings;
			sceneViewport.AttachedScene = scene;
			sceneViewport.NotifyInstantCameraMovement();

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		private void Scene_ViewportUpdateBegin( Scene scene, Viewport viewport, Viewport.CameraSettingsClass overrideCameraSettings )
		{
			SimulationApp.UpdateSceneAntialiasingByAppSettings( scene );
			SimulationApp.UpdateSceneResolutionUpscaleByAppSettings( scene );
			SimulationApp.UpdateSceneSharpnessByAppSettings( scene );
		}

		private void Scene_ViewportUpdateGetCameraSettings( Scene scene, Viewport viewport, ref bool processed )
		{
			Camera camera = scene.CameraDefault;
			if( camera == null )
				camera = scene.Mode.Value == Scene.ModeEnum._3D ? scene.CameraEditor : scene.CameraEditor2D;

			// Create new camera:
			//camera = (Camera)camera.Clone();
			////camera = new Camera();
			//camera.Transform = new Transform( cameraPosition, Quaternion.LookAt( ( lookTo - cameraPosition ).GetNormalize(), up ) );
			//camera.FixedUp = up;

			if( camera != null )
			{
				viewport.CameraSettings = new Viewport.CameraSettingsClass( viewport, camera );
				processed = true;
			}
			else
			{
				viewport.CameraSettings = new Viewport.CameraSettingsClass( viewport, 1, 90, 0.1, 1000, Vector3.Zero, Vector3.XAxis, Vector3.ZAxis, ProjectionType.Perspective, 1, 1, 1 );
				processed = true;
			}
		}

		/// <summary>
		/// Destroys background scene.
		/// </summary>
		public void DestroyScene()
		{
			if( sceneViewport != null )
			{
				if( sceneViewport.AttachedScene == scene )
					sceneViewport.AttachedScene = null;

				scene.ViewportUpdateBegin -= Scene_ViewportUpdateBegin;
				scene.ViewportUpdateGetCameraSettings -= Scene_ViewportUpdateGetCameraSettings;
				sceneViewport = null;
			}
			if( scene != null )
			{
				scene.Dispose();
				scene = null;

				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
		}

		private void ButtonPlay_Click( UIButton sender )
		{
			var playFile = (string)sender.AnyData;
			SimulationApp.PlayFile( playFile );
		}

		public void ButtonMultiplayerCreate_Click( NeoAxis.UIButton sender )
		{
			if( multiplayerCreateWindow != null && multiplayerCreateWindow.Disposed )
				multiplayerCreateWindow = null;

			if( multiplayerCreateWindow == null )
			{
				multiplayerCreateWindow = ResourceManager.LoadSeparateInstance<UIWindow>( @"Base\UI\Screens\MultiplayerCreateWindow.ui", false, true );
				if( multiplayerCreateWindow != null )
					AddComponent( multiplayerCreateWindow );
			}
			else
			{
				multiplayerCreateWindow.Dispose();
				multiplayerCreateWindow = null;
			}
		}

		public void ButtonMultiplayerJoin_Click( NeoAxis.UIButton sender )
		{
			if( multiplayerJoinWindow != null && multiplayerJoinWindow.Disposed )
				multiplayerJoinWindow = null;

			if( multiplayerJoinWindow == null )
			{
				multiplayerJoinWindow = ResourceManager.LoadSeparateInstance<UIWindow>( @"Base\UI\Screens\MultiplayerJoinWindow.ui", false, true );
				if( multiplayerJoinWindow != null )
					AddComponent( multiplayerJoinWindow );
			}
			else
			{
				multiplayerJoinWindow.Dispose();
				multiplayerJoinWindow = null;
			}
		}
	}
}