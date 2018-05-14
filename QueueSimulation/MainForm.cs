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

        static GetNextNodeId NodeId;

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

            gViewer.DragEnter += GViewer_DragEnter;
            gViewer.DragDrop += GViewer_DragDrop;

            Microsoft.Msagl.Drawing.Graph graph = new Graph("graph");
            //can generate an event at specific time
            //var tm = new System.Threading.Timer()

            NodeId = GetNextNodeId.GetInstance();
            #endregion

            #region Setup
            #endregion

            //create the graph content 

            System.Diagnostics.Debug.WriteLine(gViewer.CurrentScale);

            gViewer.Size = this.viewerPanel.Size;

            graph.AddEdge("Source", "queue").Attr.AddStyle(Microsoft.Msagl.Drawing.Style.Solid);

            var c = graph.FindNode("Source");

            c.LabelText = " ";
            //c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
            //c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Brown;


            graph.AddEdge("queue", "conveyor");
            graph.AddEdge("conveyor", "delay");
            graph.AddEdge("delay", "machine");

            graph.AddEdge("machine", "conveyor3");
            graph.AddEdge("conveyor2", "delay2");
            graph.AddEdge("conveyor3", "delay3");
            graph.AddEdge("delay2", "sink");
            graph.AddEdge("delay3", "sink");

            foreach (var node in graph.Nodes)
            {
                node.Attr.Shape = Shape.DrawFromGeometry;
                node.DrawNodeDelegate = new DelegateToOverrideNodeRendering(DrawNode);
                node.NodeBoundaryDelegate = new DelegateToSetNodeBoundary(GetNodeBoundary);
                node.LabelText = "";
            }

            //bind the graph to the viewer 

            graph.CreateGeometryGraph();
            gViewer.Graph = graph;

            gViewer.Dock = System.Windows.Forms.DockStyle.Fill;

            this.viewerPanel.Controls.Add(gViewer);
            this.propertyGrid1.SelectedObject = graph;

        }

        private void GViewer_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void GViewer_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(ListViewItem)) is ListViewItem viewItem)
            {
                var name = viewItem.Name;
            }
            else
            {
                return;
            }
            e.Effect = DragDropEffects.All;
        }

        private void MainForm_MouseDown(object sender, MsaglMouseEventArgs e)
        {
            if (e.RightButtonIsPressed && !e.Handled)
            {
                m_MouseRightButtonDownPoint = (gViewer).ScreenToSource(e);
            }
        }

        private void GViewer_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            var send = sender;
            var eee = e;
        }

        private void GViewer_EdgeAdded(object sender, EventArgs e)
        {
            //TODO: control edge adding
        }

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

            return false;//returning false would enable the default rendering
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
            //TODO: возвращает рисунок, относящийся к определенному узлу (вершине)
            return MainImageList.Images[random.Next(0, MainImageList.Images.Count)];
        }

        static internal PointF PointF(Microsoft.Msagl.Core.Geometry.Point p) { return new PointF((float)p.X, (float)p.Y); }


        private void MainForm_Load(object sender, EventArgs e)
        {
            Queue<int> asff = new Queue<int>();
        }

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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //if (WasRunning)
            //{
            //    TheTimer.Enabled = true;
            //}
            //else
            {
                TheTimer.Enabled = true;
                TheTimer.Start();
            }

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

        private void TheTimer_Tick(object sender, EventArgs e)
        {
            Simulation();
        }

        private void Simulation()
        {
            //TODO: start simulation
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

        internal virtual void RedoLayout()
        {
            gViewer.NeedToCalculateLayout = false;
            gViewer.Graph = gViewer.Graph;
            gViewer.NeedToCalculateLayout = true;
        }

        private void listModelObjects_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listModelObjects.DoDragDrop(e.Item, DragDropEffects.All);   
        }
    }

    internal class NodeTypeEntry
    {
        /// <summary>
        /// If this node type has an associated button, then this will contain a reference to the button.
        /// </summary>
        internal ToolBarButton Button;

        /// <summary>
        /// If this is not null, then a button will be created in the toolbar, which allows the user to insert a node.
        /// </summary>
        internal Image ButtonImage;

        /// <summary>
        /// The initial label for the node.
        /// </summary>
        internal string DefaultLabel;

        /// <summary>
        /// The initial fillcolor of the node.
        /// </summary>
        internal MSAGLColor FillColor;

        /// <summary>
        /// The initial fontcolor of the node.
        /// </summary>
        internal MSAGLColor FontColor;

        /// <summary>
        /// The initial fontsize of the node.
        /// </summary>
        internal int FontSize;

        /// <summary>
        /// This will contain the menu item to which this node type is associated.
        /// </summary>
        internal MenuItem MenuItem;

        /// <summary>
        /// The name for this type.
        /// </summary>
        internal string Name;

        /// <summary>
        /// The initial shape of the node.
        /// </summary>
        internal Shape Shape;

        /// <summary>
        /// A string which will be initially copied into the user data of the node.
        /// </summary>
        internal string UserData;

        /// <summary>
        /// Constructs a NodeTypeEntry with the supplied parameters.
        /// </summary>
        /// <param name="name">The name for the node type</param>
        /// <param name="shape">The initial node shape</param>
        /// <param name="fillcolor">The initial node fillcolor</param>
        /// <param name="fontcolor">The initial node fontcolor</param>
        /// <param name="fontsize">The initial node fontsize</param>
        /// <param name="userdata">A string which will be copied into the node userdata</param>
        /// <param name="deflabel">The initial label for the node</param>
        /// <param name="button">An image which will be used to create a button in the toolbar to insert a node</param>
        internal NodeTypeEntry(string name, Shape shape, MSAGLColor fillcolor, MSAGLColor fontcolor, int fontsize,
                               string userdata, string deflabel, Image button)
        {
            Name = name;
            Shape = shape;
            FillColor = fillcolor;
            FontColor = fontcolor;
            FontSize = fontsize;
            UserData = userdata;
            ButtonImage = button;
            DefaultLabel = deflabel;
        }

        /// <summary>
        /// Constructs a NodeTypeEntry with the supplied parameters.
        /// </summary>
        /// <param name="name">The name for the node type</param>
        /// <param name="shape">The initial node shape</param>
        /// <param name="fillcolor">The initial node fillcolor</param>
        /// <param name="fontcolor">The initial node fontcolor</param>
        /// <param name="fontsize">The initial node fontsize</param>
        /// <param name="userdata">A string which will be copied into the node userdata</param>
        /// <param name="deflabel">The initial label for the node</param>
        internal NodeTypeEntry(string name, Shape shape, MSAGLColor fillcolor, MSAGLColor fontcolor, int fontsize,
                               string userdata, string deflabel)
            : this(name, shape, fillcolor, fontcolor, fontsize, userdata, deflabel, null)
        {
        }
    }
}
