
namespace Graphviz4Net.WPF
{
    using System.Globalization;
    using System.Windows.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Dot;
    using Graphs;
    using ViewModels;

    /// <summary>
    /// Builds the graph layout out of WPF elements, which are positioned into the WPF canvas.
    /// The WPF elements used are determined by the <see cref="IWPFLayoutElementsFactory"/> abstract factory object.
    /// </summary>
    public class WPFLayoutBuilder : LayoutBuilder<int>
    {
        private readonly Canvas canvas;

        private readonly IWPFLayoutElementsFactory elementsFactory;

        private readonly IDictionary<object, FrameworkElement> verticesElements =
            new Dictionary<object, FrameworkElement>();

        private readonly IDictionary<object, Size> verticesSizes =
            new Dictionary<object, Size>();

        private IGraph graph;

        public WPFLayoutBuilder(Canvas canvas, IWPFLayoutElementsFactory elementsFactory)
        {
            this.canvas = canvas;
            this.elementsFactory = elementsFactory;
        }

        public override void Start(IGraph graph)
        {
            this.graph = graph;
            foreach (var vertex in graph.GetAllVertices())
            {
                var element = this.elementsFactory.CreateVertex(vertex);
                if (element == null)
                {
                    throw new InvalidOperationException(
                        string.Format("WPFLayoutBuilder.GetSize: for vertex {0} WPF control factory returned null.", vertex));
                }

                // add it to canvas:
                this.canvas.Children.Add(element);
                this.verticesElements.Add(vertex, element);

                // measure it:
                element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                double width = element.DesiredSize.Width * (1.0 / 72.0);
                double height = element.DesiredSize.Height * (1.0 / 72.0);
                this.verticesSizes.Add(vertex, new Size(width, height));

                // hide it till we finish:
                element.Visibility = Visibility.Hidden;
            }
        }

        public override Size GetSize(object vertex)
        {
            return this.verticesSizes[vertex];
        }

        public override void Finish()
        {
            foreach (var element in this.verticesElements)
            {
                element.Value.Visibility = Visibility.Visible;
            }
        }

        public override void BuildGraph(double width, double height, IGraph original, DotGraph<int> dotGraph)
        {
            this.canvas.Width = width;
            this.canvas.Height = height;
        }

        public override void BuildVertex(Point position, double width, double height, object originalVertex, DotVertex<int> dotVertex)
        {
            var element = this.verticesElements[originalVertex];
            UpdatePosition(element, position.X, position.Y, width, height, canvas);
            Panel.SetZIndex(element, 1);
        }

        public override void BuildSubGraph(
            double leftX,
            double upperY,
            double rightX,
            double lowerY,
            ISubGraph originalSubGraph,
            DotSubGraph<int> subGraph)
        {
            var element = this.elementsFactory.CreateSubGraphBorder(new BorderViewModel(subGraph.Label, originalSubGraph));
            canvas.Children.Add(element);
            element.Width = rightX - leftX;
            element.Height = upperY - lowerY;
            Point orig = new Point(leftX, upperY);
            Point p = TransformCoordinates(orig, canvas);
            Canvas.SetLeft(element, p.X);
            Canvas.SetTop(element, p.Y);

            Panel.SetZIndex(element, -1);
            this.verticesElements.Add(originalSubGraph, element);
        }

        public override void BuildEdge(Point[] path, IEdge originalEdge, DotEdge<int> edge)
        {            
            var data = Geometry.Parse(TransformCoordinates(path, this.canvas));
            var edgeElement = this.elementsFactory.CreateEdge(new EdgeViewModel(data, originalEdge));
            this.canvas.Children.Add(edgeElement);

            if (edge.DestinationArrowEnd.HasValue)
            {
                CreateArrow(
                    TransformCoordinates(edge.DestinationArrowEnd.Value, canvas),
                    TransformCoordinates(path.Last(), canvas),
                    edge.Destination,
                    this.elementsFactory.CreateEdgeArrow(new EdgeArrowViewModel(originalEdge, originalEdge.DestinationArrow)),
                    this.canvas);
            }

            if (edge.SourceArrowEnd.HasValue)
            {
                CreateArrow(
                    TransformCoordinates(edge.SourceArrowEnd.Value, canvas),
                    TransformCoordinates(path.First(), canvas),
                    edge.Source,
                    this.elementsFactory.CreateEdgeArrow(new EdgeArrowViewModel(originalEdge, originalEdge.SourceArrow)),
                    this.canvas);
            }

            if (edge.LabelPos.HasValue)
            {
                var labelElement = this.elementsFactory.CreateEdgeLabel(new EdgeLabelViewModel(edge.Label, originalEdge));
                this.CreateLabel(edge.LabelPos.Value, labelElement);
            }

            if (edge.SourceArrowLabelPosition.HasValue)
            {
                var viewModel = new EdgeArrowLabelViewModel(edge.SourceArrowLabel, originalEdge, edge.SourceArrow);
                var labelElement = this.elementsFactory.CreateEdgeArrowLabel(viewModel);
                this.CreateLabel(edge.SourceArrowLabelPosition.Value, labelElement);
            }

            if (edge.DestinationArrowLabelPosition.HasValue)
            {
                var viewModel = new EdgeArrowLabelViewModel(edge.DestinationArrowLabel, originalEdge, originalEdge.DestinationArrow);
                var labelElement = this.elementsFactory.CreateEdgeArrowLabel(viewModel);
                this.CreateLabel(edge.DestinationArrowLabelPosition.Value, labelElement);
            }
        }

        private void CreateLabel(Point pos, FrameworkElement label)
        {
            this.canvas.Children.Add(label);
            label.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Point p = TransformCoordinates(pos, canvas);
            Canvas.SetLeft(label, p.X - label.DesiredSize.Width / 2);
            Canvas.SetTop(label, p.Y - label.DesiredSize.Height / 2);
        }

        private static void CreateArrow(
            Point arrowCorner1,
            Point arrowCorner2,
            DotVertex<int> edgeDestination,
            FrameworkElement edgeArrow,
            Canvas canvas)
        {
            Point arrowStart, border;
            if (IsOnBoder(edgeDestination.Position.Value, edgeDestination.Width.Value, edgeDestination.Height.Value, arrowCorner2))
            {
                arrowStart = arrowCorner1;
                border = arrowCorner2;
            }
            else
            {
                arrowStart = arrowCorner2;
                border = arrowCorner1;
            }

            double width = arrowStart.X - border.X;
            double height = border.Y - arrowStart.Y;
            double angle = (180 / Math.PI) * Math.Atan2(width, height);

            var arrowControl = edgeArrow;
            canvas.Children.Add(arrowControl);
            arrowControl.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            double halfWidth = Math.Floor(arrowControl.DesiredSize.Width / 2);

            arrowControl.RenderTransform = new RotateTransform(angle, halfWidth, 0);

            Canvas.SetTop(arrowControl, arrowStart.Y);
            Canvas.SetLeft(arrowControl, arrowStart.X - halfWidth);
        }

        private static bool IsOnBoder(Point leftBottom, double width, double height, Point point)
        {
            return
                (Utils.AreEqual(leftBottom.Y, point.Y) && leftBottom.X <= point.X && point.X <= leftBottom.X + width) ||
                (Utils.AreEqual(leftBottom.X, point.X) && leftBottom.Y <= point.Y && point.Y <= leftBottom.Y + height) ||
                (Utils.AreEqual(leftBottom.Y + height, point.Y) && leftBottom.X <= point.X && point.X <= leftBottom.X + width) ||
                (Utils.AreEqual(leftBottom.X + width, point.X) && leftBottom.Y <= point.Y && point.Y <= leftBottom.Y + height);
        }

        private static double GetSizeInPoints(double value)
        {
            return value * 72;
        }

        private static void UpdatePosition(UIElement element, double x, double y, double width, double height, Canvas canvas)
        {
            double widthInPoints = GetSizeInPoints(width);
            double heightInPoints = GetSizeInPoints(height);
            Canvas.SetLeft(element, x - widthInPoints / 2);
            Canvas.SetTop(element, canvas.Height - (y + heightInPoints / 2));
        }

        private static Point TransformCoordinates(Point p, Canvas canvas)
        {
            return new Point(p.X, canvas.Height - p.Y);
        }

        private static string TransformCoordinates(Point[] data, Canvas canvas)
        {
            var transformed = data.Select(x => TransformCoordinates(x, canvas))
                .Select(p => p.X.ToInvariantString() + "," + p.Y.ToInvariantString());
            return "M" + transformed.First() + "C" +
                   string.Join(" ", transformed.Skip(1).ToArray());
        }
    }
}