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
		private XmlDocument templateDocument;
		private string namespaceName;
		private string typeName;
		private string membersName;
		private string memberID;
		private AssemblyNavigator assemblyNavigator;
		private AssemblyDocumentation assemblyDocumentation;
		private XmlWriter resultWriter;

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
			Evaluate(null, null, null, null, assemblyNavigator, documentation, result);
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
			Evaluate(namespaceName, null, null, null, assemblyNavigator, documentation, result);
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
			Evaluate(namespaceName, typeName, null, null, assemblyNavigator, documentation, result);
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
			Evaluate(namespaceName, typeName, membersName, null, assemblyNavigator, documentation, result);
		}

		/// <summary>
		///		<para>Evaluates the template for the specified namespace name.
		///		type name, and member ID.</para>
		/// </summary>
		/// <param name="namespaceName"></param>
		/// <param name="typeName"></param>
		/// <param name="memberID"></param>
		/// <param name="assemblyNavigator"></param>
		/// <param name="documentation"></param>
		/// <param name="result"></param>
		public void EvaluateMember(
			string namespaceName,
			string typeName,
			string memberID,
			AssemblyNavigator assemblyNavigator,
			string documentation,
			TextWriter result)
		{
			Evaluate(namespaceName, typeName, null, memberID, assemblyNavigator, documentation, result);
		}

		private void Evaluate(
			string namespaceName,
			string typeName,
			string membersName,
			string memberID,
			AssemblyNavigator assemblyNavigator,
			string documentation,
			TextWriter result)
		{
			this.namespaceName = namespaceName;
			this.typeName = typeName;
			this.membersName = membersName;
			this.memberID = memberID;
			this.assemblyNavigator = assemblyNavigator;
			assemblyDocumentation = new AssemblyDocumentation(documentation);
			resultWriter = new XmlTextWriter(result);

			assemblyNavigator.MoveToNamespace(namespaceName);

			if (typeName != null)
			{
				assemblyNavigator.MoveToType(typeName);
			}

			if (memberID != null)
			{
				assemblyNavigator.MoveToMember(memberID);
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
			switch (instructionElement.LocalName)
			{
				case "assembly-name":
					AssemblyName(instructionElement);
					break;
				case "for-each-class-in-namespace":
					ForEachClassInNamespace(instructionElement);
					break;
				case "for-each-constructor-in-type":
					ForEachConstructorInType(instructionElement);
					break;
				case "for-each-delegate-in-namespace":
					ForEachDelegateInNamespace(instructionElement);
					break;
				case "for-each-enumeration-in-namespace":
					ForEachEnumerationInNamespace(instructionElement);
					break;
				case "for-each-interface-implemented-by-type":
					ForEachInterfaceImplementedByType(instructionElement);
					break;
				case "for-each-interface-in-namespace":
					ForEachInterfaceInNamespace(instructionElement);
					break;
				case "for-each-method-in-type":
					ForEachMethodInType(instructionElement);
					break;
				case "for-each-namespace":
					ForEachNamespace(instructionElement);
					break;
				case "for-each-overloaded-member-in-type":
					ForEachOverloadedMemberInType(instructionElement);
					break;
				case "for-each-parameter-in-member":
					ForEachParameterInMember(instructionElement);
					break;
				case "for-each-property-in-type":
					ForEachPropertyInType(instructionElement);
					break;
				case "for-each-structure-in-namespace":
					ForEachStructureInNamespace(instructionElement);
					break;
				case "if-member-is-inherited":
					IfMemberIsInherited(instructionElement);
					break;
				case "if-member-is-overloaded":
					IfMemberIsOverloaded(instructionElement);
					break;
				case "if-namespace-contains-classes":
					IfNamespaceContainsClasses(instructionElement);
					break;
				case "if-namespace-contains-delegates":
					IfNamespaceContainsDelegates(instructionElement);
					break;
				case "if-namespace-contains-enumerations":
					IfNamespaceContainsEnumerations(instructionElement);
					break;
				case "if-namespace-contains-interfaces":
					IfNamespaceContainsInterfaces(instructionElement);
					break;
				case "if-namespace-contains-structures":
					IfNamespaceContainsStructures(instructionElement);
					break;
				case "if-not-last-implemented-interface":
					IfNotLastImplementedInterface(instructionElement);
					break;
				case "if-not-last-parameter":
					IfNotLastParameter(instructionElement);
					break;
				case "if-type-has-base-type":
					IfTypeHasBaseType(instructionElement);
					break;
				case "if-type-has-base-type-or-implements-interfaces":
					IfTypeHasBaseTypeOrImplementsInterfaces(instructionElement);
					break;
				case "if-type-has-constructors":
					IfTypeHasConstructors(instructionElement);
					break;
				case "if-type-has-methods":
					IfTypeHasMethods(instructionElement);
					break;
				case "if-type-has-overloaded-constructors":
					IfTypeHasOverloadedConstructors(instructionElement);
					break;
				case "if-type-has-properties":
					IfTypeHasProperties(instructionElement);
					break;
				case "if-type-has-remarks":
					IfTypeHasRemarks(instructionElement);
					break;
				case "if-type-implements-interfaces":
					IfTypeImplementsInterfaces(instructionElement);
					break;
				case "if-type-is-abstract":
					IfTypeIsAbstract(instructionElement);
					break;
				case "if-type-is-sealed":
					IfTypeIsSealed(instructionElement);
					break;
				case "implemented-interface-name":
					ImplementedInterfaceName(instructionElement);
					break;
				case "member-declaring-type":
					MemberDeclaringType(instructionElement);
					break;
				case "member-name":
					MemberName(instructionElement);
					break;
				case "member-overloads-summary":
					MemberOverloadsSummary(instructionElement);
					break;
				case "member-summary":
					MemberSummary(instructionElement);
					break;
				case "namespace-name":
					NamespaceName(instructionElement);
					break;
				case "parameter-name":
					ParameterName(instructionElement);
					break;
				case "parameter-type-name":
					ParameterTypeName(instructionElement);
					break;
				case "template":
					TemplateInstruction(instructionElement);
					break;
				case "text":
					Text(instructionElement);
					break;
				case "type-access":
					TypeAccess(instructionElement);
					break;
				case "type-base-type-name":
					TypeBaseTypeName(instructionElement);
					break;
				case "type-constructors-summary":
					TypeConstructorsSummary(instructionElement);
					break;
				case "type-name":
					TypeName(instructionElement);
					break;
				case "type-remarks":
					TypeRemarks(instructionElement);
					break;
				case "type-summary":
					TypeSummary(instructionElement);
					break;
				case "type-type":
					TypeType(instructionElement);
					break;
				default:
					resultWriter.WriteStartElement(instructionElement.LocalName);
					WriteAttributes(instructionElement);
					EvaluateChildren(instructionElement);
					resultWriter.WriteEndElement();
					break;
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
						string newValue;

						switch (nameBuilder.ToString())
						{
							case "assembly-name":
								newValue = AssemblyNameVariable;
								break;
							case "member-link":
								newValue = MemberLinkVariable;
								break;
							case "member-or-overloads-link":
								newValue = MemberOrOverloadsLinkVariable;
								break;
							case "namespace-link":
								newValue = NamespaceLinkVariable;
								break;
							case "type-link":
								newValue = TypeLinkVariable;
								break;
							case "type-constructors-link":
								newValue = TypeConstructorsLinkVariable;
								break;
							case "type-members-link":
								newValue = TypeMembersLinkVariable;
								break;
							default:
								newValue = "{$" + nameBuilder.ToString() + "}";
								break;
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

		#region Helpers

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

		private string AssemblyNameVariable
		{
			get
			{
				return assemblyNavigator.AssemblyName;
			}
		}

		private string MemberLinkVariable
		{
			get
			{
				return assemblyNavigator.NamespaceName + "." + 
					assemblyNavigator.TypeName +  "." + 
					assemblyNavigator.MemberName + ".html";
			}
		}

		private string MemberOrOverloadsLinkVariable
		{
			get
			{
				return assemblyNavigator.NamespaceName + "." + 
					assemblyNavigator.TypeName +  "." + 
					assemblyNavigator.MemberName + ".html";
			}
		}

		private string NamespaceLinkVariable
		{
			get
			{
				return assemblyNavigator.NamespaceName + ".html";
			}
		}

		private string TypeLinkVariable
		{
			get
			{
				return assemblyNavigator.NamespaceName + "." + assemblyNavigator.TypeName + ".html";
			}
		}

		private string TypeConstructorsLinkVariable
		{
			get
			{
				return assemblyNavigator.NamespaceName + "." + assemblyNavigator.TypeName + "-constructors.html";
			}
		}

		private string TypeMembersLinkVariable
		{
			get
			{
				return assemblyNavigator.NamespaceName + "." + assemblyNavigator.TypeName + "-members.html";
			}
		}

		#endregion

		#region Instructions

		private void AssemblyName(XmlElement instructionElement)
		{
			resultWriter.WriteString(AssemblyNameVariable);
		}

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

		private void IfMemberIsInherited(XmlElement instructionElement)
		{
			if (assemblyNavigator.IsMemberInherited)
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfMemberIsOverloaded(XmlElement instructionElement)
		{
			if (assemblyNavigator.IsMemberOverloaded)
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfNamespaceContainsClasses(XmlElement instructionElement)
		{
			if (assemblyNavigator.NamespaceHasClasses)
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfNamespaceContainsDelegates(XmlElement instructionElement)
		{
			if (assemblyNavigator.NamespaceHasDelegates)
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfNamespaceContainsEnumerations(XmlElement instructionElement)
		{
			if (assemblyNavigator.NamespaceHasEnumerations)
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfNamespaceContainsInterfaces(XmlElement instructionElement)
		{
			if (assemblyNavigator.NamespaceHasInterfaces)
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfNamespaceContainsStructures(XmlElement instructionElement)
		{
			if (assemblyNavigator.NamespaceHasStructures)
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfNotLastImplementedInterface(XmlElement instructionElement)
		{
			if (!assemblyNavigator.IsLastImplementedInterface)
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfNotLastParameter(XmlElement instructionElement)
		{
			if (!assemblyNavigator.IsLastParameter)
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfTypeHasBaseType(XmlElement instructionElement)
		{
			if (assemblyNavigator.TypeHasBaseType)
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfTypeHasBaseTypeOrImplementsInterfaces(XmlElement instructionElement)
		{
			if (assemblyNavigator.TypeHasBaseType || assemblyNavigator.TypeImplementsInterfaces)
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfTypeHasConstructors(XmlElement instructionElement)
		{
			string access = instructionElement.GetAttribute("access");

			if (assemblyNavigator.TypeHasConstructors(access))
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfTypeHasMethods(XmlElement instructionElement)
		{
			string access = instructionElement.GetAttribute("access");

			if (assemblyNavigator.TypeHasMethods(access))
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfTypeHasOverloadedConstructors(XmlElement instructionElement)
		{
			if (assemblyNavigator.TypeHasOverloadedConstructors())
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfTypeHasProperties(XmlElement instructionElement)
		{
			string access = instructionElement.GetAttribute("access");

			if (assemblyNavigator.TypeHasProperties(access))
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfTypeHasRemarks(XmlElement instructionElement)
		{
			XmlNode node = assemblyDocumentation.GetTypeRemarks(assemblyNavigator.CurrentType);

			if (node != null)
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfTypeImplementsInterfaces(XmlElement instructionElement)
		{
			if (assemblyNavigator.TypeImplementsInterfaces)
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfTypeIsAbstract(XmlElement instructionElement)
		{
			if (assemblyNavigator.IsTypeAbstract)
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void IfTypeIsSealed(XmlElement instructionElement)
		{
			if (assemblyNavigator.IsTypeSealed)
			{
				EvaluateChildren(instructionElement);
			}
		}

		private void ImplementedInterfaceName(XmlElement instructionElement)
		{
			resultWriter.WriteString(assemblyNavigator.ImplementedInterfaceName);
		}

		private void MemberDeclaringType(XmlElement instructionElement)
		{
			resultWriter.WriteString(assemblyNavigator.MemberDeclaringType);
		}

		private void MemberName(XmlElement instructionElement)
		{
			resultWriter.WriteString(assemblyNavigator.MemberName);
		}

		private void MemberOverloadsSummary(XmlElement instructionElement)
		{
			XmlNode node = assemblyDocumentation.GetMemberOverloadsSummary(assemblyNavigator.CurrentType, membersName);

			if (node != null)
			{
				EvaluateDocumentationChildren(node, false, true);
			}
		}

		private void MemberSummary(XmlElement instructionElement)
		{
			bool stripPara = instructionElement.GetAttribute("strip") == "first";

			XmlNode node = assemblyDocumentation.GetMemberSummary(assemblyNavigator.CurrentMember);

			if (node != null)
			{
				EvaluateDocumentationChildren(node, stripPara, true);
			}
		}

		private void NamespaceName(XmlElement instructionElement)
		{
			resultWriter.WriteString(assemblyNavigator.NamespaceName);
		}

		private void ParameterName(XmlElement instructionElement)
		{
			resultWriter.WriteString(assemblyNavigator.ParameterName);
		}

		private void ParameterTypeName(XmlElement instructionElement)
		{
			resultWriter.WriteString(assemblyNavigator.ParameterTypeName);
		}

		private void Text(XmlElement instructionElement)
		{
			resultWriter.WriteString(instructionElement.InnerText);
		}

		private void TemplateInstruction(XmlElement instructionElement)
		{
			EvaluateChildren(instructionElement);
		}

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

		private void TypeBaseTypeName(XmlElement instructionElement)
		{
			resultWriter.WriteString(assemblyNavigator.CurrentType.BaseType.Name);
		}

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

		private void TypeName(XmlElement instructionElement)
		{
			resultWriter.WriteString(assemblyNavigator.TypeName);
		}

		private void TypeRemarks(XmlElement instructionElement)
		{
			XmlNode node = assemblyDocumentation.GetTypeRemarks(assemblyNavigator.CurrentType);

			if (node != null)
			{
				EvaluateDocumentationChildren(node, false, true);
			}
		}

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
