using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Concrete.Conveyors
{
    public class MainConveyor<T> : ConveyorBase<T> where T : ProductBase
    {

        public override bool CanThrowProduct => CanThrow();

        public override bool IsEmpty => !_productsQueue.Any();

        public override IDequeueable<T> PortOut { get; set; }
        public override double Length { get; set; } = 100;
        public override int Delay { get; set; } = 3;
        public override double Speed { get; set; } = 3;

        public override bool CanTakeProduct => _productsQueue.Count < Capacity;

        public override IDequeueable<T> PortIn { get; set; }
        public override int Capacity { get; set; } = 1000;
    }
}
