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
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

namespace NDoc.Documenter.NativeHtmlHelp2.Engine
{
	/// <summary>
	/// Deleagate for handling file events
	/// </summary>
	public delegate void FileEventHandler( object sender, FileEventArgs args );

	/// <summary>
	/// Summary description for HtmlFactory.
	/// </summary>
	public class HtmlFactory
	{
		private ArrayList documentedNamespaces;
		private string _outputDirectory;
		private ExternalHtmlProvider _htmlProvider;
		private StyleSheetCollection _stylesheets;
		private SdkDocVersion linkToSdkDocVersion = SdkDocVersion.SDK_v1_1;

		private NameMapper mapper;

		/// <summary>
		/// Return the collection of properties that are passed to each stylesheet
		/// </summary>
		public readonly Hashtable Properties = new Hashtable();

		/// <summary>
		/// Constructs a new instance of HtmlFactory
		/// </summary>
		/// <param name="outputDirectory"></param>
		/// <param name="htmlProvider"></param>
		public HtmlFactory( string outputDirectory, ExternalHtmlProvider htmlProvider )
		{			
			documentedNamespaces = new ArrayList();
			_outputDirectory = outputDirectory;

			_htmlProvider = htmlProvider;

			mapper = new NameMapper();
		}


		#region events
		/// <summary>
		/// 
		/// </summary>
		public event FileEventHandler TopicStart;
		/// <summary>
		/// 
		/// </summary>
		protected virtual void OnTopicStart( string fileName )
		{
			if ( TopicStart != null )
				TopicStart( this, new FileEventArgs( fileName ) );
		}
	
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler TopicEnd;
		/// <summary>
		/// 
		/// </summary>
		protected virtual void OnTopicEnd()
		{
			if ( TopicEnd != null )
				TopicEnd( this, EventArgs.Empty );
		}
	

		/// <summary>
		/// 
		/// </summary>
		public event FileEventHandler AddFileToTopic;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="title"></param>
		/// <param name="fileName"></param>
		protected virtual void OnAddFileToTopic( string fileName )
		{
			if ( AddFileToTopic != null )
				AddFileToTopic( this, new FileEventArgs( fileName ) );
		}
		#endregion

		/// <summary>
		/// loads and compiles all the stylesheets
		/// </summary>
		/// <param name="resourceDirectory"></param>
		public void LoadStylesheets( string resourceDirectory )
		{
			_stylesheets = StyleSheetCollection.LoadStyleSheets( resourceDirectory );
		}


		/// <summary>
		/// Generates HTML for the NDoc XML
		/// </summary>
		/// <param name="documentation">NDoc generated xml</param>
		/// <param name="sdkVersion">The SDK version to use</param>
		/// <param name="includeHierarchy">Indicates whether to create a namespace hierarchy page</param>
		public void MakeHtml( XmlNode documentation, SdkDocVersion sdkVersion, bool includeHierarchy )
		{
			linkToSdkDocVersion = sdkVersion;
			mapper.MakeFilenames( documentation );
			MakeHtmlForAssemblies( documentation, includeHierarchy );
		}


		private void MakeHtmlForAssemblies( XmlNode xmlDocumentation, bool includeHierarchy )
		{
			XmlNodeList assemblyNodes = xmlDocumentation.SelectNodes("/ndoc/assembly");
			int[] indexes = SortNodesByAttribute(assemblyNodes, "name");

			System.Collections.Specialized.NameValueCollection namespaceAssemblies
				= new System.Collections.Specialized.NameValueCollection();

			int nNodes = assemblyNodes.Count;
			for (int i = 0; i < nNodes; i++)
			{
				XmlNode assemblyNode = assemblyNodes[indexes[i]];
				if (assemblyNode.ChildNodes.Count > 0)
				{
					string assemblyName = (string)assemblyNode.Attributes["name"].Value;
					GetNamespacesFromAssembly( xmlDocumentation, assemblyName, namespaceAssemblies );
				}
			}

			string [] namespaces = namespaceAssemblies.AllKeys;
			Array.Sort(namespaces);
			nNodes = namespaces.Length;
			for (int i = 0; i < nNodes; i++)
			{
				string namespaceName = namespaces[i];
				foreach ( string assemblyName in namespaceAssemblies.GetValues( namespaceName ) )
					MakeHtmlForNamespace( xmlDocumentation, assemblyName, namespaceName, includeHierarchy );
			}
		}

		private static void GetNamespacesFromAssembly( XmlNode xmlDocumentation, string assemblyName, System.Collections.Specialized.NameValueCollection namespaceAssemblies)
		{
			XmlNodeList namespaceNodes = xmlDocumentation.SelectNodes("/ndoc/assembly[@name=\"" + assemblyName + "\"]/module/namespace");
			foreach (XmlNode namespaceNode in namespaceNodes)
			{
				string namespaceName = (string)namespaceNode.Attributes["name"].Value;
				namespaceAssemblies.Add(namespaceName, assemblyName);
			}
		}


		private void TransformAndWriteResult( XmlNode xmlDocumentation, string transformName, XsltArgumentList arguments, string fileName )
		{
			Trace.WriteLine(fileName);
#if DEBUG
			int start = Environment.TickCount;
#endif

			XslTransform transform = _stylesheets[transformName];

			_htmlProvider.SetFilename( fileName );

			using ( StreamWriter streamWriter =  new StreamWriter(
				File.Open( Path.Combine( _outputDirectory, fileName ), FileMode.Create ), new UTF8Encoding( true ) ) )
			{
				foreach ( DictionaryEntry entry in Properties )				
					arguments.AddParam( entry.Key.ToString(), "", entry.Value );
				
				MsdnXsltUtilities utilities = new MsdnXsltUtilities( mapper.FileNames, mapper.ElemNames, linkToSdkDocVersion );

				arguments.AddParam("ndoc-sdk-doc-base-url", String.Empty, utilities.SdkDocBaseUrl );
				arguments.AddParam("ndoc-sdk-doc-file-ext", String.Empty, utilities.SdkDocExt );

				arguments.AddExtensionObject( "urn:NDocUtil", utilities );
				arguments.AddExtensionObject( "urn:NDocExternalHtml", _htmlProvider );

				transform.Transform( xmlDocumentation, arguments, streamWriter, null );
			}

#if DEBUG
			Trace.WriteLine((Environment.TickCount - start).ToString() + " msec.");
#endif
		}


		private void MakeHtmlForNamespace( XmlNode xmlDocumentation, string assemblyName, string namespaceName, bool includeHierarchy )
		{
			if (documentedNamespaces.Contains(namespaceName)) 
				return;

			documentedNamespaces.Add(namespaceName);

			XsltArgumentList arguments = new XsltArgumentList();
			arguments.AddParam( "namespace", String.Empty, namespaceName );
			arguments.AddParam( "includeHierarchy", String.Empty, includeHierarchy );

			string fileName = NameMapper.GetFilenameForNamespace(namespaceName);
			TransformAndWriteResult( xmlDocumentation, "namespace", arguments, fileName );
			OnTopicStart( fileName );

			arguments = new XsltArgumentList();
			arguments.AddParam( "namespace", String.Empty, namespaceName );

			if ( includeHierarchy )
				TransformAndWriteResult( xmlDocumentation, "namespacehierarchy", arguments, fileName.Insert( fileName.Length - 5, "Hierarchy" ) );

			MakeHtmlForTypes( xmlDocumentation, namespaceName );

			OnTopicEnd();
		}

		private void MakeHtmlForTypes( XmlNode xmlDocumentation, string namespaceName )
		{
			XmlNodeList typeNodes =
				xmlDocumentation.SelectNodes("/ndoc/assembly/module/namespace[@name=\"" + namespaceName + "\"]/*[local-name()!='documentation']");

			int[] indexes = SortNodesByAttribute(typeNodes, "id");
			int nNodes = typeNodes.Count;

			for (int i = 0; i < nNodes; i++)
			{
				XmlNode typeNode = typeNodes[indexes[i]];

				WhichType whichType = NameMapper.GetWhichType(typeNode);

				switch(whichType)
				{
					case WhichType.Class:
						MakeHtmlForInterfaceOrClassOrStructure( xmlDocumentation, whichType, typeNode );
						break;
					case WhichType.Interface:
						MakeHtmlForInterfaceOrClassOrStructure( xmlDocumentation, whichType, typeNode );
						break;
					case WhichType.Structure:
						MakeHtmlForInterfaceOrClassOrStructure( xmlDocumentation, whichType, typeNode );
						break;
					case WhichType.Enumeration:
						MakeHtmlForEnumerationOrDelegate( xmlDocumentation, whichType, typeNode );
						break;
					case WhichType.Delegate:
						MakeHtmlForEnumerationOrDelegate( xmlDocumentation, whichType, typeNode );
						break;
					default:
						break;
				}
			}
		}

		private void MakeHtmlForEnumerationOrDelegate( XmlNode xmlDocumentation, WhichType whichType, XmlNode typeNode )
		{
			string typeName = typeNode.Attributes["name"].Value;
			string typeID = typeNode.Attributes["id"].Value;

			XsltArgumentList arguments = new XsltArgumentList();
			arguments.AddParam( "type-id", String.Empty, typeID );

			string fileName = NameMapper.GetFilenameForType(typeNode);
			TransformAndWriteResult( xmlDocumentation, "type", arguments, fileName );
			OnAddFileToTopic( fileName );
		}

		private void MakeHtmlForInterfaceOrClassOrStructure( XmlNode xmlDocumentation, WhichType whichType, XmlNode typeNode )
		{
			string typeName = typeNode.Attributes["name"].Value;
			string typeID = typeNode.Attributes["id"].Value;

			bool hasMembers = typeNode.SelectNodes("constructor|field|property|method|operator|event").Count > 0;

			XsltArgumentList arguments = new XsltArgumentList();
			arguments.AddParam( "type-id", String.Empty, typeID );

			string fileName = NameMapper.GetFilenameForType(typeNode);
			TransformAndWriteResult( xmlDocumentation, "type", arguments, fileName );
			OnTopicStart( fileName );

			if (hasMembers)
			{
				arguments = new XsltArgumentList();
				arguments.AddParam("id", String.Empty, typeID);

				fileName = NameMapper.GetFilenameForTypeMembers(typeNode);
				TransformAndWriteResult( xmlDocumentation, "allmembers", arguments, fileName );
				OnAddFileToTopic( fileName );

				MakeHtmlForConstructors( xmlDocumentation, whichType, typeNode );
				MakeHtmlForFields( xmlDocumentation, whichType, typeNode);
				MakeHtmlForProperties( xmlDocumentation, whichType, typeNode);
				MakeHtmlForMethods( xmlDocumentation, whichType, typeNode);
				MakeHtmlForOperators( xmlDocumentation, whichType, typeNode);
				MakeHtmlForEvents( xmlDocumentation, whichType, typeNode);
			}

			OnTopicEnd();
		}

		private void MakeHtmlForConstructors( XmlNode xmlDocumentation, WhichType whichType, XmlNode typeNode)
		{
			XmlNodeList   constructorNodes;
			string        constructorID;
			string        typeName;
			string        typeID;
			string        fileName;

			typeName = typeNode.Attributes["name"].Value;
			typeID = typeNode.Attributes["id"].Value;
			constructorNodes = typeNode.SelectNodes("constructor[@contract!='Static']");

			// If the constructor is overloaded then make an overload page.
			if ( constructorNodes.Count > 1 )
			{
				constructorID = constructorNodes[0].Attributes["id"].Value;

				XsltArgumentList arguments = new XsltArgumentList();
				arguments.AddParam( "member-id", String.Empty, constructorID );

				fileName = NameMapper.GetFilenameForConstructors(typeNode);
				TransformAndWriteResult( xmlDocumentation, "memberoverload", arguments, fileName );
				OnTopicStart( fileName );
			}

			foreach ( XmlNode constructorNode in constructorNodes )
			{
				constructorID = constructorNode.Attributes["id"].Value;

				XsltArgumentList arguments = new XsltArgumentList();
				arguments.AddParam( "member-id", String.Empty, constructorID );

				fileName = NameMapper.GetFilenameForConstructor(constructorNode);
				TransformAndWriteResult( xmlDocumentation, "member", arguments, fileName );
				OnAddFileToTopic( fileName );
			}

			if ( constructorNodes.Count > 1 )
				OnTopicEnd();
			
			XmlNode staticConstructorNode = typeNode.SelectSingleNode("constructor[@contract='Static']");
			if ( staticConstructorNode != null )
			{
				constructorID = staticConstructorNode.Attributes["id"].Value;

				XsltArgumentList arguments = new XsltArgumentList();
				arguments.AddParam("member-id", String.Empty, constructorID);

				fileName = NameMapper.GetFilenameForConstructor(staticConstructorNode);
				TransformAndWriteResult( xmlDocumentation, "member", arguments, fileName );
				OnAddFileToTopic( fileName );
			}
		}

		private void MakeHtmlForFields( XmlNode xmlDocumentation, WhichType whichType, XmlNode typeNode )
		{
			XmlNodeList fields = typeNode.SelectNodes("field[not(@declaringType)]");

			if (fields.Count > 0)
			{
				string typeName = typeNode.Attributes["name"].Value;
				string typeID = typeNode.Attributes["id"].Value;

				XsltArgumentList arguments = new XsltArgumentList();
				arguments.AddParam( "id", String.Empty, typeID );
				arguments.AddParam( "member-type", String.Empty, "field" );

				string fileName = NameMapper.GetFilenameForFields( whichType, typeNode );
				TransformAndWriteResult( xmlDocumentation, "individualmembers", arguments, fileName );
				OnTopicStart( fileName );

				int[] indexes = SortNodesByAttribute(fields, "id");

				foreach (int index in indexes)
				{
					XmlNode field = fields[index];

					string fieldName = field.Attributes["name"].Value;
					string fieldID = field.Attributes["id"].Value;

					arguments = new XsltArgumentList();
					arguments.AddParam( "field-id", String.Empty, fieldID );

					fileName = NameMapper.GetFilenameForField(field);
					TransformAndWriteResult( xmlDocumentation, "field", arguments, fileName );
					OnAddFileToTopic( fileName );
				}

				OnTopicEnd();
			}
		}

		private void MakeHtmlForProperties( XmlNode xmlDocumentation, WhichType whichType, XmlNode typeNode )
		{
			XmlNodeList declaredPropertyNodes = typeNode.SelectNodes("property[not(@declaringType)]");

			if ( declaredPropertyNodes.Count > 0 )
			{
				XmlNodeList   propertyNodes;
				XmlNode     propertyNode;
				string        propertyName;
				string        propertyID;
				string        previousPropertyName;
				string        nextPropertyName;
				string        typeName;
				string        typeID;
				int         nNodes;
				int[]       indexes;
				int         i;

				typeName = typeNode.Attributes["name"].Value;
				typeID = typeNode.Attributes["id"].Value;
				propertyNodes = typeNode.SelectNodes( "property[not(@declaringType)]" );
				nNodes = propertyNodes.Count;

				indexes = SortNodesByAttribute( propertyNodes, "id" );

				XsltArgumentList arguments = new XsltArgumentList();
				arguments.AddParam( "id", String.Empty, typeID );
				arguments.AddParam( "member-type", String.Empty, "property" );

				string fileName = NameMapper.GetFilenameForProperties( whichType, typeNode );
				TransformAndWriteResult( xmlDocumentation, "individualmembers", arguments, fileName );
				OnTopicStart( fileName );

				for (i = 0; i < nNodes; i++)
				{
					propertyNode = propertyNodes[indexes[i]];

					propertyName = (string)propertyNode.Attributes["name"].Value;
					propertyID = (string)propertyNode.Attributes["id"].Value;

					previousPropertyName = ((i - 1 < 0) || (propertyNodes[indexes[i - 1]].Attributes.Count == 0))
						? "" : propertyNodes[indexes[i - 1]].Attributes[0].Value;
					nextPropertyName = ((i + 1 == nNodes) || (propertyNodes[indexes[i + 1]].Attributes.Count == 0))
						? "" : propertyNodes[indexes[i + 1]].Attributes[0].Value;

					if ( ( previousPropertyName != propertyName ) && ( nextPropertyName == propertyName ) )
					{
						arguments = new XsltArgumentList();
						arguments.AddParam("member-id", String.Empty, propertyID);

						fileName = NameMapper.GetFilenameForPropertyOverloads( typeNode, propertyNode );
						TransformAndWriteResult( xmlDocumentation, "memberoverload", arguments, fileName );
						OnTopicStart( fileName );
					}

					XsltArgumentList arguments2 = new XsltArgumentList();
					arguments2.AddParam("property-id", String.Empty, propertyID );

					fileName = NameMapper.GetFilenameForProperty(propertyNode);
					TransformAndWriteResult( xmlDocumentation, "property", arguments2, fileName );
					OnAddFileToTopic( fileName );

					if ( ( previousPropertyName == propertyName ) && ( nextPropertyName != propertyName ) )
						OnTopicEnd();

				}

				OnTopicEnd();
			}
		}
		private void MakeHtmlForMethods( XmlNode xmlDocumentation, WhichType whichType, XmlNode typeNode)
		{
			XmlNodeList declaredMethodNodes = typeNode.SelectNodes("method[not(@declaringType)]");

			if (declaredMethodNodes.Count > 0)
			{
				bool bOverloaded = false;

				string typeName = typeNode.Attributes["name"].Value;
				string typeID = typeNode.Attributes["id"].Value;
				XmlNodeList methodNodes = typeNode.SelectNodes("method");
				int nNodes = methodNodes.Count;

				int[] indexes = SortNodesByAttribute(methodNodes, "id");

				XsltArgumentList arguments = new XsltArgumentList();
				arguments.AddParam("id", String.Empty, typeID);
				arguments.AddParam("member-type", String.Empty, "method");

				string fileName = NameMapper.GetFilenameForMethods(whichType, typeNode);
				TransformAndWriteResult( xmlDocumentation, "individualmembers", arguments, fileName);
				OnTopicStart( fileName );

				for (int i = 0; i < nNodes; i++)
				{
					XmlNode methodNode = methodNodes[indexes[i]];
					string methodName = (string)methodNode.Attributes["name"].Value;
					string methodID = (string)methodNode.Attributes["id"].Value;

					if ( MethodHelper.IsMethodFirstOverload( methodNodes, indexes, i ) )
					{
						bOverloaded = true;

						arguments = new XsltArgumentList();
						arguments.AddParam("member-id", String.Empty, methodID);

						fileName = NameMapper.GetFilenameForMethodOverloads( typeNode, methodNode );
						TransformAndWriteResult( xmlDocumentation, "memberoverload", arguments, fileName );
						OnTopicStart( fileName );
					}

					if (methodNode.Attributes["declaringType"] == null)
					{
						XsltArgumentList arguments2 = new XsltArgumentList();
						arguments2.AddParam("member-id", String.Empty, methodID);

						fileName = NameMapper.GetFilenameForMethod(methodNode);
						TransformAndWriteResult( xmlDocumentation, "member", arguments2, fileName);
						OnAddFileToTopic( fileName );
					}

					if ( bOverloaded && MethodHelper.IsMethodLastOverload( methodNodes, indexes, i) )
					{
						bOverloaded = false;
						OnTopicEnd();
					}
				}

				OnTopicEnd();
			}
		}

		private void MakeHtmlForEvents( XmlNode xmlDocumentation, WhichType whichType, XmlNode typeNode)
		{
			XmlNodeList declaredEventNodes = typeNode.SelectNodes("event[not(@declaringType)]");

			if (declaredEventNodes.Count > 0)
			{
				XmlNodeList events = typeNode.SelectNodes("event");

				if (events.Count > 0)
				{
					string typeName = (string)typeNode.Attributes["name"].Value;
					string typeID = (string)typeNode.Attributes["id"].Value;

					XsltArgumentList arguments = new XsltArgumentList();
					arguments.AddParam("id", String.Empty, typeID);
					arguments.AddParam("member-type", String.Empty, "event");

					string fileName = NameMapper.GetFilenameForEvents(whichType, typeNode);
					TransformAndWriteResult( xmlDocumentation, "individualmembers", arguments, fileName);
					OnTopicStart( fileName );

					int[] indexes = SortNodesByAttribute(events, "id");

					foreach (int index in indexes)
					{
						XmlNode eventElement = events[index];

						if (eventElement.Attributes["declaringType"] == null)
						{
							string eventName = (string)eventElement.Attributes["name"].Value;
							string eventID = (string)eventElement.Attributes["id"].Value;

							arguments = new XsltArgumentList();
							arguments.AddParam("event-id", String.Empty, eventID);

							TransformAndWriteResult( xmlDocumentation, "event", arguments, fileName);
							fileName = NameMapper.GetFilenameForEvent(eventElement);
							OnAddFileToTopic( fileName );
						}
					}

					OnTopicEnd();
				}
			}
		}

		private void MakeHtmlForOperators( XmlNode xmlDocumentation, WhichType whichType, XmlNode typeNode)
		{
			XmlNodeList operators = typeNode.SelectNodes("operator");

			if (operators.Count > 0)
			{
				string typeName = (string)typeNode.Attributes["name"].Value;
				string typeID = (string)typeNode.Attributes["id"].Value;
				XmlNodeList opNodes = typeNode.SelectNodes("operator");
				bool bOverloaded = false;

				string title = "Operators";

				if (typeNode.SelectSingleNode("operator[@name = 'op_Explicit' or @name = 'op_Implicit']") != null)
					title += " and Type Conversions";
				
				XsltArgumentList arguments = new XsltArgumentList();
				arguments.AddParam("id", String.Empty, typeID);
				arguments.AddParam("member-type", String.Empty, "operator");

				string fileName = NameMapper.GetFilenameForOperators(whichType, typeNode);
				TransformAndWriteResult( xmlDocumentation, "individualmembers", arguments, fileName);
				OnTopicStart( fileName );

				int[] indexes = SortNodesByAttribute(operators, "id");
				int nNodes = opNodes.Count;

				for (int i = 0; i < nNodes; i++)
				{
					XmlNode operatorNode = operators[indexes[i]];
					string operatorID = operatorNode.Attributes["id"].Value;

					if ( MethodHelper.IsMethodFirstOverload( opNodes, indexes, i ) )
					{
						string opName = (string)operatorNode.Attributes["name"].Value;
						if ( ( opName != "op_Implicit" ) && ( opName != "op_Implicit" ) )
						{
							bOverloaded = true;

							arguments = new XsltArgumentList();
							arguments.AddParam("member-id", String.Empty, operatorID);

							fileName = NameMapper.GetFilenameForOperatorsOverloads(typeNode, operatorNode);
							TransformAndWriteResult( xmlDocumentation, "memberoverload", arguments, fileName );
							OnTopicStart( fileName );
						}
					}


					arguments = new XsltArgumentList();
					arguments.AddParam("member-id", String.Empty, operatorID);

					fileName = NameMapper.GetFilenameForOperator(operatorNode);
					TransformAndWriteResult( xmlDocumentation, "member", arguments, fileName);
					OnAddFileToTopic( fileName );

					if ( bOverloaded && MethodHelper.IsMethodLastOverload( opNodes, indexes, i ) )
					{
						bOverloaded = false;
						OnTopicEnd();
					}
				}

				OnTopicEnd();
			}
		}


		private static int[] SortNodesByAttribute( XmlNodeList nodes, string attributeName )
		{
			string[] names = new string[nodes.Count];
			int[] indexes = new int[nodes.Count];
			int i = 0;

			foreach (XmlNode node in nodes)
			{
				names[i] = (string)node.Attributes[attributeName].Value;
				indexes[i] = i++;
			}

			Array.Sort(names, indexes);

			return indexes;
		}
	}
}