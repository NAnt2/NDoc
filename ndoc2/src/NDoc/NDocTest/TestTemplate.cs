namespace NDoc.Test.Template
{
	namespace ForEachMethodInType
	{
		public class OneMethod
		{
			public void Method1() {}
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

		public class TwoOverloadedMethods
		{
			public void Method(int i) {}
			public void Method(string s) {}
		}
	}

	namespace ForEachOverloadedMemberInType
	{
		public class TwoOverloadedMethods
		{
			public void Method(int i) {}
			public void Method(string s) {}
		}
	}

	namespace ForEachParameterInMember
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

	namespace ForEachPropertyInType
	{
		public class TwoProperties
		{
			public int Property2 { get { return 0; } }
			public int Property1 { get { return 0; } }
		}
	}

	namespace IfMemberIsInherited
	{
		public class NoMembers {}

		public class OneMethod
		{
			public void Method1() {}
		}
	}

	namespace IfMemberIsOverloaded
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

	namespace IfMemberHasParameters
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

	namespace IfMemberHasNoValueType
	{
		public class IntMethod
		{
			public int Method1() { return 0; }
		}

		public class VoidMethod
		{
			public void Method1() {}
		}
	}

	namespace IfMemberHasValueType
	{
		public class IntMethod
		{
			public int Method1() { return 0; }
		}

		public class VoidMethod
		{
			public void Method1() {}
		}
	}

	namespace IfNotLastParameter
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

	namespace IfTypeHasProperties
	{
		public class NoProperties
		{
		}

		public class OneProperty
		{
			public int Property1 { get { return 0; } }
		}
	}

	namespace MemberDeclaringType
	{
		public class NoMembers {}
	}

	namespace MemberLink
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

	namespace MemberOrOverloadsLink
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

	namespace MemberOverloadsSummary
	{
		public class TwoOverloadedMethods
		{
			/// <summary>TwoOverloadedMethods.Method(int) Summary</summary>
			public void Method(int i) {}
			/// <summary>TwoOverloadedMethods.Method(string) Summary</summary>
			public void Method(string s) {}
		}
	}

	namespace MemberType
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

	namespace MemberValueTypeName
	{
		public class IntMethod
		{
			public int Method1() { return 0; }
		}

		public class VoidMethod
		{
			public void Method1() {}
		}
	}

	namespace TypeConstructorsLink
	{
		public class TwoOverloadedConstructors
		{
			public TwoOverloadedConstructors() {}
			public TwoOverloadedConstructors(int i) {}
		}
	}
}
