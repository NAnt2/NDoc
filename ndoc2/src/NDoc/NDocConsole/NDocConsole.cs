using System;
using System.IO;

using NDoc.Core;

public class NDocConsole
{
	static void Main(string[] args)
	{
		if (args.Length < 1)
		{
			Console.Error.WriteLine(
				"Usage: NDocConsole [options] assembly[,documentation] ...\n" +
				"Options:\n" +
				"  -d directory         Output directory\n" +
				"  -s style             Output syle\n" +
				"  -t                   Display timing information"
			);
		}
		else
		{
			bool showTime = false;
			string directory = Directory.GetCurrentDirectory();
			string style = "MSDN";
			string assembly = null;
			string documentation = null;

			for (int i = 0; i < args.Length; ++i)
			{
				string arg = args[i];

				if (arg[0] == '-')
				{
					switch (arg.Substring(1))
					{
						case "d":
							directory = args[++i];
							break;
						case "s":
							style = args[++i];
							break;
						case "t":
							showTime = true;
							break;
						default:
							Console.Error.WriteLine("unknown option: {0}", arg);
							break;
					}
				}
				else
				{
					assembly = arg;
					documentation = null;

					int indexOfComma = arg.IndexOf(',');

					if (indexOfComma != -1)
					{
						assembly = arg.Substring(0, indexOfComma);
						documentation = arg.Substring(indexOfComma + 1);
					}
				}
			}

			if (assembly == null)
			{
				Console.Error.WriteLine("nothing to document");
			}
			else
			{
				DateTime start = DateTime.Now;

				Driver driver = new Driver();
				driver.GenerateDocumentation(assembly, documentation, directory, style);

				DateTime end = DateTime.Now;

				if (showTime)
				{
					Console.Error.WriteLine("elapsed time = {0}", end - start);
				}
			}
		}
	}
}
