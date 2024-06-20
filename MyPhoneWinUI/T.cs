using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyPhoneWinUI {
	public static class T {
		public static void PrintAll(this object o) {
			if (o == null) { Debug.WriteLine("null"); return; }
			Debugger.Log(0, null, $"{o.GetType().FullName} Properties:" + "\r\n");
			//o.GetType().GetProperties().OrderBy(p => p.Name).ForEach(p => Debug.WriteLine(p.Name));
			o.GetType().GetProperties().Where(p => p.GetValue(o) != null).OrderBy(p => p.Name).ForEach(p => { try { Debugger.Log(0, null, p.Name); Debugger.Log(0, null, $":{p.GetValue(o).GetType().FullName}:{p.GetValue(o)}\r\n"); } catch (Exception e) { Console.WriteLine("Error happened when getting value of " + p.Name); e.PrintStackTrace(); } });
			//o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(p => p.GetValue(o) != null).OrderBy(p => p.Name).ForEach(p => { try { Debugger.Log(0, null, $"{p.Name}:{p.GetValue(o)} {(p.PropertyType == typeof(string) ? ((string) p.GetValue(o)).Length : "")}\r\n"); } catch (Exception e) { Console.WriteLine("Error happened when getting value of " + p.Name); e.PrintStackTrace(); } });
			//o.GetType().GetProperties().Where(p => p.GetValue(o) != null).OrderBy(p => p.Name).Select(p => $"{p.Name}:{p.GetValue(o)} {(p.PropertyType == typeof(string) ? ((string) p.GetValue(o)).Length : "")}").ToList().ForEach(s => Debugger.Log(0, null, s + "\r\n"));
			Debugger.Log(0, null, " ");
			Debugger.Log(0, null, "\r\n\r\n");
			//Debugger.Log(0, null, "\r\n");
		}

		public static void PrintAll(this Image c) {
			MyPhoneWindow.Instance.Dispatcher.Invoke(() => {
				if (c == null) { Debug.WriteLine("null"); return; }
				Debugger.Log(0, null, $"{c.GetType().Name} Properties:" + "\r\n");
				/*c.GetType().GetProperties().
				Where(p => c.Dispatcher.Invoke(() => p.GetValue(c)) != null).
				OrderBy(p => p.Name).
				Select(p => $"{p.Name}:" +
				c.Dispatcher.Invoke(() => p.GetValue(c))).
				ToList().
				ForEach(s => Debugger.Log(0, null, s + "\r\n"));*/

				Debugger.Log(0, null, MyPhoneWindow.Instance.Dispatcher.Invoke(() => c.Source.ToString()) + "\r\n");
				Debugger.Log(0, null, MyPhoneWindow.Instance.Dispatcher.Invoke(() => c.ActualWidth) + "\r\n");
				Debugger.Log(0, null, MyPhoneWindow.Instance.Dispatcher.Invoke(() => c.ActualHeight) + "\r\n");
				Debugger.Log(0, null, " ");
				Debugger.Log(0, null, "\r\n\r\n");
				//Debugger.Log(0, null, "\r\n");
			});
		}

		public static void PrintAll(this object[] array) {
			Debugger.Log(0, null, string.Join("\n", array) + "\r\n");
			Debugger.Log(0, null, "\r\n\r\n");
		}

		public static void PrintAll(this IEnumerable<string> list) => Debugger.Log(0, null, string.Join("\n", list) + "\r\n");

		public static void PrintAll<T>(this IEnumerable<T> enumerable) {
			enumerable.ForEach(o => o.PrintAll());
			//Debugger.Log(0, null, "\r\n\r\n");
		}

		public static void PrintAll<TKey, TValue>(this Dictionary<TKey, TValue> dict) {
			Dictionary<TKey, TValue>.Enumerator e = dict.GetEnumerator();
			if (!e.MoveNext()) { Debug.WriteLine("{}"); return; };

			for (; ; ) {
				Debug.WriteLine($"{e.Current.Key}: {e.Current.Value}");
				if (!e.MoveNext()) break;
				//if (!e.MoveNext()) return;
				Debug.WriteLine("\r\n");
			}
		}

		public static void PrintStackTrace(this Exception e) => Debugger.Log(1, null, e + "\r\n");

		/*public static void PrintStackTrace(this Exception e) {
			Debugger.Log(1, null, e.ToString() + "\r\n");
			//Debug.WriteLine(e.ToString());
			//Debug.Fail(e.ToString());
		}*/

		public static void Maximize(this Window window) {
			if (window.AppWindow.Presenter is OverlappedPresenter presenter) presenter.Maximize();
		}

		public static void Minimize(this Window window) {
			if (window.AppWindow.Presenter is OverlappedPresenter presenter) presenter.Minimize();
		}

		public static void ForEach<T>(this T[] array, Action<T> action) {
			Array.ForEach(array, action);
			//array.ToList().ForEach(action);
		}

		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
			foreach (T t in enumerable) action(t);
			//enumerable.ToArray().ForEach(action);
		}

		/*public static void ForEach(this IEnumerable enumerable, Action<object> action) {
			foreach(object o in enumerable) action(o);
		}*/

		//public static string[] Split(this string str, string split) => str.Split(split.ToCharArray());

		//public static string[] Split(this string str, string split, int count) => str.Split(split, count);

		public static bool IsEmpty(this IList list) => list.Count == 0;

		public static bool IsEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dict) => dict.Count == 0;

		public static bool IsEmpty(this string str) => str.Length == 0;

		public static string ToString1<TKey, TValue>(this Dictionary<TKey, TValue> dict) {
			Dictionary<TKey, TValue>.Enumerator e = dict.GetEnumerator();
			if (!e.MoveNext()) return "{}";

			StringBuilder sb = new();
			sb.Append('{');
			for (; ; ) {
				sb.Append(e.Current.Key);
				sb.Append('=');
				sb.Append(e.Current.Value);
				if (!e.MoveNext()) return sb.Append('}').ToString();
				sb.Append(',').Append(' ');
			}
		}

		public static BitmapSource ToImage(this byte[] bytes) {
			BitmapSource b = (BitmapSource) new ImageSourceConverter().ConvertFrom(bytes);
			MyPhoneWindow.ImageBytes.Add(b, bytes);
			return b;

			/*BitmapFrame.Create((BitmapSource) new ImageSourceConverter().ConvertFrom(bytes));
			BitmapSource b2 = ;
			BitmapSource2 b = (BitmapSource2) b2;
			b.Bytes = bytes;
			return b;*/
		}

		public static TransformedBitmap ToImage(this byte[] bytes, double width, double height) {
			BitmapSource image = bytes.ToImage();
			return new TransformedBitmap(image, new ScaleTransform(width / image.PixelWidth, height / image.PixelHeight));
		}

		public static BitmapSource ToImage(this string base64) => Convert.FromBase64String(base64).ToImage();

		//public static BitmapSource ToImage2(this string hex) => hex..ToImage();

		public static TransformedBitmap ToImage(this string base64, double width, double height) => Convert.FromBase64String(base64).ToImage(width, height);

		public static TransformedBitmap ToImage(this string base64, Size size) => Convert.FromBase64String(base64).ToImage(size.Width, size.Height);

		//public static BitmapImage ToImage(this byte[] bytes) {
		//	BitmapImage image = new();
		//	using (MemoryStream stream = new(bytes)) {
		//		image.BeginInit();
		//		image.StreamSource = stream;
		//		image.EndInit();
		//	}
		//	image.Freeze();
		//	return image;
		//}

		public static byte[] GetBytes(this System.Drawing.Image image) => (byte[]) new System.Drawing.ImageConverter().ConvertTo(image, typeof(byte[]));

		public static byte[] GetBytes(this BitmapSource image) {
			//Debug.WriteLine(MyPhoneWindow.ImageBytes.ContainsKey(image));

			if (MyPhoneWindow.ImageBytes.TryGetValue(image, out byte[]? value)) return value;
			else {
				using MemoryStream s = new();
				//BitmapEncoder encoder;
				//if ()
				new JpegBitmapEncoder() { Frames = [BitmapFrame.Create(image)], QualityLevel = 100 }.Save(s);
				//((BitmapImage) image).StreamSource.CopyTo(s);
				return s.GetBuffer();
			}

			/*if (image.Bytes != null) return image.Bytes;
			else {
				using MemoryStream s = new();
				//BitmapEncoder encoder;
				//if ()
				new JpegBitmapEncoder() { Frames = [BitmapFrame.Create(image)], QualityLevel = 100 }.Save(s);
				//((BitmapImage) image).StreamSource.CopyTo(s);
				return s.GetBuffer();
			}*/
			//return (byte[]) new ImageSourceConverter().ConvertTo(image, typeof(byte[]));
		}

		public static void FlashWindow(this Window window, bool flash) => FlashWindowP(new WindowInteropHelper(window).Handle, flash);

		// TODO: maybe eventually change this to use LibraryImport instead.
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Int eroperability", "SYSLIB1054", Justification = "<Pending>")]
		[DllImport("user32.dll", EntryPoint = "FlashWindow")]
		private static extern bool FlashWindowP(IntPtr hwnd, bool bInvert);

		/*public static string Text(this RichTextBox textBox) {
			return new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd).Text;
		}*/

		public static string Text(this RichTextBox textBox) {
			return !textBox.Document.Blocks.IsEmpty() ? new TextRange(textBox.Document.Blocks.FirstBlock.ContentStart, textBox.Document.Blocks.FirstBlock.ContentEnd).Text : "";
		}

		/*public static void ForEach(this IEnumerable list, Action<T> action) {

		}*/

		// TODO: eventually change code to use this extension method
		public static List<T> GetChildrenOfTag<T>(this Panel panel, object tag) where T : FrameworkElement {
			return panel.Children.OfType<FrameworkElement>().Where(c => c is T && c.Tag == tag).OfType<T>().ToList();
			//return panel.Children.OfType<T>().Where(c => c.Tag == tag).ToList();
		}

		//public static string[] WrappedLines(this RichTextBox textBox) {
		//	bool b = true;
		//	int i = 1;
		//	int j = 0;
		//	List<string> lines = new();

		//	while (b) {
		//		int index = textBox.GetFirstCharIndexFromLine(i);
		//		if (index != -1) {
		//			lines.Add(textBox.Text[j..index].Replace("\r", "").Replace("\n", "").Replace("", ""));
		//			j = index;
		//			i++;
		//		}
		//		else {
		//			lines.Add(textBox.Text[j..].Replace("\r", "").Replace("\n", "").Replace("", ""));
		//			b = false;
		//		}
		//	}
		//	return lines.ToArray();
		//}

		public static string[] WrappedLines(this TextBox textBox) {
			if (textBox.LineCount == -1) return [];
			List<string> lines = [];
			for (int i = 0; i < textBox.LineCount; i++) lines.Add(textBox.GetLineText(i));
			return [.. lines];
		}

		// TODO: eventually figure out how to get wrapped lines of textblock
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
		public static string[] WrappedLines(this TextBlock textBlock) {
			/*if (textBox.LineCount == -1) return Array.Empty<string>();
			List<string> lines = new();
			for (int i = 0; i < textBox.LineCount; i++) lines.Add(textBox.GetLineText(i));
			return lines.ToArray();*/

			return [];
		}

		/*public static string[] WrappedLines(this TextBox textBox) {
			bool b = true;
			int i = 0;
			int j = 0;
			List<string> lines = new();

			while (b) {
				if (i < textBox.LineCount) {
					int index = textBox.GetCharacterIndexFromLineIndex(i);
					lines.Add(textBox.Text[j..index].Replace("\r", "").Replace("\n", ""));
					j = index;
					i++;
				}
				else {
					lines.Add(textBox.Text[j..].Replace("\r", "").Replace("\n", ""));
					b = false;
				}
			}
			return lines.ToArray();
		}*/

		//public static void CallResized(this RichTextBox textBox, bool call) {
		//	try { typeof(RichTextBox).GetProperty("CallOnContentsResized").SetValue(textBox, call); } catch (Exception e) { e.PrintStackTrace(); }
		//}

		public static string Header(this TabItem? tab) => (string) tab.Header;

		public static IEnumerable<TabItem> Tabs(this TabControl tabControl) => tabControl.Items.OfType<TabItem>();

		public static List<TabItem> AddedItems(this SelectionChangedEventArgs e) => e.AddedItems.OfType<TabItem>().ToList();

		public static MessagesPanel MessagesPanel(this TabItem tab) => (MessagesPanel) tab.Content;

		// TODO: eventually change all classes to use these new show and hide methods
		public static void Show(this UIElement control) {
			control.Visibility = Visibility.Visible;
		}

		public static void Hide(this UIElement control) {
			control.Visibility = Visibility.Collapsed;
		}

		public static void Hide2(this UIElement control) {
			control.Visibility = Visibility.Hidden;
		}
	}
}
