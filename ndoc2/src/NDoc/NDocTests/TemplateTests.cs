using System;
using System.IO;
using System.Reflection;

using NUnit.Framework;

using NDoc.Core;

public class TemplateTests : TestCase
{
	public TemplateTests(string name) : base(name) {}

	AssemblyNavigator assemblyNavigator;
	string documentation;

	#region SetUp and TearDown Methods

	protected override void SetUp()
	{
		Assembly assembly = typeof(NDoc.Test.Class1).Assembly;
		string location = assembly.Location;
		string directory = Path.GetDirectoryName(location);
		documentation = Path.Combine(directory, "NDocTest.xml");
		assemblyNavigator = new AssemblyNavigator(assembly);
	}

	protected override void TearDown()
	{
	}

	#endregion

	#region Helper Methods

	private string Evaluate(string templateXml)
	{
		Template template = new Template();
		template.LoadXml(templateXml);
		StringWriter result = new StringWriter();
		template.Evaluate(assemblyNavigator, documentation, result);
		return result.ToString();
	}

	private string EvaluateNamespace(string templateXml, string namespaceName)
	{
		Template template = new Template();
		template.LoadXml(templateXml);
		StringWriter result = new StringWriter();
		template.EvaluateNamespace(namespaceName, assemblyNavigator, documentation, result);
		return result.ToString();
	}

	private string EvaluateType(string templateXml, string namespaceName, string typeName)
	{
		Template template = new Template();
		template.LoadXml(templateXml);
		StringWriter result = new StringWriter();
		template.EvaluateType(namespaceName, typeName, assemblyNavigator, documentation, result);
		return result.ToString();
	}

	private string EvaluateMember(string templateXml, string namespaceName, string typeName, string memberID)
	{
		Template template = new Template();
		template.LoadXml(templateXml);
		StringWriter result = new StringWriter();
		template.EvaluateMember(namespaceName, typeName, memberID, assemblyNavigator, documentation, result);
		return result.ToString();
	}

	#endregion

	#region <for-each-method-in-type> Tests

	public void Test_ForEachMethodInType_OneMethod()
	{
		AssertEquals(
			"EqualsGetHashCodeGetTypeMethod1ToString",
			EvaluateType(
				"<for-each-method-in-type access='public'><member-name /></for-each-method-in-type>",
				"NDoc.Test.Template.ForEachMethodInType",
				"OneMethod"));
	}

	public void Test_ForEachMethodInType_OneProperty()
	{
		AssertEquals(
			"EqualsGetHashCodeGetTypeToString",
			EvaluateType(
				"<for-each-method-in-type access='public'><member-name /></for-each-method-in-type>",
				"NDoc.Test.Template.ForEachMethodInType",
				"OneProperty"));
	}

	public void Test_ForEachMethodInType_TwoMethods()
	{
		AssertEquals(
			"EqualsGetHashCodeGetTypeMethod1Method2ToString",
			EvaluateType(
				"<for-each-method-in-type access='public'><member-name /></for-each-method-in-type>",
				"NDoc.Test.Template.ForEachMethodInType",
				"TwoMethods"));
	}

	public void Test_ForEachMethodInType_TwoOverloadedMethods()
	{
		AssertEquals(
			"EqualsGetHashCodeGetTypeMethodToString",
			EvaluateType(
				"<for-each-method-in-type access='public'><member-name /></for-each-method-in-type>",
				"NDoc.Test.Template.ForEachMethodInType",
				"TwoOverloadedMethods"));
	}

	#endregion

	#region <for-each-property-in-type> Tests

	public void Test_ForEachPropertyInType_TwoProperties()
	{
		AssertEquals(
			"Property1Property2",
			EvaluateType(
				"<for-each-property-in-type access='public'><member-name /></for-each-property-in-type>",
				"NDoc.Test.Template.ForEachPropertyInType",
				"TwoProperties"));
	}

	#endregion

	#region <if-member-is-inherited> Tests

	public void Test_IfMemberIsInherited_NoMembers()
	{
		AssertEquals(
			"true",
			EvaluateMember(
				"<if-member-is-inherited>true</if-member-is-inherited>",
				"NDoc.Test.Template.IfMemberIsInherited",
				"OneMethod",
				"Equals"));
	}

	public void Test_IfMemberIsInherited_OneMethod()
	{
		AssertEquals(
			String.Empty,
			EvaluateMember(
				"<if-member-is-inherited>true</if-member-is-inherited>",
				"NDoc.Test.Template.IfMemberIsInherited",
				"OneMethod",
				"Method1"));
	}

	#endregion

	#region <if-member-is-overloaded> Tests

	public void Test_IfMemberIsOverloaded_OneMethod()
	{
		AssertEquals(
			String.Empty,
			EvaluateMember(
				"<if-member-is-overloaded>true</if-member-is-overloaded>",
				"NDoc.Test.Template.IfMemberIsOverloaded",
				"OneMethod",
				"Method1"));
	}

	#endregion

	#region <if-type-has-properties> Tests

	public void Test_IfTypeHasProperties_NoProperties()
	{
		AssertEquals(
			String.Empty,
			EvaluateType(
				"<if-type-has-properties access='public'>true</if-type-has-properties>",
				"NDoc.Test.Template.IfTypeHasProperties",
				"NoProperties"));
	}

	#endregion

	#region <member-declaring-type> Tests

	public void Test_MemberDeclaringType_NoMembers()
	{
		AssertEquals(
			"Object",
			EvaluateMember(
				"<member-declaring-type />",
				"NDoc.Test.Template.MemberDeclaringType",
				"NoMembers",
				"Equals"));
	}

	#endregion

	#region {$member-or-overloads-link} Tests

	public void Test_MemberOrOverloadsLink_OneMethod()
	{
		AssertEquals(
			"<a href=\"NDoc.Test.Template.MemberOrOverloadsLink.OneMethod.Method1.html\">Method1</a>",
			EvaluateMember(
				"<a href='{$member-or-overloads-link}'><member-name /></a>",
				"NDoc.Test.Template.MemberOrOverloadsLink",
				"OneMethod",
				"Method1"));
	}

	public void Test_MemberOrOverloadsLink_TwoOverloadedMethods()
	{
		AssertEquals(
			"<a href=\"NDoc.Test.Template.MemberOrOverloadsLink.TwoOverloadedMethods.Method.html\">Method</a>",
			EvaluateMember(
				"<a href='{$member-or-overloads-link}'><member-name /></a>",
				"NDoc.Test.Template.MemberOrOverloadsLink",
				"TwoOverloadedMethods",
				"Method"));
	}

	#endregion

	// TODO: The following tests need to be rewritten using the same scheme as the preceding tests.

	public void TestLiteralElement()
	{
		AssertEquals("<foo />", EvaluateNamespace("<foo />", "NDoc.Test"));
	}

	public void TestLiteralElementWithTextContent()
	{
		AssertEquals("<foo>bar</foo>", EvaluateNamespace("<foo>bar</foo>", "NDoc.Test"));
	}

	public void TestLiteralAttribute()
	{
		AssertEquals("<foo bar=\"baz\" />", EvaluateNamespace("<foo bar=\"baz\" />", "NDoc.Test"));
	}

	public void TestLiteralAttributeWithAssemblyNameVariable()
	{
		AssertEquals("<foo bar=\"NDocTest\" />", EvaluateNamespace("<foo bar=\"{$assembly-name}\" />", "NDoc.Test"));
	}

	public void TestLiteralAttributeWithAssemblyNameVariableSurroundedByText()
	{
		AssertEquals("<foo bar=\"bazNDocTestquux\" />", EvaluateNamespace("<foo bar=\"baz{$assembly-name}quux\" />", "NDoc.Test"));
	}

	public void TestLiteralAttributeWithVariableWithNoDollarSign()
	{
		AssertEquals("<foo bar=\"{baz}\" />", EvaluateNamespace("<foo bar=\"{baz}\" />", "NDoc.Test"));
	}

	public void TestLiteralAttributeWithVariableWithNoCloseCurly()
	{
		AssertEquals("<foo bar=\"{$baz\" />", EvaluateNamespace("<foo bar=\"{$baz\" />", "NDoc.Test"));
	}

	public void TestLiteralAttributeWithVariableWithNoName()
	{
		AssertEquals("<foo bar=\"{$}\" />", EvaluateNamespace("<foo bar=\"{$}\" />", "NDoc.Test"));
	}

	public void TestLiteralAttributeWithVariableWithNoNameAndNoCloseCurly()
	{
		AssertEquals("<foo bar=\"{$\" />", EvaluateNamespace("<foo bar=\"{$\" />", "NDoc.Test"));
	}

	public void TestLiteralAttributeWithVariableWithUnknownName()
	{
		AssertEquals("<foo bar=\"{$baz}\" />", EvaluateNamespace("<foo bar=\"{$baz}\" />", "NDoc.Test"));
	}

	public void TestTemplate()
	{
		AssertEquals(String.Empty, EvaluateNamespace("<template />", "NDoc.Test"));
	}

	public void TestTemplateWithTextContent()
	{
		AssertEquals("foo", EvaluateNamespace("<template>foo</template>", "NDoc.Test"));
	}

	public void TestAssemblyName()
	{
		AssertEquals("NDocTest", EvaluateNamespace("<assembly-name />", "NDoc.Test"));
	}

	public void TestNamespaceName()
	{
		AssertEquals("NDoc.Test", EvaluateNamespace("<namespace-name />", "NDoc.Test"));
	}

	public void TestTypeName()
	{
		AssertEquals("Class1", EvaluateType("<type-name />", "NDoc.Test", "Class1"));
	}

	public void TestIfNamespaceContainsClasses()
	{
		AssertEquals(
			"true",
			EvaluateNamespace(
				"<if-namespace-contains-classes>true</if-namespace-contains-classes>",
				"NDoc.Test.TwoClasses"));

		AssertEquals(
			String.Empty,
			EvaluateNamespace(
				"<if-namespace-contains-classes>false</if-namespace-contains-classes>",
				"NDoc.Test.TwoInterfaces"));
	}

	public void TestIfNamespaceContainsInterfaces()
	{
		AssertEquals(
			"true",
			EvaluateNamespace(
				"<if-namespace-contains-interfaces>true</if-namespace-contains-interfaces>",
				"NDoc.Test.TwoInterfaces"));

		AssertEquals(
			String.Empty,
			EvaluateNamespace(
				"<if-namespace-contains-interfaces>false</if-namespace-contains-interfaces>",
				"NDoc.Test.TwoClasses"));
	}

	public void TestIfNamespaceContainsStructures()
	{
		AssertEquals(
			"true",
			EvaluateNamespace(
				"<if-namespace-contains-structures>true</if-namespace-contains-structures>",
				"NDoc.Test.TwoStructures"));

		AssertEquals(
			String.Empty,
			EvaluateNamespace(
				"<if-namespace-contains-structures>false</if-namespace-contains-structures>",
				"NDoc.Test.TwoClasses"));
	}

	public void TestIfNamespaceContainsDelegates()
	{
		AssertEquals(
			"true",
			EvaluateNamespace(
				"<if-namespace-contains-delegates>true</if-namespace-contains-delegates>",
				"NDoc.Test.TwoDelegates"));

		AssertEquals(
			String.Empty,
			EvaluateNamespace(
				"<if-namespace-contains-delegates>false</if-namespace-contains-delegates>",
				"NDoc.Test.TwoClasses"));
	}

	public void TestIfNamespaceContainsEnumerations()
	{
		AssertEquals(
			"true",
			EvaluateNamespace(
				"<if-namespace-contains-enumerations>true</if-namespace-contains-enumerations>",
				"NDoc.Test.TwoEnumerations"));

		AssertEquals(
			String.Empty,
			EvaluateNamespace(
				"<if-namespace-contains-enumerations>false</if-namespace-contains-enumerations>",
				"NDoc.Test.TwoClasses"));
	}

	public void TestForEachClassInNamespace()
	{
		AssertEquals(
			"Class1Class2",
			EvaluateNamespace(
				"<for-each-class-in-namespace><type-name /></for-each-class-in-namespace>",
				"NDoc.Test.TwoClasses"));
	}

	public void TestForEachInterfaceInNamespace()
	{
		AssertEquals(
			"Interface1Interface2",
			EvaluateNamespace(
				"<for-each-interface-in-namespace><type-name /></for-each-interface-in-namespace>",
				"NDoc.Test.TwoInterfaces"));
	}

	public void TestForEachStructureInNamespace()
	{
		AssertEquals(
			"Structure1Structure2",
			EvaluateNamespace(
				"<for-each-structure-in-namespace><type-name /></for-each-structure-in-namespace>",
				"NDoc.Test.TwoStructures"));
	}

	public void TestForEachDelegateInNamespace()
	{
		AssertEquals(
			"Delegate1Delegate2",
			EvaluateNamespace(
				"<for-each-delegate-in-namespace><type-name /></for-each-delegate-in-namespace>",
				"NDoc.Test.TwoDelegates"));
	}

	public void TestForEachEnumerationInNamespace()
	{
		AssertEquals(
			"Enumeration1Enumeration2",
			EvaluateNamespace(
				"<for-each-enumeration-in-namespace><type-name /></for-each-enumeration-in-namespace>",
				"NDoc.Test.TwoEnumerations"));
	}

	public void TestForEachOfEverythingInNamespace()
	{
		AssertEquals(
			"Class1Class2Interface1Interface2Structure1Structure2Delegate1Delegate2Enumeration1Enumeration2",
			EvaluateNamespace(
				"<template>" +
					"<for-each-class-in-namespace><type-name /></for-each-class-in-namespace>" +
					"<for-each-interface-in-namespace><type-name /></for-each-interface-in-namespace>" +
					"<for-each-structure-in-namespace><type-name /></for-each-structure-in-namespace>" +
					"<for-each-delegate-in-namespace><type-name /></for-each-delegate-in-namespace>" +
					"<for-each-enumeration-in-namespace><type-name /></for-each-enumeration-in-namespace>" +
				"</template>",
				"NDoc.Test.TwoOfEverything"));
	}

	public void TestTypeLinkVariable()
	{
		AssertEquals(
			"<a href=\"NDoc.Test.Class1.html\">Class1</a>",
			EvaluateType(
				"<a href=\"{$type-link}\"><type-name /></a>",
				"NDoc.Test",
				"Class1"));
	}

	public void TestTypeSummaryWithoutPara()
	{
		AssertEquals(
			"<p>This summary has no para element.</p>",
			EvaluateType(
				"<type-summary />",
				"NDoc.Test.Summaries",
				"SummaryWithoutPara"));
	}

	public void TestTypeSummaryWithPara()
	{
		AssertEquals(
			"<p>This summary has one para element.</p>",
			EvaluateType(
				"<type-summary />",
				"NDoc.Test.Summaries",
				"SummaryWithPara"));
	}

	public void TestNoTypeSummary()
	{
		AssertEquals(
			String.Empty,
			EvaluateType(
				"<type-summary />",
				"NDoc.Test.Summaries",
				"NoSummary"));
	}

	public void TestTypeSummaryWithParaStripFirst()
	{
		AssertEquals(
			"This summary has one para element.",
			EvaluateType(
				"<type-summary strip='first' />",
				"NDoc.Test.Summaries",
				"SummaryWithPara"));
	}

	public void TestTypeSummaryWithTwoParasStripFirst()
	{
		// See the System.Xml.XPath namespace page in MSDN and look at the
		// XPathNodeType summary to see how they only strip the first para element.
		AssertEquals(
			"This summary has two para elements.<p>This is the second para.</p>",
			EvaluateType(
				"<type-summary strip='first' />",
				"NDoc.Test.Summaries",
				"SummaryWithTwoParas"));
	}

	public void TestTypeSummaryWithoutParaStripFirst()
	{
		AssertEquals(
			"This summary has no para element.",
			EvaluateType(
			"<type-summary strip='first' />",
			"NDoc.Test.Summaries",
			"SummaryWithoutPara"));
	}

	public void TestTypeType()
	{
		AssertEquals(
			"Class",
			EvaluateType(
				"<type-type />",
				"NDoc.Test",
				"Class1"));

		AssertEquals(
			"Interface",
			EvaluateType(
				"<type-type />",
				"NDoc.Test",
				"Interface1"));

		AssertEquals(
			"Structure",
			EvaluateType(
				"<type-type />",
				"NDoc.Test",
				"Structure1"));

		AssertEquals(
			"Delegate",
			EvaluateType(
				"<type-type />",
				"NDoc.Test",
				"Delegate1"));

		AssertEquals(
			"Enumeration",
			EvaluateType(
				"<type-type />",
				"NDoc.Test",
				"Enumeration1"));
	}

	public void TestTypeTypeVB()
	{
		AssertEquals(
			"Class",
			EvaluateType(
				"<type-type lang='VB' />",
				"NDoc.Test",
				"Class1"));

		AssertEquals(
			"Interface",
			EvaluateType(
				"<type-type lang='VB' />",
				"NDoc.Test",
				"Interface1"));

		AssertEquals(
			"Structure",
			EvaluateType(
				"<type-type lang='VB' />",
				"NDoc.Test",
				"Structure1"));

		AssertEquals(
			"Delegate",
			EvaluateType(
				"<type-type lang='VB' />",
				"NDoc.Test",
				"Delegate1"));

		AssertEquals(
			"Enum",
			EvaluateType(
				"<type-type lang='VB' />",
				"NDoc.Test",
				"Enumeration1"));
	}

	public void TestTypeTypeCSharp()
	{
		AssertEquals(
			"class",
			EvaluateType(
				"<type-type lang='C#' />",
				"NDoc.Test",
				"Class1"));

		AssertEquals(
			"interface",
			EvaluateType(
				"<type-type lang='C#' />",
				"NDoc.Test",
				"Interface1"));

		AssertEquals(
			"struct",
			EvaluateType(
				"<type-type lang='C#' />",
				"NDoc.Test",
				"Structure1"));

		AssertEquals(
			"delegate",
			EvaluateType(
				"<type-type lang='C#' />",
				"NDoc.Test",
				"Delegate1"));

		AssertEquals(
			"enum",
			EvaluateType(
				"<type-type lang='C#' />",
				"NDoc.Test",
				"Enumeration1"));
	}

	public void TestWhitespace()
	{
		AssertEquals("<foo />", EvaluateNamespace("<foo> </foo>", "NDoc.Test"));
	}

	public void TestText()
	{
		AssertEquals(" ", EvaluateNamespace("<text> </text>", "NDoc.Test"));
	}

	public void TestTypeAccessVB()
	{
		AssertEquals(
			"Public",
			EvaluateType(
				"<type-access lang='VB' />",
				"NDoc.Test.AllAccess",
				"PublicClass"));

		AssertEquals(
			"Friend",
			EvaluateType(
				"<type-access lang='VB' />",
				"NDoc.Test.AllAccess",
				"InternalClass"));
	}

	public void TestTypeAccessCSharp()
	{
		AssertEquals(
			"public",
			EvaluateType(
				"<type-access lang='C#' />",
				"NDoc.Test.AllAccess",
				"PublicClass"));

		AssertEquals(
			"internal",
			EvaluateType(
				"<type-access lang='C#' />",
				"NDoc.Test.AllAccess",
				"InternalClass"));
	}

	public void TestIfTypeHasBaseType()
	{
		AssertEquals(
			String.Empty,
			EvaluateType(
				"<if-type-has-base-type>has-base-type</if-type-has-base-type>",
				"NDoc.Test.DerivedClasses",
				"BaseClass"));

		AssertEquals(
			"has-base-type",
			EvaluateType(
				"<if-type-has-base-type>has-base-type</if-type-has-base-type>",
				"NDoc.Test.DerivedClasses",
				"DerivedClass"));
	}

	public void TestTypeBaseTypeName()
	{
		AssertEquals(
			"Object",
			EvaluateType(
				"<type-base-type-name />",
				"NDoc.Test.DerivedClasses",
				"BaseClass"));

		AssertEquals(
			"BaseClass",
			EvaluateType(
				"<type-base-type-name />",
				"NDoc.Test.DerivedClasses",
				"DerivedClass"));
	}

	public void TestIfTypeImplementsInterfaces()
	{
		AssertEquals(
			String.Empty,
			EvaluateType(
				"<if-type-implements-interfaces>implements-interfaces</if-type-implements-interfaces>",
				"NDoc.Test.ImplementsInterfaces",
				"ImplementsZeroInterfaces"));

		AssertEquals(
			"implements-interfaces",
			EvaluateType(
				"<if-type-implements-interfaces>implements-interfaces</if-type-implements-interfaces>",
				"NDoc.Test.ImplementsInterfaces",
				"ImplementsOneInterface"));

		AssertEquals(
			"implements-interfaces",
			EvaluateType(
				"<if-type-implements-interfaces>implements-interfaces</if-type-implements-interfaces>",
				"NDoc.Test.ImplementsInterfaces",
				"ImplementsTwoInterfaces"));
	}

	public void TestIfTypeHasBaseTypeOrImplementsInterfaces()
	{
		AssertEquals(
			String.Empty,
			EvaluateType(
				"<if-type-has-base-type-or-implements-interfaces>true</if-type-has-base-type-or-implements-interfaces>",
				"NDoc.Test.DerivedClasses",
				"BaseClass"));

		AssertEquals(
			"true",
			EvaluateType(
				"<if-type-has-base-type-or-implements-interfaces>true</if-type-has-base-type-or-implements-interfaces>",
				"NDoc.Test.DerivedClasses",
				"DerivedClass"));

		AssertEquals(
			String.Empty,
			EvaluateType(
				"<if-type-has-base-type-or-implements-interfaces>true</if-type-has-base-type-or-implements-interfaces>",
				"NDoc.Test.ImplementsInterfaces",
				"ImplementsZeroInterfaces"));

		AssertEquals(
			"true",
			EvaluateType(
				"<if-type-has-base-type-or-implements-interfaces>true</if-type-has-base-type-or-implements-interfaces>",
				"NDoc.Test.ImplementsInterfaces",
				"ImplementsOneInterface"));

		AssertEquals(
			"true",
			EvaluateType(
				"<if-type-has-base-type-or-implements-interfaces>true</if-type-has-base-type-or-implements-interfaces>",
				"NDoc.Test.ImplementsInterfaces",
				"ImplementsTwoInterfaces"));
	}

	public void TestForEachInterfaceImplementedByType()
	{
		AssertEquals(
			String.Empty,
			EvaluateType(
				"<for-each-interface-implemented-by-type><implemented-interface-name /></for-each-interface-implemented-by-type>",
				"NDoc.Test.ImplementsInterfaces",
				"ImplementsZeroInterfaces"));

		AssertEquals(
			"Interface1",
			EvaluateType(
				"<for-each-interface-implemented-by-type><implemented-interface-name /></for-each-interface-implemented-by-type>",
				"NDoc.Test.ImplementsInterfaces",
				"ImplementsOneInterface"));

		AssertEquals(
			"Interface1Interface2",
			EvaluateType(
				"<for-each-interface-implemented-by-type><implemented-interface-name /></for-each-interface-implemented-by-type>",
				"NDoc.Test.ImplementsInterfaces",
				"ImplementsTwoInterfaces"));
	}

	public void TestIfNotLastImplementedInterface()
	{
		AssertEquals(
			"Interface1, Interface2",
			EvaluateType(
				"<for-each-interface-implemented-by-type>" +
					"<implemented-interface-name />" +
					"<if-not-last-implemented-interface>, </if-not-last-implemented-interface>" +
				"</for-each-interface-implemented-by-type>",
				"NDoc.Test.ImplementsInterfaces",
				"ImplementsTwoInterfaces"));
	}

	public void TestCopyEntityReference()
	{
		AssertEquals(
			"\xA9",
			EvaluateNamespace("<!DOCTYPE template [ <!ENTITY copy '&#xA9;'> ]><template>&copy;</template>", "NDoc.Test"));
	}

	public void TestIfTypeIsAbstract()
	{
		AssertEquals(
			String.Empty,
			EvaluateType(
				"<if-type-is-abstract>true</if-type-is-abstract>",
				"NDoc.Test.AbstractAndSealed",
				"NormalClass"));

		AssertEquals(
			"true",
			EvaluateType(
				"<if-type-is-abstract>true</if-type-is-abstract>",
				"NDoc.Test.AbstractAndSealed",
				"AbstractClass"));

		AssertEquals(
			String.Empty,
			EvaluateType(
				"<if-type-is-abstract>true</if-type-is-abstract>",
				"NDoc.Test.AbstractAndSealed",
				"SealedClass"));
	}

	public void TestIfTypeIsSealed()
	{
		AssertEquals(
			String.Empty,
			EvaluateType(
				"<if-type-is-sealed>true</if-type-is-sealed>",
				"NDoc.Test.AbstractAndSealed",
				"NormalClass"));

		AssertEquals(
			String.Empty,
			EvaluateType(
				"<if-type-is-sealed>true</if-type-is-sealed>",
				"NDoc.Test.AbstractAndSealed",
				"AbstractClass"));

		AssertEquals(
			"true",
			EvaluateType(
				"<if-type-is-sealed>true</if-type-is-sealed>",
				"NDoc.Test.AbstractAndSealed",
				"SealedClass"));
	}

	public void TestNamespaceLinkVariable()
	{
		AssertEquals(
			"<a href=\"NDoc.Test.html\">NDoc.Test</a>",
			EvaluateNamespace(
				"<a href=\"{$namespace-link}\"><namespace-name /></a>",
				"NDoc.Test"));
	}

	public void TestTypeMembersLinkVariable()
	{
		AssertEquals(
			"<a href=\"NDoc.Test.Class1-members.html\">Class1 Members</a>",
			EvaluateType(
				"<a href=\"{$type-members-link}\"><type-name /> Members</a>",
				"NDoc.Test",
				"Class1"));
	}

	public void TestForEachNamespace()
	{
		Assert(Evaluate("<for-each-namespace><namespace-name /></for-each-namespace>").StartsWith("(global)NDoc.TestNDoc.Test."));
	}

	public void TestTypeSummaryOnTypeWithNoSummary()
	{
		AssertEquals(
			String.Empty,
			EvaluateType(
			"<type-summary />",
			"NDoc.Test.Remarks",
			"RemarksWithPara"));
	}

	public void TestTypeRemarksWithPara()
	{
		AssertEquals(
			"<p>These remarks contains one para element.</p>",
			EvaluateType(
				"<type-remarks />",
				"NDoc.Test.Remarks",
				"RemarksWithPara"));
	}

	public void TestTypeRemarksWithoutPara()
	{
		AssertEquals(
			"<p>These remarks contains no para element.</p>",
			EvaluateType(
			"<type-remarks />",
			"NDoc.Test.Remarks",
			"RemarksWithoutPara"));
	}

	public void TestIfTypeHasRemarks()
	{
		AssertEquals(
			"true",
			EvaluateType(
				"<if-type-has-remarks>true</if-type-has-remarks>",
				"NDoc.Test.Remarks",
				"RemarksWithPara"));

		AssertEquals(
			String.Empty,
			EvaluateType(
				"<if-type-has-remarks>true</if-type-has-remarks>",
				"NDoc.Test.Remarks",
				"NoRemarks"));
	}

	public void TestIfTypeHasConstructors()
	{
		AssertEquals(
			"true",
			EvaluateType(
				"<if-type-has-constructors access='public'>true</if-type-has-constructors>",
				"NDoc.Test.Constructors",
				"DefaultConstructor"));

		AssertEquals(
			String.Empty,
			EvaluateType(
				"<if-type-has-constructors access='public'>true</if-type-has-constructors>",
				"NDoc.Test.Constructors",
				"PrivateConstructor"));
	}

	public void TestForEachConstructorInType()
	{
		AssertEquals(
			".",
			EvaluateType(
				"<for-each-constructor-in-type access='public'>.</for-each-constructor-in-type>",
				"NDoc.Test.Constructors",
				"DefaultConstructor"));

		AssertEquals(
			String.Empty,
			EvaluateType(
				"<for-each-constructor-in-type access='public'>.</for-each-constructor-in-type>",
				"NDoc.Test.Constructors",
				"PrivateConstructor"));

		AssertEquals(
			"..",
			EvaluateType(
				"<for-each-constructor-in-type access='public'>.</for-each-constructor-in-type>",
				"NDoc.Test.Constructors",
				"TwoConstructors"));
	}

	public void TestMemberSummary()
	{
		AssertEquals(
			"This constructor has a summary.",
			EvaluateType(
				"<for-each-constructor-in-type access='public'><member-summary strip='first' /></for-each-constructor-in-type>",
				"NDoc.Test.Constructors",
				"ConstructorWithSummary"));
	}

	public void TestIfTypeHasOverloadedConstructors()
	{
		AssertEquals(
			String.Empty,
			EvaluateType(
				"<if-type-has-overloaded-constructors>true</if-type-has-overloaded-constructors>",
				"NDoc.Test.Constructors",
				"DefaultConstructor"));

		AssertEquals(
			"true",
			EvaluateType(
				"<if-type-has-overloaded-constructors>true</if-type-has-overloaded-constructors>",
				"NDoc.Test.Constructors",
				"TwoConstructors"));
	}

	public void TestTypeConstructorsSummary()
	{
		AssertEquals(
			"This constructor has a summary.",
			EvaluateType(
				"<type-constructors-summary strip='first' />",
				"NDoc.Test.Constructors",
				"ConstructorWithSummary"));
	}

	public void TestMissingTypeConstructorsSummary()
	{
		AssertEquals(
			"Initializes a new instance of the DefaultConstructor class.",
			EvaluateType(
				"<type-constructors-summary strip='first' />",
				"NDoc.Test.Constructors",
				"DefaultConstructor"));
	}

	public void TestIfTypeHasMethods()
	{
		AssertEquals(
			"true",
			EvaluateType(
				"<if-type-has-methods access='public'>true</if-type-has-methods>",
				"NDoc.Test.Methods",
				"NoMethods"));
	}
}
