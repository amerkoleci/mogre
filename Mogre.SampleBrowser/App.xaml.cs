using System.Windows;

namespace Mogre.SampleBrowser
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
    {
        SampleContext _sampleContext;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _sampleContext = new SampleContext();

            MainWindow = new MainWindow();
            MainWindow.DataContext = _sampleContext;
            MainWindow.Show();
        }
    }
}