using System;
using System.IO;
using System.Reflection;

namespace NDoc.Core
{
	public class Driver
	{
		public void GenerateDocumentation(
			string assemblyFile, 
			string documentationFile,
			string outputDirectory,
			string outputStyle)
		{
			Directory.CreateDirectory(outputDirectory);

			Assembly assembly = Assembly.LoadFrom(assemblyFile);
			AssemblyNavigator assemblyNavigator = new AssemblyNavigator(assembly);

			string styleDirectory = Path.Combine(@"..\..\..\", outputStyle);

			CopyResources(styleDirectory, outputDirectory);

			Template template = new Template();
			template.Load(Path.Combine(styleDirectory, @"templates\namespace.xml"));

			assemblyNavigator.MoveToFirstNamespace();

			do
			{
				string fileName = assemblyNavigator.NamespaceName + ".html";
				string outputFile = Path.Combine(outputDirectory, fileName);
				StreamWriter streamWriter = new StreamWriter(File.Open(outputFile, FileMode.Create));

				template.EvaluateNamespace(
					assemblyNavigator.NamespaceName, 
					assemblyNavigator, 
					documentationFile, 
					streamWriter);

				streamWriter.Close();
			}
			while (assemblyNavigator.MoveToNextNamespace());
		}

		private void CopyResources(string styleDirectory, string outputDirectory)
		{
			foreach (string file in Directory.GetFiles(Path.Combine(styleDirectory, "resources")))
			{
				File.Copy(file, Path.Combine(outputDirectory, Path.GetFileName(file)), true);
			}
		}
	}
}
