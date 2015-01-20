using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;

// https://antanas.veiverys.com/mono-serialport-datareceived-event-workaround-using-a-derived-class/
// and http://stackoverflow.com/questions/7582087/how-to-mock-serialdatareceivedeventargs

namespace Psimax.IO.Ports
{
	public class EnhancedSerialPort : SerialPort
	{
	
		// a default # of bytes before calling callback
		int received_bytes_threshold = 4000;

		// the alternative to data_received
		object data_callback = new object ();

		// getter / setter for byte count threshold
		// when this is reached, data_callback should be called
		public override int ReceivedBytesThreshold {
			get {
				return received_bytes_threshold;
			}
			set {
				if (value == received_bytes_threshold)
					return;
				received_bytes_threshold = value;
			}
		}

		public EnhancedSerialPort () :base()
		{
		}

		public EnhancedSerialPort (IContainer container) : base (container)
		{
		}

		public EnhancedSerialPort (string portName) : base(portName)
		{
		}

		public EnhancedSerialPort (string portName, int baudRate) :base(portName, baudRate)
		{
		}

		public EnhancedSerialPort (string portName, int baudRate, Parity parity) : base(portName, baudRate, parity)
		{
		}

		public EnhancedSerialPort (string portName, int baudRate, Parity parity, int dataBits) : base(portName, baudRate, parity, dataBits)
		{
		}

		public EnhancedSerialPort (string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits):base(portName, baudRate, parity, dataBits,stopBits)
		{
		}
			

		public new void Open ()
		{
		
			base.Open();

			if (IsWindows == false) {
				new Thread(new ThreadStart(this.EventThreadFunction)).Start();
			}
		}

		static bool IsWindows {
			get {
				PlatformID id = Environment.OSVersion.Platform;
				return id == PlatformID.Win32Windows || id == PlatformID.Win32NT; // WinCE not supported
			}
		}

		private void EventThreadFunction( )
		{
			do
			{
				try
				{
					var _stream = BaseStream;
					if (_stream == null){
						return;
					}

					if (Poll (_stream, ReadTimeout)){
						ConstructorInfo constructor = typeof (SerialDataReceivedEventArgs).GetConstructor(
							BindingFlags.NonPublic | BindingFlags.Instance,
							null,
							new[] {typeof (SerialData)},
							null);
						SerialDataReceivedEventArgs _ea = (SerialDataReceivedEventArgs)constructor.Invoke(new object[] {SerialData.Eof});
						OnDataReceived(_ea);

					}


					if (BytesToRead >= ReceivedBytesThreshold) {
						ConstructorInfo constructor = typeof (SerialDataReceivedEventArgs).GetConstructor(
							BindingFlags.NonPublic | BindingFlags.Instance,
							null,
							new[] {typeof (SerialData)},
							null);
						SerialDataReceivedEventArgs _ea = (SerialDataReceivedEventArgs)constructor.Invoke(new object[] {SerialData.Eof});
						OnDataReceived(_ea);
					}

				}
				catch
				{
					return;
				}
			}
			while (IsOpen);
		}

		internal override void OnDataReceived (SerialDataReceivedEventArgs args)
		{
			SerialDataReceivedEventHandler handler = (SerialDataReceivedEventHandler) Events [data_received];

			if (handler != null) {
				handler (this, args);
			}
		}

		[DllImport ("MonoPosixHelper", SetLastError = true)]
		static extern bool poll_serial (int fd, out int error, int timeout);

		private bool Poll(Stream stream, int timeout)
		{
			CheckDisposed (stream);
			if (IsOpen == false){
				throw new Exception("port is closed");
			}
			int error;

			bool poll_result = poll_serial (fd, out error, ReadTimeout);
			if (error == -1) {
				ThrowIOException ();
			}
			return poll_result;
		}

		[DllImport ("libc")]
		static extern IntPtr strerror (int errnum);

		static void ThrowIOException ()
		{
			int errnum = Marshal.GetLastWin32Error ();
			string error_message = Marshal.PtrToStringAnsi (strerror (errnum));

			throw new IOException (error_message);
		}

		void CheckDisposed (Stream stream)
		{
			bool disposed = (bool)disposedFieldInfo.GetValue(stream);
			if (disposed) {
				throw new ObjectDisposedException (stream.GetType().FullName);
			}
		}
	}

}

