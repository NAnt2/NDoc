using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace NDoc.Core
{
	/// <summary>
	///		<para>Represents an XML-based template used to generate
	///		customizable documentation output.</para>
	/// </summary>
	public class Template
	{
		#region Fields

		private Hashtable instructions;
		private Hashtable variables;
		private Hashtable vbTypeNames;
		private Hashtable csTypeNames;
		private XmlDocument templateDocument;
		private string namespaceName;
		private string typeName;
		private string membersName;
		private string memberName;
		private int overloadID;
		private AssemblyNavigator assemblyNavigator;
		private AssemblyDocumentation assemblyDocumentation;
		private XmlWriter resultWriter;

		#endregion

		#region Constructor

		/// <summary>
		///		<para>Initializes a new instance of the Template class.</para>
		/// </summary>
		public Template()
		{
			FindInstructions();
			FindVariables();

			SetUpCSTypeNames();
			SetUpVBTypeNames();
		}

		#endregion

		#region Instruction and Variable Helpers

		private delegate void Instruction(XmlElement instructionElement);
		private delegate string Variable();

		private void FindInstructions()
		{
			instructions = new Hashtable();

			foreach (MethodInfo method in this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
			{
				TemplateInstructionAttribute templateInstruction = 
					Attribute.GetCustomAttribute(method, typeof(TemplateInstructionAttribute)) as TemplateInstructionAttribute;

				if (templateInstruction != null)
				{
					Instruction instruction = Delegate.CreateDelegate(typeof(Instruction), this, method.Name) as Instruction;
					instructions.Add(templateInstruction.Name, instruction);
				}
			}
		}

		private void FindVariables()
		{
			variables = new Hashtable();

			foreach (MethodInfo method in this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
			{
				TemplateVariableAttribute templateVariable = 
					Attribute.GetCustomAttribute(method, typeof(TemplateVariableAttribute)) as TemplateVariableAttribute;

				if (templateVariable != null)
				{
					Variable variable = Delegate.CreateDelegate(typeof(Variable), this, method.Name) as Variable;
					variables.Add(templateVariable.Name, variable);
				}
			}
		}

		private void SetUpCSTypeNames()
		{
			csTypeNames = new Hashtable();

			csTypeNames.Add("System.Boolean", "bool");
			csTypeNames.Add("System.Byte", "byte");
			csTypeNames.Add("System.Char", "char");
			csTypeNames.Add("System.Decimal", "decimal");
			csTypeNames.Add("System.Double", "double");
			csTypeNames.Add("System.Int16", "short");
			csTypeNames.Add("System.Int32", "int");
			csTypeNames.Add("System.Int64", "long");
			csTypeNames.Add("System.Object", "object");
			csTypeNames.Add("System.SByte", "sbyte");
			csTypeNames.Add("System.Single", "float");
			csTypeNames.Add("System.String", "string");
			csTypeNames.Add("System.UInt16", "ushort");
			csTypeNames.Add("System.UInt32", "uint");
			csTypeNames.Add("System.UInt64", "ulong");
			csTypeNames.Add("System.Void", "void");
		}

		private void SetUpVBTypeNames()
		{
			vbTypeNames = new Hashtable();

			vbTypeNames.Add("System.Boolean", "Boolean");
			vbTypeNames.Add("System.Byte", "Byte");
			vbTypeNames.Add("System.Char", "Char");
			vbTypeNames.Add("System.DateTime", "Date");
			vbTypeNames.Add("System.Decimal", "Decimal");
			vbTypeNames.Add("System.Double", "Double");
			vbTypeNames.Add("System.Int16", "Short");
			vbTypeNames.Add("System.Int32", "Integer");
			vbTypeNames.Add("System.Int64", "Long");
			vbTypeNames.Add("System.Object", "Object");
			vbTypeNames.Add("System.Single", "Single");
			vbTypeNames.Add("System.String", "String");
		}

		private string GetTypeName(string lang, string fullName, string defaultName)
		{
			string name = null;

			if (lang != null && lang.Length > 0)
			{
				if (lang == "VB")
				{
					name = vbTypeNames[fullName] as string;
				}
				else if (lang == "C#")
				{
					name = csTypeNames[fullName] as string;
				}
			}

			if (name == null || name == String.Empty)
			{
				name = defaultName;
			}

			return name;
		}

		#endregion

		#region Load Methods

		/// <summary>
		///		<para>Loads a template from the specified file.</para>
		/// </summary>
		/// <param name="fileName"></param>
		public void Load(string fileName)
		{
			Load(CreateValidatingReader(new XmlTextReader(fileName)));
		}

		/// <summary>
		///		<para>Loads a template from the specified XmlReader.</para>
		/// </summary>
		/// <param name="reader"></param>
		public void Load(XmlReader reader)
		{
			templateDocument = new XmlDocument();
			templateDocument.PreserveWhitespace = true;
			templateDocument.Load(reader);
		}

		/// <summary>
		///		<para>Loads the template from a string.</para>
		/// </summary>
		/// <param name="xml"></param>
		public void LoadXml(string xml)
		{
			XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(xml));
			Load(CreateValidatingReader(xmlTextReader));
		}

		private XmlValidatingReader CreateValidatingReader(XmlReader xmlReader)
		{
			XmlValidatingReader xmlValidatingReader = new XmlValidatingReader(xmlReader);
			xmlValidatingReader.ValidationType = ValidationType.None;
			xmlValidatingReader.EntityHandling = EntityHandling.ExpandEntities;
			return xmlValidatingReader;
		}

		#endregion

		#region Evaluate Methods

		/// <summary>
		///		<para>Evaluates the template.</para>
		/// </summary>
		/// <param name="assemblyNavigator"></param>
		/// <param name="documentation"></param>
		/// <param name="result"></param>
		public void Evaluate(
			AssemblyNavigator assemblyNavigator,
			string documentation,
			TextWriter result)
		{
			Evaluate(null, null, null, null, 0, assemblyNavigator, documentation, result);
		}

		/// <summary>
		///		<para>Evaluates the template for the specified namespace name.</para>
		/// </summary>
		/// <param name="namespaceName"></param>
		/// <param name="assemblyNavigator"></param>
		/// <param name="documentation"></param>
		/// <param name="result"></param>
		public void EvaluateNamespace(
			string namespaceName,
			AssemblyNavigator assemblyNavigator,
			string documentation,
			TextWriter result)
		{
			Evaluate(namespaceName, null, null, null, 0, assemblyNavigator, documentation, result);
		}

		/// <summary>
		///		<para>Evaluates the template for the specified namespace name
		///		and type name.</para>
		/// </summary>
		/// <param name="namespaceName"></param>
		/// <param name="typeName"></param>
		/// <param name="assemblyNavigator"></param>
		/// <param name="documentation"></param>
		/// <param name="result"></param>
		public void EvaluateType(
			string namespaceName,
			string typeName,
			AssemblyNavigator assemblyNavigator,
			string documentation,
			TextWriter result)
		{
			Evaluate(namespaceName, typeName, null, null, 0, assemblyNavigator, documentation, result);
		}

		/// <summary>
		///		<para>Evaluates the template for the specified namespace name,
		///		type name, and overloaded member name.</para>
		/// </summary>
		/// <param name="namespaceName"></param>
		/// <param name="typeName"></param>
		/// <param name="membersName"></param>
		/// <param name="assemblyNavigator"></param>
		/// <param name="documentation"></param>
		/// <param name="result"></param>
		public void EvaluateMembers(
			string namespaceName,
			string typeName,
			string membersName,
			AssemblyNavigator assemblyNavigator,
			string documentation,
			TextWriter result)
		{
			Evaluate(namespaceName, typeName, membersName, null, 0, assemblyNavigator, documentation, result);
		}

		/// <summary>
		///		<para>Evaluates the template for the specified namespace name.
		///		type name, and member ID.</para>
		/// </summary>
		/// <param name="namespaceName"></param>
		/// <param name="typeName"></param>
		/// <param name="memberName"></param>
		/// <param name="overloadID"></param>
		/// <param name="assemblyNavigator"></param>
		/// <param name="documentation"></param>
		/// <param name="result"></param>
		public void EvaluateMember(
			string namespaceName,
			string typeName,
			string memberName,
			int overloadID,
			AssemblyNavigator assemblyNavigator,
			string documentation,
			TextWriter result)
		{
			Evaluate(namespaceName, typeName, null, memberName, overloadID, assemblyNavigator, documentation, result);
		}

		private void Evaluate(
			string namespaceName,
			string typeName,
			string membersName,
			string memberName,
			int overloadID,
			AssemblyNavigator assemblyNavigator,
			string documentation,
			TextWriter result)
		{
			this.namespaceName = namespaceName;
			this.typeName = typeName;
			this.membersName = membersName;
			this.memberName = memberName;
			this.overloadID = overloadID;
			this.assemblyNavigator = assemblyNavigator;
			assemblyDocumentation = new AssemblyDocumentation(documentation);
			resultWriter = new XmlTextWriter(result);

			assemblyNavigator.MoveToNamespace(namespaceName);

			if (typeName != null)
			{
				assemblyNavigator.MoveToType(typeName);
			}

			if (memberName != null)
			{
				assemblyNavigator.MoveToMember(memberName);

				for (int i = 1; i < overloadID; ++i)
				{
					assemblyNavigator.MoveToNextMember();
				}
			}

			Evaluate(templateDocument.DocumentElement);
		}

		private void Evaluate(XmlNode node)
		{
			switch (node.NodeType)
			{
				case XmlNodeType.Element:
					Execute(node as XmlElement);
					break;
				case XmlNodeType.Text:
					resultWriter.WriteString(node.Value);
					break;
			}
		}

		private void EvaluateChildren(XmlNode parent)
		{
			XmlNode node = parent.FirstChild;

			while (node != null)
			{
				Evaluate(node);

				node = node.NextSibling;
			}
		}

		private void Execute(XmlElement instructionElement)
		{
			if (instructions.Contains(instructionElement.LocalName))
			{
				Instruction instruction = instructions[instructionElement.LocalName] as Instruction;
				instruction(instructionElement);
			}
			else
			{
				resultWriter.WriteStartElement(instructionElement.LocalName);
				WriteAttributes(instructionElement);
				EvaluateChildren(instructionElement);
				resultWriter.WriteEndElement();
			}
		}

		private void WriteAttributes(XmlElement element)
		{
			foreach (XmlAttribute attribute in element.Attributes)
			{
				resultWriter.WriteAttributeString(
					attribute.Prefix,
					attribute.LocalName,
					attribute.NamespaceURI,
					ExpandVariables(attribute.Value));
			}
		}

		private string ExpandVariables(string value)
		{
			StringBuilder valueBuilder = new StringBuilder();

			for (int i = 0; i < value.Length; ++i)
			{
				if (value[i] == '{' && i + 1 < value.Length && value[i + 1] == '$')
				{
					StringBuilder nameBuilder = new StringBuilder();

					i += 2;

					while (i < value.Length && (Char.IsLetter(value[i]) || value[i] == '-'))
					{
						nameBuilder.Append(value[i++]);
					}

					if (i < value.Length && value[i] == '}')
					{
						string variableName = nameBuilder.ToString();
						string newValue;

						if (variables.Contains(variableName))
						{
							Variable variable = variables[variableName] as Variable;
							newValue = variable();
						}
						else
						{
							newValue = "{$" + variableName + "}";
						}

						valueBuilder.Append(newValue);
					}
					else
					{
						valueBuilder.Append("{$");
						valueBuilder.Append(nameBuilder.ToString());
					}
				}
				else
				{
					valueBuilder.Append(value[i]);
				}
			}

			return valueBuilder.ToString();
		}

		private void EvaluateDocumentationChildren(XmlNode documentationNode, bool stripPara, bool addPara)
		{
			XmlNode node = documentationNode.FirstChild;

			if (stripPara)
			{
				if (node.Name == "para")
				{
					EvaluateDocumentationChildren(node, false, false);
					node = node.NextSibling;
				}
			}
			else if (addPara && node.NodeType != XmlNodeType.Element)
			{
				resultWriter.WriteStartElement("p");
				EvaluateDocumentation(node);
				resultWriter.WriteEndElement();
				node = node.NextSibling;
			}

			while (node != null)
			{
				EvaluateDocumentation(node);

				node = node.NextSibling;
			}
		}

		private void EvaluateDocumentation(XmlNode node)
		{
			switch (node.NodeType)
			{
				case XmlNodeType.Element:
					ExecuteDocumentation(node as XmlElement);
					break;
				case XmlNodeType.Text:
					resultWriter.WriteString(node.Value);
					break;
			}
		}

		private void ExecuteDocumentation(XmlElement documentationElement)
		{
			switch (documentationElement.LocalName)
			{
				case "para":
					Para(documentationElement);
					break;
				default:
					resultWriter.WriteStartElement(documentationElement.LocalName);
					EvaluateChildren(documentationElement);
					resultWriter.WriteEndElement();
					break;
			}
		}

		#endregion

		#region Variables

		[TemplateVariable("assembly-name")]
		private string AssemblyNameVariable()
		{
			return assemblyNavigator.AssemblyName;
		}

		[TemplateVariable("member-link")]
		private string MemberLinkVariable()
		{
			return Links.GetTypeMemberLink(assemblyNavigator.TypeFullName, assemblyNavigator.MemberName, assemblyNavigator.MemberOverloadID);
		}

		[TemplateVariable("member-or-overloads-link")]
		private string MemberOrOverloadsLinkVariable()
		{
			return Links.GetTypeMemberOverloadsLink(assemblyNavigator.TypeFullName, assemblyNavigator.MemberName);
		}

		[TemplateVariable("namespace-link")]
		private string NamespaceLinkVariable()
		{
			return Links.GetNamespaceLink(assemblyNavigator.NamespaceName);
		}

		[TemplateVariable("type-link")]
		private string TypeLinkVariable()
		{
			return Links.GetTypeLink(assemblyNavigator.TypeFullName);
		}

		[TemplateVariable("type-constructors-link")]
		private string TypeConstructorsLinkVariable()
		{
			return Links.GetTypeConstructorsLink(assemblyNavigator.TypeFullName);
		}

		[TemplateVariable("type-members-link")]
		private string TypeMembersLinkVariable()
		{
			return Links.GetTypeMembersLink(assemblyNavigator.TypeFullName);
		}

		#endregion

		#region Instructions

		[TemplateInstruction("assembly-name")]
		private void AssemblyName(XmlElement instructionElement)
		{
			resultWriter.WriteString(AssemblyNameVariable());
		}

		[TemplateInstruction("for-each-class-in-namespace")]
		private void ForEachClassInNamespace(XmlElement instructionElement)
		{
			if (assemblyNavigator.MoveToFirstClass())
			{
				do
				{
					EvaluateChildren(instructionElement);
				}
				while (assemblyNavigator.MoveToNextType());
			}
		}

		[TemplateInstruction("for-each-constructor-in-type")]
		private void ForEachConstructorInType(XmlElement instructionElement)
		{
			string access = instructionElement.GetAttribute("access");

			if (assemblyNavigator.MoveToFirstConstructor(access))
			{
				do
				{
					EvaluateChildren(instructionElement);
				}
				while (assemblyNavigator.MoveToNextMember());
			}
		}

		[TemplateInstruction("for-each-delegate-in-namespace")]
		private void ForEachDelegateInNamespace(XmlElement instructionElement)
		{
			if (assemblyNavigator.MoveToFirstDelegate())
			{
				do
				{
					EvaluateChildren(instructionElement);
				}
				while (assemblyNavigator.MoveToNextType());
			}
		}

		[TemplateInstruction("for-each-enumeration-in-namespace")]
		private void ForEachEnumerationInNamespace(XmlElement instructionElement)
		{
			if (assemblyNavigator.MoveToFirstEnumeration())
			{
				do
				{
					EvaluateChildren(instructionElement);
				}
				while (assemblyNavigator.MoveToNextType());
			}
		}

		[TemplateInstruction("for-each-interface-implemented-by-type")]
		private void ForEachInterfaceImplementedByType(XmlElement instructionElement)
		{
			if (assemblyNavigator.MoveToFirstInterfaceImplementedByType())
			{
				do
				{
					EvaluateChildren(instructionElement);
				}
				while (assemblyNavigator.MoveToNextInterfaceImplementedByType());
			}
		}

		[TemplateInstruction("for-each-interface-in-namespace")]
		private void ForEachInterfaceInNamespace(XmlElement instructionElement)
		{
			if (assemblyNavigator.MoveToFirstInterface())
			{
				do
				{
					EvaluateChildren(instructionElement);
				}
				while (assemblyNavigator.MoveToNextType());
			}
		}

		[TemplateInstruction("for-each-method-in-type")]
		private void ForEachMethodInType(XmlElement instructionElement)
		{
			string access = instructionElement.GetAttribute("access");

			if (assemblyNavigator.MoveToFirstMethod(access))
			{
				string previousMethodName = null;

				do
				{
					if (assemblyNavigator.MemberName != previousMethodName)
					{
						EvaluateChildren(instructionElement);
						previousMethodName = assemblyNavigator.MemberName;
					}
				}
				while (assemblyNavigator.MoveToNextMember());
			}
		}

		[TemplateInstruction("for-each-namespace")]
		private void ForEachNamespace(XmlElement instructionElement)
		{
			if (assemblyNavigator.MoveToFirstNamespace())
			{
				do
				{
					EvaluateChildren(instructionElement);
				}
				while (assemblyNavigator.MoveToNextNamespace());
			}
		}

		[TemplateInstruction("for-each-overloaded-member-in-type")]
		private void ForEachOverloadedMemberInType(XmlElement instructionElement)
		{
			if (assemblyNavigator.MoveToFirstOverloadedMember(membersName))
			{
				do
				{
					EvaluateChildren(instructionElement);
				}
				while (assemblyNavigator.MoveToNextMember());
			}
		}

		[TemplateInstruction("for-each-parameter-in-member")]
		private void ForEachParameterInMember(XmlElement instructionElement)
		{
			if (assemblyNavigator.MoveToFirstParameter())
			{
				do
				{
					EvaluateChildren(instructionElement);
				}
				while (assemblyNavigator.MoveToNextParameter());
			}
		}

		[TemplateInstruction("for-each-property-in-type")]
		private void ForEachPropertyInType(XmlElement instructionElement)
		{
			string access = instructionElement.GetAttribute("access");

			if (assemblyNavigator.MoveToFirstProperty(access))
			{
				do
				{
					EvaluateChildren(instructionElement);
				}
				while (assemblyNavigator.MoveToNextMember());
			}
		}

		[TemplateInstruction("for-each-structure-in-namespace")]
		private void ForEachStructureInNamespace(XmlElement instructionElement)
		{
			if (assemblyNavigator.MoveToFirstStructure())
			{
				do
				{
					EvaluateChildren(instructionElement);
				}
				while (assemblyNavigator.MoveToNextType());
			}
		}

		[TemplateInstruction("if-member-has-parameters")]
		private void IfMemberHasParameters(XmlElement instructionElement)
		{
			if (assemblyNavigator.ParameterCount > 0)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-member-has-no-value-type")]
		private void IfMemberHasNoValueType(XmlElement instructionElement)
		{
			if (assemblyNavigator.MemberValueTypeFullName == "System.Void")
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-member-has-value-type")]
		private void IfMemberHasValueType(XmlElement instructionElement)
		{
			if (assemblyNavigator.MemberValueTypeFullName != "System.Void")
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-member-is-inherited")]
		private void IfMemberIsInherited(XmlElement instructionElement)
		{
			if (assemblyNavigator.IsMemberInherited)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-member-is-overloaded")]
		private void IfMemberIsOverloaded(XmlElement instructionElement)
		{
			if (assemblyNavigator.IsMemberOverloaded)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-namespace-contains-classes")]
		private void IfNamespaceContainsClasses(XmlElement instructionElement)
		{
			if (assemblyNavigator.NamespaceHasClasses)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-namespace-contains-delegates")]
		private void IfNamespaceContainsDelegates(XmlElement instructionElement)
		{
			if (assemblyNavigator.NamespaceHasDelegates)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-namespace-contains-enumerations")]
		private void IfNamespaceContainsEnumerations(XmlElement instructionElement)
		{
			if (assemblyNavigator.NamespaceHasEnumerations)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-namespace-contains-interfaces")]
		private void IfNamespaceContainsInterfaces(XmlElement instructionElement)
		{
			if (assemblyNavigator.NamespaceHasInterfaces)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-namespace-contains-structures")]
		private void IfNamespaceContainsStructures(XmlElement instructionElement)
		{
			if (assemblyNavigator.NamespaceHasStructures)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-not-last-implemented-interface")]
		private void IfNotLastImplementedInterface(XmlElement instructionElement)
		{
			if (!assemblyNavigator.IsLastImplementedInterface)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-not-last-parameter")]
		private void IfNotLastParameter(XmlElement instructionElement)
		{
			if (!assemblyNavigator.IsLastParameter)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-type-has-base-type")]
		private void IfTypeHasBaseType(XmlElement instructionElement)
		{
			if (assemblyNavigator.TypeHasBaseType)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-type-has-base-type-or-implements-interfaces")]
		private void IfTypeHasBaseTypeOrImplementsInterfaces(XmlElement instructionElement)
		{
			if (assemblyNavigator.TypeHasBaseType || assemblyNavigator.TypeImplementsInterfaces)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-type-has-constructors")]
		private void IfTypeHasConstructors(XmlElement instructionElement)
		{
			string access = instructionElement.GetAttribute("access");

			if (assemblyNavigator.TypeHasConstructors(access))
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-type-has-methods")]
		private void IfTypeHasMethods(XmlElement instructionElement)
		{
			string access = instructionElement.GetAttribute("access");

			if (assemblyNavigator.TypeHasMethods(access))
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-type-has-overloaded-constructors")]
		private void IfTypeHasOverloadedConstructors(XmlElement instructionElement)
		{
			if (assemblyNavigator.TypeHasOverloadedConstructors())
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-type-has-properties")]
		private void IfTypeHasProperties(XmlElement instructionElement)
		{
			string access = instructionElement.GetAttribute("access");

			if (assemblyNavigator.TypeHasProperties(access))
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-type-has-remarks")]
		private void IfTypeHasRemarks(XmlElement instructionElement)
		{
			XmlNode node = assemblyDocumentation.GetTypeRemarks(assemblyNavigator.CurrentType);

			if (node != null)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-type-implements-interfaces")]
		private void IfTypeImplementsInterfaces(XmlElement instructionElement)
		{
			if (assemblyNavigator.TypeImplementsInterfaces)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-type-is-abstract")]
		private void IfTypeIsAbstract(XmlElement instructionElement)
		{
			if (assemblyNavigator.IsTypeAbstract)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("if-type-is-sealed")]
		private void IfTypeIsSealed(XmlElement instructionElement)
		{
			if (assemblyNavigator.IsTypeSealed)
			{
				EvaluateChildren(instructionElement);
			}
		}

		[TemplateInstruction("implemented-interface-name")]
		private void ImplementedInterfaceName(XmlElement instructionElement)
		{
			resultWriter.WriteString(assemblyNavigator.ImplementedInterfaceName);
		}

		[TemplateInstruction("member-access")]
		private void MemberAccess(XmlElement instructionElement)
		{
			string access = "ERROR";

			string lang = instructionElement.GetAttribute("lang");

			MemberInfo member = assemblyNavigator.CurrentMember;

			if (member is MethodBase)
			{
				MethodInfo method = member as MethodInfo;

				switch (method.Attributes & MethodAttributes.MemberAccessMask)
				{
					case MethodAttributes.Public:
						switch (lang)
						{
							case "C#":
								access = "public";
								break;
							case "VB":
								access = "Public";
								break;
						}
						break;
				}
			}

			resultWriter.WriteString(access);
		}

		[TemplateInstruction("member-declaring-type")]
		private void MemberDeclaringType(XmlElement instructionElement)
		{
			resultWriter.WriteString(assemblyNavigator.MemberDeclaringType);
		}

		[TemplateInstruction("member-name")]
		private void MemberName(XmlElement instructionElement)
		{
			resultWriter.WriteString(assemblyNavigator.MemberName);
		}

		[TemplateInstruction("member-overloads-summary")]
		private void MemberOverloadsSummary(XmlElement instructionElement)
		{
			XmlNode node = assemblyDocumentation.GetMemberOverloadsSummary(assemblyNavigator.CurrentType, membersName);

			if (node != null)
			{
				EvaluateDocumentationChildren(node, false, true);
			}
		}

		[TemplateInstruction("member-summary")]
		private void MemberSummary(XmlElement instructionElement)
		{
			bool stripPara = instructionElement.GetAttribute("strip") == "first";

			XmlNode node = assemblyDocumentation.GetMemberSummary(assemblyNavigator.CurrentMember);

			if (node != null)
			{
				EvaluateDocumentationChildren(node, stripPara, true);
			}
		}

		private string GetMemberType(string lang)
		{
			switch (lang)
			{
				case "":
					if (assemblyNavigator.IsConstructor)
					{
						return "Constructor";
					}
					else if (assemblyNavigator.IsEvent)
					{
						return "Event";
					}
					else if (assemblyNavigator.IsField)
					{
						return "Field";
					}
					else if (assemblyNavigator.IsMethod)
					{
						return "Method";
					}
					else if (assemblyNavigator.IsProperty)
					{
						return "Property";
					}
					break;
			}

			return "ERROR";
		}

		[TemplateInstruction("member-type")]
		private void MemberType(XmlElement instructionElement)
		{
			resultWriter.WriteString(GetMemberType(instructionElement.GetAttribute("lang")));
		}

		[TemplateInstruction("member-value-type-name")]
		private void MemberValueTypeName(XmlElement instructionElement)
		{
			string lang = instructionElement.GetAttribute("lang");
			string name = GetTypeName(lang, assemblyNavigator.MemberValueTypeFullName, assemblyNavigator.MemberValueTypeName);
			resultWriter.WriteString(name);
		}

		[TemplateInstruction("namespace-name")]
		private void NamespaceName(XmlElement instructionElement)
		{
			resultWriter.WriteString(assemblyNavigator.NamespaceName);
		}

		[TemplateInstruction("parameter-name")]
		private void ParameterName(XmlElement instructionElement)
		{
			resultWriter.WriteString(assemblyNavigator.ParameterName);
		}

		[TemplateInstruction("parameter-type-name")]
		private void ParameterTypeName(XmlElement instructionElement)
		{
			string lang = instructionElement.GetAttribute("lang");
			string name = GetTypeName(lang, assemblyNavigator.ParameterTypeFullName, assemblyNavigator.ParameterTypeName);
			resultWriter.WriteString(name);
		}

		[TemplateInstruction("text")]
		private void Text(XmlElement instructionElement)
		{
			resultWriter.WriteString(instructionElement.InnerText);
		}

		[TemplateInstruction("template")]
		private void TemplateInstruction(XmlElement instructionElement)
		{
			EvaluateChildren(instructionElement);
		}

		[TemplateInstruction("type-access")]
		private void TypeAccess(XmlElement instructionElement)
		{
			string access = "ERROR";

			switch (instructionElement.GetAttribute("lang"))
			{
				case "VB":
					switch (assemblyNavigator.CurrentType.Attributes & TypeAttributes.VisibilityMask)
					{
						case TypeAttributes.Public:
							access = "Public";
							break;
						case TypeAttributes.NotPublic:
							access = "Friend";
							break;
					}
					break;
				case "C#":
					switch (assemblyNavigator.CurrentType.Attributes & TypeAttributes.VisibilityMask)
					{
						case TypeAttributes.Public:
							access = "public";
							break;
						case TypeAttributes.NotPublic:
							access = "internal";
							break;
					}
					break;
			}

			resultWriter.WriteString(access);
		}

		[TemplateInstruction("type-base-type-name")]
		private void TypeBaseTypeName(XmlElement instructionElement)
		{
			resultWriter.WriteString(assemblyNavigator.CurrentType.BaseType.Name);
		}

		[TemplateInstruction("type-constructors-summary")]
		private void TypeConstructorsSummary(XmlElement instructionElement)
		{
			bool stripPara = instructionElement.GetAttribute("strip") == "first";

			XmlNode summaryNode = assemblyDocumentation.GetTypeConstructorsSummary(assemblyNavigator.CurrentType);

			if (summaryNode != null)
			{
				EvaluateDocumentationChildren(summaryNode, stripPara, true);
			}
			else
			{
				if (!stripPara)
				{
					resultWriter.WriteStartElement("p");
				}

				resultWriter.WriteString(
					"Initializes a new instance of the " + assemblyNavigator.TypeName + " " + GetTypeType(String.Empty).ToLower() + ".");

				if (!stripPara)
				{
					resultWriter.WriteEndElement();
				}
			}
		}

		[TemplateInstruction("type-name")]
		private void TypeName(XmlElement instructionElement)
		{
			resultWriter.WriteString(assemblyNavigator.TypeName);
		}

		[TemplateInstruction("type-remarks")]
		private void TypeRemarks(XmlElement instructionElement)
		{
			XmlNode node = assemblyDocumentation.GetTypeRemarks(assemblyNavigator.CurrentType);

			if (node != null)
			{
				EvaluateDocumentationChildren(node, false, true);
			}
		}

		[TemplateInstruction("type-summary")]
		private void TypeSummary(XmlElement instructionElement)
		{
			bool stripPara = instructionElement.GetAttribute("strip") == "first";

			XmlNode node = assemblyDocumentation.GetTypeSummary(assemblyNavigator.CurrentType);

			if (node != null)
			{
				EvaluateDocumentationChildren(node, stripPara, true);
			}
		}

		private string GetTypeType(string lang)
		{
			switch (lang)
			{
				case "":
					if (assemblyNavigator.IsClass)
					{
						return "Class";
					}
					else if (assemblyNavigator.IsInterface)
					{
						return "Interface";
					}
					else if (assemblyNavigator.IsStructure)
					{
						return "Structure";
					}
					else if (assemblyNavigator.IsDelegate)
					{
						return "Delegate";
					}
					else if (assemblyNavigator.IsEnumeration)
					{
						return "Enumeration";
					}
					break;
				case "VB":
					if (assemblyNavigator.IsClass)
					{
						return "Class";
					}
					else if (assemblyNavigator.IsInterface)
					{
						return "Interface";
					}
					else if (assemblyNavigator.IsStructure)
					{
						return "Structure";
					}
					else if (assemblyNavigator.IsDelegate)
					{
						return "Delegate";
					}
					else if (assemblyNavigator.IsEnumeration)
					{
						return "Enum";
					}
					break;
				case "C#":
					if (assemblyNavigator.IsClass)
					{
						return "class";
					}
					else if (assemblyNavigator.IsInterface)
					{
						return "interface";
					}
					else if (assemblyNavigator.IsStructure)
					{
						return "struct";
					}
					else if (assemblyNavigator.IsDelegate)
					{
						return "delegate";
					}
					else if (assemblyNavigator.IsEnumeration)
					{
						return "enum";
					}
					break;
			}

			return "ERROR";
		}

		[TemplateInstruction("type-type")]
		private void TypeType(XmlElement instructionElement)
		{
			resultWriter.WriteString(GetTypeType(instructionElement.GetAttribute("lang")));
		}

		#endregion

		#region Documentation Elements

		private void Para(XmlElement documentationElement)
		{
			resultWriter.WriteStartElement("p");
			EvaluateDocumentationChildren(documentationElement, false, false);
			resultWriter.WriteEndElement();
		}

		#endregion
	}
}
