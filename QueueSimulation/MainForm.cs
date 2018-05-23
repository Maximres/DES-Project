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
using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Concrete;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

namespace QueueSimulation
{
    public partial class MainForm : Form
    {
        private const string FILENAME = "Statistics.xml";

        TimeSpan _timeSpan;
        DateTime _past;
        GViewer gViewer;
        static Random random = new Random();
        const float radiusRatio = 0.3f;
        internal event EventHandler<NodeInsertedByUserEventArgs> NodeInsertedByUser = delegate { };
        protected MSAGLPoint m_MouseRightButtonDownPoint;
        protected MSAGLPoint m_MouseLeftButtonDownPoint;
        static IndustryFactory<ProductBase> _factory;
        static IndustryObjectsSetter<ProductBase> _factoryTuner;

        public string ElapsedTime { get; private set; } = "";
        public int Count { get; private set; } = 0;
        public int CrashCount { get; private set; } = 0;

        Source<ProductBase> Source;
        Seed<ProductBase> Seed;


        List<ISimulation<ProductBase>> _objectsSequence;

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
            gViewer.MouseWheel += GViewer_MouseWheel; ;
            (gViewer as IViewer).MouseDown += MainForm_MouseDown;
            (gViewer as IViewer).MouseUp += MainForm_MouseUp;
            (gViewer as IViewer).ObjectUnderMouseCursorChanged += MainForm_ObjectUnderMouseCursorChanged;
            gViewer.DragEnter += GViewer_DragEnter;
            gViewer.DragDrop += GViewer_DragDrop;
            propertyGrid1.TextChanged += PropertyGrid1_TextChanged;
            propertyGrid1.PropertyValueChanged += PropertyGrid1_PropertyValueChanged;

            Microsoft.Msagl.Drawing.Graph graph = new Graph("graph");
            graph.Attr.BackgroundColor = MSAGLColor.LightGray;
            //can generate an event at specific time
            //var tm = new System.Threading.Timer()

            NodeId = NextNodeId.GetInstance();
            _objectsSequence = new List<ISimulation<ProductBase>>();
            _factory = new IndustryFactory<ProductBase>();
            _factoryTuner = new IndustryObjectsSetter<ProductBase>(_factory);
            #endregion

            #region Setup

            //create the graph content 
            //var obj1 = _factory.CreateConveyor();
            //var obj2 = _factory.CreateProduct(1);

            System.Diagnostics.Debug.WriteLine(gViewer.CurrentScale);

            gViewer.Size = this.viewerPanel.Size;

            InitSequence(graph);

            SetImagesToNodes(graph);

            //bind the graph to the viewer 

            graph.CreateGeometryGraph();
            gViewer.Graph = graph;

            gViewer.Dock = System.Windows.Forms.DockStyle.Fill;

            this.viewerPanel.Controls.Add(gViewer);
            this.propertyGrid1.SelectedObject = _factory.CreateProduct("Продукт1");

            ///Setup buttons
            btnStop.Enabled = false;
            #endregion

        }

        private static bool IsRunning { get; set; } = false;

        private void PropertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            PropertyGrid property = s as PropertyGrid;
            var theItem = e.ChangedItem;

            ValidationContext validation = new ValidationContext(property.SelectedObject, null, null);
            IList<ValidationResult> results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(property.SelectedObject, validation, results, true))
            {
                foreach (var item in results)
                {
                    MessageBox.Show(item.ErrorMessage);
                }
                var theItm = e.ChangedItem;
                var propName = theItm.PropertyDescriptor.Name;
                PropertyInfo propInf = property.SelectedObject.GetType().GetProperty(propName);
                var oldValue = e.OldValue;
                propInf.SetValue(property.SelectedObject, oldValue, null);
                propertyGrid1.Refresh();

            }
            else
            {
                _factoryTuner.SetNewParametresToObject(property.SelectedObject);
            }
        }

        private void PropertyGrid1_TextChanged(object sender, EventArgs e)
        {

        }


        private void GViewer_MouseWheel(object sender, MouseEventArgs e)
        {
            int delta = e.Delta;
            if (delta != 0)
                gViewer.ZoomF *= delta < 0 ? 0.9 : 1.1;
        }

        #region Methods
        private void MainForm_ObjectUnderMouseCursorChanged(object sender, ObjectUnderMouseCursorChangedEventArgs e)
        {
            //toolPanelTime.Text = e.OldObject != null ? e.OldObject.DrawingObject.ToString() : "nothing";
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
                node.LabelText = "0";
            }
        }

        private void InitSequence(Graph graph)
        {
            var orangeBox = _factory.CreateProduct(2) as OrangeBoxProduct;
            int elemts = 2;
            Source = new Source<ProductBase>(elemts, orangeBox);
            var conveyor1 = _factory.CreateConveyor();
            var first = _factory.CreateMachine("Станок1");
            var conveyor2 = _factory.CreateConveyor();
            var second = _factory.CreateMachine("Станок2");
            var conveyor3 = _factory.CreateConveyor();
            var third = _factory.CreateMachine("Станок3");
            var conveyor4 = _factory.CreateConveyor();
            Seed = new Seed<ProductBase>(elemts);
            Seed.OnEmpty += Source_OnEmpty;

            Node sourceNode = new Node("Source")
            {
                UserData = Source,
            };

            Node nodeConveyor1 = new Node("c1")
            {
                UserData = conveyor1,
            };

            Node nodeFirst = new Node("m1")
            {
                UserData = first,
            };

            Node nodeConveyor2 = new Node("c2")
            {
                UserData = conveyor2,
            };

            Node nodeSecond = new Node("m2")
            {
                UserData = second,
            };

            Node nodeConveyor3 = new Node("c3")
            {
                UserData = conveyor3,
            };

            Node nodeThird = new Node("m3")
            {
                UserData = third,
            };

            Node nodeConveyor4 = new Node("c4")
            {
                UserData = conveyor4,
            };
            Node seedNode = new Node("Seed")
            {
                UserData = Seed,
            };

            Edge sourceToc1 = new Edge(sourceNode, nodeConveyor1, ConnectionToGraph.Connected);
            Edge c1Tom1 = new Edge(nodeConveyor1, nodeFirst, ConnectionToGraph.Connected);
            Edge m1Toc2 = new Edge(nodeFirst, nodeConveyor2, ConnectionToGraph.Connected);
            Edge c2Tom3 = new Edge(nodeConveyor2, nodeSecond, ConnectionToGraph.Connected);
            Edge m3Toc4 = new Edge(nodeSecond, nodeConveyor3, ConnectionToGraph.Connected);
            Edge c4Tom4 = new Edge(nodeConveyor3, nodeThird, ConnectionToGraph.Connected);
            Edge m4Toc4 = new Edge(nodeThird, nodeConveyor4, ConnectionToGraph.Connected);
            Edge c4Toseed = new Edge(nodeConveyor4, seedNode, ConnectionToGraph.Connected);


            sourceToc1.LabelText = "0";
            c1Tom1.LabelText = "0";
            m1Toc2.LabelText = "0";
            c2Tom3.LabelText = "0";
            m3Toc4.LabelText = "0";
            c4Tom4.LabelText = "0";
            m4Toc4.LabelText = "0";
            c4Toseed.LabelText = "0";
            //labelToChange = sourceToSeed.Label;
            var setts = graph.LayoutAlgorithmSettings;
            graph.AddNode(sourceNode);
            graph.AddNode(nodeConveyor1);
            graph.AddNode(nodeFirst);
            graph.AddNode(nodeConveyor2);
            graph.AddNode(nodeSecond);
            graph.AddNode(nodeConveyor3);
            graph.AddNode(nodeThird);
            graph.AddNode(nodeConveyor4);
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
                _timeSpan = DateTime.Now - _past;
                toolPanelTime.Text = ElapsedTime = _timeSpan.Minutes.ToString() + ":" + _timeSpan.Seconds.ToString();
                ///Запускает эмулицию продукции на производстве
                Simulation();
            }
            catch (Exception ex)
            {
                TheTimer.Stop();
                MessageBox.Show(ex.Message);
                //return;
            }
        }

        void GViewerOnMouseMove(Edge edge, string value)
        {
            if (edge.Label == null) return;
            edge.Label.Text = value;

            var rect = edge.Label.BoundingBox;
            var font = new Font(edge.Label.FontName, (int)edge.Label.FontSize);
            StringMeasure.MeasureWithFont(edge.Label.Text, font, out double width, out double height);

            if (width <= 0)
                StringMeasure.MeasureWithFont("a", font, out width, out height);

            edge.Label.Width = width;
            edge.Label.Height = height;
            rect.Add(edge.Label.BoundingBox);
            gViewer.Invalidate(gViewer.MapSourceRectangleToScreenRectangle(rect));
        }

        private void Simulation()
        {
            nodesWithCountList.Items.Clear();
            int number = 0;

            //TODO: start simulation
            foreach (var item in _objectsSequence)
            {
                item.Simulate();
                if (IsRunning == false)
                {
                    return;
                }
                if (item is Machine<ProductBase>)
                {
                    var machine = (item as Machine<ProductBase>);
                    if (machine.IsBroken)
                    {
                        CrashCount++;
                    }
                    var cond = machine.IsBroken == true ? "(broken)" : item.Name;
                    nodesWithCountList.Items.Add(++number + "\t" + cond + "\t" + item.Count);
                }
                else
                {
                    nodesWithCountList.Items.Add(++number + "\t" + item.Name + "\t" + item.Count);
                }
                foreach (var element in gViewer.Entities.Where(s => s.DrawingObject is Edge))
                {
                    if (IsRunning == false)
                    {
                        return;
                    }

                    var edge = element.DrawingObject as Edge;
                    var data = edge?.SourceNode?.UserData as ISimulation<ProductBase>;
                    var count = data?.Count.ToString() ?? "0";
                    //edge value
                    GViewerOnMouseMove(edge, count);
                }
            }
        }



        internal virtual void RedoLayout()
        {
            gViewer.NeedToCalculateLayout = false;
            gViewer.Graph = gViewer.Graph;
            gViewer.NeedToCalculateLayout = true;
            gViewer.Graph = gViewer.Graph;
            //gViewer.Invalidate();
            //gViewer.Refresh();
        }

        internal virtual void RedoLayout(Node node)
        {

        }
        #endregion

        #region Events


        private void Source_OnEmpty(object sender, EventArgs e)
        {
            TheTimer.Stop();
            TheTimer.Enabled = false;
            IsRunning = false;
            MessageBox.Show("Источник пуст!");
            Statisctics statisctics = new Statisctics()
            {
                Count = this.Count,
                CrashCount = this.CrashCount,
                ElapsedTime = this.ElapsedTime
            };
            if (CreateStatisctics(statisctics))
            {
                toolStripButtonStatisctics.Enabled = true;
                toolStripButtonStatisctics.Checked = true;
                btnStop.PerformClick();
                statusPanel.Text = "complete";
            }
        }

        private static bool CreateStatisctics(Statisctics statisctics)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Statisctics));
            if (File.Exists(FILENAME))
            {
                try
                {
                    File.Delete(FILENAME);
                    using (FileStream fs = new FileStream(FILENAME, FileMode.CreateNew))
                    {
                        xmlSerializer.Serialize(fs, statisctics);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
            else
            {
                using (FileStream fs = new FileStream(FILENAME, FileMode.CreateNew))
                {
                    xmlSerializer.Serialize(fs, statisctics);
                }
                return true;
            }

        }

        private void GViewer_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
            if (e.Data.GetData(typeof(ListViewItem)) is ListViewItem viewItem)
            {
                var mPoint = (gViewer).ScreenToSource(e.X, e.Y);
                //var mPoint2 = m_MouseLeftButtonDownPoint;
                //MSAGLPoint point = mPoint;
                var obj = _factory.CreateObject(viewItem.Text);
                switch (obj)
                {
                    case Conveyor<ProductBase> conv:
                        InsertNode(new MSAGLPoint(MousePosition.X, MousePosition.Y), conv);
                        break;
                    case Machine<ProductBase> machine:
                        InsertNode(new MSAGLPoint(MousePosition.X, MousePosition.Y), machine);
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
            if (gViewer.ObjectUnderMouseCursor is DrawingObject drawing)
            {
                this.propertyGrid1.SelectedObject = Source;
            }

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
                if (edge.SourceNode.Equals(edge.TargetNode))
                {
                    DeleteEdge(edge);
                }
                if (edge.SourceNode.InEdges is IEnumerable<Edge> inputEdges && inputEdges.Count() > 1)
                {
                    DeleteEdge(edge);
                }
                if (edge.SourceNode.OutEdges is IEnumerable<Edge> outputEdges && outputEdges.Count() > 1)
                {
                    DeleteEdge(edge);
                }
                var inEdge = edge.SourceNode.InEdges.FirstOrDefault();
                var targer = edge.TargetNode;
                if (inEdge is Edge edge1)
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
        }

        //TODO: реализовать отображение свойств
        private void ListView_SelectedIndexChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            //var selected = e.IsSelected;
            //var ind = e.ItemIndex;
            //var gg = e.Item;
            ListViewItem item = null;
            if (!(sender is ListView listView))
            {
                return;
            }
            if (listView.SelectedItems.Count > 0)
            {
                item = listView.SelectedItems[0];
            }

            if (item != null)
            {
                var obj = _factory.CreateObject(item.Text);
                propertyGrid1.SelectedObject = obj;
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

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateInput(out int elements, out int interval);
                Count = elements;
                //preparing
                var source = gViewer.Entities.Select(s => s.DrawingObject).FirstOrDefault(s => s.UserData is Source<ProductBase>) as Node;
                var seed = gViewer.Entities.Select(s => s.DrawingObject).FirstOrDefault(s => s.UserData is Seed<ProductBase>) as Node;
                var productType = _factory.CreateProduct(Convert.ToInt32((source.UserData as Source<ProductBase>).Id));

                //Updatin for new run
                ResetSS(elements, interval, source, seed, productType);
                UpdateNodes<ProductBase>(source, seed);
                _objectsSequence.Clear();
                GetSequence(source, ref _objectsSequence);
                _objectsSequence.Reverse();

                if (_objectsSequence is null || _objectsSequence.Count < 2)
                {
                    throw new ArgumentNullException("Последовательность не может быть пуста");
                }
                if (_objectsSequence.FirstOrDefault() is Source<ProductBase> == false)
                {
                    throw new ArgumentNullException("Генератор продуктов должен начинать последовательность");
                }
                if (_objectsSequence.LastOrDefault() is Seed<ProductBase> == false)
                {
                    throw new ArgumentNullException("Утилизатор продуктов должен заканчивать последовательность");
                }

                //adding connections
                for (int current = 0, next = 1; current < _objectsSequence.Count; current++, next++)
                {
                    if (next < _objectsSequence.Count)
                    {
                        _objectsSequence[next].JoinPrevious(_objectsSequence[current]);
                    }
                }

                _past = DateTime.Now;
                IsRunning = true;
                propertyGrid1.Enabled = false;
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                toolStripButtonStatisctics.Enabled = false;
                statusPanel.Text = "on";
                //run
                TheTimer.Start();

            }
            catch (ArgumentNullException nullExp)
            {
                MessageBox.Show(nullExp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (ArgumentException argExp)
            {
                MessageBox.Show(argExp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла неопределенная ошибка", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void ValidateInput(out int elements, out int interval)
        {
            if (Int32.TryParse(toolStripItemsCount.Text, out elements) == false)
            {
                throw new ArgumentException("Число имеет неверный формат:\n" + toolStripItemsCount.Text);
            }
            if (Int32.TryParse(toolStripTextBox1.Text, out interval) == false)
            {
                throw new ArgumentException("Число имеет неверный формат:\n" + toolStripItemsCount.Text);
            }
        }

        private static void ResetSS(int elements, int interval, Node source, Node seed, ProductBase productType)
        {
            (source.UserData as Source<ProductBase>).Reset(elements, productType);
            (source.UserData as Source<ProductBase>).ArrivalRate = interval;
            (seed.UserData as Seed<ProductBase>).Reset(elements);
        }

        private void UpdateNodes<T>(Node source, Node seed) where T : ProductBase
        {
            foreach (var item in gViewer.Entities.Where(s => s.DrawingObject is Node))
            {
                if (item.DrawingObject is Node sourceNode && sourceNode.Id == "Source")
                {
                    item.DrawingObject.UserData = source.UserData;
                    continue;
                }
                if (item.DrawingObject is Node seedNode && seedNode.Id == "Seed")
                {
                    item.DrawingObject.UserData = seed.UserData;
                    continue;
                }
                if (item.DrawingObject is Node containerNode && containerNode.UserData is ContainerBase<T>)
                {
                    var id = (containerNode.UserData as ContainerBase<T>).Id;
                    item.DrawingObject.UserData = _factory.CreateObject(id);
                    continue;
                }
                if (item.DrawingObject is Node productNode && productNode.UserData is T)
                {
                    var id = (productNode.UserData as T).Id;
                    item.DrawingObject.UserData = _factory.CreateProduct(id);
                    continue;
                }

            }
        }

        private void ToolStripButton4_Click(object sender, EventArgs e)
        {
            TheTimer.Stop();
            IsRunning = false;
            btnStart.Enabled = true;
        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            //TheTimer.Enabled = false;
            TheTimer.Stop();
            IsRunning = false;
            propertyGrid1.Enabled = true;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            statusPanel.Text = "off";
        }



        private void ContextItemDeleteSelectedElements_Click(object sender, EventArgs e)
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

        private void ContextItemDeleteElement_Click(object sender, EventArgs e)
        {
            if (!(gViewer.SelectedObject is Node selectedObj))
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
        private void ContextItemConnectElements_Click(object sender, EventArgs e)
        {
            var node = InsertNode(m_MouseLeftButtonDownPoint, "15");
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
            gViewer.Invalidate(dNode);
            //NodeInsertedByUser(this, new NodeInsertedByUserEventArgs(node));
            return node;
        }

        internal virtual Node InsertNode(MSAGLPoint center, object simulationObj)
        {
            Node node = null;
            switch (simulationObj)
            {
                case Conveyor<ProductBase> con:
                    node = new Node(con.Name)
                    {
                        UserData = con
                    };
                    break;
                case Machine<ProductBase> frth:
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
            node.LabelText = "0";

            IViewerNode dNode = gViewer.CreateIViewerNode(node, center, null);
            gViewer.AddNode(dNode, true);

            Invalidate();

            return node;
        }

        private void ListModelObjects_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listModelObjects.DoDragDrop(e.Item, DragDropEffects.All);
        }

        #endregion

        private void ToolStripItemsCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            PreventInvalidInput(sender, e);
        }

        private static void PreventInvalidInput(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.Delete || e.KeyChar == (int)Keys.Back)
            {
                return;
            }
            if (false == char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
        }

        private void ToolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            PreventInvalidInput(sender, e);
        }

        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            PreventUnckeckedInput(sender, e, 120);
        }

        private static void PreventUnckeckedInput(object sender, KeyEventArgs e, int val)
        {
            if (sender is ToolStripTextBox box && string.IsNullOrEmpty(box.Text) == false)
            {
                var textBox = Int32.Parse(box.Text);
                if (textBox > val)
                {
                    box.Text = val.ToString();
                }
                if (textBox == 0)
                {
                    box.Text = "1";
                }
                e.Handled = true;
                return;

            }
        }

        private void toolStripItemsCount_KeyUp(object sender, KeyEventArgs e)
        {
            PreventUnckeckedInput(sender, e, 1000);
        }

        private void toolStripButtonStatisctics_Click(object sender, EventArgs e)
        {
            if (File.Exists(FILENAME))
            {
                Process.Start(FILENAME);
            }
            else
            {
                MessageBox.Show("Файл со статистикой не найден", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

}
