using System.Windows;

namespace Mogre.SampleBrowser
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			using (var root = new Root())
			{

			}
		}
	}
}