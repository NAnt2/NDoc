// AssemblyDocumentation.cs
// Copyright (C) 2002  Jason Diamond
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace NDoc.Core
{
	/// <summary>
	///		<para>Encapsulates the csc-generated documentation XML files.</para>
	/// </summary>
	public class AssemblyDocumentation
	{
		private XmlDocument _Document;
		private Hashtable _Hashtable;

		/// <summary>
		///		<para>Initializes a new instance of the AssemblyDocumentation class.</para>
		/// </summary>
		/// <param name="uri"></param>
		public AssemblyDocumentation(string uri)
		{
			_Document = new XmlDocument();
			_Hashtable = new Hashtable();

			if (uri != null)
			{
				_Document.Load(uri);

				XmlNodeList nodes = _Document.SelectNodes("/doc/members/member");

				foreach (XmlElement element in nodes)
				{
					string memberName = element.GetAttribute("name");

					if (memberName != null)
					{
						_Hashtable.Add(memberName, element);
					}
				}
			}
		}

		private XmlNode GetMemberNode(string memberName)
		{
			return _Hashtable[memberName] as XmlNode;
		}

		private XmlNode GetMemberNode(Type type)
		{
			string memberName = "T:" + type.FullName.Replace('+', '.');
			return GetMemberNode(memberName);
		}

		private XmlNode GetMemberNode(MethodBase method)
		{
			string memberName = null;

			if (method.IsConstructor)
			{
				memberName = "M:" + method.DeclaringType.FullName + ".#ctor";
			}
			else
			{
				memberName = "M:" + method.DeclaringType.FullName + "." + method.Name;
			}

			if (memberName != null)
			{
				int i = 0;

				foreach (ParameterInfo parameter in method.GetParameters())
				{
					if (i == 0)
					{
						memberName += "(";
					}
					else
					{
						memberName += ",";
					}

					string parameterName = parameter.ParameterType.FullName;

					memberName += parameterName;

					++i;
				}

				if (i > 0)
				{
					memberName += ")";
				}

				return GetMemberNode(memberName);
			}

			return null;
		}

		private XmlNode GetMemberNode(PropertyInfo property)
		{
			string memberName = null;

			memberName = "P:" + property.DeclaringType.FullName + "." + property.Name;
			return GetMemberNode(memberName);
		}

		/// <summary>
		///		<para>Gets the element containing the specified type's summary comments.</para>
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public XmlNode GetTypeSummary(Type type)
		{
			XmlNode memberNode = GetMemberNode(type);

			if (memberNode != null)
			{
				return memberNode["summary"];
			}

			return null;
		}

		/// <summary>
		///		<para>Gets the element containing the specified type's remarks comments.</para>
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public XmlNode GetTypeRemarks(Type type)
		{
			XmlNode memberNode = GetMemberNode(type);

			if (memberNode != null)
			{
				return memberNode["remarks"];
			}

			return null;
		}

		/// <summary>
		///		<para>Gets the element containing the specified type's constructors summary comments.</para>
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public XmlNode GetTypeConstructorsSummary(Type type)
		{
			string memberName = "M:" + type.FullName + ".#ctor";

			foreach (DictionaryEntry entry in _Hashtable)
			{
				if (((string)entry.Key).StartsWith(memberName))
				{
					XmlNode memberNode = entry.Value as XmlNode;
					XmlNode summaryNode = memberNode["summary"];

					if (summaryNode != null)
					{
						return summaryNode;
					}
				}
			}

			return null;
		}

		/// <summary>
		///		<para>Gets the element containing the specified member's summary comments.</para>
		/// </summary>
		/// <param name="member"></param>
		/// <returns></returns>
		public XmlNode GetMemberSummary(MemberInfo member)
		{
			XmlNode memberNode = null;

			if (member is MethodBase)
			{
				memberNode = GetMemberNode(member as MethodBase);
			}
			else if (member is PropertyInfo)
			{
				memberNode = GetMemberNode(member as PropertyInfo);
			}

			if (memberNode != null)
			{
				return memberNode["summary"];
			}

			return null;
		}

		/// <summary>
		///		<para>Gets the element containing the summary for the specified
		///		overloaded members.</para>
		/// </summary>
		/// <param name="type"></param>
		/// <param name="membersName"></param>
		/// <returns></returns>
		public XmlNode GetMemberOverloadsSummary(Type type, string membersName)
		{
			MemberInfo member = type.GetMember(membersName)[0];
			return GetMemberSummary(member);
		}
	}
}
