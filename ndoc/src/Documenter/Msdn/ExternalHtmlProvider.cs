using System;

namespace NDoc.Documenter.Msdn
{
	/// <summary>
	/// Used as an extension object to the xslt processor to allow
	/// retrieving user-provided raw html.
	/// </summary>
	public class ExternalHtmlProvider
	{
		/// <summary>
		/// Contructor.
		/// </summary>
		/// <param name="config">MsdnDocumenterConfig from which the property values can be retrieved.</param>
		public ExternalHtmlProvider(MsdnDocumenterConfig config)
		{
			_config = config;
		}

		/// <summary>
		/// Retrieves user-provided raw html to use as page headers.
		/// </summary>
		/// <param name="topicTitle">The title of the current topic.</param>
		/// <returns></returns>
		public string GetHeaderHtml(string topicTitle)
		{
			string headerHtml = _config.HeaderHtml;

			if (headerHtml == null)
				return string.Empty;

			headerHtml = headerHtml.Replace("%TOPIC-TITLE%", topicTitle);

			return headerHtml;
		}

		/// <summary>
		/// Retrieves user-provided raw html to use as page footers.
		/// </summary>
		/// <param name="assemblyName">The name of the assembly for the current topic.</param>
		/// <param name="assemblyVersion">The version of the assembly for the current topic.</param>
		/// <param name="topicTitle">The title of the current topic.</param>
		/// <returns></returns>
		public string GetFooterHtml(string assemblyName, string assemblyVersion, string topicTitle)
		{
			string footerHtml = _config.FooterHtml;

			if (footerHtml == null)
				return string.Empty;

			footerHtml = footerHtml.Replace("%ASSEMBLY-NAME%", assemblyName);
			footerHtml = footerHtml.Replace("%ASSEMBLY-VERSION%", assemblyVersion);
			footerHtml = footerHtml.Replace("%TOPIC-TITLE%", topicTitle);

			return footerHtml;
		}

		private MsdnDocumenterConfig _config;
	}
}
