namespace NDoc.Test.AssemblyNavigator
{
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
}
