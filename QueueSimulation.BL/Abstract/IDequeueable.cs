using QueueSimulation.BL.Infrastructure;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Abstract
{
    public interface IDequeueable<T> where T : ProductBase
    {
        /// <summary>
        /// Происходит, когда станок выпускает объект.
        /// </summary>
        event EventHandler<ProductEngagedEventArgs<T>> OnDequeue;
    }
}
