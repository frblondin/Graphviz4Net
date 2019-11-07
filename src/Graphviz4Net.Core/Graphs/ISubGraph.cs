
namespace Graphviz4Net.Graphs
{
    using System;
    using System.Collections.Generic;

    public interface ISubGraph
    {
        IEnumerable<object> Vertices { get; }

        string Label { get; }

        /// <summary>
        /// This event should be fired when <see cref="Vertices"/> collection is changed.
        /// </summary>
        event EventHandler<GraphChangedArgs> Changed;
    }

    public interface ISubGraph<out TVertex> : ISubGraph
    {
        new IEnumerable<TVertex> Vertices { get; }
    }
}
