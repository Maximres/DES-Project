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
        public Seed(int count):base(count)
        {

        }

        public override bool CanSeedObject { get; set; } = true;

        public override int Capacity { get; set; } = 1000;

        public override int Id { get; set; } = 9;

        public override void SeedObject(T product)
        {
            product = null;
        }
    }
}
