namespace NDoc.Test.AssemblyNavigator
{
	namespace AssemblyName
	{
		// The MoveToFirstNamespace test doesn't really use this namespace.
	}

	namespace IsClass
	{
		public class Class1 {}
		public delegate void Delegate1();
		public enum Enumeration1 {}
		public interface Interface1 {}
		public struct Structure1 {}
	}

	namespace IsConstructor
	{
		public class OneConstructor
		{
			public OneConstructor() {}
		}

		public class OneEvent
		{
			public event System.EventHandler Event1;
			private void StopWarning() { Event1(this, new System.EventArgs()); }
		}

		public class OneField
		{
			public int Field1;
		}

		public class OneMethod
		{
			public void Method1() {}
		}

		public class OneProperty
		{
			public int Property1 { get { return 0; } }
		}
	}

	namespace IsDelegate
	{
		public class Class1 {}
		public delegate void Delegate1();
		public enum Enumeration1 {}
		public interface Interface1 {}
		public struct Structure1 {}
	}

	namespace IsEnumeration
	{
		public class Class1 {}
		public delegate void Delegate1();
		public enum Enumeration1 {}
		public interface Interface1 {}
		public struct Structure1 {}
	}

	namespace IsEvent
	{
		public class OneConstructor
		{
			public OneConstructor() {}
		}

		public class OneEvent
		{
			public event System.EventHandler Event1;
			private void StopWarning() { Event1(this, new System.EventArgs()); }
		}

		public class OneField
		{
			public int Field1;
		}

		public class OneMethod
		{
			public void Method1() {}
		}

		public class OneProperty
		{
			public int Property1 { get { return 0; } }
		}
	}

	namespace IsField
	{
		public class OneConstructor
		{
			public OneConstructor() {}
		}

		public class OneEvent
		{
			public event System.EventHandler Event1;
			private void StopWarning() { Event1(this, new System.EventArgs()); }
		}

		public class OneField
		{
			public int Field1;
		}

		public class OneMethod
		{
			public void Method1() {}
		}

		public class OneProperty
		{
			public int Property1 { get { return 0; } }
		}
	}

	namespace IsInterface
	{
		public class Class1 {}
		public delegate void Delegate1();
		public enum Enumeration1 {}
		public interface Interface1 {}
		public struct Structure1 {}
	}

	namespace IsLastImplementedInterface
	{
		public interface Interface1 {}
		public interface Interface2 {}
		public class OneInterface : Interface1 {}
		public class TwoInterfaces : Interface2, Interface1 {}
	}

	namespace IsLastParameter
	{
		public class OneMethodOneParameter
		{
			public void Method1(int i) {}
		}

		public class OneMethodTwoParameters
		{
			public void Method1(int i, string s) {}
		}
	}

	namespace IsMemberInherited
	{
		public class OneMethod
		{
			public void Method1() {}
		}
	}

	namespace IsMemberOverloaded
	{
		public class OneMethod
		{
			public void Method1() {}
		}

		public class TwoOverloadedMethods
		{
			public void Method(int i) {}
			public void Method(string s) {}
		}
	}

	namespace IsMethod
	{
		public class OneConstructor
		{
			public OneConstructor() {}
		}

		public class OneEvent
		{
			public event System.EventHandler Event1;
			private void StopWarning() { Event1(this, new System.EventArgs()); }
		}

		public class OneField
		{
			public int Field1;
		}

		public class OneMethod
		{
			public void Method1() {}
		}

		public class OneProperty
		{
			public int Property1 { get { return 0; } }
		}
	}

	namespace IsProperty
	{
		public class OneConstructor
		{
			public OneConstructor() {}
		}

		public class OneEvent
		{
			public event System.EventHandler Event1;
			private void StopWarning() { Event1(this, new System.EventArgs()); }
		}

		public class OneField
		{
			public int Field1;
		}

		public class OneMethod
		{
			public void Method1() {}
		}

		public class OneProperty
		{
			public int Property1 { get { return 0; } }
		}
	}

	namespace IsStructure
	{
		public class Class1 {}
		public delegate void Delegate1();
		public enum Enumeration1 {}
		public interface Interface1 {}
		public struct Structure1 {}
	}

	namespace IsTypeAbstract
	{
		public abstract class AbstractClass {}
		public class NormalClass {}
		public sealed class SealedClass {}
	}

	namespace IsTypeSealed
	{
		public abstract class AbstractClass {}
		public class NormalClass {}
		public sealed class SealedClass {}
	}

	namespace MemberDeclaringType
	{
		public class NoMembers {}
	}

	namespace MemberOverloadID
	{
		public class OneMethod
		{
			public void Method1() {}
		}

		public class TwoOverloadedMethods
		{
			public void Method(int i) {}
			public void Method(string s) {}
		}
	}

	namespace MoveToFirstClass
	{
		namespace OneClass
		{
			public class Class1 {}
		}

		namespace OneInterface
		{
			public interface Interface1 {}
		}

		namespace TwoClasses
		{
			public class Class2 {}
			public class Class1 {}
		}
	}

	namespace MoveToFirstConstructor
	{
		public class DefaultConstructor {}
	}

	namespace MoveToFirstDelegate
	{
		namespace OneClass
		{
			public class Class1 {}
		}

		namespace OneDelegate
		{
			public delegate void Delegate1();
		}

		namespace TwoDelegates
		{
			public delegate void Delegate2();
			public delegate void Delegate1();
		}
	}

	namespace MoveToFirstEnumeration
	{
		namespace OneClass
		{
			public class Class1 {}
		}

		namespace OneEnumeration
		{
			public enum Enumeration1 {}
		}

		namespace TwoEnumerations
		{
			public enum Enumeration2 {}
			public enum Enumeration1 {}
		}
	}

	namespace MoveToFirstInterface
	{
		namespace OneClass
		{
			public class Class1 {}
		}

		namespace OneInterface
		{
			public interface Interface1 {}
		}

		namespace TwoInterfaces
		{
			public interface Interface2 {}
			public interface Interface1 {}
		}
	}

	namespace MoveToFirstInterfaceImplementedByType
	{
		public interface Interface1 {}
		public interface Interface2 {}
		public class NoInterfaces {}
		public class OneInterface : Interface1 {}
		public class TwoInterfaces : Interface2, Interface1 {}
	}

	namespace MoveToFirstMethod
	{
		public class OneMethod
		{
			public void Method1() {}
		}

		public class OneMethodAndOneProperty
		{
			public void Method1() {}
			public int Property1 { get { return 0; } }
		}

		public class OneProperty
		{
			public int Property1 { get { return 0; } }
		}

		public class TwoMethods
		{
			public void Method2() {}
			public void Method1() {}
		}
	}

	namespace MoveToFirstNamespace
	{
		// The MoveToFirstNamespace test doesn't really use this namespace.
	}

	namespace MoveToFirstOverloadedMember
	{
		public class TwoOverloadedMethods
		{
			public void Method(int i) {}
			public void Method(string s) {}
		}
	}

	namespace MoveToFirstParameter
	{
		public class OneMethodNoParameters
		{
			public void Method1() {}
		}

		public class OneMethodOneParameter
		{
			public void Method1(int i) {}
		}
	}

	namespace MoveToFirstProperty
	{
		public class OneMethod
		{
			public void Method1() {}
		}

		public class OneMethodAndOneProperty
		{
			public void Method1() {}
			public int Property1 { get { return 0; } }
		}

		public class OneProperty
		{
			public int Property1 { get { return 0; } }
		}

		public class TwoProperties
		{
			public int Property2 { get { return 0; } }
			public int Property1 { get { return 0; } }
		}
	}

	namespace MoveToFirstStructure
	{
		namespace OneClass
		{
			public class Class1 {}
		}

		namespace OneStructure
		{
			public struct Structure1 {}
		}

		namespace TwoStructures
		{
			public struct Structure2 {}
			public struct Structure1 {}
		}
	}

	namespace MoveToMember
	{
		public class NoMembers {}

		public class OneMethod
		{
			public void Method1() {}
		}
	}

	namespace MoveToNamespace
	{
		public class Class1 {}
	}

	namespace MoveToNextInterfaceImplementedByType
	{
		public interface Interface1 {}
		public interface Interface2 {}
		public class OneInterface : Interface1 {}
		public class TwoInterfaces : Interface2, Interface1 {}
	}

	namespace MoveToNextNamespace
	{
		// The MoveToNextNamespace test doesn't really use this namespace.
	}

	namespace MoveToNextParameter
	{
		public class OneMethodOneParameter
		{
			public void Method1(int i) {}
		}

		public class OneMethodTwoParameters
		{
			public void Method1(int i, string s) {}
		}
	}

	namespace MoveToNextType
	{
		namespace TwoClasses
		{
			public class Class2 {}
			public interface Interface1 {}
			public struct Structure1 {}
			public delegate void Delegate1();
			public enum Enumeration1 {}
			public class Class1 {}
		}
	}

	namespace MoveToType
	{
		public class Class1 {}
	}

	namespace NamespaceHasClasses
	{
		namespace OneClass
		{
			public class Class1 {}
		}

		namespace OneDelegate
		{
			public delegate void Delegate1();
		}

		namespace OneEnumeration
		{
			public enum Enumeration1 {}
		}

		namespace OneInterface
		{
			public interface Interface1 {}
		}

		namespace OneStructure
		{
			public struct Structure1 {}
		}
	}

	namespace NamespaceHasDelegates
	{
		namespace OneClass
		{
			public class Class1 {}
		}

		namespace OneDelegate
		{
			public delegate void Delegate1();
		}

		namespace OneEnumeration
		{
			public enum Enumeration1 {}
		}

		namespace OneInterface
		{
			public interface Interface1 {}
		}

		namespace OneStructure
		{
			public struct Structure1 {}
		}
	}

	namespace NamespaceHasEnumerations
	{
		namespace OneClass
		{
			public class Class1 {}
		}

		namespace OneDelegate
		{
			public delegate void Delegate1();
		}

		namespace OneEnumeration
		{
			public enum Enumeration1 {}
		}

		namespace OneInterface
		{
			public interface Interface1 {}
		}

		namespace OneStructure
		{
			public struct Structure1 {}
		}
	}

	namespace NamespaceHasInterfaces
	{
		namespace OneClass
		{
			public class Class1 {}
		}

		namespace OneDelegate
		{
			public delegate void Delegate1();
		}

		namespace OneEnumeration
		{
			public enum Enumeration1 {}
		}

		namespace OneInterface
		{
			public interface Interface1 {}
		}

		namespace OneStructure
		{
			public struct Structure1 {}
		}
	}

	namespace NamespaceHasStructures
	{
		namespace OneClass
		{
			public class Class1 {}
		}

		namespace OneDelegate
		{
			public delegate void Delegate1();
		}

		namespace OneEnumeration
		{
			public enum Enumeration1 {}
		}

		namespace OneInterface
		{
			public interface Interface1 {}
		}

		namespace OneStructure
		{
			public struct Structure1 {}
		}
	}

	namespace NamespaceHasTypes
	{
		namespace OneClass
		{
			public class Class1 {}
		}

		namespace OneDelegate
		{
			public delegate void Delegate1();
		}

		namespace OneEnumeration
		{
			public enum Enumeration1 {}
		}

		namespace OneInterface
		{
			public interface Interface1 {}
		}

		namespace OneStructure
		{
			public struct Structure1 {}
		}
	}

	namespace ParameterCount
	{
		public class OneMethodNoParameters
		{
			public void Method1() {}
		}

		public class OneMethodOneParameter
		{
			public void Method1(int i) {}
		}

		public class OneMethodTwoParameters
		{
			public void Method1(int i, string s) {}
		}
	}

	namespace TypeHasBaseType
	{
		public class BaseClass {}
		public class DerivedClass : BaseClass {}
	}

	namespace TypeHasConstructors
	{
		public class DefaultConstructor {}

		public class OnePrivateConstructor
		{
			private OnePrivateConstructor() {}
		}

		public class OnePublicConstructor
		{
			public OnePublicConstructor() {}
		}
	}

	namespace TypeHasMethods
	{
		public class NoDeclaredMethods
		{
		}
	}

	namespace TypeHasProperties
	{
		public class NoProperties
		{
		}

		public class OneProperty
		{
			public int Property1 { get { return 0; } }
		}
	}

	namespace TypeHasOverloadedConstructors
	{
		public class DefaultConstructor {}

		public class OnePublicAndOnePrivateConstructors
		{
			public OnePublicAndOnePrivateConstructors() {}
			private OnePublicAndOnePrivateConstructors(int i) {}
		}

		public class OnePublicConstructor
		{
			public OnePublicConstructor() {}
		}

		public class TwoPublicConstructors
		{
			public TwoPublicConstructors() {}
			public TwoPublicConstructors(int i) {}
		}
	}

	namespace TypeImplementsInterfaces
	{
		public interface Interface1 {}
		public class Interfaces : Interface1 {}
		public class NoInterfaces {}
	}

	namespace TypeName
	{
		public class OuterClass
		{
			public class NestedClass {}
		}
	}
}
