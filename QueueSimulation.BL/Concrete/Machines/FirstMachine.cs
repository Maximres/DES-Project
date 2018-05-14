using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Infrastructure;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Concrete.Machines
{
    public class FirstMachine<T> : Machine<T>, IMachine<T> where T : Product
    {
        protected FirstMachine(Conveyor<T> conveyor)
        {
            PortIn = conveyor;
        }

        public override Conveyor<T> PortIn { get; set; }
        public override Container<T> PortOut { get; set; } = null;
        public override double Delay { get; set; } = 3;
        public override long Capacity { get; set; } = 0;

        public override bool IsBroken => false;

        public override double InactiveTime { get; set; } = 5;
        public override float CrashChance { get; set; } = 0.05f;

        public event EventHandler<ProductEngagedEventArgs<T>> OnDequeue = delegate { };
        public event EventHandler<ProductEngagedEventArgs<T>> OnEnqueue = delegate { };

        public void EmulateMachine()
        {
            throw new NotImplementedException();
        }
    }
}
