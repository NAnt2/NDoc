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
}
