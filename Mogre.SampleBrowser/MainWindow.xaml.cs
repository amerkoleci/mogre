using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mogre.SampleBrowser
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();

        }

        void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (ListBoxItem)sender;
            var sampleInfo = item.DataContext as SampleInfo;
            RunSample(sampleInfo);
        }

        void RunSample(SampleInfo sampleInfo)
        {

        }
    }
}
