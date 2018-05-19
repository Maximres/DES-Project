using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;
using QueueSimulation.BL.Objects;
using System.Diagnostics;
using QueueSimulation.BL.Concrete.Products;
using Microsoft.Msagl.Core.Geometry.Curves;
using System.Drawing.Drawing2D;
using MSAGLPoint = Microsoft.Msagl.Core.Geometry.Point;
using MSAGLColor = Microsoft.Msagl.Drawing.Color;
using System.Collections;
using QueueSimulation.Infrastructure;
using QueueSimulation.Infrastructure.Nodes;
using QueueSimulation.BL.Concrete.Sources;
using QueueSimulation.BL.Concrete.Machines;
using QueueSimulation.BL.Concrete.Conveyors;
using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Concrete;

namespace QueueSimulation
{
    public partial class MainForm : Form
    {

        Stopwatch stopwatch;
        GViewer gViewer;
        static Random random = new Random();
        const float radiusRatio = 0.3f;
        internal event EventHandler<NodeInsertedByUserEventArgs> NodeInsertedByUser = delegate { };
        protected MSAGLPoint m_MouseRightButtonDownPoint;
        protected MSAGLPoint m_MouseLeftButtonDownPoint;
        IndustryFactory<ProductBase> _factory;

        Source<ProductBase> Source;
        Seed<ProductBase> Seed;

        List<ISimulation<ProductBase>> _objectsSequence;
        List<ISimulation<ProductBase>> temp = new List<ISimulation<ProductBase>>();

        static NextNodeId NodeId;

        public MainForm()
        {
            InitializeComponent();
            #region Init
            //create a viewer object 
            gViewer = new GViewer()
            {
                AllowDrop = true,
                ToolBarIsVisible = true,
                Size = this.viewerPanel.Size,
                ContextMenuStrip = contextMenu,
            };
            //create a graph object 
            gViewer.EdgeAdded += GViewer_EdgeAdded;
            gViewer.GiveFeedback += GViewer_GiveFeedback;
            (gViewer as IViewer).MouseDown += MainForm_MouseDown;
            (gViewer as IViewer).MouseUp += MainForm_MouseUp;
            (gViewer as IViewer).MouseMove += MainForm_MouseMove;
            (gViewer as IViewer).ObjectUnderMouseCursorChanged += MainForm_ObjectUnderMouseCursorChanged;
            gViewer.DragEnter += GViewer_DragEnter;
            gViewer.DragDrop += GViewer_DragDrop;

            Microsoft.Msagl.Drawing.Graph graph = new Graph("graph");
            //can generate an event at specific time
            //var tm = new System.Threading.Timer()

            NodeId = NextNodeId.GetInstance();
            _objectsSequence = new List<ISimulation<ProductBase>>();
            _factory = new IndustryFactory<ProductBase>();
            #endregion

            #region Setup

            //create the graph content 

            System.Diagnostics.Debug.WriteLine(gViewer.CurrentScale);

            gViewer.Size = this.viewerPanel.Size;

            //graph.AddEdge("SourceBase", "queue").Attr.AddStyle(Microsoft.Msagl.Drawing.Style.Solid);

            //var c = graph.FindNode("SourceBase");

            //c.LabelText = " ";
            ////c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
            ////c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Brown;


            //graph.AddEdge("queue", "conveyor");
            //graph.AddEdge("conveyor", "delay");
            //graph.AddEdge("delay", "machine");

            //graph.AddEdge("machine", "conveyor3");
            //graph.AddEdge("conveyor2", "delay2");
            //graph.AddEdge("conveyor3", "delay3");
            //graph.AddEdge("delay2", "sink");
            //graph.AddEdge("delay3", "sink");


            InitSequence(graph);

            SetImagesToNodes(graph);

            //bind the graph to the viewer 

            graph.CreateGeometryGraph();
            gViewer.Graph = graph;

            gViewer.Dock = System.Windows.Forms.DockStyle.Fill;

            this.viewerPanel.Controls.Add(gViewer);
            this.propertyGrid1.SelectedObject = graph;
            #endregion

        }

        #region Methods
        private void MainForm_ObjectUnderMouseCursorChanged(object sender, ObjectUnderMouseCursorChangedEventArgs e)
        {

        }

        private void MainForm_MouseMove(object sender, MsaglMouseEventArgs e)
        {
            toolPanelStatus.Text = "X: " + e.X + " " + "Y: " + e.Y;
        }

        private void MainForm_MouseUp(object sender, MsaglMouseEventArgs e)
        {
            m_MouseLeftButtonDownPoint = (gViewer).ScreenToSource(e);
        }

        /// <summary>
        /// Устанавливает картинку, изменяет графическое представление узла.
        /// </summary>
        /// <param name="graph"></param>
        private void SetImagesToNodes(Graph graph)
        {
            foreach (var node in graph.Nodes)
            {
                node.Attr.Shape = Shape.DrawFromGeometry;
                node.DrawNodeDelegate = new DelegateToOverrideNodeRendering(DrawNode);
                node.NodeBoundaryDelegate = new DelegateToSetNodeBoundary(GetNodeBoundary);
                node.LabelText = "";
            }
        }

        private void InitSequence(Graph graph)
        {
            var mintBox = _factory.CreateProduct("Продукт1") as MintBoxProduct;
            Source = new Source<ProductBase>(2, mintBox);
            Source.OnEmpty += Source_OnEmpty;
            _objectsSequence.Add(Source);
            Seed = new Seed<ProductBase>();
            _objectsSequence.Add(Seed);

            Node sourceNode = new Node("Source")
            {
                UserData = Source,
                LabelText = Source.Count.ToString(),

            };

            Node seedNode = new Node("Seed")
            {
                UserData = Seed,
                LabelText = Seed.Capacity.ToString()
            };

            Edge sourceToSeed = new Edge(sourceNode, seedNode, ConnectionToGraph.Connected);
            graph.AddNode(sourceNode);
            graph.AddNode(seedNode);
        }

        private void DeleteEdge(Edge edge, bool registreForUdno = false)
        {
            foreach (IViewerObject item in gViewer.Entities)
            {
                if (item.DrawingObject is Edge drawingEdge && drawingEdge.Equals(edge))
                {
                    gViewer.RemoveEdge((IViewerEdge)item, registreForUdno);
                    break;
                }
            }
        }



        static internal PointF PointF(Microsoft.Msagl.Core.Geometry.Point p) { return new PointF((float)p.X, (float)p.Y); }


        ICurve GetNodeBoundary(Microsoft.Msagl.Drawing.Node node)
        {
            Image image = ImageOfNode(node);
            double width = image.Width;
            double height = image.Height;

            return CurveFactory.CreateRectangleWithRoundedCorners(width, height, width * radiusRatio, height * radiusRatio, new MSAGLPoint());
        }

        /// <summary>
        /// Рисует границу узла
        /// </summary>
        /// <param name="node"></param>
        /// <param name="graphics"></param>
        /// <returns></returns>
        private bool DrawNode(Node node, object graphics)
        {
            Graphics g = (Graphics)graphics;
            //возвращает рисунок
            Image image = ImageOfNode(node);
            //flip the image around its center
            using (System.Drawing.Drawing2D.Matrix m = g.Transform)
            {
                using (System.Drawing.Drawing2D.Matrix saveM = m.Clone())
                {

                    g.SetClip(FillTheGraphicsPath(node.GeometryNode.BoundaryCurve));
                    using (var m2 = new System.Drawing.Drawing2D.Matrix(1, 0, 0, -1, 0, 2 * (float)node.GeometryNode.Center.Y))
                        m.Multiply(m2);

                    g.Transform = m;
                    g.DrawImage(image, new PointF((float)(node.GeometryNode.Center.X - node.GeometryNode.Width / 2),
                        (float)(node.GeometryNode.Center.Y - node.GeometryNode.Height / 2)));
                    g.Transform = saveM;
                    g.ResetClip();
                }
            }

            return true;//returning false would enable the default rendering
        }

        private System.Drawing.Drawing2D.GraphicsPath FillTheGraphicsPath(ICurve boundaryCurve)
        {
            var curve = ((RoundedRect)boundaryCurve).Curve;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            foreach (ICurve seg in curve.Segments)
                AddSegmentToPath(seg, ref path);
            return path;
        }

        private void AddSegmentToPath(ICurve seg, ref GraphicsPath p)
        {
            const float radiansToDegrees = (float)(180.0 / Math.PI);
            if (seg is LineSegment line)
                p.AddLine(PointF(line.Start), PointF(line.End));
            else
            {
                if (seg is CubicBezierSegment cb)
                    p.AddBezier(PointF(cb.B(0)), PointF(cb.B(1)), PointF(cb.B(2)), PointF(cb.B(3)));
                else
                {
                    if (seg is Ellipse ellipse)
                        p.AddArc((float)(ellipse.Center.X - ellipse.AxisA.Length), (float)(ellipse.Center.Y - ellipse.AxisB.Length),
                            (float)(2 * ellipse.AxisA.Length), (float)(2 * ellipse.AxisB.Length), (float)(ellipse.ParStart * radiansToDegrees),
                            (float)((ellipse.ParEnd - ellipse.ParStart) * radiansToDegrees));

                }
            }
        }

        private Image ImageOfNode(Node node)
        {
            //TODO: возвращает рисунок, относящийся к определенному узлу (вершине) по ID
            return MainImageList.Images[(node.UserData as ISimulation<ProductBase>).Id];
        }

        private void TheTimer_Tick(object sender, EventArgs e)
        {
            try
            {

                Simulation();
                RedoLayout();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " ");
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                throw;
            }
        }

        private void Simulation()
        {
            //TODO: start simulation
            foreach (var item in gViewer.Entities)
            {
                if (item.DrawingObject.UserData is ISimulation<ProductBase> obj)
                {
                    obj.Simulate();
                    continue;
                }
                if (item.DrawingObject is Edge edge)
                {
                    edge.LabelText = (edge?.SourceNode?.UserData as ISimulation<ProductBase>)?.Count.ToString() ?? "0";
                }
            }
        }

        internal virtual Node InsertNode(MSAGLPoint center, object simulationObj)
        {
            Node node = null;
            switch (simulationObj)
            {
                case MainConveyor<ProductBase> con:
                    node = new Node(con.Name)
                    {
                        UserData = con
                    };
                    break;
                case FirstMachine<ProductBase> fst:
                    node = new Node(fst.Name)
                    {
                        UserData = fst
                    };
                    break;
                case SecondMachine<ProductBase> snd:
                    node = new Node(snd.Name)
                    {
                        UserData = snd
                    };
                    break;
                case ThirdMachine<ProductBase> thrd:
                    node = new Node(thrd.Name)
                    {
                        UserData = thrd
                    };
                    break;
                case FourthMachine<ProductBase> frth:
                    node = new Node(frth.Name)
                    {
                        UserData = frth
                    };
                    break;
                case MintBoxProduct mint:
                case RedBoxProduct red:
                case OrangeBoxProduct orange:
                default:
                    return null;
            }
            node.Attr.Shape = Shape.DrawFromGeometry;
            node.DrawNodeDelegate = new DelegateToOverrideNodeRendering(DrawNode);
            node.NodeBoundaryDelegate = new DelegateToSetNodeBoundary(GetNodeBoundary);
            node.LabelText = "";

            IViewerNode dNode = gViewer.CreateIViewerNode(node, center, null);
            gViewer.AddNode(dNode, true);

            return node;
        }

        internal virtual void RedoLayout()
        {
            gViewer.NeedToCalculateLayout = false;
            gViewer.Graph = gViewer.Graph;
            gViewer.NeedToCalculateLayout = true;
            gViewer.Graph = gViewer.Graph;
        }
        #endregion

        #region Events


        private void Source_OnEmpty(object sender, EventArgs e)
        {
            TheTimer.Stop();
            MessageBox.Show("Источник пуст!");
        }

        private void GViewer_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
            if (e.Data.GetData(typeof(ListViewItem)) is ListViewItem viewItem)
            {
                var mPoint = (gViewer).ScreenToSource(e.X, e.Y);
                var mPoint2 = m_MouseLeftButtonDownPoint;
                MSAGLPoint point = mPoint;

                var obj = _factory.CreateObject(viewItem.Text);
                switch (obj)
                {
                    case MainConveyor<ProductBase> con:
                        var nd = InsertNode(point, con);
                        break;
                    case FirstMachine<ProductBase> fst:
                        InsertNode(point, fst);
                        break;
                    case SecondMachine<ProductBase> snd:
                        InsertNode(point, snd);
                        break;
                    case ThirdMachine<ProductBase> thrd:
                        InsertNode(point, thrd);
                        break;
                    case FourthMachine<ProductBase> frth:
                        InsertNode(point, frth);
                        break;
                    case MintBoxProduct mint:
                        Source.Id = mint.Id;
                        break;
                    case RedBoxProduct red:
                        Source.Id = red.Id;
                        break;
                    case OrangeBoxProduct orange:
                        Source.Id = orange.Id;
                        break;
                    default:
                        return;
                }
                RedoLayout();
            }
            else
            {
                return;
            }
        }

        private void GViewer_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(ListViewItem)) is ListViewItem viewItem)
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                return;
            }

        }

        private void MainForm_MouseDown(object sender, MsaglMouseEventArgs e)
        {
            if (e.RightButtonIsPressed && e.LeftButtonIsPressed && !e.Handled)
            {
                m_MouseRightButtonDownPoint = (gViewer).ScreenToSource(e);
            }
        }

        private void GViewer_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            var send = sender;
            var eee = e;
        }

        /// <summary>
        /// Удаляет добавленную грань, если вершина уже содержит одну.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GViewer_EdgeAdded(object sender, EventArgs e)
        {
            if (sender is Edge edge)
            {
                if (edge.SourceNode.InEdges is IEnumerable<Edge> inputEdges && inputEdges.Count() > 1)
                {
                    DeleteEdge(edge);
                }
                if (edge.SourceNode.OutEdges is IEnumerable<Edge> outputEdges && outputEdges.Count() > 1)
                {
                    DeleteEdge(edge);
                }
                var inEdge = edge.SourceNode.InEdges.FirstOrDefault();
                var outEdge = edge.SourceNode.OutEdges.FirstOrDefault();
                var targer = edge.TargetNode;
                if (inEdge is Edge edge1 && outEdge is Edge edge2)
                {
                    if (edge1.SourceNode.Equals(targer))
                    {
                        DeleteEdge(edge);
                    }
                }
            }
        }



        private void MainForm_Load(object sender, EventArgs e)
        {
            MintBoxProduct mintBox = new MintBoxProduct(1, "mintbox", new Size(1, 2));
            //Source<ProductBase> Source = new Source<ProductBase>(50, mintBox);
            //MainConveyor<ProductBase> mainConveyor = new MainConveyor<ProductBase>()
            //FirstMachine<ProductBase> firstMachine = new FirstMachine<ProductBase>(Source);

            Source<ProductBase> source = new Source<ProductBase>(100, mintBox);
            MainConveyor<ProductBase> mainConveyor = new MainConveyor<ProductBase>();
            mainConveyor.AddNode(source);
            FirstMachine<ProductBase> firstMachine = new FirstMachine<ProductBase>();
            firstMachine.AddNode(mainConveyor);
            Seed<ProductBase> seed = new Seed<ProductBase>();
            seed.AddNode(firstMachine);
            source.Simulate();
            mainConveyor.Simulate();
            firstMachine.Simulate();
            seed.Simulate();
        }

        //TODO: реализовать отображение свойств
        private void ListView_SelectedIndexChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            //var selected = e.IsSelected;
            //var ind = e.ItemIndex;
            //var gg = e.Item;
            ListViewItem item = null;
            var listView = sender as ListView;
            if (listView == null)
            {
                return;
            }
            if (listView.SelectedItems.Count > 0)
            {
                item = listView.SelectedItems[0];
            }

            if (item != null)
            {
                //show properies
            }
        }

        /// <summary>
        /// Рекурсивно возвращает последовательность элементов в очереди в обратном порядке.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="simulations"></param>
        private void GetSequence(Node node, ref List<ISimulation<ProductBase>> simulations)
        {
            if (node.OutEdges.FirstOrDefault() == null)
            {
                simulations.Add(node.UserData as ISimulation<ProductBase>);
                return;
            }
            else
            {
                GetSequence(node.OutEdges.First().TargetNode, ref simulations);
                simulations.Add(node.UserData as ISimulation<ProductBase>);
            }
          
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var source = gViewer.Entities.Select(s => s.DrawingObject).FirstOrDefault(s => s.UserData is Source<ProductBase>) as Node;
            var seed = gViewer.Entities.Select(s => s.DrawingObject).FirstOrDefault(s => s.UserData is Seed<ProductBase>);
            _objectsSequence.Clear();
            GetSequence(source, ref _objectsSequence);
            _objectsSequence.Reverse();
            TheTimer.Enabled = true;
            TheTimer.Start();

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            TheTimer.Enabled = false;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            TheTimer.Enabled = false;
            TheTimer.Stop();
        }

        

        private void contextItemDeleteSelectedElements_Click(object sender, EventArgs e)
        {
            var al = new ArrayList();
            foreach (IViewerObject ob in gViewer.Entities)
                if (ob.MarkedForDragging)
                    al.Add(ob);
            foreach (IViewerObject ob in al)
            {
                if (ob.DrawingObject is IViewerEdge edge)
                    gViewer.RemoveEdge(edge, true);
                else
                {
                    if (ob is IViewerNode node && node.Node.Id != "Source" && node.Node.Id != "Seed")
                    {
                        gViewer.RemoveNode(node, true);
                    }
                }
            }
        }

        private void contextItemDeleteElement_Click(object sender, EventArgs e)
        {
            var selectedObj = gViewer.SelectedObject as Node;
            if (selectedObj == null)
            {
                return;
            }

            foreach (IViewerNode elem in gViewer.Entities.Where(obj => obj.DrawingObject is Node))
            {
                if (elem.Node.Equals(selectedObj))
                {
                    if (elem.Node.Id != "Source" && elem.Node.Id != "Seed")
                    {
                        gViewer.RemoveNode(elem, true);
                        return;
                    }
                }
            }
        }

        //TODO: Добавляет узлей к графу и обновляет поле.
        private void contextItemConnectElements_Click(object sender, EventArgs e)
        {
            var node = InsertNode(m_MouseRightButtonDownPoint, "15");
            RedoLayout();
        }

        internal virtual Node InsertNode(MSAGLPoint center, string id)
        {
            var node = new Node(id);
            node.Label.Text = "default";
            node.Attr.FillColor = MSAGLColor.Black;
            node.Label.FontColor = MSAGLColor.White;
            node.Label.FontSize = 10;
            node.Attr.Shape = Shape.Circle;
            string s = "User Data object";
            node.UserData = s;
            IViewerNode dNode = gViewer.CreateIViewerNode(node, center, null);
            gViewer.AddNode(dNode, true);

            NodeInsertedByUser(this, new NodeInsertedByUserEventArgs(node));
            return node;
        }

        private void listModelObjects_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listModelObjects.DoDragDrop(e.Item, DragDropEffects.All);
        }

        #endregion
    }

}
