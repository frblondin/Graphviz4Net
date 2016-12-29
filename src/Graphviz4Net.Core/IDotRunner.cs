
namespace Graphviz4Net
{
    using System;
    using System.IO;

    public interface IDotRunner
    {
        /// <summary>
        /// Takes an action which should write the input into the DOT input stream and
        /// returns a reader from which the output of the DOT can be read.
        /// </summary>
        /// <param name="writeGraph">An action which writers the input form the DOT into the given stream.</param>
        /// <param name="engine">The engine.</param>
        /// <returns>
        /// A reader to read the output of the DOT.
        /// </returns>
        TextReader RunDot(Action<TextWriter> writeGraph, Graphs.LayoutEngine engine = Graphs.LayoutEngine.Dot);

        /// <summary>
        /// Takes an action which should write the input into the DOT input stream and
        /// returns a reader from which the output of the DOT can be read.
        /// </summary>
        /// <param name="writeGraph">An action which writers the input form the DOT into the given stream.</param>
        /// <param name="format">The format.</param>
        /// <param name="engine">The engine.</param>
        /// <returns>
        /// A reader to read the output of the DOT.
        /// </returns>
        TextReader Run(Action<TextWriter> writeGraph, string format, Graphs.LayoutEngine engine = Graphs.LayoutEngine.Dot);
    }
}