using System;

namespace HWTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Arduino Hardware Test");
			PsimaxSerialTest p = new PsimaxSerialTest {RunLoop = true};
			while ( p.RunLoop ) p.loop();
			p.exit();

		}
	}
}
