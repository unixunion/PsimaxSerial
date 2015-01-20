#PsimaxSerial

A Serial communications library for Kerbal Space Program! Made from genuine recycled other serial libraries around Keb!

## About
This plugin is basically a duplication of the System.IO.Ports namespace with one or two tweaks and depends on Mono.Posix and System only.

## Known Issues/Limitations

### DataReceived Event Callback
I have not been able to get DataReceived events to fire, but this is a general problem with Mono/.NET and I believe it is due to the Arduino lacking RTS, DTS and DTR signalling. I am investigating possible solutions. It seems the methods are just never called.

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

## Config
AFter running HelloWorld ( by actually flying a craft ), the config.xml will exist int he plugin data directory. If you are impatient, here is a example config.

	<?xml version="1.0" encoding="utf-8"?>
	<config>
    	<string name="portName">/dev/tty.foo</string>
    	<int name="baudRate">115200</int>
	</config>

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
