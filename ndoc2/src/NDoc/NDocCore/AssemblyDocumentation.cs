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
using System.Xml;
using System.Xml.XPath;

namespace NDoc.Core
{
	public class AssemblyDocumentation
	{
		private XmlDocument _Document;
		private Hashtable _Hashtable;

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

		public XmlNode GetMemberNode(string memberName)
		{
			return _Hashtable[memberName] as XmlNode;
		}

		public XmlNode GetParamNode(XmlNode memberNode, string paramName)
		{
			string xpath = String.Format("param[@name='{1}']", paramName);
			return memberNode.SelectSingleNode(xpath);
		}

		public XmlNode GetReturnsNode(XmlNode memberNode)
		{
			return memberNode.SelectSingleNode("returns");
		}

		public XmlNode GetValueNode(XmlNode memberNode)
		{
			return memberNode.SelectSingleNode("value");
		}

		public XmlNode GetMemberNode(Type type)
		{
			string memberName = "T:" + type.FullName;
			return GetMemberNode(memberName);
		}

		public XmlNode GetMemberNode(MethodBase method)
		{
			string memberName = "M:";

			if (method.IsConstructor)
			{
				memberName += method.DeclaringType.FullName + ".#ctor";
				return GetMemberNode(memberName);
			}

			return null;
		}

		public XmlNode GetTypeConstructorsSummary(Type type)
		{
			string memberName = "M:" + type.FullName + ".#ctor";

			foreach (DictionaryEntry entry in _Hashtable)
			{
				if (((string)entry.Key).StartsWith(memberName))
				{
					XmlNode memberNode = entry.Value as XmlNode;

					if (memberNode["summary"] != null)
					{
						return memberNode;
					}
				}
			}

			return null;
		}
	}
}
