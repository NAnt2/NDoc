using System;

// This creates a global namespace.
public class Class1 {}

// As long as all the other namespaces in this assembly are prefixed with
// NDoc.Test, this will always be the second namespace.
namespace NDoc.Test
{
	/// <summary>This is Class1.</summary>
	public class Class1 {}
	/// <summary>This is Interface1.</summary>
	public interface Interface1 {}
	/// <summary>This is Structure1.</summary>
	public struct Structure1 {}
	/// <summary>This is Delegate1.</summary>
	public delegate void Delegate1();
	/// <summary>This is Enumeration1.</summary>
	public enum Enumeration1 {}
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

namespace NDoc.Test.AllAccess
{
	public class PublicClass {}
	class InternalClass {}
}

namespace NDoc.Test.DerivedClasses
{
	public class BaseClass {}
	public class DerivedClass : BaseClass {}
	public class DerivedDerivedClass : DerivedClass {}
}

namespace NDoc.Test.ImplementsInterfaces
{
	public interface Interface1 {}
	public interface Interface2 {}
	public class ImplementsZeroInterfaces {}
	public class ImplementsOneInterface : Interface1 {}
	public class ImplementsTwoInterfaces : Interface1, Interface2 {}
}

namespace NDoc.Test.DerivedClassAndImplementsInterfaces
{
	public class BaseClass {}
	public interface Interface1 {}
	public interface Interface2 {}
	public class DerivedAndImplements : BaseClass, Interface1, Interface2 {}
}

namespace NDoc.Test.AbstractAndSealed
{
	public class NormalClass {}
	public abstract class AbstractClass {}
	public sealed class SealedClass {}
}

namespace NDoc.Test.Remarks
{
	/// <remarks><para>These remarks contains one para element.</para></remarks>
	public class RemarksWithPara {}
	/// <remarks>These remarks contains no para element.</remarks>
	public class RemarksWithoutPara {}
	public class NoRemarks {}
}

namespace NDoc.Test.Constructors
{
	public class DefaultConstructor {}

	public class PrivateConstructor
	{
		private PrivateConstructor() { }
	}

	public class TwoConstructors
	{
		public TwoConstructors() { }
		public TwoConstructors(int i) { }
	}

	public class ConstructorWithSummary
	{
		/// <summary>This constructor has a summary.</summary>
		public ConstructorWithSummary() {}
	}

	public class ConstructorWithSummaryAndRemarks
	{
		/// <summary>This constructor has a summary.</summary>
		/// <remarks>This constructor has remarks.</remarks>
		public ConstructorWithSummaryAndRemarks() {}
	}

	public class TwoConsructorsWithSummaries
	{
		/// <summary>This constructor has a summary.</summary>
		public TwoConsructorsWithSummaries() {}

		/// <summary>This constructor also has a summary.</summary>
		public TwoConsructorsWithSummaries(int i) {}
	}

	public class TwoConsructorsWithSummariesAndRemarks
	{
		/// <summary>This constructor has a summary.</summary>
		/// <remarks>This constructor has remarks.</remarks>
		public TwoConsructorsWithSummariesAndRemarks() {}

		/// <summary>This constructor also has a summary.</summary>
		/// <remarks>This constructor also has remarks.</remarks>
		public TwoConsructorsWithSummariesAndRemarks(int i) {}
	}
}

namespace NDoc.Test.PrivateImplementationDetails
{
	/// <summary>This class causes the &lt;PrivateImplementationDetails>
	/// class to appear in the compiled assembly.</summary>
	public class PrivateImplementationDetails
	{
		static byte[] foo = new byte[] { 1, 2, 3 };
	}
}

namespace NDoc.Test.NestedClassWithSummary
{
	public class OuterClass
	{
		/// <summary>This is a nested class.</summary>
		public class NestedClass {}
	}
}

namespace NDoc.Test.Methods
{
	public class NoMethods {}

	public class OneMethod
	{
		public void Method1() {}
	}

	public class TwoMethods
	{
		public void Method1() {}
		public void Method2() {}
	}

	public class OneMethodWithSummary
	{
		/// <summary>This method has a summary.</summary>
		public void Method1() {}
	}

	public class OneMethodWithParameterWithSummary
	{
		/// <summary>This method has a summary.</summary>
		public void Method1(int i) {}
	}

	public class OneMethodWithTwoParametersWithSummary
	{
		/// <summary>This method has a summary.</summary>
		public void Method1(int i, string s) {}
	}
}
