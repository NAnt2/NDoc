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
		XmlNode node = documentation.GetTypeSummary(typeof(NDoc.Test.Class1));
		AssertNotNull(node);
		AssertEquals("This is Class1.", node.InnerText);
	}

	public void TestConstructorSummary()
	{
		XmlNode node = documentation.GetMemberSummary(typeof(NDoc.Test.Constructors.ConstructorWithSummary).GetConstructors()[0]);
		AssertNotNull(node);
		AssertEquals("This constructor has a summary.", node.InnerText);
	}

	public void TestTypeConstructorsSummary()
	{
		XmlNode node = documentation.GetTypeConstructorsSummary(typeof(NDoc.Test.Constructors.ConstructorWithSummary));
		AssertNotNull(node);
		AssertEquals("This constructor has a summary.", node.InnerText);

		node = documentation.GetTypeConstructorsSummary(typeof(NDoc.Test.Constructors.ConstructorWithSummaryAndRemarks));
		AssertNotNull(node);
		AssertEquals("This constructor has a summary.", node.InnerText);
	}

	public void TestNestedTypeSummary()
	{
		XmlNode node = documentation.GetTypeSummary(typeof(NDoc.Test.NestedClassWithSummary.OuterClass.NestedClass));
		AssertNotNull(node);
		AssertEquals("This is a nested class.", node.InnerText);
	}
}
