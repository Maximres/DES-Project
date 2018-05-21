using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Infrastructure;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;

namespace QueueSimulation.BL.Concrete.Machines
{
    public class FirstMachine<T> : MachineBase<T> where T : ProductBase
    {
        public FirstMachine()
        {
            this._exponential = new Exponential(CrashRatePerProduct);
        }

        public override double Delay { get; set; } = 2;
        public override int Capacity { get; set; } = 1000;

        public override bool IsBroken => NotBroken();

        public override double InactiveTime { get; set; } = 2;
        public override int CrashRatePerProduct { get; set; } = 1000;

        public override bool CanTakeProduct => _productsQueue.Count < Capacity;

        public override double CrashChance { get; set; } = 0.1;

        public override bool CanThrowProduct => CanThrow() && NotBroken();


        public override int Id { get; set; } = 3;
    }

}
