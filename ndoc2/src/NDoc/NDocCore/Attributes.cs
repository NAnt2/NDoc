using System;

namespace NDoc.Core
{
	/// <summary>
	///		<para>Indicates a method implements a template instruction.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class TemplateInstructionAttribute : Attribute
	{
		private string name;

		/// <summary>
		///		<para>Initializes a new instance of the TemplateInstructionAttribute
		///		class, specifying the name of the instruction as it should be spelled
		///		in templates.</para>
		/// </summary>
		/// <param name="name"></param>
		public TemplateInstructionAttribute(string name)
		{
			this.name = name;
		}

		/// <summary>
		///		<para>Gets the name of the template instruction.</para>
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
		}
	}

	/// <summary>
	///		<para>Indicates a method implements a template variable.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class TemplateVariableAttribute : Attribute
	{
		private string name;

		/// <summary>
		///		<para>Initializes a new instance of the TemplateVariableAttribute
		///		class, specifying the name of the variable as it should be spelled
		///		in templates.</para>
		/// </summary>
		/// <param name="name"></param>
		public TemplateVariableAttribute(string name)
		{
			this.name = name;
		}

		/// <summary>
		///		<para>Gets the name of the template variable.</para>
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
		}
	}
}
