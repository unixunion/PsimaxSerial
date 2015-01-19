#PsimaxSerial

Serial Arduino communication via a KSP addon.

## About
This plugin is basically a duplication of the System.IO.Ports namespace with one or two tweaks and depends on Mono.Posix and System only.

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
