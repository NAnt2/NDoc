using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace NDoc.Gui
{
	/// <summary>
	/// Configurable application settings
	/// </summary>
	public class NDocOptions : ICloneable
	{
		/// <summary>
		/// Creates a new instance of the NDocOptions class
		/// </summary>
		public NDocOptions()
		{

		}

		private bool _ShowProgressOnBuild = false;

		/// <summary>
		/// Get/Set the ShowProgressOnBuild property
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[Description("If true, the build progress trace window will automatically be shown whenever a build is started.")]
		[Category("User Specific Settings")]
		public bool ShowProgressOnBuild
		{
			get{ return _ShowProgressOnBuild; }
			set{ _ShowProgressOnBuild = value; }
		}


		private bool _LoadLastProjectOnStart = true;

		/// <summary>
		/// Get/Set the LoadLastProjectOnStart property
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[Description("If true, NDoc will open the last loaded project when it starts.")]
		[Category("User Specific Settings")]
		public bool LoadLastProjectOnStart
		{
			get{ return _LoadLastProjectOnStart; }
			set{ _LoadLastProjectOnStart = value; }
		}


		private string _HtmlHelpWorkshopLocation = string.Empty;
		
		/// <summary>
		/// Get/Set the HtmlHelpWorkshopLocation property
		/// </summary>
		[Browsable(true)]
		[DefaultValue("")]
		[Description("The path to the html help workshop, where the HHC.EXE compiler is located. Only set this value if the MSDN documenter cannot find the html help compiler.")]
		[Category("Machine Specific Settings")]
#if !MONO //System.Windows.Forms.Design.FolderNameEditor is not implemented in mono 0.28
		[Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
#endif
		public string HtmlHelpWorkshopLocation
		{
			get{ return _HtmlHelpWorkshopLocation; }
			set{ _HtmlHelpWorkshopLocation = value; }
		}

		/// <summary>
		/// Create a clone of this object
		/// </summary>
		/// <returns>The clone</returns>
		public object Clone()
		{
			return base.MemberwiseClone();
		}
	}
}