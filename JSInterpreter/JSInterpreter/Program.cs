using System;
using System.IO;
using Jint;

namespace JSInterpreter
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var engine = new Engine ().SetValue ("log", new Action<object> (Console.WriteLine));
			var code = @"log('Test run successful')";
			engine.Execute (code);
			Console.Write("Enter the path of a file you want to execute -> ");
			StreamReader openFile = new StreamReader ("C:\\Users\\John\\Desktop\\sample.txt");
			var code1 = openFile.ReadToEnd ();
			Console.WriteLine (code1);
			engine.Execute (code1);
			Console.ReadLine ();
		}
	}
}
