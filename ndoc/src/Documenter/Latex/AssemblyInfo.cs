using System.Reflection;

[assembly: AssemblyTitle("LaTeX documenter")]
[assembly: AssemblyDescription("LaTeX documenter implementation for the NDoc code documentation generator.")]

#if (!DEBUG)
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("NDoc.snk")]
[assembly: AssemblyKeyName("")]
#endif
