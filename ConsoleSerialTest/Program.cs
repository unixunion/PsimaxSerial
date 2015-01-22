using System;
using System.Threading;
using Psimax.IO.Ports;

/* This is a test of the OnData callback
 * Arduino sketch to test this

int inByte;

void setup() {
  // put your setup code here, to run once:
 Serial.begin(9600);
 pinMode(13, OUTPUT);
 digitalWrite(13, LOW);
}

void loop() {
  // put your main code here, to run repeatedly:
  if (Serial.available() > 0) {
    digitalWrite(13, HIGH);
    inByte = Serial.read();
    Serial.print("0");
  }
}


 */

namespace ConsoleSerialTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			SerialPort port = new SerialPort ("/dev/tty.usbmodem621", 115200);
			port.ReceivedBytesThreshold = 10;
			port.DataReceived += DataCallback;
			port.Open ();

			Console.WriteLine ("Sending echo's while the handler waits for replies");
			do {
				while (! Console.KeyAvailable) {
					port.Write("1");
					Thread.Sleep(50);
				}
			} while (Console.ReadKey(true).Key != ConsoleKey.Escape);


			port.DataReceived -= DataCallback;
			port.Close ();
			Thread.Sleep (100);
		}

		public static void DataCallback(object sender, SerialDataReceivedEventArgs e) 
		{
			SerialPort sp = (SerialPort)sender;
			Console.WriteLine ("Data callback");
			while (sp.BytesToRead==0) {
				Console.Write ("databyte: " + sp.ReadByte ());
			}
			Console.WriteLine("out of bytes");
			readEvents (sp);
		}


		static byte[] GetBytes(string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		static string GetString(byte[] bytes)
		{
			char[] chars = new char[bytes.Length / sizeof(char)];
			System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
			return new string(chars);
		}


		private static void readEvents(SerialPort port) {
			if (port.IsOpen) {

				while (port.ReadByte() != 0xBE) {
					if (port.BytesToRead == 0)
						return;
				}

				if (port.ReadByte () == 0xEF) {
					Console.WriteLine ("start of packet seen!");

					byte payloadLen = (byte)port.ReadByte ();
					Console.WriteLine ("payloadLen: " + payloadLen);
					byte id = (byte)port.ReadByte ();

					Console.WriteLine ("id byte: " + id);
					byte[] payload = new byte[payloadLen];
				
					int i = 0;
					while (i <= payloadLen) {
//						port.Read (payload, 0, payloadLen);
						payload [i] = (byte)port.ReadByte ();
						i++;
					}

					Console.WriteLine ("payload:" + GetString (payload));
					byte cs = (byte)port.ReadByte ();
					Console.WriteLine ("cs: " + cs);


					switch (id) {
					case 0:
						Console.WriteLine ("message id 0");
						Console.WriteLine (GetString (payload));
						break;
					case 1:
						Console.WriteLine ("message if 1");
						break;
					}

				} else {
					port.ReadByte ();
				}


			}
		}



	}
}
