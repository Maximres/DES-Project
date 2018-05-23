using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.Infrastructure.Nodes
{
    public sealed class NextNodeId
    {
        private static NextNodeId _instance;
        private static int counter;
        private static readonly object locker = new object();

        private NextNodeId()
        {
            counter = 0;
        }

        public static NextNodeId GetInstance()
        {
            if (_instance == null)
            {
                lock (locker)
                {
                    _instance = new NextNodeId();
                }
                
            }
            return _instance;
        }

        public int GetNextId()
        {
            lock (locker)
            {
                return counter++;
            }
        }
    }
}
