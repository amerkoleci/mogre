using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Mogre.SampleBrowser
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		Root _root;
		RenderWindow _window;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);


#if DEBUG
			var pluginFileName = "plugins_d.cfg";
#else
			var pluginFileName = "plugins.cfg";
#endif

			MainWindow = new MainWindow();
			MainWindow.Show();

			_root = new Root(pluginFileName);
			InitResources();
			SetupRenderSystem();

			var handle = new WindowInteropHelper(MainWindow).Handle;
			CreateRenderWindow(handle);
			InitializeResources();

			CompositionTarget.Rendering += OnCompositionTargetRendering;
		}

		protected override void OnExit(ExitEventArgs e)
		{
			if (_root != null)
			{
				_root.Dispose();
				_root = null;
			}

			base.OnExit(e);
		}

		protected virtual void InitResources()
		{
			//ConfigFile configFile = new ConfigFile();
			//configFile.Load("resources.cfg", "\t:=", true);
			//ConfigFile.SectionIterator sectionIterator = configFile.GetSectionIterator();
			//while (sectionIterator.MoveNext())
			//{
			//	string currentKey = sectionIterator.CurrentKey;
			//	ConfigFile.SettingsMultiMap current = sectionIterator.Current;
			//	foreach (KeyValuePair<string, string> current2 in current)
			//	{
			//		string key = current2.Key;
			//		string value = current2.Value;
			//		ResourceGroupManager.Singleton.AddResourceLocation(value, key, currentKey);
			//	}
			//}
		}

		private void SetupRenderSystem()
		{
			const string RenderSystemName = "Direct3D11 Rendering Subsystem";
			RenderSystem renderSystemByName = _root.GetRenderSystemByName(RenderSystemName);
			_root.RenderSystem = renderSystemByName;
			renderSystemByName.SetConfigOption("Full Screen", "No");
			renderSystemByName.SetConfigOption("Video Mode", "800 x 600 @ 32-bit colour");
		}

		protected virtual void CreateRenderWindow(IntPtr handle)
		{
			_root.Initialise(false);
			if (handle != IntPtr.Zero)
			{
				_window = _root.CreateRenderWindow("Test RenderWindow", handle, 800, 600);
				return;
			}

			_window = _root.CreateRenderWindow("Test RenderWindow", 800, 600);
		}

		private static void InitializeResources()
		{
			TextureManager.Singleton.DefaultNumMipmaps = 5;
			ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
		}

		private void OnCompositionTargetRendering(object sender, EventArgs e)
		{
			_root.RenderOneFrame();
		}
	}
}