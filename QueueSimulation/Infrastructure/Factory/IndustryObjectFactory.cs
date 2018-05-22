using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation
{
    public abstract class IndustryObjectFactory<T> where T : ProductBase
    {
        public abstract ProductBase CreateProduct(string name);
        public abstract Machine<T> CreateMachine(string name);
        public abstract Conveyor<T> CreateConveyor();
        public abstract object CreateObject(string name);
    }
}
