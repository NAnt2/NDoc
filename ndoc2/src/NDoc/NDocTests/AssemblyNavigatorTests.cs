using System;
using System.Reflection;

using NUnit.Framework;

using NDoc.Core;

public class AssemblyNavigatorTests : TestCase
{
	public AssemblyNavigatorTests(string name) : base(name) {}

	private Assembly assembly;
	private AssemblyNavigator navigator;

	#region SetUp and TearDown Methods

	protected override void SetUp()
	{
		assembly = typeof(NDoc.Test.Class1).Assembly;
		navigator = new AssemblyNavigator(assembly);
	}

	protected override void TearDown()
	{
	}

	#endregion

	#region AssemblyName Tests

	public void Test_AssemblyName()
	{
		AssertEquals("NDocTest", navigator.AssemblyName);
	}

	#endregion

	#region IsClass Tests

	public void Test_IsClass_Class1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsClass"));
		Assert(navigator.MoveToType("Class1"));
		Assert(navigator.IsClass);
	}

	public void Test_IsClass_Delegate1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsClass"));
		Assert(navigator.MoveToType("Delegate1"));
		Assert(!navigator.IsClass);
	}

	public void Test_IsClass_Enumeration1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsClass"));
		Assert(navigator.MoveToType("Enumeration1"));
		Assert(!navigator.IsClass);
	}

	public void Test_IsClass_Interface1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsClass"));
		Assert(navigator.MoveToType("Interface1"));
		Assert(!navigator.IsClass);
	}

	public void Test_IsClass_Structure1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsClass"));
		Assert(navigator.MoveToType("Structure1"));
		Assert(!navigator.IsClass);
	}

	#endregion

	#region IsDelegate Tests

	public void Test_IsDelegate_Class1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsDelegate"));
		Assert(navigator.MoveToType("Class1"));
		Assert(!navigator.IsDelegate);
	}

	public void Test_IsDelegate_Delegate1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsDelegate"));
		Assert(navigator.MoveToType("Delegate1"));
		Assert(navigator.IsDelegate);
	}

	public void Test_IsDelegate_Enumeration1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsDelegate"));
		Assert(navigator.MoveToType("Enumeration1"));
		Assert(!navigator.IsDelegate);
	}

	public void Test_IsDelegate_Interface1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsDelegate"));
		Assert(navigator.MoveToType("Interface1"));
		Assert(!navigator.IsDelegate);
	}

	public void Test_IsDelegate_Structure1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsDelegate"));
		Assert(navigator.MoveToType("Structure1"));
		Assert(!navigator.IsDelegate);
	}

	#endregion

	#region IsEnumeration Tests

	public void Test_IsEnumeration_Class1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsEnumeration"));
		Assert(navigator.MoveToType("Class1"));
		Assert(!navigator.IsEnumeration);
	}

	public void Test_IsEnumeration_Delegate1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsEnumeration"));
		Assert(navigator.MoveToType("Delegate1"));
		Assert(!navigator.IsEnumeration);
	}

	public void Test_IsEnumeration_Enumeration1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsEnumeration"));
		Assert(navigator.MoveToType("Enumeration1"));
		Assert(navigator.IsEnumeration);
	}

	public void Test_IsEnumeration_Interface1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsEnumeration"));
		Assert(navigator.MoveToType("Interface1"));
		Assert(!navigator.IsEnumeration);
	}

	public void Test_IsEnumeration_Structure1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsEnumeration"));
		Assert(navigator.MoveToType("Structure1"));
		Assert(!navigator.IsEnumeration);
	}

	#endregion

	#region IsInterface Tests

	public void Test_IsInterface_Class1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsInterface"));
		Assert(navigator.MoveToType("Class1"));
		Assert(!navigator.IsInterface);
	}

	public void Test_IsInterface_Delegate1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsInterface"));
		Assert(navigator.MoveToType("Delegate1"));
		Assert(!navigator.IsInterface);
	}

	public void Test_IsInterface_Enumeration1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsInterface"));
		Assert(navigator.MoveToType("Enumeration1"));
		Assert(!navigator.IsInterface);
	}

	public void Test_IsInterface_Interface1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsInterface"));
		Assert(navigator.MoveToType("Interface1"));
		Assert(navigator.IsInterface);
	}

	public void Test_IsInterface_Structure1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsInterface"));
		Assert(navigator.MoveToType("Structure1"));
		Assert(!navigator.IsInterface);
	}

	#endregion

	#region IsLastImplementedInterface Tests

	public void Test_IsLastImplementedInterface_OneInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToNextInterfaceImplementedByType"));
		Assert(navigator.MoveToType("OneInterface"));
		Assert(navigator.MoveToFirstInterfaceImplementedByType());
		Assert(navigator.IsLastImplementedInterface);
	}

	public void Test_IsLastImplementedInterface_TwoInterfaces()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToNextInterfaceImplementedByType"));
		Assert(navigator.MoveToType("TwoInterfaces"));
		Assert(navigator.MoveToFirstInterfaceImplementedByType());
		Assert(!navigator.IsLastImplementedInterface);
		Assert(navigator.MoveToNextInterfaceImplementedByType());
		Assert(navigator.IsLastImplementedInterface);
	}

	#endregion

	#region IsMemberInherited Tests

	public void Test_IsMemberInherited_OneMethod()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsMemberInherited"));
		Assert(navigator.MoveToType("OneMethod"));
		Assert(navigator.MoveToMember("Method1"));
		Assert(!navigator.IsMemberInherited);
	}

	#endregion

	#region IsStructure Tests

	public void Test_IsStructure_Class1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsStructure"));
		Assert(navigator.MoveToType("Class1"));
		Assert(!navigator.IsStructure);
	}

	public void Test_IsStructure_Delegate1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsStructure"));
		Assert(navigator.MoveToType("Delegate1"));
		Assert(!navigator.IsStructure);
	}

	public void Test_IsStructure_Enumeration1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsStructure"));
		Assert(navigator.MoveToType("Enumeration1"));
		Assert(!navigator.IsStructure);
	}

	public void Test_IsStructure_Interface1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsStructure"));
		Assert(navigator.MoveToType("Interface1"));
		Assert(!navigator.IsStructure);
	}

	public void Test_IsStructure_Structure1()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsStructure"));
		Assert(navigator.MoveToType("Structure1"));
		Assert(navigator.IsStructure);
	}

	#endregion

	#region IsTypeAbstract Tests

	public void Test_IsTypeAbstract_AbstractClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsTypeAbstract"));
		Assert(navigator.MoveToType("AbstractClass"));
		Assert(navigator.IsTypeAbstract);
	}

	public void Test_IsTypeAbstract_NormalClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsTypeAbstract"));
		Assert(navigator.MoveToType("NormalClass"));
		Assert(!navigator.IsTypeAbstract);
	}

	public void Test_IsTypeAbstract_SealedClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsTypeAbstract"));
		Assert(navigator.MoveToType("SealedClass"));
		Assert(!navigator.IsTypeAbstract);
	}

	#endregion

	#region IsTypeSealed Tests

	public void Test_IsTypeSealed_AbstractClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsTypeSealed"));
		Assert(navigator.MoveToType("AbstractClass"));
		Assert(!navigator.IsTypeSealed);
	}

	public void Test_IsTypeSealed_NormalClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsTypeSealed"));
		Assert(navigator.MoveToType("NormalClass"));
		Assert(!navigator.IsTypeSealed);
	}

	public void Test_IsTypeSealed_SealedClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.IsTypeSealed"));
		Assert(navigator.MoveToType("SealedClass"));
		Assert(navigator.IsTypeSealed);
	}

	#endregion

	#region MemberDeclaringType Tests

	public void Test_MemberDeclaringType_NoMembers()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MemberDeclaringType"));
		Assert(navigator.MoveToType("NoMembers"));
		Assert(navigator.MoveToMember("Equals"));
		AssertEquals("Object", navigator.MemberDeclaringType);
	}

	#endregion

	#region MoveToFirstClass Tests

	public void Test_MoveToFirstClass_OneClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstClass.OneClass"));
		Assert(navigator.MoveToFirstClass());
		AssertEquals("Class1", navigator.TypeName);
	}

	public void Test_MoveToFirstClass_OneInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstClass.OneInterface"));
		Assert(!navigator.MoveToFirstClass());
	}

	public void Test_MoveToFirstClass_TwoClasses()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstClass.TwoClasses"));
		Assert(navigator.MoveToFirstClass());
		AssertEquals("Class1", navigator.TypeName);
	}

	#endregion

	#region MoveToFirstConstructor Tests

	public void Test_MoveToFirstConstructor_DefaultConstructor()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstConstructor"));
		Assert(navigator.MoveToType("DefaultConstructor"));
		Assert(navigator.MoveToFirstConstructor("public"));
	}

	#endregion

	#region MoveToFirstDelegate Tests

	public void Test_MoveToFirstDelegate_OneClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstDelegate.OneClass"));
		Assert(!navigator.MoveToFirstDelegate());
	}

	public void Test_MoveToFirstDelegate_OneDelegate()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstDelegate.OneDelegate"));
		Assert(navigator.MoveToFirstDelegate());
		AssertEquals("Delegate1", navigator.TypeName);
	}

	public void Test_MoveToFirstDelegate_TwoDelegates()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstDelegate.TwoDelegates"));
		Assert(navigator.MoveToFirstDelegate());
		AssertEquals("Delegate1", navigator.TypeName);
	}

	#endregion

	#region MoveToFirstEnumeration Tests

	public void Test_MoveToFirstEnumeration_OneClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstEnumeration.OneClass"));
		Assert(!navigator.MoveToFirstEnumeration());
	}

	public void Test_MoveToFirstEnumeration_OneEnumeration()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstEnumeration.OneEnumeration"));
		Assert(navigator.MoveToFirstEnumeration());
		AssertEquals("Enumeration1", navigator.TypeName);
	}

	public void Test_MoveToFirstEnumeration_TwoEnumerations()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstEnumeration.TwoEnumerations"));
		Assert(navigator.MoveToFirstEnumeration());
		AssertEquals("Enumeration1", navigator.TypeName);
	}

	#endregion

	#region MoveToFirstInterface Tests

	public void Test_MoveToFirstInterface_OneClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstInterface.OneClass"));
		Assert(!navigator.MoveToFirstInterface());
	}

	public void Test_MoveToFirstInterface_OneInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstInterface.OneInterface"));
		Assert(navigator.MoveToFirstInterface());
		AssertEquals("Interface1", navigator.TypeName);
	}

	public void Test_MoveToFirstInterface_TwoInterfaces()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstInterface.TwoInterfaces"));
		Assert(navigator.MoveToFirstInterface());
		AssertEquals("Interface1", navigator.TypeName);
	}

	#endregion

	#region MoveToFirstInterfaceImplementedByType Tests

	public void Test_MoveToFirstInterfaceImplementedByType_NoInterfaces()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstInterfaceImplementedByType"));
		Assert(navigator.MoveToType("NoInterfaces"));
		Assert(!navigator.MoveToFirstInterfaceImplementedByType());
	}

	public void Test_MoveToFirstInterfaceImplementedByType_OneInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstInterfaceImplementedByType"));
		Assert(navigator.MoveToType("OneInterface"));
		Assert(navigator.MoveToFirstInterfaceImplementedByType());
		AssertEquals("Interface1", navigator.ImplementedInterfaceName);
	}

	public void Test_MoveToFirstInterfaceImplementedByType_TwoInterfaces()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstInterfaceImplementedByType"));
		Assert(navigator.MoveToType("TwoInterfaces"));
		Assert(navigator.MoveToFirstInterfaceImplementedByType());
		AssertEquals("Interface2", navigator.ImplementedInterfaceName);
	}

	#endregion

	#region MoveToFirstMethod Tests

	public void Test_MoveToFirstMethod_OneMethod()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstMethod"));
		Assert(navigator.MoveToType("OneMethod"));
		Assert(navigator.MoveToFirstMethod("public"));
		AssertEquals("Equals", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("GetHashCode", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("GetType", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("Method1", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("ToString", navigator.MemberName);
		Assert(!navigator.MoveToNextMember());
	}

	public void Test_MoveToFirstMethod_OneMethodAndOneProperty()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstMethod"));
		Assert(navigator.MoveToType("OneMethodAndOneProperty"));
		Assert(navigator.MoveToFirstMethod("public"));
		AssertEquals("Equals", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("GetHashCode", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("GetType", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("Method1", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("ToString", navigator.MemberName);
		Assert(!navigator.MoveToNextMember());
	}

	public void Test_MoveToFirstMethod_OneProperty()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstMethod"));
		Assert(navigator.MoveToType("OneProperty"));
		Assert(navigator.MoveToFirstMethod("public"));
		AssertEquals("Equals", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("GetHashCode", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("GetType", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("ToString", navigator.MemberName);
		Assert(!navigator.MoveToNextMember());
	}

	public void Test_MoveToFirstMethod_TwoMethods()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstMethod"));
		Assert(navigator.MoveToType("TwoMethods"));
		Assert(navigator.MoveToFirstMethod("public"));
		AssertEquals("Equals", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("GetHashCode", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("GetType", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("Method1", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("Method2", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("ToString", navigator.MemberName);
		Assert(!navigator.MoveToNextMember());
	}

	#endregion

	#region MoveToFirstNamespace Tests

	public void Test_MoveToFirstNamespace()
	{
		Assert(navigator.MoveToFirstNamespace());
		AssertEquals("(global)", navigator.NamespaceName);
	}

	#endregion

	#region MoveToFirstProperty Tests

	public void Test_MoveToFirstProperty_OneMethod()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstProperty"));
		Assert(navigator.MoveToType("OneMethod"));
		Assert(!navigator.MoveToFirstProperty("public"));
	}

	public void Test_MoveToFirstProperty_OneMethodAndOneProperty()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstProperty"));
		Assert(navigator.MoveToType("OneMethodAndOneProperty"));
		Assert(navigator.MoveToFirstProperty("public"));
		AssertEquals("Property1", navigator.MemberName);
		Assert(!navigator.MoveToNextMember());
	}

	public void Test_MoveToFirstProperty_OneProperty()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstProperty"));
		Assert(navigator.MoveToType("OneProperty"));
		Assert(navigator.MoveToFirstProperty("public"));
		AssertEquals("Property1", navigator.MemberName);
		Assert(!navigator.MoveToNextMember());
	}

	public void Test_MoveToFirstProperty_TwoProperties()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstProperty"));
		Assert(navigator.MoveToType("TwoProperties"));
		Assert(navigator.MoveToFirstProperty("public"));
		AssertEquals("Property1", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("Property2", navigator.MemberName);
		Assert(!navigator.MoveToNextMember());
	}

	#endregion

	#region MoveToFirstStructure Tests

	public void Test_MoveToFirstStructure_OneClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstStructure.OneClass"));
		Assert(!navigator.MoveToFirstStructure());
	}

	public void Test_MoveToFirstStructure_OneInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstStructure.OneStructure"));
		Assert(navigator.MoveToFirstStructure());
		AssertEquals("Structure1", navigator.TypeName);
	}

	public void Test_MoveToFirstStructure_TwoStructures()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToFirstStructure.TwoStructures"));
		Assert(navigator.MoveToFirstStructure());
		AssertEquals("Structure1", navigator.TypeName);
	}

	#endregion

	#region MoveToMember Tests

	public void Test_MoveToMember_NoMethods()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToMember"));
		Assert(navigator.MoveToType("NoMembers"));
		Assert(!navigator.MoveToMember("NonExistantMember"));
	}

	public void Test_MoveToMember_OneMethod()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToMember"));
		Assert(navigator.MoveToType("OneMethod"));
		Assert(navigator.MoveToMember("Method1"));
		AssertEquals("Method1", navigator.MemberName);
	}

	#endregion

	#region MoveToNamespace Tests

	public void Test_MoveToNamespace()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToNamespace"));
		AssertEquals("NDoc.Test.AssemblyNavigator.MoveToNamespace", navigator.NamespaceName);
	}

	public void Test_MoveToNamespace_GlobalNamespace()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test"));
		Assert(navigator.MoveToNamespace(null));
		AssertEquals("(global)", navigator.NamespaceName);
	}

	#endregion

	#region MoveToNextInterfaceImplementedByType Tests

	public void Test_MoveToNextInterfaceImplementedByType_OneInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToNextInterfaceImplementedByType"));
		Assert(navigator.MoveToType("OneInterface"));
		Assert(navigator.MoveToFirstInterfaceImplementedByType());
		AssertEquals("Interface1", navigator.ImplementedInterfaceName);
		Assert(!navigator.MoveToNextInterfaceImplementedByType());
	}

	public void Test_MoveToNextInterfaceImplementedByType_TwoInterfaces()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToNextInterfaceImplementedByType"));
		Assert(navigator.MoveToType("TwoInterfaces"));
		Assert(navigator.MoveToFirstInterfaceImplementedByType());
		AssertEquals("Interface2", navigator.ImplementedInterfaceName);
		Assert(navigator.MoveToNextInterfaceImplementedByType());
		AssertEquals("Interface1", navigator.ImplementedInterfaceName);
		Assert(!navigator.MoveToNextInterfaceImplementedByType());
	}

	#endregion

	#region MoveToNextNamespace Tests

	public void Test_MoveToNextNamespace()
	{
		// Since we'll be constantly adding new namespaces to the NDocTest
		// assembly, we can't test for each namespace name explicitly.
		// So just make sure that there's more than two and that they're in
		// alphabetic order.

		navigator.MoveToFirstNamespace(); // the global namespace
		navigator.MoveToNextNamespace(); // the NDoc.Test namespace

		string previousNamespaceName = navigator.NamespaceName;
		int n = 1;

		while (navigator.MoveToNextNamespace())
		{
			Assert(previousNamespaceName.CompareTo(navigator.NamespaceName) < 0);
			previousNamespaceName = navigator.NamespaceName;
			++n;
		}

		Assert(n > 2);
	}

	#endregion

	#region MoveToNextType Tests

	public void Test_MoveToNextType_TwoClasses()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToNextType.TwoClasses"));
		Assert(navigator.MoveToFirstClass());
		AssertEquals("Class1", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("Class2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());
	}

	public void Test_MoveToNextType_PrivateImplementationDetails()
	{
		AssertEquals("(global)", navigator.NamespaceName);

		navigator.MoveToFirstClass();

		do
		{
			Assert(!navigator.TypeName.StartsWith("<PrivateImplementationDetails>"));
		}
		while (navigator.MoveToNextType());
	}

	#endregion

	#region MoveToType Tests

	public void Test_MoveToType()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.MoveToType"));
		Assert(navigator.MoveToType("Class1"));
		AssertEquals("Class1", navigator.TypeName);
	}

	#endregion

	#region NamespaceHasClasses Tests

	public void Test_NamespaceHasClasses_OneClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasClasses.OneClass"));
		Assert(navigator.NamespaceHasClasses);
	}

	public void Test_NamespaceHasClasses_OneDelegate()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasClasses.OneDelegate"));
		Assert(!navigator.NamespaceHasClasses);
	}

	public void Test_NamespaceHasClasses_OneEnumeration()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasClasses.OneEnumeration"));
		Assert(!navigator.NamespaceHasClasses);
	}

	public void Test_NamespaceHasClasses_OneInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasClasses.OneInterface"));
		Assert(!navigator.NamespaceHasClasses);
	}

	public void Test_NamespaceHasClasses_OneStructure()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasClasses.OneStructure"));
		Assert(!navigator.NamespaceHasClasses);
	}

	#endregion

	#region NamespaceHasDelegates Tests

	public void Test_NamespaceHasDelegates_OneClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasDelegates.OneClass"));
		Assert(!navigator.NamespaceHasDelegates);
	}

	public void Test_NamespaceHasDelegates_OneDelegate()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasDelegates.OneDelegate"));
		Assert(navigator.NamespaceHasDelegates);
	}

	public void Test_NamespaceHasDelegates_OneEnumeration()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasDelegates.OneEnumeration"));
		Assert(!navigator.NamespaceHasDelegates);
	}

	public void Test_NamespaceHasDelegates_OneInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasDelegates.OneInterface"));
		Assert(!navigator.NamespaceHasDelegates);
	}

	public void Test_NamespaceHasDelegates_OneStructure()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasDelegates.OneStructure"));
		Assert(!navigator.NamespaceHasDelegates);
	}

	#endregion

	#region NamespaceHasEnumerations Tests

	public void Test_NamespaceHasEnumerations_OneClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasEnumerations.OneClass"));
		Assert(!navigator.NamespaceHasEnumerations);
	}

	public void Test_NamespaceHasEnumerations_OneDelegate()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasEnumerations.OneDelegate"));
		Assert(!navigator.NamespaceHasEnumerations);
	}

	public void Test_NamespaceHasEnumerations_OneEnumeration()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasEnumerations.OneEnumeration"));
		Assert(navigator.NamespaceHasEnumerations);
	}

	public void Test_NamespaceHasEnumerations_OneInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasEnumerations.OneInterface"));
		Assert(!navigator.NamespaceHasEnumerations);
	}

	public void Test_NamespaceHasEnumerations_OneStructure()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasEnumerations.OneStructure"));
		Assert(!navigator.NamespaceHasEnumerations);
	}

	#endregion

	#region NamespaceHasInterfaces Tests

	public void Test_NamespaceHasInterfaces_OneClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasInterfaces.OneClass"));
		Assert(!navigator.NamespaceHasInterfaces);
	}

	public void Test_NamespaceHasInterfaces_OneDelegate()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasInterfaces.OneDelegate"));
		Assert(!navigator.NamespaceHasInterfaces);
	}

	public void Test_NamespaceHasInterfaces_OneEnumeration()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasInterfaces.OneEnumeration"));
		Assert(!navigator.NamespaceHasInterfaces);
	}

	public void Test_NamespaceHasInterfaces_OneInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasInterfaces.OneInterface"));
		Assert(navigator.NamespaceHasInterfaces);
	}

	public void Test_NamespaceHasInterfaces_OneStructure()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasInterfaces.OneStructure"));
		Assert(!navigator.NamespaceHasInterfaces);
	}

	#endregion

	#region NamespaceHasStructures Tests

	public void Test_NamespaceHasStructures_OneClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasStructures.OneClass"));
		Assert(!navigator.NamespaceHasStructures);
	}

	public void Test_NamespaceHasStructures_OneDelegate()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasStructures.OneDelegate"));
		Assert(!navigator.NamespaceHasStructures);
	}

	public void Test_NamespaceHasStructures_OneEnumeration()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasStructures.OneEnumeration"));
		Assert(!navigator.NamespaceHasStructures);
	}

	public void Test_NamespaceHasStructures_OneInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasStructures.OneInterface"));
		Assert(!navigator.NamespaceHasStructures);
	}

	public void Test_NamespaceHasStructures_OneStructure()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasStructures.OneStructure"));
		Assert(navigator.NamespaceHasStructures);
	}

	#endregion

	#region NamespaceHasTypes Tests

	public void Test_NamespaceHasTypes_OneClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasTypes.OneClass"));
		Assert(navigator.NamespaceHasTypes);
	}

	public void Test_NamespaceHasTypes_OneDelegate()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasTypes.OneDelegate"));
		Assert(navigator.NamespaceHasTypes);
	}

	public void Test_NamespaceHasTypes_OneEnumeration()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasTypes.OneEnumeration"));
		Assert(navigator.NamespaceHasTypes);
	}

	public void Test_NamespaceHasTypes_OneInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasTypes.OneInterface"));
		Assert(navigator.NamespaceHasTypes);
	}

	public void Test_NamespaceHasTypes_OneStructure()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.NamespaceHasTypes.OneStructure"));
		Assert(navigator.NamespaceHasTypes);
	}

	#endregion

	#region TypeHasBaseType Tests

	public void Test_TypeHasBaseType_BaseClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.TypeHasBaseType"));
		Assert(navigator.MoveToType("BaseClass"));
		Assert(!navigator.TypeHasBaseType);
	}

	public void Test_TypeHasBaseType_DerivedClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.TypeHasBaseType"));
		Assert(navigator.MoveToType("DerivedClass"));
		Assert(navigator.TypeHasBaseType);
	}

	#endregion

	#region TypeHasConstructors Tests

	public void Test_TypeHasConstructors_DefaultConstructor()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.TypeHasConstructors"));
		Assert(navigator.MoveToType("DefaultConstructor"));
		Assert(navigator.TypeHasConstructors("public"));
	}

	public void Test_TypeHasConstructors_OnePrivateConstructor()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.TypeHasConstructors"));
		Assert(navigator.MoveToType("OnePrivateConstructor"));
		Assert(!navigator.TypeHasConstructors("public"));
	}

	public void Test_TypeHasConstructors_OnePublicConstructor()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.TypeHasConstructors"));
		Assert(navigator.MoveToType("OnePublicConstructor"));
		Assert(navigator.TypeHasConstructors("public"));
	}

	#endregion

	#region TypeHasMethods Tests

	public void Test_TypeHasMethods_NoDeclaredMethods()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.TypeHasMethods"));
		Assert(navigator.MoveToType("NoDeclaredMethods"));
		Assert(navigator.TypeHasMethods("public"));
	}

	#endregion

	#region TypeHasOverloadedConstructors Tests

	public void Test_TypeHasOverloadedConstructors_DefaultConstructor()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.TypeHasOverloadedConstructors"));
		Assert(navigator.MoveToType("DefaultConstructor"));
		Assert(!navigator.TypeHasOverloadedConstructors());
	}

	public void Test_TypeHasOverloadedConstructors_OnePublicAndOnePrivateConstructors()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.TypeHasOverloadedConstructors"));
		Assert(navigator.MoveToType("OnePublicAndOnePrivateConstructors"));
		Assert(navigator.TypeHasOverloadedConstructors());
	}

	public void Test_TypeHasOverloadedConstructors_OnePublicConstructor()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.TypeHasOverloadedConstructors"));
		Assert(navigator.MoveToType("OnePublicConstructor"));
		Assert(!navigator.TypeHasOverloadedConstructors());
	}

	public void Test_TypeHasOverloadedConstructors_TwoPublicConstructors()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.TypeHasOverloadedConstructors"));
		Assert(navigator.MoveToType("TwoPublicConstructors"));
		Assert(navigator.TypeHasOverloadedConstructors());
	}

	#endregion

	#region TypeHasProperties Tests

	public void Test_TypeHasProperties_NoProperties()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.TypeHasProperties"));
		Assert(navigator.MoveToType("NoProperties"));
		Assert(!navigator.TypeHasProperties("public"));
	}

	#endregion

	#region TypeImplementsInterfaces Tests

	public void Test_TypeImplementsInterfaces_Interfaces()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.TypeImplementsInterfaces"));
		Assert(navigator.MoveToType("Interfaces"));
		Assert(navigator.TypeImplementsInterfaces);
	}

	public void Test_TypeImplementsInterfaces_NoInterfaces()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.TypeImplementsInterfaces"));
		Assert(navigator.MoveToType("NoInterfaces"));
		Assert(!navigator.TypeImplementsInterfaces);
	}

	#endregion

	#region TypeName Tests

	public void Test_TypeName_OuterClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.TypeName"));
		Assert(navigator.MoveToType("OuterClass"));
		AssertEquals("OuterClass", navigator.TypeName);
	}

	public void Test_TypeName_OuterClass_NestedClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AssemblyNavigator.TypeName"));
		Assert(navigator.MoveToType("OuterClass.NestedClass"));
		AssertEquals("OuterClass.NestedClass", navigator.TypeName);
	}

	#endregion
}
