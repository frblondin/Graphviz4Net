﻿
namespace Graphviz4Net.WPF
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using Graphs;
#if !SILVERLIGHT
    using System.Threading.Tasks;
    using System.Windows.Data;
#endif

    [TemplatePart(Name = "PART_Canvas", Type = typeof(Canvas))]
    public class GraphLayout : Control
    {
        private Canvas canvas;

        private IWPFLayoutElementsFactory elementsFactory = new DefaultLayoutElementsFactory();

        public static readonly DependencyProperty GraphProperty =
            DependencyProperty.Register(
                "Graph",
                typeof(IGraph),
                typeof(GraphLayout),
                new PropertyMetadata(OnPropertyGraphChanged));

        public static readonly DependencyProperty UseContentPresenterForAllElementsProperty =
            DependencyProperty.Register(
                "UseContentPresenterForAllElements",
                typeof(bool),
                typeof(GraphLayout),
                new PropertyMetadata(OnUseContentPresenterForAllElementsChanged));

        public static readonly DependencyProperty LogGraphvizOutputProperty =
            DependencyProperty.Register(
                "LogGraphvizOutput",
                typeof(bool),
                typeof(GraphLayout),
                new PropertyMetadata(false));

        public static readonly DependencyProperty EngineProperty =
            DependencyProperty.Register(
                "Engine",
                typeof(LayoutEngine),
                typeof(GraphLayout),
                new PropertyMetadata(OnPropertyGraphChanged));

        public static readonly DependencyProperty DotExecutablePathProperty =
            DependencyProperty.Register(
                "DotExecutablePath",
                typeof(string),
                typeof(GraphLayout),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ErrorTemplateProperty =
           DependencyProperty.Register(
               "ErrorTemplate",
               typeof(DataTemplate),
               typeof(GraphLayout),
               new PropertyMetadata());

        private LayoutDirector director;

        private Exception backgroundException = null;

        private ProgressBar progress = null;

#if !SILVERLIGHT
        static GraphLayout()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                            typeof(GraphLayout),
                            new FrameworkPropertyMetadata(typeof(GraphLayout)));
        }
#endif

        public GraphLayout()
        {
#if SILVERLIGHT
            this.DefaultStyleKey = typeof(GraphLayout);
#endif

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                var graph = new Graph<string, Edge<string>>();
                var a = "A";
                var b = "B";
                var c = "C";
                var d = "D";
                var e = "E";
                var f = "F";
                graph.AddVertex(a);
                graph.AddVertex(b);
                graph.AddVertex(c);
                graph.AddVertex(d);
                graph.AddVertex(e);
                graph.AddVertex(f);
                graph.AddEdge(new Edge<string>(a, b));
                graph.AddEdge(new Edge<string>(a, c));
                graph.AddEdge(new Edge<string>(a, d));
                graph.AddEdge(new Edge<string>(b, c));
                graph.AddEdge(new Edge<string>(b, d));
                graph.AddEdge(new Edge<string>(c, d));
                graph.AddEdge(new Edge<string>(e, f));
                graph.AddEdge(new Edge<string>(e, c));
                graph.AddEdge(new Edge<string>(c, f));
                this.Graph = graph;
                graph.Ratio = 0.5;
            }
        }

        public IGraph Graph
        {
            get { return (IGraph)GetValue(GraphProperty); }
            set { SetValue(GraphProperty, value); }
        }

        public bool LogGraphvizOutput
        {
            get { return (bool)this.GetValue(LogGraphvizOutputProperty); }
            set { this.SetValue(LogGraphvizOutputProperty, value); }
        }

        public LayoutEngine Engine
        {
            get { return (LayoutEngine)this.GetValue(EngineProperty); }
            set { this.SetValue(EngineProperty, value); }
        }

        public event EventHandler<EventArgs> OnLayoutUpdated;

        public bool UseContentPresenterForAllElements
        {
            get { return (bool)this.GetValue(UseContentPresenterForAllElementsProperty); }
            set { this.SetValue(UseContentPresenterForAllElementsProperty, value); }
        }

        public string DotExecutablePath
        {
            get { return (string)this.GetValue(DotExecutablePathProperty); }
            set { this.SetValue(DotExecutablePathProperty, value); }
        }

        public DataTemplate ErrorTemplate
        {
            get { return (DataTemplate)this.GetValue(ErrorTemplateProperty); }
            set { this.SetValue(ErrorTemplateProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.canvas = base.GetTemplateChild("PART_Canvas") as Canvas;
            this.UpdateVerticesLayout();
        }

        private static void OnUseContentPresenterForAllElementsChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs args)
        {
            var graphLayout = (GraphLayout)dependencyObject;
            if ((bool)args.NewValue)
            {
                graphLayout.elementsFactory = new ContentPresenterFactory();
            }
            else
            {
                graphLayout.elementsFactory = new DefaultLayoutElementsFactory();
            }
        }

        private static void OnPropertyGraphChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is GraphLayout)
            {
                var graphLayout = (GraphLayout)obj;
                graphLayout.UpdateVerticesLayout();
                graphLayout.Graph.Changed += graphLayout.GraphChanged;
            }
        }

        private void GraphChanged(object sender, GraphChangedArgs e)
        {
            this.UpdateVerticesLayout();
        }

        private void UpdateVerticesLayout()
        {
            if (this.canvas == null ||
                this.Graph == null ||
                DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            var builder = new WPFLayoutBuilder(this.canvas, this.elementsFactory);
            IDotRunner runner;
#if SILVERLIGHT
            runner = null;
#else
            if (string.IsNullOrWhiteSpace(this.DotExecutablePath) == false)
            {
                runner = new DotExeRunner { DotExecutablePath = this.DotExecutablePath };
            }
            else
            {
                runner = new DotExeRunner();
            }
#endif

            if (this.LogGraphvizOutput)
            {
                runner = new DotRunnerLogDecorator(runner);
            }

            director = LayoutDirector.GetLayoutDirector(builder, dotRunner: runner);

            this.canvas.Children.Clear();
            this.progress = new ProgressBar { MinWidth = 100, MinHeight = 12, IsIndeterminate = true, Margin = new Thickness(50) };
            this.canvas.Children.Add(this.progress);
            try
            {
                director.StartBuilder(this.Graph);
#if SILVERLIGHT
                ThreadPool.QueueUserWorkItem(new LayoutAction(this, director).Run);
#else
                Task.Factory.StartNew(new LayoutAction(this, director).Run);
#endif                    
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void BuildGraph()
        {
            this.canvas.Children.Remove(this.progress);

            if (this.backgroundException != null)
            {
                this.ShowError(this.backgroundException);
                this.backgroundException = null;
                return;
            }

            try
            {
                director.BuildGraph();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }

            if (this.OnLayoutUpdated != null)
            {
                this.OnLayoutUpdated(this, new EventArgs());
            }
        }

        private void ShowError(Exception ex)
        {
            var content = new ContentControl { ContentTemplate = ErrorTemplate, Width = 300 };
            content.Content = new
            {
                ErrorMessage = string.Format(
                    "Graphviz4Net: an exception was thrown during layouting." +
                    "Exception message: {0}.",
                    ex.Message),
                Error = ex
            };
            this.canvas.Children.Clear();
            this.canvas.Children.Add(content);
        }

        private delegate void VoidDelegate();

        private class LayoutAction
        {
            private readonly GraphLayout parent;
            private readonly LayoutDirector director;

            public LayoutAction(GraphLayout parent, LayoutDirector director)
            {
                this.parent = parent;
                this.director = director;
            }

            public void Run()
            {
                var engine = default(LayoutEngine);
                this.parent.Dispatcher.Invoke(new VoidDelegate(() => engine = this.parent.Engine));

                try
                {
                    this.director.RunDot(engine);
                }
                catch (Exception ex)
                {
                    this.parent.backgroundException = ex;
                }

                this.parent.Dispatcher.BeginInvoke(new VoidDelegate(this.parent.BuildGraph));
            }

            public void Run(object stateInfo)
            {
                this.Run();
            }
        }
    }
}
