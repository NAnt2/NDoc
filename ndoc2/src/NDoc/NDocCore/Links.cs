using System;

namespace NDoc.Core
{
	/// <summary>
	///		<para>Contains methods that calculates link URIs.</para>
	/// </summary>
	public class Links
	{
		/// <summary>
		///		<para>Gets the link to the pages listing all namespaces.</para>
		/// </summary>
		/// <returns></returns>
		public static string GetNamespacesLink()
		{
			return "index.html";
		}

		/// <summary>
		///		<para>Gets the link to a namespace.</para>
		/// </summary>
		/// <param name="namespaceName"></param>
		/// <returns></returns>
		public static string GetNamespaceLink(string namespaceName)
		{
			return namespaceName + ".html";
		}

		/// <summary>
		///		<para>Gets the link to a type.</para>
		/// </summary>
		/// <param name="fullName"></param>
		/// <returns></returns>
		public static string GetTypeLink(string fullName)
		{
			return fullName.Replace('+', '.') + ".html";
		}

		/// <summary>
		/// <para>Gets the link to the pages that lists all members for a type.</para>
		/// </summary>
		/// <param name="fullName"></param>
		/// <returns></returns>
		public static string GetTypeMembersLink(string fullName)
		{
			return fullName.Replace('+', '.') + "-members.html";
		}

		/// <summary>
		///		<para>Gets the link to the page that lists all constructors for a type.</para>
		/// </summary>
		/// <param name="fullName"></param>
		/// <returns></returns>
		public static string GetTypeConstructorsLink(string fullName)
		{
			return fullName.Replace('+', '.') + "-constructors.html";
		}

		/// <summary>
		///		<para>Gets the link to the page that lists the overloads for a member.</para>
		/// </summary>
		/// <param name="typeFullName"></param>
		/// <param name="memberName"></param>
		/// <returns></returns>
		public static string GetTypeMemberOverloadsLink(string typeFullName, string memberName)
		{
			return typeFullName.Replace('+', '.') + "." + memberName + ".html";
		}

		/// <summary>
		///		<para>Gets the link to a member's page.</para>
		/// </summary>
		/// <param name="typeFullName"></param>
		/// <param name="memberName"></param>
		/// <param name="overloadID"></param>
		/// <returns></returns>
		public static string GetTypeMemberLink(string typeFullName, string memberName, int overloadID)
		{
			return 
				typeFullName.Replace('+', '.') + 
				"." + 
				memberName + 
				(overloadID == 0 ? "" : "-" + overloadID.ToString()) + 
				".html";
		}
	}
}
