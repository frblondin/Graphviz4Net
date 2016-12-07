using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphviz4Net.Graphs
{
    /// <summary>
    /// Layout algorithm to use.
    /// </summary>
    public enum LayoutEngine
    {
        /// <summary>
        /// "hierarchical" or layered drawings of directed graphs. This is the default tool
        /// to use if edges have directionality.
        /// </summary>
        Dot,
        /// <summary>
        /// "spring model'' layouts.  This is the default tool to use if the graph is not too
        /// large (about 100 nodes) and you don't know anything else about it. Neato attempts
        /// to minimize a global energy function, which is equivalent to statistical
        /// multi-dimensional scaling.
        /// </summary>
        Neato,
        /// <summary>
        /// "spring model'' layouts similar to those of neato, but does this by reducing
        /// forces rather than working with energy.
        /// </summary>
        Fdp,
        /// <summary>
        /// Multiscale version of fdp for the layout of large graphs.
        /// </summary>
        Sfdp,
        /// <summary>
        /// Radial layouts, after Graham Wills 97. Nodes are placed on concentric circles
        /// depending their distance from a given root node.
        /// </summary>
        Twopi,
        /// <summary>
        /// circular layout, after Six and Tollis 99, Kauffman and Wiese 02. This is
        /// suitable for certain diagrams of multiple cyclic structures, such as certain
        /// telecommunications networks.
        /// </summary>
        Circo
    }
}
