// MsdnDocumenter.cs - a MSDN-like documenter
// Copyright (C) 2003 Don Kackman
// Parts copyright 2001  Kral Ferch, Jason Diamond
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
using System.Xml;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

namespace NDoc.Documenter.NativeHtmlHelp2.Engine
{
	/// <summary>
	/// The types of code elemements that topics are generated for
	/// </summary>
	public enum WhichType
	{
		/// <summary>
		/// classes
		/// </summary>
		Class,
		/// <summary>
		/// interfaces
		/// </summary>
		Interface,
		/// <summary>
		/// structs
		/// </summary>
		Structure,
		/// <summary>
		/// enumberations
		/// </summary>
		Enumeration,
		/// <summary>
		/// delegates
		/// </summary>
		Delegate,
		/// <summary>
		/// error case
		/// </summary>
		Unknown
	};

	/// <summary>
	/// Provides methods for mapping type name to file names
	/// </summary>
	public class FileNameMapper
	{
		/// <summary>
		/// Creates a new isntance of a FileNameMapper
		/// </summary>
		public FileNameMapper()
		{
		}

		private StringDictionary elemNames;

		/// <summary>
		/// A collection of element names generated for the documentation xml
		/// </summary>
		public StringDictionary ElemNames
		{
			get{ return elemNames; }
		}

		/// <summary>
		/// Creates the element name type mapping
		/// </summary>
		/// <param name="documentation">The NDoc XML documentation summary</param>
		public void MakeElementNames( XmlNode documentation )
		{
			elemNames = new StringDictionary();

			XmlNodeList namespaces = documentation.SelectNodes( "/ndoc/assembly/module/namespace" );
			foreach ( XmlElement namespaceNode in namespaces )
			{
				string namespaceName = namespaceNode.Attributes["name"].Value;
				string namespaceId = "N:" + namespaceName;

				elemNames[namespaceId] = namespaceName;

				XmlNodeList types = namespaceNode.SelectNodes( "*[@id]" );
				foreach (XmlElement typeNode in types)
				{
					string typeId = typeNode.Attributes["id"].Value;
					elemNames[typeId] = typeNode.Attributes["name"].Value;

					XmlNodeList members = typeNode.SelectNodes( "*[@id]" );
					foreach ( XmlElement memberNode in members )
					{
						string id = memberNode.Attributes["id"].Value;
						switch ( memberNode.Name )
						{
							case "constructor":
								elemNames[id] = elemNames[typeId];
								break;
							case "field":
								elemNames[id] = memberNode.Attributes["name"].Value;
								break;
							case "property":
								elemNames[id] = memberNode.Attributes["name"].Value;
								break;
							case "method":
								elemNames[id] = memberNode.Attributes["name"].Value;
								break;
							case "operator":
								elemNames[id] = memberNode.Attributes["name"].Value;
								break;
							case "event":
								elemNames[id] = memberNode.Attributes["name"].Value;
								break;
						}
					}
				}
			}
		}

		/// <summary>
		/// Determines what type of item a node described
		/// </summary>
		/// <param name="typeNode">The documantaion node</param>
		/// <returns>An enumeration for the item type</returns>
		public static WhichType GetWhichType( XmlNode typeNode )
		{
			switch ( typeNode.Name )
			{
				case "class":		return WhichType.Class;
				case "interface":	return WhichType.Interface;
				case "structure":	return WhichType.Structure;
				case "enumeration":	return WhichType.Enumeration;
				case "delegate":	return WhichType.Delegate;
				default:			return WhichType.Unknown;
			}
		}

		/// <summary>
		/// Determines the filename for the namespace hierarchy topic
		/// </summary>
		/// <param name="namespaceName">The namespace</param>
		/// <returns>Topic Filename</returns>
		public static string GetFileNameForNamespaceHierarchy( string namespaceName )
		{
			return namespaceName.Replace( ".", "" ) + "Hierarchy.html";
		}

		/// <summary>
		/// Determines the filename for a namespace topic
		/// </summary>
		/// <param name="namespaceName">The namespace</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForNamespace( string namespaceName )
		{
			return namespaceName.Replace( ".", "" ) + ".html";
		}

		/// <summary>
		/// Determines the filename for a type overview topic
		/// </summary>
		/// <param name="typeID">The id of the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForType( string typeID )
		{
			return BaseNameFromTypeId( typeID ) + "Topic.html";
		}
		/// <summary>
		/// Determines the filename for a type overview topic
		/// </summary>
		/// <param name="typeNode">XmlNode representing the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForType( XmlNode typeNode )
		{
			return GetFilenameForType( typeNode.Attributes["id"].Value );
		}

		/// <summary>
		/// Determines the filename for a type member list topic
		/// </summary>
		/// <param name="typeID">The id of the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForTypeMembers( string typeID )
		{
			return BaseNameFromTypeId( typeID ) + "MembersTopic.html";
		}
		/// <summary>
		/// Determines the filename for a type member list topic
		/// </summary>
		/// <param name="typeNode">XmlNode representing the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForTypeMembers( XmlNode typeNode )
		{
			return GetFilenameForTypeMembers( typeNode.Attributes["id"].Value );
		}

		/// <summary>
		/// Determines the filename for a constructor list topic
		/// </summary>
		/// <param name="typeID">The id of the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForConstructors( string typeID )
		{
			return BaseNameFromTypeId( typeID ) + "ConstructorTopic.html";
		}

		/// <summary>
		/// Determines the filename for a constructor list topic
		/// </summary>
		/// <param name="typeNode">XmlNode representing the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForConstructors( XmlNode typeNode )
		{
			return GetFilenameForConstructors( typeNode.Attributes["id"].Value );
		}

		/// <summary>
		/// Determines the filename for a constructor topic
		/// </summary>
		/// <param name="constructorID">The id of the constructor</param>
		/// <param name="isStatic">Is it a static constructor</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForConstructor( string constructorID, bool isStatic  )
		{
			int dotHash = constructorID.IndexOf(".#"); // constructors could be #ctor or #cctor

			StringBuilder sb = new StringBuilder( BaseNameFromTypeId( constructorID.Substring( 0, dotHash ) ) );
		
			if ( isStatic )
				sb.Append( "Static" );

			return sb.Append( "ctorTopic.html" ).ToString();
		}
		/// <summary>
		/// Determines the filename for a constructor topic
		/// </summary>
		/// <param name="constructorID">The id of the constructor</param>
		/// <param name="isStatic">Is it a static constructor</param>
		/// <param name="overLoad">The oerload of the constructor</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForConstructor( string constructorID, bool isStatic, string overLoad  )
		{
			int dotHash = constructorID.IndexOf(".#"); // constructors could be #ctor or #cctor

			StringBuilder sb = new StringBuilder( BaseNameFromTypeId( constructorID.Substring( 0, dotHash ) ) );
		
			if ( isStatic )
				sb.Append( "Static" );

			return sb.AppendFormat( "ctorTopic{0}.html", overLoad ).ToString();
		}
		/// <summary>
		/// Determines the filename for a constructor topic
		/// </summary>
		/// <param name="constructorNode">The XmlNode representing the constructor</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForConstructor( XmlNode constructorNode )
		{
			if ( constructorNode.Attributes["overload"] != null )	
				return GetFilenameForConstructor( constructorNode.Attributes["id"].Value, 
					constructorNode.Attributes["contract"].Value == "Static",
					constructorNode.Attributes["overload"].Value );
			else
				return GetFilenameForConstructor( constructorNode.Attributes["id"].Value, 
					constructorNode.Attributes["contract"].Value == "Static" );
		}

		/// <summary>
		/// Determines the filename for a type field list topic
		/// </summary>
		/// <param name="typeID">The id of the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForTypeFields( string typeID )
		{
			return BaseNameFromTypeId( typeID ) + "FieldsTopic.html";
		}
		/// <summary>
		/// Determines the filename for a type field list topic
		/// </summary>
		/// <param name="typeNode">XmlNode representing the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForTypeFields( XmlNode typeNode )
		{
			return GetFilenameForTypeFields( typeNode.Attributes["id"].Value );
		}

		/// <summary>
		/// Gets the filename for a particular field topic
		/// </summary>
		/// <param name="fieldID"></param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForField( string fieldID )
		{
			return BaseNameFromMemberId( fieldID ) + "Topic.html";
		}
		/// <summary>
		/// Gets the filename for a particular field topic
		/// </summary>
		/// <param name="fieldNode"></param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForField( XmlNode fieldNode )
		{
			return GetFilenameForField( fieldNode.Attributes["id"].Value );
		}

		/// <summary>
		/// Determines the filename for a type operator list topic
		/// </summary>
		/// <param name="typeID">The id of the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForTypeOperators( string typeID )
		{
			return BaseNameFromTypeId( typeID ) + "OperatorsTopic.html";
		}
		/// <summary>
		/// Determines the filename for a type operator list topic
		/// </summary>
		/// <param name="typeNode">XmlNode representing the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForTypeOperators( XmlNode typeNode )
		{
			return GetFilenameForTypeOperators( typeNode.Attributes["id"].Value );
		}

		/// <summary>
		/// Determines the filename for an operator overloads list topic
		/// </summary>
		/// <param name="typeID">The id of the type</param>
		/// <param name="opName">The name of the operator</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForOperatorsOverloads( string typeID, string opName )
		{
			return BaseNameFromTypeId( typeID ) + opName + "Topic.html";
		}
		/// <summary>
		/// Determines the filename for an operator overloads list topic
		/// </summary>
		/// <param name="typeNode">XmlNode representing the type</param>
		/// <param name="opNode">The XmlNode repsenting the operator</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForOperatorsOverloads( XmlNode typeNode, XmlNode opNode )
		{
			return GetFilenameForOperatorsOverloads( typeNode.Attributes["id"].Value, opNode.Attributes["name"].Value );
		}

		/// <summary>
		/// Gets the filename for a particular operator topic
		/// </summary>
		/// <param name="operatorNode">The XmlNode repsenting the operator</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForOperator( XmlNode operatorNode )
		{
			string operatorID = operatorNode.Attributes["id"].Value;

			StringBuilder sb;

			int leftParenIndex = operatorID.IndexOf( '(' );

			if ( leftParenIndex != -1 )
				sb = new StringBuilder( BaseNameFromMemberId( operatorID.Substring( 0, leftParenIndex ) ) );
			else
				sb = new StringBuilder( BaseNameFromMemberId( operatorID ) );

			if ( operatorNode.Attributes["overload"] != null )
				sb.Append( operatorNode.Attributes["overload"].Value );

			return sb.Append( "Topic.html" ).ToString();
		}
		
		/// <summary>
		/// Determines the filename for a type event list topic
		/// </summary>
		/// <param name="typeID">The id of the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForTypeEvents( string typeID )
		{
			return BaseNameFromTypeId( typeID ) + "EventsTopic.html";
		}
		/// <summary>
		/// Determines the filename for a type event list topic
		/// </summary>
		/// <param name="typeNode">XmlNode representing the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForTypeEvents( XmlNode typeNode )
		{
			return GetFilenameForTypeEvents( typeNode.Attributes["id"].Value );
		}

		/// <summary>
		/// Gets the filename for a particular event topic
		/// </summary>
		/// <param name="eventID"></param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForEvent( string eventID )
		{
			return BaseNameFromMemberId( eventID ) + "Topic.html";
		}
		/// <summary>
		/// Gets the filename for a particular event topic
		/// </summary>
		/// <param name="eventNode"></param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForEvent( XmlNode eventNode )
		{
			return GetFilenameForEvent( eventNode.Attributes["id"].Value );
		}
		
		/// <summary>
		/// Determines the filename for a type property list topic
		/// </summary>
		/// <param name="typeID">The id of the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForTypeProperties( string typeID )
		{
			return BaseNameFromTypeId( typeID ) + "PropertiesTopic.html";
		}
		/// <summary>
		/// Determines the filename for a type property list topic
		/// </summary>
		/// <param name="typeNode">XmlNode representing the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForTypeProperties( XmlNode typeNode )
		{
			return GetFilenameForTypeProperties( typeNode.Attributes["id"].Value );
		}

		/// <summary>
		/// Determines the filename for an property overloads list topic
		/// </summary>
		/// <param name="typeID">The id of the type</param>
		/// <param name="propertyName">The property name</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForPropertyOverloads( string typeID, string propertyName )
		{
			return BaseNameFromTypeId( typeID ) + propertyName + "Topic.html";
		}
		/// <summary>
		/// Determines the filename for an property overloads list topic
		/// </summary>
		/// <param name="typeNode">XmlNode representing the type</param>
		/// <param name="propertyNode"></param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForPropertyOverloads( XmlNode typeNode, XmlNode propertyNode )
		{
			 return GetFilenameForPropertyOverloads( typeNode.Attributes["id"].Value, propertyNode.Attributes["name"].Value );
		}

		/// <summary>
		/// Gets the filename for a particular property topic
		/// </summary>
		/// <param name="propertyNode">XmlNode representing the property</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForProperty( XmlNode propertyNode )
		{
			string propertyID = propertyNode.Attributes["id"].Value;

			StringBuilder sb;

			int leftParenIndex = propertyID.IndexOf( '(' );

			if ( leftParenIndex != -1 )
				sb = new StringBuilder( BaseNameFromMemberId( propertyID.Substring( 0, leftParenIndex ) ) );
			else
				sb = new StringBuilder( BaseNameFromMemberId( propertyID ) );

			if ( propertyNode.Attributes["overload"] != null )
				sb.Append( propertyNode.Attributes["overload"].Value );

			return sb.Append( "Topic.html" ).ToString();
		}

		/// <summary>
		/// Determines the filename for a type method list topic
		/// </summary>
		/// <param name="typeID">The id of the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForTypeMethods( string typeID )
		{
			return BaseNameFromTypeId( typeID ) + "MethodsTopic.html";
		}
		/// <summary>
		/// Determines the filename for a type method list topic
		/// </summary>
		/// <param name="typeNode">XmlNode representing the type</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForTypeMethods( XmlNode typeNode )
		{
			 return GetFilenameForTypeMethods( typeNode.Attributes["id"].Value );
		}

		/// <summary>
		/// Determines the filename for a method voerload list topic
		/// </summary>
		/// <param name="typeID">The id of the type</param>
		/// <param name="methodName">The name of the method</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForMethodOverloads( string typeID, string methodName )
		{
			return BaseNameFromTypeId( typeID ) + methodName + "Topic.html";
		}
		/// <summary>
		/// Determines the filename for a method voerload list topic
		/// </summary>
		/// <param name="typeNode">XmlNode representing the type</param>
		/// <param name="methodNode">XmlNode representing the method</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForMethodOverloads( XmlNode typeNode, XmlNode methodNode )
		{
			return GetFilenameForMethodOverloads( typeNode.Attributes["id"].Value, methodNode.Attributes["name"].Value );
		}

		/// <summary>
		/// Gets the filename for a particular method topic
		/// </summary>
		/// <param name="methodNode">XmlNode representing the method</param>
		/// <returns>Topic Filename</returns>
		public static string GetFilenameForMethod( XmlNode methodNode )
		{
			string methodID = methodNode.Attributes["id"].Value;

			StringBuilder sb;

			int leftParenIndex = methodID.IndexOf( '(' );

			if ( leftParenIndex != -1 )
				sb = new StringBuilder( BaseNameFromMemberId( methodID.Substring( 0, leftParenIndex ) ) );
			else
				sb = new StringBuilder( BaseNameFromMemberId( methodID ) );

			if ( methodNode.Attributes["overload"] != null )
				sb.Append( methodNode.Attributes["overload"].Value );
			
			sb.Replace( "#", "" );

			return sb.Append( "Topic.html" ).ToString();
		}

		/// <summary>
		/// Given a type id (including the T: prefix)
		/// determines the Base name for the topic file
		/// </summary>
		/// <param name="typeID">The ndoc generated id of a type</param>
		/// <returns>Topic's base name</returns>
		private static string BaseNameFromTypeId( string typeID )
		{
			return String.Format( "ndoc{0}Class", typeID.Substring(2).Replace( ".", "" ) );
		}

		/// <summary>
		/// Given a fully qualified member id (including the prefix)
		/// determines the Base name for the topic file
		/// </summary>
		/// <param name="memberID">The ndoc generated id of a type member</param>
		/// <returns>Topic's base name</returns>
		private static string BaseNameFromMemberId( string memberID )
		{
			int lastDot = memberID.LastIndexOf( '.' );
			return BaseNameFromTypeId( memberID.Substring( 0, lastDot ) ) + memberID.Substring( lastDot + 1 );
		}
	}
}