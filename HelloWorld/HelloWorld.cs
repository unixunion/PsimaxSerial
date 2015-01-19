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


	[KSPAddon(KSPAddon.Startup.Flight,false)]
	public class PACS : MonoBehaviourExtended
	{
		public static PluginConfiguration cfg = PluginConfiguration.CreateForType<PACS>();
		public static SerialPort port;
		private bool _sasState = false;
		private bool _rcsState = false;
		public static string portName;
		public static int baudRate;

		internal override void Start() {
			LogFormatted ("PACS is starting up...");

			LogFormatted ("Checking for config");
			cfg.load ();
			portName = cfg.GetValue<string>("portName", "/dev/tty.usbserial");
			baudRate = cfg.GetValue<int>("baudRate", 115200);
			cfg.save();

			LogFormatted("Connecting to Arduino on " + portName + " baud " + baudRate);
			port = new SerialPort (portName, baudRate);
			port.Open ();
			LogFormatted ("Connected");
		}


		internal override void Awake()
		{
			LogFormatted("I'm is awake");
			StartRepeatingWorker(1);
		}


		internal override void RepeatingWorker()
		{
			LogFormatted ("RepeatingWorker");
			LogFormatted ("reading SAS");
			_sasState = (FlightGlobals.ActiveVessel.ActionGroups [KSPActionGroup.SAS]);
			LogFormatted ("reading RCS");
			_rcsState = (FlightGlobals.ActiveVessel.ActionGroups [KSPActionGroup.RCS]);

			LogFormatted ("Sending SAS:" + _sasState + " RCS:" + _rcsState);
			// send the states one at a time
			port.Write (_sasState ? "S" : "s");
			port.Write (_rcsState ? "R" : "r");
			LogFormatted ("Done");
			Thread.Sleep(1000);  
		}




	}
}
