using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.Infrastructure
{
    [Serializable]
    public class Statisctics
    {
        public string ElapsedTime { get; set; }
        public int Count { get; set; }
        public int CrashCount { get; set; }
    }
}
