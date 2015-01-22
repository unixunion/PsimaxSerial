#PsimaxSerial

A Serial communications library for Kerbal Space Program! Made from genuine recycled other serial libraries!

## About
This plugin is basically a duplication of the System.IO.Ports namespace with one or two tweaks and depends on Mono.Posix and System only. It also support Firmata.

## Whats New

### Firmata
Firmata support has now been added based off Tim Farley's Arduion.cs. 

## Features

* SerialPort access from KSP Addon's
* Firmata protocol support

## Known Issues/Limitations

### DataReceived Event Callback
Due to an apparent Mono and Event issue when running code via Unity, the OnDataReceived method within the serial driver never gets called on Mac / *nix at least. My recommendation is to check the port instance for available bytes during Unity's MonoBehaviour->Update() call. See my fork of KSPSerialIO for an example. Make sure to run the serial device at a decent speed so you dont hold up the Update() routine.

## Config
AFter running HelloWorld ( by actually flying a craft ), the config.xml will exist int he plugin data directory. If you are impatient, here is a example config.

	<?xml version="1.0" encoding="utf-8"?>
	<config>
    	<string name="portName">/dev/tty.usbmodem621</string>
    	<int name="baudRate">115200</int>
	</config>

## IDE Setup

Xamarin Studio on Mac, projects are .NET 3.5 targets. 

### HelloWorld

#### Project References

* Mono.Posix ( local copy enabled ) 
* UnityEngine 
* Assemby-CSharp
* PsimaxSerial

See HelloWorld.cs

When delivering your addon, be sure to Copy

* PsimaxSerial.dll
* Mono.Posix.dll

### PsimaxSerial

#### Project References

* Mono.Posix(local copy enabled)
* System



## LogOutput

	[LOG 15:00:33.479] 1/19/2015 3:00:33 PM,HelloWorld,PACS is starting up...
	[LOG 15:00:33.479] 1/19/2015 3:00:33 PM,HelloWorld,Connecting to Arduino
	[LOG 15:00:33.490] 1/19/2015 3:00:33 PM,HelloWorld,Connected
	[LOG 15:00:37.756] 1/19/2015 3:00:37 PM,HelloWorld,RepeatingWorker
	[LOG 15:00:37.756] 1/19/2015 3:00:37 PM,HelloWorld,reading SAS
	[LOG 15:00:37.756] 1/19/2015 3:00:37 PM,HelloWorld,reading RCS
	[LOG 15:00:37.756] 1/19/2015 3:00:37 PM,HelloWorld,Sending SAS:False RCS:False
	[LOG 15:00:37.756] 1/19/2015 3:00:37 PM,HelloWorld,Done
	[LOG 15:00:40.236] 1/19/2015 3:00:40 PM,HelloWorld,RepeatingWorker
	[LOG 15:00:40.236] 1/19/2015 3:00:40 PM,HelloWorld,reading SAS
	[LOG 15:00:40.236] 1/19/2015 3:00:40 PM,HelloWorld,reading RCS
	[LOG 15:00:40.237] 1/19/2015 3:00:40 PM,HelloWorld,Sending SAS:False RCS:False
	[LOG 15:00:40.237] 1/19/2015 3:00:40 PM,HelloWorld,Done
