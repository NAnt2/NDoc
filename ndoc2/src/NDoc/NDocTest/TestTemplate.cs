namespace NDoc.Test.Template
{
	namespace ForEachPropertyInType
	{
		public class TwoProperties
		{
			public int Property1 { get { return 0; } }
			public int Property2 { get { return 0; } }
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
}
