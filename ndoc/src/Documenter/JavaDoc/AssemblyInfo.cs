using System;
using System.Reflection;

[assembly: CLSCompliantAttribute(true)]
[assembly: AssemblyTitle("NDoc JavaDoc Documenter")]
[assembly: AssemblyDescription("JavaDoc documenter for the NDoc code documentation generator.")]

#if (!DEBUG)
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("NDoc.snk")]
[assembly: AssemblyKeyName("")]
#endif
