using System;
using System.IO;
using System.Reflection;

namespace NDoc.Core
{
	public class Driver
	{
		private string outputDirectory;

		public void GenerateDocumentation(
			string assemblyFile, 
			string documentationFile,
			string outputDirectory,
			string outputStyle)
		{
			this.outputDirectory = outputDirectory;

			Directory.CreateDirectory(outputDirectory);

			Assembly assembly = Assembly.LoadFrom(assemblyFile);
			AssemblyNavigator assemblyNavigator = new AssemblyNavigator(assembly);

			string styleDirectory = Path.Combine(@"..\..\..\", outputStyle);

			CopyResources(styleDirectory, outputDirectory);

			Template namespacesTemplate = new Template();
			namespacesTemplate.Load(Path.Combine(styleDirectory, @"templates\namespaces.xml"));

			Template namespaceTemplate = new Template();
			namespaceTemplate.Load(Path.Combine(styleDirectory, @"templates\namespace.xml"));

			Template typeTemplate = new Template();
			typeTemplate.Load(Path.Combine(styleDirectory, @"templates\type.xml"));

			StreamWriter streamWriter = OpenNamespaces();

			namespacesTemplate.Evaluate(
				assemblyNavigator, 
				documentationFile, 
				streamWriter);

			streamWriter.Close();

			assemblyNavigator.MoveToFirstNamespace();

			do
			{
				streamWriter = OpenNamespace(assemblyNavigator.NamespaceName);

				namespaceTemplate.EvaluateNamespace(
					assemblyNavigator.NamespaceName, 
					assemblyNavigator, 
					documentationFile, 
					streamWriter);

				streamWriter.Close();

				assemblyNavigator.MoveToFirstClass();

				do
				{
					streamWriter = OpenType(assemblyNavigator.CurrentType);

					typeTemplate.EvaluateType(
						assemblyNavigator.NamespaceName, 
						assemblyNavigator.TypeName,
						assemblyNavigator, 
						documentationFile, 
						streamWriter);

					streamWriter.Close();
				}
				while (assemblyNavigator.MoveToNextType());
			}
			while (assemblyNavigator.MoveToNextNamespace());
		}

		private StreamWriter OpenNamespaces()
		{
			string fileName = "index.html";
			string outputFile = Path.Combine(outputDirectory, fileName);
			return new StreamWriter(File.Open(outputFile, FileMode.Create));
		}

		private StreamWriter OpenNamespace(string namespaceName)
		{
			string fileName = namespaceName + ".html";
			string outputFile = Path.Combine(outputDirectory, fileName);
			return new StreamWriter(File.Open(outputFile, FileMode.Create));
		}

		private StreamWriter OpenType(Type type)
		{
			string fileName = type.FullName + ".html";
			string outputFile = Path.Combine(outputDirectory, fileName);
			return new StreamWriter(File.Open(outputFile, FileMode.Create));
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
