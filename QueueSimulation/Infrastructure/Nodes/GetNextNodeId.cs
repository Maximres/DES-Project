using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.Infrastructure.Nodes
{
    internal sealed class GetNextNodeId
    {
        private static GetNextNodeId _instance;
        private static int counter;
        private static object locker = new object();

        private GetNextNodeId()
        {
            counter = 0;
        }

        public static GetNextNodeId GetInstance()
        {
            if (_instance == null)
            {
                lock (locker)
                {
                    _instance = new GetNextNodeId();
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
