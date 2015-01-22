using System;
using KSPPluginFramework;
using KSP;
using UnityEngine;
using System.Threading;
using Psimax.IO.Ports;
using KSP.IO;

/*
 * A Stupid plugin which sends chars to an arduino when in FLIGHT!
 * 
 * r = RCS off
 * R = RCS ON
 * s = SAS off
 * S = SAS on
 * 
 * // arduino loop code, shows when RCS state changes in FLIGHT!

void setup() {
	Serial.begin(57600); 
	pinMode(13, OUTPUT); 
	digitalWrite(13, LOW);
}

void loop() {
	if (Serial.available() > 0) {
		char c = (char)Serial.read();
		if (c == 'R') {
			digitalWrite(13, HIGH);
		}
		if (c == 'r') {
			digitalWrite(13, LOW);
		}
		if (c == 'S') {


		}
		if (c == 's') {

		}

	}
}
 * 
 * 
 */
namespace HelloWorld
{


	[KSPAddon(KSPAddon.Startup.Flight, false)]
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
			Console.WriteLine ("OnData callback");
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
				LogFormatted ("reading data");
				_sasState = (FlightGlobals.ActiveVessel.ActionGroups [KSPActionGroup.SAS]);
//				LogFormatted ("reading RCS");
				_rcsState = (FlightGlobals.ActiveVessel.ActionGroups [KSPActionGroup.RCS]);

				LogFormatted ("Sending SAS:" + _sasState + " RCS:" + _rcsState);
				// send the states one at a time
				port.Write (_sasState ? "S" : "s");
				port.Write (_rcsState ? "R" : "r");
				LogFormatted ("Done");
			} else {
				LogFormatted ("Not initialized, skipping update");
			}

		}

		private void readEvents() {
			if (port.IsOpen) {
				LogFormatted ("readEvents and port is open!");
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
