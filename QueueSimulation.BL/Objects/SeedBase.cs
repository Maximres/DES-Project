using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Infrastructure;

namespace QueueSimulation.BL.Objects
{
    public abstract class SeedBase<T> : ContainerBase<T>, ISimulation<T> where T : ProductBase
    {
        int _count;

        public SeedBase(int count)
        {
            _count = count;
        }

        public abstract bool CanSeedObject { get; set; }
        public IDequeueable<T> PortIn { get; set; }
        public IDequeueable<T> PorOut { get; set; }

        public int Count => 0;

        public string Name { get; set; } = "Seed";

        public event EventHandler OnEmpty = delegate { };
        public event EventHandler<ProductEngagedEventArgs<T>> OnEnqueue = delegate { };
        public event EventHandler<ProductEngagedEventArgs<T>> OnDequeue = delegate { };

        public abstract void SeedObject(T product);

        private void SeedObject(object sender, ProductEngagedEventArgs<T> e)
        {
            SeedObject(e.Product);
        }

        //public override void JoinWithPrevious(IDequeueable<T> node)
        //{
        //    if (PortIn != null)
        //    {
        //        RemoveNode(node);
        //    }
        //    node.PorOut = this;
        //    PortIn = node;
        //    node.OnDequeue += SeedObject;
        //}

        //public override void RemoveNode(IDequeueable<T> node)
        //{
        //    base.RemoveNode(node);
        //}

        public void Simulate()
        {
            //Seed automatically dispose every product that gets from objects
            --_count;
            if (_count <= 0)
            {
                OnEmpty(this, EventArgs.Empty);
            }
        }

        public void Dequeue(object sender, ProductEngagedEventArgs<T> e)
        {
            throw new NotSupportedException();
        }

        public void Enqueue(object sender, ProductEngagedEventArgs<T> e)
        {
            throw new NotSupportedException();
        }

        public void JoinWithPrevious(IDequeueable<T> node)
        {
            node.PorOut = this;
            PortIn = node;
            node.OnDequeue += SeedObject;
        }
    }
}
