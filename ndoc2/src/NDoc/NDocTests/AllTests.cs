using NUnit.Framework;

namespace NDoc.Tests
{
	public class AllTests : TestCase
	{
		public AllTests(string name) : base(name) { }

		public static ITest Suite 
		{
			get 
			{
				TestSuite suite =  new TestSuite();

				suite.AddTest(new TestSuite(typeof(AssemblyDocumentationTests)));
				suite.AddTest(new TestSuite(typeof(AssemblyNavigatorTests)));
				suite.AddTest(new TestSuite(typeof(TemplateTests)));

				return suite;
			}
		}
	}
}
