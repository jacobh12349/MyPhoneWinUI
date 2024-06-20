using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyPhoneWinUI {
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public partial class App : Application {
		private static bool debug;

		class T : TraceListener {
			#nullable enable

			public override void Write(string? message) {
				if (debug) Debugger.Log(0, null, $"Write: \r\n {Environment.StackTrace} \r\n");
			}

			public override void WriteLine(string? message) {
				if (debug) Debugger.Log(0, null, $"WriteLine: \r\n {Environment.StackTrace} \r\n");
			}
		}

		/*private void Test(string DownloadUrl, string SavePath, string SaveName) {
			try {
				ServicePointManager.SecurityProtocol = (SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12);
				WebRequest webRequest = WebRequest.Create(DownloadUrl);
				webRequest.Proxy = null;
				webRequest.Method = "GET";
				webRequest.Timeout = 30000;
				webRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
				using (HttpWebResponse httpWebResponse = (HttpWebResponse) webRequest.GetResponse()) {
					httpWebResponse.PrintAll();
					long contentLength = httpWebResponse.ContentLength;
					Debug.WriteLine(string.Format("{0} : {1}", "Download File", Path.GetFileName(DownloadUrl)));
					Debug.WriteLine(string.Format("  {0} : {1}", "Date", httpWebResponse.Headers.Get("Date")));
					Debug.WriteLine(string.Format("  {0} : {1}", "Content-Length", httpWebResponse.Headers.Get("Content-Length")));
					Debug.WriteLine(string.Format("  {0} : {1}", "StatusCode", Convert.ToInt32(httpWebResponse.StatusCode)));
					using (Stream stream = httpWebResponse.GetResponseStream()) {
						stream.ReadTimeout = 30000;
						using (FileStream fileStream = new FileStream(SavePath + SaveName + ".bak", FileMode.Create, FileAccess.Write)) {
							int num = 0;
							byte[] array = new byte[1024000];
							do {
								Array.Clear(array, 0, 1024000);
								num = stream.Read(array, 0, array.Length);
								if (num > 0) {
									fileStream.Write(array, 0, num);
								}
							}
							while (num > 0);
							fileStream.Flush();
						}
					}
					if (File.Exists(SavePath + SaveName)) {
						File.Delete(SavePath + SaveName);
					}
					File.Move(SavePath + SaveName + ".bak", SavePath + SaveName);
				}
			}
			catch (Exception e) {
				e.PrintStackTrace();
			}
		}*/

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App() {
			InitializeComponent();
		}

		/// <summary>
		/// Invoked when the application is launched.
		/// </summary>
		/// <param name="args">Details about the launch request and process.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e) {
			m_window = new MyPhoneWindow();
			m_window.Activate();

			AppDomain.CurrentDomain.FirstChanceException += (_, e) => {
				if (!(e.Exception is FormatException && e.Exception.Message == "Token is not valid.") && !(e.Exception is ArgumentException && e.Exception.Message == "Cannot delete a subkey tree because the subkey does not exist.")) {
					MyPhoneWindow.Instance.connection?.Stop();
					//Debugger.Break();
					Debug.WriteLine("First Chance Exception:");
					//e.Exception.PrintAll();
					e.Exception.PrintStackTrace();
					Debug.WriteLine(Environment.StackTrace);
				}
			};

			AppDomain.CurrentDomain.UnhandledException += (_, e) => {
				if (e.ExceptionObject is Exception exception) {
					MyPhoneWindow.Instance.connection?.Stop();
					MyPhoneWindow.Instance.Minimize();
					Debug.WriteLine("Unhandled Exception:");
					exception.PrintStackTrace();
				}
			};

			//debug = e.Arguments.Length == 1 && e.Arguments[0] == "debug";

			//Trace.Listeners.Clear();
			Trace.Listeners.Add(new T());
		}

		private Window m_window;
	}
}
