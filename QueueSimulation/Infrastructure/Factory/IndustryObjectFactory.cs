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
        public abstract MachineBase<T> CreateMachine(string name);
        public abstract ConveyorBase<T> CreateConveyor();
        public abstract object CreateObject(string name);
    }
}
