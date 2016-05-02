
namespace Graphviz4Net.Graphs
{
    using System;
    using System.Collections.Generic;

    public interface IGraph
    {
        IEnumerable<IEdge> Edges { get; }

        IEnumerable<object> Vertices { get; }

        IEnumerable<ISubGraph> SubGraphs { get; }

        /// <summary>
        /// This event should be fired when <see cref="Edges"/>, <see cref="Vertices"/> 
        /// or <see cref="SubGraphs"/> is changed.
        /// </summary>
        event EventHandler<GraphChangedArgs> Changed;
    }

    public class GraphChangedArgs : EventArgs
    {        
    }
}
