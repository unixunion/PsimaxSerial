using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using Psimax.IO.Ports;

namespace SerialTest
{
	public class App
	{

		public static SerialPort port;
		int byteThreshold = 100;

		public App ()
		{
			port = new SerialPort("/dev/tty.usbmodem621", 9600);
			port.Open ();

			// start a thread to monitor bytes
			new Thread(new ThreadStart(this.EventThreadFunction)).Start();

		}


		private void onData() {
			Console.WriteLine ("onData: " + port.ReadExisting ());
		}

		private void EventThreadFunction( )
		{
			do
			{
				try
				{
					var _stream = port.BaseStream;
					if (_stream == null){
						return;
					}
						
					// call data
					if (port.BytesToRead >= byteThreshold) {
						onData();
					}

				}
				catch
				{
					return;
				}
			}
			while (port.IsOpen);
		}


	}
}

