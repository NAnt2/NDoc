namespace NDoc.Test.AssemblyDocumentation
{
	namespace GetMemberSummary
	{
		public class OneMethodNoSummary
		{
			public void Method1() {}
		}

		public class OneMethodWithSummary
		{
			/// <summary>OneMethodWithSummary.Method1() Summary</summary>
			public void Method1() {}
		}

		public class OneParameterWithSummary
		{
			/// <summary>OneParameterWithSummary.Method1(int) Summary</summary>
			public void Method1(int i) {}
		}

		public class OnePropertyWithSummary
		{
			/// <summary>OnePropertyWithSummary.Property1 Summary</summary>
			public int Property1 { get { return 0; } }
		}
		
		public class TwoParametersWithSummary
		{
			/// <summary>TwoParametersWithSummary.Method1(int,string) Summary</summary>
			public void Method1(int i, string s) {}
		}		
	}

	namespace GetTypeConstructorsSummary
	{
		public class DefaultConstructor {}

		public class OneConstructorNoSummary
		{
			public OneConstructorNoSummary() {}
		}

		public class OneConstructorWithSummary
		{
			/// <summary>OneConstructorWithSummary.OneConstructorWithSummary() Summary</summary>
			public OneConstructorWithSummary() {}
		}

		public class OverloadedConstructorsWithSummary
		{
			/// <summary>OverloadedConstructorsWithSummary.OverloadedConstructorsWithSummary() Summary</summary>
			public OverloadedConstructorsWithSummary() {}

			/// <summary>OverloadedConstructorsWithSummary.OverloadedConstructorsWithSummary(int) Summary</summary>
			public OverloadedConstructorsWithSummary(int i) {}
		}

		public class OverloadedConstructorsFirstNoSummary
		{
			public OverloadedConstructorsFirstNoSummary() {}

			/// <summary>OverloadedConstructorsFirstNoSummary.OverloadedConstructorsFirstNoSummary(int) Summary</summary>
			public OverloadedConstructorsFirstNoSummary(int i) {}
		}
	}

	namespace GetTypeRemarks
	{
		public class NestedTypeRemarks
		{
			/// <remarks>NestedTypeRemarks.NestedType Remarks</remarks>
			public class NestedType {}
		}

		public class NoSummaryOrRemarks {}

		/// <remarks>RemarksNoSummary Remarks</remarks>
		public class RemarksNoSummary {}

		/// <summary>SummaryAndRemarks Summary</summary>
		/// <remarks>SummaryAndRemarks Remarks</remarks>
		public class SummaryAndRemarks {}

		/// <summary>SummaryNoRemarks Summary</summary>
		public class SummaryNoRemarks {}
	}

	namespace GetTypeSummary
	{
		public class NestedTypeSummary
		{
			/// <summary>NestedTypeSummary.NestedType Summary</summary>
			public class NestedType {}
		}

		public class NoSummaryOrRemarks {}

		/// <remarks>RemarksNoSummary Remarks</remarks>
		public class RemarksNoSummary {}

		/// <summary>SummaryAndRemarks Summary</summary>
		/// <remarks>SummaryAndRemarks Remarks</remarks>
		public class SummaryAndRemarks {}

		/// <summary>SummaryNoRemarks Summary</summary>
		public class SummaryNoRemarks {}
	}
}
