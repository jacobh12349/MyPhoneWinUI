using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Windowing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Diagnostics;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Media.Playback;
using Windows.Storage.Streams;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyPhoneWinUI {
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MyPhoneWindow : Window {
		public static MyPhoneWindow Instance { get; private set; }
		private readonly MediaPlayer notificationPlayer;
		private InMemoryRandomAccessStream notificationStream;
		public Connection? connection;
		public OpenFileDialog fileChooser;
		public SaveFileDialog fileSaver;

		//public AppNotificationManager notificationManager;

		public static Dictionary<BitmapSource, byte[]> ImageBytes = [];
		public MyPhoneWindow() {
			//Application.Current.DispatcherUnhandledException += (s, e) => Debug.WriteLine("UnhandledException:" );

			notificationPlayer = new() { Volume = 0.15 };
			notificationPlayer.MediaEnded += delegate { notificationPlayer.Source = null; };

			notificationStream = new();
			//RandomAccessStream.CopyAndCloseAsync(Properties.Resources.notification.AsInputStream(), notificationStream.GetOutputStreamAt(0)).Cancel();

			Instance = this;

			// TODO: Test creating connection before window is loaded more.
			//connection = new(this);

			InitializeComponent();
			this.Maximize();
			//Debug.WriteLine(AppWindow.Presenter.GetType().FullName);
		}

		private void ChangedTab(object sender, SelectionChangedEventArgs e) {

		}

		private void OnInput(object sender, KeyRoutedEventArgs e) {
			//if (tabs.SelectedItem )
		}
	}
}
