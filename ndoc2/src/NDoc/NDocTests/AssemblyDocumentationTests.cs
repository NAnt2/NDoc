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

	public void Test_GetMemberSummary_OneMethodNoSummary()
	{
		XmlNode summaryNode = documentation.GetMemberSummary(typeof(NDoc.Test.AssemblyDocumentation.GetMemberSummary.OneMethodNoSummary).GetMethod("Method1"));
		AssertNull(summaryNode);
	}

	public void Test_GetMemberSummary_OneMethodWithSummary()
	{
		XmlNode summaryNode = documentation.GetMemberSummary(typeof(NDoc.Test.AssemblyDocumentation.GetMemberSummary.OneMethodWithSummary).GetMethod("Method1"));
		AssertNotNull(summaryNode);
		AssertEquals("OneMethodWithSummary.Method1() Summary", summaryNode.InnerText);
	}

	public void Test_GetMemberSummary_OneParameterWithSummary()
	{
		XmlNode summaryNode = documentation.GetMemberSummary(typeof(NDoc.Test.AssemblyDocumentation.GetMemberSummary.OneParameterWithSummary).GetMethod("Method1"));
		AssertNotNull(summaryNode);
		AssertEquals("OneParameterWithSummary.Method1(int) Summary", summaryNode.InnerText);
	}

	public void Test_GetMemberSummary_OnePropertyWithSummary()
	{
		XmlNode summaryNode = documentation.GetMemberSummary(typeof(NDoc.Test.AssemblyDocumentation.GetMemberSummary.OnePropertyWithSummary).GetProperty("Property1"));
		AssertNotNull(summaryNode);
		AssertEquals("OnePropertyWithSummary.Property1 Summary", summaryNode.InnerText);
	}

	public void Test_GetMemberSummary_TwoParametersWithSummary()
	{
		XmlNode summaryNode = documentation.GetMemberSummary(typeof(NDoc.Test.AssemblyDocumentation.GetMemberSummary.TwoParametersWithSummary).GetMethod("Method1"));
		AssertNotNull(summaryNode);
		AssertEquals("TwoParametersWithSummary.Method1(int,string) Summary", summaryNode.InnerText);
	}

	public void Test_GetTypeConstructorsSummary_DefaultConstructor()
	{
		XmlNode summaryNode = documentation.GetTypeConstructorsSummary(typeof(NDoc.Test.AssemblyDocumentation.GetTypeConstructorsSummary.DefaultConstructor));
		AssertNull(summaryNode);
	}

	public void Test_GetTypeConstructorsSummary_OneConstructorNoSummary()
	{
		XmlNode summaryNode = documentation.GetTypeConstructorsSummary(typeof(NDoc.Test.AssemblyDocumentation.GetTypeConstructorsSummary.OneConstructorNoSummary));
		AssertNull(summaryNode);
	}

	public void Test_GetTypeConstructorsSummary_OneConstructorWithSummary()
	{
		XmlNode summaryNode = documentation.GetTypeConstructorsSummary(typeof(NDoc.Test.AssemblyDocumentation.GetTypeConstructorsSummary.OneConstructorWithSummary));
		AssertNotNull(summaryNode);
		AssertEquals("OneConstructorWithSummary.OneConstructorWithSummary() Summary", summaryNode.InnerText);
	}

	public void Test_GetTypeConstructorsSummary_OverloadedConstructorsWithSummary()
	{
		XmlNode summaryNode = documentation.GetTypeConstructorsSummary(typeof(NDoc.Test.AssemblyDocumentation.GetTypeConstructorsSummary.OverloadedConstructorsWithSummary));
		AssertNotNull(summaryNode);
		AssertEquals("OverloadedConstructorsWithSummary.OverloadedConstructorsWithSummary() Summary", summaryNode.InnerText);
	}

	public void Test_GetTypeConstructorsSummary_OverloadedConstructorsFirstNoSummary()
	{
		XmlNode summaryNode = documentation.GetTypeConstructorsSummary(typeof(NDoc.Test.AssemblyDocumentation.GetTypeConstructorsSummary.OverloadedConstructorsFirstNoSummary));
		AssertNotNull(summaryNode);
		AssertEquals("OverloadedConstructorsFirstNoSummary.OverloadedConstructorsFirstNoSummary(int) Summary", summaryNode.InnerText);
	}

	public void Test_GetTypeRemarks_NestedTypeRemarks()
	{
		XmlNode remarksNode = documentation.GetTypeRemarks(typeof(NDoc.Test.AssemblyDocumentation.GetTypeRemarks.NestedTypeRemarks.NestedType));
		AssertNotNull(remarksNode);
		AssertEquals("NestedTypeRemarks.NestedType Remarks", remarksNode.InnerText);
	}

	public void Test_GetTypeRemarks_NoSummaryOrRemarks()
	{
		XmlNode remarksNode = documentation.GetTypeRemarks(typeof(NDoc.Test.AssemblyDocumentation.GetTypeRemarks.NoSummaryOrRemarks));
		AssertNull(remarksNode);
	}

	public void Test_GetTypeRemarks_RemarksNoSummary()
	{
		XmlNode remarksNode = documentation.GetTypeRemarks(typeof(NDoc.Test.AssemblyDocumentation.GetTypeRemarks.RemarksNoSummary));
		AssertNotNull(remarksNode);
		AssertEquals("RemarksNoSummary Remarks", remarksNode.InnerText);
	}

	public void Test_GetTypeRemarks_SummaryAndRemarks()
	{
		XmlNode remarksNode = documentation.GetTypeRemarks(typeof(NDoc.Test.AssemblyDocumentation.GetTypeRemarks.SummaryAndRemarks));
		AssertNotNull(remarksNode);
		AssertEquals("SummaryAndRemarks Remarks", remarksNode.InnerText);
	}

	public void Test_GetTypeRemarks_SummaryNoRemarks()
	{
		XmlNode remarksNode = documentation.GetTypeRemarks(typeof(NDoc.Test.AssemblyDocumentation.GetTypeRemarks.SummaryNoRemarks));
		AssertNull(remarksNode);
	}

	public void Test_GetTypeSummary_NestedTypeSummary()
	{
		XmlNode summaryNode = documentation.GetTypeSummary(typeof(NDoc.Test.AssemblyDocumentation.GetTypeSummary.NestedTypeSummary.NestedType));
		AssertNotNull(summaryNode);
		AssertEquals("NestedTypeSummary.NestedType Summary", summaryNode.InnerText);
	}

	public void Test_GetTypeSummary_NoSummaryOrRemarks()
	{
		XmlNode summaryNode = documentation.GetTypeSummary(typeof(NDoc.Test.AssemblyDocumentation.GetTypeSummary.NoSummaryOrRemarks));
		AssertNull(summaryNode);
	}

	public void Test_GetTypeSummary_RemarksNoSummary()
	{
		XmlNode summaryNode = documentation.GetTypeSummary(typeof(NDoc.Test.AssemblyDocumentation.GetTypeSummary.RemarksNoSummary));
		AssertNull(summaryNode);
	}

	public void Test_GetTypeSummary_SummaryAndRemarks()
	{
		XmlNode summaryNode = documentation.GetTypeSummary(typeof(NDoc.Test.AssemblyDocumentation.GetTypeSummary.SummaryAndRemarks));
		AssertNotNull(summaryNode);
		AssertEquals("SummaryAndRemarks Summary", summaryNode.InnerText);
	}

	public void Test_GetTypeSummary_SummaryNoRemarks()
	{
		XmlNode summaryNode = documentation.GetTypeSummary(typeof(NDoc.Test.AssemblyDocumentation.GetTypeSummary.SummaryNoRemarks));
		AssertNotNull(summaryNode);
		AssertEquals("SummaryNoRemarks Summary", summaryNode.InnerText);
	}
}
