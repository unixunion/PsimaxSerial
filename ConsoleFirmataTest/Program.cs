using System;
using System.Threading;
using Psimax.IO.Ports;

namespace ConsoleFirmataTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("init");
			Firmata f = new Firmata ("/dev/tty.usbmodem621", 57600, true, 3000);
			Console.WriteLine ("Set pin output");
			f.pinMode (13, Firmata.OUTPUT);
			f.pinMode (8, Firmata.INPUT);

			Console.WriteLine ("set 13 high");
			f.digitalWrite (13, Firmata.HIGH);

			do {
				while (! Console.KeyAvailable) {

					Console.WriteLine("8: " + f.digitalRead(8));

//					Thread.Sleep(250);
//					f.digitalWrite (13, Firmata.HIGH);
				}
			} while (Console.ReadKey(true).Key != ConsoleKey.Escape);


			Console.WriteLine ("done");
			f.Close ();

		}
	}
}
