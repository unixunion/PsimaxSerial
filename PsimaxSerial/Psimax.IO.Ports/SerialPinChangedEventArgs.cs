/* -*- Mode: Csharp; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

using System;

namespace Psimax.IO.Ports
{
	public class SerialPinChangedEventArgs : EventArgs
	{
		internal SerialPinChangedEventArgs (SerialPinChange eventType)
		{
			this.eventType = eventType;
		}

		// properties

		public SerialPinChange EventType {
			get {
				return eventType;
			}
		}

		SerialPinChange eventType;
	}
}

