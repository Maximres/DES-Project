using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Infrastructure;

namespace QueueSimulation.BL.Objects
{
    public abstract class SeedBase<T> : ContainerBase<T> where T : ProductBase
    {
        public abstract bool CanSeedObject { get; set; }

        public abstract void SeedObject(T product);

        private void SeedObject(object sender, ProductEngagedEventArgs<T> e)
        {
            SeedObject(e.Product);
        }

        public override void AddNode(IDequeueable<T> node)
        {
            PortIn = node;
            node.OnDequeue += SeedObject;
        }
    }
}
