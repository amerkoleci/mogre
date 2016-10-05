// Alimer - Copyright (C) Amer Koleci
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Resources;
using Miyagi.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Mogre.Framework
{
	[SampleInfo("Camera Tracking Sample", "thumb_camtrack.png", "An example of using AnimationTracks to make a node smoothly follow a predefined path.")]
	public class CameraTrackSample : Sample
	{
		AnimationState _animState;

		SceneManager _rttSceneManager;
		Camera _rttCamera;
		CompositorWorkspace _rttWorkspace;
		SceneNode _penguinNode;
		MiyagiSystem _miyagiSystem;

		protected override void CreateScene()
		{
			// setup some basic lighting for our scene
			_sceneManager.AmbientLight = new ColourValue(0.3f, 0.3f, 0.3f);
			var lightNode = _sceneManager.RootSceneNode.CreateChildSceneNode();
			lightNode.SetPosition(20, 80, 50);
			lightNode.AttachObject(_sceneManager.CreateLight());

			_sceneManager.SetSkyBox(true, "Examples/SpaceSkyBox");

			// create an ogre head entity and attach it to a node
			Entity head = _sceneManager.CreateEntity("ogrehead.mesh");
			head.Name = "Head";
			SceneNode headNode = _sceneManager.GetRootSceneNode().CreateChildSceneNode();
			headNode.Translate(0, 40, 0);
			headNode.AttachObject(head);

			// create a camera node and attach camera to it
			SceneNode camNode = _sceneManager.GetRootSceneNode().CreateChildSceneNode();
			_camera.DetachFromParent();
			camNode.AttachObject(_camera);

			// set up a 10 second animation for our camera, using spline interpolation for nice curves
			Animation anim = _sceneManager.CreateAnimation("CameraTrack", 10);
			anim.SetInterpolationMode(Animation.InterpolationMode.IM_SPLINE);

			// create a track to animate the camera's node
			NodeAnimationTrack track = anim.CreateNodeTrack(camNode);

			// create keyframes for our track
			track.CreateNodeKeyFrame(0).Translate = new Vector3(200, 0, 0);
			track.CreateNodeKeyFrame(2.5f).Translate = new Vector3(0, -50, 100);
			track.CreateNodeKeyFrame(5).Translate = new Vector3(-500, 100, 0);
			track.CreateNodeKeyFrame(7.5f).Translate = new Vector3(0, 200, -300);
			track.CreateNodeKeyFrame(10).Translate = new Vector3(200, 0, 0);

			// create a new animation state to track this
			_animState = _sceneManager.CreateAnimationState("CameraTrack");
			_animState.Enabled = true;

			// Now create another scene manager with only
			_rttSceneManager = CreateSceneManager();

			// Create the RenderTargetTexture
			_rttCamera = _rttSceneManager.CreateCamera("RttCamera");
			_rttCamera.Position = new Vector3(0f, 10f, 500f);
			_rttCamera.LookAt(new Vector3(0f, 0f, -300f));
			_rttCamera.AutoAspectRatio = true;
			_rttCamera.NearClipDistance = 5.0f;

			// Penguin
			Entity penguin = _rttSceneManager.CreateEntity("penguin.mesh");
			_penguinNode = _rttSceneManager.RootSceneNode.CreateChildSceneNode();
			_penguinNode.AttachObject(penguin);
			penguin.Name = "Penguin";

			MaterialPtr penguinMaterial = MaterialManager.Singleton.Create("PenguinMaterial", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
			penguinMaterial.GetTechnique(0).GetPass(0).LightingEnabled = false;
			penguinMaterial.GetTechnique(0).GetPass(0).CreateTextureUnitState("penguin.jpg");
			penguin.SetMaterialName(penguinMaterial.Name);

			var screenTexture0 = TextureManager.Singleton.CreateManual(
				"screenTexture0",
				ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
				TextureType.TEX_TYPE_2D,
				640,
				480,
				0,
				PixelFormat.PF_R8G8B8,
				(int)TextureUsage.TU_RENDERTARGET
			);

			CompositorManager2 compositorManager = _root.CompositorManager2;
			string workspaceName = GetType().Name + "RttWorkspace";
			if (!compositorManager.HasWorkspaceDefinition(workspaceName))
			{
				compositorManager.CreateBasicWorkspaceDef(workspaceName, new ColourValue(1.0f, 0.0f, 0.0f, 1.0f));
			}

			_rttWorkspace = _root.CompositorManager2.AddWorkspace(
				_rttSceneManager,
				screenTexture0.GetBuffer().GetRenderTarget(),
				_rttCamera,
				workspaceName,
				true);

			MaterialPtr renderMat = MaterialManager.Singleton.Create("RttMat", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
			var pass = renderMat.GetTechnique(0).GetPass(0);
			pass.LightingEnabled = false;
			var textureUnit = pass.CreateTextureUnitState();
			textureUnit.SetContentType(TextureUnitState.ContentType.CONTENT_COMPOSITOR);
			textureUnit.SetTextureName("screenTexture0", TextureType.TEX_TYPE_2D);
			textureUnit.SetTextureAddressingMode(TextureUnitState.TextureAddressingMode.TAM_WRAP);

			Plane plane = new Plane(Vector3.UNIT_Y, 0);
			MeshManager.Singleton.CreatePlane(
			  "planeMesh",
			  ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
			  plane,
			  700.0f, 1300.0f,
			  10, 10, true, 1,
			  4.0f, 4.0f,
			  Vector3.UNIT_Z);

			Entity planeEntity = _sceneManager.CreateEntity("planeMesh");
			planeEntity.SetMaterialName("RttMat");
			planeEntity.CastShadows = false;
			_sceneManager.RootSceneNode.CreateChildSceneNode().AttachObject(planeEntity);

			// Test Miyagi
			// Init MiyagiSystem
			_miyagiSystem = new MiyagiSystem("Mogre", (int)_window.Width, (int)_window.Height);
			//const string PluginPath = @"..\..\..\debug\Plugins";
			//this.miyagiSystem.PluginManager.LoadPlugin(Path.Combine(PluginPath, "Miyagi.Plugin.Input.Mois.dll"), this.inputKeyboard, this.inputMouse);

			Resources.Create(_miyagiSystem);
			//Utilities.CreateCursor(system.GUIManager);

			// create a default GUI
			var gui = new GUI();


			// A Button is a simple skinned control capable of changing its current texture automatically on certain mouse events
			// (MouseDown/MouseEnter/MouseLeave/MouseUp). Those subskins are optinal, if a subskin is not defined, Miyagi will fall
			// back to an appropriate alternative.
			// Since Button inherits from Label it provides the same TextStyle options.
			var button1 = new Miyagi.UI.Controls.Button
			{
				Text = "HELLO WORLD",
				Location = new Point(140, 140),
				Size = new Size(200, 50),
				Skin = Resources.Skins["ButtonSkin"],
				TextStyle =
				{
					Alignment = Alignment.MiddleCenter,
					ForegroundColour = Colours.White
				}
			};

			// add the Buttons to the GUI
			gui.Controls.Add(button1);
			//gui.Controls.Add(button2);

			var pictureBox = new Miyagi.UI.Controls.PictureBox
			{
				Name = "testingPictureBox",
				Location = new Point(380, 140),
				Size = new Size(400, 100),
			};

			pictureBox.Bitmap = new System.Drawing.Bitmap(new System.Drawing.Bitmap(400, 100));

			// add the Buttons to the GUI
			gui.Controls.Add(pictureBox);

			// add the GUI to the GUIManager
			_miyagiSystem.GUIManager.GUIs.Add(gui);
			gui.SpriteRenderer.CacheToTexture = true;

			System.Windows.Forms.Timer dummyTimer = new System.Windows.Forms.Timer();
			dummyTimer.Interval = 2000;
			dummyTimer.Tick += dummyTimer_Tick;
			dummyTimer.Start();
		}

		void dummyTimer_Tick(object sender, EventArgs e)
		{
			Miyagi.UI.Controls.PictureBox testingPictureBox = _miyagiSystem.GUIManager.GetControl<Miyagi.UI.Controls.PictureBox>("testingPictureBox");
			testingPictureBox.Size = new Miyagi.Common.Data.Size(399, 99);
		}

		protected override void DestroyScene()
		{
			Utilities.Dispose(ref _miyagiSystem);
			_root.DestroySceneManager(_rttSceneManager);
			Utilities.Dispose(ref _rttSceneManager);
		}

		protected override void LoadResources()
		{
			// Load resources
			//ResourceGroupManager.Singleton.AddResourceLocation(@"../../Media/", "FileSystem");
			ResourceGroupManager.Singleton.AddResourceLocation(@"../../../Media/Gfx/Cursor/", "FileSystem");
			ResourceGroupManager.Singleton.AddResourceLocation(@"../../../Media/Gfx/GUI/", "FileSystem");
			ResourceGroupManager.Singleton.AddResourceLocation(@"../../../Media/Gfx/GUI/Extra/", "FileSystem");
			ResourceGroupManager.Singleton.AddResourceLocation(@"../../../Media/Gfx/Shader/", "FileSystem");
			ResourceGroupManager.Singleton.AddResourceLocation(@"../../../Media/Gfx/Fonts/", "FileSystem");
			ResourceGroupManager.Singleton.AddResourceLocation(@"../../../Media/Gfx/TileMap/", "FileSystem");

			base.LoadResources();
		}

		protected override bool OnFrameStarted(FrameEvent evt)
		{
			_animState.AddTime(evt.timeSinceLastFrame);   // increment animation time
			_penguinNode.Yaw(evt.timeSinceLastFrame * 30);

			if (_miyagiSystem != null)
			{
				var mgr = this._miyagiSystem.TwoDManager;
				if (mgr.GetElement("FPS") != null)
				{
					//mgr.GetElement<TextOverlay>("FPS").Text = "FPS: " + this.window.LastFPS;
				}
				if (mgr.GetElement("Batch") != null)
				{
					//mgr.GetElement<TextOverlay>("Batch").Text = "Batch: " + this.window.BatchCount;
				}
				if (mgr.GetElement("Vertex") != null)
				{
					//mgr.GetElement<TextOverlay>("Vertex").Text = "Vertex: " + this.window.TriangleCount * 3;
				}

				this._miyagiSystem.Update();
			}


			return base.OnFrameStarted(evt);
		}
	}

	public class Resources
	{
		#region Properties

		#region Public Static Properties

		public static Dictionary<string, Font> Fonts
		{
			get;
			private set;
		}

		public static Dictionary<string, Skin> Skins
		{
			get;
			private set;
		}

		#endregion Public Static Properties

		#endregion Properties

		#region Methods

		#region Public Static Methods

		public static void Create(MiyagiSystem system)
		{
			CreateFonts(system);
			CreateSkins();
		}

		#endregion Public Static Methods

		#region Private Static Methods

		private static void CreateFonts(MiyagiSystem system)
		{
			const string FontPath = @"../../../Media/Gfx/Fonts/";
			var fonts = new[]
						{
                            // load ttf definitions from xml file
                            TrueTypeFont.CreateFromXml(Path.Combine(FontPath, "TrueTypeFonts.xml"), system)
								.Cast<Font>().ToDictionary(f => f.Name),
                            // load image font definitions from xml file
                            ImageFont.CreateFromXml(Path.Combine(FontPath, "ImageFonts.xml"), system)
								.Cast<Font>().ToDictionary(f => f.Name)
						};

			Fonts = fonts.SelectMany(dict => dict)
				.ToDictionary(pair => pair.Key, pair => pair.Value);

			var font = TrueTypeFont.Create(system, "DejaVuSans", Path.Combine(FontPath, "DejaVuSans.ttf"), 12, 96, System.Drawing.FontStyle.Regular);
			Fonts.Add(font.Name, font);

			// set BlueHighway as default font
			Font.Default = Fonts["BlueHighway"];
		}

		private static void CreateSkins()
		{
			// auto create Skins
			var skins = new List<Skin>();

			skins.AddRange(Skin.CreateFromXml(@"../../../Media/Gfx/GUI/skins.xml", null));
			skins.AddRange(Skin.CreateFromXml(@"../../../Media/Gfx/Cursor/CursorSkin.xml", null));

			// manual create Skins
			var logo = new Skin("Logo");
			var rect = RectangleF.FromLTRB(0, 0, 1, 1);
			var frame1 = new TextureFrame("Logo1.png", rect, 1000);
			var frame2 = new TextureFrame("Logo2.png", rect, 800);
			var frame3 = new TextureFrame("Logo3.png", rect, 600);
			var frame4 = new TextureFrame("Logo4.png", rect, 400);
			var frame5 = new TextureFrame("Logo5.png", rect, 200);

			logo.SubSkins["Logo"] = new Miyagi.Common.Resources.Texture(frame1, frame2, frame3, frame4, frame5)
			{
				FrameAnimationMode = FrameAnimationMode.ForwardBackwardLoop
			};

			skins.Add(logo);

			Skins = skins.ToDictionary(s => s.Name);
		}

		#endregion Private Static Methods

		#endregion Methods
	}
}
