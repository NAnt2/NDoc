using System;

// As long as all the other namespaces in this assembly are prefixed with 
// NDoc.Test, this will always be the first namespace.
namespace NDoc.Test
{
	/// <summary>This is Class1.</summary>
	public class Class1 {}
}

namespace NDoc.Test.TwoClasses
{
	public class Class1 {}
	public class Class2 {}
}

namespace NDoc.Test.TwoInterfaces
{
	public interface Interface1 {}
	public interface Interface2 {}
}

namespace NDoc.Test.TwoStructures
{
	public struct Structure1 {}
	public struct Structure2 {}
}

namespace NDoc.Test.TwoDelegates
{
	public delegate void Delegate1();
	public delegate void Delegate2();
}

namespace NDoc.Test.TwoEnumerations
{
	public enum Enumeration1 {}
	public enum Enumeration2 {}
}

namespace NDoc.Test.TwoOfEverything
{
	public class Class1 {}
	public interface Interface1 {}
	public struct Structure1 {}
	public delegate void Delegate1();
	public enum Enumeration1 {};

	public class Class2 {}
	public interface Interface2 {}
	public struct Structure2 {}
	public delegate void Delegate2();
	public enum Enumeration2 {};
}

namespace NDoc.Test.TwoOfEverythingOutOfOrder
{
	public class Class2 {}
	public interface Interface2 {}
	public struct Structure2 {}
	public delegate void Delegate2();
	public enum Enumeration2 {};

	public class Class1 {}
	public interface Interface1 {}
	public struct Structure1 {}
	public delegate void Delegate1();
	public enum Enumeration1 {};
}

namespace NDoc.Test.Summaries
{
	/// <summary>This summary has no para element.</summary>
	public class SummaryWithoutPara {}

	/// <summary><para>This summary has one para element.</para></summary>
	public class SummaryWithPara {}

	public class NoSummary {}

	/// <summary>
	///		<para>This summary has two para elements.</para>
	///		<para>This is the second para.</para>
	/// </summary>
	public class SummaryWithTwoParas {}
}
