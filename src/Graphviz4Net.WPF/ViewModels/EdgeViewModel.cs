
namespace Graphviz4Net.WPF.ViewModels
{
    using System.Windows.Media;
    using Graphs;

    public class EdgeViewModel
    {
        public EdgeViewModel(Geometry data, IEdge edge)
        {
            this.Data = data;
            this.Edge = edge;
        }

        public Geometry Data { get; private set; }

        public IEdge Edge { get; private set; }
    }
}
