Testing NDocConsole
-------------------

I've added a reference from NDocConsole to NDocTest to make it easy to
generate documentation on that assembly.

In NDocConsole's Debugging property page, set the "Command Line Arguments" 
to this:

-d ..\..\..\..\..\examples\NDocTest NDocTest.dll,NDocTest.xml

Then you can hit F5 (when NDocConsole is your StartUp Project) and check the
output in the examples\NDocTest directory.

Don't add anything to the NDocTest project unless you add a unit test for
whatever you want to check in the NDocTests project.
