using System;
using System.IO;
using System.Reflection;

namespace NDoc.Core
{
	/// <summary>
	///		<para>Performs all the work necessary to generate documentation for an
	///		assembly.</para>
	/// </summary>
	public class Driver
	{
		private string outputDirectory;

		/// <summary>
		///		<para>Generates the documentation for an assembly.</para>
		/// </summary>
		/// <param name="assemblyFile"></param>
		/// <param name="documentationFile"></param>
		/// <param name="outputDirectory"></param>
		/// <param name="outputStyle"></param>
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
			AssemblyNavigator assemblyNavigator2 = new AssemblyNavigator(assembly);

			string styleDirectory = Path.Combine(@"..\..\..\", outputStyle);

			CopyResources(styleDirectory, outputDirectory);

			Template namespacesTemplate = new Template();
			namespacesTemplate.Load(Path.Combine(styleDirectory, @"templates\namespaces.xml"));

			Template namespaceTemplate = new Template();
			namespaceTemplate.Load(Path.Combine(styleDirectory, @"templates\namespace.xml"));

			Template typeTemplate = new Template();
			typeTemplate.Load(Path.Combine(styleDirectory, @"templates\type.xml"));

			Template typeMembersTemplate = new Template();
			typeMembersTemplate.Load(Path.Combine(styleDirectory, @"templates\type-members.xml"));

			Template typeConstructorsTemplate = new Template();
			typeConstructorsTemplate.Load(Path.Combine(styleDirectory, @"templates\type-constructors.xml"));

			Template typeMemberOverloadsTemplate = new Template();
			typeMemberOverloadsTemplate.Load(Path.Combine(styleDirectory, @"templates\type-member-overloads.xml"));

			Template typeMemberTemplate = new Template();
			typeMemberTemplate.Load(Path.Combine(styleDirectory, @"templates\type-member.xml"));

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

				if (assemblyNavigator.MoveToFirstClass())
				{
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

						streamWriter = OpenTypeMembers(assemblyNavigator.CurrentType);

						typeMembersTemplate.EvaluateType(
							assemblyNavigator.NamespaceName,
							assemblyNavigator.TypeName,
							assemblyNavigator,
							documentationFile,
							streamWriter);

						streamWriter.Close();

						if (assemblyNavigator.TypeHasOverloadedConstructors())
						{
							streamWriter = OpenTypeConstructors(assemblyNavigator.CurrentType);

							typeConstructorsTemplate.EvaluateType(
								assemblyNavigator.NamespaceName,
								assemblyNavigator.TypeName,
								assemblyNavigator,
								documentationFile,
								streamWriter);

							streamWriter.Close();
						}

						if (assemblyNavigator.MoveToFirstMethod("public"))
						{
							string previousMethodName = null;

							do
							{
								if (assemblyNavigator.MemberName != previousMethodName &&
									assemblyNavigator.IsMemberOverloaded)
								{
									streamWriter = OpenTypeMemberOverloads(assemblyNavigator.CurrentType, assemblyNavigator.CurrentMember);

									typeMemberOverloadsTemplate.EvaluateMembers(
										assemblyNavigator.NamespaceName,
										assemblyNavigator.TypeName,
										assemblyNavigator.MemberName,
										assemblyNavigator,
										documentationFile,
										streamWriter);

									streamWriter.Close();
								}

								if (!assemblyNavigator.IsMemberInherited)
								{
									streamWriter = OpenTypeMember(assemblyNavigator.CurrentType, assemblyNavigator.CurrentMember);

									typeMemberTemplate.EvaluateMember(
										assemblyNavigator.NamespaceName,
										assemblyNavigator.TypeName,
										assemblyNavigator.MemberName,
										assemblyNavigator2,
										documentationFile,
										streamWriter);

									streamWriter.Close();
								}
							}
							while (assemblyNavigator.MoveToNextMember());
						}
					}
					while (assemblyNavigator.MoveToNextType());
				}
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

		private StreamWriter OpenTypeMembers(Type type)
		{
			string fileName = type.FullName + "-members.html";
			string outputFile = Path.Combine(outputDirectory, fileName);
			return new StreamWriter(File.Open(outputFile, FileMode.Create));
		}

		private StreamWriter OpenTypeConstructors(Type type)
		{
			string fileName = type.FullName + "-constructors.html";
			string outputFile = Path.Combine(outputDirectory, fileName);
			return new StreamWriter(File.Open(outputFile, FileMode.Create));
		}

		private StreamWriter OpenTypeMemberOverloads(Type type, MemberInfo member)
		{
			string fileName = type.FullName + "." + member.Name + ".html";
			string outputFile = Path.Combine(outputDirectory, fileName);
			return new StreamWriter(File.Open(outputFile, FileMode.Create));
		}

		private StreamWriter OpenTypeMember(Type type, MemberInfo member)
		{
#warning Don't forget to pass in the overload ID here.
			string fileName = type.FullName + "." + member.Name + ".html";
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
