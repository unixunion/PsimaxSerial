using System;
using KSPPluginFramework;
using KSP;
using UnityEngine;
using System.Threading;
using Psimax.IO.Ports;
using KSP.IO;

/*
 * A Stupid plugin which sends chars to an arduino when in FLIGHT!
 * be sure to run FirmataStandard on your Arduino and that its running at 57600 or higher!
 * 
 */

namespace HelloWorldFirmata
{


	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class FirmataTest : MonoBehaviourExtended
	{
		public static PluginConfiguration cfg = PluginConfiguration.CreateForType<FirmataTest>();
		public static Firmata port;
		public static Boolean initialized = false;
		private bool _rcsState = false;
		public static string portName;
		public static int baudRate;
		public static int warmDelay;

		internal override void Start() {
			LogFormatted ("start");
		}

		internal void Begin() 
		{
			LogFormatted("Connecting to Arduino on " + portName + " baud " + baudRate);
			port = new Firmata (portName, baudRate, true, 2000);
			LogFormatted ("Connected");
			port.pinMode (13, Firmata.OUTPUT);
			initialized = true;
		}

		internal override void Awake()
		{
			LogFormatted ("FirmataTest is awake...");
			LogFormatted ("Checking for config");
			cfg.load ();
			portName = cfg.GetValue<string>("portName", "/dev/tty.usbmodem621");
			baudRate = cfg.GetValue<int>("baudRate", 57600);
			warmDelay = cfg.GetValue<int> ("warmupDelay", 2000);
			cfg.save();

			if (!initialized) {
				LogFormatted ("calling init");
				Begin ();
			}

			if (!port.IsOpen) {
				LogFormatted ("port isnt open! dafuq?");
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
				_rcsState = (FlightGlobals.ActiveVessel.ActionGroups [KSPActionGroup.RCS]);
				LogFormatted ("Sending RCS:" + _rcsState);
				port.digitalWrite (13, _rcsState ? Firmata.HIGH : Firmata.LOW);
				LogFormatted ("Done");
			} else {
				LogFormatted ("Not initialized, skipping update");
			}

		}

		private void readEvents() {
			LogFormatted ("readEvents");
		}

		internal override void OnDestroy()
		{
			if (port.IsOpen)
			{
				port.Close();
				Debug.Log("Port closed");
			}
		}


	}
}
