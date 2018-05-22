using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Concrete.Conveyors
{
    public class MainConveyor<T> : Conveyor<T> where T : ProductBase
    {

        //public override bool CanThrowProduct => CanThrow();

        //public override bool IsEmpty => _productsQueue?.Count > 0 == false;

        //public override double Length { get; set; } = 100;
        //public override int Delay { get; set; } = 3;
        //public override double Speed { get; set; } = 3;

        //public override bool CanTakeProduct => _productsQueue.Count() < Capacity;

        //public override int Capacity { get; set; } = 1000;

        //public override int Id { get; set; } = 7;
    }
}
