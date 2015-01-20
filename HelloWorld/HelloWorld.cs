using System;
using KSPPluginFramework;
using KSP;
using UnityEngine;
using System.Threading;
using Psimax.IO.Ports;
using KSP.IO;


using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Win32;
using System.Runtime.InteropServices;


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


	// data structure
	public struct Data
	{
		public bool sas;
		public bool rcs;
		public bool gear;
		public bool light;
	}


	[KSPAddon(KSPAddon.Startup.Flight,false)]
	public class PACS : MonoBehaviourExtended
	{
		public static PluginConfiguration cfg = PluginConfiguration.CreateForType<PACS>();
		public static SerialPort port;
		private bool _sasState = false;
		private bool _rcsState = false;
		public static string portName;
		public static int baudRate;

		// data objects
		Data data;

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

			// initialize the data object
			data = getData ();

		}

		public Data getData() {
			Data newData = new Data();
			newData.sas = (FlightGlobals.ActiveVessel.ActionGroups [KSPActionGroup.SAS]);
			newData.rcs = (FlightGlobals.ActiveVessel.ActionGroups [KSPActionGroup.RCS]);
			newData.gear = (FlightGlobals.ActiveVessel.ActionGroups [KSPActionGroup.Gear]);
			newData.light = (FlightGlobals.ActiveVessel.ActionGroups [KSPActionGroup.Light]);
			return newData;
		}

		internal override void Awake()
		{
			LogFormatted("I'm is awake");
			StartRepeatingWorker(10);
		}


		internal override void RepeatingWorker()
		{

			// delta calc



			LogFormatted ("RepeatingWorker");
			LogFormatted ("reading SAS");
			_sasState = (FlightGlobals.ActiveVessel.ActionGroups [KSPActionGroup.SAS]);
			LogFormatted ("reading RCS");
			_rcsState = (FlightGlobals.ActiveVessel.ActionGroups [KSPActionGroup.RCS]);

			LogFormatted ("Sending SAS:" + _sasState + " RCS:" + _rcsState);
			// send the states one at a time
			port.Write (data.sas ? "S" : "s");
			port.Write (data.rcs ? "R" : "r");
			LogFormatted ("Done");
			Thread.Sleep (0);
//			Thread.Sleep(1000);  
		}

		void Update() {

		}


		//these are copied from the intarwebs, converts struct to byte array
		private static byte[] StructureToByteArray(object obj)
		{
			int len = Marshal.SizeOf(obj);
			byte[] arr = new byte[len];
			IntPtr ptr = Marshal.AllocHGlobal(len);
			Marshal.StructureToPtr(obj, ptr, true);
			Marshal.Copy(ptr, arr, 0, len);
			Marshal.FreeHGlobal(ptr);
			return arr;
		}

	}
}
