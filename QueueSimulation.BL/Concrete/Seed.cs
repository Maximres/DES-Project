using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Concrete
{
    public class Seed<T> : SeedBase<T> where T : ProductBase
    {
        public override bool CanSeedObject { get; set; } = true;


        public override IDequeueable<T> PortIn { get; set; }
        public override int Capacity { get; set; } = 0;
        public override IDequeueable<T> PortOut { get; set ; }

        public override void SeedObject(T product)
        {
            product = null;
        }
    }
}
