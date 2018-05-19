using QueueSimulation.BL.Infrastructure;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Abstract
{
    public interface IEnqueueable<T> where T : ProductBase
    {
        /// <summary>
        /// Происходит, когда станок получает объект.
        /// </summary>
        event EventHandler<ProductEngagedEventArgs<T>> OnEnqueue;

        IDequeueable<T> PortIn { get; set; }
    }
}
