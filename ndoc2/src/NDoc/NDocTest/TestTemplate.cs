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
}
