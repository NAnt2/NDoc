using System;
using System.Reflection;

using NUnit.Framework;

using NDoc.Core;

public class AssemblyNavigatorTests : TestCase
{
	public AssemblyNavigatorTests(string name) : base(name) {}

	private Assembly assembly;
	private AssemblyNavigator navigator;

	protected override void SetUp()
	{
		assembly = typeof(NDoc.Test.Class1).Assembly;
		navigator = new AssemblyNavigator(assembly);
	}

	protected override void TearDown()
	{
	}

	public void TestAssemblyName()
	{
		AssertEquals("NDocTest", navigator.AssemblyName);
	}

	public void TestMoveToNamespace()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test"));
		AssertEquals("NDoc.Test", navigator.NamespaceName);
	}

	public void TestMoveToFirstNamespace()
	{
		Assert(navigator.MoveToFirstNamespace());
		AssertEquals("(global)", navigator.NamespaceName);
	}

	public void TestMoveToNextNamespace()
	{
		// Since we'll be constantly adding new namespaces to the NDocTest
		// assembly, we can't test for each namespace name explicitly.
		// So just make sure that there's more than one and that they're in
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

		Assert(n > 1);
	}

	public void TestNamespaceHasClasses()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoClasses"));
		Assert(navigator.NamespaceHasClasses);
		Assert(!navigator.NamespaceHasInterfaces);
		Assert(!navigator.NamespaceHasStructures);
		Assert(!navigator.NamespaceHasDelegates);
		Assert(!navigator.NamespaceHasEnumerations);
	}

	public void TestNamespaceHasInterfaces()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoInterfaces"));
		Assert(!navigator.NamespaceHasClasses);
		Assert(navigator.NamespaceHasInterfaces);
		Assert(!navigator.NamespaceHasStructures);
		Assert(!navigator.NamespaceHasDelegates);
		Assert(!navigator.NamespaceHasEnumerations);
	}

	public void TestNamespaceHasStructures()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoStructures"));
		Assert(!navigator.NamespaceHasClasses);
		Assert(!navigator.NamespaceHasInterfaces);
		Assert(navigator.NamespaceHasStructures);
		Assert(!navigator.NamespaceHasDelegates);
		Assert(!navigator.NamespaceHasEnumerations);
	}

	public void TestNamespaceHasDelegates()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoDelegates"));
		Assert(!navigator.NamespaceHasClasses);
		Assert(!navigator.NamespaceHasInterfaces);
		Assert(!navigator.NamespaceHasStructures);
		Assert(navigator.NamespaceHasDelegates);
		Assert(!navigator.NamespaceHasEnumerations);
	}

	public void TestNamespaceHasEnumerations()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoEnumerations"));
		Assert(!navigator.NamespaceHasClasses);
		Assert(!navigator.NamespaceHasInterfaces);
		Assert(!navigator.NamespaceHasStructures);
		Assert(!navigator.NamespaceHasDelegates);
		Assert(navigator.NamespaceHasEnumerations);
	}

	public void TestNamespaceHasEverything()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoOfEverything"));
		Assert(navigator.NamespaceHasClasses);
		Assert(navigator.NamespaceHasInterfaces);
		Assert(navigator.NamespaceHasStructures);
		Assert(navigator.NamespaceHasDelegates);
		Assert(navigator.NamespaceHasEnumerations);
	}

	public void TestMoveToFirstClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test"));
		AssertNull(navigator.TypeName);
		Assert(navigator.MoveToFirstClass());
		AssertEquals("Class1", navigator.TypeName);
	}

	public void TestMoveToNextClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoClasses"));
		Assert(navigator.MoveToFirstClass());
		Assert(navigator.MoveToNextType());
		AssertEquals("Class2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());
		AssertEquals("Class2", navigator.TypeName);
	}

	public void TestMoveToFirstInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoInterfaces"));
		AssertNull(navigator.TypeName);
		Assert(navigator.MoveToFirstInterface());
		AssertEquals("Interface1", navigator.TypeName);
	}

	public void TestMoveToNextInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoInterfaces"));
		Assert(navigator.MoveToFirstInterface());
		AssertEquals("Interface1", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("Interface2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());
		AssertEquals("Interface2", navigator.TypeName);
	}

	public void TestMoveToFirstStructure()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoStructures"));
		AssertNull(navigator.TypeName);
		Assert(navigator.MoveToFirstStructure());
		AssertEquals("Structure1", navigator.TypeName);
	}

	public void TestMoveToNextStructure()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoStructures"));
		Assert(navigator.MoveToFirstStructure());
		AssertEquals("Structure1", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("Structure2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());
		AssertEquals("Structure2", navigator.TypeName);
	}

	public void TestMoveToFirstDelegate()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoDelegates"));
		AssertNull(navigator.TypeName);
		Assert(navigator.MoveToFirstDelegate());
		AssertEquals("Delegate1", navigator.TypeName);
	}

	public void TestMoveToNextDelegate()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoDelegates"));
		Assert(navigator.MoveToFirstDelegate());
		AssertEquals("Delegate1", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("Delegate2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());
		AssertEquals("Delegate2", navigator.TypeName);
	}

	public void TestMoveToFirstEnumeration()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoEnumerations"));
		AssertNull(navigator.TypeName);
		Assert(navigator.MoveToFirstEnumeration());
		AssertEquals("Enumeration1", navigator.TypeName);
	}

	public void TestMoveToNextEnumeration()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoEnumerations"));
		Assert(navigator.MoveToFirstEnumeration());
		AssertEquals("Enumeration1", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("Enumeration2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());
		AssertEquals("Enumeration2", navigator.TypeName);
	}

	public void TestMoveThroughEverything()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoOfEverything"));
		
		Assert(navigator.MoveToFirstClass());
		AssertEquals("Class1", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("Class2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());

		Assert(navigator.MoveToFirstInterface());
		AssertEquals("Interface1", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("Interface2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());

		Assert(navigator.MoveToFirstStructure());
		AssertEquals("Structure1", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("Structure2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());

		Assert(navigator.MoveToFirstDelegate());
		AssertEquals("Delegate1", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("Delegate2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());

		Assert(navigator.MoveToFirstEnumeration());
		AssertEquals("Enumeration1", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("Enumeration2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());
	}

	public void TestMoveThroughEverythingOutOfOrder()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.TwoOfEverythingOutOfOrder"));
		
		Assert(navigator.MoveToFirstClass());
		AssertEquals("Class1", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("Class2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());

		Assert(navigator.MoveToFirstInterface());
		AssertEquals("Interface1", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("Interface2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());

		Assert(navigator.MoveToFirstStructure());
		AssertEquals("Structure1", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("Structure2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());

		Assert(navigator.MoveToFirstDelegate());
		AssertEquals("Delegate1", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("Delegate2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());

		Assert(navigator.MoveToFirstEnumeration());
		AssertEquals("Enumeration1", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("Enumeration2", navigator.TypeName);
		Assert(!navigator.MoveToNextType());
	}

	public void TestMoveToType()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test"));
		Assert(navigator.MoveToType("Class1"));
		AssertEquals("Class1", navigator.TypeName);
	}

	public void TestIsClass()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test"));

		Assert(navigator.MoveToType("Class1"));
		Assert(navigator.IsClass);
		Assert(!navigator.IsInterface);
		Assert(!navigator.IsStructure);
		Assert(!navigator.IsDelegate);
		Assert(!navigator.IsEnumeration);
	}

	public void TestIsInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test"));

		Assert(navigator.MoveToType("Interface1"));
		Assert(!navigator.IsClass);
		Assert(navigator.IsInterface);
		Assert(!navigator.IsStructure);
		Assert(!navigator.IsDelegate);
		Assert(!navigator.IsEnumeration);
	}

	public void TestIsStructure()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test"));

		Assert(navigator.MoveToType("Structure1"));
		Assert(!navigator.IsClass);
		Assert(!navigator.IsInterface);
		Assert(navigator.IsStructure);
		Assert(!navigator.IsDelegate);
		Assert(!navigator.IsEnumeration);
	}

	public void TestIsDelegate()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test"));

		Assert(navigator.MoveToType("Delegate1"));
		Assert(!navigator.IsClass);
		Assert(!navigator.IsInterface);
		Assert(!navigator.IsStructure);
		Assert(navigator.IsDelegate);
		Assert(!navigator.IsEnumeration);
	}

	public void TestIsEnumeration()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test"));

		Assert(navigator.MoveToType("Enumeration1"));
		Assert(!navigator.IsClass);
		Assert(!navigator.IsInterface);
		Assert(!navigator.IsStructure);
		Assert(!navigator.IsDelegate);
		Assert(navigator.IsEnumeration);
	}

	public void TestTypeHasBaseType()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.DerivedClasses"));

		Assert(navigator.MoveToType("BaseClass"));
		Assert(!navigator.TypeHasBaseType);

		Assert(navigator.MoveToType("DerivedClass"));
		Assert(navigator.TypeHasBaseType);
	}

	public void TestTypeImplementsInterfaces()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.ImplementsInterfaces"));

		Assert(navigator.MoveToType("ImplementsZeroInterfaces"));
		Assert(!navigator.TypeImplementsInterfaces);

		Assert(navigator.MoveToType("ImplementsOneInterface"));
		Assert(navigator.TypeImplementsInterfaces);

		Assert(navigator.MoveToType("ImplementsTwoInterfaces"));
		Assert(navigator.TypeImplementsInterfaces);
	}

	public void TestMoveToInterfaceImplementedByType()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.ImplementsInterfaces"));

		Assert(navigator.MoveToType("ImplementsZeroInterfaces"));
		Assert(!navigator.MoveToFirstInterfaceImplementedByType());

		Assert(navigator.MoveToType("ImplementsOneInterface"));
		Assert(navigator.MoveToFirstInterfaceImplementedByType());
		AssertEquals("Interface1", navigator.ImplementedInterfaceName);
		Assert(!navigator.MoveToNextInterfaceImplementedByType());

		Assert(navigator.MoveToType("ImplementsTwoInterfaces"));
		Assert(navigator.MoveToFirstInterfaceImplementedByType());
		AssertEquals("Interface1", navigator.ImplementedInterfaceName);
		Assert(navigator.MoveToNextInterfaceImplementedByType());
		AssertEquals("Interface2", navigator.ImplementedInterfaceName);
		Assert(!navigator.MoveToNextInterfaceImplementedByType());
	}

	public void TestIsLastImplementedInterface()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.ImplementsInterfaces"));

		Assert(navigator.MoveToType("ImplementsTwoInterfaces"));
		Assert(navigator.MoveToFirstInterfaceImplementedByType());
		Assert(!navigator.IsLastImplementedInterface);
		Assert(navigator.MoveToNextInterfaceImplementedByType());
		Assert(navigator.IsLastImplementedInterface);
	}

	public void TestIsTypeAbstract()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AbstractAndSealed"));

		Assert(navigator.MoveToType("NormalClass"));
		Assert(!navigator.IsTypeAbstract);

		Assert(navigator.MoveToType("AbstractClass"));
		Assert(navigator.IsTypeAbstract);

		Assert(navigator.MoveToType("SealedClass"));
		Assert(!navigator.IsTypeAbstract);
	}

	public void TestIsTypeSealed()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.AbstractAndSealed"));

		Assert(navigator.MoveToType("NormalClass"));
		Assert(!navigator.IsTypeSealed);

		Assert(navigator.MoveToType("AbstractClass"));
		Assert(!navigator.IsTypeSealed);

		Assert(navigator.MoveToType("SealedClass"));
		Assert(navigator.IsTypeSealed);
	}

	public void TestTypeHasConstructorsPublic()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.Constructors"));

		Assert(navigator.MoveToType("DefaultConstructor"));
		Assert(navigator.TypeHasConstructors("public"));

		Assert(navigator.MoveToType("PrivateConstructor"));
		Assert(!navigator.TypeHasConstructors("public"));
	}

	public void TestMoveToConstructors()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.Constructors"));

		Assert(navigator.MoveToType("DefaultConstructor"));
		Assert(navigator.MoveToFirstConstructor("public"));
		Assert(!navigator.MoveToNextMember());

		Assert(navigator.MoveToType("PrivateConstructor"));
		Assert(!navigator.MoveToFirstConstructor("public"));

		Assert(navigator.MoveToType("TwoConstructors"));
		Assert(navigator.MoveToFirstConstructor("public"));
		Assert(navigator.MoveToNextMember());
		Assert(!navigator.MoveToNextMember());
	}

	public void TestTypeHasOverloadedConstructors()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.Constructors"));

		Assert(navigator.MoveToType("DefaultConstructor"));
		Assert(!navigator.TypeHasOverloadedConstructors());

		Assert(navigator.MoveToType("TwoConstructors"));
		Assert(navigator.TypeHasOverloadedConstructors());
	}

	public void TestMoveToGlobalNamespace()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test"));
		Assert(navigator.MoveToNamespace(null));
		AssertEquals("(global)", navigator.NamespaceName);
	}

	public void TestPrivateImplementationDetails()
	{
		AssertEquals("(global)", navigator.NamespaceName);

		navigator.MoveToFirstClass();

		do
		{
			Assert(!navigator.TypeName.StartsWith("<PrivateImplementationDetails>"));
		}
		while (navigator.MoveToNextType());
	}

	public void TestNestedTypeName()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.NestedClassWithSummary"));
		Assert(navigator.MoveToType("OuterClass.NestedClass"));
		Assert(navigator.MoveToFirstClass());
		AssertEquals("OuterClass", navigator.TypeName);
		Assert(navigator.MoveToNextType());
		AssertEquals("OuterClass.NestedClass", navigator.TypeName);
		Assert(!navigator.MoveToNextType());
	}

	public void TestNamespaceHasTypes()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test"));
		Assert(navigator.NamespaceHasTypes);
	}

	public void TestMoveToMethods()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.Methods"));

		Assert(navigator.MoveToType("NoMethods"));
		Assert(navigator.MoveToFirstMethod("public"));
		AssertEquals("Equals", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("GetHashCode", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("GetType", navigator.MemberName);
		Assert(navigator.MoveToNextMember());
		AssertEquals("ToString", navigator.MemberName);
		Assert(!navigator.MoveToNextMember());

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

	public void TestTypeHasMethodsPublic()
	{
		Assert(navigator.MoveToNamespace("NDoc.Test.Methods"));

		Assert(navigator.MoveToType("NoMethods"));
		Assert(navigator.TypeHasMethods("public"));
	}
}
