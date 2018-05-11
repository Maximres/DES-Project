using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Infrastructure
{
    public class ProductEngagedEventArgs<T> : EventArgs where T : Product
    {
        public T Product { get; private set; }

        public ProductEngagedEventArgs(T product)
        {
            this.Product = product;
        }
    }
}
