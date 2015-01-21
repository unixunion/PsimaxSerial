using System;
using KSPPluginFramework;
using KSP;
using UnityEngine;
using System.Threading;
using Psimax.IO.Ports;
using KSP.IO;

/*
 * A Stupid plugin which sends chars to an arduino when in Flight!
 * 
 * r = RCS off
 * R = RCS ON
 * s = SAS off
 * S = SAS on
 * 
 * 
 */
namespace HelloWorld
{


	[KSPAddon(KSPAddon.Startup.MainMenu, false)]
	public class PACS : MonoBehaviourExtended
	{
		public static PluginConfiguration cfg = PluginConfiguration.CreateForType<PACS>();
		public static SerialPort port;
		public static Boolean initialized = false;
		private bool _sasState = false;
		private bool _rcsState = false;
		public static string portName;
		public static int baudRate;

		internal override void Start() {
			LogFormatted ("start");
		}


		internal void Begin() 
		{
			LogFormatted("Connecting to Arduino on " + portName + " baud " + baudRate);
			port = new SerialPort (portName, baudRate);
			port.ReceivedBytesThreshold = 3;
			port.DataReceived += OnData;
			port.Open ();
			LogFormatted ("Connected");
			initialized = true;
		}
			

		public static void OnData(object sender, SerialDataReceivedEventArgs e) {
			SerialPort sp = (SerialPort)sender;
			Console.WriteLine ("DataCallBack: " + sp.ReadByte());
		}

		internal override void Awake()
		{
			LogFormatted ("PACS is awake...");
			LogFormatted ("Checking for config");
			cfg.load ();
			portName = cfg.GetValue<string>("portName", "/dev/tty.usbserial");
			baudRate = cfg.GetValue<int>("baudRate", 115200);
			cfg.save();

			if (!initialized) {
				LogFormatted ("calling init");
				Begin ();
			}

			if (!port.IsOpen) {
				LogFormatted ("Port is not open fool!");
				Begin ();
			}

			if (!port.IsOpen) {
				LogFormatted ("Port is STILL not open fool!");
			}

			LogFormatted ("Starting worker");
			StartRepeatingWorker (1);
		}


		internal override void RepeatingWorker()
		{
			LogFormatted ("RepeatingWorker");
		}

		internal override void Update() {
			if (initialized) {
				readEvents ();
				LogFormatted ("reading SAS");
				//			_sasState = (FlightGlobals.ActiveVessel.ActionGroups [KSPActionGroup.SAS]);
				LogFormatted ("reading RCS");
				//			_rcsState = (FlightGlobals.ActiveVessel.ActionGroups [KSPActionGroup.RCS]);

				LogFormatted ("Sending SAS:" + _sasState + " RCS:" + _rcsState);
				// send the states one at a time
				port.Write (_sasState ? "S" : "s");
				port.Write (_rcsState ? "R" : "r");
				LogFormatted ("Done");
				Thread.Sleep(1000);  
			} else {
				LogFormatted ("Not initialized, skipping update");
			}

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

		private void readEvents() {
			if (port.IsOpen) {

				// Begin message bute
				while (port.ReadByte() != 0xBE) {
					if (port.BytesToRead == 0)
						return;
				}

				// Second Byte in Begin Message
				if (port.ReadByte () == 0xEF) {
					LogFormatted ("start of command seen!");

					// the length of the message
					int rx_len = (byte)port.ReadByte();

					// the type of the message
					int id = (byte)port.ReadByte();

					switch (id)
					{
					case 0:
						LogFormatted ("message id 0");
						break;
					case 1:
						LogFormatted ("message if 1");
						break;
					}

				}


			}
		}

		internal override void OnDestroy()
		{
			if (port.IsOpen)
			{
				port.Close();
				port.DataReceived -= OnData;
				Debug.Log("Port closed");
			}
		}


	}
}
