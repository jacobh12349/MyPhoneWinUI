using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace MyPhoneWinUI {
	public class TcpClient2 : TcpClient {
		public bool Closed = false;
		public NetworkStream Stream => GetStream();

		public TcpClient2() : base() { Debug.WriteLine("TCPClient2 Created");/* Debug.WriteLine(Environment.StackTrace);*/ }

		public new void Close() {
			//Client.Disconnect(true);
			base.Close();
			Closed = true;
			Debug.WriteLine("TCPClient2 Closed");
			//Debug.WriteLine(Environment.StackTrace);
			//new Exception().PrintStackTrace();
		}
	}
}
