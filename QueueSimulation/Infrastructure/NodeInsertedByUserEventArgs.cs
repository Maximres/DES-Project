using Microsoft.Msagl.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.Infrastructure
{
    public class NodeInsertedByUserEventArgs : EventArgs
    {
        public Node Node { get; private set; }

        public NodeInsertedByUserEventArgs(Node node)
        {
            this.Node = node;
        }
    }
}
