using System;
using System.Collections;
using System.Reflection;

namespace NDoc.Core
{
	/// <summary>
	///		<para>Enables convenient traversal over the namespaces, types,
	///		and members in an assembly.</para>
	/// </summary>
	public class AssemblyNavigator
	{
		private Assembly assembly;
		private ArrayList namespaceNames;
		private IEnumerator namespaceEnumerator;
		private string currentNamespaceName;
		private ArrayList currentTypes;
		private IEnumerator typeEnumerator;
		private Type currentType;
		private IEnumerator interfaceEnumerator;
		private int interfaceCount;
		private int currentInterfaceIndex;
		private IEnumerator memberEnumerator;

		public AssemblyNavigator(Assembly assembly)
		{
			this.assembly = assembly;
			this.namespaceNames = GetNamespaceNames();
			currentNamespaceName = null;
		}

		public string AssemblyName
		{
			get
			{
				return assembly.GetName().Name;
			}
		}

		private ArrayList GetNamespaceNames()
		{
			ArrayList namespaceNames = new ArrayList();

			Type[] types = assembly.GetTypes();

			foreach (Type type in types)
			{
				if (!namespaceNames.Contains(type.Namespace))
				{
					namespaceNames.Add(type.Namespace);
				}
			}

			namespaceNames.Sort();

			return namespaceNames;
		}

		public bool MoveToNamespace(string namespaceName)
		{
			if (currentNamespaceName != namespaceName && namespaceNames.Contains(namespaceName))
			{
				currentNamespaceName = namespaceName;
				currentTypes = null;
				typeEnumerator = null;
				return true;
			}

			return false;
		}

		public bool MoveToFirstNamespace()
		{
			namespaceEnumerator = null;
			return MoveToNextNamespace();
		}

		public bool MoveToNextNamespace()
		{
			if (namespaceEnumerator == null)
			{
				namespaceEnumerator = namespaceNames.GetEnumerator();
			}

			if (namespaceEnumerator.MoveNext())
			{
				currentNamespaceName = namespaceEnumerator.Current as string;
				return true;
			}

			return false;
		}

		public string NamespaceName
		{
			get
			{
				return currentNamespaceName;
			}
		}

		public string TypeName
		{
			get
			{
				return currentType != null ? GetTypeName(currentType) : null;
			}
		}

		/// <summary>
		///		<para>Compares types by their names.</para>
		/// </summary>
		private class TypeComparer : IComparer
		{
			int IComparer.Compare(object x, object y)
			{
				return String.Compare(GetTypeName((Type)x), GetTypeName((Type)y));
			}
		}

		private bool IsDelegateType(Type type)
		{
			return 
				type.IsClass &&
				(type.BaseType.FullName == "System.Delegate" ||
				type.BaseType.FullName == "System.MulticastDelegate");
		}

		public void GetClasses()
		{
			currentTypes = new ArrayList();

			foreach (Type type in assembly.GetTypes())
			{
				if (type.Namespace == currentNamespaceName && 
					type.IsClass && 
					!IsDelegateType(type) &&
					!type.FullName.StartsWith("<PrivateImplementationDetails>"))
				{
					currentTypes.Add(type);
				}
			}

			currentTypes.Sort(new TypeComparer());

			typeEnumerator = null;
		}

		public void GetInterfaces()
		{
			currentTypes = new ArrayList();

			foreach (Type type in assembly.GetTypes())
			{
				if (type.Namespace == currentNamespaceName && type.IsInterface)
				{
					currentTypes.Add(type);
				}
			}

			currentTypes.Sort(new TypeComparer());

			typeEnumerator = null;
		}

		public void GetStructures()
		{
			currentTypes = new ArrayList();

			foreach (Type type in assembly.GetTypes())
			{
				if (type.Namespace == currentNamespaceName && type.IsValueType && !type.IsEnum)
				{
					currentTypes.Add(type);
				}
			}

			currentTypes.Sort(new TypeComparer());

			typeEnumerator = null;
		}

		public void GetDelegates()
		{
			currentTypes = new ArrayList();

			foreach (Type type in assembly.GetTypes())
			{
				if (type.Namespace == currentNamespaceName && IsDelegateType(type))
				{
					currentTypes.Add(type);
				}
			}

			currentTypes.Sort(new TypeComparer());

			typeEnumerator = null;
		}

		public void GetEnumerations()
		{
			currentTypes = new ArrayList();

			foreach (Type type in assembly.GetTypes())
			{
				if (type.Namespace == currentNamespaceName && type.IsEnum)
				{
					currentTypes.Add(type);
				}
			}

			currentTypes.Sort(new TypeComparer());

			typeEnumerator = null;
		}

		public bool NamespaceHasClasses
		{
			get
			{
				GetClasses();
				return currentTypes.Count > 0;
			}
		}

		public bool NamespaceHasInterfaces
		{
			get
			{
				GetInterfaces();
				return currentTypes.Count > 0;
			}
		}

		public bool NamespaceHasStructures
		{
			get
			{
				GetStructures();
				return currentTypes.Count > 0;
			}
		}

		public bool NamespaceHasDelegates
		{
			get
			{
				GetDelegates();
				return currentTypes.Count > 0;
			}
		}

		public bool NamespaceHasEnumerations
		{
			get
			{
				GetEnumerations();
				return currentTypes.Count > 0;
			}
		}

		public bool MoveToFirstClass()
		{
			GetClasses();
			return MoveToNextType();
		}

		public bool MoveToFirstInterface()
		{
			GetInterfaces();
			return MoveToNextType();
		}

		public bool MoveToFirstStructure()
		{
			GetStructures();
			return MoveToNextType();
		}

		public bool MoveToFirstDelegate()
		{
			GetDelegates();
			return MoveToNextType();
		}

		public bool MoveToFirstEnumeration()
		{
			GetEnumerations();
			return MoveToNextType();
		}

		public bool MoveToNextType()
		{
			if (typeEnumerator == null)
			{
				typeEnumerator = currentTypes.GetEnumerator();
			}

			if (typeEnumerator.MoveNext())
			{
				currentType = typeEnumerator.Current as Type;
				return true;
			}

			return false;
		}

		internal static string GetTypeName(Type type)
		{
			string name = type.FullName;
			
			int indexOfDot = name.LastIndexOf('.');

			if (indexOfDot != -1)
			{
				name = name.Substring(indexOfDot + 1);
			}

			name = name.Replace('+', '.');

			return name;
		}

		private bool NameMatches(Type type, string typeName)
		{
			return GetTypeName(type) == typeName;
		}

		public bool MoveToType(string typeName)
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (type.Namespace == currentNamespaceName && NameMatches(type, typeName))
				{
					currentType = type;
					return true;
				}
			}
			
			return false;
		}

		public Type CurrentType
		{
			get
			{
				return currentType;
			}
		}

		public bool IsClass
		{
			get
			{
				return currentType.IsClass && !IsDelegateType(currentType);
			}
		}

		public bool IsInterface
		{
			get
			{
				return currentType.IsInterface;
			}
		}

		public bool IsStructure
		{
			get
			{
				return currentType.IsValueType && !currentType.IsEnum;
			}
		}

		public bool IsDelegate
		{
			get
			{
				return IsDelegateType(currentType);
			}
		}

		public bool IsEnumeration
		{
			get
			{
				return currentType.IsEnum;
			}
		}

		public bool TypeHasBaseType
		{
			get
			{
				return currentType.BaseType != null && currentType.BaseType.FullName != "System.Object";
			}
		}

		public bool TypeImplementsInterfaces
		{
			get
			{
				return currentType.GetInterfaces().Length > 0;
			}
		}

		public bool MoveToFirstInterfaceImplementedByType()
		{
			Type[] interfaces = currentType.GetInterfaces();
			interfaceEnumerator = interfaces.GetEnumerator();
			interfaceCount = interfaces.Length;
			currentInterfaceIndex = 0;
						
			return MoveToNextInterfaceImplementedByType();
		}

		public bool MoveToNextInterfaceImplementedByType()
		{
			++currentInterfaceIndex;
			return interfaceEnumerator.MoveNext();
		}

		public string ImplementedInterfaceName
		{
			get
			{
				return ((Type)interfaceEnumerator.Current).Name;
			}
		}

		public bool IsLastImplementedInterface
		{
			get
			{
				return currentInterfaceIndex >= interfaceCount;
			}
		}

		public bool IsTypeAbstract
		{
			get
			{
				return currentType.IsAbstract;
			}
		}

		public bool IsTypeSealed
		{
			get
			{
				return currentType.IsSealed;
			}
		}

		private bool AccessMatches(string access, MethodBase method)
		{
			if (access == null || access == String.Empty)
			{
				return true;
			}
			else if ((method.Attributes & MethodAttributes.Public) == MethodAttributes.Public && access == "public")
			{
				return true;
			}

			return false;
		}

		private ArrayList GetConstructors(string access)
		{
			ArrayList constructors = new ArrayList();

			foreach (ConstructorInfo constructor in currentType.GetConstructors())
			{
				if (AccessMatches(access, constructor))
				{
					constructors.Add(constructor);
				}
			}

			return constructors;
		}

		public bool TypeHasConstructors(string access)
		{
			return GetConstructors(access).Count > 0;
		}

		public bool TypeHasOverloadedConstructors()
		{
			return GetConstructors(null).Count > 1;
		}

		public bool MoveToFirstConstructor(string access)
		{
			memberEnumerator = GetConstructors(access).GetEnumerator();
			return MoveToNextMember();
		}

		public bool MoveToNextMember()
		{
			return memberEnumerator.MoveNext();
		}

		public MemberInfo CurrentMember
		{
			get
			{
				return memberEnumerator.Current as MemberInfo;
			}
		}

		public MethodBase CurrentMethod
		{
			get
			{
				return memberEnumerator.Current as MethodBase;
			}
		}
	}
}
