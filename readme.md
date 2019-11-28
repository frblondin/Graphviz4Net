[![NuGet Badge](https://buildstats.info/nuget/GraphViz4Net)](https://www.nuget.org/packages/GraphViz4Net/)
[![NuGet Badge](https://buildstats.info/nuget/GraphViz4Net.WPF2)](https://www.nuget.org/packages/GraphViz4Net.WPF2/)
[![Build Status](https://dev.azure.com/thomas0449/GitHub/_apis/build/status/frblondin.GraphViz4Net?branchName=master)](https://dev.azure.com/thomas0449/GitHub/_build/latest?definitionId=1&branchName=master)

This is a fork of Graphviz4net.

Following things are different from Codeplex:
* Build with .Net 4.5 & Standard
* Published NuGet package:
  * https://www.nuget.org/packages/GraphViz4Net/
  * https://www.nuget.org/packages/GraphViz4Net.WPF2/
* Silverlight no longer supported

Below, original readme
-------------------------------------------

This is the development distribution of Graphviz4Net it contains source codes, 
documentation and tools needed to build Graphviz4Net from sources.

-------------------------------------------
What is Graphviz4Net?

Graphviz4Net, at the moment, provides WPF control that is capable of 
generating "nice looking" graph layouts with sub-graphs, curved edges 
with arrows, edges between sub-graphs and more. 
Nodes, edges and all other elements in the graph are fully customizable and 
can contain any other WPF controls (e.g., click-able buttons). 
Besides this WPF control, Graphviz4Net also provides .NET API for 
generating input and consuming the output of the Graphviz command line tool. 
	
Binary package can be downloaded from http://graphviz4net.codeplex.com/
A hard requirement is an installation of the Graphviz (http://www.graphviz.org/) 
for both development and usage of Graphviz4Net.


-------------------------------------------
The directory layout of the development distribution:
top level
  |
  +-- doc (supposed to contain Graphviz4Net documentation and also documentation for tools used in it, especially DOT)
  |
  +-- dot (playground for testing dot, examining its output etc.)
  |
  +-- lib (third party libraries binaries: antlr, wpf extensions, nunit)
  |
  +-- src (source codes, contains also a Visual Studio 2010 solution)
  |    |
  |    +-- Graphviz4Net.Core (The core of Graphviz4Net - running DOT and parsing its output)
  |    |
  |    +-- Graphviz4Net.WPF (The WPF layout control)
  |    |
  |    +-- Graphviz4Net.WPF.Example (An example of Graphviz4Net WPF control usage)
  |    |
  |    +-- Graphviz4Net.Tests (NUnit tests for Graphviz4Net, but in the future tests for all project should be here in a single project)
  |
  +-- tools (thrid party tools needed to develop Graphviz4Net: antlr and nunit runner)

-------------------------------------------
There is an example WPF application and usage of DOT parser 
can be seen in the parser unit tests.

-------------------------------------------
I would like to thank the authors of GraphViz for their effort in 
this project and for making it open-source.

Web: http://graphviz4net.codeplex.com/
Licence: New BSD
Author: Steve Sindelar
Contact: me@stevesindelar.cz

