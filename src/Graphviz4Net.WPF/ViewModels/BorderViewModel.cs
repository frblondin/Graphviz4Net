
using Graphviz4Net.Graphs;

namespace Graphviz4Net.WPF.ViewModels
{
    public class BorderViewModel
    {
        public BorderViewModel(string label, ISubGraph subGraph)
        {
            this.Label = label;
            this.SubGraph = subGraph;
        }

        public string Label { get; private set; }

        public ISubGraph SubGraph { get; private set; }
    }
}
