using System;
using System.IO;
using System.Reflection;
using System.Xml;

using NUnit.Framework;

using NDoc.Core;

public class AssemblyDocumentationTests : TestCase
{
	public AssemblyDocumentationTests(string name) : base(name) {}

	private AssemblyDocumentation documentation;

	protected override void SetUp()
	{
		Assembly assembly = typeof(NDoc.Test.Class1).Assembly;
		string location = assembly.Location;
		string directory = Path.GetDirectoryName(location);
		string path = Path.Combine(directory, "NDocTest.xml");
		documentation = new AssemblyDocumentation(path);
	}

	protected override void TearDown()
	{
	}

	public void TestTypeSummary()
	{
		XmlNode node = documentation.GetMemberNode(typeof(NDoc.Test.Class1));
		AssertNotNull(node);
		AssertEquals("This is Class1.", node["summary"].InnerText);
	}

	public void TestConstructorSummary()
	{
		XmlNode node = documentation.GetMemberNode(typeof(NDoc.Test.Constructors.ConstructorWithSummary).GetConstructors()[0]);
		AssertNotNull(node);
		AssertEquals("This constructor has a summary.", node["summary"].InnerText);
	}
}
