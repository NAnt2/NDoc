Unit Tests?
-----------

It's very important that you write your unit tests BEFORE writing any code.
Kent Beck says "Never write a line of functional code without a broken test 
case." Have you ever heard the old cliche that goes, "If it ain't broke don't
fix it"? How will you know what to fix if you don't have a test telling you
what's broken?

Unit tests are not just about testing. They help you sketch out the
API you're about to implement by letting you see it in action first. They 
also tell you when you're done implementing a specific unit. Your tests are
also the best form of documentation for how your code is supposed to work.
Have you ever seen a comment in code that says one thing right next to code
that does the exact opposite? You can't do that with unit tests. Most 
importantly, they make sure that nobody else (even you) can break your new
code long after you first wrote and forgot about it.

You can read a great article on why you'd want to do all this at
<http://www.stqemagazine.com/featured.asp?id=20>. There's also a wealth of
information on the XP Wiki at <http://c2.com/cgi/wiki?UnitTests>.

Running the Tests
-----------------

To get the tests to run, you need to set up the project to start one of the
NUnit executables. These settings are stored in the .csproj.user file which
do not get checked in to CVS.

Open up the NDocTests project properties by right-clicking on the NDocTests
project in the Solution Explorer. 

Switch to the "Configuration Settings\Debugging" page.

Set "Debug Mode" to "Program".

For "Start Application", browse to the bin directory (the one that's at the
same level as src) and choose "NUnitConsole.exe" or "NUnitGUI.exe".

Set "Command Line Arguments" to "NDoc.Tests.AllTests,NDocTests.dll".

Make sure the NDocTests is the "StartUp Project" and hit F5 or Ctrl+F5 to run.

If you use NUnitConsole, hitting F5 or Ctrl+F5 runs the tests which you can 
see in a console window but disappears immediately after all tests run making
it extremely difficult to see what went wrong. To get the window to stay, I 
create a new external tool.

From the menu bar, choose "Tools\External Tools...".

Set "Title" to something like "NDocTests". (You can put an ampersand in front
of any of the letters to make that letter a keyboard shortcut.)

Set "Command" to "NUnitConsole.exe" (in the bin directory).

Set "Arguments" to "NDoc.Tests.AllTests,NDocTests.dll".

Set "Initial directry" to "$(SolutionDir)\NDocTests\bin\Debug\". 

Make sure that "Close on exit" is NOT checked.

Now you can execute this new tool and the results will be shown in a console
windows until you press a key to dismiss it. Make sure you always remember to
build before executing the tool.

Writing Your Own Tests
----------------------

Before fixing a bug, write a test that demonstrates the bug. You need to
write your test first so that you know what to fix. Do this even if you need
to create a new type or member. The fact that your test isn't compiling is 
the first indication that it's failing.

How do you know how much to test? XP says to "Test everything that 
could possibly break" but be reasonable. See the the Wiki at
<http://c2.com/cgi/wiki?TestEverythingThatCouldPossiblyBreak>.

Once you have your test(s) written, you can start adding and/or editing code.
You know you can stop working when the test that was failing now passes. If,
during the course of fixing your test, you break other tests, make sure that
you fix those tests, too! Nothing can be checked in unless all tests pass.

Debugging the Tests
-------------------

With either NUnitConsole or NUnitGUI, you can simply set breakpoints in any 
of the test methods and hit F5.
