using System;
using Psimax.IO.Ports;
using System.Threading;
using System.ComponentModel;

class Program
{
	public static void Main()
	{
		SerialPort mySerialPort = new EnhancedSerialPort("/dev/tty.usbmodem621", 9600);
		mySerialPort.ReadBufferSize = 128;
		mySerialPort.DtrEnable = false;
//		mySerialPort.ReceivedBytesThreshold = 3;
//		mySerialPort.BaudRate = 9600;
//		mySerialPort.Parity = Parity.None;
//		mySerialPort.StopBits = StopBits.One;
//		mySerialPort.DataBits = 8;
//		mySerialPort.WriteBufferSize = 1;
//		mySerialPort.ReadBufferSize = 1;
//		mySerialPort.Handshake = Handshake.None;
//		mySerialPort.RtsEnable = true;
//		mySerialPort.DtrEnable = false;
		mySerialPort.ReadTimeout = 100;
		mySerialPort.ReceivedBytesThreshold = 100;
		Console.WriteLine ("ReceivedBytesThreshold = " + mySerialPort.ReceivedBytesThreshold);
		mySerialPort.DataReceived += DataReceivedHandler;
		mySerialPort.Open();
//		mySerialPort.Disposed += SerialDataReceivedEventHandler(DataReceivedHandler);

		Console.WriteLine("Press any key to continue...");
		Console.WriteLine();
		mySerialPort.WriteLine ("a");
		System.Threading.Thread.Sleep(1000);
		mySerialPort.WriteLine ("a");
		mySerialPort.WriteLine ("a");
		while (Console.ReadKey(true).KeyChar != 'x'){
			mySerialPort.Write("012");
			Thread.Sleep (50);
		}
		mySerialPort.Close();
	}

	static void DataReceivedHandler(
		object sender,
		SerialDataReceivedEventArgs e)
	{
		Console.WriteLine("Data Received:");
		SerialPort sp = (SerialPort)sender;
		string indata = sp.ReadExisting();
		Console.Write(indata);
	}
}