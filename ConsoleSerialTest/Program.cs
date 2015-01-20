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
			SerialPort port = new SerialPort ("/dev/tty.usbmodem411", 9600);
			port.ReceivedBytesThreshold = 3;
			port.DataReceived += DataCallback;
//			Thread.Sleep (100);
			port.Open ();

			Console.WriteLine ("Sending echo's while the handler waits for replies");
			do {
				while (! Console.KeyAvailable) {
					port.Write("1");
					Thread.Sleep(20);
				}
			} while (Console.ReadKey(true).Key != ConsoleKey.Escape);


			port.DataReceived -= DataCallback;
			port.Close ();
			Thread.Sleep (100);
		}

		public static void DataCallback(object sender, SerialDataReceivedEventArgs e) 
		{
			SerialPort sp = (SerialPort)sender;
			Console.WriteLine ("DataCallBack: " + sp.ReadByte());
		}

	}
}
