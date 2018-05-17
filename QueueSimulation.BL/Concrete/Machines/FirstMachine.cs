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
    public class FirstMachine<T> : MachineBase<T>, IMachine<T> where T : ProductBase
    {
        public FirstMachine()
        {
            this._exponential = new Exponential(CrashRatePerProduct);
        }

        public override IDequeueable<T> PortIn { get; set; }
        public override IDequeueable<T> PortOut { get; set; } = null;
        public override double Delay { get; set; } = 3;
        public override int Capacity { get; set; } = 0;

        public override bool IsBroken => false;

        public override double InactiveTime { get; set; } = 5;
        public override int CrashRatePerProduct { get; set; } = 100;

        public override bool CanTakeProduct => false;

        public override double CrashChance { get; set; } = 0.1;

        public override bool CanThrowProduct => true;

    }

}
