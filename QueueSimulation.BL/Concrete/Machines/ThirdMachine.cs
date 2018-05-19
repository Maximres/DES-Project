using MathNet.Numerics.Distributions;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Concrete.Machines
{
    public class ThirdMachine<T> : MachineBase<T> where T : ProductBase
    {
        public ThirdMachine()
        {
            this._exponential = new Exponential(CrashRatePerProduct);
        }

        public override bool CanTakeProduct => _productsQueue.Count < Capacity;

        public override bool CanThrowProduct => CanThrow() && NotBroken();

        public override double Delay { get; set; } = 7;

        public override bool IsBroken => NotBroken();

        public override double InactiveTime { get; set; } = 4;
        public override double CrashChance { get; set; } = 0.12;
        public override int CrashRatePerProduct { get; set; } = 80;
        public override int Capacity { get; set; } = 1000;

        public override int Id { get; set; } = 5;
    }
}
