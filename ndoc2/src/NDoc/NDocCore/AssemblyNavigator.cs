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
		private MemberInfo currentMember;
		private IEnumerator parameterEnumerator;
		private int currentParameterIndex;

		/// <summary>
		///		<para>Initializes a new instance of the AssemblyNavigator class.</para>
		/// </summary>
		/// <param name="assembly"></param>
		public AssemblyNavigator(Assembly assembly)
		{
			this.assembly = assembly;
			this.namespaceNames = GetNamespaceNames();
			currentNamespaceName = null;
		}

		/// <summary>
		///		<para>Gets the underlying assembly's friendly name.</para>
		/// </summary>
		public string AssemblyName
		{
			get
			{
				return assembly.GetName().Name;
			}
		}

		private bool NamespaceHasAtLeastOneType(string namespaceName)
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (type.Namespace == namespaceName &&
					!type.FullName.StartsWith("<PrivateImplementationDetails>"))
				{
					return true;
				}
			}

			return false;
		}

		private ArrayList GetNamespaceNames()
		{
			ArrayList namespaceNames = new ArrayList();

			Type[] types = assembly.GetTypes();

			foreach (Type type in types)
			{
				if (!namespaceNames.Contains(type.Namespace) && NamespaceHasAtLeastOneType(type.Namespace))
				{
					namespaceNames.Add(type.Namespace);
				}
			}

			namespaceNames.Sort();

			return namespaceNames;
		}

		/// <summary>
		///		<para>Positions the navigator's namespace cursor on the specified namespace.</para>
		/// </summary>
		/// <param name="namespaceName"></param>
		/// <returns></returns>
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

		/// <summary>
		///		<para>Positions the navigator's namespace cursor on the first
		///		namespace (in alphabetic order) in the assembly.</para>
		/// </summary>
		/// <returns></returns>
		public bool MoveToFirstNamespace()
		{
			namespaceEnumerator = null;
			return MoveToNextNamespace();
		}

		/// <summary>
		///		<para>Positions the navigator's namespace cursor on the next
		///		namespace in the assembly.</para>
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		///		<para>Gets the current namespace's name.</para>
		/// </summary>
		public string NamespaceName
		{
			get
			{
				return currentNamespaceName != null ? currentNamespaceName : "(global)";
			}
		}

		/// <summary>
		///		<para>Gets the current type's name.</para>
		/// </summary>
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

		/// <summary>
		///		<para>Compares members by their names.</para>
		/// </summary>
		private class MemberComparer : IComparer
		{
			int IComparer.Compare(object x, object y)
			{
				return String.Compare(((MemberInfo)x).Name, ((MemberInfo)y).Name);
			}
		}

		private bool IsDelegateType(Type type)
		{
			return
				type.IsClass &&
				(type.BaseType.FullName == "System.Delegate" ||
				type.BaseType.FullName == "System.MulticastDelegate");
		}

		private void GetClasses()
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

		private void GetInterfaces()
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

		private void GetStructures()
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

		private void GetDelegates()
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

		private void GetEnumerations()
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

		/// <summary>
		///		<para>Checks to see if the current namespace has one or more
		///		types.</para>
		/// </summary>
		public bool NamespaceHasTypes
		{
			get
			{
				return NamespaceHasAtLeastOneType(currentNamespaceName);
			}
		}

		/// <summary>
		///		<para>Checks to see if the current namespace has one or more
		///		classes.</para>
		/// </summary>
		public bool NamespaceHasClasses
		{
			get
			{
				GetClasses();
				return currentTypes.Count > 0;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current namespace has one or more
		///		interfaces.</para>
		/// </summary>
		public bool NamespaceHasInterfaces
		{
			get
			{
				GetInterfaces();
				return currentTypes.Count > 0;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current namespace has one or more
		///		structures.</para>
		/// </summary>
		public bool NamespaceHasStructures
		{
			get
			{
				GetStructures();
				return currentTypes.Count > 0;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current namespace has one or more
		///		delegates.</para>
		/// </summary>
		public bool NamespaceHasDelegates
		{
			get
			{
				GetDelegates();
				return currentTypes.Count > 0;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current namespace has one or more
		///		enumerations.</para>
		/// </summary>
		public bool NamespaceHasEnumerations
		{
			get
			{
				GetEnumerations();
				return currentTypes.Count > 0;
			}
		}

		/// <summary>
		///		<para>Positions the navigator's type cursor on the first
		///		class (in alphabetic order) in the current namespace.</para>
		/// </summary>
		/// <returns></returns>
		public bool MoveToFirstClass()
		{
			GetClasses();
			return MoveToNextType();
		}

		/// <summary>
		///		<para>Positions the navigator's type cursor on the first
		///		interface (in alphabetic order) in the current namespace.</para>
		/// </summary>
		/// <returns></returns>
		public bool MoveToFirstInterface()
		{
			GetInterfaces();
			return MoveToNextType();
		}

		/// <summary>
		///		<para>Positions the navigator's type cursor on the first
		///		structure (in alphabetic order) in the current namespace.</para>
		/// </summary>
		/// <returns></returns>
		public bool MoveToFirstStructure()
		{
			GetStructures();
			return MoveToNextType();
		}

		/// <summary>
		///		<para>Positions the navigator's type cursor on the first
		///		delegate (in alphabetic order) in the current namespace.</para>
		/// </summary>
		/// <returns></returns>
		public bool MoveToFirstDelegate()
		{
			GetDelegates();
			return MoveToNextType();
		}

		/// <summary>
		///		<para>Positions the navigator's type cursor on the first
		///		enumeration (in alphabetic order) in the current namespace.</para>
		/// </summary>
		/// <returns></returns>
		public bool MoveToFirstEnumeration()
		{
			GetEnumerations();
			return MoveToNextType();
		}

		/// <summary>
		///		<para>Positions the navigator's type cursor on the next
		///		type in the current namespace.</para>
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		///		<para>Positions the navigator's type cursor on the specified
		///		type in the current namespace.</para>
		/// </summary>
		/// <param name="typeName"></param>
		/// <returns></returns>
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

		/// <summary>
		///		<para>Gets the current type pointed to by the navigator's
		///		type cursor.</para>
		/// </summary>
		public Type CurrentType
		{
			get
			{
				return currentType;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current type is a class.</para>
		/// </summary>
		public bool IsClass
		{
			get
			{
				return currentType.IsClass && !IsDelegateType(currentType);
			}
		}

		/// <summary>
		///		<para>Checks to see if the current type is an interface.</para>
		/// </summary>
		public bool IsInterface
		{
			get
			{
				return currentType.IsInterface;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current type is a structure.</para>
		/// </summary>
		public bool IsStructure
		{
			get
			{
				return currentType.IsValueType && !currentType.IsEnum;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current type is a delegate.</para>
		/// </summary>
		public bool IsDelegate
		{
			get
			{
				return IsDelegateType(currentType);
			}
		}

		/// <summary>
		///		<para>Checks to see if the current type is an enumeration.</para>
		/// </summary>
		public bool IsEnumeration
		{
			get
			{
				return currentType.IsEnum;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current type has a base type
		///		other than System.Object.</para>
		/// </summary>
		public bool TypeHasBaseType
		{
			get
			{
				return currentType.BaseType != null && currentType.BaseType.FullName != "System.Object";
			}
		}

		/// <summary>
		///		<para>Checks to see if the current type implements any interfaces.</para>
		/// </summary>
		public bool TypeImplementsInterfaces
		{
			get
			{
				return currentType.GetInterfaces().Length > 0;
			}
		}

		/// <summary>
		///		<para>Positions the navigator's implemented interface cursor
		///		on the first interface implemented by the current type.</para>
		/// </summary>
		/// <returns></returns>
		public bool MoveToFirstInterfaceImplementedByType()
		{
			Type[] interfaces = currentType.GetInterfaces();
			interfaceEnumerator = interfaces.GetEnumerator();
			interfaceCount = interfaces.Length;
			currentInterfaceIndex = 0;

			return MoveToNextInterfaceImplementedByType();
		}

		/// <summary>
		///		<para>Positions the navigator's implemented interface cursor
		///		on the next interface implemented by the current type.</para>
		/// </summary>
		/// <returns></returns>
		public bool MoveToNextInterfaceImplementedByType()
		{
			++currentInterfaceIndex;
			return interfaceEnumerator.MoveNext();
		}

		/// <summary>
		///		<para>Gets the current implemented interface's name.</para>
		/// </summary>
		public string ImplementedInterfaceName
		{
			get
			{
				return ((Type)interfaceEnumerator.Current).Name;
			}
		}

		/// <summary>
		///		<para>Returns true if the implemented interface cursor is
		///		pointing to the last implemented interface for the current
		///		type.</para>
		/// </summary>
		public bool IsLastImplementedInterface
		{
			get
			{
				return currentInterfaceIndex >= interfaceCount;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current type is abstract.</para>
		/// </summary>
		public bool IsTypeAbstract
		{
			get
			{
				return currentType.IsAbstract;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current type is sealed.</para>
		/// </summary>
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

		private bool AccessMatches(string access, PropertyInfo property)
		{
			if (access == null || access == String.Empty)
			{
				return true;
			}

			MethodInfo method = null;

			if (property.CanRead)
			{
				method = property.GetGetMethod();
			}
			else if (property.CanWrite)
			{
				method = property.GetSetMethod();
			}

			if (method != null)
			{
				return AccessMatches(access, method);
			}

			return false;
		}

		private ArrayList GetConstructors(string access)
		{
			ArrayList constructors = new ArrayList();

			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

			foreach (ConstructorInfo constructor in currentType.GetConstructors(bindingFlags))
			{
				if (AccessMatches(access, constructor))
				{
					constructors.Add(constructor);
				}
			}

			return constructors;
		}

		private bool IsPropertyMethod(MethodInfo method)
		{
			if (method.Name.StartsWith("get_") || method.Name.StartsWith("set_"))
			{
				return true;
			}

			return false;
		}

		private ArrayList GetMethods(string access)
		{
			ArrayList methods = new ArrayList();

			foreach (MethodInfo method in currentType.GetMethods())
			{
				if (!IsPropertyMethod(method) && AccessMatches(access, method))
				{
					methods.Add(method);
				}
			}

			methods.Sort(new MemberComparer());

			return methods;
		}

		private ArrayList GetProperties(string access)
		{
			ArrayList properties = new ArrayList();

			foreach (PropertyInfo property in currentType.GetProperties())
			{
				if (AccessMatches(access, property))
				{
					properties.Add(property);
				}
			}

			properties.Sort(new MemberComparer());

			return properties;
		}

		/// <summary>
		///		<para>Checks to see if the current type has one or more
		///		constructors with the specified access.</para>
		/// </summary>
		/// <param name="access"></param>
		/// <returns></returns>
		public bool TypeHasConstructors(string access)
		{
			return GetConstructors(access).Count > 0;
		}

		/// <summary>
		///		<paa>Checks to see if the current type has more than one
		///		constructor regardless of their access.</paa>
		/// </summary>
		/// <returns></returns>
		public bool TypeHasOverloadedConstructors()
		{
			return GetConstructors(null).Count > 1;
		}

		/// <summary>
		///		<para>Positions the navigator's member cursor on the first
		///		constructor for the current type with the specified access.</para>
		/// </summary>
		/// <param name="access"></param>
		/// <returns></returns>
		public bool MoveToFirstConstructor(string access)
		{
			memberEnumerator = GetConstructors(access).GetEnumerator();
			return MoveToNextMember();
		}

		/// <summary>
		///		<para>Checks to see if the current type has one or more methods.</para>
		/// </summary>
		/// <param name="access"></param>
		/// <returns></returns>
		public bool TypeHasMethods(string access)
		{
			return GetMethods(access).Count > 0;
		}

		/// <summary>
		///		<para>Checks to see if the current type has one or more properties.</para>
		/// </summary>
		/// <param name="access"></param>
		/// <returns></returns>
		public bool TypeHasProperties(string access)
		{
			return GetProperties(access).Count > 0;
		}

		/// <summary>
		///		<para>Positions the navigator's member cursor on the first
		///		method in the current type with the specified access.</para>
		/// </summary>
		/// <param name="access"></param>
		/// <returns></returns>
		public bool MoveToFirstMethod(string access)
		{
			memberEnumerator = GetMethods(access).GetEnumerator();
			return MoveToNextMember();
		}

		/// <summary>
		///		<para>Positions the navigator's memeber cursor on the first
		///		property in the current type with the specified acces.</para>
		/// </summary>
		/// <param name="access"></param>
		/// <returns></returns>
		public bool MoveToFirstProperty(string access)
		{
			memberEnumerator = GetProperties(access).GetEnumerator();
			return MoveToNextMember();
		}

		/// <summary>
		///		<para>Positions the navigator's member cursor on the next
		///		member in the type.</para>
		/// </summary>
		/// <returns></returns>
		public bool MoveToNextMember()
		{
			if (memberEnumerator.MoveNext())
			{
				currentMember = memberEnumerator.Current as MemberInfo;
				return true;
			}

			return false;
		}

		/// <summary>
		///		<para>Positions the navigator's member cursor on the first
		///		overloaded member with the specified name.</para>
		/// </summary>
		/// <param name="membersName"></param>
		/// <returns></returns>
		public bool MoveToFirstOverloadedMember(string membersName)
		{
			memberEnumerator = currentType.GetMember(membersName).GetEnumerator();
			return MoveToNextMember();
		}

		/// <summary>
		///		<para>Gets the current member pointed to by the navigator's
		///		member cursor.</para>
		/// </summary>
		public MemberInfo CurrentMember
		{
			get
			{
				return currentMember;
			}
		}

		/// <summary>
		///		<para>Gets the current member's name.</para>
		/// </summary>
		public string MemberName
		{
			get
			{
				return
					memberEnumerator != null ?
					currentMember.Name :
					null;
			}
		}

		/// <summary>
		///		<para>Positions the navigator's member cursor on the specified member.</para>
		/// </summary>
		/// <param name="memberName"></param>
		/// <returns></returns>
		public bool MoveToMember(string memberName)
		{
			memberEnumerator = currentType.GetMembers().GetEnumerator();

			while (memberEnumerator.MoveNext())
			{
				if (((MemberInfo)memberEnumerator.Current).Name == memberName)
				{
					currentMember = memberEnumerator.Current as MemberInfo;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		///		<para>Checks to see if the current member is inherited from a base type.</para>
		/// </summary>
		public bool IsMemberInherited
		{
			get
			{
				return currentMember.DeclaringType != currentType;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current member is overloaded.</para>
		/// </summary>
		public bool IsMemberOverloaded
		{
			get
			{
				return currentType.GetMember(MemberName).Length > 1;
			}
		}

		/// <summary>
		///		<para>Gets the name of the current member's declaring type.</para>
		/// </summary>
		public string MemberDeclaringType
		{
			get
			{
				return currentMember.DeclaringType.Name;
			}
		}

		private ParameterInfo[] GetParameters(MemberInfo member)
		{
			if (member is MethodBase)
			{
				return ((MethodBase)member).GetParameters();
			}

			return new ParameterInfo[0];
		}

		/// <summary>
		///		<para>Gets the number of parameters in the current member.</para>
		/// </summary>
		public int ParameterCount
		{
			get
			{
				return GetParameters(currentMember).Length;
			}
		}

		/// <summary>
		///		<para>Positions the navigator's parameter cursor on the
		///		current member's first parameter.</para>
		/// </summary>
		/// <returns></returns>
		public bool MoveToFirstParameter()
		{
			parameterEnumerator = GetParameters(currentMember).GetEnumerator();
			currentParameterIndex = 0;
			return MoveToNextParameter();
		}

		/// <summary>
		///		<para>Positions the navigator's parameter cursor on the
		///		current member's next parameter.</para>
		/// </summary>
		/// <returns></returns>
		public bool MoveToNextParameter()
		{
			++currentParameterIndex;
			return parameterEnumerator.MoveNext();
		}

		/// <summary>
		///		<para>Gets the current parameter's type's name.</para>
		/// </summary>
		public string ParameterTypeName
		{
			get
			{
				return ((ParameterInfo)parameterEnumerator.Current).ParameterType.Name;
			}
		}

		/// <summary>
		///		<para>Gets the current parameter's name.</para>
		/// </summary>
		public string ParameterName
		{
			get
			{
				return ((ParameterInfo)parameterEnumerator.Current).Name;
			}
		}

		/// <summary>
		///		<para>Returns true if the parameter cursor is pointing to 
		///		the last parameter for the current member.</para>
		/// </summary>
		public bool IsLastParameter
		{
			get
			{
				return currentParameterIndex == ParameterCount;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current member is a constructor.</para>
		/// </summary>
		public bool IsConstructor
		{
			get
			{
				return currentMember is ConstructorInfo;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current member is an event.</para>
		/// </summary>
		public bool IsEvent
		{
			get
			{
				return currentMember is EventInfo;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current member is a field.</para>
		/// </summary>
		public bool IsField
		{
			get
			{
				return currentMember is FieldInfo;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current member is a method.</para>
		/// </summary>
		public bool IsMethod
		{
			get
			{
				return currentMember is MethodInfo;
			}
		}

		/// <summary>
		///		<para>Checks to see if the current member is a property.</para>
		/// </summary>
		public bool IsProperty
		{
			get
			{
				return currentMember is PropertyInfo;
			}
		}
	}
}
