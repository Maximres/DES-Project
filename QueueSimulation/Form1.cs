using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;
using QueueSimulation.BL.Objects;
using System.Diagnostics;

namespace QueueSimulation
{
    public partial class Form1 : Form
    {
        Stopwatch stopwatch;
        GViewer gViewer;

        public Form1()
        {
            InitializeComponent();

            #region Init
            //create a viewer object 
            gViewer = new GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Graph("graph");
            #endregion

            #region Setup

            #endregion

            //create the graph content 

            gViewer.ToolBarIsVisible = false;
            gViewer.BackColor = System.Drawing.Color.Red;
            System.Diagnostics.Debug.WriteLine(gViewer.CurrentScale);

            gViewer.Size = this.viewerPanel.Size;

            graph.AddEdge("source", "queue").Attr.AddStyle(Microsoft.Msagl.Drawing.Style.Solid);

            var c = graph.FindNode("source");
            c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
            c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Brown;

            graph.AddEdge("queue", "conveyor");
            graph.AddEdge("conveyor", "delay");
            graph.AddEdge("delay", "machine");

            graph.AddEdge("machine", "conveyor3");
            graph.AddEdge("conveyor2", "delay2");
            graph.AddEdge("conveyor3", "delay3");
            graph.AddEdge("delay2", "sink");
            graph.AddEdge("delay3", "sink");

            //bind the graph to the viewer 

            graph.CreateGeometryGraph();
            gViewer.Graph = graph;

            gViewer.Dock = System.Windows.Forms.DockStyle.Fill;

            this.viewerPanel.Controls.Add(gViewer);
            this.propertyGrid1.SelectedObject = graph;

        }



        private void Form1_Load(object sender, EventArgs e)
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
        }
    }

}
