using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Runtime.InteropServices;
using Psimax.IO.Ports;

namespace HWTest
{
	public class PsimaxSerialTest
	{
		public bool RunLoop { get; set; }

		String[] portList;
		public static SerialPort port;
		bool negotiated;
		DateTime startHandshakeTime;
		TimeSpan deltaTime;

		// packet id's
		public const byte HSPid = 0, VDid = 1, Cid = 101;

		// handshake struct
		public struct HandShakePacket
		{
			public byte id;
			public byte M1;
			public byte M2;
			public byte M3;
		}

		// scanning for start of message
		private static byte rx_len;
		private static byte rx_array_inx;

		// packet declarations
		public static ControlPacket CPacket;
		private static HandShakePacket HPacket;


		public PsimaxSerialTest ()
		{
			Console.WriteLine ("New Instance of PsimaxSerialTest");
			portList = SerialPort.GetPortNames();
			port = new SerialPort();
			port.PortName = "/dev/tty.usbmodem621";
			port.Handshake = Handshake.None;
			port.DataBits = 8;
			port.Parity = Parity.None;
			port.StopBits = StopBits.One;
			port.DataReceived += onData;
			negotiated = false;
//			findArduino ();
			begin ();

		}


		// read input from serial
		public void loop() {
			Console.WriteLine ("Buffer: " + port.BytesToRead);

			while (port.BytesToRead > 0) {
				if (startOfCom()) {
					// pass
				}
			}
			

			byte[] buffer = new byte[port.ReadBufferSize];
			int bytesRead = port.Read(buffer, 0, buffer.Length); 


			Console.WriteLine("read: " + Encoding.ASCII.GetString(buffer,0, bytesRead));


		}

		
		private static bool processCOM()
		{
			byte calc_CS;

			if (rx_len == 0)
			{
				while (port.ReadByte() != 0xBE)
				{
					if (port.BytesToRead == 0)
						return false;
				}

				if (port.ReadByte() == 0xEF)
				{
					rx_len = (byte)port.ReadByte();
					id = (byte)port.ReadByte();
					rx_array_inx = 1;

					switch (id)
					{
					case HSPid:
						structSize = Marshal.SizeOf(HPacket);
						break;
					case Cid:
						structSize = Marshal.SizeOf(CPacket);
						break;
					}

					//make sure the binary structs on both Arduino and plugin are the same size.
					if (rx_len != structSize || rx_len == 0)
					{
						rx_len = 0;
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			else
			{
				while (Port.BytesToRead > 0 && rx_array_inx <= rx_len)
				{
					buffer[rx_array_inx++] = (byte)Port.ReadByte();
				}
				buffer[0] = id;

				if (rx_len == (rx_array_inx - 1))
				{
					//seem to have got whole message
					//last uint8_t is CS
					calc_CS = rx_len;
					for (int i = 0; i < rx_len; i++)
					{
						calc_CS ^= buffer[i];
					}

					if (calc_CS == buffer[rx_array_inx - 1])
					{//CS good
						rx_len = 0;
						rx_array_inx = 1;
						return true;
					}
					else
					{
						//failed checksum, need to clear this out anyway
						rx_len = 0;
						rx_array_inx = 1;
						return false;
					}
				}
			}

			return false;
		}


		// direct connect to test arduino
		void begin() {
			port.Open ();
		}

		public void exit() {
			port.Close ();
		}

		void findArduino()
		{
			foreach (string portName in portList)
			{
				try
				{
					port.Close();
					Console.WriteLine("Trying port " + portName);
					port.PortName = portName;
					port.BaudRate = 9600;
					port.DataReceived -= onData;
					port.DataReceived += onData;
					port.Open();
					Console.WriteLine("Port open, waiting for reset cycle to complete");
					Thread.Sleep(3000);
					sendHandshake();
					sendHandshake();
//					port.Write("s");
					startHandshakeTime = DateTime.Now;
					deltaTime = DateTime.Now - startHandshakeTime;
					while (!negotiated && deltaTime.TotalMilliseconds < 3000)
					{
						// just sleep
//						port.Write("s");
//						sendHandshake();
						Console.WriteLine(portName + ": waiting..." + deltaTime.TotalMilliseconds);
						Thread.Sleep(100);
						deltaTime = DateTime.Now - startHandshakeTime;
					}

					port.Close();

					if ( negotiated )
					{
						Console.WriteLine("Port found: " + portName);
					} else {
						Console.WriteLine("next port");
						port.Close();
					}

				}
				catch (Exception e) 
				{
					Console.WriteLine("Error trying to open port: " + portName);
//					Console.WriteLine (e);
				}
			}
		}


		void sendHandshake()
		{
			HandShakePacket hp = new HandShakePacket();
			hp.id = HSPid;
			hp.M1 = 1;
			hp.M2 = 2;
			hp.M3 = 3;
			sendPacket(hp);
		}

		public static void sendPacket(object data)
		{
			byte[] payload = StructureToByteArray (data);
			byte header1 = 0xBF;
			byte header2 = 0xEF;
			byte size = (byte)payload.Length;
			byte checksum = size;

			byte[] packet = new byte[size + 4];


			for (int i = 0; i < size; i++)
			{
				checksum ^= payload[i];
			}

			payload.CopyTo(packet, 3);
			packet[0] = header1;
			packet[1] = header2;
			packet[2] = size;
			packet[packet.Length - 1] = checksum;

			port.Write(packet, 0, packet.Length);

		}



		private static void onData(object sender, SerialDataReceivedEventArgs e)
		{
			Console.WriteLine ("Data from port");
		}


		// converts a struct data to byte array
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

