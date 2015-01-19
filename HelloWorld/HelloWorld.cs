using System;
using KSPPluginFramework;
using KSP;
using UnityEngine;
using System.Threading;
using Psimax.IO.Ports;


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

		public static SerialPort port;
		private bool _sasState = false;
		private bool _rcsState = false;

		internal override void Start() {
			LogFormatted ("PACS is starting up...");
			LogFormatted("Connecting to Arduino");
			port = new SerialPort ("/dev/tty.usbserial", 115200);
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
