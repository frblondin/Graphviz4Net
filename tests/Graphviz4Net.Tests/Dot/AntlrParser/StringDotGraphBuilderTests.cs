﻿
namespace Graphviz4Net.Tests.Dot.AntlrParser
{
    using System.Linq;
    using Graphviz4Net.Dot.AntlrParser;
    using NUnit.Framework;

    [TestFixture]
    public class StringDotGraphBuilderTests
    {
        [Test]
        public void ParseSimpleGraph()
        {
            var parser = AntlrParserAdapter<string>.GetParser();
            var result = parser.Parse(
                @"
                digraph {
                    node [label=""\N""];
                    graph [bb=""0,0,74,112""];
                    Hello [pos=""37,93"", width=""0.91667"", height=""0.52778""];
                    World [pos=""37,19"", width=""1.0278"", height=""0.52778""];
                    Hello -> World [pos=""e,37,38.249 37,73.943 37,66.149 37,56.954 37,48.338""];
                }");

            Assert.AreEqual(2, result.AllVertices.Count());
            Assert.AreEqual(1, result.VerticesEdges.Count());

            var hello = result.VerticesEdges.First().Source;
            var world = result.VerticesEdges.First().Destination;
            Assert.AreEqual("Hello", hello.Id);
            Assert.AreEqual("World", world.Id);
        }

        [Test]
        public void ParseNodeColonRecordSyntax()
        {
            var parser = AntlrParserAdapter<string>.GetParser();
            var result = parser.Parse(
                @"
                digraph {
                    node [label=""\N"", shape=record];
                    graph [bb=""0,0,264,260""];
                    14 [label=""<left> | 14 | <right>"", pos=""101,241"", rects=""62.5,223,83.5,259 83.5,223,118.5,259 118.5,223,139.5,259"", width=""1.0556"", height=""0.51389""];
                    7 [label=""<left> | 7 | <right>"", pos=""42,167"", rects=""8,149,29,185 29,149,55,185 55,149,76,185"", width=""0.94444"", height=""0.51389""];
                    26 [label=""<left> | 26 | <right>"", pos=""160,167"", rects=""121.5,149,142.5,185 142.5,149,177.5,185 177.5,149,198.5,185"", width=""1.0556"", height=""0.51389""];
                    12 [label=""<left> | 12 | <right>"", pos=""38,93"", rects=""-0.5,75,20.5,111 20.5,75,55.5,111 55.5,75,76.5,111"", width=""1.0556"", height=""0.51389""];
                    20 [label=""<left> | 20 | <right>"", pos=""132,93"", rects=""93.5,75,114.5,111 114.5,75,149.5,111 149.5,75,170.5,111"", width=""1.0556"", height=""0.51389""];
                    31 [label=""<left> | 31 | <right>"", pos=""226,93"", rects=""187.5,75,208.5,111 208.5,75,243.5,111 243.5,75,264.5,111"", width=""1.0556"", height=""0.51389""];
                    17 [label=""<left> | 17 | <right>"", pos=""104,19"", rects=""65.5,1,86.5,37 86.5,1,121.5,37 121.5,1,142.5,37"", width=""1.0556"", height=""0.51389""];
                    14:left -> 7 [pos=""e,58.226,185.32 73,223 73,212.77 68.994,202.66 63.861,193.9""];
                    14:right -> 26 [pos=""e,143.77,185.32 129,223 129,212.77 133.01,202.66 138.14,193.9""];
                    7:right -> 12 [pos=""e,52.328,111.06 66,149 66,138.84 62.264,128.62 57.521,119.76""];
                    26:left -> 20 [pos=""e,132,111.16 132,149 132,140.01 132,130.22 132,121.37""];
                    26:right -> 31 [pos=""e,205.67,111.19 188,149 188,138.14 192.96,127.82 199.29,119.06""];
                    20:left -> 17 [pos=""e,104,37.159 104,75 104,66.015 104,56.218 104,47.367""];
                }");

            Assert.AreEqual(7, result.AllVertices.Count());
            Assert.AreEqual(6, result.VerticesEdges.Count());

            var seventeen = result.Vertices.First(x => x.Id == "17");
            var twenty = result.Vertices.First(x => x.Id == "20");
            Assert.IsTrue(
                result.VerticesEdges.Any(e => e.Source == twenty && e.Destination == seventeen), 
                "Parsed graph does not contain edge 20->17");
        }
    }
}
